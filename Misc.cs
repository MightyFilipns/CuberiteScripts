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
                Utils.save($"\t{item},");
            }
            Utils.SaveToFile();
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

        public static void GenSpawnEggs(string file_path)
        {
            var eggs = Utils.GetItemsFromJsonRaw(file_path).Select(a => a.FormatNameNoPrefix())
                .Where(a => a.Contains("SpawnEgg"));
            int max = eggs.Max(a => a.Length);
            foreach (var egg in eggs)
            {
                Utils.save($"case Item::{egg}:{Utils.Spacing(max-egg.Length)} return mt{egg.Remove(egg.IndexOf("SpawnEgg"),"SpawnEgg".Length)};");   
            }
            Utils.SaveToFile();
        }

        public static void GenBlockEntities(string file_path)
        {
            var blocks = Utils.GetBlockEntities(file_path);
            int max = blocks.Max(a => a.name.Length);
            foreach (var block in blocks)
            {
                Console.WriteLine($"case BlockType::{block.name}:{Utils.Spacing(max - block.name.Length)} Action = {block.protocol_id}; break;");
            }
        }
        public static void EffectsToString(string file_path)
        {
            var blocks = Utils.GetEffects(file_path);
            //blocks = blocks.Select((a => a with { name = "eff" + a.name.FormatNameNoPrefix() })).ToList();
            blocks.Sort((a,b) => a.protocol_id - b.protocol_id);
            int max = blocks.Max(a => ("eff" + a.name.FormatNameNoPrefix()).Length);
            foreach (var block in blocks)
            {
                Console.WriteLine($"case cEntityEffect::eType::{"eff" + block.name.FormatNameNoPrefix()}:{Utils.Spacing(max - ("eff" + block.name.FormatNameNoPrefix()).Length)} return \"{block.name.Remove(0,"minecraft:".Length)}\";");
            }
        }
        public static void StringToEffect(string file_path)
        {
            var blocks = Utils.GetEffects(file_path);
            //blocks = blocks.Select((a => a with { name = "eff" + a.name.FormatNameNoPrefix() })).ToList();
            blocks.Sort((a,b) => a.protocol_id - b.protocol_id);
            int max = blocks.Max(a => a.name.RemovePrefix().Length);
            foreach (var block in blocks)
            {
                string effname = block.name.RemovePrefix();
                Console.WriteLine($"{{ \"{effname}\",{Utils.Spacing(max - effname.Length)} {"cEntityEffect::eType::eff" + block.name.FormatNameNoPrefix()}}},");
            }
        }

        public static void ListSounds(string data_path)
        {
            var sounds = Utils.GetSounds(data_path);
            int max = sounds.Max(a => a.name.Length - "minecraft:".Length);
            foreach (var (name, protocolId) in sounds)
            {
                string name_no_prefix = name.Remove(0, "minecraft:".Length);
                Console.WriteLine($"{{ \"{name_no_prefix}\",{Utils.Spacing(max - name_no_prefix.Length)}{protocolId} }},");
            }
        }

        public static void ListSoundsEnum(string data_path)
        {
            var sounds = Utils.GetSounds(data_path);
            int max = sounds.Max(a => a.name.Length - "minecraft:".Length);
            foreach (var (name, protocolId) in sounds)
            {
                string name_no_prefix = name.Remove(0, "minecraft:".Length);
                //Console.WriteLine($"{{ \"{name_no_prefix}\",{Utils.Spacing(max - name_no_prefix.Length)}{protocolId} }},");
                Console.WriteLine($"{name.FormatNameNoPrefixSound()},");
            }
        }

        public static void ListBlocksSwitch(string file_path)
        {
            var blocks = Utils.GetBlocks(file_path);
            blocks.Sort((a,b) => a.protocol_id - b.protocol_id);
            int max = blocks.Max(a => (a.name.FormatNameNoPrefix()).Length);
            foreach (var block in blocks)
            {
                Console.WriteLine($"case BlockType::{block.name.FormatNameNoPrefix()}:{Utils.Spacing(max - (block.name.FormatNameNoPrefix()).Length)} return {block.protocol_id};");
            }
        }

        public static void ListEntities(string file_path)
        {
            var blocks = Utils.GetEntities(file_path);
            foreach (var block in blocks)
            {
                Console.WriteLine($"et{block.name.FormatNameNoPrefix()},");
            }
        }
        public static void GenEntitiesTranslation(string file_path)
        {
            var blocks = Utils.GetEntities(file_path);
            int max = blocks.Max(a => a.name.FormatNameNoPrefix().Length);
            foreach (var block in blocks)
            {
                Console.WriteLine($"case Type::et{block.name.FormatNameNoPrefix()}:{Utils.Spacing(max - block.name.FormatNameNoPrefix().Length)} return {block.protocol_id};");
            }
        }

        public static void ListFluids(string file_path)
        {
            var blocks = Utils.GetFluids(file_path);
            foreach (var block in blocks)
            {
                Console.WriteLine($"{block.name.FormatNameNoPrefix()},");
            }
        }
    }
}