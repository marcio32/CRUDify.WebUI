
using Domain.Entities;

namespace Application.UnitTests
{
    public class ProductTests
    {

        [Fact]
        public async Task Product_Stock_LowAsync()
        {
            var product = new Product { Id = 1, Name = "Product 1", Price = 10, Stock = 9, Active = true };

            bool isSotckLow = product.Stock < 10;

            Assert.True(isSotckLow, "El stock debe considerarse bajo por debajo de 10");
        }

        [Fact]
        public async Task Product_Stock_Async()
        {
            var product = new Product { Id = 1, Name = "Product 1", Price = 10, Stock = 12, Active = true };

            bool isSotckLow = product.Stock < 10;

            Assert.False(isSotckLow, "El stock no debe considerarse bajo cuando es 10 o mas");
        }


    }
}
