using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CuberiteScripts
{
    internal static class Misc
    {
        public static void ListItems(string data_path)
        {
            object? obj = JsonConvert.DeserializeObject(File.ReadAllText(data_path));
            if (obj is not JObject jObject) return;
            var Items = jObject.Children<JProperty>().ToList().Find(a => a.Name == "minecraft:item").Children()
                .ToList().Children().Cast<JProperty>().ToList().Find(a => a.Name == "entries").Children()
                .ToList().Children().Cast<JProperty>().ToList()
                .Select(itm => itm.Name.FormatNameNoPrefix()).ToList();
            foreach (var item in Items)
            {
                Console.WriteLine($"\t{item},");
            }
        }

        public static void ListBlocks(string data_path)
        {
            object? obj = JsonConvert.DeserializeObject(File.ReadAllText(data_path));
            if (obj is JObject jo)
            {
                var blocks = jo.Children<JProperty>().ToList();
                foreach (JProperty block in blocks)
                {
                    Console.WriteLine($"\t{block.Name.FormatNameNoPrefix()},");
                }
            }
        }

        public static void ListStats(string data_path)
        {
            var stats = Utils.GetCustomStats(data_path);
            foreach (var item in stats)
            {
                Console.WriteLine($"\t{item.name},");
            }
        }
    }
}
