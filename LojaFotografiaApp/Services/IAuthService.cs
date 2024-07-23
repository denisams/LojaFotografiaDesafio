using LojaFotografiaApp.DTOs;
using System.Threading.Tasks;

namespace LojaFotografiaApp.Services
{
    public interface IAuthService
    {
        Task<LoginResult> Login(LoginDto loginDto);
        bool IsLoggedIn();
        void Logout();
        string GetUsername();
        string GetToken();
    }

    public class LoginResult
    {
        public bool Success { get; set; }
        public string ErrorMessage { get; set; }
    }

    public class TokenResponse
    {
        public string Token { get; set; }
    }
}
