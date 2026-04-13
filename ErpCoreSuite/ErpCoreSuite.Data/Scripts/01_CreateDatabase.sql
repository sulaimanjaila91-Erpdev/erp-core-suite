-- ============================================================
-- ErpCoreSuite Database Setup Script
-- Author: K. Syed Sulaiman
-- Stack:  MS SQL Server 2014+
-- Run this first before running 02_SeedData.sql
-- ============================================================

USE master;
GO

IF NOT EXISTS (SELECT name FROM sys.databases WHERE name = 'ErpCoreDB')
BEGIN
    CREATE DATABASE ErpCoreDB;
END
GO

USE ErpCoreDB;
GO

-- ============================================================
-- AUTH
-- ============================================================
CREATE TABLE Users (
    UserId      INT IDENTITY(1,1) PRIMARY KEY,
    Username    NVARCHAR(50) NOT NULL UNIQUE,
    PasswordHash NVARCHAR(256) NOT NULL,
    FullName    NVARCHAR(100),
    Role        NVARCHAR(30) NOT NULL,  -- Admin / HR / Accountant
    IsActive    BIT DEFAULT 1,
    CreatedDate DATETIME DEFAULT GETDATE()
);
GO

-- ============================================================
-- INVENTORY
-- ============================================================
CREATE TABLE Products (
    ProductId     INT IDENTITY(1,1) PRIMARY KEY,
    ProductCode   NVARCHAR(20) NOT NULL UNIQUE,
    ProductName   NVARCHAR(150) NOT NULL,
    Category      NVARCHAR(80),
    Unit          NVARCHAR(20),
    CostPrice     DECIMAL(18,3) DEFAULT 0,
    SalePrice     DECIMAL(18,3) DEFAULT 0,
    StockQty      INT DEFAULT 0,
    ReorderLevel  INT DEFAULT 0,
    IsActive      BIT DEFAULT 1,
    CreatedDate   DATETIME DEFAULT GETDATE()
);
GO

CREATE TABLE StockTransactions (
    TransactionId   INT IDENTITY(1,1) PRIMARY KEY,
    ProductId       INT NOT NULL REFERENCES Products(ProductId),
    TransactionType NVARCHAR(10) NOT NULL,   -- IN / OUT
    Quantity        INT NOT NULL,
    UnitPrice       DECIMAL(18,3),
    TotalAmount     DECIMAL(18,3),
    Reference       NVARCHAR(100),
    Remarks         NVARCHAR(300),
    TransactionDate DATETIME DEFAULT GETDATE(),
    CreatedBy       NVARCHAR(50)
);
GO

-- ============================================================
-- PAYROLL
-- ============================================================
CREATE TABLE Employees (
    EmployeeId         INT IDENTITY(1,1) PRIMARY KEY,
    EmployeeCode       NVARCHAR(20) NOT NULL UNIQUE,
    FullName           NVARCHAR(100) NOT NULL,
    Department         NVARCHAR(80),
    Designation        NVARCHAR(80),
    NationalId         NVARCHAR(30),
    JoiningDate        DATE,
    BasicSalary        DECIMAL(18,3) DEFAULT 0,
    HousingAllowance   DECIMAL(18,3) DEFAULT 0,
    TransportAllowance DECIMAL(18,3) DEFAULT 0,
    IsActive           BIT DEFAULT 1
);
GO

CREATE TABLE PayrollRecords (
    PayrollId          INT IDENTITY(1,1) PRIMARY KEY,
    EmployeeId         INT NOT NULL REFERENCES Employees(EmployeeId),
    PayMonth           INT NOT NULL,
    PayYear            INT NOT NULL,
    BasicSalary        DECIMAL(18,3),
    HousingAllowance   DECIMAL(18,3),
    TransportAllowance DECIMAL(18,3),
    OtherAllowances    DECIMAL(18,3) DEFAULT 0,
    GrossSalary        DECIMAL(18,3),
    Deductions         DECIMAL(18,3) DEFAULT 0,
    NetSalary          DECIMAL(18,3),
    Status             NVARCHAR(20) DEFAULT 'Draft',  -- Draft/Approved/Paid
    ProcessedDate      DATETIME DEFAULT GETDATE(),
    ProcessedBy        NVARCHAR(50),
    ApprovedBy         NVARCHAR(50),
    ApprovedDate       DATETIME
);
GO

-- ============================================================
-- ACCOUNTING
-- ============================================================
CREATE TABLE ChartOfAccounts (
    AccountId         INT IDENTITY(1,1) PRIMARY KEY,
    AccountCode       NVARCHAR(20) NOT NULL UNIQUE,
    AccountName       NVARCHAR(150) NOT NULL,
    AccountType       NVARCHAR(20) NOT NULL,  -- Asset/Liability/Equity/Revenue/Expense
    ParentAccountCode NVARCHAR(20),
    IsActive          BIT DEFAULT 1
);
GO

CREATE TABLE JournalEntries (
    JournalId   INT IDENTITY(1,1) PRIMARY KEY,
    VoucherNo   NVARCHAR(30) NOT NULL UNIQUE,
    EntryDate   DATE NOT NULL,
    Description NVARCHAR(300),
    Reference   NVARCHAR(100),
    Status      NVARCHAR(20) DEFAULT 'Draft',  -- Draft / Posted
    CreatedBy   NVARCHAR(50),
    CreatedDate DATETIME DEFAULT GETDATE(),
    PostedBy    NVARCHAR(50),
    PostedDate  DATETIME
);
GO

