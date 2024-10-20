using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Reflection.Metadata.Ecma335;
using System.Text;
using System.Threading.Tasks;

namespace CuberiteScripts
{
    internal static class block_state_printer
    {
        static string tosavefile = "C:\\Users\\Filip\\Desktop\\states.txt";
        static List<string> towrite = new List<string>();

        enum EntryType
        {
            Facing,
            Int,
            Bool,
            Enum
        }
        public static void generateBlockStatesH(string file_path)
        {
            object? obj = JsonConvert.DeserializeObject(File.ReadAllText(file_path));
            Console.WriteLine(obj.GetType());
            List<(string toptrint, int id)> lines = new List<(string toptrint, int id)>();

            if (obj is not JObject jObject) return;

            JObject jo = jObject;
            List<JToken> jsonBlocks = jo.Children().ToList();
            foreach (JToken jCurrentBlock in jsonBlocks) // for each block
            {
                JProperty currentBlock = (JProperty)jCurrentBlock;
                string blockId = currentBlock.Name.FormatNameNoPrefix();
                towrite.Add($"\tnamespace {blockId}\n\t{{");

                List<JProperty> props = currentBlock.Children().ToList()[0].Children<JProperty>().ToList();
                JProperty? states = props.Find(a => a.Name == "properties");
                List<(EntryType type, string name, List<String> entries)> blockStates = new List<(EntryType type, string name, List<string> entries)>();
                int start_id;
                if (states is not null)
                {
                    foreach (var state in states?.Children().ToList()[0].Children().ToList()!)  // for each block state
                    {
                        string state_name = ((JProperty)state).Name;
                        var arr = state.First.Children().ToList().Select(a => a.Value<string>()).Select(Utils.FormatName).ToList();
                        EntryType type = EntryType.Enum;
                        if (arr.All(a => a is "True" or "False"))
                        {
                            type = EntryType.Bool;
                        }
                        else if (arr.All(a => int.TryParse(a, out _)))
                        {
                            type = EntryType.Int;
                        }
                        else if (arr.Contains("North") && arr.Contains("South") && arr.Contains("East") && arr.Contains("West") )
                        {
                            type = EntryType.Facing;
                            var old_arr = arr.Select(a => a);
                            arr = ["BLOCK_FACE_ZM", "BLOCK_FACE_ZP", "BLOCK_FACE_XM", "BLOCK_FACE_XP"];
                            if (old_arr.Contains("Up"))
                            {
                                arr.Add("BLOCK_FACE_YP");
                            }
                            if (old_arr.Contains("Down"))
                            {
                                arr.Add("BLOCK_FACE_YM");
                            }
                        }

                        if (type == EntryType.Enum)
                        {
                            towrite.Add($"\t\tenum class {state_name.FormatName()}\n\t\t{{");
                            foreach (var se in arr)
                            {
                                towrite.Add("\t\t\t" + se + ",");
                            }
                            towrite.Add("\t\t};");
                        }
                        blockStates.Add((type, state_name, arr.ToList()));
                    }
                }


                JProperty? everyState = props.Find(a => a.Name == "states");
                start_id = (int)everyState?.Children().ToList()[0].Children().First().Children<JProperty>().ToList().Find(a => a.Name == "id");

                string args = $"\t\tconstexpr BlockState {blockId}(";

                blockStates.Sort((a,b) => string.Compare(a.name, b.name, StringComparison.Ordinal));

                foreach (var blockState in blockStates)
                {
                    args += "const ";
                    switch (blockState.type)
                    {
                        case EntryType.Bool:
                            args += "bool ";
                            break;
                        case EntryType.Enum:
                            args += "enum " + blockState.name.FormatName() + " ";
                            break;
                        case EntryType.Int:
                            args += "int ";
                            break;
                        case EntryType.Facing:
                            args += "eBlockFace ";
                            break;
                    }
                    args += blockState.name.FormatName() + ", ";
                }

                if (blockStates.Count > 0)
                {
                    args = args.Remove(args.Length - 2);
                }
                args += ")";

                towrite.Add(args + "\n\t\t{");

                printBlockStates(blockStates, ref start_id, 3);

                towrite.Add("\t\t}");

                if (blockStates.Count != 0)
                {
                    towrite.Add($"\t\tBlockState {blockId}();");
                }
                foreach (var blockState in blockStates)
                {
                    string curr = "\t\t";
                    switch (blockState.type)
                    {
                        case EntryType.Bool:
                            curr += "bool ";
                            break;
                        case EntryType.Enum:
                            curr += "enum " + blockState.name.FormatName() + " ";
                            break;
                        case EntryType.Int:
                            curr += "int ";
                            break;
                        case EntryType.Facing:
                            curr += "eBlockFace ";
                            break;
                    }
                    curr += blockState.name.FormatName() + "(BlockState Block);";
                    towrite.Add(curr);
                }
                towrite.Add("\t}");
            }
            foreach (var se in towrite)
            {
                Console.WriteLine(se);
            }
            File.WriteAllLines(tosavefile, towrite);
        }

