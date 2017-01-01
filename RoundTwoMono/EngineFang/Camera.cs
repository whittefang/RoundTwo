using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace RoundTwoMono.EngineFang
{
    public static class Camera
    {
        public static Vector2 Position { get; set; }
        public static float Rotation { get; set; }
        public static float Zoom { get; set; }
        public static Vector2 Origin { get; set; }
        public static float  screenSize { get; set; }

        public static void Init(Viewport viewport) {
            Rotation = 0;
            Zoom = 1;
            Origin = new Vector2(viewport.Width / 2f, viewport.Height / 2f);
            Position = Vector2.Zero;
            screenSize = 200;
        }

        public static Matrix GetViewMatrix()
        {
            return
                Matrix.CreateTranslation(new Vector3(-Position, 0.0f)) *
                Matrix.CreateTranslation(new Vector3(-Origin, 0.0f)) *
                Matrix.CreateRotationZ(Rotation) *
                Matrix.CreateScale(Zoom, Zoom, 1) *
                Matrix.CreateTranslation(new Vector3(Origin, 0.0f));
        }
        public static float GetBound(bool right = true) {
            float pos = Origin.X + Position.X;
            if (right)
            {
                pos += screenSize;
            }
            else
            {
                pos -= screenSize;

            }
            return pos;
        }

    }
}
