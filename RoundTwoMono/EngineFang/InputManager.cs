using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework;

namespace EngineFang
{


    class InputManager: Component, Updateable
    {
        public PlayerIndex playerNumber;
        GamePadState state, prevState;

        public delegate bool boolDel();
        public boolDel aPress, aRelease, bPress, bRelease, xPress, xRelease, yPress, yRelease, rbPress, rbRelease, lbPress, lbRelease;
        public boolDel rtPress, rtRelease, ltPress, ltRelease;
        public boolDel XAPress,YBPress;

        int bufferRepeatAmount;
        int bufferRepeatRemaining;

        boolDel bufferButton;

        public InputManager(PlayerIndex playerNumber) {
            this.playerNumber = playerNumber;
            bufferRepeatAmount = 6;
        }
        public void Update() {
            prevState = state;
            state = GamePad.GetState(playerNumber);
            
            
            // button pressed block
            if (prevState.Buttons.A == ButtonState.Released && state.Buttons.A == ButtonState.Pressed && aPress != null) {
                if (!XADoublePress())
                {
                    ExecuteButtonBuffer(aPress);
                }
            }
            if (prevState.Buttons.B == ButtonState.Released && state.Buttons.B == ButtonState.Pressed && bPress != null)
            {
                if (!YBDoublePress())
                {
                    ExecuteButtonBuffer(bPress);
                }
            }
            if (prevState.Buttons.X == ButtonState.Released && state.Buttons.X == ButtonState.Pressed &&  xPress != null)
            {
                if (!XADoublePress())
                {
                    ExecuteButtonBuffer(xPress);
                }
            }
            if (prevState.Buttons.Y == ButtonState.Released && state.Buttons.Y == ButtonState.Pressed && yPress != null)
            {
                if (!YBDoublePress())
                {
                    ExecuteButtonBuffer(yPress);
                }
            }
            if (prevState.Buttons.RightShoulder == ButtonState.Released && state.Buttons.RightShoulder == ButtonState.Pressed && rbPress != null)
            {
                ExecuteButtonBuffer(rbPress);
            }
            if (prevState.Buttons.LeftShoulder == ButtonState.Released && state.Buttons.LeftShoulder == ButtonState.Pressed && lbPress != null)
            {
                ExecuteButtonBuffer(lbPress);
            }
            if (prevState.Triggers.Right < .5  && state.Triggers.Right >= .5 && rtPress != null)
            {
                ExecuteButtonBuffer(rtPress);
            }
            if (prevState.Triggers.Left < .5 && state.Triggers.Left >= .5 && ltPress != null)
            {
                ExecuteButtonBuffer(ltPress);
            }

            // button released block
            if (state.Buttons.A == ButtonState.Released && prevState.Buttons.A == ButtonState.Pressed && aRelease != null)
            {
                ExecuteButtonBuffer(aRelease);
            }
            if (state.Buttons.B == ButtonState.Released && prevState.Buttons.B == ButtonState.Pressed && bRelease != null)
            {
                ExecuteButtonBuffer(bRelease);
            }
            if (state.Buttons.X == ButtonState.Released && prevState.Buttons.X == ButtonState.Pressed && xRelease != null)
            {
                ExecuteButtonBuffer(xRelease);
            }
            if (state.Buttons.Y == ButtonState.Released && prevState.Buttons.Y == ButtonState.Pressed && yRelease != null)
            {
                ExecuteButtonBuffer(yRelease);
            }
            if (state.Buttons.RightShoulder == ButtonState.Released && prevState.Buttons.RightShoulder == ButtonState.Pressed && rbRelease != null)
            {
                ExecuteButtonBuffer(rbRelease);
            }
            if (state.Buttons.LeftShoulder == ButtonState.Released && prevState.Buttons.LeftShoulder == ButtonState.Pressed && lbRelease != null)
            {
                ExecuteButtonBuffer(lbRelease);
            }
            if (state.Triggers.Right < .5 && prevState.Triggers.Right >= .5 && rtRelease != null)
            {
                ExecuteButtonBuffer(rtRelease);
            }
            if (state.Triggers.Left < .5 && prevState.Triggers.Left >= .5 && ltRelease != null)
            {
                ExecuteButtonBuffer(ltRelease);
            }

            if (bufferRepeatRemaining > 0 && bufferButton != null) {
                bool success = bufferButton();
                if (success)
                {
                    bufferRepeatRemaining = 0;
                }
                else
                {
                    bufferRepeatRemaining--;
                }
            }
        }

        void ExecuteButtonBuffer(boolDel button) {
            bool success = button();
            bufferButton = button;
            if (!success)
            {
                bufferRepeatRemaining = bufferRepeatAmount;
            }

        }
        // returns true if it is a successful input
        bool XADoublePress() {
            if (state.Buttons.A == ButtonState.Pressed && state.Buttons.X == ButtonState.Pressed) {
                ExecuteButtonBuffer( XAPress);
                return true;
            }
            return false;

        }
        // returns true if it is a successful input
        bool YBDoublePress()
        {
            if (state.Buttons.B == ButtonState.Pressed && state.Buttons.Y == ButtonState.Pressed)
            {
                ExecuteButtonBuffer(YBPress);
                return true;
            }
            return false;
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
