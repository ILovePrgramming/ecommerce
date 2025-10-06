namespace ECommerce.Core.DTOs
{
    /// <summary>
    /// Represents a single item in an order.
    /// </summary>
    public class OrderItemDto
    {
        /// <summary>
        /// Gets or sets the product identifier.
        /// </summary>
        public int ProductId { get; set; }

        /// <summary>
        /// Gets or sets the quantity of the product.
        /// </summary>
        public int Quantity { get; set; }
    }
}
