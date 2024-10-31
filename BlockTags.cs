using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace CuberiteScripts
{
    internal static class BlockTags
    {
        public static void PrintBlockTagCpp(string folder_path)
        {
            DirectoryInfo di = new DirectoryInfo(folder_path);
            var files = di.EnumerateFiles("*.json").ToList();
            foreach (var fileInfo in files)
            {
                var data = File.ReadAllText(fileInfo.FullName);
                BlockTag blockTag = JsonConvert.DeserializeObject<BlockTag>(data);
                var ref_totag = blockTag.values.Where(a => a[0] == '#').ToList();
                var tags = blockTag.values.Where(a => a[0] != '#').ToList();
                string tag_name = fileInfo.Name.Replace(".json", "").FormatName();
                Console.WriteLine($"bool {tag_name}(BlockType a_block)\n{{");
                foreach (var se in ref_totag)
                {
                    Console.WriteLine($"\tif ({se.FormatNameNoPrefixTag()}(a_block)) {{ return true; }}");
                   
                }

                if (tags.Count > 0)
                {
                    Console.WriteLine("\tswitch(a_block) {");
                    PrintTagsSwitch(tags);
                    Console.WriteLine("\t\t\treturn true;");
                    Console.WriteLine("\t\tdefault: return false;");
                    Console.WriteLine("\t}");
                }
                else
                {
                    Console.WriteLine("\treturn false;");
                }
                Console.WriteLine("}");
            }
        }

        public static void PrintBlockTagH(string folder_path)
        {
            DirectoryInfo di = new DirectoryInfo(folder_path);
            var files = di.EnumerateFiles("*.json").ToList();
            foreach (var fileInfo in files)
            {
                var data = File.ReadAllText(fileInfo.FullName);
                BlockTag blockTag = JsonConvert.DeserializeObject<BlockTag>(data);
                //var ref_totag = blockTag.values.Where(a => a[0] == '#').ToList();
                //var tags = blockTag.values.Where(a => a[0] != '#').ToList();
                string tag_name = fileInfo.Name.Replace(".json", "").FormatName();
                Console.WriteLine($"bool {tag_name}(BlockType a_block);");
            }
        }


        public static void PrintTagsSwitch(List<string> blocks)
        {
            foreach (var block in blocks)
            {
                Console.WriteLine($"\t\tcase BlockType::{block.FormatNameNoPrefix()}:");
            }
        }

        struct BlockTag
        {
            public List<string> values;
        }
    }
}
