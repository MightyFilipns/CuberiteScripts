using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Reflection.Metadata.Ecma335;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;

namespace CuberiteScripts
{
    internal static class ItemConverterGenerator
    {
        public static readonly bool SortItemAlphabetically = true;
        public static void IdToItemConvGen(string file_path)
        {
            var items = GetItemsFromJson(file_path);
            foreach (var (name, id) in items)
            {
                Utils.save($"\t\t\tcase {id}: return Item::{name.FormatNameNoPrefix()};");
            }
        }

        public static void ItemToIdConvGen(string file_path)
        {
            var items = GetItemsFromJson(file_path);
            foreach (var (name, id) in items)
            {
                Utils.save($"\t\t\tcase Item::{name.FormatNameNoPrefix()}: return {id};");
            }
        }

        public static List<(string name, int id)> GetItemsFromJson(string file_path)
        {
            object? obj = JsonConvert.DeserializeObject(File.ReadAllText(file_path));
            if (obj is not JObject jObject) return null;
            List<(string name, int id)> jsonBlocks = jObject.Children<JProperty>().ToList().Find(a => a.Name == "minecraft:item").Children()
                .ToList().Children().Cast<JProperty>().ToList().Find(a => a.Name == "entries").Children()
                .ToList().Children().Cast<JProperty>().ToList()
                .Select(itm => (itm.Name, (int)itm.Children().First().Children<JProperty>().First().Value)).ToList();
            if (SortItemAlphabetically)
            {
                jsonBlocks.Sort(static (a, b) => string.Compare(a.name, b.name, StringComparison.Ordinal));
            }
            else
            {
                jsonBlocks.Sort((a, b) => a.id - b.id);
            }
            return jsonBlocks;
        }
    }
}
