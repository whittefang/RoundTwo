using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework;

namespace RoundTwoMono
{


    class InputManager: Component, Updateable
    {
        public int playerNumber;
        GamePadState state, prevState;

        public delegate void voidDel();
        public voidDel aPress, aRelease, bPress, bRelease, xPress, xRelease, yPress, yRelease, rbPress, rbRelease, lbPress, lbRelease;
        public voidDel rtPress, rtRelease, ltPress, ltRelease;



        public InputManager(int playerNumber) {
            this.playerNumber = playerNumber;
        }
        public void Update() {
            prevState = state;
            state = GamePad.GetState(playerNumber);

            // button pressed block
            if (prevState.Buttons.A == ButtonState.Released && state.Buttons.A == ButtonState.Pressed && aPress != null) {
                aPress();
            }
            if (prevState.Buttons.B == ButtonState.Released && state.Buttons.B == ButtonState.Pressed && bPress != null)
            {
                bPress();
            }
            if (prevState.Buttons.X == ButtonState.Released && state.Buttons.X == ButtonState.Pressed &&  xPress != null)
            {
                xPress();
            }
            if (prevState.Buttons.Y == ButtonState.Released && state.Buttons.Y == ButtonState.Pressed && yPress != null)
            {
                yPress();
            }
            if (prevState.Buttons.RightShoulder == ButtonState.Released && state.Buttons.RightShoulder == ButtonState.Pressed && rbPress != null)
            {
                rbPress();
            }
            if (prevState.Buttons.LeftShoulder == ButtonState.Released && state.Buttons.LeftShoulder == ButtonState.Pressed && lbPress != null)
            {
                lbPress();
            }
            if (prevState.Triggers.Right < .5  && state.Triggers.Right >= .5 && rtPress != null)
            {
                rtPress();
            }
            if (prevState.Triggers.Left < .5 && state.Triggers.Left >= .5 && ltPress != null)
            {
                ltPress();
            }

            // button released block
            if (state.Buttons.A == ButtonState.Released && prevState.Buttons.A == ButtonState.Pressed && aRelease != null)
            {
                aRelease();
            }
            if (state.Buttons.B == ButtonState.Released && prevState.Buttons.B == ButtonState.Pressed && bRelease != null)
            {
                bRelease();
            }
            if (state.Buttons.X == ButtonState.Released && prevState.Buttons.X == ButtonState.Pressed && xRelease != null)
            {
                xRelease();
            }
            if (state.Buttons.Y == ButtonState.Released && prevState.Buttons.Y == ButtonState.Pressed && yRelease != null)
            {
                yRelease();
            }
            if (state.Buttons.RightShoulder == ButtonState.Released && prevState.Buttons.RightShoulder == ButtonState.Pressed && rbRelease != null)
            {
                rbRelease();
            }
            if (state.Buttons.LeftShoulder == ButtonState.Released && prevState.Buttons.LeftShoulder == ButtonState.Pressed && lbRelease != null)
            {
                lbRelease();
            }
            if (state.Triggers.Right < .5 && prevState.Triggers.Right >= .5 && rtRelease != null)
            {
                rtRelease();
            }
            if (state.Triggers.Left < .5 && prevState.Triggers.Left >= .5 && ltRelease != null)
            {
                ltRelease();
            }

        }
        public Vector2 GetRightStick()
        {
            return new Vector2(state.ThumbSticks.Right.X, state.ThumbSticks.Right.Y);
        }
        public Vector2 GetLeftStick()
        {
            return new Vector2(state.ThumbSticks.Left.X, state.ThumbSticks.Left.Y);
        }
    }


}
