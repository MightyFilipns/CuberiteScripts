using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CuberiteScripts
{
    public class FluidGen
    {
        public static void StringToFluid(string file_path)
        {
            var fluids = Utils.GetFluids(file_path);
            int max = fluids.Max(a => a.name.RemovePrefix().Length);
            foreach (var (name, protocolId) in fluids)
            {
                Utils.save($"{{ \"{name.RemovePrefix()}\",{Utils.Spacing(max - name.RemovePrefix().Length)} FluidType::{name.FormatNameNoPrefix()}}},");
            }
        }

        public static void FluidToString(string file_path)
        {
            var fluids = Utils.GetFluids(file_path);
            int max = fluids.Max(a => a.name.FormatNameNoPrefix().Length);
            foreach (var (name, protocolId) in fluids)
            {
                Utils.save($"case FluidType::{name.FormatNameNoPrefix()}:{Utils.Spacing(max - name.FormatNameNoPrefix().Length)} return \"{name.RemovePrefix()}\";");
            }
        }

        public static void FluidToProtocol(string file_path)
        {
            var fluids = Utils.GetFluids(file_path);
            int max = fluids.Max(a => a.name.FormatNameNoPrefix().Length);
            foreach (var (name, protocolId) in fluids)
            {
                Utils.save($"case FluidType::{name.FormatNameNoPrefix()}:{Utils.Spacing(max - name.FormatNameNoPrefix().Length)} return {protocolId};");
            }
        }
    }
}
