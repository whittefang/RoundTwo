﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace RoundTwoMono
{
    class Transform
    {
        public Vector3 position;
        public bool flipRenderingHorizontal = false;
        public Transform() {
            position = Vector3.Zero;
        }
        public Transform(float x, float y, float z) {
            position.X = x;
            position.Y = y;
            position.Z = z;
            
        }
        public Vector2 getPositionV2() {
            return new Vector2(position.X, position.Y);
        }
        public void Translate(Vector2 amount) {
            position += new Vector3( amount.X, amount.Y, 0);
        }
        public SpriteEffects GetHorizontalFlip() {
            if (flipRenderingHorizontal)
            {
                return SpriteEffects.FlipHorizontally;
            }
            else {
                return SpriteEffects.None;
            }
        }
       
    }
}
