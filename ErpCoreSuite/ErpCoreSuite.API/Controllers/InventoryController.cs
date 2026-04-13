// ErpCoreSuite.API/Controllers/InventoryController.cs
using ErpCoreSuite.Core.Entities;
using ErpCoreSuite.Core.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ErpCoreSuite.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class InventoryController : ControllerBase
    {
        private readonly IInventoryRepository _repo;
        private readonly ILogger<InventoryController> _logger;

        public InventoryController(IInventoryRepository repo, ILogger<InventoryController> logger)
        {
            _repo = repo;
            _logger = logger;
        }

        // GET api/inventory/products
        [HttpGet("products")]
        public async Task<IActionResult> GetAllProducts()
        {
            try
            {
                var products = await _repo.GetAllProductsAsync();
                return Ok(new { success = true, data = products });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error fetching products");
                return StatusCode(500, new { success = false, message = "Internal server error" });
            }
        }

        // GET api/inventory/products/{id}
        [HttpGet("products/{id}")]
        public async Task<IActionResult> GetProduct(int id)
        {
            var product = await _repo.GetProductByIdAsync(id);
            if (product == null)
                return NotFound(new { success = false, message = "Product not found" });

            return Ok(new { success = true, data = product });
        }

        // GET api/inventory/lowstock
        [HttpGet("lowstock")]
        public async Task<IActionResult> GetLowStockProducts()
        {
            var products = await _repo.GetLowStockProductsAsync();
            return Ok(new { success = true, data = products, count = products.Count() });
        }

        // POST api/inventory/products
        [HttpPost("products")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> AddProduct([FromBody] Product product)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            product.CreatedDate = DateTime.Now;
            product.IsActive = true;
            var id = await _repo.AddProductAsync(product);
            return Ok(new { success = true, productId = id, message = "Product added successfully" });
        }

        // PUT api/inventory/products/{id}
        [HttpPut("products/{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> UpdateProduct(int id, [FromBody] Product product)
        {
            product.ProductId = id;
            var result = await _repo.UpdateProductAsync(product);
            if (!result)
                return NotFound(new { success = false, message = "Product not found" });

            return Ok(new { success = true, message = "Product updated successfully" });
        }

        // GET api/inventory/transactions?from=2024-01-01&to=2024-12-31
        [HttpGet("transactions")]
        public async Task<IActionResult> GetTransactions([FromQuery] DateTime from, [FromQuery] DateTime to)
        {
            var transactions = await _repo.GetStockTransactionsAsync(from, to);
            return Ok(new { success = true, data = transactions });
        }

        // POST api/inventory/transactions
        [HttpPost("transactions")]
        public async Task<IActionResult> PostTransaction([FromBody] StockTransaction transaction)
        {
            transaction.CreatedBy = User.Identity?.Name;
            transaction.TransactionDate = DateTime.Now;
            var id = await _repo.PostStockTransactionAsync(transaction);
            return Ok(new { success = true, transactionId = id, message = "Stock transaction posted successfully" });
        }
    }
}
