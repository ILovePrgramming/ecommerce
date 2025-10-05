namespace ECommerce.Services.Services
{
    using Core.DTOs;
    using Core.Interfaces;
    using Data.Context;
    using ECommerce.Core.Entities;
    using System;
    using System.Linq;
    using BCrypt = BCrypt.Net.BCrypt;

    /// <summary>
    /// Provides authentication services for user registration and login.
    /// </summary>
    public class AuthService : IAuthService
    {
        private readonly AppDbContext _context;
        private readonly IJwtProvider _jwt;

        /// <summary>
        /// Initializes a new instance of the <see cref="AuthService"/> class.
        /// </summary>
        /// <param name="context">The database context for accessing user data.</param>
        /// <param name="jwt">The JWT provider for token generation.</param>
        public AuthService(AppDbContext context, IJwtProvider jwt)
        {
            _context = context;
            _jwt = jwt;
        }

        /// <summary>
        /// Registers a new user with the provided authentication data.
        /// </summary>
        /// <param name="dto">The authentication data transfer object containing user credentials.</param>
        /// <returns>A JWT token string representing the registered user.</returns>
        public string Register(AuthDto dto)
        {
            var user = new User
            {
                Username = dto.Username,
                PasswordHash = BCrypt.HashPassword(dto.Password),
                Role = "User" // Set default role if needed
            };
            _context.Users.Add(user);
            _context.SaveChanges();
            return _jwt.GenerateToken(user);
        }

        /// <summary>
        /// Authenticates a user with the provided credentials.
        /// </summary>
        /// <param name="dto">The authentication data transfer object containing user credentials.</param>
        /// <returns>A JWT token string representing the authenticated user.</returns>
        /// <exception cref="UnauthorizedAccessException">Thrown when credentials are invalid.</exception>
        public string Login(AuthDto dto)
        {
            var user = _context.Users.FirstOrDefault(u => u.Username == dto.Username);
            if (user == null || !BCrypt.Verify(dto.Password, user.PasswordHash))
                throw new UnauthorizedAccessException("Invalid credentials");

            return _jwt.GenerateToken(user);
        }
    }
}
