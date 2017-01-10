using System;
using System.Diagnostics;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;
using RoundTwoMono.EngineFang;

namespace RoundTwoMono
{
    class Health : Component, Updateable, Renderable
    {
        float maximumHealth;
        float currentHealth;
        float comboProration;

        public Rectangle hurtbox;
        Vector2 hurtboxOrigin;
        Vector2 hurtboxOffset;

        PlayerMovement playerMovement;
        SpriteAnimator<FigherAnimations> animator;

        int hitstunRemaining, blockstunRemaining, stunMovementRemaining, recoveryRemaining;
        Vector2 movementStepSize;
        Texture2D hurtboxTexture;

        Hitbox previousAttack;

        Texture2D healthBar, healthBarArt;
        Rectangle healthBarRect;

        SpriteFont font;
        ObjectPool lightHitSparks;
        ObjectPool mediumHitSparks;
        ObjectPool heavyHitSparks;
        ObjectPool specialHitSparks;
        ObjectPool blockHitSparks;
        bool playerOne = false;
        bool displayComboData, recoveryPeriod;
        public int comboHits, comboDamage, comboFadeBuffer;

        SuperMeter superMeter;

        public Health(float maxHealth, PlayerMovement playerMovement, SpriteAnimator<FigherAnimations> animator, PlayerIndex playerNumber, SuperMeter superMeter)
        {
            maximumHealth = maxHealth;
            currentHealth = maxHealth;
            this.playerMovement = playerMovement;
            this.animator = animator;
            this.superMeter = superMeter;
            hurtbox = new Rectangle(0, 0, 40, 90);
            hurtboxOrigin = new Vector2(hurtbox.Width / 2, hurtbox.Height);
            hurtboxOffset = new Vector2(0, 0);
            comboHits = 0;
            comboDamage = 0;
            displayComboData = true;
            if (playerNumber == PlayerIndex.Two)
            {
                playerOne = true;
                healthBarRect = new Rectangle(725, 425, 100, 5);
            }
            else
            {
                healthBarRect = new Rectangle(990, 425, 200, 5);
            }
        }

        // returns true if attack is successful and false if it has 'missed' eg invincible 
        public bool ProcessHit(Hitbox hitData, Vector2 hitPoint)
        {
            bool successfulHit = true;

            if (!(hitData.moveMasterID == previousAttack.moveMasterID && hitData.moveCurrentUseID == previousAttack.moveCurrentUseID))
            {
                if (hitData.attackProperty == AttackProperty.Throw && (playerMovement.GetState() != FighterState.jumping || playerMovement.GetState() != FighterState.jumpingAttack
                    || playerMovement.GetState() != FighterState.hitstun || playerMovement.GetState() != FighterState.blockstun))
                {
                    // successful throw

                }
                else if (playerMovement.GetState() == FighterState.neutral && playerMovement.CheckIfBlocking() || playerMovement.GetState() == FighterState.blockstun)
                {
                    // successful blocking
                    playerMovement.CancelActions();
                    blockstunRemaining = hitData.blockstun;
                    stunMovementRemaining = 5;
                    movementStepSize = new Vector2(hitData.pushback.X, 0);
                    playerMovement.SetState(FighterState.blockstun);
                    DealDamage(hitData.chipDamage);
                    animator.PlayAnimation(FigherAnimations.blocking);
                }
                else if (playerMovement.GetState() != FighterState.invincible)
                {
                    // successful hit

                    playerMovement.CancelActions();
                    hitstunRemaining = hitData.hitstun;
                    stunMovementRemaining = hitData.pushbackDuration;
                    movementStepSize = hitData.pushback;

                    DealDamage(hitData.damage * comboProration);
                    superMeter.AddMeter((int)(hitData.damage * comboProration * 1.3f));
                    if (playerMovement.GetState() == FighterState.hitstun)
                    {
                        // in combo
                        if (comboProration > .4f)
                        {
                            comboProration -= .1f;
                        }
                    }
                    else {
                        comboHits = 1;
                        comboDamage = hitData.damage;
                        comboProration = 1;
                    }

                    if (playerMovement.GetState() == FighterState.jumping || playerMovement.GetState() == FighterState.jumpingAttack || playerMovement.GetState() == FighterState.airHitstun || hitData.attackProperty == AttackProperty.Launcher || currentHealth <= 0)
                    {
                        playerMovement.SetState(FighterState.airHitstun);
                        animator.PlayAnimation(FigherAnimations.airHit, true);
                    }
                    else {
                        playerMovement.SetState(FighterState.hitstun);
                        animator.PlayAnimation(FigherAnimations.hit, true);
                    }

                    
                    MasterObjectContainer.hitstopRemaining = hitData.hitStop;
                    displayComboData = true;
                    comboFadeBuffer = 30;

                    if (hitData.attackStrength == HitSpark.light)
                    {
                        lightHitSparks.Get().Play(hitPoint);
                    }
                    else if (hitData.attackStrength == HitSpark.medium)
                    {
                        mediumHitSparks.Get().Play(hitPoint);
                    }
                    else if (hitData.attackStrength == HitSpark.heavy)
                    {
                        heavyHitSparks.Get().Play(hitPoint);
                    }
                    else if (hitData.attackStrength == HitSpark.special)
                    {
                        specialHitSparks.Get().Play(hitPoint);
                    }
                    // cancel other active moves
                    // set state to hitstun
                    // 

                }
                else {
                    successfulHit = false;
                }
            }
            else
            {
                successfulHit = false;

            }
            previousAttack = hitData;

            return successfulHit;
        }

