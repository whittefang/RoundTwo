using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework;
using EngineFang;
namespace RoundTwoMono
{
    class SuperMeter: Component
    {
        const float superMeterMaximum = 1000;
        float currentMeter = 0;

        FighterStateHandler state;

        public SuperMeter() {
            currentMeter = 000;
        }
        public override void Load(ContentManager content)
        {
            state = entity.getComponent<FighterStateHandler>();

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
        
        public void EmptyMeter() {
            currentMeter = 0;
            AdjustBar();
        }
        void AdjustBar() {
            UIMatch.SuperbarUpdate((currentMeter / superMeterMaximum), state.isPlayerOne());
        }

        

    }
}
