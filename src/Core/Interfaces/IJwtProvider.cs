using ECommerce.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.Core.Interfaces
{
    /// <summary>
    /// Provides functionality for generating JWT tokens for users.
    /// </summary>
    public interface IJwtProvider
    {
        /// <summary>
        /// Generates a JWT token for the specified user.
        /// </summary>
        /// <param name="user">The user for whom to generate the token.</param>
        /// <returns>A JWT token string representing the user.</returns>
        string GenerateToken(User user);
    }
}
