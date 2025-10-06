namespace ECommerce.Services.Services
{
    using Core.DTOs;
    using Core.Entities;
    using Core.Interfaces;
    using Data.Repositories;
    using Microsoft.Extensions.Logging;
    using System.Collections.Generic;
    using System.Linq;

    /// <summary>
    /// Provides methods for managing a user's shopping cart.
    /// </summary>
    public class CartService : ICartService
    {
        private readonly CartRepository _repo;
        private readonly ProductRepository _productRepo;
        private readonly ILogger<CartService> _logger;
        private readonly IPaymentService _paymentService;

        /// <summary>
        /// Initializes a new instance of the <see cref="CartService"/> class with the specified repositories and payment service.
        /// </summary>
        /// <param name="repo">The cart repository to use for data access.</param>
        /// <param name="productRepo">The product repository to use for product data.</param>
        /// <param name="logger">The logger to use for logging errors.</param>
        /// <param name="paymentService">The payment service to process orders.</param>
        public CartService(CartRepository repo, ProductRepository productRepo, ILogger<CartService> logger, IPaymentService paymentService)
        {
            _repo = repo;
            _productRepo = productRepo;
            _logger = logger;
            _paymentService = paymentService;
        }

        /// <summary>
        /// Retrieves the cart items for the specified user.
        /// </summary>
        /// <param name="userId">The user's identifier.</param>
        /// <returns>A collection of <see cref="CartDto"/> objects representing the user's cart items.</returns>
        public IEnumerable<CartDto> GetCart(string userId)
        {
            if (string.IsNullOrWhiteSpace(userId))
                throw new System.ArgumentException("UserId is required.");

            try
            {
                return _repo.GetCart(userId).Select(c => new CartDto { ProductId = c.ProductId, Quantity = c.Quantity });
            }
            catch (System.Exception ex)
            {
                _logger.LogError(ex, "Error retrieving cart for user {UserId}", userId);
                throw;
            }
        }

        /// <summary>
        /// Adds an item to the user's cart.
        /// </summary>
        /// <param name="userId">The user's identifier.</param>
        /// <param name="dto">The cart item to add.</param>
        public void AddToCart(string userId, CartDto dto)
        {
            if (string.IsNullOrWhiteSpace(userId))
                throw new System.ArgumentException("UserId is required.");
            if (dto == null || dto.ProductId <= 0 || dto.Quantity <= 0)
                throw new System.ArgumentException("Invalid cart item.");

            var product = _productRepo.GetById(dto.ProductId);
            if (product == null || product.Stock < dto.Quantity)
                throw new InvalidOperationException("Insufficient stock.");

            try
            {
                var item = new CartItem
                {
                    UserId = userId,
                    ProductId = dto.ProductId,
                    Quantity = dto.Quantity,
                    AddedAt = DateTime.UtcNow
                };
                _repo.Add(item);
                _repo.Save();
            }
            catch (System.Exception ex)
            {
                _logger.LogError(ex, "Error adding item to cart for user {UserId}", userId);
                throw;
            }
        }

        /// <summary>
        /// Updates the quantity of an item in the user's cart.
        /// </summary>
        /// <param name="userId">The user's identifier.</param>
        /// <param name="productId">The product identifier.</param>
        /// <param name="newQuantity">The new quantity for the cart item.</param>
        public void UpdateCartItem(string userId, int productId, int newQuantity)
        {
            if (string.IsNullOrWhiteSpace(userId) || productId <= 0 || newQuantity <= 0)
                throw new System.ArgumentException("Invalid input.");

            var product = _productRepo.GetById(productId);
            if (product == null || product.Stock < newQuantity)
                throw new InvalidOperationException("Insufficient stock.");

            try
            {
                _repo.UpdateQuantity(userId, productId, newQuantity);
                _repo.Save();
            }
            catch (System.Exception ex)
            {
                _logger.LogError(ex, "Error updating cart item {ProductId} for user {UserId}", productId, userId);
                throw;
            }
        }

        /// <summary>
        /// Removes a product from the user's cart.
        /// </summary>
        /// <param name="userId">The user's identifier.</param>
        /// <param name="productId">The product identifier to remove.</param>
        public void RemoveFromCart(string userId, int productId)
        {
            if (string.IsNullOrWhiteSpace(userId))
                throw new System.ArgumentException("UserId is required.");
            if (productId <= 0)
                throw new System.ArgumentException("Invalid productId.");

            try
            {
                _repo.Remove(userId, productId);
                _repo.Save();
            }
            catch (System.Exception ex)
            {
                _logger.LogError(ex, "Error removing item {ProductId} from cart for user {UserId}", productId, userId);
                throw;
            }
        }

        /// <summary>
        /// Clears all items from the user's cart.
        /// </summary>
        /// <param name="userId">The user's identifier.</param>
        public void ClearCart(string userId)
        {
            if (string.IsNullOrWhiteSpace(userId))
                throw new System.ArgumentException("UserId is required.");

            try
            {
                _repo.Clear(userId);
                _repo.Save();
            }
            catch (System.Exception ex)
            {
                _logger.LogError(ex, "Error clearing cart for user {UserId}", userId);
                throw;
            }
        }

        /// <summary>
        /// Checks out the user's cart and submits the order to the payment service.
        /// </summary>
        /// <param name="userId">The user's identifier.</param>
        /// <returns>True if checkout and payment succeed; otherwise, false.</returns>
        public bool Checkout(string userId)
        {
            if (string.IsNullOrWhiteSpace(userId))
                throw new System.ArgumentException("UserId is required.");

            try
            {
                var cartItems = _repo.GetCart(userId).ToList();
                if (!cartItems.Any())
                {
                    _logger.LogWarning("Checkout failed: cart is empty for user {UserId}", userId);
                    return false;
                }

                // Map cart items to order DTO
                var order = new OrderDto
                {
                    UserId = userId,
                    Items = cartItems.Select(c => new OrderItemDto
                    {
                        ProductId = c.ProductId,
                        Quantity = c.Quantity
                    }).ToList()
                };

                // Call payment service
                var paymentResult = _paymentService.ProcessPayment(order);

                if (paymentResult)
                {
                    _logger.LogInformation("Checkout successful for user {UserId}", userId);
                    _repo.Clear(userId);
                    _repo.Save();
                    return true;
                }
                else
                {
                    _logger.LogWarning("Payment failed for user {UserId}", userId);
                    return false;
                }
            }
            catch (System.Exception ex)
            {
                _logger.LogError(ex, "Error during checkout for user {UserId}", userId);
                throw;
            }
        }
    }
}
