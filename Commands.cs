using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CuberiteScripts
{
    internal static class Commands
    {
        public static void PrintCommandParsers(string file_path)
        {
            var commandParsers = GetCommandArgumentTypes(file_path);
            commandParsers.Sort((a,b) => a.protocol_id - b.protocol_id);
            int max = commandParsers.Max(a => a.name.Length);
            foreach (var (name, protocolId) in commandParsers)
            {
                Console.WriteLine($"case eCommandParserType::{name}:{Utils.Spacing(max - name.Length)} return {protocolId};");
            }
        }
        public static List<(string name, int protocol_id)> GetCommandArgumentTypes(string file_path, bool format = true)
        {
            object? obj = JsonConvert.DeserializeObject(File.ReadAllText(file_path));
            if (obj is not JObject jObject) return null;
            var jsonBlocks = jObject.Children<JProperty>().ToList().Find(a => a.Name == "minecraft:command_argument_type").Children()
                .ToList().Children().Cast<JProperty>().ToList().Find(a => a.Name == "entries").Children()
                .ToList().Children().Cast<JProperty>().ToList()
                .Select(itm => (format ? itm.Name.FormatNameNoPrefix() : itm.Name, (int)itm.Children().First().Children<JProperty>().First().Value)).ToList();
            return jsonBlocks;
        }
    }
}
