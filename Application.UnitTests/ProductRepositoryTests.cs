using Application.Interfaces;
using Domain.Entities;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.UnitTests
{
    public class ProductRepositoryTests
    {
        private readonly Mock<IProductRepository> _mockProductRepository;
        private readonly IProductRepository _productRepository;

        public ProductRepositoryTests()
        {
            _mockProductRepository = new Mock<IProductRepository>();
            _productRepository = _mockProductRepository.Object;
        }

        [Fact]
        public async Task GetAllProductsAsync()
        {
            var products = new List<Product>
            {
                new Product { Id = 1, Name = "Product 1", Price = 10, Stock = 10, Active = true },
                new Product { Id = 2, Name = "Product 2", Price = 20, Stock = 20, Active = true }
            };

            _mockProductRepository.Setup(x => x.GetAllAsync()).ReturnsAsync(products);

            var result = await _productRepository.GetAllAsync();


            Assert.Equal(2, result.Count());
            Assert.Equal("Product 1", result.First().Name);
        }

        [Fact]
        public async Task AddProductAsync()
        {
            var product = new Product { Id = 1, Name = "Product 1", Price = 10, Stock = 10, Active = true };
            await _productRepository.AddAsync(product);
            _mockProductRepository.Verify(x => x.AddAsync(product), Times.Once);
        }

        [Fact]
        public async Task UpdateProductAsync()
        {
            var product = new Product { Id = 1, Name = "Product 1", Price = 10, Stock = 10, Active = true };
            await _productRepository.UpdateAsync(product);
            _mockProductRepository.Verify(x => x.UpdateAsync(product), Times.Once);
        }


        [Fact]
        public async Task DeleteProductAsync()
        {
            var product = new Product { Id = 1 };
            await _productRepository.DeleteAsync(product);
            _mockProductRepository.Verify(x => x.DeleteAsync(product), Times.Once);
        }
    }
}
