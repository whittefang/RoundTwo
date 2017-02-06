using System;
using System.Diagnostics;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;
using EngineFang;

namespace RoundTwoMono
{
    class Health : Component, Updateable
    {
        float maximumHealth;
        float currentHealth;
        float comboProration;
      
        FighterSound playerSound;
        FighterStateHandler state;

        PlayerMovement playerMovement;
        SpriteAnimator<FighterAnimations> animator;
        
        Attack chunLiThrow;
        
        public int comboHits, comboDamage;

        SuperMeter superMeter;

        public Health(float maxHealth)
        {
            maximumHealth = maxHealth;
            currentHealth = maxHealth;
            comboHits = 0;
            comboDamage = 0;
            comboProration = 1;
           
           
        }
        public override void Load(ContentManager content)
        {
            playerMovement = entity.getComponent<PlayerMovement>();
            animator = entity.getComponent <SpriteAnimator<FighterAnimations>>();
            superMeter = entity.getComponent<SuperMeter>();
            playerSound = entity.getComponent<FighterSound>();
            state = entity.getComponent<FighterStateHandler>();
        }

        public void SetThrows(Attack chunThrow) {
            chunLiThrow = chunThrow;
        }


        int CalculateProration(int amount) {
            if (state.GetState() == FighterState.hitstun)
            {
                // in combo
                if (comboProration > .4f)
                {
                    comboProration -= .1f;
                }
                amount = (int)(amount * comboProration);
            }
            else {
                comboHits = 1;
                comboProration = 1;
            }

            return amount;
        }

        void DisplayComboInformation(float amount) {
            if (state.GetState() == FighterState.hitstun)
            {
                comboDamage += (int)amount;
                comboHits++;
            }
            else {
                comboDamage = (int)amount;
            }
            UIMatch.ComboTextUpdate(comboDamage, comboHits, state.isPlayerOne());
        }

        public int DealDamage(int amount, bool chip = false)
        {

            amount = CalculateProration(amount);
            DisplayComboInformation(amount);
            currentHealth -= amount;
            
            if (currentHealth <= 0)
            {
                if (!chip)
                {
                    // resolve death
                    playerMovement.otherPlayerMovement.PlayWin();
                    MasterObjectContainer.EndRound(state.isPlayerOne());
                }
            }
            return amount;

        }
        public int GetHealth() {
            return (int)currentHealth;
        }
        public void Update()
        {
            HealthbarUpdate();
        }

        void HealthbarUpdate()
        {
            UIMatch.HealthbarUpdate((currentHealth / maximumHealth), state.isPlayerOne());
        }
        public void ResetHealth() {
            currentHealth = maximumHealth;

        }
        

       

       

        
    }
}
