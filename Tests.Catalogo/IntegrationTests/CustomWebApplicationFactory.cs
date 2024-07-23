using Catalogo.Core.Entities;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Tokens;
using MongoDB.Driver;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Tests.Catalogo.IntegrationTests
{
    public class CustomWebApplicationFactory : WebApplicationFactory<Program>
    {
        protected override void ConfigureWebHost(IWebHostBuilder builder)
        {
            builder.ConfigureServices(services =>
            {
                // Configurações adicionais de serviço para testes, se necessário
            });
        }

        protected override IHost CreateHost(IHostBuilder builder)
        {
            builder.UseEnvironment("Testing");
            return base.CreateHost(builder);
        }

        public string GenerateJwtToken()
        {
            var key = Encoding.UTF8.GetBytes("dSk6vQJ3yB9nZ5uL1wF7jK8mS2xE4rT8pW6yC3uH2vQ7dR1bN4kA0qX9tF6wR8e");
            var claims = new[]
            {
                new Claim(JwtRegisteredClaimNames.Sub, "teste"),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new Claim(ClaimTypes.Name, "teste")
            };

            var creds = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256);
            var token = new JwtSecurityToken(
                issuer: "PhotoStoreAPI",
                audience: "PhotoStoreClient",
                claims: claims,
                expires: DateTime.Now.AddMinutes(30),
                signingCredentials: creds);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        public async Task<int> GetLastInsertedIdAsync()
        {
            var client = new MongoClient("mongodb://root:example@localhost:27017");
            var database = client.GetDatabase("LojaFotografia");
            var collection = database.GetCollection<Camera>("Cameras");

            var lastCamera = await collection.Find(FilterDefinition<Camera>.Empty)
                                             .SortByDescending(c => c.Id)
                                             .FirstOrDefaultAsync();

            return lastCamera?.Id ?? 0;
        }
    }
}