CREATE TABLE JournalLines (
    LineId       INT IDENTITY(1,1) PRIMARY KEY,
    JournalId    INT NOT NULL REFERENCES JournalEntries(JournalId),
    AccountCode  NVARCHAR(20) NOT NULL REFERENCES ChartOfAccounts(AccountCode),
    DebitAmount  DECIMAL(18,3) DEFAULT 0,
    CreditAmount DECIMAL(18,3) DEFAULT 0,
    Narration    NVARCHAR(200)
);
GO

-- ============================================================
-- STORED PROCEDURES
-- ============================================================

-- Get all products with low stock flag
CREATE OR ALTER PROCEDURE sp_GetAllProducts
AS
BEGIN
    SET NOCOUNT ON;
    SELECT
        p.*,
        CASE WHEN p.StockQty <= p.ReorderLevel THEN 1 ELSE 0 END AS IsLowStock
    FROM Products p
    WHERE p.IsActive = 1
    ORDER BY p.ProductName;
END
GO

-- Post stock transaction and update stock qty
CREATE OR ALTER PROCEDURE sp_PostStockTransaction
    @ProductId       INT,
    @TransactionType NVARCHAR(10),
    @Quantity        INT,
    @UnitPrice       DECIMAL(18,3),
    @Reference       NVARCHAR(100),
    @Remarks         NVARCHAR(300),
    @CreatedBy       NVARCHAR(50)
AS
BEGIN
    SET NOCOUNT ON;
    BEGIN TRANSACTION;

    INSERT INTO StockTransactions
        (ProductId, TransactionType, Quantity, UnitPrice, TotalAmount, Reference, Remarks, CreatedBy)
    VALUES
        (@ProductId, @TransactionType, @Quantity, @UnitPrice, @Quantity * @UnitPrice, @Reference, @Remarks, @CreatedBy);

    IF @TransactionType = 'IN'
        UPDATE Products SET StockQty = StockQty + @Quantity WHERE ProductId = @ProductId;
    ELSE IF @TransactionType = 'OUT'
        UPDATE Products SET StockQty = StockQty - @Quantity WHERE ProductId = @ProductId;

    COMMIT;
    SELECT SCOPE_IDENTITY() AS TransactionId;
END
GO

-- Generate payroll for a month
CREATE OR ALTER PROCEDURE sp_GeneratePayroll
    @PayMonth    INT,
    @PayYear     INT,
    @ProcessedBy NVARCHAR(50)
AS
BEGIN
    SET NOCOUNT ON;

    -- Remove any existing draft for this month
    DELETE FROM PayrollRecords
    WHERE PayMonth = @PayMonth AND PayYear = @PayYear AND Status = 'Draft';

    -- Insert fresh payroll records from current employee salary data
    INSERT INTO PayrollRecords
        (EmployeeId, PayMonth, PayYear, BasicSalary, HousingAllowance,
         TransportAllowance, OtherAllowances, GrossSalary, Deductions, NetSalary,
         Status, ProcessedBy)
    SELECT
        EmployeeId,
        @PayMonth,
        @PayYear,
        BasicSalary,
        HousingAllowance,
        TransportAllowance,
        0 AS OtherAllowances,
        (BasicSalary + HousingAllowance + TransportAllowance) AS GrossSalary,
        0 AS Deductions,
        (BasicSalary + HousingAllowance + TransportAllowance) AS NetSalary,
        'Draft',
        @ProcessedBy
    FROM Employees
    WHERE IsActive = 1;

    SELECT COUNT(*) AS RecordsGenerated FROM PayrollRecords
    WHERE PayMonth = @PayMonth AND PayYear = @PayYear AND Status = 'Draft';
END
GO

-- Get Ledger for an account
CREATE OR ALTER PROCEDURE sp_GetLedger
    @AccountCode NVARCHAR(20),
    @FromDate    DATE,
    @ToDate      DATE
AS
BEGIN
    SET NOCOUNT ON;

    SELECT
        coa.AccountCode,
        coa.AccountName,
        je.EntryDate,
        je.VoucherNo,
        je.Description,
        jl.DebitAmount  AS Debit,
        jl.CreditAmount AS Credit,
        SUM(jl.DebitAmount - jl.CreditAmount)
            OVER (ORDER BY je.EntryDate, je.JournalId) AS Balance
    FROM JournalLines jl
    INNER JOIN JournalEntries je ON jl.JournalId = je.JournalId
    INNER JOIN ChartOfAccounts coa ON jl.AccountCode = coa.AccountCode
    WHERE jl.AccountCode = @AccountCode
      AND je.EntryDate BETWEEN @FromDate AND @ToDate
      AND je.Status = 'Posted'
    ORDER BY je.EntryDate, je.JournalId;
END
GO

-- Trial Balance view
CREATE OR ALTER VIEW vw_TrialBalance AS
    SELECT
        coa.AccountCode,
        coa.AccountName,
        coa.AccountType,
        SUM(jl.DebitAmount)  AS TotalDebit,
        SUM(jl.CreditAmount) AS TotalCredit,
        SUM(jl.DebitAmount - jl.CreditAmount) AS NetBalance
    FROM ChartOfAccounts coa
    LEFT JOIN JournalLines jl ON coa.AccountCode = jl.AccountCode
    LEFT JOIN JournalEntries je ON jl.JournalId = je.JournalId AND je.Status = 'Posted'
    WHERE coa.IsActive = 1
    GROUP BY coa.AccountCode, coa.AccountName, coa.AccountType;
GO

PRINT 'ErpCoreDB setup completed successfully.';
GO
