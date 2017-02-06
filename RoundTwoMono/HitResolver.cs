using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using EngineFang;
namespace RoundTwoMono
{
    class HitResolver: Component, Updateable, Renderable
    {
        Health health;
        // hitboxes
        public Rectangle hurtbox;
        Vector2 hurtboxOrigin;
        Vector2 hurtboxOffset;

        Texture2D hurtboxTexture;

        Hitbox previousHitData;
        SuperMeter superMeter;
        SpriteAnimator<FighterAnimations> animator;
        AttackPlayer attackPlayer;
        ActionManager actionManager;

        FighterStateHandler state;

        ObjectPool lightHitSparks;
        ObjectPool mediumHitSparks;
        ObjectPool heavyHitSparks;
        ObjectPool specialHitSparks;
        ObjectPool blockHitSparks;

        Attack chunLiThrow;

        public HitResolver() {
            hurtbox = new Rectangle(0, 0, 40, 90);
            hurtboxOrigin = new Vector2(hurtbox.Width / 2, hurtbox.Height);
            hurtboxOffset = new Vector2(0, 0);
        }
        public void SetThrows(Attack chunThrow) {
            chunLiThrow = chunThrow;
        }
        public override void Load(ContentManager content)
        {

            superMeter = entity.getComponent<SuperMeter>();
            animator = entity.getComponent<SpriteAnimator<FighterAnimations>>();
            attackPlayer = entity.getComponent<AttackPlayer>();
            actionManager = entity.getComponent<ActionManager>();
            state = entity.getComponent<FighterStateHandler>();
            health = entity.getComponent<Health>();


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

        // checkif hitboxs and hurtboxes are overlapping hit occured
        public bool CheckForHit(Hitbox hitdata) {
           
            Rectangle hitUnion = Rectangle.Intersect(hitdata.hitboxBounds, hurtbox);
            Vector2 hitPoint = new Vector2(hitUnion.X - hitUnion.Width / 2, hitUnion.Y - hitUnion.Height / 2);

            bool hit = false;
            // deal damage
            if (hitdata.hitboxBounds.Intersects(hurtbox)) {
                hit =  ProcessHit(hitdata, hitPoint);
            }

            return hit;
        }

        //
        // returns true if attack is successful and false if it has 'missed' eg invincible 
        public bool ProcessHit(Hitbox hitData, Vector2 hitPoint)
        {
            bool successfulHit = true;

            if (!(hitData.moveMasterID == previousHitData.moveMasterID && hitData.moveCurrentUseID == previousHitData.moveCurrentUseID))
            {
                if (hitData.attackProperty == AttackProperty.Throw && (state.GetState() != FighterState.jumping || state.GetState() != FighterState.jumpingAttack
                    || state.GetState() != FighterState.hitstun || state.GetState() != FighterState.blockstun))
                {
                    // successful throw
                    // play throw animation and attack

                    actionManager.StartHitstun(hitData.hitstun,0,Vector2.Zero, true);
                    
                    
                    state.SetState(FighterState.invincible);
                    if (hitData.throwType == ThrowType.chun)
                    {
                        attackPlayer.StartAttack(chunLiThrow, FighterAnimations.chunliThrow);                        
                        actionManager.StartDelayedDamage(hitData.damage, 30);
                    }

                    // deal damage on delay

                    // play sound
                    MasterSound.grab.Play();


                }
                else if (state.GetState() == FighterState.neutral && state.CheckIfBlocking() || state.GetState() == FighterState.blockstun)
                {
                    // successful blocking
                    actionManager.CancelActions();
                    actionManager.StartBlockstun(hitData.hitstun, hitData.pushbackDuration, hitData.pushback, hitData.ignorePushback);

                    state.SetState(FighterState.blockstun);
                    health.DealDamage(hitData.chipDamage, true);
                    animator.PlayAnimation(FighterAnimations.blocking);
                }
                else if (state.GetState() != FighterState.invincible)
                {
                    // successful hit

                    actionManager.CancelActions();
                    MasterObjectContainer.hitstopRemaining = hitData.hitStop;
                    actionManager.StartHitstun(hitData.hitstun, hitData.pushbackDuration, hitData.pushback, hitData.ignorePushback);

                    int amount = health.DealDamage(hitData.damage);
                    superMeter.AddMeter((int)(hitData.damage * 1.3f));


                    if (state.GetState() == FighterState.jumping || state.GetState() == FighterState.jumpingAttack || state.GetState() == FighterState.airHitstun || hitData.attackProperty == AttackProperty.Launcher || health.GetHealth() <= 0)
                    {
                        state.SetState(FighterState.airHitstun);
                        animator.PlayAnimation(FighterAnimations.airHit, true);
                    }
                    else {
                        state.SetState(FighterState.hitstun);
                        animator.PlayAnimation(FighterAnimations.hit, true);
                    }

                    

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
            previousHitData = hitData;

            return successfulHit;
        }

        public void Update() {
            hurtbox = transform.GetRenderPosition(hurtbox, TransformOriginPoint.bottom);
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

    }
}
