namespace ECommerce.Services.Services
{
    using Core.DTOs;
    using Core.Entities;
    using Core.Interfaces;
    using Data.Repositories;
    using System.Collections.Generic;
    using System.Linq;

    /// <summary>
    /// Provides methods for managing a user's shopping cart.
    /// </summary>
    public class CartService : ICartService
    {
        private readonly CartRepository _repo;

        /// <summary>
        /// Initializes a new instance of the <see cref="CartService"/> class with the specified cart repository.
        /// </summary>
        /// <param name="repo">The cart repository to use for data access.</param>
        public CartService(CartRepository repo) => _repo = repo;

        /// <summary>
        /// Retrieves the cart items for the specified user.
        /// </summary>
        /// <param name="userId">The user's identifier.</param>
        /// <returns>A collection of <see cref="CartDto"/> objects representing the user's cart items.</returns>
        public IEnumerable<CartDto> GetCart(string userId) =>
            _repo.GetCart(userId).Select(c => new CartDto { ProductId = c.ProductId, Quantity = c.Quantity });

        /// <summary>
        /// Adds an item to the user's cart.
        /// </summary>
        /// <param name="userId">The user's identifier.</param>
        /// <param name="dto">The cart item to add.</param>
        public void AddToCart(string userId, CartDto dto)
        {
            var item = new CartItem { UserId = userId, ProductId = dto.ProductId, Quantity = dto.Quantity };
            _repo.Add(item);
            _repo.Save();
        }

        /// <summary>
        /// Removes a product from the user's cart.
        /// </summary>
        /// <param name="userId">The user's identifier.</param>
        /// <param name="productId">The product identifier to remove.</param>
        public void RemoveFromCart(string userId, int productId)
        {
            _repo.Remove(userId, productId);
            _repo.Save();
        }

        /// <summary>
        /// Clears all items from the user's cart.
        /// </summary>
        /// <param name="userId">The user's identifier.</param>
        public void ClearCart(string userId)
        {
            _repo.Clear(userId);
            _repo.Save();
        }
    }
}