        public static void generateBlockStatesCPP(string file_path)
        {
            object? obj = JsonConvert.DeserializeObject(File.ReadAllText(file_path));
            Console.WriteLine(obj.GetType());
            List<(string toptrint, int id)> lines = new List<(string toptrint, int id)>();

            if (obj is not JObject jObject) return;

            JObject jo = jObject;
            List<JToken> jsonBlocks = jo.Children().ToList();
            foreach (JToken jCurrentBlock in jsonBlocks) // for each block
            {
                JProperty currentBlock = (JProperty)jCurrentBlock;
                string blockId = currentBlock.Name.FormatNameNoPrefix();
                towrite.Add($"\tnamespace {blockId}\n\t{{");

                List<JProperty> props = currentBlock.Children().ToList()[0].Children<JProperty>().ToList();
                JProperty? properties = props.Find(a => a.Name == "properties");
                List<(EntryType type, string name, List<String> entries)> blockStates = new List<(EntryType type, string name, List<string> entries)>();
                int start_id;
                if (properties is not null)
                {
                    foreach (var state in properties?.Children().ToList()[0].Children().ToList()!)  // for each block state
                    {
                        string state_name = ((JProperty)state).Name;
                        var arr = state.First.Children().ToList().Select(a => a.Value<string>()).Select(Utils.FormatName).ToList();
                        EntryType type = EntryType.Enum;
                        if (arr.All(a => a is "True" or "False"))
                        {
                            type = EntryType.Bool;
                        }
                        else if (arr.All(a => int.TryParse(a, out _)))
                        {
                            type = EntryType.Int;
                        }
                        else if (arr.Contains("North") && arr.Contains("South") && arr.Contains("East") && arr.Contains("West"))
                        {
                            type = EntryType.Facing;
                            var old_arr = arr.Select(a=>a);
                            arr = ["BLOCK_FACE_ZM", "BLOCK_FACE_ZP", "BLOCK_FACE_XM", "BLOCK_FACE_XP"];
                            if (old_arr.Contains("Up"))
                            {
                                arr.Add("BLOCK_FACE_YP");
                            }
                            if (old_arr.Contains("Down"))
                            {
                                arr.Add("BLOCK_FACE_YM");
                            }
                        }

                        blockStates.Add((type, state_name, arr.ToList()));
                    }
                }


                JProperty? block_states = props.Find(a => a.Name == "states");
                var List_states = block_states?.Children().ToList()[0].Children().ToList();
                start_id = (int)List_states.First().Children<JProperty>().ToList().Find(a => a.Name == "id");
                int defualt_id1 = (int)List_states
                    .Find(a => a.Children<JProperty>().ToList().FindIndex(b => b.Name == "default") > -1)
                    .Children<JProperty>().ToList().Find(a => a.Name == "id");

                List<(int id, List<(string state_name, string state_value)> corresponding_states)> per_state_id = Utils.GetAllBlockStates(block_states);

                if (blockStates.Count != 0)
                {
                    towrite.Add($"\t\tBlockState {blockId}()");
                    towrite.Add("\t\t{");
                    towrite.Add($"\t\t\treturn {defualt_id1};");
                    towrite.Add("\t\t}");
                }
                foreach (var blockState in blockStates)
                {
                    string curr = "\t\t";
                    switch (blockState.type)
                    {
                        case EntryType.Bool:
                            curr += "bool ";
                            break;
                        case EntryType.Enum:
                            curr += $"enum {blockState.name.FormatName()} ";
                            break;
                        case EntryType.Int:
                            curr += "int ";
                            break;
                        case EntryType.Facing:
                            curr += "eBlockFace ";
                            break;
                    }
                    curr += blockState.name.FormatName() + "(BlockState Block)";
                    towrite.Add(curr);
                    towrite.Add("\t\t{");
                    towrite.Add("\t\t\tswitch(Block.ID)");
                    towrite.Add("\t\t\t{");

                    blockState.entries.Sort();
                    var ids = blockState.entries;

                    
                    foreach (var id in ids)
                    {
                        string to_print_str = "\t\t\t\t";
                        var this_case = per_state_id.FindAll(a => a.corresponding_states.FindIndex(b => b.state_value == id && blockState.name.FormatName() == b.state_name) > -1);
                        if (this_case.Count == 0)
                        {
                            int a = 0;
                        }

                        if (id != ids.Last())
                        {
                            foreach (var valueTuple in this_case)
                            {
                                to_print_str += $"case {valueTuple.id}: ";
                            }
                        }
                        else
                        {
                            to_print_str += $"default: ";
                        }

                        to_print_str += $" return {HandleRight(id)};";
                        save(to_print_str);

                        string HandleRight(string v)
                        {
                            var entry = blockState;
                            return entry.type switch
                            {
                                EntryType.Enum => entry.name.FormatName() + $"::{v.FormatName()}",
                                EntryType.Int => v,
                                EntryType.Bool => v == "True" ? entry.name.FormatName() : $"!{entry.name.FormatName()}",
                                EntryType.Facing => "eBlockFace::" + v,
                                _ => throw new Exception("unknown"),
                            };
                        }
                    }

                    towrite.Add("\t\t\t}");
                    towrite.Add("\t\t}");
                }
                towrite.Add("\t}");
            }
            foreach (var se in towrite)
            {
                Console.WriteLine(se);
            }
            File.WriteAllLines(tosavefile, towrite);
        }

