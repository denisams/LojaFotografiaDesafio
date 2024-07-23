using LojaFotografiaApp.DTOs;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System;

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

    public class AuthService : IAuthService
    {
        private readonly HttpClient _httpClient;
        private string _username;

        public AuthService()
        {
            var handler = new HttpClientHandler
            {
                ServerCertificateCustomValidationCallback = (message, cert, chain, errors) => true
            };
            _httpClient = new HttpClient(handler);
        }

        public async Task<LoginResult> Login(LoginDto loginDto)
        {
            try
            {
                var response = await _httpClient.PostAsJsonAsync("https://localhost:7261/api/auth/login", loginDto);
                if (response.IsSuccessStatusCode)
                {
                    var tokenResponse = await response.Content.ReadFromJsonAsync<TokenResponse>();
                    if (tokenResponse != null && CanReadToken(tokenResponse.Token))
                    {
                        StoreToken(tokenResponse.Token);
                        DecodeToken(tokenResponse.Token);
                        return new LoginResult { Success = true };
                    }
                    else
                    {
                        return new LoginResult { Success = false, ErrorMessage = "Token inválido retornado pela API." };
                    }
                }
                else
                {
                    var errorResponse = await response.Content.ReadAsStringAsync();
                    return new LoginResult { Success = false, ErrorMessage = errorResponse };
                }
            }
            catch (HttpRequestException ex)
            {
                return new LoginResult { Success = false, ErrorMessage = $"Erro na requisição: {ex.Message}. Inner Exception: {ex.InnerException?.Message}" };
            }
            catch (Exception ex)
            {
                return new LoginResult { Success = false, ErrorMessage = $"Erro inesperado: {ex.Message}" };
            }
        }

        private bool CanReadToken(string token)
        {
            var handler = new JwtSecurityTokenHandler();
            return handler.CanReadToken(token);
        }

        private void DecodeToken(string token)
        {
            var handler = new JwtSecurityTokenHandler();
            var jsonToken = handler.ReadToken(token) as JwtSecurityToken;

            _username = jsonToken?.Claims.FirstOrDefault(claim => claim.Type == "unique_name")?.Value;
        }

        public bool IsLoggedIn()
        {
            return !string.IsNullOrEmpty(GetToken());
        }

        public void Logout()
        {
            var vault = new Windows.Security.Credentials.PasswordVault();
            try
            {
                var credential = vault.Retrieve("LojaFotografiaApp", "AuthToken");
                vault.Remove(credential);
            }
            catch (Exception) { }
            _username = null;
        }

        public string GetUsername()
        {
            return _username;
        }

        public string GetToken()
        {
            var vault = new Windows.Security.Credentials.PasswordVault();
            try
            {
                var credential = vault.Retrieve("LojaFotografiaApp", "AuthToken");
                return credential?.Password;
            }
            catch (Exception)
            {
                return null;
            }
        }

        private void StoreToken(string token)
        {
            var vault = new Windows.Security.Credentials.PasswordVault();
            var credential = new Windows.Security.Credentials.PasswordCredential("LojaFotografiaApp", "AuthToken", token);
            vault.Add(credential);
        }
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
