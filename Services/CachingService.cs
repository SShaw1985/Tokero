using Akavache;
using System.Reactive.Linq;
using Tokero.Interfaces;

namespace Tokero.Services
{
    public class CachingService : ICachingService
    {
        private readonly IBlobCache _cache;
        private readonly TimeSpan _defaultExpiration = TimeSpan.FromMinutes(15);

        public CachingService ()
        {
            _cache = BlobCache.LocalMachine;
        }

        public async Task<T?> GetAsync<T> (string key)
        {
            try
            {
                return await _cache.GetObject<T>(key);
            }
            catch(KeyNotFoundException)
            {
                return default(T);
            }
            catch(Exception)
            {
                return default(T);
            }
        }

        public async Task SetAsync<T> (string key, T value)
        {
            await SetAsync(key, value, _defaultExpiration);
        }

        public async Task SetAsync<T> (string key, T value, TimeSpan expiration)
        {
            try
            {
                await _cache.InsertObject(key, value, expiration);
            }
            catch(Exception)
            {
            }
        }

        public async Task RemoveAsync (string key)
        {
            try
            {
                await _cache.Invalidate(key);
            }
            catch(Exception)
            {
            }
        }

        public async Task ClearAllAsync ()
        {
            try
            {
                await _cache.InvalidateAll();
            }
            catch(Exception)
            {
            }
        }

        public async Task<bool> ExistsAsync (string key)
        {
            try
            {
                await _cache.GetObject<object>(key);
                return true;
            }
            catch(KeyNotFoundException)
            {
                return false;
            }
            catch(Exception)
            {
                return false;
            }
        }
    }
}