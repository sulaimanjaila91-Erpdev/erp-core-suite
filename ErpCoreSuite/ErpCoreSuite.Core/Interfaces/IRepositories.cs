// ErpCoreSuite.Core/Interfaces/IInventoryRepository.cs
using ErpCoreSuite.Core.Entities;

namespace ErpCoreSuite.Core.Interfaces
{
    public interface IInventoryRepository
    {
        Task<IEnumerable<Product>> GetAllProductsAsync();
        Task<Product> GetProductByIdAsync(int productId);
        Task<IEnumerable<Product>> GetLowStockProductsAsync();
        Task<int> AddProductAsync(Product product);
        Task<bool> UpdateProductAsync(Product product);
        Task<bool> DeleteProductAsync(int productId);
        Task<IEnumerable<StockTransaction>> GetStockTransactionsAsync(DateTime from, DateTime to);
        Task<int> PostStockTransactionAsync(StockTransaction transaction);
    }

    public interface IPayrollRepository
    {
        Task<IEnumerable<Employee>> GetAllEmployeesAsync();
        Task<Employee> GetEmployeeByIdAsync(int employeeId);
        Task<int> AddEmployeeAsync(Employee employee);
        Task<bool> UpdateEmployeeAsync(Employee employee);
        Task<IEnumerable<PayrollRecord>> GetPayrollByMonthAsync(int month, int year);
        Task<int> GeneratePayrollAsync(int month, int year, string processedBy);
        Task<bool> ApprovePayrollAsync(int payrollId, string approvedBy);
        Task<PayrollRecord> GetPayslipAsync(int payrollId);
    }

    public interface IAccountingRepository
    {
        Task<IEnumerable<ChartOfAccount>> GetChartOfAccountsAsync();
        Task<int> AddJournalEntryAsync(JournalEntry entry);
        Task<bool> PostJournalEntryAsync(int journalId, string postedBy);
        Task<IEnumerable<LedgerEntry>> GetLedgerAsync(string accountCode, DateTime from, DateTime to);
        Task<IEnumerable<LedgerEntry>> GetTrialBalanceAsync(DateTime asOfDate);
    }
}
