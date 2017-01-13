using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework;
namespace RoundTwoMono
{
    class UIMatch : Component, Renderable
    {
        Texture2D healthBarArt, superMeterArtLeft, superMeterArtRight;
        Rectangle hpLeft, hpRight;

        public UIMatch() {
            hpLeft = new Rectangle(725, 410, 233, 42);
            hpRight = new Rectangle(960, 410, 233, 42);
        }

        public void Load(ContentManager content) {
            healthBarArt = content.Load<Texture2D>("ui/hpBarLeft");
            superMeterArtLeft = content.Load<Texture2D>("ui/superBarLeft");
            superMeterArtRight = content.Load<Texture2D>("ui/SuperBarRight");
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(healthBarArt, hpLeft, color: Color.White);
           spriteBatch.Draw(healthBarArt,null, hpRight, effects: SpriteEffects.FlipHorizontally);

         spriteBatch.Draw(superMeterArtLeft, new Vector2(725, 645), color: Color.White);
            spriteBatch.Draw(superMeterArtRight, new Vector2(1065, 645), color: Color.White);
        }
    }
}
