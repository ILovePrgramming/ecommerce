namespace ECommerce.Core.Interfaces
{
    using ECommerce.Core.DTOs;

    /// <summary>
    /// Provides methods for processing payments through a payment gateway.
    /// </summary>
    public interface IPaymentService
    {
        /// <summary>
        /// Processes payment for the specified order.
        /// </summary>
        /// <param name="order">The order to process payment for.</param>
        /// <returns>True if payment is successful; otherwise, false.</returns>
        bool ProcessPayment(OrderDto order);
    }
}