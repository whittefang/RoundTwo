using System;
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


    class PlayerMovement : Component, Updateable, Renderable
    {

        public delegate void voidDel();
        voidDel jumpForward, jumpBack, jumpNeutral;

        InputManager input;
        public float speed;

        static float DeadSize = .15f;

        SpriteAnimator<FighterAnimations> spriteAnimator;
      
        public CancelState cancelState;

        public float groundBound;
        public PlayerMovement otherPlayerMovement;
        Vector2 attackDirection;

        FighterStateHandler state;

        public Attack currentJump;
        bool isAttacking, isJumping;
        Texture2D groundIcon;
        Rectangle groundIconRect;

        Rectangle playerMovementBox;

        Boolean playerMovementBoxEnabled = true;

        public PlayerMovement() {
            speed = 0;
        }
        public PlayerMovement(float speed) {
            
            cancelState = CancelState.none;
            this.speed = speed;

            isAttacking = false;
            isJumping = false;

            groundBound = 0;

            groundIconRect = new Rectangle(2, 2, 4, 4);
            attackDirection = new Vector2(1, 1);

            playerMovementBox = new Rectangle(20, 10, 40, 10); ;


        }
        public override void Load(ContentManager content) {
            spriteAnimator = entity.getComponent<SpriteAnimator<FighterAnimations>>();
            input = entity.getComponent<InputManager>();
            state = entity.getComponent<FighterStateHandler>();
            groundIcon = content.Load<Texture2D>("square");
        }
        public void Update() {
           
            ProcessJumping();
            ProcessMovement();

            // update debug ground point
            groundIconRect = transform.GetRenderPosition(groundIconRect, TransformOriginPoint.bottom);

            playerMovementBox = transform.GetRenderPosition(playerMovementBox, TransformOriginPoint.bottom);

        }
        public void PlayWin() {
            CancelActions();
            transform.position.Y = 0;
            state.SetState(FighterState.invincible);
            spriteAnimator.PlayAnimation(FighterAnimations.win);
        }
        public void PlayIntro()
        {
            CancelActions();
            state.ProcessFacingDirection();
            state.SetState(FighterState.invincible);
            spriteAnimator.PlayAnimation(FighterAnimations.intro);
        }
        public void SetJumps(voidDel forward, voidDel back, voidDel neutral) {
            jumpForward = forward;
            jumpBack = back;
            jumpNeutral = neutral;
        }

        public void SetOtherPlayer(PlayerMovement otherPlayerMovement) {
            this.otherPlayerMovement = otherPlayerMovement;
        }

        // handle beginning of jump command
        public void StartJump(Attack newJump, FighterAnimations newAnimation)
        {

            currentJump = newJump;
            state.SetState(newJump.state);
            spriteAnimator.PlayAnimation(newAnimation, true);

            currentJump.Start();
            isJumping = true;
        }

        

        

        //handle jumping updates
        void ProcessJumping()
        {
            if (isJumping)
            {
                bool finished = currentJump.NextStep(attackDirection);
                if (finished)
                {
                    isJumping = false;
                    state.SetState(FighterState.neutral);
                }
            }
        }

        


        // take in stick position and move character accordingly
        void ProcessMovement() {

            // quit out if not in the neutral state
            if (state.GetState() != FighterState.neutral) {
                return;
            }

            Vector2 inputAxis = input.GetLeftStick();

            // jump movement block
            if (inputAxis.Y > .5f) {
                state.ProcessFacingDirection();
                if (inputAxis.X > DeadSize) {
                    // jump right
                    jumpForward();
                }
                else if (inputAxis.X < -DeadSize)
                {
                    // jump left
                    jumpBack();

                } else {
                    // jump up
                    jumpNeutral();
                }
            }
            else if (Math.Abs(inputAxis.X) > DeadSize)
            {
                // left right movement block

                int direction = 0;
                // check if left or right movement
                if (inputAxis.X > 0)
                {
                    if (state.GetFacingDirection())
                    {
                        spriteAnimator.PlayAnimation(FighterAnimations.walkBack);
                    } else
                    {
                        spriteAnimator.PlayAnimation(FighterAnimations.walkToward);
                    }
                    direction = 1;
                }
                else
                {
                    if (state.GetFacingDirection())
                    {
                        spriteAnimator.PlayAnimation(FighterAnimations.walkToward);
                    }
                    else
                    {
                        spriteAnimator.PlayAnimation(FighterAnimations.walkBack);
                    }
                    direction = -1;
                }
                TryMove(new Vector2(direction * speed, 0));

            }
            else {
                spriteAnimator.PlayAnimation(FighterAnimations.neutral);
            }
        }


        public void MoveTowards(Vector2 addVector) {
            if (state.GetFacingDirection()) {
                addVector.X *= -1;
            }
            TryMove(addVector);
        }
        public void TryMove(Vector2 addVector) {


            // check for player collision
            // temporarily move rectangle to proposed position
            playerMovementBox.X = (int)(transform.position.X + addVector.X - (playerMovementBox.Width / 2));
            playerMovementBox.Y = (int)(transform.position.Y + addVector.Y - (playerMovementBox.Height));

            // TODO: fix crash for pusing over 0,0

            if (playerMovementBox.Intersects(otherPlayerMovement.playerMovementBox) && playerMovementBoxEnabled) {
                // check for moving into other player and disallow it
                if (transform.position.X > otherPlayerMovement.transform.position.X && !(addVector.X > 0))
                {
                    // right player(us) is trying to move into the left player
                    // resolve if other needs to be pushed or movement needs to be stopped

                    // calculate amount of pushing
                    // calcuate the bounds of rectangles and amount of overlap, pushamount is the amount and direction to move other player
                    float pushAmount = (transform.position.X + addVector.X - playerMovementBox.Width / 2) - (otherPlayerMovement.transform.position.X + otherPlayerMovement.playerMovementBox.Width / 2);
                    otherPlayerMovement.TryMove(new Vector2(pushAmount, 0));
                    // calcualte new movement vector based on how far other player was successfully pushed
                    addVector.X = (otherPlayerMovement.transform.position.X + otherPlayerMovement.playerMovementBox.Width / 2) - (transform.position.X - playerMovementBox.Width / 2);
                }
                else if (transform.position.X < otherPlayerMovement.transform.position.X && !(addVector.X < 0))
                {
                    // left player(us) is trying to move into right player
                    // resolve if other needs to be pushed or movement needs to be stopped

                    // calculate amount of pushing
                    // calcuate the bounds of rectangles and amount of overlap, pushamount is the amount and direction to move other player
                    float pushAmount = (transform.position.X + addVector.X + playerMovementBox.Width / 2) - (otherPlayerMovement.transform.position.X - otherPlayerMovement.playerMovementBox.Width / 2);
                    otherPlayerMovement.TryMove(new Vector2(pushAmount, 0));
                    // calcualte new movement vector based on how far other player was successfully pushed
                    addVector.X = (otherPlayerMovement.transform.position.X - otherPlayerMovement.playerMovementBox.Width / 2) - (transform.position.X + playerMovementBox.Width / 2);
                } else if (transform.position.X == otherPlayerMovement.transform.position.X) {
                    // we are trying to jump into the corner
                    // resolve by pusing towards center
                    //WARNING: current calculation may not suffice for a variety of different sized moveboxes
                    if (transform.position.X < 0)
                    {
                        otherPlayerMovement.TryMove(new Vector2(40, 0));
                    }
                    else {
                        otherPlayerMovement.TryMove(new Vector2(-40, 0));
                    }
                }


            }
            // check for camera bound
            if (transform.position.X + addVector.X > Camera.GetBound())
            {
                // clean for moving further than right bound
                addVector.X = Camera.GetBound() - transform.position.X;
            }
            else if (transform.position.X + addVector.X < Camera.GetBound(false))
            {
                // clean for mocing further than left bound
                addVector.X = Camera.GetBound(false) - transform.position.X;
            }

            if (transform.position.X + addVector.X > Camera.GetBound()) {
                // clean for moving further than right bound
                addVector.X = Camera.GetBound() - transform.position.X;
            } else if (transform.position.X + addVector.X < Camera.GetBound(false)) {
                // clean for mocing further than left bound
                addVector.X = Camera.GetBound() - transform.position.X;
            }

            if (transform.position.Y + addVector.Y < groundBound) {
                // clean for moving too far down
                addVector.Y = groundBound - transform.position.Y;
            }


            transform.Translate(addVector);
            playerMovementBox.X = (int)transform.position.X - (playerMovementBox.Width / 2);
            playerMovementBox.Y = (int)transform.position.Y - (playerMovementBox.Height);
        }
        public void EnableMovementCollision(bool enabled) {
            playerMovementBoxEnabled = enabled;
        }
       

        public void CancelActions() {
            isAttacking = false;
            isJumping = false;
            cancelState = CancelState.none;
        }
        public void CancelJump() {

            isJumping = false;
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            if (MasterObjectContainer.showHitboxes)
            {
                // render ground marker
                spriteBatch.Draw(groundIcon, groundIconRect, new Color(Color.Purple, .5f));
                // render movebox
                spriteBatch.Draw(groundIcon, playerMovementBox, new Color(Color.Blue, .5f));
                
            }
        }
    }
    
    
}
