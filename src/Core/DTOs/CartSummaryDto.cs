namespace ECommerce.Core.DTOs
{
    public class CartSummaryDto
    {
        public decimal Subtotal { get; set; }
        public decimal Tax { get; set; }
        public decimal Shipping { get; set; }
        public decimal Total { get; set; }
    }
}
