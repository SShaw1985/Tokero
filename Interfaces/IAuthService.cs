namespace Tokero.Interfaces
{
    public interface IAuthService
    {
        bool IsAuthenticated { get; }

        Task<bool> SignInAsync (string username, string password);
    }
}