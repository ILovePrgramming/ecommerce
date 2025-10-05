namespace ECommerce.Tests.ServiceTests
{
    using Core.Interfaces;
    using Data.Context;
    using ECommerce.Core.DTOs;
    using ECommerce.Services.Services;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.Extensions.Configuration;
    using NUnit.Framework;

    [TestFixture]
    public class AuthServiceTests
    {
        private IAuthService _authService;
        private AppDbContext _context;
        private IJwtProvider _jwt;

        [SetUp]
        public void Setup()
        {
            var options = new DbContextOptionsBuilder<AppDbContext>()
                .UseInMemoryDatabase("TestDb")
                .Options;

            _context = new AppDbContext(options);
            _jwt = new JwtProvider(new ConfigurationBuilder().AddInMemoryCollection(new Dictionary<string, string>
        {
            { "Jwt:Key", "testkey1234567890" },
            { "Jwt:Issuer", "TestIssuer" },
            { "Jwt:Audience", "TestAudience" }
        }).Build());

            _authService = new AuthService(_context, _jwt);
        }

        [Test]
        public void Register_ShouldReturnToken()
        {
            var dto = new AuthDto { Username = "testuser", Password = "password" };
            var token = _authService.Register(dto);
            Assert.That(token, Is.Null);
        }

        [Test]
        public void Login_WithValidCredentials_ShouldReturnToken()
        {
            var dto = new AuthDto { Username = "testuser", Password = "password" };
            _authService.Register(dto);
            var token = _authService.Login(dto);
            Assert.That(token, Is.Not.Null);
        }
    }
}
