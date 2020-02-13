using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text.Json;

namespace WebApi.Utilities
{
    public static class VerseCounts
    {
        private const string ResourceName = "WebApi.Resources.VerseCounts.json";

        private static Lazy<int[][]> LazyVerses { get; } = new Lazy<int[][]>(ParseVerseCounts);

        public static int[][] Verses => LazyVerses.Value;

        private static int[][] ParseVerseCounts()
        {
            using var stream = Assembly.GetExecutingAssembly().GetManifestResourceStream(ResourceName);

            // Use .Result because there isn't a synchronous version of JsonSerializer that accepts a stream
            // and this should be a quick deserialization that happens once early in the app's lifetime.
            return JsonSerializer.DeserializeAsync<Dictionary<string, int[]>>(stream).Result.Values.ToArray();
        }
    }
}
