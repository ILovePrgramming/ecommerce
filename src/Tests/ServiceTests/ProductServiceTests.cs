namespace ECommerce.Tests.ServiceTests
{
    using ECommerce.Core.DTOs;
    using ECommerce.Data.Repositories;
    using ECommerce.Services.Services;
    using ECommerce.Tests.MockData;
    using Moq;
    using NUnit.Framework;

    public class ProductServiceTests
    {
        private Mock<CartRepository> _repoMock;
        private FakeProductRepo _repo;
        private ProductService _service;

        [SetUp]
        public void Setup()
        {
            _repo = new FakeProductRepo();
            _service = new ProductService(_repo, null);
        }

        [Test]
        public void GetAll_ReturnsCorrectCount()
        {
            var result = _service.GetAll();
            Assert.That(result.Count(), Is.EqualTo(2));
        }

        [Test]
        public void GetById_ReturnsCorrectProduct()
        {
            var product = _service.GetById(1);
            Assert.That(product, Is.Not.Null);
        }

        [Test]
        public void GetById_ThrowsForNonExistentProduct()
        {
            Assert.Throws<KeyNotFoundException>(() => _service.GetById(999));
        }

        [Test]
        public void Add_IncreasesCount()
        {
            var newProduct = new ProductDto { Name = "New Product", Price = 10.0m, Stock = 5 };
            _service.Add(newProduct);
            var result = _service.GetAll();
            Assert.That(result.Count(), Is.EqualTo(3));
            Assert.That(result.Any(p => p.Name == "New Product"), Is.True);
        }

        [Test]
        public void Update_ChangesProductData()
        {
            var updatedProduct = new ProductDto { Name = "Updated", Price = 20.0m, Stock = 10 };
            _service.Update(1, updatedProduct);
            var product = _service.GetById(1);
            Assert.That(product.Name, Is.EqualTo("Updated"));
            Assert.That(product.Price, Is.EqualTo(20.0m));
            Assert.That(product.Stock, Is.EqualTo(10));
        }

        [Test]
        public void Delete_RemovesProduct()
        {
            _service.Delete(1);
            var result = _service.GetAll();
            Assert.That(result.Count(), Is.EqualTo(1));
        }
    }
}
