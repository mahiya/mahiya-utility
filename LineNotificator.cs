using Newtonsoft.Json;
using System;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace Mahiya.Utility
{
    public class LineNotificator
    {
        public static async Task BroadcastAsync(string text, string accessToken)
        {
            using var httpClient = new HttpClient();
            httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);
            var req = new
            {
                messages = new[]
                {
                    new
                    {
                        type = "text",
                        text = text
                    }
                }
            };
            var content = new StringContent(JsonConvert.SerializeObject(req), Encoding.UTF8, "application/json");
            var resp = await httpClient.PostAsync("https://api.line.me/v2/bot/message/broadcast", content);
            if (resp.StatusCode != HttpStatusCode.OK)
                throw new Exception($"Failed with calling LINE Messaging API：{await resp.Content.ReadAsStringAsync()}");
        }
    }
}
