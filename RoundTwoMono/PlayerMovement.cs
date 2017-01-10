﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;
using RoundTwoMono.EngineFang;

namespace RoundTwoMono
{


    class PlayerMovement: Component, Updateable, Renderable
    {

        public delegate void voidDel();
        voidDel jumpForward, jumpBack, jumpNeutral;

        InputManager input;
        public float speed;

        static float DeadSize = .15f;

        SpriteAnimator<FigherAnimations> spriteAnimator;
        FighterState state;
        public CancelState cancelState;

        public float groundBound, leftBound, rightBound;
        public PlayerMovement otherPlayerMovement;
        public bool isFacingLeft { set; get; }
        Vector2 attackDirection;

        public Attack currentAttack, currentJump;
        bool isAttacking, isJumping;
        Texture2D groundIcon;
        Rectangle groundIconRect;

        Rectangle playerMovementBox;
            
        public PlayerMovement() {
            state = FighterState.neutral;
            speed = 0;
        }
        public PlayerMovement(InputManager input, SpriteAnimator<FigherAnimations> spriteAnimator, float speed) {
            state = FighterState.neutral;
            cancelState = CancelState.none;
            this.input = input;
            this.speed = speed;
            this.spriteAnimator = spriteAnimator;

            isAttacking = false;
            isJumping = false;

            groundBound = 650;
            leftBound = 100;
            rightBound = 1800;

            groundIconRect = new Rectangle(2, 2, 4, 4);
            attackDirection = new Vector2(1,1);

            playerMovementBox = new Rectangle(20, 10, 40, 10); ;


        }

        public void Update() {
            if (state == FighterState.neutral && otherPlayerMovement != null)
            {
                ProcessFacingDirection();
            }
            ProcessAttacking();
            ProcessJumping();
            ProcessMovement();

            // update debug ground point
            groundIconRect.X = (int)transform.position.X - (groundIconRect.Width / 2);
            groundIconRect.Y = (int)transform.position.Y - (groundIconRect.Height / 2);

            playerMovementBox.X = (int)transform.position.X - (playerMovementBox.Width / 2);
            playerMovementBox.Y = (int)transform.position.Y - (playerMovementBox.Height);

        }
        public void PlayWin() {
            CancelActions();
            transform.position.Y = 650;
            SetState(FighterState.invincible);
            spriteAnimator.PlayAnimation(FigherAnimations.win);
        }
        public void PlayIntro()
        {
            CancelActions();
            ProcessFacingDirection();
            SetState(FighterState.invincible);
            spriteAnimator.PlayAnimation(FigherAnimations.intro);
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
        public void StartJump(Attack newJump, FigherAnimations newAnimation)
        {
            currentJump = newJump;
            SetState(newJump.state);
            spriteAnimator.PlayAnimation(newAnimation, true);

            currentJump.Start();
            isJumping = true;
        }

        // handle beginning of attack command
        public void StartAttack(Attack newAttack, FigherAnimations newAnimation)
        {
            currentAttack = newAttack;
            SetState(newAttack.state);
            spriteAnimator.PlayAnimation(newAnimation, true);

            currentAttack.Start();
            isAttacking = true;
        }

        // handle attack updates
        void ProcessAttacking() {
            if (isAttacking)
            {
                bool finished = currentAttack.NextStep(attackDirection);
                if (finished) {
                    isAttacking = false;
                    if (!currentAttack.isJumpingAttack)
                    {
                        SetState(FighterState.neutral);
                    }
                }
            }
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
                    SetState(FighterState.neutral);
                }
            }
        }

