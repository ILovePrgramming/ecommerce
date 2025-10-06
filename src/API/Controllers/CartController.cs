namespace ECommerce.API.Controllers
{
    using Core.DTOs;
    using Core.Interfaces;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.Extensions.Logging;
    using System.ComponentModel.DataAnnotations;
    using System.Text.Json;

    /// <summary>
    /// Provides API endpoints for managing a user's shopping cart, including session-based cart operations.
    /// </summary>
    [ApiController]
    [Route("api/[controller]")]
    public class CartController : ControllerBase
    {
        private const string CartSessionKey = "Cart";
        private readonly ICartService _service;
        private readonly ILogger<CartController> _logger;

        /// <summary>
        /// Initializes a new instance of the <see cref="CartController"/> class.
        /// </summary>
        /// <param name="service">The cart service used for cart operations.</param>
        /// <param name="logger">The logger instance for logging errors and information.</param>
        public CartController(ICartService service, ILogger<CartController> logger)
        {
            _service = service;
            _logger = logger;
        }

        private List<CartDto> GetSessionCart()
        {
            var cartJson = HttpContext.Session.GetString(CartSessionKey);
            return cartJson == null ? new List<CartDto>() : JsonSerializer.Deserialize<List<CartDto>>(cartJson);
        }

        private void SaveSessionCart(List<CartDto> cart)
        {
            var cartJson = JsonSerializer.Serialize(cart);
            HttpContext.Session.SetString(CartSessionKey, cartJson);
        }

        /// <summary>
        /// Retrieves the cart for the specified user from persistent storage.
        /// Time Complexity: O(n) where n = number of cart items for the user.
        /// Space Complexity: O(n) for the returned cart list.
        /// </summary>
        [HttpGet("{userId}")]
        public IActionResult Get(string userId)
        {
            if (string.IsNullOrWhiteSpace(userId))
                return BadRequest("UserId is required.");

            try
            {
                var cart = _service.GetCart(userId);
                return Ok(cart);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving cart for user {UserId}", userId);
                return StatusCode(500, "Internal server error.");
            }
        }

        /// <summary>
        /// Adds an item to the cart stored in the current session.
        /// Time Complexity: O(n) for searching existing item; O(1) for add.
        /// Space Complexity: O(n) for session cart.
        /// </summary>
        [HttpPost("session/add")]
        public IActionResult AddToSessionCart([FromBody, Required] CartDto dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            try
            {
                var cart = GetSessionCart();
                var existing = cart.FirstOrDefault(x => x.ProductId == dto.ProductId);
                if (existing != null)
                    existing.Quantity += dto.Quantity;
                else
                    cart.Add(dto);

                SaveSessionCart(cart);
                return Ok(cart);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error adding item to session cart.");
                return StatusCode(500, "Internal server error.");
            }
        }

        /// <summary>
        /// Removes an item from the cart stored in the current session.
        /// Time Complexity: O(n) for removal.
        /// Space Complexity: O(n) for session cart.
        /// </summary>
        [HttpDelete("session/remove/{productId}")]
        public IActionResult RemoveFromSessionCart(int productId)
        {
            if (productId <= 0)
                return BadRequest("Invalid productId.");

            try
            {
                var cart = GetSessionCart();
                cart.RemoveAll(x => x.ProductId == productId);
                SaveSessionCart(cart);
                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error removing item {ProductId} from session cart.", productId);
                return StatusCode(500, "Internal server error.");
            }
        }

        /// <summary>
        /// Clears all items from the cart stored in the current session.
        /// Time Complexity: O(1).
        /// Space Complexity: O(1).
        /// </summary>
        [HttpDelete("session/clear")]
        public IActionResult ClearSessionCart()
        {
            try
            {
                SaveSessionCart(new List<CartDto>());
                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error clearing session cart.");
                return StatusCode(500, "Internal server error.");
            }
        }

        /// <summary>
        /// Adds an item to the user's cart in persistent storage.
        /// Time Complexity: O(1) for add; O(1) for stock check.
        /// Space Complexity: O(1).
        /// </summary>
        [HttpPost("{userId}")]
        public IActionResult AddToCart(string userId, [FromBody, Required] CartDto dto)
        {
            if (string.IsNullOrWhiteSpace(userId))
                return BadRequest("UserId is required.");
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            try
            {
                _service.AddToCart(userId, dto);
                return Ok();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error adding item to cart for user {UserId}", userId);
                return StatusCode(500, "Internal server error.");
            }
        }

        /// <summary>
        /// Removes an item from the user's cart in persistent storage.
        /// Time Complexity: O(1) for removal.
        /// Space Complexity: O(1).
        /// </summary>
        [HttpDelete("{userId}/{productId}")]
        public IActionResult Remove(string userId, int productId)
        {
            if (string.IsNullOrWhiteSpace(userId))
                return BadRequest("UserId is required.");
            if (productId <= 0)
                return BadRequest("Invalid productId.");

            try
            {
                _service.RemoveFromCart(userId, productId);
                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error removing item {ProductId} from cart for user {UserId}", productId, userId);
                return StatusCode(500, "Internal server error.");
            }
        }

        /// <summary>
        /// Clears all items from the user's cart in persistent storage.
        /// Time Complexity: O(n) where n = number of cart items.
        /// Space Complexity: O(1).
        /// </summary>
        [HttpDelete("clear/{userId}")]
        public IActionResult Clear(string userId)
        {
            if (string.IsNullOrWhiteSpace(userId))
                return BadRequest("UserId is required.");

            try
            {
                _service.ClearCart(userId);
                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error clearing cart for user {UserId}", userId);
                return StatusCode(500, "Internal server error.");
            }
        }
    }
}
