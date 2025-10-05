
namespace ECommerce.API.Controllers
{
    using Core.DTOs;
    using Core.Interfaces;
    using Microsoft.AspNetCore.Mvc;

    /// <summary>
    /// Controller for managing products in the ECommerce API.
    /// Provides endpoints to get, create, update, and delete products.
    /// </summary>
    [ApiController]
    [Route("api/[controller]")]
    public class ProductController : ControllerBase
    {
        private readonly IProductService _service;

        /// <summary>
        /// Initializes a new instance of the <see cref="ProductController"/> class.
        /// </summary>
        /// <param name="service">The product service.</param>
        public ProductController(IProductService service)
        {
            _service = service;
        }

        /// <summary>
        /// Gets all products.
        /// </summary>
        /// <returns>A list of products.</returns>
        [HttpGet]
        public IActionResult GetAll() => Ok(_service.GetAll());

        /// <summary>
        /// Gets a product by its identifier.
        /// </summary>
        /// <param name="id">The product identifier.</param>
        /// <returns>The product with the specified identifier.</returns>
        [HttpGet("{id}")]
        public IActionResult Get(int id) => Ok(_service.GetById(id));

        /// <summary>
        /// Creates a new product.
        /// </summary>
        /// <param name="dto">The product data transfer object.</param>
        /// <returns>The created product.</returns>
        [HttpPost]
        public IActionResult Create(ProductDto dto)
        {
            _service.Add(dto);
            return CreatedAtAction(nameof(Get), new { id = dto.Name }, dto);
        }

        /// <summary>
        /// Updates an existing product.
        /// </summary>
        /// <param name="id">The product identifier.</param>
        /// <param name="dto">The product data transfer object.</param>
        /// <returns>No content.</returns>
        [HttpPut("{id}")]
        public IActionResult Update(int id, ProductDto dto)
        {
            _service.Update(id, dto);
            return NoContent();
        }

        /// <summary>
        /// Deletes a product by its identifier.
        /// </summary>
        /// <param name="id">The product identifier.</param>
        /// <returns>No content.</returns>
        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            _service.Delete(id);
            return NoContent();
        }
    }

}
