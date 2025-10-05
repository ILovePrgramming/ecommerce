namespace ECommerce.Core.Interfaces
{
    using DTOs;

    /// <summary>
    /// Provides methods for managing a user's shopping cart.
    /// </summary>
    public interface ICartService
    {
        /// <summary>
        /// Retrieves the cart items for the specified user.
        /// </summary>
        /// <param name="userId">The user's identifier.</param>
        /// <returns>A collection of cart items.</returns>
        IEnumerable<CartDto> GetCart(string userId);

        /// <summary>
        /// Adds an item to the user's cart.
        /// </summary>
        /// <param name="userId">The user's identifier.</param>
        /// <param name="item">The cart item to add.</param>
        void AddToCart(string userId, CartDto item);

        /// <summary>
        /// Removes a product from the user's cart.
        /// </summary>
        /// <param name="userId">The user's identifier.</param>
        /// <param name="productId">The product identifier to remove.</param>
        void RemoveFromCart(string userId, int productId);

        /// <summary>
        /// Clears all items from the user's cart.
        /// </summary>
        /// <param name="userId">The user's identifier.</param>
        void ClearCart(string userId);
    }
}
