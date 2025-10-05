namespace ECommerce.Services.Services
{
    using Core.DTOs;
    using Core.Entities;
    using Core.Interfaces;
    using Data.Repositories;
    using System.Collections.Generic;
    using System.Linq;

    /// <summary>
    /// Provides business logic for managing products in the e-commerce system.
    /// </summary>
    public class ProductService : IProductService
    {
        private readonly ProductRepository _repo;

        /// <summary>
        /// Initializes a new instance of the <see cref="ProductService"/> class.
        /// </summary>
        /// <param name="repo">The product repository used for data access.</param>
        public ProductService(ProductRepository repo)
        {
            _repo = repo;
        }

        /// <summary>
        /// Retrieves all products as DTOs.
        /// </summary>
        /// <returns>An enumerable collection of <see cref="ProductDto"/> objects.</returns>
        public IEnumerable<ProductDto> GetAll() =>
            _repo.GetAll().Select(p => new ProductDto { Name = p.Name, Price = p.Price, Stock = p.Stock });

        /// <summary>
        /// Retrieves a product by its unique identifier.
        /// </summary>
        /// <param name="id">The unique identifier of the product.</param>
        /// <returns>The <see cref="ProductDto"/> if found; otherwise, throws <see cref="KeyNotFoundException"/>.</returns>
        public ProductDto GetById(int id)
        {
            var product = _repo.GetById(id);
            if (product == null) throw new KeyNotFoundException("Product not found");
            return new ProductDto { Name = product.Name, Price = product.Price, Stock = product.Stock };
        }

        /// <summary>
        /// Adds a new product to the system.
        /// </summary>
        /// <param name="dto">The product data transfer object to add.</param>
        public void Add(ProductDto dto)
        {
            var product = new Product { Name = dto.Name, Price = dto.Price, Stock = dto.Stock };
            _repo.Add(product);
            _repo.Save();
        }

        /// <summary>
        /// Updates an existing product.
        /// </summary>
        /// <param name="id">The unique identifier of the product to update.</param>
        /// <param name="dto">The updated product data transfer object.</param>
        public void Update(int id, ProductDto dto)
        {
            var product = _repo.GetById(id);
            if (product == null) throw new KeyNotFoundException("Product not found");
            product.Name = dto.Name;
            product.Price = dto.Price;
            product.Stock = dto.Stock;
            _repo.Save();
        }

        /// <summary>
        /// Deletes a product by its unique identifier.
        /// </summary>
        /// <param name="id">The unique identifier of the product to delete.</param>
        public void Delete(int id)
        {
            var product = _repo.GetById(id);
            if (product == null) throw new KeyNotFoundException("Product not found");
            // _context is not defined in this class; deletion should be handled by repository.
            // Assuming ProductRepository should have a Delete method.
            // _repo.Delete(product);
            // _repo.Save();
            throw new NotImplementedException("Delete logic should be implemented in ProductRepository.");
        }
    }
}
