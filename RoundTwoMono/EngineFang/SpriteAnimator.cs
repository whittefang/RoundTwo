using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace EngineFang
{
    class SpriteAnimator<TEnum> : Component, Updateable, Renderable
    {
        public TEnum AnimationKey {
            get { return currentAnimationIndex; }
            set { }
        }

        int currentTimeBetweenFrame;
        bool isPlaying = false;

        TEnum currentAnimationIndex;
        Animation currentAnimation;
        Dictionary<TEnum, Animation> animations;
        Texture2D frameToRender;
        Vector2 adjustedOrigin;
        Vector2 renderOffset;
        Vector2 renderDirection;

        public SpriteAnimator() {
            animations = new Dictionary<TEnum, Animation>();
            currentTimeBetweenFrame = 0;
            renderDirection = new Vector2(1,1);

        }
        public SpriteAnimator(Vector2 renderOffset) : this()
        {
            this.renderOffset = renderOffset;
        }

        public void addAnimation(TEnum key, Animation newAnimation) {
            animations.Add( key, newAnimation);
        }
        public void PlayAnimation(TEnum animKey, bool force = false) {
            if (currentAnimation != animations[animKey] || force)
            {
                currentAnimation = animations[animKey];
                frameToRender = currentAnimation.playFromBeginning();
                adjustedOrigin = new Vector2(frameToRender.Width / 2, frameToRender.Height / 2);
                currentTimeBetweenFrame = 0;
                isPlaying = true;
            }
        }
        public void Update() {
            if (isPlaying) {
                if (currentTimeBetweenFrame > currentAnimation.timeBetweenFrames)
                {
                    currentTimeBetweenFrame = 0;
                    frameToRender = currentAnimation.getNext();
                    if (frameToRender != null)
                    {
                        adjustedOrigin = new Vector2(frameToRender.Width / 2, frameToRender.Height / 2);
                        currentTimeBetweenFrame++;
                    }
                    else {
                        isPlaying = false;
                    }
                }
                else {
                    currentTimeBetweenFrame++;
                }
            }
        }
        public void Draw(SpriteBatch spriteBatch) {
            if (frameToRender != null )
            {
                //spriteBatch.Draw(frameToRender, transform.getPositionV2() - adjustedOrigin - (renderOffset * renderDirection), color: Color.White, effects: transform.GetHorizontalFlip());
                spriteBatch.Draw(frameToRender, transform.GetRenderPosition(frameToRender) - (renderOffset * renderDirection), color: Color.White, effects: transform.GetHorizontalFlip());

            }
        }
        public void Flip(bool isFacingLeft) {
            if (isFacingLeft)
            {
                renderDirection.X = -1;
            }
            else
            {
                renderDirection.X = 1;
            }
            transform.flipRenderingHorizontal = isFacingLeft;
        }
    }
    enum FigherAnimations
    {
        neutral,
        walkToward,
        walkBack,
        light,
        medium,
        heavy,
        sp1,
        sp2,
        sp3,
        jumpLight,
        jumpMedium,
        jumpHeavy,
        jumpRising,
        jumpDecending,
        jumpLanding,
        hit,
        airHit,
        knockdown,
        blocking,
        throwTry,
        throwComplete,
        Super,
        intro,
        win,
        deathKnockdown

    }
}
