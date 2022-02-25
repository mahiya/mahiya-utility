using Newtonsoft.Json;
using System;
using System.IO;
using System.Threading.Tasks;

namespace Mahiya.Utility
{
    public static class JsonExtention
    {
        public static void DisplayJson(this object obj)
        {
            Console.WriteLine(JsonConvert.SerializeObject(obj, Formatting.Indented));
        }

        public static async Task WriteJsonAsync(this object obj, string path)
        {
            await File.WriteAllTextAsync(path, JsonConvert.SerializeObject(obj, Formatting.Indented));
        }
    }
}
