
namespace ECommerce.API.Controllers
{
    using Core.Interfaces;
    using Core.DTOs;
    using Microsoft.AspNetCore.Mvc;

    /// <summary>
    /// Provides API endpoints for managing a user's shopping cart.
    /// </summary>
    [ApiController]
    [Route("api/[controller]")]
    public class CartController : ControllerBase
    {
        private readonly ICartService _service;

        /// <summary>
        /// Initializes a new instance of the <see cref="CartController"/> class.
        /// </summary>
        /// <param name="service">The cart service used for cart operations.</param>
        public CartController(ICartService service) => _service = service;

        /// <summary>
        /// Retrieves the cart for the specified user.
        /// </summary>
        /// <param name="userId">The identifier of the user.</param>
        /// <returns>The user's cart.</returns>
        [HttpGet("{userId}")]
        public IActionResult Get(string userId) => Ok(_service.GetCart(userId));

        /// <summary>
        /// Adds an item to the user's cart.
        /// </summary>
        /// <param name="userId">The identifier of the user.</param>
        /// <param name="dto">The cart item to add.</param>
        /// <returns>An <see cref="IActionResult"/> indicating the result of the operation.</returns>
        [HttpPost("{userId}")]
        public IActionResult Add(string userId, CartDto dto)
        {
            _service.AddToCart(userId, dto);
            return Ok();
        }

        /// <summary>
        /// Removes an item from the user's cart.
        /// </summary>
        /// <param name="userId">The identifier of the user.</param>
        /// <param name="productId">The identifier of the product to remove.</param>
        /// <returns>An <see cref="IActionResult"/> indicating the result of the operation.</returns>
        [HttpDelete("{userId}/{productId}")]
        public IActionResult Remove(string userId, int productId)
        {
            _service.RemoveFromCart(userId, productId);
            return NoContent();
        }

        /// <summary>
        /// Clears all items from the user's cart.
        /// </summary>
        /// <param name="userId">The identifier of the user.</param>
        /// <returns>An <see cref="IActionResult"/> indicating the result of the operation.</returns>
        [HttpDelete("clear/{userId}")]
        public IActionResult Clear(string userId)
        {
            _service.ClearCart(userId);
            return NoContent();
        }
    }

}
