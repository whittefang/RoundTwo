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
     static class UIMatch
    {
        static SpriteFont font;
        static Texture2D healthBarArt, superMeterArtLeft, superMeterArtRight;
        static Rectangle hpArtLeftRect, hpArtRightRect;
        static Texture2D healthBarTex;
        static Rectangle healthBarLeftRect, healthBarRightRect, superBarRightRect, superBarLeftRect;
        static Vector2 superMeterArtPosLeft, superMeterArtPosRight;
        static int healthBarWidth = 167, superMeterWidth = 80;
        static Vector2 healthbarPositionLeft = new Vector2(-31, 193), healthBarPositionRight = new Vector2(31, 193);
        static Vector2 superBarPositionLeft = new Vector2(-81, -15), superBarPositionRight = new Vector2(81, -15);
        static bool DisplayComboDataP1 = false, DisplayComboDataP2 = false;

        static int comboDamageP1, comboDamageP2;
        static int comboCountP1, comboCountP2;

        static UIMatch() {
            // art
            hpArtLeftRect = Transform.GetCustomRenderPosition(new Rectangle(0, 0, 205, 42), new Vector2(0,180), TransformOriginPoint.right);
            hpArtRightRect = Transform.GetCustomRenderPosition(new Rectangle(0, 0, 205, 42), new Vector2(0, 180), TransformOriginPoint.left);
            superMeterArtPosLeft = new Vector2(-141, -10);
            superMeterArtPosRight = new Vector2(140, -10);

            // bars
            healthBarLeftRect = Transform.GetCustomRenderPosition(new Rectangle(0, 0, healthBarWidth, 5), healthbarPositionLeft, TransformOriginPoint.right);
            healthBarRightRect = Transform.GetCustomRenderPosition(new Rectangle(0, 0, healthBarWidth, 5), healthBarPositionRight, TransformOriginPoint.left);

            superBarRightRect = Transform.GetCustomRenderPosition(new Rectangle(0, 0, superMeterWidth, 4), superBarPositionRight, TransformOriginPoint.left);
            superBarLeftRect = Transform.GetCustomRenderPosition(new Rectangle(0, 0, superMeterWidth, 4), superBarPositionLeft, TransformOriginPoint.right);

        }

        static public void Load(ContentManager content) {
            healthBarArt = content.Load<Texture2D>("ui/hpBarLeft");
            superMeterArtLeft = content.Load<Texture2D>("ui/superBarLeft");
            superMeterArtRight = content.Load<Texture2D>("ui/SuperBarRight");
            healthBarTex = content.Load<Texture2D>("square");
            font = content.Load<SpriteFont>("arial");
        }

        static public void Draw(SpriteBatch spriteBatch)
        {
            // boarder art
            spriteBatch.Draw(healthBarArt, hpArtLeftRect, color: Color.White);
            spriteBatch.Draw(healthBarArt,null, hpArtRightRect, effects: SpriteEffects.FlipHorizontally);

            spriteBatch.Draw(superMeterArtLeft, Transform.GetCustomRenderPosition(superMeterArtLeft, superMeterArtPosLeft), color: Color.White);
            spriteBatch.Draw(superMeterArtRight, Transform.GetCustomRenderPosition(superMeterArtRight, superMeterArtPosRight), color: Color.White);

            // health bars and super bars
            spriteBatch.Draw(healthBarTex, healthBarLeftRect, Color.Green);
            spriteBatch.Draw(healthBarTex, healthBarRightRect, Color.Green);

            spriteBatch.Draw(healthBarTex, superBarRightRect, Color.Blue);
            spriteBatch.Draw(healthBarTex, superBarLeftRect, Color.Blue);
            if (DisplayComboDataP1)
            {
                spriteBatch.DrawString(font, comboCountP1.ToString(), new Vector2(-150, -150), Color.White, 0, Vector2.Zero, .25f, SpriteEffects.None, 0);
                spriteBatch.DrawString(font, comboDamageP1.ToString(), new Vector2(-150, -125), Color.White, 0, Vector2.Zero, .25f, SpriteEffects.None, 0);
            }
            if (DisplayComboDataP2)
            {
                spriteBatch.DrawString(font, comboCountP2.ToString(), new Vector2(150, -150), Color.White, 0, Vector2.Zero, .25f, SpriteEffects.None, 0);
                spriteBatch.DrawString(font, comboDamageP2.ToString(), new Vector2(150, -125), Color.White, 0, Vector2.Zero, .25f, SpriteEffects.None, 0);
            }
        }
        static public void HealthbarUpdate(float healthPercent, bool playerOne)
        {
            if (playerOne)
            {
                healthBarLeftRect.Width = (int)(healthBarWidth * healthPercent);
                healthBarLeftRect = Transform.GetCustomRenderPosition(healthBarLeftRect, healthbarPositionLeft, TransformOriginPoint.right);
            }
            else {
                healthBarRightRect.Width = (int)(healthBarWidth * healthPercent);
                healthBarRightRect = Transform.GetCustomRenderPosition(healthBarRightRect, healthBarPositionRight, TransformOriginPoint.left);

            }
        }
        static public void SuperbarUpdate(float superPercent, bool playerOne)
        {
            if (playerOne)
            {
                superBarLeftRect.Width = (int)(superMeterWidth * superPercent);
                superBarLeftRect = Transform.GetCustomRenderPosition(superBarLeftRect, superBarPositionLeft, TransformOriginPoint.right);
            }
            else {
                superBarRightRect.Width = (int)(superMeterWidth * superPercent);
                superBarRightRect = Transform.GetCustomRenderPosition(superBarRightRect, superBarPositionRight, TransformOriginPoint.left);

            }
        }
        static public void ComboTextUpdate(int comboDamage, int comboCount, bool playerOne) {
            if (playerOne)
            {
                comboCountP1 = comboCount;
                comboDamageP1 = comboDamage;
                DisplayComboDataP1 = true;
            }
            else
            {
                comboCountP2 = comboCount;
                comboDamageP2 = comboDamage;
                DisplayComboDataP2 = true;
            }
        }
        static public void HideComboText(bool playerOne) {
            if (playerOne)
            {
                DisplayComboDataP1 = false;
            }
            else
            {
                DisplayComboDataP2 = false;

            }
        }
    }
}
