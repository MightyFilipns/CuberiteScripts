using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CuberiteScripts
{
    internal static class BlockTypeToBlockStateGen
    {
        public static void Generate(string file_path)
        {
            var blocks = Utils.GetBlocksFromJson(file_path);
            int max_name_len = blocks.Max(a => a.Length);
            foreach (var block in blocks)
            {
                Utils.save($"\t\t\tcase BlockType::{block}:{Utils.Spacing(max_name_len-block.Length)} return {block}::{block}().ID;");
            }
        }
    }
}
