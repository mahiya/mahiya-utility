using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace Mahiya.Utility
{
    public static class CsvExtention
    {
        const char Separator = ',';

        public static async Task WriteCsvAsync<T>(this IEnumerable<T> contents, string path)
        {
            var lines = contents.ToCsvLines();
            await File.WriteAllLinesAsync(path, lines);
        }

        static IEnumerable<string> ToCsvLines<T>(this IEnumerable<T> contents)
        {
            var properties = typeof(T).GetProperties();
            var lines = new List<string>();
            lines.Add(string.Join(Separator, properties.Select(p => p.Name)));
            lines.AddRange(contents.Select(c => string.Join(Separator, properties.Select(
                p => (
                        p.PropertyType == typeof(int)
                        || p.PropertyType == typeof(long)
                        || p.PropertyType == typeof(float)
                        || p.PropertyType == typeof(double)
                    ) 
                    ? p.GetValue(c) 
                    : $"\"{p.GetValue(c)}\""
            ))));
            return lines;
        }
    }
}
