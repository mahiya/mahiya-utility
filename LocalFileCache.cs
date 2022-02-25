using Newtonsoft.Json;
using System;
using System.IO;
using System.Threading.Tasks;

namespace Mahiya.Utility
{
    public class LocalFileCache
    {
        private string _cacheDirPath;

        public LocalFileCache(string cacheDirPath = "cache")
        {
            _cacheDirPath = cacheDirPath;
            if (!Directory.Exists(_cacheDirPath))
                Directory.CreateDirectory(_cacheDirPath);
        }

        public async Task<T> GetAsync<T>(string key)
        {
            var json = await GetAsync(key);
            return JsonConvert.DeserializeObject<T>(json);
        }

        public async Task SetAsync<T>(T obj, string key)
        {
            var json = JsonConvert.SerializeObject(obj);
            await SetAsync(json, key);
        }

        public async Task<string> GetAsync(string key)
        {
            var filePath = GetCacheFilePath(key);
            return await File.ReadAllTextAsync(filePath);
        }

        public async Task SetAsync(string text, string key)
        {
            var filePath = GetCacheFilePath(key);
            await File.WriteAllTextAsync(filePath, text);
        }

        public async Task<T> GetAsync<T>(string key, Func<Task<T>> func)
        {
            if (await ExistsAsync(key))
                return await GetAsync<T>(key);
            var value = await func();
            await SetAsync(value, key);
            return value;
        }

        public async Task<T> GetAsync<T>(string key, Func<T> func)
        {
            if (await ExistsAsync(key))
                return await GetAsync<T>(key);
            var value = func();
            await SetAsync(value, key);
            return value;
        }

        public async Task<string> GetAsync(string key, Func<Task<string>> func)
        {
            if (await ExistsAsync(key))
                return await GetAsync(key);
            var value = await func();
            await SetAsync(value, key);
            return value;
        }

        public async Task<string> GetAsync(string key, Func<string> func)
        {
            if (await ExistsAsync(key))
                return await GetAsync(key);
            var value = func();
            await SetAsync(value, key);
            return value;
        }

        public Task<bool> ExistsAsync(string key)
        {
            var filePath = GetCacheFilePath(key);
            return Task.FromResult(File.Exists(filePath));
        }

        public Task DeleteAsync(string key)
        {
            var filePath = GetCacheFilePath(key);
            if (File.Exists(filePath))
                File.Delete(filePath);
            return Task.CompletedTask;
        }

        public void RegenerateCacheDir()
        {
            if (Directory.Exists(_cacheDirPath))
                Directory.Delete(_cacheDirPath, true);
            Directory.CreateDirectory(_cacheDirPath);
        }

        string GetCacheFilePath(string key)
        {
            return Path.Combine(_cacheDirPath, key); 
        }
    }
}
