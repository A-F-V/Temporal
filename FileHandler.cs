using System.IO;
using System.Text.Json;

namespace Temporal
{
    internal static class FileHandler
    {
        public static T Load<T>(string json_file)
        {
            string json_string = File.ReadAllText(json_file);
            return JsonSerializer.Deserialize<T>(json_string);
        }
    }
}