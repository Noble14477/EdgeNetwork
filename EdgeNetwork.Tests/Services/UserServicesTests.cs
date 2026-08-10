using System;
using System.Collections.Generic;
using System.Text;
using EdgeNetworkApplication.Dtos;
using EdgeNetworkApplication.Services;
using EdgeNetworkDomain.Entities;
using EdgeNetworkDomain.Interface;
using Moq;

namespace EdgeNetwork.Tests.Services
{
    public class UserServicesTests
    {
        [Fact]
        public async Task RegisterUser_ShouldSucceed_WhenDoesNotExist()
        {
            // Arrange
            var userRepository = new Mock<IUserRepository>();
            var unitOfWork = new Mock<IUnitOfWork>();

            var dto = new RegisterUserDto
            {
                FirstName = "Noble",
                LastName = "Chinonso",
                Email = "noble@test.com",
                PhoneNumber = "08012345678"
            };

            var userId = Guid.NewGuid();

            userRepository.Setup(x => x.GetByEmailAsync(dto.Email)).ReturnsAsync((AppUser?)null);

            var userService = new UserService(userRepository.Object, unitOfWork.Object);

            // Act
            var result = await userService.RegisterAsync(dto, userId);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(userId, result.Id);
            Assert.Equal(dto.Email, result.Email.Value);

            userRepository.Verify(
                x => x.AddAsync(It.IsAny<AppUser>()), Times.Once);

            unitOfWork.Verify(x => x.SaveChangesAsync(), Times.Once);

        }

    }
}
