
namespace ECommerce.Core.Interfaces
{
    using DTOs;

    /// <summary>
    /// Provides operations for managing products in the system.
    /// </summary>
    public interface IProductService
    {
        /// <summary>
        /// Retrieves all products.
        /// </summary>
        /// <returns>A collection of <see cref="ProductDto"/> objects.</returns>
        IEnumerable<ProductDto> GetAll();

        /// <summary>
        /// Retrieves a product by its unique identifier.
        /// </summary>
        /// <param name="id">The unique identifier of the product.</param>
        /// <returns>The <see cref="ProductDto"/> with the specified identifier.</returns>
        ProductDto GetById(int id);

        /// <summary>
        /// Adds a new product.
        /// </summary>
        /// <param name="product">The product to add.</param>
        void Add(ProductDto product);

        /// <summary>
        /// Updates an existing product.
        /// </summary>
        /// <param name="id">The unique identifier of the product to update.</param>
        /// <param name="product">The updated product information.</param>
        void Update(int id, ProductDto product);

        /// <summary>
        /// Deletes a product by its unique identifier.
        /// </summary>
        /// <param name="id">The unique identifier of the product to delete.</param>
        void Delete(int id);
    }
}
