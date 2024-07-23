using Auth.Core.DTOs;
using Auth.Core.Entities;
using Auth.Core.Interfaces;
using MongoDB.Driver;

namespace Auth.Infrastructure.Services
{
    public class AuthService : IAuthService
    {
        private readonly IMongoCollection<User> _users;

        public AuthService(IMongoDatabase database)
        {
            _users = database.GetCollection<User>("Users");
        }

        public async Task<User> ValidateUserAsync(LoginDto loginDto)
        {
            // Busca no banco de dados
            var user = await _users.Find(u => u.Username == loginDto.Username).FirstOrDefaultAsync();

            if (user == null || !BCrypt.Net.BCrypt.Verify(loginDto.Password, user.PasswordHash))
            {
                return null;
            }

            return user;
        }

        public async Task CreateUserAsync(User user)
        {
            // Verifica se o usuário já existe
            var existingUser = await _users.Find(u => u.Username == user.Username).FirstOrDefaultAsync();
            if (existingUser != null)
            {
                throw new Exception("Usuário já existe");
            }

            user.PasswordHash = BCrypt.Net.BCrypt.HashPassword(user.PasswordHash);
            await _users.InsertOneAsync(user);
        }
    }
}
