using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.VisualBasic;

namespace CuberiteScripts
{
    internal class HandlerGen
    {
        public static void GenerateItemHandlers(string file_path, string blocks_file)
        {
            object? obj = JsonConvert.DeserializeObject(File.ReadAllText(file_path));
            if (obj is not JObject jObject) return;
            var jsonBlocks = jObject.Children<JProperty>().ToList().Select(a=> ((JObject)a.Children().First(), a.Name)).ToList();
            jsonBlocks.Sort((a, b) => string.Compare(a.Name, b.Name, StringComparison.Ordinal));
            string str;
            List<string> existing = new();
            while ((str = Console.ReadLine()) != "")
            {
                existing.Add(str);
            }

            var processed = existing.Select(line =>
            {
                var b = line.Split(" ").Where(a => a.Length != 0).ToList();
                var c = b[3];
                c = c.Substring(7);
                if (line.Contains("Apple"))
                {
                    int a = 0;
                }
                if (b.Count < 5)
                {
                    return (ItemName: c.Remove(c.Length - 2, 2),line);
                }
                else
                {
                    return (ItemName: c.Remove(c.Length - 1, 1), line);
                }
            }).ToList();

            var blocks = Utils.GetBlocksFromJson(blocks_file);
            int max = 45;
            existing.Add("/t  //  -- new starts here");
            foreach (var (jToken, name) in jsonBlocks)
            {
                if (processed.All(a => a.ItemName != name.FormatNameNoPrefix()))
                {
                    string handlername = GetConstructor(name);
                    string constructor = $"(Item::{name.FormatNameNoPrefix()});";
                    if (jToken?.Children().First()?.Children().First()?["minecraft:food"] is not null)
                    {
                        var food = jToken?.Children().First()?.Children().First()?["minecraft:food"];
                        float foodValue = food["nutrition"].Value<float>();
                        float saturation = food["saturation"].Value<float>();
                        handlername = "cItemSimpleFoodHandler";
                        constructor = $"(Item::{name.FormatNameNoPrefix()}, cItemHandler::FoodInfo({foodValue}, {saturation}));";
                    }
                    else if (blocks.Contains(name.FormatNameNoPrefix()) && handlername == "cUnimplementedItemHandler")
                    {
                        handlername = "cSimplePlaceableItemHandler";
                    }
                    existing.Add($"\tconstexpr {handlername}{Utils.Spacing(30-handlername.Length)}Item{name.FormatNameNoPrefix()}Handler{Utils.Spacing(max - name.FormatNameNoPrefix().Length - 4 - 7)}{constructor}");
                }
            }
            Console.Clear();
            foreach (var se in existing)
            {
                Console.WriteLine(se);
            }

            string GetConstructor(string name)
            {
                name = name.FormatNameNoPrefix();
                if (name.Contains("SpawnEgg"))
                {
                    return "cItemSpawnEggHandler";
                }
                if (name.Contains("Stairs"))
                {
                    return "cItemStairsHandler";
                }
                if (name.Contains("Trapdoor"))
                {
                    return "cItemTrapdoorHandler";
                }
                if (name.Contains("Slab"))
                {
                    return "cItemSlabHandler";
                }
                if (name.Contains("Sign"))
                {
                    return "cItemSignHandler";
                }
                if (name.Contains("Log"))
                {
                    return "cItemLogHandler";
                }
                if (name.Contains("Leaves"))
                {
                    return "cItemLeavesHandler";
                }
                if (name.Contains("FenceGate"))
                {
                    return "cItemFenceGateHandler";
                }
                if (name.Contains("Fence"))
                {
                    return "cItemFenceHandler";
                }
                if (name.Contains("Door"))
                {
                    return "cItemDoorHandler";
                }
                if (name.Contains("Button"))
                {
                    return "cItemButtonHandler";
                }
                if (name.Contains("cItemBoatHandler"))
                {
                    return "cItemButtonHandler";
                }
                if (name.Contains("Dye"))
                {
                    return "cItemDyeHandler";
                }
                return "cUnimplementedItemHandler";
            }
        }

