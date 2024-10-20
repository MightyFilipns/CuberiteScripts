using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CuberiteScripts
{
    internal static class NamespaceSerializerGen
    {
        public static void GenerateItemToString(string file_path)
        {
            var items =Utils.GetItemsFromJsonRaw(file_path);
            int max = items.Max(a => a.FormatNameNoPrefix().Length);
            foreach (var item in items)
            {
                Utils.save($"case Item::{item.FormatNameNoPrefix()}:{Utils.Spacing(max-item.FormatNameNoPrefix().Length)} return \"{item.Remove(0,"minecraft:".Length)}\";");
            }
        }

        public static void GenerateStringToItemMap(string file_path)
        {
            var items = Utils.GetItemsFromJsonRaw(file_path);
            int max = items.Max(a => a.Length);
            foreach (var item in items)
            {
                Utils.save($"{{ \"{item}\",{Utils.Spacing(max-item.Length)}Item::{item.FormatNameNoPrefix()} }},");
            }
        }

        public static void GenerateBlockToString(string file_path)
        {
            var items = Utils.GetBlocksFromJsonRaw(file_path);
            int max = items.Max(a => a.FormatNameNoPrefix().Length);
            foreach (var item in items)
            {
                Utils.save($"case BlockType::{item.FormatNameNoPrefix()}:{Utils.Spacing(max - item.FormatNameNoPrefix().Length)} return \"{item.Remove(0, "minecraft:".Length)}\";");
            }
        }

        public static void GenerateStringToBlockMap(string file_path)
        {
            var items = Utils.GetBlocksFromJsonRaw(file_path);
            int max = items.Max(a => a.Length);
            foreach (var item in items)
            {
                Utils.save($"{{ \"{item}\",{Utils.Spacing(max - item.Length)}BlockType::{item.FormatNameNoPrefix()} }},");
            }
        }

        public static void GenerateCustomStateToString(string file_path)
        {
            var items = Utils.GetCustomStats(file_path, false).Select(a => a.name);
            int max = items.Max(a => a.FormatNameNoPrefix().Length);
            foreach (var item in items)
            {
                Utils.save($"case CustomStatistic::{item.FormatNameNoPrefix()}:{Utils.Spacing(max - item.FormatNameNoPrefix().Length)} return \"{item.RemovePrefix()}\";");
            }
        }

        public static void GenerateStringToCustomStatkMap(string file_path)
        {
            var items = Utils.GetCustomStats(file_path, false).Select(a => a.name);
            int max = items.Max(a => a.Length);
            foreach (var item in items)
            {
                Utils.save($"{{ \"{item}\",{Utils.Spacing(max - item.Length)}CustomStatistic::{item.FormatNameNoPrefix()} }},");
            }
        }
    }
}
