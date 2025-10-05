using ECommerce.Core.Entities;
using ECommerce.Data.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.Tests.MockData
{
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
