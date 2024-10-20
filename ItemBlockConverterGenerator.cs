using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CuberiteScripts
{
    internal static class ItemBlockConverterGenerator
    {
        public static void GenerateItemToBlockConverter(string data_path)
        {
            var (items, blocks) = GetItemsAndBlocks(data_path);
            var tims = items.Intersect(blocks).ToImmutableList();
            int max_name_len = items.Max(a => a.Length);

            foreach (var item in tims)
            {
                Utils.save($"\t\t\tcase Item::{item}:{Utils.Spacing(max_name_len - item.Length)} return BlockType::{item};");
            }
            Utils.SaveToFile();
        }

        public static void GenerateBlockToItemConverter(string data_path)
        {
            var (items, blocks) = GetItemsAndBlocks(data_path);
            var tims = blocks.Intersect(items).ToImmutableList();
            int max_name_len = blocks.Max(a => a.Length);

            foreach (var item in tims)
            {
                Utils.save($"\t\t\tcase BlockType::{item}:{Utils.Spacing(max_name_len- item.Length)} return Item::{item};");
            }
            Utils.SaveToFile();
        }

        static (List<string> items, List<string> block) GetItemsAndBlocks(string data_path)
        {
            var items = GetItemsFromJson(data_path + "\\registries.json");
            var block = Utils.GetBlocksFromJson(data_path + "\\blocks.json");
            return (items, block);
        }

        public static List<string> GetItemsFromJson(string file_path)
        {
            object? obj = JsonConvert.DeserializeObject(File.ReadAllText(file_path));
            if (obj is not JObject jObject) return null;
            var jsonBlocks = jObject.Children<JProperty>().ToList().Find(a => a.Name == "minecraft:item").Children()
                .ToList().Children().Cast<JProperty>().ToList().Find(a => a.Name == "entries").Children()
                .ToList().Children().Cast<JProperty>().ToList()
                .Select(itm => itm.Name.FormatNameNoPrefix()).ToList();
            return jsonBlocks;
        }
    }
}
