
namespace ECommerce.Core.DTOs
{
    /// <summary>
    /// Data Transfer Object representing a product.
    /// </summary>
    public class ProductDto
    {
        /// <summary>
        /// Gets or sets the name of the product.
        /// </summary>
        public string Name { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the price of the product.
        /// </summary>
        public decimal Price { get; set; }

        /// <summary>
        /// Gets or sets the available stock for the product.
        /// </summary>
        public int Stock { get; set; }
    }
}