        static void printBlockStates(List<(EntryType type, string name, List<String> entries)> blockStates, ref int startId, int depth)
        {
            var cloned = blockStates.Select(a => a).ToList();
            if (cloned.Count == 0)
            {
                save(GetSpacing(depth) + $"return {startId};");
                startId++;
                return;
            }
            var entry = cloned.First();
            cloned.RemoveAt(0);
            foreach (var valueTuple in entry.entries)
            {
                string ret = "";
                if (cloned.Count == 0)
                {
                    ret = $" return {startId};" + ret;
                    startId++;
                }
                if (valueTuple == entry.entries.Last())
                {
                    save(GetSpacing(depth) + $"else" + ret);
                }
                else if(valueTuple == entry.entries.First())
                {
                    save(GetSpacing(depth) + $"if ({HandleLeft()}{HandleRight(valueTuple)})" + ret);
                }
                else
                {
                    save(GetSpacing(depth) + $"else if ({HandleLeft()}{HandleRight(valueTuple)})" + ret);
                }

                if (cloned.Count != 0)
                {
                    printBlockStates(cloned,ref startId,depth+1);
                }
            }

            string HandleLeft()
            {
                if (entry.type == EntryType.Bool)
                {
                    return "";
                }
                return $"{entry.name.FormatName()} == ";
            }

            string HandleRight(string v)
            {
                return entry.type switch
                {
                    EntryType.Enum => entry.name.FormatName() + $"::{v.FormatName()}",
                    EntryType.Int => v,
                    EntryType.Bool => v == "True" ? entry.name.FormatName() : $"!{entry.name.FormatName()}",
                    EntryType.Facing => "eBlockFace::" + v,
                    _ => throw new Exception("unknown"),
                };
            }
        }

        public static void BlockStateType(string file_path)
        {
            object? obj = JsonConvert.DeserializeObject(File.ReadAllText(file_path));
            if (obj is not JObject jObject) return;
            JObject jo = jObject;
            List<JToken> jsonBlocks = jo.Children().ToList();
            foreach (JToken jCurrentBlock in jsonBlocks) // for each block
            {
                JProperty currentBlock = (JProperty)jCurrentBlock;
                string blockId = currentBlock.Name.FormatNameNoPrefix();

                List<JProperty> props = currentBlock.Children().ToList()[0].Children<JProperty>().ToList();


                JProperty? block_states = props.Find(a => a.Name == "states");

                string current_block = "";
                foreach (var state in block_states?.Children().ToList()[0].Children().ToList()!)  // for each block state
                {
                    var ls3 = state.Children<JProperty>().ToList();
                    var id = ls3.Find(a => a.Name == "id")?.Value;
                    current_block += $"case {id}: ";
                }

                current_block += $"return BlockType::{blockId};";
                Utils.save(current_block);
            }
            Utils.SaveToFile();
        }

        static string GetSpacing(int len)
        {
            return Enumerable.Repeat("\t", len).Aggregate((a, b) => a + b);
        }

        static void save(string arg)
        {
            Console.WriteLine(arg);
            towrite.Add(arg);
        }
    }
}
