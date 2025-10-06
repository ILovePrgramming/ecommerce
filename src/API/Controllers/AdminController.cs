namespace ECommerce.API.Controllers
{
    using Core.Interfaces;
    using ECommerce.Core.DTOs;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Http.HttpResults;
    using Microsoft.AspNetCore.Mvc;

    /// <summary>
    /// Controller for admin-specific operations.
    /// Only accessible by users with the Admin role.
    /// </summary>
    [ApiController]
    [Route("api/[controller]")]
    [Authorize(Roles = "Admin")]
    public class AdminController : ControllerBase
    {
        private readonly IAdminService _service;
        private readonly IProductService _productService;

        /// <summary>
        /// Initializes a new instance of the <see cref="AdminController"/> class.
        /// </summary>
        /// <param name="service">The admin service.</param>
        /// <param name="productService">The product service.</param>
        public AdminController(IAdminService service, IProductService productService)
        {
            _service = service;
            _productService = productService;
        }

        /// <summary>
        /// Gets all users in the system.
        /// </summary>
        /// <returns>A list of users.</returns>
        [HttpGet("users")]
        public IActionResult GetUsers() => Ok(_service.GetAllUsers());

        /// <summary>
        /// Promotes a user to admin.
        /// </summary>
        /// <param name="userId">The ID of the user to promote.</param>
        /// <returns>Status of the operation.</returns>
        [HttpPost("promote/{userId}")]
        public IActionResult Promote(string userId)
        {
            _service.PromoteToAdmin(userId);
            return Ok();
        }

        /// <summary>
        /// Creates a new product.
        /// </summary>
        [HttpPost("products")]
        public IActionResult AddProduct(ProductDto dto)
        {
            _productService.Add(dto);
            return Ok();
        }

        /// <summary>
        /// Updates an existing product.
        /// </summary>
        [HttpPut("products/{id}")]
        public IActionResult UpdateProduct(int id, ProductDto dto)
        {
            _productService.Update(id, dto);
            return Ok();
        }

        /// <summary>
        /// Deletes a product.
        /// </summary>
        [HttpDelete("products/{id}")]
        public IActionResult DeleteProduct(int id)
        {
            _productService.Delete(id);
            return NoContent();
        }
    }
}

