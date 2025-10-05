
namespace ECommerce.Core.Entities
{
    /// <summary>
    /// Represents a user entity in the e-commerce system.
    /// </summary>
    public class User
    {
        /// <summary>
        /// Gets or sets the unique identifier for the user.
        /// </summary>
        public string Id { get; set; } = Guid.NewGuid().ToString();

        /// <summary>
        /// Gets or sets the username of the user.
        /// </summary>
        public string Username { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the hashed password of the user.
        /// </summary>
        public string PasswordHash { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the role of the user (e.g., User, Admin).
        /// </summary>
        public string Role { get; set; } = "User";
    }
}
