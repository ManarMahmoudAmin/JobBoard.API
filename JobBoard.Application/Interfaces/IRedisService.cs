using System;
using System.Collections.Generic;
using System.Text;

namespace JobBoard.Application.Interfaces
{
    public interface IRedisService
    {
         
        // Stores a value in Redis using a key, with the option to specify an expiry period.
        Task SetAsync<T>(string key, T value, TimeSpan? expiry = null);

        //Returns the value from Redis using the key.
        Task<T?> GetAsync<T>(string key);

        //removes the value from Redis using the key.
        Task<bool> RemoveAsync(string key);

        public Task DeleteByPrefixAsync(string prefix);
        // List operations
        Task AddToListAsync<T>(string key, T value); // Push to list
        Task<List<T>> GetListAsync<T>(string key);   // Get all items from list
        Task SetExpiryAsync(string key, TimeSpan expiry); // Set expiration for list
    }
}
