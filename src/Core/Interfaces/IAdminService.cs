
namespace ECommerce.Core.Interfaces
{
    using Entities;

    /// <summary>
    /// Provides administrative operations for managing users in the e-commerce system.
    /// </summary>
    public interface IAdminService
    {
        /// <summary>
        /// Retrieves all users in the system.
        /// </summary>
        /// <returns>An enumerable collection of <see cref="User"/> objects.</returns>
        IEnumerable<User> GetAllUsers();

        /// <summary>
        /// Promotes the specified user to an administrator role.
        /// </summary>
        /// <param name="userId">The unique identifier of the user to promote.</param>
        void PromoteToAdmin(string userId);
    }
}
