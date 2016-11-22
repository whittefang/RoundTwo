using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework.Graphics;

namespace RoundTwoMono
{
    class Animation
    {
        struct Frame {
            public Frame(int timeTillNext, Texture2D sprite) {
                this.timeTillNext = timeTillNext;
                this.sprite = sprite;
            }
            public int timeTillNext;
            public Texture2D sprite;
        }
        List<Frame> frames;
        public int timeBetweenFrames;
        int currentFrame;
        bool animationDirection;
        animationType animType;

        public Animation() {
            frames = new List<Frame>();
            currentFrame = 0;
            timeBetweenFrames = 1;
            animType = animationType.oneShot;
            animationDirection = true;
        }

        public Animation(animationType animType)
        {
            frames = new List<Frame>();
            currentFrame = 0;
            timeBetweenFrames = 0;
            this.animType = animType;
            animationDirection = true;
        }

        public void addFrame(Texture2D newFrame, int timeTillNextFrame) {
            frames.Add(new Frame(timeTillNextFrame, newFrame));
        }
        public Texture2D playFromBeginning() {
            if (animType == animationType.looping || animType == animationType.oneShot || animType == animationType.pingPong)
            {
                currentFrame = 0;
                timeBetweenFrames = frames[currentFrame].timeTillNext;
            }
            else if (animType == animationType.reverseLoop){
                currentFrame = frames.Count - 1;
                timeBetweenFrames = frames[currentFrame].timeTillNext;
            }

            return frames[currentFrame].sprite;
        }
        public Texture2D getNext() {

            if (animType == animationType.looping) { 
                currentFrame++;
                if (currentFrame >= frames.Count) {
                    currentFrame = 0;
                }
            }
            else if (animType == animationType.oneShot)
            {
                currentFrame++;
                if (currentFrame >= frames.Count)
                {
                    currentFrame = frames.Count - 1;
                }
            }
            else if (animType == animationType.pingPong)
            {
                if (animationDirection)
                {
                    currentFrame++;
                    if (currentFrame >= frames.Count)
                    {
                        currentFrame = frames.Count - 1;
                        animationDirection = false;
                    }
                }
                else {
                    currentFrame--;
                    if (currentFrame <= 0)
                    {
                        currentFrame = 1;
                        animationDirection = true;
                    }
                }
            }
            else if (animType == animationType.reverseLoop)
            {
                currentFrame--;
                if (currentFrame <= 0)
                {
                    currentFrame = frames.Count-1;
                }
            }

            Texture2D frameToReturn = frames[currentFrame].sprite;
            timeBetweenFrames = frames[currentFrame].timeTillNext;
            return frameToReturn;

        }
    }

    enum animationType {
        oneShot,
        looping,
        reverseLoop,
        pingPong

    };
}
