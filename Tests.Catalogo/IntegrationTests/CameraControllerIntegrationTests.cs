using Catalogo.Core.DTOs;
using Catalogo.Core.Entities;
using Newtonsoft.Json;
using System.Net.Http.Headers;
using System.Text;

namespace Tests.Catalogo.IntegrationTests
{
    public class CameraControllerIntegrationTests : IClassFixture<CustomWebApplicationFactory>
    {
        private readonly HttpClient _client;
        private readonly CustomWebApplicationFactory _factory;
     

        public CameraControllerIntegrationTests(CustomWebApplicationFactory factory)
        {
            _factory = factory;
            _client = factory.CreateClient();

        }

        [Fact]
        public async Task GetAllAsync_ReturnsOkResult_WithListOfCameras()
        {
            // Act
            var response = await _client.GetAsync("/api/camera");

            // Assert
            response.EnsureSuccessStatusCode();
            var responseString = await response.Content.ReadAsStringAsync();
            var cameras = JsonConvert.DeserializeObject<List<Camera>>(responseString);
            Assert.NotNull(cameras);
        }

        [Fact]
        public async Task GetByIdAsync_ReturnsOkResult_WithCamera()
        {
            // Arrange
            var cameraId = await _factory.GetLastInsertedIdAsync();

            // Act
            var response = await _client.GetAsync($"/api/camera/{cameraId}");

            // Assert
            response.EnsureSuccessStatusCode();
            var responseString = await response.Content.ReadAsStringAsync();
            var camera = JsonConvert.DeserializeObject<Camera>(responseString);
            Assert.NotNull(camera);
        }
        

        [Fact]
        public async Task AddAsync_ReturnsOkResult_WithCreatedCamera()
        {
            // Arrange
            var cameraDto = new CameraDto { Marca = "Canon", Modelo = "EOS R5", Preco = 4000, Descricao = "Camera Full-Frame" };
            var content = new StringContent(JsonConvert.SerializeObject(cameraDto), Encoding.UTF8, "application/json");

            var token = _factory.GenerateJwtToken();

            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);


            // Act
            var response = await _client.PostAsync("/api/camera", content);

            // Assert
            response.EnsureSuccessStatusCode();
            var responseString = await response.Content.ReadAsStringAsync();
            var createdCamera = JsonConvert.DeserializeObject<Camera>(responseString);
            Assert.NotNull(createdCamera);
            Assert.Equal("Canon", createdCamera.Marca);
        }

        [Fact]
        public async Task UpdateAsync_ReturnsNoContentResult()
        {
            // Arrange
            var cameraDto = new CameraDto { Marca = "Canon", Modelo = "EOS R5", Preco = 4000, Descricao = "Camera Full-Frame" };
            var content = new StringContent(JsonConvert.SerializeObject(cameraDto), Encoding.UTF8, "application/json");
            var cameraId = await _factory.GetLastInsertedIdAsync();
            var token = _factory.GenerateJwtToken();

            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            // Act
            var response = await _client.PutAsync($"/api/camera/{cameraId}", content);

            // Assert
            Assert.Equal(System.Net.HttpStatusCode.NoContent, response.StatusCode);
        }

        [Fact]
        public async Task DeleteAsync_ReturnsNoContentResult()
        {
            // Arrange
            var cameraId = await _factory.GetLastInsertedIdAsync();
            var token = _factory.GenerateJwtToken();

            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            // Act
            var response = await _client.DeleteAsync($"/api/camera/{cameraId}");

            // Assert
            Assert.Equal(System.Net.HttpStatusCode.NoContent, response.StatusCode);
        }
    }
}