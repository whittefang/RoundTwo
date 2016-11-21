using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;

namespace RoundTwoMono
{


    class PlayerMovement: Component, Updateable
    {
        InputManager input;
        public float speed;

        static float DeadSize = .15f;

        SpriteAnimator<FigherAnimations> spriteAnimator;
        FighterState state;

        public PlayerMovement() {
            state = FighterState.neutral;
            speed = 0;
        }
        public PlayerMovement(InputManager input, SpriteAnimator<FigherAnimations> spriteAnimator, float speed) {
            state = FighterState.neutral;
            this.input = input;
            this.speed = speed;
            this.spriteAnimator = spriteAnimator;
        }

        public void Update() {
            
            ProcessMovement();
            
        }

        // take in stick position and move character accordingly
        void ProcessMovement() {

            // quit out if not in the neutral state
            if (state != FighterState.neutral) {
                return;
            }

            Vector2 inputAxis = input.GetLeftStick();

            // left right movement block
            if (Math.Abs(inputAxis.X) > DeadSize)
            {
                int direction = 0;
                // check if left or right movement
                if (inputAxis.X > 0)
                {
                    spriteAnimator.PlayAnimation(FigherAnimations.walkToward);
                    direction = 1;
                }
                else
                {
                    spriteAnimator.PlayAnimation(FigherAnimations.walkBack);
                    direction = -1;
                }
                transform.Translate(new Vector2(direction * speed, 0));

            }
            else {
                spriteAnimator.PlayAnimation(FigherAnimations.neutral);
            }
            

            

            // jumping movement block
            if (inputAxis.Y > DeadSize)
            {

            }
        }

        public void SetInputManager(InputManager input) {
            this.input = input;
        }

        public FighterState GetState()
        {
            return state;
        }
        public void SetState(FighterState newState)
        {
            state = newState;
        }

    }

    // fighter character state enumerator
    enum FighterState
    {
        neutral,
        walk,
        jumping,
        invincible,
        hitstun,
        blockstun,
        projectileInvincible,
        attackStartup,
        attackRecovery,
        jumpingAttack
    };
}
