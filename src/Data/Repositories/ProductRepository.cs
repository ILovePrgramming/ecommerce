namespace ECommerce.Data.Repositories
{
    using Context;
    using Core.Entities;
    using System.Collections.Generic;
    using System.Linq;

    /// <summary>
    /// Provides CRUD operations for <see cref="Product"/> entities in the e-commerce database.
    /// </summary>
    public class ProductRepository
    {
        private readonly AppDbContext _context;

        /// <summary>
        /// Initializes a new instance of the <see cref="ProductRepository"/> class.
        /// </summary>
        /// <param name="context">The database context to use for data operations.</param>
        public ProductRepository(AppDbContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Retrieves all products from the database.
        /// </summary>
        /// <returns>An enumerable collection of <see cref="Product"/> objects.</returns>
        public virtual IEnumerable<Product> GetAll() => _context.Products.ToList();

        /// <summary>
        /// Retrieves a product by its unique identifier.
        /// </summary>
        /// <param name="id">The unique identifier of the product.</param>
        /// <returns>The <see cref="Product"/> if found; otherwise, <c>null</c>.</returns>
        public Product? GetById(int id) => _context.Products.Find(id);

        /// <summary>
        /// Adds a new product to the database context.
        /// </summary>
        /// <param name="product">The <see cref="Product"/> to add.</param>
        public void Add(Product product) => _context.Products.Add(product);

        /// <summary>
        /// Saves all changes made in the context to the database.
        /// </summary>
        public void Save() => _context.SaveChanges();
    }
}
