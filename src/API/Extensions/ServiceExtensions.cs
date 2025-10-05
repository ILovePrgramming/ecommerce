namespace ECommerce.API.Extensions
{
    using Core.Interfaces;
    using Data.Repositories;
    using Microsoft.AspNetCore.Authentication.JwtBearer;
    using Microsoft.IdentityModel.Tokens;
    using Services.Services;
    using System.Text;

    /// <summary>
    /// Provides extension methods for registering application services and JWT authentication.
    /// </summary>
    public static class ServiceExtensions
    {
        /// <summary>
        /// Registers application services and repositories with the dependency injection container.
        /// </summary>
        /// <param name="services">The service collection to add services to.</param>
        public static void AddAppServices(this IServiceCollection services)
        {
            services.AddScoped<IProductService, ProductService>();
            services.AddScoped<ICartService, CartService>();
            services.AddScoped<IAuthService, AuthService>();
            services.AddScoped<IAdminService, AdminService>();
            services.AddScoped<IJwtProvider, JwtProvider>();
            services.AddScoped<ProductRepository>();
            services.AddScoped<CartRepository>();
        }

        /// <summary>
        /// Configures JWT authentication using settings from the configuration.
        /// </summary>
        /// <param name="services">The service collection to add authentication to.</param>
        /// <param name="config">The application configuration containing JWT settings.</param>
        public static void AddJwtAuthentication(this IServiceCollection services, IConfiguration config)
        {
            var key = Encoding.UTF8.GetBytes(config["Jwt:Key"]);

            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(options =>
                {
                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuer = true,
                        ValidateAudience = true,
                        ValidateLifetime = true,
                        ValidateIssuerSigningKey = true,
                        ValidIssuer = config["Jwt:Issuer"],
                        ValidAudience = config["Jwt:Audience"],
                        IssuerSigningKey = new SymmetricSecurityKey(key)
                    };
                });
        }
    }

}
