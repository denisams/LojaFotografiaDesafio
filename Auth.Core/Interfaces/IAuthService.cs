using Auth.Core.DTOs;
using Auth.Core.Entities;

namespace Auth.Core.Interfaces
{
    public interface IAuthService
    {
        Task CreateUserAsync(User user);
        Task<User> ValidateUserAsync(LoginDto loginDto);
    }
}
