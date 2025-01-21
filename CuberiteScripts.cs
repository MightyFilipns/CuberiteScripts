using System.Collections;
using System.Globalization;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Security.Cryptography;
using System.Text;
using System.Xml.Linq;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace CuberiteScripts
{
    internal class CuberiteScripts
    {
        static string tosavefile = "C:\\Users\\Filip\\Desktop\\states.txt";
        static string tosavefile2 = "C:\\Users\\Filip\\Desktop\\tostring.txt";
        /* Uncomment the function you want to use here*/
        static void Main(string[] args)
        {
            //genrateBlockStateConverter();
            //GenerateBlockStateWriter();
            //convert4();
            //sortItemhdlByType();
            //GenerateItemtoBlock();
            //sortpackets();
            //extractbl();
            //generateConvert();
            //extractmap();
            //waterlogfix2();


            //GenerateRegistryDataWriter("E:\\minecraft-servers\\server-1-21-3\\generated\\data\\minecraft\\damage_type");
            //GenerateParticleMap();
            //block_state_printer.generateBlockStatesCPP("E:\\minecraft-servers\\server-1-21-4\\generated\\reports\\blocks.json");
            //block_state_printer.generateBlockStatesH("E:\\minecraft-servers\\server-1-21-4\\generated\\reports\\blocks.json");
            //block_state_printer.BlockStateType("E:\\minecraft-servers\\server-1-21-4\\generated\\reports\\blocks.json");
            //ItemConverterGenerator.IdToItemConvGen("E:\\minecraft-servers\\server-1-21\\generated\\reports\\registries.json");
            //ItemConverterGenerator.ItemToIdConvGen("E:\\minecraft-servers\\server-1-21\\generated\\reports\\registries.json");
            //PaletteGenerator.GeneratePalette("E:\\minecraft-servers\\server-1-21-3\\generated\\reports\\blocks.json", "E:\\minecraft-servers\\server-1-21-3\\generated\\reports\\blocks.json");
            //NamespaceSerializerGen.GenerateItemToString("E:\\minecraft-servers\\server-1-21-4\\generated\\reports\\registries.json");
            //NamespaceSerializerGen.GenerateStringToItemMap("E:\\minecraft-servers\\server-1-21-4\\generated\\reports\\registries.json");
            //NamespaceSerializerGen.GenerateBlockToString("E:\\minecraft-servers\\server-1-21-4\\generated\\reports\\blocks.json");
            //NamespaceSerializerGen.GenerateStringToBlockMap("E:\\minecraft-servers\\server-1-21-4\\generated\\reports\\blocks.json");
            //NamespaceSerializerGen.GenerateCustomStateToString("E:\\minecraft-servers\\server-1-21-4\\generated\\reports\\registries.json");
            //NamespaceSerializerGen.GenerateStringToCustomStatkMap("E:\\minecraft-servers\\server-1-21-4\\generated\\reports\\registries.json");
            //Misc.ListStats("E:\\minecraft-servers\\server-1-21-4\\generated\\reports\\registries.json");
            //Misc.ListBlocks("E:\\minecraft-servers\\server-1-21-4\\generated\\reports\\blocks.json");
            //ItemBlockConverterGenerator.GenerateItemToBlockConverter("E:\\minecraft-servers\\server-1-21-4\\generated\\reports");
            //ItemBlockConverterGenerator.GenerateBlockToItemConverter("E:\\minecraft-servers\\server-1-21-4\\generated\\reports");
            //PaletteGenerator.GenerateCustomStats("E:\\minecraft-servers\\server-1-21\\generated\\reports\\registries.json");
            //Misc.ListItems("E:\\minecraft-servers\\server-1-21-4\\generated\\reports\\registries.json");
            //BlockTypeToBlockStateGen.Generate("E:\\minecraft-servers\\server-1-21-4\\generated\\reports\\blocks.json");
            //HandlerGen.GenerateItemToHandler("E:\\minecraft-servers\\server-1-21-4\\generated\\reports\\registries.json");
            //HandlerGen.GenerateItemHandlers("E:\\minecraft-servers\\server-1-21-4\\generated\\reports\\items.json", "E:\\minecraft-servers\\server-1-21-4\\generated\\reports\\blocks.json");
            //HandlerGen.GenerateMaxStackSize("E:\\minecraft-servers\\server-1-21-4\\generated\\reports\\items.json");
            //HandlerGen.GenerateBlockHandlers("E:\\minecraft-servers\\server-1-21-4\\generated\\reports\\blocks.json");
            //HandlerGen.GenerateBlockToHandler("E:\\minecraft-servers\\server-1-21-4\\generated\\reports\\blocks.json");
            //Misc.GenSpawnEggs("E:\\minecraft-servers\\server-1-21-4\\generated\\reports\\registries.json");
            //PacketIdGen.GenPacketIds("E:\\minecraft-servers\\server-1-21-3\\generated\\reports\\packets.json","play","clientbound");
            //Commands.PrintCommandParsers("E:\\minecraft-servers\\server-1.18\\generated\\reports\\registries.json");
            //PaletteGenerator.CompletePaletteFileGen(
            //    "E:\\minecraft-servers\\server-1-21-4\\generated\\reports\\blocks.json",
            //    "E:\\minecraft-servers\\server-1-21-4\\generated\\reports\\registries.json",
            //    "E:\\minecraft-servers\\server-1-21-4\\generated\\reports\\blocks.json",
            //    "1.21.4",
            //    false);
            //Misc.ListSounds("E:\\minecraft-servers\\server-1-21\\generated\\reports\\registries.json");
            //BlockTags.PrintBlockTagH("E:\\minecraft-servers\\server-1-21-3\\generated\\data\\minecraft\\tags\\block\\");
            //BlockTags.PrintBlockTagCpp("E:\\minecraft-servers\\server-1-21-3\\generated\\data\\minecraft\\tags\\block\\");
            //Misc.GenBlockEntities("E:\\minecraft-servers\\server-1-21\\generated\\reports\\registries.json");
            //Misc.ListSoundsEnum("E:\\minecraft-servers\\server-1-21\\generated\\reports\\registries.json");
            //Sounds.EnumToString("E:\\minecraft-servers\\server-1-21\\generated\\reports\\registries.json");

            //idtransform(old =>
            //{
            //    int toret = old;
            //    if (old >= 32)
            //    {
            //        toret++;
            //    }
            //    if (old >= 50)
            //    {
            //        toret++;
            //    }
            //    if (old >= 68)
            //    {
            //        toret += 3;
            //    }
            //    if (old >= 100)
            //    {
            //        toret ++;
            //    }
            //    if (old >= 103)
            //    {
            //        toret++;
            //    }
            //    return toret;
            //});
        }


        private static void GenerateParticleMap()
        {
            String data = File.ReadAllText("E:\\minecraft-servers\\server-1-21\\generated\\reports\\registries.json");
            JObject obj = JObject.Parse(data);
            var obj2 = obj.GetValue("minecraft:particle_type").Children().ToList().Find(static a => ((JProperty)a).Name == "entries").Children().ToList().Children().ToList();
            int max = 32;
            foreach (JProperty item in obj2)
            {
                var name = item.Name.Substring("minecraft:".Length);
                int id = (int)((JProperty)item.Children().ToList().Children().ToList()[0]).Value;
                max = Math.Max(max,name.Length);
                var spacing =  string.Join("", Enumerable.Repeat(" ", max - name.Length));
                Console.WriteLine($"{{ \"{name}\",{spacing}{id} }},");
            }
        }

        private static void GenerateRegistryDataWriter(string file_path)
        {
            string fileloc = "E:\\minecraft-servers\\server-1-21\\generated\\data\\minecraft\\damage_type";
            DirectoryInfo di = new(file_path);
            foreach (var item in di.EnumerateFiles())
            {
                Console.WriteLine($"Pkt.WriteString(\"{ item.Name.Substring(0, item.Name.IndexOf('.'))}\"); Pkt.WriteBool(false);");
            }
        }

        static void GenerateBlockStateWriter()
        {
            string extract = "E:\\minecraft-servers\\another-ms-erver\\generated\\reports\\blocks.json";
            object? obj = JsonConvert.DeserializeObject(File.ReadAllText(extract));
            Console.WriteLine(obj.GetType());
            List<(string toptrint, int id)> lines = new List<(string toptrint, int id)>();
            List<string> toprint = new List<string>();
            int maxbls = 0;
            if (obj is JObject)
            {
                JObject jo = (JObject)obj;
                List<JToken> jtoks = jo.Children().ToList();
                for (int i = 0; i < jtoks.Count; i++)
                {
                    JProperty jtk = (JProperty)jtoks[i];
                    string blockname = jtk.Name.Remove(0, "minecraft:".Length) + "__";
                    string blockname2 = jtk.Name.Remove(0, "minecraft:".Length) + " ";
                    List<string> states = new List<string>();
                    foreach (var item in jtk.Children().ToList())
                    {
                        JToken jtk2 = item;
                        List<JProperty> props = jtk2.Children<JProperty>().ToList();
                        JProperty sts = props.Find(a => a.Name == "states");
                        foreach (var state in sts.Children().ToList()[0].Children().ToList())
                        {
                            //var ls2 = state.Children().ToList();
                            var ls3 = state.Children<JProperty>().ToList();
                            var id = ls3.Find(a => a.Name == "id").Value;
                            //string blockstate = blockname + " ";
                            List<string> bls = new List<string>();
                            List<string> bls2 = new List<string>();
                            List<(string bname, string val)> bpairs = new List<(string bname, string val)>();
                            var pl = ls3.Find(a => a.Name == "properties");
                            bool hasblockstates = pl != null;
                            if (pl != null)
                            {
                                var props2 = pl.Children().ToList()[0].Children<JProperty>().ToList();
                                foreach (var item3 in props2)
                                {
                                    bpairs.Add((item3.Name, (string)item3.Value));
                                    
                                    string pname = item3.Name;
                                    string toadd = pname + "_" + item3.Value + "__";
                                    string toadd2 = pname + ": " + item3.Value + ", ";
                                    bls.Add(toadd);
                                    bls2.Add(toadd2);
                                }
                                maxbls = Math.Max(maxbls, props2.Count);
                            }
                            
                            bls.Sort();
                            bls2.Sort();
                            string finalstr = blockname;
                            string finalstr2 = blockname2;
                            bls.ForEach(a => finalstr += a);
                            bls2.ForEach(a => finalstr2 += a);
                            if (finalstr[finalstr.Length - 2] == '_')
                            {
                                finalstr = finalstr.Substring(0, finalstr.Length - 2);
                            }
                            else
                            {
                                finalstr = finalstr.Substring(0, finalstr.Length - 1);
                            }
                            finalstr2 = finalstr2.Substring(0, finalstr2.Length - 2);
                            states.Add(finalstr.ToUpper());
                            Console.WriteLine($"\tcase {id}: return {finalstr.ToUpper()};");
                            string writerstates = "";
                            for (int k = 0; k < bpairs.Count(); k++)
                            {
                                writerstates += $",{{\"{bpairs[k].bname}\",\"{bpairs[k].val}\"}}";
                                //writerstates += $"aWriter.AddString(\"{bpairs[k].bname}\",\"{bpairs[k].val}\"); ";
                            }

                            //Console.WriteLine($"\tcase {finalstr.ToUpper()}: aWriter.AddString(\"{"name"},{"id"}\"); {(hasblockstates ? $"aWriter.BeginList(\"Properties\",\"TAG_COMPOUND\"); {writerstates} aWriter.EndList();" : "  // no block states" )}");
                            string tp = $"\tcase {finalstr.ToUpper()}: return {{{{{{\"\",\"{jtk.Name.Remove(0, "minecraft:".Length)}\"}}{writerstates}}}}};";
                            Console.WriteLine(tp);
                            toprint.Add(tp);
                            //lines.Add((finalstr.ToUpper(), (int)id));
                        }
                    }
                    /*
                    foreach (var item in states)
                    {
                        Console.WriteLine($"\tcase {item}:");
                    }
                    List<string> names = jtk.Name.Remove(0, "minecraft:".Length).Split("_").Select(a =>
                    {
                        StringBuilder strb = new(a);
                        strb[0] = char.ToUpper(strb[0]);
                        return strb.ToString();
                    }).ToList();
                    string finalname = "";
                    foreach (var item in names)
                    {
                        finalname += item;
                    }
                    Console.WriteLine($"\t\t return BlockType::{finalname};");*/
                }
            }
            File.WriteAllLines(tosavefile2,toprint);
        }

        static void sortItemhdlByType()
        {
            List<string> inp = new List<string>();
            List<string> newinp = new List<string>();
            string inps;
            while (true)
            {
                inps = Console.ReadLine();
                if (inps == "" || inps == null)
                {
                    break;
                }
                inp.Add(inps);
            }
            inp.Sort((a,b) => string.Compare(a.Split(" ")[1], b.Split(" ")[1]));
            foreach (var item in inp)
            {
                Console.WriteLine(item);
            }
        }

        static void convert1()
        {
            List<string> ls = new List<string>();
            for (int i = 0; i < 96; i++)
            {
                ls.Add(Console.ReadLine());
            }
            foreach (var item in ls)
            {
                Console.WriteLine($"case {item.Split(" ")[7].Replace(';', ' ')}: return {{ {item.Substring(9, 3)}, 0 }};");
            }
        }

        static string ConvertIdName(string name)
        {
            return string.Join("", name.Remove(0, "minecraft:".Length).Split("_").Select(a =>
            {
                StringBuilder strb = new(a);
                strb[0] = char.ToUpper(strb[0]);
                return strb.ToString();
            }));
        }

        static void GenerateItemtoBlock()
        {
            string extract = "E:\\minecraft-servers\\another-ms-erver\\generated\\reports\\blocks.json";
            object? obj = JsonConvert.DeserializeObject(File.ReadAllText(extract));
            Console.WriteLine(obj.GetType());
            List<(string toptrint, int id)> lines = new List<(string toptrint, int id)>();
            List<string> toexclude = new List<string>()
            {
                "WallHangingSign",
                "WallSign",
                "WallBanner",
                "WallFan",
                "Potted",
                "WallSkull",
                "WallHead",
                "CandleCake"
            };
            int max = 32;
            if (obj is JObject)
            {
                JObject jo = (JObject)obj;
                List<JToken> jtoks = jo.Children().ToList();
                for (int i = 0; i < jtoks.Count; i++)
                {
                    JProperty jtk = (JProperty)jtoks[i];
                    string rawname = jtk.Name.Remove(0, "minecraft:".Length);
                    string blockname = rawname + "__";
                    string blockname2 = rawname + " ";
                    string fname = string.Join("", jtk.Name.Remove(0, "minecraft:".Length).Split("_").Select(a =>
                    {
                        StringBuilder strb = new(a);
                        strb[0] = char.ToUpper(strb[0]);
                        return strb.ToString();
                    }));
                    max = Math.Max(max, fname.Length);
                    if (!toexclude.Any(a => fname.Contains(a)))
                    {
                        Console.WriteLine($"case Item::{fname}:{string.Join("",Enumerable.Repeat(" ", max - fname.Length))}return BlockType::{fname};");
                    }
                    /*List<string> states = new List<string>();
                    foreach (var item in jtk.Children().ToList())
                    {
                        JToken jtk2 = item;
                        List<JProperty> props = jtk2.Children<JProperty>().ToList();
                        JProperty sts = props.Find(a => a.Name == "states");
                        foreach (var state in sts.Children().ToList()[0].Children().ToList())
                        {
                            //var ls2 = state.Children().ToList();
                            var ls3 = state.Children<JProperty>().ToList();
                            var id = ls3.Find(a => a.Name == "id").Value;
                            //string blockstate = blockname + " ";
                            List<string> bls = new List<string>();
                            List<string> bls2 = new List<string>();
                            var pl = ls3.Find(a => a.Name == "properties");
                            if (pl != null)
                            {
                                var props2 = pl.Children().ToList()[0].Children<JProperty>().ToList();
                                foreach (var item3 in props2)
                                {
                                    string pname = item3.Name;
                                    string toadd = pname + "_" + item3.Value + "__";
                                    string toadd2 = pname + ": " + item3.Value + ", ";
                                    bls.Add(toadd);
                                    bls2.Add(toadd2);
                                }
                            }
                            bls.Sort();
                            bls2.Sort();
                            string finalstr = blockname;
                            string finalstr2 = blockname2;
                            bls.ForEach(a => finalstr += a);
                            bls2.ForEach(a => finalstr2 += a);
                            if (finalstr[finalstr.Length - 2] == '_')
                            {
                                finalstr = finalstr.Substring(0, finalstr.Length - 2);
                            }
                            else
                            {
                                finalstr = finalstr.Substring(0, finalstr.Length - 1);
                            }
                            finalstr2 = finalstr2.Substring(0, finalstr2.Length - 2);
                            states.Add(finalstr.ToUpper());
                            //Console.WriteLine($"\t case {id}: return {finalstr.ToUpper()};");
                            lines.Add((finalstr.ToUpper(), (int)id));
                        }
                    }*/

                }
            }
        }

        static void convert2()
        {
            List<string> ls = new List<string>();
            for (int i = 0; i < 120; i++)
            {
                ls.Add(Console.ReadLine());
            }
            int i2 = 453;
            foreach (var item in ls)
            {
                string toprint = "";
                foreach (var item2 in item)
                {
                    if (char.IsUpper(item2))
                    {
                        toprint += "_";
                    }
                    toprint += item2;
                }
                toprint = Encoding.UTF8.GetString(toprint.Select(char.ToUpper).Select(a => (byte)a).ToArray());
                toprint = toprint.Insert(0, "E_ITEM");
                //toprint += $" = {i2},";
                Console.WriteLine(toprint);
                i2++;
            }
        }

        static void convert4()
        {
            List<string> ls = new List<string>();
            List<string> ls2 = new List<string>();
            for (int i = 0; i < 1312; i++)
            {
                string str = Console.ReadLine().Substring(3);
                str = str.Substring(0, str.Length - 1);
                ls.Add(str);
            }
            for (int i = 0; i < 120; i++)
            {
                //ls2.Add(Console.ReadLine());
            }
            int max = 43;
            for (int i = 0; i < 1312; i++)
            {
                string spaces = Encoding.UTF8.GetString(Enumerable.Repeat(' ', 35 - ls[i].Length + 4).Select(a => (byte)a).ToArray());
                Console.WriteLine($"constexpr cDefaultItemHandler           Item" + ls[i] + spaces + "(Item::" + ls[i] + ");");
            }
        }
        static void convert5()
        {
            List<string> ls = new List<string>();
            List<string> ls2 = new List<string>();
            for (int i = 0; i < 120; i++)
            {
                ls.Add(Console.ReadLine());
            }
            for (int i = 0; i < 120; i++)
            {
                ls2.Add(Console.ReadLine());
            }
            int max = 38;
            for (int i = 0; i < 120; i++)
            {
                string spaces = Encoding.UTF8.GetString(Enumerable.Repeat(' ', max - ls2[i].Length).Select(a => (byte)a).ToArray());
                Console.WriteLine($"case {ls2[i]}:{spaces} return Item{ls[i]};");
            }
        }
        static void extractmap()
        {
            int max = 0;
            string extract = "E:\\minecraft-servers\\another-ms-erver\\generated\\reports\\blocks.json";
            object? obj = JsonConvert.DeserializeObject(File.ReadAllText(extract));
            //Console.WriteLine(obj.GetType());
            List<(string toptrint, int id)> lines = new List<(string toptrint, int id)>();
            if (obj is JObject)
            {
                int idcounter = 0;
                JObject jo = (JObject)obj;
                List<JToken> jtoks = jo.Children().ToList();
                for (int i = 0; i < jtoks.Count; i++)
                {
                    JProperty jtk = (JProperty)jtoks[i];
                    string blockname = jtk.Name.Remove(0, "minecraft:".Length) + "__";
                    string blockname2 = jtk.Name.Remove(0, "minecraft:".Length) + " ";
                    foreach (var item in jtk.Children().ToList())
                    {
                        JToken jtk2 = item;
                        List<JProperty> props = jtk2.Children<JProperty>().ToList();
                        JProperty sts = props.Find(a => a.Name == "states");
                        foreach (var state in sts.Children().ToList()[0].Children().ToList())
                        {
                            //var ls2 = state.Children().ToList();
                            var ls3 = state.Children<JProperty>().ToList();
                            var id = ls3.Find(a => a.Name == "id").Value;
                            //string blockstate = blockname + " ";
                            List<string> bls = new List<string>();
                            List<string> bls2 = new List<string>();
                            var pl = ls3.Find(a => a.Name == "properties");
                            if (pl != null)
                            {
                                var props2 = pl.Children().ToList()[0].Children<JProperty>().ToList();
                                foreach (var item3 in props2)
                                {
                                    string pname = item3.Name;
                                    string toadd = pname + "_" + item3.Value + "__";
                                    string toadd2 = pname + ": " + item3.Value + ", ";
                                    bls.Add(toadd);
                                    bls2.Add(toadd2);
                                    //string nm = item3.Name;
                                }
                            }
                            bls.Sort();
                            bls2.Sort();
                            string finalstr = blockname;
                            string finalstr2 = blockname2;
                            bls.ForEach(a => finalstr += a);
                            bls2.ForEach(a => finalstr2 += a);
                            if (finalstr[finalstr.Length - 2] == '_')
                            {
                                finalstr = finalstr.Substring(0, finalstr.Length - 2);
                            }
                            else
                            {
                                finalstr = finalstr.Substring(0, finalstr.Length - 1);
                            }
                            //finalstr2 = finalstr2.Substring(0, finalstr2.Length - 2);
                            if (finalstr2[finalstr2.Length - 2] == ',')
                            {
                                finalstr2 = finalstr2.Substring(0, finalstr2.Length - 2);
                            }
                            else
                            {
                                finalstr2 = finalstr2.Substring(0, finalstr2.Length - 1);
                            }
                            //Console.WriteLine($"{{\"{finalstr}\", {id}}},");
                            //Console.WriteLine($"\t{finalstr.ToUpper()},");                            

                            Console.WriteLine($"\t case {finalstr.ToUpper()}: return {id};");

                            //Console.WriteLine($"\t case {id}: return {finalstr.ToUpper()};");

                            //Console.WriteLine($"{{\"{finalstr2}\", {finalstr.ToUpper()}}},");
                            
                            //Console.WriteLine($"{idcounter}-{finalstr2}");
                            idcounter++;
                            //Console.WriteLine($"\t case {finalstr2}: return {finalstr.ToUpper()};");
                            lines.Add((finalstr.ToUpper(),(int)id));
                        }
                        //JToken jtk3 = [0];
                        //Console.WriteLine(jtk3.GetType());
                    }
                    //jtk2.First(a => a.name)
                    string fname = string.Join("", jtk.Name.Remove(0, "minecraft:".Length).Split("_").Select(a =>
                    {
                        StringBuilder strb = new(a);
                        strb[0] = char.ToUpper(strb[0]);
                        return strb.ToString();
                    }));
                    max = Math.Max(max, fname.Length);
                    var spacing = string.Join("",Enumerable.Repeat(" ",32-fname.Length));
                    //Console.WriteLine($"\tcase Item::{fname}:{spacing}return Item{fname}Handler;");
                }
            }

            /*
            lines.Sort((a,b) => (a.id > b.id) ? 1 : ((a.id < b.id) ? -1 : 0));
            foreach (var item in lines)
            {
                Console.WriteLine(item);
            }*/
            int a = 0;
        }
        static List<string> tosave = new List<string>();
        static void extractbl()
        {
            string extract = "E:\\minecraft-servers\\another-ms-erver\\generated\\reports\\blocks.json";
            object? obj = JsonConvert.DeserializeObject(File.ReadAllText(extract));
            if (obj is JObject)
            {
                JObject jo = (JObject)obj;
                List<JToken> jsonBlocks = jo.Children().ToList();
                for (int i = 0; i < jsonBlocks.Count; i++)
                {
                    JProperty current_block = (JProperty)jsonBlocks[i];
                    string blockName = current_block.Name.FormatName();
                    List<blockstate> mainlist = new List<blockstate>();
                    foreach (var item in current_block.Children().ToList())
                    {
                        JToken jtk2 = item;
                        List<JProperty> props = jtk2.Children<JProperty>().ToList();
                        JProperty? sts = props.Find( a => a.Name == "states");              
                        JProperty? sts2 = props.Find(a => a.Name == "properties");
                        if (sts2 == null)
                        {
                            continue;
                        }
                        foreach (var state in sts2.Children().ToList()[0].Children().ToList())
                        {
                            string name = ((JProperty)state).Name;
                            var ls3 = state.Children().ToList()[0].Children<JValue>().ToList();
                            var stringlist = ls3.Select(a => (string)a.Value).ToArray();
                            mainlist.Add(new blockstate() {blockstatesname = name,possiblevalues = stringlist});
                        }
                    }
                    printblockstates(mainlist, blockName);
                }
            }

            File.WriteAllLines(tosavefile,tosave);
        }

        static void waterlogfix2()
        {
            List<string> inp = new List<string>();
            List<string> newinp = new List<string>();
            string inps;
            List<string> suffixes = ["TRUE", "FALSE"]; /*["HEAD", "SKULL"];*/ /*["NORTH","SOUTH","WEST","EAST"];*/
            while (true)
            {
                inps = Console.ReadLine();
                if (inps == "" || inps == null)
                {
                    break;
                }
                inp.Add(inps);
            }
            foreach (string item in inp)
            {
                string insert1 = /*"__POWERED_FALSE";*/   "__WATERLOGGED_FALSE";
                string insert2 = /*"__POWERED_TRUE";*/   "__WATERLOGGED_TRUE";
                string edit = item;
                int fi = -1;
                int len = 0;
                List<(int len, int index)> pairs = new List<(int len, int index)>();
                foreach (var item1 in suffixes)
                {
                    int i = item.LastIndexOf(item1);
                    if (i != -1)
                    {
                        len = item1.Length;
                        fi = i;
                        pairs.Add((len, i));
                    }
                }
                pairs.Sort((a, b) => a.index > b.index ? -1 : a.index < b.index ? 1 : 0);
                var p = pairs[0];
                fi = p.index;
                len = p.len;
                if (fi == -1)
                {
                    Console.WriteLine($"Can find suffix in {item}. Skipping");
                    continue;
                }
                if (fi != -1)
                {
                    edit = edit.Insert(fi + len, insert1);
                    newinp.Add(edit);
                    edit = item;
                    edit = edit.Insert(fi + len, insert2);
                    newinp.Add(edit);
                }
            }
            int k = suffixes.Count-1;
            bool mode2 = false;
            foreach (var item in newinp)
            {
                if (k % suffixes.Count == 0 && mode2)
                {
                    Console.WriteLine("\t "+item.Substring(item.IndexOf(":")+1));
                }
                else
                {
                    Console.WriteLine(item);
                }
                k++;
            }
        }
        static void waterlogfix()
        {
            List<string> inp = new List<string>();
            List<string> newinp = new List<string>();
            string inps;
            while (true)
            {
                inps = Console.ReadLine();
                if (inps == "" || inps == null)
                {
                    break;
                }
                inp.Add(inps);
            }
            foreach (string item in inp)
            {
                string edit = item;
                int fi = item.IndexOf("FALSE");
                int len;
                if (fi != -1)
                {
                    len = "FALSE".Length;
                }
                else
                {
                    fi = item.IndexOf("TRUE");
                    len = "TRUE".Length;
                }
                if (fi != -1)
                {
                    edit = edit.Insert(fi + len, "__WATERLOGGED_FALSE");
                    newinp.Add(edit);
                    edit = item;
                    edit = edit.Insert(fi + len, "__WATERLOGGED_TRUE");
                    newinp.Add(edit);
                }
            }
            foreach (var item in newinp)
            {
                Console.WriteLine(item);
            }
        }

        static void generateConvert()
        {
            string extract = "E:\\minecraft-servers\\another-ms-erver\\generated\\reports\\blocks.json";
            object? obj = JsonConvert.DeserializeObject(File.ReadAllText(extract));
            Console.WriteLine(obj.GetType());
            List<(string toptrint, int id)> lines = new List<(string toptrint, int id)>();
            if (obj is JObject)
            {
                JObject jo = (JObject)obj;
                List<JToken> jtoks = jo.Children().ToList();
                for (int i = 0; i < jtoks.Count; i++)
                {
                    JProperty jtk = (JProperty)jtoks[i];
                    string blockname = jtk.Name.Remove(0, "minecraft:".Length) + "__";
                    string blockname2 = jtk.Name.Remove(0, "minecraft:".Length) + " ";
                    List<string> states = new List<string>();
                    foreach (var item in jtk.Children().ToList())
                    {
                        JToken jtk2 = item;
                        List<JProperty> props = jtk2.Children<JProperty>().ToList();
                        JProperty sts = props.Find(a => a.Name == "states");
                        foreach (var state in sts.Children().ToList()[0].Children().ToList())
                        {
                            //var ls2 = state.Children().ToList();
                            var ls3 = state.Children<JProperty>().ToList();
                            var id = ls3.Find(a => a.Name == "id").Value;
                            //string blockstate = blockname + " ";
                            List<string> bls = new List<string>();
                            List<string> bls2 = new List<string>();
                            var pl = ls3.Find(a => a.Name == "properties");
                            if (pl != null)
                            {
                                var props2 = pl.Children().ToList()[0].Children<JProperty>().ToList();
                                foreach (var item3 in props2)
                                {
                                    string pname = item3.Name;
                                    string toadd = pname + "_" + item3.Value + "__";
                                    string toadd2 = pname + ": " + item3.Value + ", ";
                                    bls.Add(toadd);
                                    bls2.Add(toadd2);
                                }
                            }
                            bls.Sort();
                            bls2.Sort();
                            string finalstr = blockname;
                            string finalstr2 = blockname2;
                            bls.ForEach(a => finalstr += a);
                            bls2.ForEach(a => finalstr2 += a);
                            if (finalstr[finalstr.Length - 2] == '_')
                            {
                                finalstr = finalstr.Substring(0, finalstr.Length - 2);
                            }
                            else
                            {
                                finalstr = finalstr.Substring(0, finalstr.Length - 1);
                            }
                            finalstr2 = finalstr2.Substring(0, finalstr2.Length - 2);
                            states.Add(finalstr.ToUpper());
                            //Console.WriteLine($"\t case {id}: return {finalstr.ToUpper()};");
                            lines.Add((finalstr.ToUpper(), (int)id));
                        }
                    }
                    foreach (var item in states)
                    {
                        Console.WriteLine($"\tcase {item}:");
                    }
                    List<string> names = jtk.Name.Remove(0, "minecraft:".Length).Split("_").Select(a =>
                    {
                        StringBuilder strb = new(a);
                        strb[0] = char.ToUpper(strb[0]);
                        return strb.ToString();
                    }).ToList();
                    string finalname = "";
                    foreach (var item in names)
                    {
                        finalname += item;
                    }
                    Console.WriteLine($"\t\t return BlockType::{finalname};");
                }
            }
        }

        static void genrateBlockStateConverter()
        {
            string extract = "E:\\minecraft-servers\\another-ms-erver\\generated\\reports\\blocks.json";
            object? obj = JsonConvert.DeserializeObject(File.ReadAllText(extract));
            Console.WriteLine(obj.GetType());
            List<(string toptrint, int id)> lines = new List<(string toptrint, int id)>();
            List<string> towrite = new List<string>();
            if (obj is JObject)
            {
                JObject jo = (JObject)obj;
                List<JToken> jtoks = jo.Children().ToList();
                for (int i = 0; i < jtoks.Count; i++) // for each block
                {
                    JProperty jtk = (JProperty)jtoks[i];
                    string rawname = jtk.Name.Remove(0, "minecraft:".Length);
                    string blockname = rawname + "__";
                    string blockname2 = rawname + " ";
                    List<string> states = new List<string>();
                    foreach (var item in jtk.Children().ToList())
                    {
                        JToken jtk2 = item;
                        List<JProperty> props = jtk2.Children<JProperty>().ToList();
                        JProperty sts = props.Find(a => a.Name == "states");
                        foreach (var state in sts.Children().ToList()[0].Children().ToList())
                        {
                            //var ls2 = state.Children().ToList();
                            var ls3 = state.Children<JProperty>().ToList();
                            var id = ls3.Find(a => a.Name == "id").Value;
                            //string blockstate = blockname + " ";
                            List<string> bls = new List<string>();
                            List<string> bls2 = new List<string>();
                            var pl = ls3.Find(a => a.Name == "properties");
                            if (pl != null)
                            {
                                var props2 = pl.Children().ToList()[0].Children<JProperty>().ToList();
                                foreach (var item3 in props2)
                                {
                                    string pname = item3.Name;
                                    string toadd = pname + "_" + item3.Value + "__";
                                    string toadd2 = pname + ": " + item3.Value + ", ";
                                    bls.Add(toadd);
                                    bls2.Add(toadd2);
                                }
                            }
                            bls.Sort();
                            bls2.Sort();
                            string finalstr = blockname;
                            string finalstr2 = blockname2;
                            bls.ForEach(a => finalstr += a);
                            bls2.ForEach(a => finalstr2 += a);
                            if (finalstr[finalstr.Length - 2] == '_')
                            {
                                finalstr = finalstr.Substring(0, finalstr.Length - 21);
                            }
                            else
                            {
                                finalstr = finalstr.Substring(0, finalstr.Length - 1);
                            }
                            finalstr2 = finalstr2.Substring(0, finalstr2.Length - 2);
                            states.Add(finalstr.ToUpper());
                            //Console.WriteLine($"\t case {id}: return {finalstr.ToUpper()};");
                            Console.WriteLine($"{{\"{finalstr2}\", {finalstr.ToUpper()}}},");
                            towrite.Add($"{{\"{finalstr2}\", {finalstr.ToUpper()}}},");
                            lines.Add((finalstr.ToUpper(), (int)id));
                        }
                    }
                    /*
                    foreach (var item in states)
                    {
                        Console.WriteLine($"\tcase {item}:");
                    }
                    List<string> names = jtk.Name.Remove(0, "minecraft:".Length).Split("_").Select(a =>
                    {
                        StringBuilder strb = new(a);
                        strb[0] = char.ToUpper(strb[0]);
                        return strb.ToString();
                    }).ToList();
                    string finalname = "";
                    foreach (var item in names)
                    {
                        finalname += item;
                    }
                    Console.WriteLine($"\t\t return BlockType::{finalname};");*/
                    //jtk2.First(a => a.name)
                    File.WriteAllLines(tosavefile,towrite);
                }
            }
        }

        static string[] sides = ["east","west","north","south"];
        static string getstateName(string name,string pname)
        {
            if (name.Contains("RAIL") && pname == "shape")
            {
                return "rail_" + pname;
            }
            if (name.Contains("STAIR") && pname == "shape")
            {
                return "stairs_" + pname;
            }
            else if (name.Contains("WALL") && sides.Contains(pname))
            {
                return "wall_" + pname;
            }
            else if (name.Contains("WIRE") && sides.Contains(pname) && !name.Equals("TRIPWIRE"))
            {
                return "wire_" + pname;
            }
            else if ((name.Contains("TRAPDOOR") || name.Contains("STAIR")) && pname == "half")
            {
                return "block_" + pname;
            }
            else if (name.Contains("COMPARATOR") && pname == "mode")
            {
                return "comp_" + pname;
            }
            else if (name.Contains("PISTON") && pname == "type")
            {
                return "piston_" + pname;
            }
            else if (name.Contains("SLAB") && pname == "type")
            {
                return "slab_" + pname;
            }
            else if (name.Contains("STRUCTURE") && pname == "mode")
            {
                return "str_" + pname;
            }
            else if (name.Contains("NETHER_PORTAL") && pname == "axis")
            {
                return "v_" + pname;
            }
            else if (pname == "short")
            {
                return "shorts";
            }
            return pname;
        }

        static string getEnumName(string pname)
        {
            switch (pname)
            {
                case "v_axis":
                    return "VAxis";
                case "axis":
                    return "Axis";
                case "facing":
                    return "Direction";
                case "orientation":
                    return "Orientation";
                case "face":
                    return "BlockFace";
                case "attachment":
                    return "Attachment";
                case "wall_east":
                case "wall_north":
                case "wall_south":
                case "wall_west":
                    return "WallShape";
                case "wire_east":
                case "wire_north":
                case "wire_south":
                case "wire_west":
                    return "WireConnection";
                case "half":
                    return "DoubleBlockHalf";
                case "block_half":
                    return "BlockHalf";
                case "rail_shape":
                    return "RailShape";
                case "part":
                    return "BedPart";
                case "type":
                    return "ChestType";
                case "comp_mode":
                    return "ComparatorMode";
                case "hinge":
                    return "DoorHinge";
                case "instrument":
                    return "Instrument";
                case "piston_type":
                    return "PistonType";
                case "slab_type":
                    return "SlabType";
                case "stairs_shape":
                    return "StairShape";
                case "str_mode":
                    return "StructureBlockMode";
                case "leaves":
                    return "BambooLeaves";
                case "tilt":
                    return "Tilt";
                case "vertical_direction":
                    return "Direction";
                case "thickness":
                    return "Thickness";
                case "sculk_sensor_phase":
                    return "SculkSensorPhase";
                case "trial_spawner_state":
                    return "TrialSpawnerState";
                case "vault_state":
                    return "VaultState";
                default:
                    return "";
            }
        }
        static void printblockstates(List<blockstate> states,string blockid)
        {
            var name = string.Concat(blockid.Split("_").Select(a =>
            {
                StringBuilder strb = new(a);
                strb[0] = char.ToUpper(strb[0]);
                return strb.ToString();
            }).ToList());
            Console.WriteLine($"case BlockType::{name}:");
            tosave.Add($"case BlockType::{name}:");
            states.Sort((a,b) => string.Compare(a.blockstatesname,b.blockstatesname));
            printone(states, 0,blockid.ToUpper(),blockid.ToUpper());
        }
        static void printone(List<blockstate> states,int depth,string name,string blockname)
        {
            var indent = new string(Enumerable.Repeat('\t', depth + 1).ToArray());
            if (states.Count == 0)
            {
                Console.WriteLine($"{indent}return ENUM_BLOCKS::{name};");
                tosave[tosave.Count - 1] += $" return ENUM_BLOCKS::{name};";
                //tosave.Add($"{indent}return ENUM_BLOCKS::{name};");
                return;
            }
            blockstate st = states[0];
            bool first = true;
            bool useswitch = true;//!st.possiblevalues.Contains("true");
            string stname = getstateName(blockname, st.blockstatesname);
            string enumname = getEnumName(stname);
            if (useswitch)
            {
                save($"{indent}switch (blockstate.{stname})");
                save(indent+"{");

            }
            foreach (var item in st.possiblevalues)
            {

                string cond = first ? "if" : "else if";
                first = false;
                if (useswitch)
                {
                    save($"{indent+"\t"}case {(enumname == "" ? "" + item : enumname + "::" + item.ToUpper())}:");
                }
                else
                {
                    save($"{indent}{cond} (blockstate.{stname} == {(enumname == "" ? ""+item : enumname+"::"+item.ToUpper())})");
                }
                //Console.WriteLine($"{indent}{{");
                List <blockstate> states2 = states.ToList();
                states2.RemoveAt(0);
                string newname = String.Concat(name);
                printone(states2,depth+(useswitch ? 2 : 1), newname += $"__{st.blockstatesname.ToUpper()}_{item.ToUpper()}",blockname);
                //Console.WriteLine(indent + "}");
            }
            if (useswitch)
            {
                save(indent+"}");
            }
        }
        static void sortpackets()
        {
            List<string> ls = new List<string>();
            string toadd = "";
            do
            {
                toadd = Console.ReadLine();
                if (toadd != null && toadd != "")
                    ls.Add(toadd);
                else
                    break;
            } while (true);
            ls.Sort((a,b) => ExtractPacketId(a) - ExtractPacketId(b));
            foreach (var item in ls)
            {
                Console.WriteLine(item);
            }
            static int ExtractPacketId(string sid)
            {
                return int.Parse(((string[])sid.Split(" ")).Last().Replace(';', ' ').Replace("0x", "").Replace(":",""), NumberStyles.HexNumber);
            }
        }
        static void idtransform(Func<int,int> transform)
        {
            List<string> ls = new List<string>();
            string toadd = "";
            do
            {
                toadd = Console.ReadLine();
                if (toadd != null && toadd != "")
                    ls.Add(toadd);
                else
                    break;
            } while (true);
            ls.Sort((a, b) => ExtractPacketId(a) - ExtractPacketId(b));
            for (int i = 0; i < ls.Count; i++)
            {
                var pckt = ls[i];
                int oldid = ExtractPacketId(pckt);
                int newid = transform.Invoke(oldid);
                string olds = oldid.ToString("X2");
                string news = newid.ToString("X2");
                pckt = pckt.Replace("0x"+olds, "0x"+news);
                ls[i] = pckt;
            }
            foreach (var item in ls)
            {
                Console.WriteLine(item);
            }
            static int ExtractPacketId(string sid)
            {
                return int.Parse(((string[])sid.Split(" ")).Last(a => a.Contains("0x")).Replace(';', ' ').Replace("0x", "").Replace(":", ""), NumberStyles.HexNumber);
            }
        }

        static void GenerateItemHandlerFor()
        {

        }
        struct blockstate
        {
            public string blockstatesname;
            public string[] possiblevalues;
        }
        static void save(string name)
        {
            Console.WriteLine(name);
            tosave.Add(name);
        }
        class EnumEnt
        {
            string name = "";
            List<string> possiblevalue = [];
        }
        enum FType
        {
            Enum,
            Int,
            Bool,
            Facing,
            Face,
            Axis,
        }

        class Strholder
        {
            public string strname = "";
            public List<(FType type, string name, string[]? possiblevalue)> fields = [];
        }
    }
}
