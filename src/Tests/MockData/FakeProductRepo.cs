namespace ECommerce.Tests.MockData
{
    using ECommerce.Core.Entities;
    using ECommerce.Data.Repositories;

    public class FakeProductRepo : ProductRepository
    {
        public FakeProductRepo() : base(null) { }

        public override IEnumerable<Product> GetAll()
        {
            return new List<Product> {
            new Product { Name = "Test1", Price = 10, Stock = 5 },
            new Product { Name = "Test2", Price = 20, Stock = 3 }
            };
        }
    }
}
