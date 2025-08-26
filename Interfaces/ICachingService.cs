namespace Tokero.Interfaces
{
    public interface ICachingService
    {
        Task<T?> GetAsync<T> (string key);

        Task SetAsync<T> (string key, T value);

        Task SetAsync<T> (string key, T value, TimeSpan expiration);

        Task RemoveAsync (string key);

        Task ClearAllAsync ();

        Task<bool> ExistsAsync (string key);
    }
}