        public static void GenerateItemToHandler(string file_path)
        {
            var items = Utils.GetItemsFromJsonRaw(file_path).Select(a => a.FormatNameNoPrefix()).ToList();
            int max = items.Max(a => a.Length);
            foreach (var item in items)
            {
                Utils.save($"\t\t\tcase Item::{item}:{Utils.Spacing(max-item.Length)} return Item{item}Handler;");
            }
            Utils.SaveToFile();
        }

        public static void GenerateMaxStackSize(string file_path)
        {
            object? obj = JsonConvert.DeserializeObject(File.ReadAllText(file_path));
            if (obj is not JObject jObject) return;
            var jsonBlocks = jObject.Children<JProperty>().ToList().Select(a => ((JObject)a.Children().First(), a.Name)).ToList();
            int max = jsonBlocks.Max(a => a.Name.FormatNameNoPrefix().Length);
            foreach (var (item_root, name) in jsonBlocks)
            {
                var stack_size = (int)item_root["components"]["minecraft:max_stack_size"];
               // var item_name = (string)item_root["components"]["minecraft:item_name"];
                if ((stack_size is 1) || (stack_size == 64 && name.Contains("block")))  // TODO: check if it is a block properly
                {
                    continue;
                }
                Utils.save($"case Item::{name.FormatNameNoPrefix()}:{Utils.Spacing(max-name.FormatNameNoPrefix().Length)} return {stack_size};");
            }
            Utils.SaveToFile();
        }

        public static void GenerateBlockHandlers(string file_path)
        {
            List<string> existing = new();
            string str;
            while ((str = Console.ReadLine()) != "")
            {
                existing.Add(str);
            }

            var processed = existing.Select(line =>
            {
                var b = line.Split(" ").Where(a => a.Length != 0).ToList();
                var c = b[2];
                int indx = c.IndexOf("Handler") > 0 ? c.IndexOf("Handler") : c.Length;
                c = c.Substring("Block".Length, indx - "Block".Length);
                return c;
            }).ToList();
            var blocks = Utils.GetBlocksFromJson(file_path);
            var enumerable = blocks.Except(processed).ToList();
            Console.Clear();
            foreach (var se in enumerable)
            {
                string type = GetConstructor(se);
                Utils.save($"\tconstexpr {type}{Utils.Spacing(45-type.Length)} Block{se}Handler(BlockType::{se});");
            }
            Utils.SaveToFile();

            string GetConstructor(string name)
            {
                if (name.Contains("Stairs"))
                {
                    return "cBlockStairsHandler";
                }
                if (name.Contains("Trapdoor"))
                {
                    return "cBlockTrapdoorHandler";
                }
                if (name.Contains("Slab"))
                {
                    return "cBlockSlabHandler";
                }
                if (name.Contains("Sign"))
                {
                    return "cBlockSignPostHandler";
                }
                if (name.Contains("Log"))
                {
                    return "cBlockLogHandler";
                }
                if (name.Contains("Leaves"))
                {
                    return "cBlockLeavesHandler";
                }
                if (name.Contains("FenceGate"))
                {
                    return "cBlockFenceGateHandler";
                }
                if (name.Contains("Fence"))
                {
                    return "cBlockFenceHandler";
                }
                if (name.Contains("Door"))
                {
                    return "cBlockDoorHandler";
                }
                if (name.Contains("Button"))
                {
                    return "cBlockButtonHandler";
                }
                return "cDefaultBlockHandler";
            }
        }

        public static void GenerateBlockToHandler(string file_path)
        {
            var items = Utils.GetBlocksFromJson(file_path);
            int max = items.Max(a => a.Length);
            foreach (var item in items)
            {
                Utils.save($"\t\t\tcase BlockType::{item}:{Utils.Spacing(max - item.Length)} return Block{item}Handler;");
            }
            Utils.SaveToFile();
        }
    }
}
