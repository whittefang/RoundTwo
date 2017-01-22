using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework.Graphics;
using EngineFang;

namespace RoundTwoMono
{
    class Stage : Component, Updateable, Renderable
    {

        List<StagePart> stageParts;
        public Stage() {
            stageParts = new List<StagePart>();
        }
        public void Draw(SpriteBatch spriteBatch)
        {
            foreach (StagePart stagePart in stageParts) {
                stagePart.Draw(spriteBatch);
            }
        }
        public void AddStagePart(StagePart stagePart) {
            stageParts.Add(stagePart);
        }
       
        public void Update()
        {
            foreach (StagePart stagePart in stageParts)
            {
                if (stagePart.movementFactor > 0)
                {
                    stagePart.position.X = Transform.GetCustomRenderPosition(stagePart.image, Camera.Position * stagePart.movementFactor).X;
                }
            }
        }
    }
}
