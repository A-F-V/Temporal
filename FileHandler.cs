using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Temporal
{
    static class FileHandler
    {
        public static T Load<T>(string json_file)
        {
            string json_string = File.ReadAllText(json_file);
            return JsonSerializer.Deserialize<T>(json_string);
        }
    }
}
