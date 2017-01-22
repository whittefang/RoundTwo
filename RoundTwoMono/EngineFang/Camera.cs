using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace EngineFang
{
    public static class Camera
    {
        public static Vector2 Position { get; set; }
        public static float Rotation { get; set; }
        public static float Zoom { get; set; }
        public static Vector2 Origin { get; set; }
        public static float  screenSize { get; set; }

        public static void Init(Viewport viewport, float Zoom =1) {
            Rotation = 0;
            Camera.Zoom = Zoom;
            //Origin = new Vector2(viewport.Width / 2f, viewport.Height / 2f);
            Origin = new Vector2((-viewport.Width / Zoom) / 2, (-viewport.Height / Zoom) / 2);
            Position = Vector2.Zero;
            screenSize = (viewport.Width/Zoom)/2;
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
        public static Matrix GetUIMatrix()
        {
            return
                Matrix.CreateTranslation(new Vector3(213,110, 0.0f)) *
                Matrix.CreateTranslation(new Vector3(0, -Origin.Y, 0.0f)) *
                Matrix.CreateRotationZ(Rotation) *
                Matrix.CreateScale(Zoom, Zoom, 1) *
                Matrix.CreateTranslation(new Vector3(0, Origin.Y, 0.0f));
        }
        public static void SetScale(int newScale){


        }
        public static void SetPosition(Vector2 newPosition) {
            newPosition.Y = -newPosition.Y;
            Position = newPosition;
        }
        public static float GetBound(bool right = true) {
            float pos = Position.X ;
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
