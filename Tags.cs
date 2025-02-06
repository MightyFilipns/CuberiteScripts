using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;

namespace CuberiteScripts
{
    public class Tags
    {
        public static void ListTags(string string_path)
        {
            DirectoryInfo di = new DirectoryInfo(string_path);
            var files = di.EnumerateFiles("*.json", new EnumerationOptions()
            {
                RecurseSubdirectories = true
            }).ToList();
            foreach (var fileInfo in files)
            {
                var rootdif = "";
                if (string_path != fileInfo.DirectoryName)
                {
                    rootdif = fileInfo.DirectoryName.Replace(string_path, "").Split('\\').Select(Utils.FormatName).Aggregate((a,b) => b + "_" + a);
                }
                var name = rootdif + fileInfo.Name.Replace(".json", "").FormatName() + ",";
                Utils.save(name);
            }
        }

        public static void PrintTagTrans(string string_path, string enum_name)
        {
            DirectoryInfo di = new DirectoryInfo(string_path);
            var files = di.EnumerateFiles("*.json", new EnumerationOptions()
            {
                RecurseSubdirectories = true
            }).ToList();
            List<(string formatted_name, string original)> tags = new List<(string, string)>();
            foreach (var fileInfo in files)
            {
                var rootdif = "";
                if (string_path != fileInfo.DirectoryName)
                {
                    rootdif = fileInfo.DirectoryName.Replace(string_path, "").Split('\\').Select(Utils.FormatName).Aggregate((a,b) => b + "_" + a);
                }

                var file_name = fileInfo.Name.Replace(".json", "").Replace(@"\", "");

                var formatted_name = rootdif + file_name.FormatName();
                var original = fileInfo.DirectoryName.Replace(string_path, "");
                if (original != "")
                {
                    original = original.Substring(1) + @"\\";
                }
                tags.Add((formatted_name, original + file_name));
            }

            int max = tags.Max(a => a.formatted_name.Length);
            foreach (var (formattedName, original) in tags)
            {
                Utils.save($"case {enum_name}::{formattedName}:{Utils.Spacing(max - formattedName.Length)} return \"{original}\";");
            }
        }
    }
}
