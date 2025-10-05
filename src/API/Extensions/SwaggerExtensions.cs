
namespace ECommerce.API.Extensions
{
    using Microsoft.OpenApi.Models;

    /// <summary>
    /// Provides extension methods for configuring Swagger with JWT authentication.
    /// </summary>
    public static class SwaggerExtensions
    {
        /// <summary>
        /// Adds Swagger generation and configures JWT Bearer authentication for API documentation.
        /// </summary>
        /// <param name="services">The service collection to add Swagger to.</param>
        public static void AddSwaggerWithJwt(this IServiceCollection services)
        {
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "ECommercePlatform API", Version = "v1" });

                var jwtScheme = new OpenApiSecurityScheme
                {
                    Name = "Authorization",
                    Type = SecuritySchemeType.Http,
                    Scheme = "bearer",
                    BearerFormat = "JWT",
                    In = ParameterLocation.Header,
                    Description = "JWT Authorization header using the Bearer scheme."
                };

                c.AddSecurityDefinition("Bearer", jwtScheme);
                c.AddSecurityRequirement(new OpenApiSecurityRequirement
            {
                { jwtScheme, Array.Empty<string>() }
            });
            });
        }
    }
}
