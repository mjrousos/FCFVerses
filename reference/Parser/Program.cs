using System;
using System.Collections.Generic;
using System.IO;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Parser
{
    public class Program
    {
        private const string FilePath = @"..\en_kjv.json";
        public static void Main(string[] args)
        {
            JArray? books = null;
            Dictionary<string, int[]> VerseCounts = new Dictionary<string, int[]>();

            var serializer = new JsonSerializer();
            using (var input = File.OpenText(FilePath))
            using (var jr = new JsonTextReader(input))
            {
                books = serializer.Deserialize(jr) as JArray;
            }

            if (books == null)
            {
                Console.WriteLine("Could not deserialize input file into a JSON array");
                return;
            }
            else
            {
                Console.WriteLine($"Found {books.Count} books");
            }

            foreach (JObject book in books)
            {
                var name = book.Property("name")?.Value.ToString();
                var chapters = new List<int>();
                foreach (JArray chapter in (JArray)(book.Property("chapters")!.Value))
                {
                    if (name == "Matthew" && chapters.Count == 1)
                    {
                        Console.WriteLine(chapter.ToString());
                    }
                    chapters.Add(chapter.Count);
                }
                Console.WriteLine(name);
                Console.WriteLine(string.Join(",", chapters));
                VerseCounts.Add(name, chapters.ToArray());
            }

            using (var fs = new FileStream("VerseCounts.json", FileMode.Create, FileAccess.Write))
            using (var sw = new StreamWriter(fs))
            using (var jw = new JsonTextWriter(sw))
            {
                serializer.Serialize(jw, VerseCounts);
            }
        }
    }
}
