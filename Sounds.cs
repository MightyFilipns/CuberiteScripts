using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CuberiteScripts
{
    public class Sounds
    {
        public static void EnumToString(string data_path)
        {
            var sounds = Utils.GetSounds(data_path);
            int max = sounds.Max(a => a.name.FormatNameNoPrefixSound().Length) + 1;
            foreach (var sound in sounds)
            {
                var enum_name = sound.name.FormatNameNoPrefixSound();
                Utils.save($"case SoundEvent::{enum_name}:{Utils.Spacing(max - enum_name.Length)} return \"{sound.name}\";");
            }
        }
    }
}
