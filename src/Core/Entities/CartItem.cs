
namespace ECommerce.Core.Entities
{
    /// <summary>
    /// Represents a cart item entity stored in the database.
    /// </summary>
    public class CartItem
    {
        /// <summary>
        /// Gets or sets the unique identifier for the cart item.
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Gets or sets the product identifier.
        /// </summary>
        public int ProductId { get; set; }

        /// <summary>
        /// Gets or sets the quantity of the product.
        /// </summary>
        public int Quantity { get; set; }

        /// <summary>
        /// Gets or sets the user identifier associated with the cart item.
        /// </summary>
        public string UserId { get; set; } = string.Empty;
    }
}
