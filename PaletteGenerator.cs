using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CuberiteScripts
{
    internal class PaletteGenerator
    {
        public static IComparer<(int id, List<(string state_name, string state_value)>)>? com = Comparer<(int id, List<(string state_name, string state_value)>)>.Create(
            (a, b) =>
            {
                if (a.Item2.Count != b.Item2.Count)
                {
                    return a.Item2.Count - b.Item2.Count;
                }

                var nums = a.Item2.Zip(b.Item2);
                foreach (var (a1, b1) in nums)
                {
                    if (a1.state_name != b1.state_name)
                    {
                        return a1.state_name.CompareTo(b1.state_name);
                    }
                    if (a1.state_value != b1.state_value)
                    {
                        return a1.state_value.CompareTo(b1.state_value);
                    }
                }

                return 0;
            });

        /*
         * target_version specifies for which version of the game the palette is for
         * block_states_generated_using is the file that as used to genearted the blockstates.cpp/.h file. This is to ensure that if block states have changed the palette is properly generated
         */
        public static void GeneratePalette(string target_version, string block_states_generated_using)
        {
            object? obj = JsonConvert.DeserializeObject(File.ReadAllText(target_version));

            if (obj is not JObject jObject) return;

            object? obj2 = JsonConvert.DeserializeObject(File.ReadAllText(block_states_generated_using));

            if (obj2 is not JObject jObject2) return;
            var other_blocks = jObject2.Children().Cast<JProperty>().ToList();


            JObject jo = jObject;
            List<JToken> jsonBlocks = jo.Children().ToList();
            foreach (JToken jCurrentBlock in jsonBlocks) // for each block
            {
                JProperty currentBlock = (JProperty)jCurrentBlock;
                string blockId = currentBlock.Name.FormatNameNoPrefix();

                var corresponding_block = other_blocks.Find(a => a.Name.FormatNameNoPrefix() == blockId);
                if (corresponding_block == null)
                {
                    continue;
                }

                List<JProperty> props = currentBlock.Children().ToList()[0].Children<JProperty>().ToList();
                JProperty? block_states = props.Find(a => a.Name == "states");
                List<(int id, List<(string state_name, string state_value)> corresponding_states)> per_state_id = Utils.GetAllBlockStates(block_states);

                List<JProperty> corr_props = corresponding_block.Children().ToList()[0].Children<JProperty>().ToList();
                JProperty? corr_block_states = corr_props.Find(a => a.Name == "states");
                List<(int id, List<(string state_name, string state_value)> corresponding_states)> per_state_id2 = Utils.GetAllBlockStates(corr_block_states);

                per_state_id2.Sort(com);
                per_state_id.Sort(com);
                bool identical = per_state_id.Count == per_state_id2.Count;
                foreach (var (First, Second) in per_state_id.Zip(per_state_id2))
                {
                    if (com.Compare(First, Second) != 0)
                    {
                        identical = false;
                        break;
                    }
                }

                if (identical && per_state_id.Count == 0)
                {
                    Utils.save($"\t\t\tcase {blockId}::{blockId}().ID: return {(int)block_states?.Children().ToList()[0].Children().ToList().First().Children<JProperty>().ToList().Find(a => a.Name == "id").Value};");
                    continue;
                }

                per_state_id.Sort((a, b) => a.id - b.id);
                foreach (var (id, correspondingStates) in per_state_id)
                {
                    if (!identical)
                    {
                        Utils.save($"\t\t\tcase {blockId}::{blockId}().ID: return {id};");
                        break;
                    }
                    string args = "";
                    correspondingStates.Sort((a, b) => string.Compare(a.state_name, b.state_name, StringComparison.Ordinal));
                    args = correspondingStates.Aggregate("",(arg, state) =>
                    {
                        var (state_name, state_value) = state;
                        string final_state = "";
                        if (!int.TryParse(state_value, out _))
                        {
                            if (state_value == "True" || state_value == "False")
                            {
                                final_state = Utils.FixBool(state_value);
                            }
                            else if (Utils.IsBlockFace(state_value))
                            {
                                final_state = $"eBlockFace::{state_value}";
                            }
                            else // is enum
                            {
                                final_state = $"{blockId}::{state_name}::{state_value}";
                            }
                        }
                        else
                        {
                            final_state = state_value;
                        }

                        arg += final_state + ", ";
                        return arg;
                    });

                    if (correspondingStates.Count != 0)
                    {
                        args = args.Remove(args.Length - 2);
                    }

                    Utils.save($"\t\t\tcase {blockId}::{blockId}({args}).ID: return {id};");
                }
            }
            Utils.SaveToFile();
        }

        public static void GenerateCustomStats(string file_path)
        {
            var stats = Utils.GetCustomStats(file_path);
            int max_name_len = stats.Max(a => a.name.Length);
            foreach (var (name, protocol_id) in stats)
            {
                Utils.save($"\t\t\tcase CustomStatistic::{name}:{Utils.Spacing(max_name_len - name.Length)} return {protocol_id};");
            }
        }
        /* Generates the complete palette cpp file so that it can be just copy-pasted */
        public static void CompletePaletteFileGen(string target_version_blocks,string target_version_registry, string block_states_generated_using, string version_name, bool print_blocks)
        {
            string formatted_version = version_name.Replace(".", "_");
            Utils.save("#include \"Globals.h\"");
            Utils.save($"#include \"Palette_{formatted_version}.h\"");
            Utils.save("#include \"Registries/BlockStates.h\"");
            Utils.save("#include \"BlockType.h\"");
            Utils.save($"namespace Palette_{formatted_version}\n{{");
            if (print_blocks)
            {
                Utils.save("\tUInt32 From(const BlockState Block)\r\n\t{\r\n\t\tusing namespace Block;\r\n\r\n\t\tswitch (Block.ID)\r\n\t\t{");
                GeneratePalette(target_version_blocks, block_states_generated_using);
                Utils.save("\t\t\tdefault: return 0;\r\n\t\t}\r\n\t}");
            }
            Utils.save("\tUInt32 From(const CustomStatistic ID)\r\n\t{\r\n\t\tswitch (ID)\r\n\t\t{");
            GenerateCustomStats(target_version_registry);
            Utils.save("\t\t\tdefault: return -1;\r\n\t\t}\r\n\t}");
            Utils.save("\tItem ToItem(const UInt32 ID)\r\n\t{\r\n\t\tswitch (ID)\r\n\t\t{");
            ItemConverterGenerator.IdToItemConvGen(target_version_registry);
            Utils.save("\t\t}\r\n\t}");
            Utils.save("\tUInt32 From(const Item ID)\r\n\t{\r\n\t\tswitch (ID)\r\n\t\t{");
            ItemConverterGenerator.ItemToIdConvGen(target_version_registry);
            Utils.save("\t\t\tdefault: return -1;\r\n\t\t}\r\n\t}");
            Utils.save($"}}  // namespace Palette_{formatted_version}");
            Utils.SaveToFile();
        }
    }
}
