namespace ECommerce.Services.Services
{
    using ECommerce.Core.DTOs;
    using ECommerce.Core.Interfaces;
    using Microsoft.Extensions.Logging;

    /// <summary>
    /// Mock implementation of a payment service integrating with a third-party payment gateway.
    /// </summary>
    public class PaymentService : IPaymentService
    {
        private readonly ILogger<PaymentService> _logger;

        public PaymentService(ILogger<PaymentService> logger)
        {
            _logger = logger;
        }

        /// <summary>
        /// Processes payment for the specified order using a mock gateway.
        /// </summary>
        /// <param name="order">The order to process payment for.</param>
        /// <returns>True if payment is successful; otherwise, false.</returns>
        public bool ProcessPayment(OrderDto order)
        {
            try
            {
                _logger.LogInformation("Processing payment for user {UserId} with {ItemCount} items.", order.UserId, order.Items.Count);

                // Simulate payment gateway call
                if (order.Items.Count > 0)
                {
                    // Simulate success
                    _logger.LogInformation("Payment successful for user {UserId}.", order.UserId);
                    return true;
                }
                else
                {
                    _logger.LogWarning("Payment failed: no items in order for user {UserId}.", order.UserId);
                    return false;
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Payment processing error for user {UserId}.", order.UserId);
                // Optionally, rethrow or handle the exception as needed
                return false;
            }
        }
    }
}