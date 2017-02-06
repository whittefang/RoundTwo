using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EngineFang;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace RoundTwoMono
{
    class AttackPlayer: Component, Updateable, Renderable
    {
        Attack currentAttack;
        FighterStateHandler state;
        SpriteAnimator<FighterAnimations> animator;
        bool isAttacking;
        Vector2 attackDirection;

        public AttackPlayer() {
            attackDirection = new Vector2(1, 0);
        }

        public override void Load(ContentManager content)
        {
            animator = entity.getComponent<SpriteAnimator<FighterAnimations>>();
            state = entity.getComponent<FighterStateHandler>();
        }

        // handle beginning of attack command
        public void StartAttack(Attack newAttack, FighterAnimations newAnimation)
        {
            
            currentAttack = newAttack;
            state.SetState(newAttack.state);
            animator.PlayAnimation(newAnimation, true);

            currentAttack.Start();
            isAttacking = true;
        }

        public void CancelAttacks() {
            
        }

        // handle attack updates
        void ProcessAttacking()
        {
            if (isAttacking)
            {
                bool finished = currentAttack.NextStep(attackDirection);
                if (finished)
                {
                    isAttacking = false;
                    if (!currentAttack.isJumpingAttack)
                    {
                        state.SetState(FighterState.neutral);
                    }
                }
            }
        }

        public void SetAttackDirection(bool faceLeft) {
            int direction;
            if (faceLeft)
            {
                direction = -1;
            }
            else
            {
                direction = 1;
            }
            attackDirection.X = direction;
        }

        public void Update()
        {
            ProcessAttacking();
        }
        public void Draw(SpriteBatch spriteBatch)
        {
            if (currentAttack != null && isAttacking)
            {
                currentAttack.Draw(spriteBatch);
            }
        }
    }
}
