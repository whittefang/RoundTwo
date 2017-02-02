﻿using System;
using System.Diagnostics;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;
using EngineFang;

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

        FighterSound playerSound;

        PlayerMovement playerMovement;
        SpriteAnimator<FighterAnimations> animator;

        int hitstunRemaining, blockstunRemaining, stunMovementRemaining, recoveryRemaining;
        Vector2 movementStepSize;
        Texture2D hurtboxTexture;

        Hitbox previousAttack;

        bool ignoreCornerPushback = true;



        Attack chunLiThrow;


        ObjectPool lightHitSparks;
        ObjectPool mediumHitSparks;
        ObjectPool heavyHitSparks;
        ObjectPool specialHitSparks;
        ObjectPool blockHitSparks;
        bool playerOne = false;
        bool  recoveryPeriod;
        public int comboHits, comboDamage, comboFadeBuffer;

        SuperMeter superMeter;

        public Health(float maxHealth, PlayerMovement playerMovement, SpriteAnimator<FighterAnimations> animator, PlayerIndex playerNumber, SuperMeter superMeter, FighterSound fighterSound)
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
            if (playerNumber == PlayerIndex.Two)
            {
                playerOne = true;
            }
            playerSound = fighterSound;
           
        }

        public void SetThrows(Attack chunThrow) {
            chunLiThrow = chunThrow;
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
                    // play throw animation and attack
                    if (hitData.throwType == ThrowType.chun) {
                        playerMovement.StartAttack(chunLiThrow, FighterAnimations.chunliThrow);
                    }
                   
                    // deal damage on delay

                    // play sound
                    MasterSound.grab.Play();


                }
                else if (playerMovement.GetState() == FighterState.neutral && playerMovement.CheckIfBlocking() || playerMovement.GetState() == FighterState.blockstun)
                {
                    // successful blocking
                    playerMovement.CancelActions();
                    blockstunRemaining = hitData.blockstun;
                    stunMovementRemaining = 5;
                    ignoreCornerPushback = hitData.ignorePushback;
                    movementStepSize = new Vector2(hitData.pushback.X, 0);
                    playerMovement.SetState(FighterState.blockstun);
                    DealDamage(hitData.chipDamage, true);
                    animator.PlayAnimation(FighterAnimations.blocking);
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

                    ignoreCornerPushback = hitData.ignorePushback;

                    UIMatch.ComboTextUpdate(comboDamage, comboHits, playerOne);

                    if (playerMovement.GetState() == FighterState.jumping || playerMovement.GetState() == FighterState.jumpingAttack || playerMovement.GetState() == FighterState.airHitstun || hitData.attackProperty == AttackProperty.Launcher || currentHealth <= 0)
                    {
                        playerMovement.SetState(FighterState.airHitstun);
                        animator.PlayAnimation(FighterAnimations.airHit, true);
                    }
                    else {
                        playerMovement.SetState(FighterState.hitstun);
                        animator.PlayAnimation(FighterAnimations.hit, true);
                    }

                    
                    MasterObjectContainer.hitstopRemaining = hitData.hitStop;
                    comboFadeBuffer = 30;

                    ///playerSound.hit.Play();

                    if (hitData.attackStrength == HitSpark.light)
                    {
                        lightHitSparks.Get().Play(hitPoint);
                        MasterSound.hitLight.Play();
                    }
                    else if (hitData.attackStrength == HitSpark.medium)
                    {
                        mediumHitSparks.Get().Play(hitPoint);
                        MasterSound.hitMedium.Play();
                    }
                    else if (hitData.attackStrength == HitSpark.heavy)
                    {
                        heavyHitSparks.Get().Play(hitPoint);
                        MasterSound.hitHard.Play();
                    }
                    else if (hitData.attackStrength == HitSpark.special)
                    {
                        specialHitSparks.Get().Play(hitPoint);
                        MasterSound.hitHard.Play();
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

        bool DealDamage(float amount, bool chip = false)
        {

            comboDamage += (int)amount;
            comboHits++;

            currentHealth -= amount;

            Debug.WriteLine(currentHealth);
            if (currentHealth <= 0)
            {
                if (!chip)
                {
                    // resolve death
                    playerMovement.otherPlayerMovement.PlayWin();
                    MasterObjectContainer.EndRound(playerOne);
                    return true;
                }
            }
            return false;

        }

        public void Update()
        {
            // move hurtbox

            hurtbox = transform.GetRenderPosition(hurtbox, TransformOriginPoint.bottom);
            //hurtbox.X = (int)(transform.position.X - hurtboxOrigin.X - hurtboxOffset.X);
            //hurtbox.Y = (int)(transform.position.Y - hurtboxOrigin.Y - hurtboxOffset.Y);

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
            UIMatch.HealthbarUpdate((currentHealth / maximumHealth), playerOne);
        }

        void HitstunUpdate() {
            if (playerMovement.GetState() == FighterState.hitstun)
            {

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
                    playerMovement.MoveTowards(new Vector2(0, -10));
                    if (transform.position.Y <= playerMovement.groundBound)
                    {
                        // hitground
                        recoveryRemaining = 80;
                        if (currentHealth <= 0)
                        {
                            animator.PlayAnimation(FighterAnimations.deathKnockdown);
                        }
                        else {
                            animator.PlayAnimation(FighterAnimations.knockdown);
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
                UIMatch.HideComboText(playerOne);
            }
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            if (MasterObjectContainer.showHitboxes)
            {
                spriteBatch.Draw(hurtboxTexture, hurtbox, new Color(Color.Green, .5f));
            }

           

            //spriteBatch.Draw(healthBar, healthBarRect, Color.Green);


            spriteBatch.Draw(hurtboxTexture, new Rectangle(-5, -5, 10, 10), Color.Yellow);

        }

        public void load(ContentManager content)
        {

            hurtboxTexture = content.Load<Texture2D>("square");
            
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
