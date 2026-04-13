// ErpCoreSuite.API/Controllers/PayrollController.cs
using ErpCoreSuite.Core.Entities;
using ErpCoreSuite.Core.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ErpCoreSuite.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class PayrollController : ControllerBase
    {
        private readonly IPayrollRepository _repo;
        private readonly ILogger<PayrollController> _logger;

        public PayrollController(IPayrollRepository repo, ILogger<PayrollController> logger)
        {
            _repo = repo;
            _logger = logger;
        }

        // GET api/payroll/employees
        [HttpGet("employees")]
        public async Task<IActionResult> GetEmployees()
        {
            var employees = await _repo.GetAllEmployeesAsync();
            return Ok(new { success = true, data = employees });
        }

        // GET api/payroll/employees/{id}
        [HttpGet("employees/{id}")]
        public async Task<IActionResult> GetEmployee(int id)
        {
            var employee = await _repo.GetEmployeeByIdAsync(id);
            if (employee == null)
                return NotFound(new { success = false, message = "Employee not found" });

            return Ok(new { success = true, data = employee });
        }

        // POST api/payroll/employees
        [HttpPost("employees")]
        [Authorize(Roles = "Admin,HR")]
        public async Task<IActionResult> AddEmployee([FromBody] Employee employee)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var id = await _repo.AddEmployeeAsync(employee);
            return Ok(new { success = true, employeeId = id, message = "Employee added successfully" });
        }

        // PUT api/payroll/employees/{id}
        [HttpPut("employees/{id}")]
        [Authorize(Roles = "Admin,HR")]
        public async Task<IActionResult> UpdateEmployee(int id, [FromBody] Employee employee)
        {
            employee.EmployeeId = id;
            var result = await _repo.UpdateEmployeeAsync(employee);
            if (!result)
                return NotFound(new { success = false, message = "Employee not found" });

            return Ok(new { success = true, message = "Employee updated" });
        }

        // GET api/payroll/records?month=1&year=2025
        [HttpGet("records")]
        public async Task<IActionResult> GetPayrollRecords([FromQuery] int month, [FromQuery] int year)
        {
            var records = await _repo.GetPayrollByMonthAsync(month, year);
            return Ok(new { success = true, data = records });
        }

        // POST api/payroll/generate?month=1&year=2025
        [HttpPost("generate")]
        [Authorize(Roles = "Admin,HR")]
        public async Task<IActionResult> GeneratePayroll([FromQuery] int month, [FromQuery] int year)
        {
            var processedBy = User.Identity?.Name;
            var count = await _repo.GeneratePayrollAsync(month, year, processedBy);
            return Ok(new
            {
                success = true,
                recordsGenerated = count,
                message = $"Payroll generated for {month}/{year} — {count} employee records created"
            });
        }

        // PUT api/payroll/approve/{id}
        [HttpPut("approve/{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> ApprovePayroll(int id)
        {
            var approvedBy = User.Identity?.Name;
            var result = await _repo.ApprovePayrollAsync(id, approvedBy);
            if (!result)
                return NotFound(new { success = false, message = "Payroll record not found" });

            return Ok(new { success = true, message = "Payroll approved" });
        }

        // GET api/payroll/payslip/{id}
        [HttpGet("payslip/{id}")]
        public async Task<IActionResult> GetPayslip(int id)
        {
            var payslip = await _repo.GetPayslipAsync(id);
            if (payslip == null)
                return NotFound(new { success = false, message = "Payslip not found" });

            return Ok(new { success = true, data = payslip });
        }
    }
}
