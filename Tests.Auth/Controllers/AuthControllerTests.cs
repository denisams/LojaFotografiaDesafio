using Auth.Core.DTOs;
using Auth.Core.Entities;
using Auth.Core.Interfaces;
using AuthAPI.Controllers;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Moq;

namespace Tests.Auth.Controllers
{
    public class AuthControllerTests
    {
        private readonly AuthController _controller;
        private readonly Mock<IAuthService> _mockAuthService;
        private readonly Mock<IConfiguration> _mockConfiguration;

        public AuthControllerTests()
        {
            _mockAuthService = new Mock<IAuthService>();
            _mockConfiguration = new Mock<IConfiguration>();

            // Mock configuration settings for JWT
            _mockConfiguration.SetupGet(c => c["Jwt:Key"]).Returns("dSk6vQJ3yB9nZ5uL1wF7jK8mS2xE4rT8pW6yC3uH2vQ7dR1bN4kA0qX9tF6wR8e");
            _mockConfiguration.SetupGet(c => c["Jwt:Issuer"]).Returns("PhotoStoreAPI");
            _mockConfiguration.SetupGet(c => c["Jwt:Audience"]).Returns("PhotoStoreClient");

            _controller = new AuthController(_mockAuthService.Object, _mockConfiguration.Object);
        }

        [Fact]
        public async Task Login_InvalidModelState_ReturnsBadRequest()
        {
            // Arrange
            _controller.ModelState.AddModelError("Username", "Required");

            // Act
            var result = await _controller.Login(new LoginDto());

            // Assert
            var badRequestResult = Assert.IsType<BadRequestObjectResult>(result.Result);
            Assert.IsType<SerializableError>(badRequestResult.Value);
        }

        [Fact]
        public async Task Login_InvalidCredentials_ReturnsUnauthorized()
        {
            // Arrange
            var loginDto = new LoginDto { Username = "user", Password = "pass" };
            _mockAuthService.Setup(s => s.ValidateUserAsync(loginDto)).ReturnsAsync((User)null);

            // Act
            var result = await _controller.Login(loginDto);

            // Assert
            Assert.IsType<UnauthorizedResult>(result.Result);
        }

        [Fact]
        public async Task Login_ValidCredentials_ReturnsToken()
        {
            // Arrange
            var loginDto = new LoginDto { Username = "teste", Password = "password" };
            var user = new User { Id = Guid.NewGuid(), Username = "teste", Email = "teste@teste.com" };
            _mockAuthService.Setup(s => s.ValidateUserAsync(loginDto)).ReturnsAsync(user);

            // Act
            var result = await _controller.Login(loginDto);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            var tokenDto = Assert.IsType<TokenDto>(okResult.Value);
            Assert.NotNull(tokenDto.Token);
        }
    }
}