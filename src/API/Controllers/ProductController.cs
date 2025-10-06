namespace ECommerce.API.Controllers
{
    using Core.Interfaces;
    using Core.DTOs;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.Extensions.Logging;
    using System.ComponentModel.DataAnnotations;

    /// <summary>
    /// Controller for managing products in the ECommerce API.
    /// Provides endpoints to get, create, update, and delete products.
    /// </summary>
    [ApiController]
    [Route("api/[controller]")]
    public class ProductController : ControllerBase
    {
        private readonly IProductService _service;
        private readonly ILogger<ProductController> _logger;

        /// <summary>
        /// Initializes a new instance of the <see cref="ProductController"/> class.
        /// </summary>
        /// <param name="service">The product service.</param>
        /// <param name="logger">The logger instance.</param>
        public ProductController(IProductService service, ILogger<ProductController> logger)
        {
            _service = service;
            _logger = logger;
        }

        /// <summary>
        /// Gets all products.
        /// Time Complexity: O(m) where m = number of products.
        /// Space Complexity: O(m).
        /// </summary>
        /// <returns>A list of products.</returns>
        [HttpGet]
        public IActionResult GetAll()
        {
            try
            {
                var products = _service.GetAll();
                return Ok(products);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving all products.");
                return StatusCode(500, "Internal server error.");
            }
        }

        /// <summary>
        /// Gets a product by its identifier.
        /// Time Complexity: O(1).
        /// Space Complexity: O(1).
        /// </summary>
        /// <param name="id">The product identifier.</param>
        /// <returns>The product with the specified identifier.</returns>
        [HttpGet("{id}")]
        public IActionResult Get(int id)
        {
            if (id <= 0)
                return BadRequest("Invalid product ID.");

            try
            {
                var product = _service.GetById(id);
                if (product == null)
                    return NotFound();
                return Ok(product);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving product {ProductId}", id);
                return StatusCode(500, "Internal server error.");
            }
        }

        /// <summary>
        /// Gets products with optional search, filter, and pagination.
        /// Time Complexity: O(m) for filtering and search, O(k) for pagination (k = pageSize).
        /// Space Complexity: O(k)
        /// GET /api/product/query?search=phone&minPrice=100&page=2&pageSize=5
        /// </summary>
        [HttpGet("query")]
        public IActionResult Query(
            [FromQuery] string? search,
            [FromQuery] decimal? minPrice,
            [FromQuery] decimal? maxPrice,
            [FromQuery] int page = 1,
            [FromQuery] int pageSize = 10)
        {
            if (page <= 0 || pageSize <= 0)
                return BadRequest("Page and pageSize must be greater than zero.");

            try
            {
                var products = _service.GetAll();

                if (!string.IsNullOrWhiteSpace(search))
                    products = products.Where(p => p.Name.Contains(search, StringComparison.OrdinalIgnoreCase));

                if (minPrice.HasValue)
                    products = products.Where(p => p.Price >= minPrice.Value);
                if (maxPrice.HasValue)
                    products = products.Where(p => p.Price <= maxPrice.Value);

                var totalCount = products.Count();
                var paged = products
                    .Skip((page - 1) * pageSize)
                    .Take(pageSize)
                    .ToList();

                return Ok(new
                {
                    TotalCount = totalCount,
                    Page = page,
                    PageSize = pageSize,
                    Items = paged
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error querying products.");
                return StatusCode(500, "Internal server error.");
            }
        }

        /// <summary>
        /// Adds a new product.
        /// Time Complexity: O(1).
        /// Space Complexity: O(1).
        /// </summary>
        [HttpPost]
        public IActionResult Add([FromBody, Required] ProductDto product)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            try
            {
                _service.Add(product);
                return Ok();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error adding product.");
                return StatusCode(500, "Internal server error.");
            }
        }

        /// <summary>
        /// Updates an existing product.
        /// Time Complexity: O(1).
        /// Space Complexity: O(1).
        /// </summary>
        [HttpPut("{id}")]
        public IActionResult Update(int id, [FromBody, Required] ProductDto product)
        {
            if (id <= 0)
                return BadRequest("Invalid product ID.");
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            try
            {
                _service.Update(id, product);
                return Ok();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error updating product {ProductId}", id);
                return StatusCode(500, "Internal server error.");
            }
        }

        /// <summary>
        /// Deletes a product by its identifier.
        /// Time Complexity: O(1).
        /// Space Complexity: O(1).
        /// </summary>
        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            if (id <= 0)
                return BadRequest("Invalid product ID.");

            try
            {
                _service.Delete(id);
                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error deleting product {ProductId}", id);
                return StatusCode(500, "Internal server error.");
            }
        }
    }
}
