using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;


namespace RoundTwoMono
{
    class Attack : Renderable
    {
        
        enum AttackStage{
            startup,
            active,
            recovery
        }

    
        int currentStep, totalSteps, currentActionFrame;
        List<ActionFrame> actionFrames;
        Rectangle otherHurtbox;
        Texture2D hitboxTexture;
        Color hitboxColor;
        Transform parentTransform;
        bool incrementActionFrame, done;

        public Attack(InputManager input, Transform parentTransform, int totalSteps) {
            if (input.playerNumber == 0)
            {
                otherHurtbox = MasterObjectContainer.playerOneHurtbox;
            }
            else
            {
                otherHurtbox = MasterObjectContainer.playerTwoHurtbox;
            }
            hitboxColor = new Color(Color.Red, .5f);
            actionFrames = new List<ActionFrame>();
            this.parentTransform = parentTransform;
            this.totalSteps = totalSteps;
            
        }

        public void Load(ContentManager content) {
            hitboxTexture = content.Load<Texture2D>("square");
        }

        public void AddActionFrame(ActionFrame actionFrame, int duration =1) {
            for (int i = 0; i < duration; i++)
            {
                actionFrames.Add(actionFrame);
                actionFrame = new ActionFrame(actionFrame);
                actionFrame.setActiveFrame(actionFrame.activeFrame + 1);
            }
        }

        public void Start() {
            currentStep = -1;
            currentActionFrame = 0;
            done = false;
            incrementActionFrame = false;
        }

        // runs the current step of the attack
        // returns true if the attack has finished, false if it is still going on
        public bool NextStep()
        {
            currentStep++;
            if (!done)
            {
                if (incrementActionFrame)
                {
                    if ((currentActionFrame + 1) >= actionFrames.Count)
                    {
                        done = true;
                    }
                    else
                    {
                        currentActionFrame++;
                    }
                }
                incrementActionFrame = false;

                if (currentStep == actionFrames[currentActionFrame].activeFrame && actionFrames[currentActionFrame].isAttack)
                {
                    Hitbox tmp = actionFrames[currentActionFrame].hitbox;
                    tmp.hitboxBounds.X = (int)(parentTransform.position.X + tmp.positionOffset.X);
                    tmp.hitboxBounds.Y = (int)(parentTransform.position.Y + tmp.positionOffset.Y);
                    actionFrames[currentActionFrame].hitbox = tmp;
                    incrementActionFrame = true;
                    if (actionFrames[currentActionFrame].hitbox.hitboxBounds.Intersects(otherHurtbox))
                    {
                        // deal damage
                    }
                }

                if (currentStep == actionFrames[currentActionFrame].activeFrame && actionFrames[currentActionFrame].isMovement)
                {
                    parentTransform.Translate(actionFrames[currentActionFrame].movementTranslation);
                    incrementActionFrame = true;
                }
            }

            

            if (currentStep >= totalSteps)
            {
                return true;
            }
            return false;
            
        }

        // renders the hitbox as a red square
        public void Draw(SpriteBatch spriteBatch)
        {
            if (currentStep == actionFrames[currentActionFrame].activeFrame && actionFrames[currentActionFrame].isMovement)
            {
                spriteBatch.Draw(hitboxTexture, actionFrames[currentActionFrame].hitbox.hitboxBounds, hitboxColor);
            }
        }
    }

    class ActionFrame {
        public ActionFrame(int activeFrame) {
            this.activeFrame = activeFrame;
        }

        public ActionFrame(ActionFrame actionframe) {
            activeFrame = actionframe.activeFrame;
            isAttack = actionframe.isAttack;
            isMovement = actionframe.isMovement;
            movementTranslation = actionframe.movementTranslation;
            hitbox = actionframe.hitbox;
            optionalFunction = actionframe.optionalFunction;
        }

        public void setAttack(Hitbox hitbox) {
            this.hitbox = hitbox;
            isAttack = true;
        }
        public void setMovement(Vector2 offset)
        {
            movementTranslation = offset;
            isMovement = true;
        }
        public void setActiveFrame(int activeFrame) {
            this.activeFrame = activeFrame;
        }

        public int activeFrame;
        public bool isAttack;
        public bool isMovement;
        public Vector2 movementTranslation;
        public Hitbox hitbox;
        public delegate void voidDel();
        public voidDel optionalFunction;

    }

    struct Hitbox
    {
        public Hitbox( int damage, int hitstun, int blockstun, int pushback, 
            Rectangle hitboxBounds, Vector2 positionOffset)
        {
            this.damage = damage;
            this.hitstun = hitstun;
            this.blockstun = blockstun;
            this.pushback = pushback;
            this.hitboxBounds = hitboxBounds;
            this.positionOffset = positionOffset;
        }

        
        public int damage;
        public int hitstun;
        public int blockstun;
        public int pushback;
        public Rectangle hitboxBounds;
        public Vector2 positionOffset;
    }

}
