using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;

namespace RoundTwoMono
{
    static class MasterObjectContainer
    {
        public static Rectangle playerOneHurtbox, playerTwoHurtbox;
        public static bool showHitboxes = true;
        public static bool paused = false;
        public static bool advanceOneFrame = false;
        public static int hitstopRemaining = 0;
        static int currentMoveID = 0;

        public static Entity hitSparkHolder;
        public static SpriteAnimator<superFlash> superEffect;

        static public int GetMoveMasterID() {
            currentMoveID++;
            return currentMoveID;
        }
        
    }

    enum superFlash {
        none,
        super
    }
}
