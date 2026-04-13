-- ============================================================
-- ErpCoreSuite Seed Data
-- Run after 01_CreateDatabase.sql
-- ============================================================

USE ErpCoreDB;
GO

-- Users (passwords are hashed; default = Admin@123, Hr@123, Acc@123)
INSERT INTO Users (Username, PasswordHash, FullName, Role) VALUES
('admin',      'AQAAAAEAACcQAAAAEB+...hash...', 'System Administrator', 'Admin'),
('hruser',     'AQAAAAEAACcQAAAAEB+...hash...', 'HR Manager',           'HR'),
('accountant', 'AQAAAAEAACcQAAAAEB+...hash...', 'Senior Accountant',    'Accountant');

-- Chart of Accounts
INSERT INTO ChartOfAccounts (AccountCode, AccountName, AccountType) VALUES
('1000', 'Cash and Bank',           'Asset'),
('1001', 'Cash in Hand',            'Asset'),
('1002', 'Bank Account',            'Asset'),
('1100', 'Accounts Receivable',     'Asset'),
('1200', 'Inventory',               'Asset'),
('2000', 'Accounts Payable',        'Liability'),
('2100', 'Accrued Salaries',        'Liability'),
('3000', 'Owner Equity',            'Equity'),
('4000', 'Sales Revenue',           'Revenue'),
('4100', 'Other Income',            'Revenue'),
('5000', 'Cost of Goods Sold',      'Expense'),
('5100', 'Salary Expense',          'Expense'),
('5200', 'Rent Expense',            'Expense'),
('5300', 'Utilities Expense',       'Expense'),
('5400', 'General & Admin Expense', 'Expense');

-- Products
INSERT INTO Products (ProductCode, ProductName, Category, Unit, CostPrice, SalePrice, StockQty, ReorderLevel) VALUES
('PRD-001', 'Office Chair',          'Furniture',    'Pcs',  45.000,  75.000,  50,  10),
('PRD-002', 'Writing Desk',          'Furniture',    'Pcs',  90.000, 150.000,  30,   5),
('PRD-003', 'A4 Paper Ream',         'Stationery',   'Ream',  2.500,   4.000, 500, 100),
('PRD-004', 'Ballpoint Pen Box',     'Stationery',   'Box',   1.200,   2.500, 300,  50),
('PRD-005', 'Laptop Stand',          'Electronics',  'Pcs',  12.000,  22.000,  40,   8),
('PRD-006', 'USB Hub 4-Port',        'Electronics',  'Pcs',   4.500,   9.000, 100,  20),
('PRD-007', 'Whiteboard Marker Set', 'Stationery',   'Set',   2.000,   4.500, 150,  30),
('PRD-008', 'Filing Cabinet',        'Furniture',    'Pcs', 120.000, 200.000,  15,   3);

-- Employees
INSERT INTO Employees (EmployeeCode, FullName, Department, Designation, NationalId, JoiningDate, BasicSalary, HousingAllowance, TransportAllowance) VALUES
('EMP-001', 'Ahmed Al-Rashidi',  'IT',          'Senior Developer',    '280123456', '2020-01-15', 800.000, 200.000, 80.000),
('EMP-002', 'Fatima Al-Kulaib',  'HR',          'HR Manager',          '281234567', '2019-03-01', 700.000, 180.000, 70.000),
('EMP-003', 'Mohammed Al-Azmi',  'Accounting',  'Senior Accountant',   '282345678', '2018-07-10', 750.000, 190.000, 75.000),
('EMP-004', 'Sara Al-Mutairi',   'Operations',  'Operations Analyst',  '283456789', '2021-06-01', 600.000, 150.000, 60.000),
('EMP-005', 'Khalid Al-Shammari','Management',  'Project Manager',     '284567890', '2017-09-20', 950.000, 250.000, 100.000);

PRINT 'Seed data inserted successfully.';
GO
