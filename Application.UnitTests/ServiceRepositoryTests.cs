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
    public class ServiceRepositoryTests
    {
        private readonly Mock<IServiceRepository> _mockServiceRepository;
        private readonly IServiceRepository _serviceRepository;

        public ServiceRepositoryTests()
        {
            _mockServiceRepository = new Mock<IServiceRepository>();
            _serviceRepository = _mockServiceRepository.Object;
        }

        [Fact]
        public async Task GetAllServicesAsync()
        {
            var services = new List<Service>
            {
                new Service { Id = 1, Nombre = "Service 1", Activo = true },
                new Service { Id = 2, Nombre = "Service 2", Activo = true }
            };

            _mockServiceRepository.Setup(x => x.GetAllAsync()).ReturnsAsync(services);

            var result = await _serviceRepository.GetAllAsync();


            Assert.Equal(2, result.Count());
            Assert.Equal("Service 1", result.First().Nombre);
        }

        [Fact]
        public async Task AddServiceAsync()
        {
            var service = new Service { Id = 1, Nombre = "Service 1", Activo = true };
            await _serviceRepository.AddAsync(service);
            _mockServiceRepository.Verify(x => x.AddAsync(service), Times.Once);
        }

        [Fact]
        public async Task UpdateServiceAsync()
        {
            var service = new Service { Id = 1, Nombre = "Service 1", Activo = true };
            await _serviceRepository.UpdateAsync(service);
            _mockServiceRepository.Verify(x => x.UpdateAsync(service), Times.Once);
        }


        [Fact]
        public async Task DeleteServiceAsync()
        {
            var service = new Service { Id = 1 };
            await _serviceRepository.DeleteAsync(service);
            _mockServiceRepository.Verify(x => x.DeleteAsync(service), Times.Once);
        }
    }
}
