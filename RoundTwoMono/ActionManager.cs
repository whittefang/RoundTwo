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
    class ActionManager: Component, Updateable
    {
        FighterStateHandler state;
        AttackPlayer attackPlayer;
        PlayerMovement playerMovement;
        SpriteAnimator<FighterAnimations> animator;
        Health health;
        
        int stunMovementRemaining, blockstunRemaining, hitstunRemaining, damageDelayRemaining;
        int groundRecoveryRemaining;
        int delayDamage;
        bool ignoreCornerPushback;
        Vector2 movementStepSize;

        public override void Load(ContentManager content)
        {

            attackPlayer = entity.getComponent<AttackPlayer>();
            animator = entity.getComponent<SpriteAnimator<FighterAnimations>>();
            playerMovement = entity.getComponent<PlayerMovement>();
            health = entity.getComponent<Health>();
            state = entity.getComponent<FighterStateHandler>();
        }

        public void StartAttack(Attack newAttack, FighterAnimations newAnimation, bool allowFlip = true)
        {
            if (allowFlip)
            {
                state.ProcessFacingDirection();
            }
            attackPlayer.StartAttack(newAttack, newAnimation);
        }
        public void StartDelayedDamage(int amount, int delay) {
            delayDamage = amount;
            damageDelayRemaining = delay;
        }
        public void StartHitstun(int hitstun, int stunMovement, Vector2 movementStepSize,bool ignoreCornerPushback) {
            stunMovementRemaining = stunMovement;
            hitstunRemaining = hitstun;
            this.movementStepSize = movementStepSize;
            this.ignoreCornerPushback = ignoreCornerPushback;
        }
        public void StartBlockstun(int blockStun, int stunMovement, Vector2 movementStepSize, bool ignoreCornerPushback)
        {
            stunMovementRemaining = stunMovement;
            blockstunRemaining = blockStun;
            this.movementStepSize = movementStepSize;
            this.ignoreCornerPushback = ignoreCornerPushback;

        }

        void ProcessDelayDamage() {
            // delayed damage for throws
            if (damageDelayRemaining > 0)
            {
                if (damageDelayRemaining == 1)
                {
                    health.DealDamage(delayDamage);
                }
                damageDelayRemaining--;
            }
        }
        void knockdownUpdate() {
            // process recovery time from knockdown
            if (groundRecoveryRemaining > 0)
            {
                groundRecoveryRemaining--;
                if (groundRecoveryRemaining == 0 && health.GetHealth() > 0)
                {
                    state.SetState(FighterState.neutral);
                }
            }
        }

        void HitstunUpdate()
        {
            if (state.GetState() == FighterState.hitstun)
            {
                // stun that the player is pushed back during
                if (stunMovementRemaining > 0)
                {
                    if (transform.position.X >= Camera.GetBound() || transform.position.X <= Camera.GetBound(false) && !ignoreCornerPushback)
                    {
                        playerMovement.otherPlayerMovement.MoveTowards(movementStepSize);
                    }
                    else {
                        playerMovement.MoveTowards(movementStepSize);
                    }
                    stunMovementRemaining--;
                }


                //regular hitstun
                if (hitstunRemaining <= 0)
                {
                    UIMatch.HideComboText(state.isPlayerOne());
                    state.SetState(FighterState.neutral);
                }
                hitstunRemaining--;
            }
            else if (state.GetState() == FighterState.airHitstun)
            {

                if (stunMovementRemaining > 0)
                {
                    playerMovement.MoveTowards(movementStepSize);
                    stunMovementRemaining--;
                }
                else {
                    playerMovement.MoveTowards(new Vector2(0, -10));
                    if (transform.position.Y <= playerMovement.groundBound)
                    {

                        // hitground
                        groundRecoveryRemaining = 80;
                        if (health.GetHealth() <= 0)
                        {
                            animator.PlayAnimation(FighterAnimations.deathKnockdown);
                        }
                        else
                        {
                            UIMatch.HideComboText(state.isPlayerOne());
                            animator.PlayAnimation(FighterAnimations.knockdown);
                        }
                        state.SetState(FighterState.invincible);
                    }
                }


            }
            else if (state.GetState() == FighterState.blockstun)
            {

                if (stunMovementRemaining > 0)
                {
                    playerMovement.MoveTowards(movementStepSize);
                    stunMovementRemaining--;
                }
                if (blockstunRemaining <= 0)
                {
                    state.SetState(FighterState.neutral);
                }
                blockstunRemaining--;
            }    
        }

        

        public void Update() {
            ProcessDelayDamage();
            HitstunUpdate();
            knockdownUpdate();
        }
        
        public void CancelActions() {

        }

    }
}
