using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace EngineFang
{
    class Transform
    {
        public Vector3 position;
        public bool flipRenderingHorizontal = false;
        public Transform()
        {
            position = Vector3.Zero;
        }
        public Transform(float x, float y, float z)
        {
            position.X = x;
            position.Y = y;
            position.Z = z;

        }
        public Vector2 getPositionV2()
        {
            return new Vector2(position.X, position.Y);
        }
        public void Translate(Vector2 amount)
        {
            position += new Vector3(amount.X, amount.Y, 0);
        }
        public SpriteEffects GetHorizontalFlip()
        {
            if (flipRenderingHorizontal)
            {
                return SpriteEffects.FlipHorizontally;
            }
            else {
                return SpriteEffects.None;
            }
        }
        public static Rectangle GetCustomRenderPosition(Rectangle rect, Vector2 positionToRenderAt, TransformOriginPoint origin = TransformOriginPoint.center)
        {
            rect.X = (int)positionToRenderAt.X;
            rect.Y = (int)-positionToRenderAt.Y;
            if (origin == TransformOriginPoint.center)
            {
                rect.X -= (rect.Width / 2);
                rect.Y -= (rect.Height / 2);
            }
            else if (origin == TransformOriginPoint.bottom)
            {
                rect.X -= (rect.Width / 2);
                rect.Y -= (rect.Height);
            }
            else if (origin == TransformOriginPoint.top)
            {
                rect.X -= (rect.Width / 2);
            }
            else if (origin == TransformOriginPoint.right)
            {
                rect.Y -= (rect.Height/2);
                rect.X -= rect.Width;

            }
            else if (origin == TransformOriginPoint.left)
            {
                rect.Y -= (rect.Height / 2);

            }
            return rect;
        }
        public static Vector2 GetCustomRenderPosition(Texture2D texture, Vector2 positionToRenderA, TransformOriginPoint origin = TransformOriginPoint.center)
        {
            Vector2 renderPosition = new Vector2(positionToRenderA.X, -positionToRenderA.Y);
            renderPosition.X = positionToRenderA.X;
            renderPosition.Y = -positionToRenderA.Y;
            if (origin == TransformOriginPoint.center)
            {
                renderPosition.X -= (texture.Width / 2);
                renderPosition.Y -= (texture.Height / 2);
            }
            else if (origin == TransformOriginPoint.bottom)
            {
                renderPosition.X -= (texture.Width / 2);
                renderPosition.Y -= (texture.Height);
            }
            else if (origin == TransformOriginPoint.top)
            {
                renderPosition.X -= (texture.Width / 2);
            }
            else if (origin == TransformOriginPoint.right)
            {
                renderPosition.Y -= (texture.Height / 2);
                renderPosition.X -= (texture.Width);

            }
            else if (origin == TransformOriginPoint.left)
            {
                renderPosition.Y -= (texture.Height / 2);

            }
            return renderPosition;
        }
        public Rectangle GetRenderPosition(Rectangle rect, TransformOriginPoint origin = TransformOriginPoint.center)
        {
            rect.X = (int)position.X;
            rect.Y = (int)-position.Y;
            if (origin == TransformOriginPoint.center)
            {
                rect.X -= (rect.Width / 2);
                rect.Y -= (rect.Height / 2);
            }
            else if (origin == TransformOriginPoint.bottom)
            {
                rect.X -= (rect.Width / 2);
                rect.Y -= (rect.Height);
            }
            else if (origin == TransformOriginPoint.top)
            {
                rect.X -= (rect.Width / 2);
            }
            else if (origin == TransformOriginPoint.right)
            {
                rect.Y -= (rect.Height / 2);
                rect.X -= (-rect.Width);

            }
            else if (origin == TransformOriginPoint.left)
            {
                rect.Y -= (rect.Height / 2);

            }
            return rect;
        }
        public Vector2 GetRenderPosition(Texture2D texture)
        {
            Vector2 renderPosition = new Vector2(position.X, -position.Y);
            renderPosition.X -= (texture.Width / 2);
            renderPosition.Y -= (texture.Height / 2);
            return renderPosition;
        }

    }
    enum TransformOriginPoint{
        center,
        bottom,
        top,
        right,
        left
    }
}
