using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CuberiteScripts
{
    internal static class Utils
    {
        static List<String> dirs = ["BLOCK_FACE_ZM", "BLOCK_FACE_ZP", "BLOCK_FACE_XM", "BLOCK_FACE_XP", "BLOCK_FACE_YP", "BLOCK_FACE_YM"];

        private static List<string> towrite = new();

        public static readonly string filepath = "C:\\Users\\Filip\\Desktop\\states.txt";

        public static void save(string v)
        {
            towrite.Add(v);
            Console.WriteLine(v);
        }

        public static void SaveToFile()
        {
            File.WriteAllLines(filepath, towrite);
        }
        public static string FormatName(this string name)
        {
            return name.Split("_").Select(a =>
            {
                StringBuilder strb = new(a);
                strb[0] = char.ToUpper(strb[0]);
                return strb.ToString();
            }).ToList().Aggregate((s1, s2) => s1 + s2);
        }
        public static string FormatNameNoPrefix(this string name)
        {
            return name.Remove(0,"minecraft:".Length).Split("_").Select(a =>
            {
                StringBuilder strb = new(a);
                strb[0] = char.ToUpper(strb[0]);
                return strb.ToString();
            }).ToList().Aggregate((s1, s2) => s1 + s2);
        }

        public static string RemovePrefix(this string name)
        {
            return name.Remove(0, "minecraft:".Length);
        }

        public static string Spacing(int len)
        {
            return Enumerable.Repeat(" ", len).Aggregate("",(a, b) => a += b);
        }

        public static bool IsBlockFace(string str)
        {
            return dirs.Contains(str);
        }
        public static string FixBool(string str)
        {
            return str == "True" ? "true" : "false";
        }

        public static List<string> GetBlocksFromJson(string file_path)
        {
            object? obj = JsonConvert.DeserializeObject(File.ReadAllText(file_path));
            if (obj is not JObject jObject) return null;
            return jObject.Children<JProperty>().ToList().Select(a => a.Name.FormatNameNoPrefix()).ToList();
        }

        public static List<string> GetItemsFromJsonRaw(string file_path)
        {
            object? obj = JsonConvert.DeserializeObject(File.ReadAllText(file_path));
            if (obj is not JObject jObject) return null;
            var jsonBlocks = jObject.Children<JProperty>().ToList().Find(a => a.Name == "minecraft:item").Children()
                .ToList().Children().Cast<JProperty>().ToList().Find(a => a.Name == "entries").Children()
                .ToList().Children().Cast<JProperty>().ToList()
                .Select(itm => itm.Name).ToList();
            return jsonBlocks;
        }

        public static List<string> GetBlocksFromJsonRaw(string file_path)
        {
            object? obj = JsonConvert.DeserializeObject(File.ReadAllText(file_path));
            if (obj is not JObject jObject) return null;
            return jObject.Children<JProperty>().ToList().Select(a => a.Name).ToList();
        }

        public static List<(string name, int protocol_id)> GetCustomStats(string file_path, bool format = true)
        {
            object? obj = JsonConvert.DeserializeObject(File.ReadAllText(file_path));
            if (obj is not JObject jObject) return null;
            var jsonBlocks = jObject.Children<JProperty>().ToList().Find(a => a.Name == "minecraft:custom_stat").Children()
                .ToList().Children().Cast<JProperty>().ToList().Find(a => a.Name == "entries").Children()
                .ToList().Children().Cast<JProperty>().ToList()
                .Select(itm => (format ? itm.Name.FormatNameNoPrefix() : itm.Name, (int)itm.Children().First().Children<JProperty>().First().Value)).ToList();
            return jsonBlocks;
        }

        public static List<(int id, List<(string state_name, string state_value)> corresponding_states)> GetAllBlockStates(JProperty? block_states)
        {
            List<(int id, List<(string state_name, string state_value)> corresponding_states)> per_state_id = new List<(int id, List<(string state_name, string state_value)> corresponding_states)>();
            var enumerable = block_states?.Children().ToList()[0].Children().ToList()!;
            foreach (var state in enumerable)  // for each block state
            {
                var ls3 = state.Children<JProperty>().ToList();
                var id = ls3.Find(a => a.Name == "id")?.Value;
                var pl = ls3.Find(a => a.Name == "properties");
                if (pl == null) continue;
                var props2 = pl.Children().ToList()[0].Children<JProperty>().ToList();
                List<(string state_name, string state_value)> prop_values = new List<(string state_name, string state_value)>();
                foreach (var item3 in props2)
                {
                    var str1 = ((string)item3.Value);
                    var name = item3.Name.FormatName();
                    var str = str1 switch
                    {
                        "north" => "BLOCK_FACE_ZM",
                        "south" => "BLOCK_FACE_ZP",
                        "west" => "BLOCK_FACE_XM",
                        "east" => "BLOCK_FACE_XP",
                        _ => str1.FormatName()
                    };

                    if (str1 == "up" || str1 == "down")
                    {
                        var isfacing = enumerable.Any(a => a.Children<JProperty>().ToList().Find(a => a.Name == "properties").Children().First().Children<JProperty>().ToList().Any(a=> (string)a.Value == "north"));
                        if (isfacing) // just check if theres another direction
                        {
                            str = str1 switch
                            {
                                "up" => "BLOCK_FACE_YP",
                                "down" => "BLOCK_FACE_YM",
                                _ => str1.FormatName()
                            };
                        }
                    }

                    prop_values.Add((name, str));
                }
                per_state_id.Add(((int)id, prop_values));
            }

            return per_state_id;
        }
    }
}
