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
    class SuperMeter: Component, Renderable
    {
        const float superMeterMaximum = 1000;
        float currentMeter = 0;

        Texture2D barTexture;
        Rectangle barRect;
        const float rectangleWidth = 50;

        public SuperMeter(PlayerIndex playerNumber) {
            if (playerNumber == PlayerIndex.Two)
            {
                barRect = new Rectangle(725, 650, 50, 5);
            }
            else {
                barRect = new Rectangle(1100, 650, 50, 5);
            }
            currentMeter = 000;
            AdjustBar();
        }

        public void AddMeter(int amount) {
            currentMeter += amount;
            if (currentMeter > superMeterMaximum) {
                currentMeter = superMeterMaximum;
            }
            AdjustBar();
        }
        public float GetMeter() {
            return currentMeter;
        }
        public void Load(ContentManager content) {
            barTexture = content.Load<Texture2D>("square");
        }
        public void EmptyMeter() {
            currentMeter = 0;
            AdjustBar();
        }
        void AdjustBar() {
            barRect.Width = (int)((currentMeter/superMeterMaximum)*rectangleWidth) ;
        }

        public void Draw(SpriteBatch spriteBatch) {
            spriteBatch.Draw(barTexture, barRect, Color.Blue);
        }

    }
}
