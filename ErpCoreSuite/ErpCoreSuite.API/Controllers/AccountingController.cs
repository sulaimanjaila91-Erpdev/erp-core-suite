// ErpCoreSuite.API/Controllers/AccountingController.cs
using ErpCoreSuite.Core.Entities;
using ErpCoreSuite.Core.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ErpCoreSuite.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class AccountingController : ControllerBase
    {
        private readonly IAccountingRepository _repo;
        private readonly ILogger<AccountingController> _logger;

        public AccountingController(IAccountingRepository repo, ILogger<AccountingController> logger)
        {
            _repo = repo;
            _logger = logger;
        }

        // GET api/accounting/accounts
        [HttpGet("accounts")]
        public async Task<IActionResult> GetChartOfAccounts()
        {
            var accounts = await _repo.GetChartOfAccountsAsync();
            return Ok(new { success = true, data = accounts });
        }

        // POST api/accounting/journal
        [HttpPost("journal")]
        [Authorize(Roles = "Admin,Accountant")]
        public async Task<IActionResult> AddJournalEntry([FromBody] JournalEntry entry)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            // Validate that total debits == total credits
            var totalDebit  = entry.Lines.Sum(l => l.DebitAmount);
            var totalCredit = entry.Lines.Sum(l => l.CreditAmount);

            if (totalDebit != totalCredit)
                return BadRequest(new
                {
                    success = false,
                    message = $"Journal entry is unbalanced. Debit: {totalDebit}, Credit: {totalCredit}"
                });

            entry.CreatedBy = User.Identity?.Name;
            var id = await _repo.AddJournalEntryAsync(entry);
            return Ok(new { success = true, journalId = id, message = "Journal entry saved as Draft" });
        }

        // PUT api/accounting/journal/{id}/post
        [HttpPut("journal/{id}/post")]
        [Authorize(Roles = "Admin,Accountant")]
        public async Task<IActionResult> PostJournalEntry(int id)
        {
            var postedBy = User.Identity?.Name;
            var result = await _repo.PostJournalEntryAsync(id, postedBy);
            if (!result)
                return NotFound(new { success = false, message = "Journal entry not found" });

            return Ok(new { success = true, message = "Journal entry posted to ledger" });
        }

        // GET api/accounting/ledger?accountCode=1001&from=2024-01-01&to=2024-12-31
        [HttpGet("ledger")]
        public async Task<IActionResult> GetLedger(
            [FromQuery] string accountCode,
            [FromQuery] DateTime from,
            [FromQuery] DateTime to)
        {
            var ledger = await _repo.GetLedgerAsync(accountCode, from, to);
            return Ok(new { success = true, data = ledger });
        }

        // GET api/accounting/trialbalance?asOfDate=2024-12-31
        [HttpGet("trialbalance")]
        public async Task<IActionResult> GetTrialBalance([FromQuery] DateTime asOfDate)
        {
            var trial = await _repo.GetTrialBalanceAsync(asOfDate);

            var totalDebit  = trial.Sum(t => t.Debit);
            var totalCredit = trial.Sum(t => t.Credit);

            return Ok(new
            {
                success = true,
                asOfDate,
                totalDebit,
                totalCredit,
                isBalanced = totalDebit == totalCredit,
                data = trial
            });
        }
    }
}
