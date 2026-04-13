// ErpCoreSuite.Core/Entities/PayrollEntities.cs
namespace ErpCoreSuite.Core.Entities
{
    public class Employee
    {
        public int EmployeeId { get; set; }
        public string EmployeeCode { get; set; }
        public string FullName { get; set; }
        public string Department { get; set; }
        public string Designation { get; set; }
        public string NationalId { get; set; }
        public DateTime JoiningDate { get; set; }
        public decimal BasicSalary { get; set; }
        public decimal HousingAllowance { get; set; }
        public decimal TransportAllowance { get; set; }
        public bool IsActive { get; set; }
    }

    public class PayrollRecord
    {
        public int PayrollId { get; set; }
        public int EmployeeId { get; set; }
        public string EmployeeName { get; set; }
        public string Department { get; set; }
        public int PayMonth { get; set; }
        public int PayYear { get; set; }
        public decimal BasicSalary { get; set; }
        public decimal HousingAllowance { get; set; }
        public decimal TransportAllowance { get; set; }
        public decimal OtherAllowances { get; set; }
        public decimal GrossSalary { get; set; }
        public decimal Deductions { get; set; }
        public decimal NetSalary { get; set; }
        public string Status { get; set; }  // Draft / Approved / Paid
        public DateTime ProcessedDate { get; set; }
        public string ProcessedBy { get; set; }
    }
}

// ErpCoreSuite.Core/Entities/AccountingEntities.cs
namespace ErpCoreSuite.Core.Entities
{
    public class ChartOfAccount
    {
        public int AccountId { get; set; }
        public string AccountCode { get; set; }
        public string AccountName { get; set; }
        public string AccountType { get; set; }  // Asset / Liability / Equity / Revenue / Expense
        public string ParentAccountCode { get; set; }
        public bool IsActive { get; set; }
    }

    public class JournalEntry
    {
        public int JournalId { get; set; }
        public string VoucherNo { get; set; }
        public DateTime EntryDate { get; set; }
        public string Description { get; set; }
        public string Reference { get; set; }
        public List<JournalLine> Lines { get; set; }
        public string CreatedBy { get; set; }
        public string Status { get; set; }  // Draft / Posted
    }

    public class JournalLine
    {
        public int LineId { get; set; }
        public int JournalId { get; set; }
        public string AccountCode { get; set; }
        public string AccountName { get; set; }
        public decimal DebitAmount { get; set; }
        public decimal CreditAmount { get; set; }
        public string Narration { get; set; }
    }

    public class LedgerEntry
    {
        public string AccountCode { get; set; }
        public string AccountName { get; set; }
        public DateTime EntryDate { get; set; }
        public string VoucherNo { get; set; }
        public string Description { get; set; }
        public decimal Debit { get; set; }
        public decimal Credit { get; set; }
        public decimal Balance { get; set; }
    }
}
