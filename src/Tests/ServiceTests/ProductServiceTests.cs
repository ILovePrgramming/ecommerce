namespace ECommerce.Tests.ServiceTests
{
    using ECommerce.Services.Services;
    using ECommerce.Tests.MockData;
    using NUnit.Framework;
    using System;
    using System.Linq;

    public class ProductServiceTests
    {
        [Test]
        public void GetAll_ReturnsCorrectCount()
        {
            var repo = new FakeProductRepo();
            var service = new ProductService(repo);
            var result = service.GetAll();
            Assert.That(result.Count(), Is.EqualTo(2));
        }
    }
}
