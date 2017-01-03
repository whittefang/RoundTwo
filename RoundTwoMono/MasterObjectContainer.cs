using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;

namespace RoundTwoMono
{
    static class MasterObjectContainer
    {
        public static Rectangle playerOneHurtbox, playerTwoHurtbox;
        public static PlayerMovement playerOneMovement, playerTwoMovement;
        public static bool showHitboxes = true;
        public static bool DevMode = false;
        public static bool paused = false;
        public static bool advanceOneFrame = false;
        public static int hitstopRemaining = 0;
        static int currentMoveID = 0;

        public static Entity hitSparkHolder;
        public static SpriteAnimator<superFlash> superEffect;

        static int playerOneWins, playerTwoWins;
        static int introFramesRemaining = 0;
        static int roundEndFramesRemaining = 0;


        static public int GetMoveMasterID() {
            currentMoveID++;
            return currentMoveID;
        }
        public static void EndRound(bool playerOneWon ) {
            if (playerOneWon)
            {
                playerOneWins++;
                // show win text
                // set round win icon

            }
            else
            {
                playerTwoWins++;

            }
            roundEndFramesRemaining = 120;
            // ui black screen wipe
        }
        public static void NextRound() {
            

            
            introFramesRemaining = 120;
            // move players to start position
            playerOneMovement.transform.position = new Vector3(900, 650, 0);
            playerTwoMovement.transform.position = new Vector3(1000, 650, 0);
            // reset health
            playerOneMovement.entity.getComponent<Health>().ResetHealth();
            playerTwoMovement.entity.getComponent<Health>().ResetHealth();
            
            // play intro animations
            playerOneMovement.PlayIntro();
            playerTwoMovement.PlayIntro();
            if (DevMode)
            {

                playerOneMovement.SetState(FighterState.neutral);
                playerTwoMovement.SetState(FighterState.neutral);
            }
        }
        public static void Update()
        {
            if (introFramesRemaining > 0)
            {
                // play neutral animations after intro finishes
                if (introFramesRemaining == 60)
                {
                    playerTwoMovement.entity.getComponent<SpriteAnimator<FigherAnimations>>().PlayAnimation(FigherAnimations.neutral);
                    playerOneMovement.entity.getComponent<SpriteAnimator<FigherAnimations>>().PlayAnimation(FigherAnimations.neutral);
                }


                // show round number
                // show fight text
                // enable controls
                if (introFramesRemaining == 1)
                {
                    playerOneMovement.SetState(FighterState.neutral);
                    playerTwoMovement.SetState(FighterState.neutral);
                }

                introFramesRemaining--;
            }


            if (roundEndFramesRemaining > 0)
            {
                if (roundEndFramesRemaining == 60)
                {
                    // black screen wipe
                }
                if (roundEndFramesRemaining == 1)
                {
                    NextRound();
                }
                roundEndFramesRemaining--;
            }
        }
    }

    enum superFlash {
        none,
        super
    }
}
