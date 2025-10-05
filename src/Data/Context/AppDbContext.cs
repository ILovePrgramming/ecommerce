using Microsoft.EntityFrameworkCore;
using ECommerce.Core.Entities;

namespace ECommerce.Data.Context
{
    /// <summary>
    /// Represents the Entity Framework database context for the e-commerce application.
    /// </summary>
    public class AppDbContext : DbContext
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="AppDbContext"/> class using the specified options.
        /// </summary>
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        /// <summary>
        /// Gets the <see cref="DbSet{User}"/> representing the users in the database.
        /// </summary>
        public DbSet<User> Users { get; set; }

        /// <summary>
        /// Gets the <see cref="DbSet{Product}"/> representing the products in the database.
        /// </summary>
        public DbSet<Product> Products { get; set; }

        /// <summary>
        /// Gets the <see cref="DbSet{CartItem}"/> representing the cart items in the database.
        /// </summary>
        public DbSet<CartItem> CartItems { get; set; }
    }
}
