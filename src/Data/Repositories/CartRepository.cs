namespace ECommerce.Data.Repositories
{
    using Context;
    using Core.Entities;
    using System.Collections.Generic;
    using System.Linq;

    /// <summary>
    /// Provides methods for managing shopping cart items in the database for a specific user.
    /// </summary>
    public class CartRepository
    {
        private readonly AppDbContext _context;

        /// <summary>
        /// Initializes a new instance of the <see cref="CartRepository"/> class with the specified database context.
        /// </summary>
        /// <param name="context">The application's database context.</param>
        public CartRepository(AppDbContext context) => _context = context;

        /// <summary>
        /// Retrieves all cart items for the specified user.
        /// </summary>
        /// <param name="userId">The user identifier.</param>
        /// <returns>A collection of <see cref="CartItem"/> objects belonging to the user.</returns>
        public IEnumerable<CartItem> GetCart(string userId) =>
            _context.CartItems.Where(c => c.UserId == userId).ToList();

        /// <summary>
        /// Adds a new cart item to the database.
        /// </summary>
        /// <param name="item">The <see cref="CartItem"/> to add.</param>
        public void Add(CartItem item) => _context.CartItems.Add(item);

        /// <summary>
        /// Removes a cart item for the specified user and product.
        /// </summary>
        /// <param name="userId">The user identifier.</param>
        /// <param name="productId">The product identifier.</param>
        public void Remove(string userId, int productId)
        {
            var item = _context.CartItems.FirstOrDefault(c => c.UserId == userId && c.ProductId == productId);
            if (item != null) _context.CartItems.Remove(item);
        }

        /// <summary>
        /// Removes all cart items for the specified user.
        /// </summary>
        /// <param name="userId">The user identifier.</param>
        public void Clear(string userId)
        {
            var items = _context.CartItems.Where(c => c.UserId == userId);
            _context.CartItems.RemoveRange(items);
        }

        /// <summary>
        /// Saves all changes made in this context to the database.
        /// </summary>
        public void Save() => _context.SaveChanges();
    }
}
