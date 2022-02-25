using Azure.Identity;
using Azure.Storage.Blobs;
using System;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace Mahiya.Utility
{
    public class AzureBlob
    {
        public static async Task UploadAsync(string accountName, string containerName, string blobName, byte[] contents)
        {
            var client = GetBlobClient(accountName, containerName, blobName);
            var uploadData = new BinaryData(contents);
            await client.UploadAsync(uploadData, overwrite: true);
        }

        public static async Task UploadAsync(string accountName, string containerName, string blobName, string contents)
        {
            var bytes = Encoding.UTF8.GetBytes(contents);
            await UploadAsync(accountName, containerName, blobName, bytes);
        }

        public static async Task UploadAsync<T>(string accountName, string containerName, string blobName, T contents)
        {
            var json = JsonSerializer.Serialize(contents);
            await UploadAsync(accountName, containerName, blobName, json);
        }

        public static async Task<byte[]> DownloadAsync(string accountName, string containerName, string blobName)
        {
            var client = GetBlobClient(accountName, containerName, blobName);
            var resp = await client.DownloadContentAsync();
            return resp.Value.Content.ToArray();
        }

        public static async Task<string> DownloadStringAsync(string accountName, string containerName, string blobName)
        {
            var bytes = await DownloadAsync(accountName, containerName, blobName);
            return Encoding.UTF8.GetString(bytes);
        }

        public static async Task<T> DownloadAsync<T>(string accountName, string containerName, string blobName)
        {
            var json = await DownloadStringAsync(accountName, containerName, blobName);
            return JsonSerializer.Deserialize<T>(json);
        }

        private static BlobClient GetBlobClient(string accountName, string containerName, string blobName)
        {
            var blobUri = new Uri($"https://{accountName}.blob.core.windows.net/{containerName}/{blobName}");
            var credential = new DefaultAzureCredential();
            return new BlobClient(blobUri, credential);
        }
    }
}
