
namespace ECommerce.API.Controllers
{
    using Core.Interfaces;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;

    /// <summary>
    /// Controller for admin-specific operations.
    /// Only accessible by users with the Admin role.
    /// </summary>
    [ApiController]
    [Route("api/[controller]")]
    [Authorize(Roles = "Admin")]
    public class AdminController : ControllerBase
    {
        private readonly IAdminService _service;

        /// <summary>
        /// Initializes a new instance of the <see cref="AdminController"/> class.
        /// </summary>
        /// <param name="service">The admin service.</param>
        public AdminController(IAdminService service) => _service = service;

        /// <summary>
        /// Gets all users in the system.
        /// </summary>
        /// <returns>A list of users.</returns>
        [HttpGet("users")]
        public IActionResult GetUsers() => Ok(_service.GetAllUsers());

        /// <summary>
        /// Promotes a user to admin.
        /// </summary>
        /// <param name="userId">The ID of the user to promote.</param>
        /// <returns>Status of the operation.</returns>
        [HttpPost("promote/{userId}")]
        public IActionResult Promote(string userId)
        {
            _service.PromoteToAdmin(userId);
            return Ok();
        }
    }
}