        // handle facing direction updates
        void ProcessFacingDirection(){
            if (transform.position.X > otherPlayerMovement.transform.position.X && !isFacingLeft)
            {
                transform.flipRenderingHorizontal = true;
                isFacingLeft = true;
                spriteAnimator.Flip(true);
                attackDirection.X = -1;
            }
            else if (transform.position.X < otherPlayerMovement.transform.position.X && isFacingLeft) {
                transform.flipRenderingHorizontal = false;
                isFacingLeft = false;
                spriteAnimator.Flip(false);
                attackDirection.X = 1;

            }
        }

        
        // take in stick position and move character accordingly
        void ProcessMovement() {

            // quit out if not in the neutral state
            if (state != FighterState.neutral) {
                return;
            }

            Vector2 inputAxis = input.GetLeftStick();

            // jump movement block
            if (inputAxis.Y > .5f) {
               
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
                    if (isFacingLeft)
                    {
                        spriteAnimator.PlayAnimation(FigherAnimations.walkBack);
                    }else
                    {
                        spriteAnimator.PlayAnimation(FigherAnimations.walkToward);
                    }
                    direction = 1;
                }
                else
                {
                    if (isFacingLeft)
                    {
                        spriteAnimator.PlayAnimation(FigherAnimations.walkToward);
                    }
                    else
                    {
                        spriteAnimator.PlayAnimation(FigherAnimations.walkBack);
                    }
                    direction = -1;
                }
                TryMove(new Vector2(direction * speed, 0));

            }
            else {
                spriteAnimator.PlayAnimation(FigherAnimations.neutral);
            }
            

            

            
        }

        public void SetInputManager(InputManager input) {
            this.input = input;
        }

        public FighterState GetState()
        {
            return state;
        }
        public void SetState(FighterState newState)
        {
            state = newState;
        }
        public void MoveTowards(Vector2 addVector) {
            if (isFacingLeft) {
                addVector.X *= -1;
            }
            TryMove(addVector);
        }
        public void TryMove(Vector2 addVector) {


            // check for player collision
            // temporarily move rectangle to proposed position
            playerMovementBox.X = (int)(transform.position.X + addVector.X - (playerMovementBox.Width / 2));
            playerMovementBox.Y = (int)(transform.position.Y + addVector.Y - (playerMovementBox.Height));

            // TODO: resolve for jumping into other player

            if (playerMovementBox.Intersects(otherPlayerMovement.playerMovementBox)) {
                // check for moving into other player and disallow it
                if (transform.position.X > otherPlayerMovement.transform.position.X && addVector.X < 0)
                {
                    // right player(us) is trying to move into the left player
                    // resolve if other needs to be pushed or movement needs to be stopped

                    // calculate amount of pushing
                    // calcuate the bounds of rectangles and amount of overlap, pushamount is the amount and direction to move other player
                    float pushAmount =  (transform.position.X + addVector.X - playerMovementBox.Width / 2) - (otherPlayerMovement.transform.position.X + otherPlayerMovement.playerMovementBox.Width / 2);
                    otherPlayerMovement.TryMove(new Vector2(pushAmount, 0));
                    // calcualte new movement vector based on how far other player was successfully pushed
                    addVector.X = (otherPlayerMovement.transform.position.X + otherPlayerMovement.playerMovementBox.Width / 2) -(transform.position.X  - playerMovementBox.Width / 2);
                }
                else if (transform.position.X < otherPlayerMovement.transform.position.X && addVector.X > 0)
                {
                    // left player(us) is trying to move into right player
                    // resolve if other needs to be pushed or movement needs to be stopped

                    // calculate amount of pushing
                    // calcuate the bounds of rectangles and amount of overlap, pushamount is the amount and direction to move other player
                    float pushAmount =   (transform.position.X + addVector.X + playerMovementBox.Width / 2) - (otherPlayerMovement.transform.position.X - otherPlayerMovement.playerMovementBox.Width / 2);
                    otherPlayerMovement.TryMove(new Vector2(pushAmount, 0));
                    // calcualte new movement vector based on how far other player was successfully pushed
                    addVector.X = (otherPlayerMovement.transform.position.X - otherPlayerMovement.playerMovementBox.Width / 2) - (transform.position.X  + playerMovementBox.Width / 2);
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

            if (transform.position.X + addVector.X > rightBound) {
                // clean for moving further than right bound
                addVector.X = rightBound - transform.position.X;
            } else if (transform.position.X + addVector.X < leftBound) {
                // clean for mocing further than left bound
                addVector.X = leftBound - transform.position.X;
            }

            if (transform.position.Y + addVector.Y > groundBound) {
                // clean for moving too far down
                addVector.Y = groundBound - transform.position.Y;
            }


            transform.Translate(addVector);
            playerMovementBox.X = (int)transform.position.X - (playerMovementBox.Width / 2);
            playerMovementBox.Y = (int)transform.position.Y - (playerMovementBox.Height);
        }

        public bool CheckIfBlocking()
        {
            if (isFacingLeft && input.GetLeftStick().X > 0) {
                return true;
            }else if (!isFacingLeft && input.GetLeftStick().X < 0)
            {
                return true;
            }
            return false;
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
                if (currentAttack != null && isAttacking)
                {
                    currentAttack.Draw(spriteBatch);
                }
            }
        }
        public void Load(ContentManager content)
        {
            groundIcon = content.Load<Texture2D>("square");
        }
    }
    
    // fighter character state enumerator
    enum FighterState
    {
        neutral,
        walk,
        jumping,
        invincible,
        hitstun,
        airHitstun,
        blockstun,
        projectileInvincible,
        attackStartup,
        attackRecovery,
        jumpingAttack

    };

    enum CancelState
    {
        light,
        medium,
        heavy,
        special,
        none
    };
}
