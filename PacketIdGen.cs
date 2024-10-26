using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CuberiteScripts
{
    internal class PacketIdGen
    {
        public static void GenPacketIds(string orignal_path,string phase_name, string bound_to)
        {
            bool print_ids = true;

            var old_packt = GetPackets(orignal_path, phase_name, bound_to);
            old_packt.Sort((a,b) => a.protocol_id - b.protocol_id);

            if (print_ids)
            {
                foreach (var (name, protocolId) in old_packt)
                {
                    Console.WriteLine($"{name} - {protocolId}");
                }
            }
            else
            {
                foreach (var (name, protocolId) in old_packt)
                {
                    Console.WriteLine($"{name}");
                }
            }

        }

        public static List<(string name, int protocol_id)> GetPackets(string file_path,string phase_name,string bound_to, bool format = true)
        {
            object? obj = JsonConvert.DeserializeObject(File.ReadAllText(file_path));
            if (obj is not JObject jObject) return null;
            var jsonBlocks = jObject.Children<JProperty>().ToList().Find(a => a.Name == phase_name).Children()
                .ToList().Children().Cast<JProperty>().ToList().Find(a => a.Name == bound_to).Children()
                .ToList().Children().Cast<JProperty>().ToList()
                .Select(itm => (format ? itm.Name.FormatNameNoPrefix() : itm.Name, (int)itm.Children().First().Children<JProperty>().First().Value)).ToList();
            return jsonBlocks;
        }
    }
}
