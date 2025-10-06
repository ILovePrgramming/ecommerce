namespace ECommerce.Services.Services
{
    using Core.DTOs;
    using Core.Entities;
    using Core.Interfaces;
    using ECommerce.Data.Context;
    using ECommerce.Data.Repositories;
    using Microsoft.Extensions.Logging;
    using System;
    using System.Collections.Generic;
    using System.Linq;

    /// <summary>
    /// Provides business logic for managing products in the e-commerce system.
    /// </summary>
    public class ProductService : IProductService
    {
        private readonly ProductRepository _productRepo;
        private readonly ILogger<ProductService> _logger;

        /// <summary>
        /// Initializes a new instance of the <see cref="ProductService"/> class.
        /// </summary>
        /// <param name="context">The database context used for data access.</param>
        /// <param name="logger">The logger for logging errors and information.</param>
        public ProductService(ProductRepository productRepo, ILogger<ProductService> logger)
        {
            _productRepo = productRepo;
            _logger = logger;
        }

        /// <summary>
        /// Retrieves all products as DTOs.
        /// </summary>
        /// <returns>An enumerable collection of <see cref="ProductDto"/> objects.</returns>
        public IEnumerable<ProductDto> GetAll()
        {
            try
            {
                return _productRepo.GetAll().Select(p => new ProductDto
                {
                    Name = p.Name,
                    Price = p.Price,
                    Stock = p.Stock
                }).ToList();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving all products.");
                throw;
            }
        }

        /// <summary>
        /// Retrieves a product by its unique identifier.
        /// </summary>
        /// <param name="id">The unique identifier of the product.</param>
        /// <returns>The <see cref="ProductDto"/> if found; otherwise, throws <see cref="KeyNotFoundException"/>.</returns>
        public ProductDto GetById(int id)
        {
            if (id <= 0)
                throw new ArgumentException("Invalid product ID.");

            try
            {
                var product = _productRepo.GetById(id);
                if (product == null) return null;
                return new ProductDto
                {
                    Name = product.Name,
                    Price = product.Price,
                    Stock = product.Stock
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving product {ProductId}", id);
                throw;
            }
        }

        /// <summary>
        /// Adds a new product to the system.
        /// </summary>
        /// <param name="dto">The product data transfer object to add.</param>
        public void Add(ProductDto dto)
        {
            if (dto == null)
                throw new ArgumentNullException(nameof(dto));
            if (string.IsNullOrWhiteSpace(dto.Name) || dto.Price <= 0)
                throw new ArgumentException("Invalid product data.");

            try
            {
                var entity = new Product
                {
                    Name = dto.Name,
                    Price = dto.Price,
                    Stock = dto.Stock
                };
                _productRepo.Add(entity);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error adding product.");
                throw;
            }
        }

        /// <summary>
        /// Updates an existing product.
        /// </summary>
        /// <param name="id">The unique identifier of the product to update.</param>
        /// <param name="dto">The updated product data transfer object.</param>
        public void Update(int id, ProductDto dto)
        {
            if (id <= 0)
                throw new ArgumentException("Invalid product ID.");
            if (dto == null)
                throw new ArgumentNullException(nameof(dto));

            try
            {
                var entity = _productRepo.GetById(id);
                if (entity == null)
                    throw new InvalidOperationException("Product not found.");

                entity.Name = dto.Name;
                entity.Price = dto.Price;
                entity.Stock = dto.Stock;
                _productRepo.Save();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error updating product {ProductId}", id);
                throw;
            }
        }

        /// <summary>
        /// Deletes a product by its unique identifier.
        /// </summary>
        /// <param name="id">The unique identifier of the product to delete.</param>
        public void Delete(int id)
        {
            if (id <= 0)
                throw new ArgumentException("Invalid product ID.");

            try
            {
                _productRepo.Delete(id);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error deleting product {ProductId}", id);
                throw;
            }
        }
    }
}
