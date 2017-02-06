using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using EngineFang;

namespace RoundTwoMono
{
    class Projectile: Component, Renderable, Updateable
    {
        
        SpriteAnimator<ProjectileAnim> animator;
        public Vector2 movementVector, SpawnPoint;
        public bool isActive = false;
        Hitbox hitData;
        HitResolver otherHitResolver;

        Texture2D hitboxTexture;
        Color hitboxColor;

        public delegate void voidDel();
        public voidDel optionalFunction;

        enum ProjectileAnim
        {
            active,
            inactive,
            dissipate
        }
        public Projectile(Hitbox  hitData){
            this.hitData = hitData;
            hitboxColor = new Color(Color.Red, .5f);
        }

        public void Load(ContentManager Content)
        {
            animator = new SpriteAnimator<ProjectileAnim>(new Vector2(-80, 70));
            entity.addComponent(animator);

            hitboxTexture = Content.Load<Texture2D>("square");
            
        }
        public void SetActiveAnimation(Animation anim) {
            anim.renderOneshotAfterCompletion = false;
            animator.addAnimation(ProjectileAnim.active, anim);
        }
        public void SetDissipateAnimation(Animation anim)
        {
            anim.renderOneshotAfterCompletion = false;
            animator.addAnimation(ProjectileAnim.dissipate, anim);
        }
        public void setOtherPlayer(ref Entity other)
        {
            otherHitResolver = other.getComponent<HitResolver>();
        }
        public void Activate(Vector3 position, bool travelLeft) {

            if ((!travelLeft && movementVector.X < 0 ) ||  (travelLeft && movementVector.X > 0)) {
                movementVector.X *= -1;
                SpawnPoint.X *= -1;
            }
            
            animator.Flip(travelLeft);
            

            transform.position = position +new Vector3( SpawnPoint.X, SpawnPoint.Y, 0);
            hitData.hitboxBounds = transform.GetRenderPosition(hitData.hitboxBounds);
            hitData.moveCurrentUseID++;
            animator.PlayAnimation(ProjectileAnim.active);
            isActive = true;
        }
        public void Dissipate() {
            // TODO fix for disipate animation to play out fully
            isActive = false;
            animator.PlayAnimation(ProjectileAnim.dissipate);
        }
        public void Update() {
            if (isActive)
            {
                // move projectile
                transform.Translate(movementVector);
                hitData.hitboxBounds = transform.GetRenderPosition(hitData.hitboxBounds);
                // check for hit
                if (otherHitResolver.CheckForHit(hitData)) {                    
                    Dissipate();
                    if (optionalFunction != null) {
                        optionalFunction();
                    }                    
                }
            }
        }

        public void Draw(SpriteBatch spriteBatch) {
            if (MasterObjectContainer.showHitboxes && isActive)
            {
                spriteBatch.Draw(hitboxTexture, hitData.hitboxBounds, hitboxColor);
            }
            
        }
    }
    
}
