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
                Utils.save($"bool {tag_name}(BlockType a_block)\n{{");
                if (tags.Count == 0 && ref_totag.Count > 0)
                {
                    Utils.save($"\treturn {BuildExpression(ref_totag)};");
                }
                else
                {
                    if (ref_totag.Count != 0)
                    {
                        string expr = ref_totag.First().FormatNameNoPrefixTag() + "(a_block)";
                        foreach (var se in ref_totag.Skip(1))
                        {
                            expr += " || " + se.FormatNameNoPrefixTag() + "(a_block)";
                        }
                        Utils.save($"\tif ({expr})\n\t{{\n\t\treturn true;\n\t}}");
                    }
                    if (tags.Count > 0)
                    {
                        Utils.save("\tswitch (a_block)\n\t{");
                        PrintTagsSwitch(tags);
                        Utils.save("\t\t\treturn true;");
                        Utils.save("\t\tdefault: return false;");
                        Utils.save("\t}");
                    }
                    else
                    {
                        Utils.save("\treturn false;");
                    }
                }
                Utils.save("}");
            }
            Utils.SaveToFile();
        }

        static string BuildExpression(List<string> ref_tags)
        {
            string expr = ref_tags.First().FormatNameNoPrefixTag() + "(a_block)";
            foreach (var se in ref_tags.Skip(1))
            {
                expr += " || " + se.FormatNameNoPrefixTag() + "(a_block)";
            }
            return expr;
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
                Utils.save($"bool {tag_name}(BlockType a_block);");
            }
        }


        public static void PrintTagsSwitch(List<string> blocks)
        {
            foreach (var block in blocks)
            {
                Utils.save($"\t\tcase BlockType::{block.FormatNameNoPrefix()}:");
            }
        }

        struct BlockTag
        {
            public List<string> values;
        }
    }
}
