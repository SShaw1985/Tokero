using Tokero.Interfaces;

namespace Tokero.Services
{
    public class AuthService : IAuthService
    {
        public bool IsAuthenticated { get; set; } = false;

        public async Task<bool> SignInAsync (string username, string password)
        {
            if(!string.IsNullOrWhiteSpace(username) && !string.IsNullOrWhiteSpace(password))
            {
                IsAuthenticated = true;
                return true;
            }
            return false;
        }
    }
}