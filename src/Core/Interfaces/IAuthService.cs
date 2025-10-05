
namespace ECommerce.Core.Interfaces
{
    using DTOs;

    /// <summary>
    /// Provides authentication services for user registration and login.
    /// </summary>
    public interface IAuthService
    {
        /// <summary>
        /// Registers a new user with the provided authentication data.
        /// </summary>
        /// <param name="dto">The authentication data transfer object containing user credentials.</param>
        /// <returns>A string representing the result of the registration process.</returns>
        string Register(AuthDto dto);

        /// <summary>
        /// Authenticates a user with the provided credentials.
        /// </summary>
        /// <param name="dto">The authentication data transfer object containing user credentials.</param>
        /// <returns>A string representing the result of the login process.</returns>
        string Login(AuthDto dto);
    }
}
