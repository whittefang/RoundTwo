using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;

namespace RoundTwoMono
{
    static class MasterSound
    {
        public static SoundEffect hitLight, hitMedium, hitHard;
        public static SoundEffect block, grab, fireball, super;

        public static void Load(ContentManager content) {
            hitLight = content.Load<SoundEffect>("3rdStrikeSounds/hitLight");
            hitMedium = content.Load<SoundEffect>("3rdStrikeSounds/hitMedium");
            hitHard = content.Load<SoundEffect>("3rdStrikeSounds/hitHeavy");

            block = content.Load<SoundEffect>("3rdStrikeSounds/block");
            grab = content.Load<SoundEffect>("3rdStrikeSounds/Grab");
            fireball = content.Load<SoundEffect>("3rdStrikeSounds/fireball");
            super = content.Load<SoundEffect>("3rdStrikeSounds/super");
        }
    }
}
