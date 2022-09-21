using System.IO;
using System.Text.Json;

namespace AEIRevitDesignAutomation.Common
{
    internal static class Helpers
    {
        internal static void SaveResultAsJson<T>(T dResult)
        {
            var json = JsonSerializer.Serialize(dResult, new JsonSerializerOptions { WriteIndented = true });

            // ReSharper disable once StringLiteralTypo
            const string path = ".\\result.json";
            File.WriteAllText(path, json);
        }

    }
}
