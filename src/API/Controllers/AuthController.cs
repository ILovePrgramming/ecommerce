
namespace ECommerce.API.Controllers
{
    using Core.DTOs;
    using Core.Interfaces;
    using Microsoft.AspNetCore.Mvc;

    /// <summary>
    /// Controller for authentication operations such as registration and login.
    /// </summary>
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _service;

        /// <summary>
        /// Initializes a new instance of the <see cref="AuthController"/> class.
        /// </summary>
        /// <param name="service">The authentication service.</param>
        public AuthController(IAuthService service) => _service = service;

        /// <summary>
        /// Registers a new user.
        /// </summary>
        /// <param name="dto">The registration data transfer object.</param>
        /// <returns>The result of the registration process.</returns>
        [HttpPost("register")]
        public IActionResult Register(AuthDto dto) => Ok(_service.Register(dto));

        /// <summary>
        /// Authenticates a user and returns a token if successful.
        /// </summary>
        /// <param name="dto">The login data transfer object.</param>
        /// <returns>The result of the login process.</returns>
        [HttpPost("login")]
        public IActionResult Login(AuthDto dto) => Ok(_service.Login(dto));
    }

}