        bool DealDamage(float amount)
        {

            comboDamage += (int)amount;
            comboHits++;

            currentHealth -= amount;

            Debug.WriteLine(currentHealth);
            if (currentHealth <= 0)
            {
                // resolve death
                playerMovement.otherPlayerMovement.PlayWin();
                MasterObjectContainer.EndRound(playerOne);
                return true;
            }
            return false;

        }

        public void Update()
        {
            // move hurtbox
            hurtbox.X = (int)(transform.position.X - hurtboxOrigin.X - hurtboxOffset.X);
            hurtbox.Y = (int)(transform.position.Y - hurtboxOrigin.Y - hurtboxOffset.Y);

            // hitstun resolver
            HitstunUpdate();

            HealthbarUpdate();

            // process recovery time from knockdown
            if (recoveryRemaining > 0) {
                recoveryRemaining--;
                if (recoveryRemaining == 0 && currentHealth > 0) {
                    playerMovement.SetState(FighterState.neutral);
                }
            }
        }
        public void ResetHealth() {
            currentHealth = maximumHealth;

        }
        void HealthbarUpdate() {
            healthBarRect.Width = (int)(200f * (currentHealth / maximumHealth));
        }

        void HitstunUpdate() {
            if (playerMovement.GetState() == FighterState.hitstun)
            {

                if (stunMovementRemaining > 0)
                {
                    playerMovement.MoveTowards(movementStepSize);
                    stunMovementRemaining--;
                }

                if (hitstunRemaining <= 0)
                {
                    playerMovement.SetState(FighterState.neutral);
                }
                hitstunRemaining--;
            }
            else if (playerMovement.GetState() == FighterState.airHitstun)
            {

                if (stunMovementRemaining > 0)
                {
                    playerMovement.MoveTowards(movementStepSize);
                    stunMovementRemaining--;
                }
                else {
                    playerMovement.MoveTowards(new Vector2(0, 10));
                    if (transform.position.Y >= playerMovement.groundBound)
                    {
                        // hitground
                        recoveryRemaining = 80;
                        if (currentHealth <= 0)
                        {
                            animator.PlayAnimation(FigherAnimations.deathKnockdown);
                        }
                        else {
                            animator.PlayAnimation(FigherAnimations.knockdown);
                        }
                        playerMovement.SetState(FighterState.invincible);
                    }
                }


            }
            else if (playerMovement.GetState() == FighterState.blockstun)
            {

                if (stunMovementRemaining > 0)
                {
                    playerMovement.MoveTowards(movementStepSize);
                    stunMovementRemaining--;
                }
                if (blockstunRemaining <= 0)
                {
                    playerMovement.SetState(FighterState.neutral);
                }
                blockstunRemaining--;
            }
            else if (comboFadeBuffer > 0)
            {
                comboFadeBuffer--;
            }
            else {
                displayComboData = false;
            }
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            if (MasterObjectContainer.showHitboxes)
            {
                spriteBatch.Draw(hurtboxTexture, hurtbox, new Color(Color.Green, .5f));
            }

            if (displayComboData)
            {
                spriteBatch.DrawString(font, comboHits.ToString(), new Vector2(800, 425), Color.Black,0, Vector2.Zero, .25f,SpriteEffects.None, 0);
                spriteBatch.DrawString(font, comboDamage.ToString(), new Vector2(800, 450), Color.Black, 0, Vector2.Zero, .25f, SpriteEffects.None, 0);

            }

            spriteBatch.Draw(healthBar, healthBarRect, Color.Green);
        }

        public void load(ContentManager content)
        {

            hurtboxTexture = content.Load<Texture2D>("square");
            healthBar = content.Load<Texture2D>("square");
            font = content.Load<SpriteFont>("arial");
            foreach (ObjectPool h in MasterObjectContainer.hitSparkHolder.getComponents<ObjectPool>())
            {
                if (h.type == HitSpark.light)
                {
                    lightHitSparks = h;
                }
                else if (h.type == HitSpark.medium)
                {
                    mediumHitSparks = h;
                }
                else if (h.type == HitSpark.heavy)
                {
                    heavyHitSparks = h;
                }
                else if (h.type == HitSpark.special)
                {
                    specialHitSparks = h;
                }
                else if (h.type == HitSpark.block)
                {
                    blockHitSparks = h;
                }
            }
        }
    }
}
