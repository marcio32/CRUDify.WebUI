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
    public class UserRepositoryTests
    {
        private readonly Mock<IUserRepository> _mockUserRepository;
        private readonly IUserRepository _userRepository;

        public UserRepositoryTests()
        {
            _mockUserRepository = new Mock<IUserRepository>();
            _userRepository = _mockUserRepository.Object;
        }

        [Fact]
        public async Task GetAllUsersAsync()
        {
            var users = new List<User>
            {
                new User { Id = "1", UserName = "admin", Email = "marcioabriola@gmail.com", NormalizedEmail = "marcioabriola@gmail.com", EmailConfirmed = true, FirstName = "Marcio", LastName = "Abriola", Age = 28},
                new User { Id = "2", UserName = "admin2", Email = "marcioabriola2@gmail.com", NormalizedEmail = "marcioabriola2@gmail.com", EmailConfirmed = true, FirstName = "Marcio", LastName = "Abriola", Age = 28},
            };

            _mockUserRepository.Setup(x => x.GetAllAsync()).ReturnsAsync(users);

            var result = await _userRepository.GetAllAsync();


            Assert.Equal(2, result.Count());
            Assert.Equal("admin", result.First().UserName);
        }

        [Fact]
        public async Task AddUserAsync()
        {
            var user = new User { Id = "1", UserName = "admin", Email = "marcioabriola@gmail.com", NormalizedEmail = "marcioabriola@gmail.com", EmailConfirmed = true, FirstName = "Marcio", LastName = "Abriola", Age = 28 };
            await _userRepository.AddAsync(user);
            _mockUserRepository.Verify(x => x.AddAsync(user), Times.Once);
        }

        [Fact]
        public async Task UpdateUserAsync()
        {
            var user = new User { Id = "1", UserName = "admin", Email = "marcioabriola@gmail.com", NormalizedEmail = "marcioabriola@gmail.com", EmailConfirmed = true, FirstName = "Marcio", LastName = "Abriola", Age = 28 };
            await _userRepository.UpdateAsync(user);
            _mockUserRepository.Verify(x => x.UpdateAsync(user), Times.Once);
        }


        [Fact]
        public async Task DeleteUserAsync()
        {
            var user = new User { Id = "1" };
            await _userRepository.DeleteAsync(user);
            _mockUserRepository.Verify(x => x.DeleteAsync(user), Times.Once);
        }
    }
}
