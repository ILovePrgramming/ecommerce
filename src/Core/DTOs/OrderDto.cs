namespace ECommerce.Core.DTOs
{
    /// <summary>
    /// Represents an order to be submitted for payment.
    /// </summary>
    public class OrderDto
    {
        /// <summary>
        /// Gets or sets the user identifier for the order.
        /// </summary>
        public string UserId { get; set; }

        /// <summary>
        /// Gets or sets the collection of order items.
        /// </summary>
        public List<OrderItemDto> Items { get; set; } = new List<OrderItemDto>();
    }
}
