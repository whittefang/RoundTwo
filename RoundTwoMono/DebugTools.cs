using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework.Input;
using EngineFang;

namespace RoundTwoMono
{
    class DebugTools
    {
        KeyboardState state, preState;
        public void Update() {
            preState = state;
            state = Keyboard.GetState();

            // switch for hitboxes
            if (state.IsKeyDown(Keys.F1) && preState.IsKeyUp(Keys.F1)) {
                MasterObjectContainer.showHitboxes = !MasterObjectContainer.showHitboxes;
            }

            // switch for pausing game
            if (state.IsKeyDown(Keys.Space) && preState.IsKeyUp(Keys.Space))
            {
                MasterObjectContainer.paused = !MasterObjectContainer.paused;
            }

            if (state.IsKeyDown(Keys.Right) && preState.IsKeyUp(Keys.Right))
            {
                MasterObjectContainer.advanceOneFrame = true;
            }

        }
    }
}
