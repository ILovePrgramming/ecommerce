namespace ECommerce.Services.Services
{
    using Core.Entities;
    using Core.Interfaces;
    using Data.Context;
    using System.Collections.Generic;
    using System.Linq;

    /// <summary>
    /// Provides administrative operations for managing users in the e-commerce system.
    /// </summary>
    public class AdminService : IAdminService
    {
        private readonly AppDbContext _context;

        /// <summary>
        /// Initializes a new instance of the <see cref="AdminService"/> class with the specified database context.
        /// </summary>
        /// <param name="context">The database context to use for data access.</param>
        public AdminService(AppDbContext context) => _context = context;

        /// <summary>
        /// Retrieves all users in the system.
        /// </summary>
        /// <returns>An enumerable collection of <see cref="User"/> objects.</returns>
        public IEnumerable<User> GetAllUsers() => _context.Users.ToList();

        /// <summary>
        /// Promotes the specified user to an administrator role.
        /// </summary>
        /// <param name="userId">The unique identifier of the user to promote.</param>
        public void PromoteToAdmin(string userId)
        {
            var user = _context.Users.Find(userId);
            if (user != null)
            {
                user.Role = "Admin";
                _context.SaveChanges();
            }
        }
    }
}
