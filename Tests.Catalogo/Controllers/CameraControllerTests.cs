using Catalogo.Core.DTOs;
using Catalogo.Core.Entities;
using Catalogo.Core.Interfaces;
using CatalogoAPI.Controllers;
using Microsoft.AspNetCore.Mvc;
using Moq;

namespace Tests.Catalogo.Controllers
{
    public class CameraControllerTests
    {
        private readonly CameraController _controller;
        private readonly Mock<ICameraRepository> _mockCameraRepository;

        public CameraControllerTests()
        {
            _mockCameraRepository = new Mock<ICameraRepository>();
            _controller = new CameraController(_mockCameraRepository.Object);
        }

        [Fact]
        public async Task GetAllAsync_ReturnsOkResult_WithListOfCameras()
        {
            // Arrange
            var cameras = new List<Camera>
            {
                new Camera { Id = 1, Brand = "Canon", Model = "EOS R5", Price = 4000, Description = "Camera Full-Frame" },
                new Camera { Id = 2, Brand = "Nikon", Model = "Z7 II", Price = 3000, Description = "Mirrorless Camera" }
            };
            _mockCameraRepository.Setup(repo => repo.GetAllAsync()).ReturnsAsync(cameras);

            // Act
            var result = await _controller.GetAllAsync();

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var returnCameras = Assert.IsType<List<Camera>>(okResult.Value);
            Assert.Equal(2, returnCameras.Count);
        }

        [Fact]
        public async Task GetByIdAsync_ReturnsOkResult_WithCamera()
        {
            // Arrange
            var camera = new Camera { Id = 1, Brand = "Canon", Model = "EOS R5", Price = 4000, Description = "Camera Full-Frame" };
            _mockCameraRepository.Setup(repo => repo.GetByIdAsync(1)).ReturnsAsync(camera);

            // Act
            var result = await _controller.GetByIdAsync(1);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var returnCamera = Assert.IsType<Camera>(okResult.Value);
            Assert.Equal(camera.Id, returnCamera.Id);
        }

        [Fact]
        public async Task GetByIdAsync_ReturnsNotFoundResult_WhenCameraNotFound()
        {
            // Arrange
            _mockCameraRepository.Setup(repo => repo.GetByIdAsync(1)).ReturnsAsync((Camera)null);

            // Act
            var result = await _controller.GetByIdAsync(1);

            // Assert
            Assert.IsType<NotFoundResult>(result);
        }

        [Fact]
        public async Task AddAsync_ReturnsOkResult_WithCreatedCamera()
        {
            // Arrange
            var cameraDto = new CameraDto { Brand = "Canon", Model = "EOS R5", Price = 4000, Description = "Camera Full-Frame" };
            _mockCameraRepository.Setup(repo => repo.GetNextIdAsync()).ReturnsAsync(1);

            // Act
            var result = await _controller.AddAsync(cameraDto);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var createdCamera = Assert.IsType<Camera>(okResult.Value);
            Assert.Equal(1, createdCamera.Id);
        }

        [Fact]
        public async Task UpdateAsync_ReturnsNoContentResult_WhenCameraUpdated()
        {
            // Arrange
            var cameraDto = new CameraDto { Brand = "Canon", Model = "EOS R5", Price = 4000, Description = "Camera Full-Frame" };
            var camera = new Camera { Id = 1, Brand = "Canon", Model = "EOS R", Price = 3500, Description = "Mirrorless Camera" };
            _mockCameraRepository.Setup(repo => repo.GetByIdAsync(1)).ReturnsAsync(camera);

            // Act
            var result = await _controller.UpdateAsync(1, cameraDto);

            // Assert
            Assert.IsType<NoContentResult>(result);
        }

        [Fact]
        public async Task UpdateAsync_ReturnsNotFoundResult_WhenCameraNotFound()
        {
            // Arrange
            var cameraDto = new CameraDto { Brand = "Canon", Model = "EOS R5", Price = 4000, Description = "Camera Full-Frame" };
            _mockCameraRepository.Setup(repo => repo.GetByIdAsync(1)).ReturnsAsync((Camera)null);

            // Act
            var result = await _controller.UpdateAsync(1, cameraDto);

            // Assert
            Assert.IsType<NotFoundResult>(result);
        }

        [Fact]
        public async Task DeleteAsync_ReturnsNoContentResult_WhenCameraDeleted()
        {
            // Arrange
            var camera = new Camera { Id = 1, Brand = "Canon", Model = "EOS R5", Price = 4000, Description = "Camera Full-Frame" };
            _mockCameraRepository.Setup(repo => repo.GetByIdAsync(1)).ReturnsAsync(camera);

            // Act
            var result = await _controller.DeleteAsync(1);

            // Assert
            Assert.IsType<NoContentResult>(result);
        }

        [Fact]
        public async Task DeleteAsync_ReturnsNotFoundResult_WhenCameraNotFound()
        {
            // Arrange
            _mockCameraRepository.Setup(repo => repo.GetByIdAsync(1)).ReturnsAsync((Camera)null);

            // Act
            var result = await _controller.DeleteAsync(1);

            // Assert
            Assert.IsType<NotFoundResult>(result);
        }
    }
}