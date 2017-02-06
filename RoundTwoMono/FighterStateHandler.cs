using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EngineFang;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;

namespace RoundTwoMono
{
    class FighterStateHandler: Component, Updateable
    {
        FighterState currentState;
        SpriteAnimator<FighterAnimations> animator;
        InputManager input;
        Entity otherPlayer;
        AttackPlayer attackPlayer;

        bool playerOne;
        bool isFacingLeft;

        public FighterStateHandler(PlayerIndex index) {
            if (index == PlayerIndex.Two) {
                playerOne = false;
            }
            else
            {
                playerOne = true;
            }

            
        }

        public override void Load(ContentManager content)
        {

            input = entity.getComponent<InputManager>();
            attackPlayer = entity.getComponent<AttackPlayer>();
            animator = entity.getComponent<SpriteAnimator<FighterAnimations>>();

            if (!playerOne)
            {
                transform.flipRenderingHorizontal = true;
                isFacingLeft = true;
                animator.Flip(true);
                attackPlayer.SetAttackDirection(true);
            }
            else {
                transform.flipRenderingHorizontal = false;
                isFacingLeft = false;
                animator.Flip(false);
                attackPlayer.SetAttackDirection(false);
            }
        }

        public bool isPlayerOne() {
            return playerOne;
        }
        public FighterStateHandler() {

            currentState = FighterState.neutral;
        }

        public FighterState GetState() {
            return currentState;
        }

        public void SetState(FighterState newState)
        {
            currentState = newState;
        }
        public void SetOtherPlayer(Entity otherPlayer) {
            this.otherPlayer = otherPlayer;
            
        }
        public bool CheckIfBlocking()
        {
            if (isFacingLeft && input.GetLeftStick().X > 0)
            {
                return true;
            }
            else if (!isFacingLeft && input.GetLeftStick().X < 0)
            {
                return true;
            }
            return false;
        }

        // handle facing direction updates
        public void ProcessFacingDirection()
        {
            if (transform.position.X > otherPlayer.transform.position.X && !isFacingLeft)
            {
                transform.flipRenderingHorizontal = true;
                isFacingLeft = true;
                animator.Flip(true);
                attackPlayer.SetAttackDirection(true);
            }
            else if (transform.position.X < otherPlayer.transform.position.X && isFacingLeft)
            {
                transform.flipRenderingHorizontal = false;
                isFacingLeft = false;
                animator.Flip(false);
                attackPlayer.SetAttackDirection(false);
            }
        }
        public bool GetFacingDirection() {
            return isFacingLeft;
        }

        public void Update()
        {
            if (GetState() == FighterState.neutral && otherPlayer != null)
            {
                ProcessFacingDirection();
            }
        }

    }
}
