using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace RoundTwoMono
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

        public SpriteAnimator() {
            animations = new Dictionary<TEnum, Animation>();
            currentTimeBetweenFrame = 0;

        }

        public void addAnimation(TEnum key, Animation newAnimation) {
            animations.Add( key, newAnimation);
        }
        public void PlayAnimation(TEnum animKey) {
            if (currentAnimation != animations[animKey])
            {
                currentAnimation = animations[animKey];
                frameToRender = currentAnimation.playFromBeginning();
                isPlaying = true;
            }
        }
        public void Update() {
            if (isPlaying) {
                if (currentTimeBetweenFrame > currentAnimation.timeBetweenFrames)
                {
                    currentTimeBetweenFrame = 0;
                    frameToRender = currentAnimation.getNext();
                    adjustedOrigin = new Vector2(frameToRender.Width / 2, frameToRender.Height / 2);
                }
                else {
                    currentTimeBetweenFrame++;
                }
            }
        }
        public void Draw(SpriteBatch spriteBatch) {
            if (frameToRender != null)
            {
                spriteBatch.Draw(frameToRender, transform.getPositionV2() - adjustedOrigin, Color.White);
            }
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
        sp3
    }
}
