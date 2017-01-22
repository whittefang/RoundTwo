using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace RoundTwoMono
{
    class StagePart
    {
        public Vector2 position;
        public Texture2D image;
        public float movementFactor;
        public StagePart(Vector2 position, Texture2D image, float movementFactor) {
            this.position = position;
            this.image = image;
            this.movementFactor = movementFactor;
        }
        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(image, position, Color.White);
        }

    }
}
