using System;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace Mahiya.Utility
{
    public class HttpClientWithCache
    {
        static LocalFileCache _cache = new LocalFileCache("htmlcache");

        public static async Task<string> GetStringAsync(string url)
        {
            return await GetStringAsync(url, Encoding.UTF8);
        }

        public static async Task<string> GetStringAsync(string url, Encoding encoding)
        {
            var cacheKey = GetHashString<SHA256CryptoServiceProvider>(url);
            return await _cache.GetAsync(cacheKey, async () =>
            {
                try
                {
                    Console.WriteLine($"Get HTML from {url}");
                    using var client = new HttpClient();
                    var bytes = await client.GetByteArrayAsync(url);
                    return encoding.GetString(bytes);
                }
                catch (HttpRequestException e)
                {
                    if (e.StatusCode == HttpStatusCode.NotFound) return null;
                    throw e;
                }
            });
        }

        static string GetHashString<T>(string text) where T : HashAlgorithm, new()
        {
            var algorithm = new T();
            var bytes = Encoding.UTF8.GetBytes(text);
            var hash = algorithm.ComputeHash(bytes);
            return string.Join("", hash.Select(b => b.ToString("X2")));
        }

        public void ClearCache()
        {
            _cache.RegenerateCacheDir();
        }
    }
}
