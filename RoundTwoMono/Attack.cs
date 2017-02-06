using System;
using System.Diagnostics;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using EngineFang;


namespace RoundTwoMono
{
    class Attack : Renderable
    {
        
        enum AttackStage{
            startup,
            active,
            recovery
        }

    
        int currentStep, totalSteps;
        Dictionary< int,ActionFrame> actionFrames;
        Entity otherPlayer;
        HitResolver otherHitResolver; 
        Texture2D hitboxTexture;
        Color hitboxColor;
        Transform parentTransform;
        PlayerMovement playerMovement;
        public FighterState state;
        public bool isJumpingAttack;

        public Attack(InputManager input, Transform parentTransform, PlayerMovement playerMovement, FighterState newState, int totalSteps) {
            
            hitboxColor = new Color(Color.Red, .5f);
            actionFrames = new Dictionary<int ,ActionFrame>();
            this.parentTransform = parentTransform;
            this.totalSteps = totalSteps;
            this.playerMovement = playerMovement;
            state = newState;
            isJumpingAttack = false;


        }
        public void SetOtherPlayer(ref Entity otherPlayer) {
            this.otherPlayer = otherPlayer;
            otherHitResolver = otherPlayer.getComponent<HitResolver>();
        }

        public void Load(ContentManager content) {
            hitboxTexture = content.Load<Texture2D>("square");
        }

        public void AddActionFrame(ActionFrame actionFrame, int duration =1) {

            int initialActiveFrame = actionFrame.activeFrame;

            for (int i = 0; i < duration; i++)
            {
                // if an action frame exists we replace relevant fields
                // WARING: possible to overwrite important data 
                if (actionFrames.ContainsKey(actionFrame.activeFrame + i)) {
                    if (actionFrame.isAttack) {
                        actionFrames[actionFrame.activeFrame + i].setAttack(actionFrame.hitbox);
                    }
                    if (actionFrame.isMovement)
                    {
                        actionFrames[actionFrame.activeFrame + i].setMovement(actionFrame.movementTranslation);
                    }
                    if(actionFrame.optionalFunction != null)
                    {
                        actionFrames[actionFrame.activeFrame + i].optionalFunction = actionFrame.optionalFunction;
                    }
                    if (actionFrame.optionalHitFunction != null) {
                        actionFrames[actionFrame.activeFrame + i].optionalHitFunction = actionFrame.optionalHitFunction;
                    }
                    if (actionFrame.optionalSound != null) {
                        actionFrames[actionFrame.activeFrame + i].optionalSound = actionFrame.optionalSound;
                    }
                } else {
                    actionFrames.Add(initialActiveFrame + i, actionFrame);
                    actionFrame = new ActionFrame(actionFrame);
                    actionFrame.setActiveFrame(initialActiveFrame + i);
                }
            }
        }

        public void Start() {
            currentStep = -1;
            playerMovement.cancelState = CancelState.none;
        }

        // runs the current step of the attack
        // returns true if the attack has finished, false if it is still going on
        public bool NextStep(Vector2 direction)
        {
            currentStep++;

            if (currentStep <= totalSteps && actionFrames.ContainsKey(currentStep))
            {
                // move 
                if (actionFrames[currentStep].isMovement)
                {
                    playerMovement.MoveTowards(actionFrames[currentStep].movementTranslation);
                }

                // activate hitbox
                if (actionFrames[currentStep].isAttack)
                {
                    // check if this is the first frame of an attack and mark its id 
                    // else set the following active frame ids to the same as the previous
                    if (!actionFrames.ContainsKey(currentStep - 1) || !actionFrames[currentStep - 1].isAttack)
                    {
                        actionFrames[currentStep].hitbox.moveCurrentUseID++;
                    }
                    else
                    {
                        actionFrames[currentStep].hitbox.moveCurrentUseID = actionFrames[currentStep-1].hitbox.moveCurrentUseID;
                    }
                    
                    actionFrames[currentStep].hitbox.hitboxBounds = Transform.GetCustomRenderPosition(actionFrames[currentStep].hitbox.hitboxBounds, new Vector2(parentTransform.position.X+(actionFrames[currentStep].hitbox.positionOffset.X * direction.X), parentTransform.position.Y + actionFrames[currentStep].hitbox.positionOffset.Y));
                    // check for hit
                    if (otherHitResolver.CheckForHit(actionFrames[currentStep].hitbox))
                    {

                        //TODO: fix cancel state for invincible
                        playerMovement.cancelState = actionFrames[currentStep].hitbox.cancelStrength;
                        // run optional hit function when we have made contact
                        if (actionFrames[currentStep].optionalHitFunction != null) {
                            actionFrames[currentStep].optionalHitFunction();
                        }
                        
                    }

                }

                
                // run optional function
                if (actionFrames[currentStep].optionalFunction != null)
                {
                    actionFrames[currentStep].optionalFunction();
                }

                // run optional sound effect if it exists
                if (actionFrames[currentStep].optionalSound != null)
                {
                    actionFrames[currentStep].optionalSound.Play();
                }

            }
                
            // when move is over return true and reset cancel state
            if (currentStep >= totalSteps)
            {
                playerMovement.cancelState = CancelState.none;
                return true;
            }
            return false;
            
        }

        // renders the hitbox as a red square
        public void Draw(SpriteBatch spriteBatch)
        {
            if ( actionFrames.ContainsKey(currentStep))  //&& actionFrames[currentActionFrame].isMovement)
            {
                spriteBatch.Draw(hitboxTexture, actionFrames[currentStep].hitbox.hitboxBounds, hitboxColor);
            }
        }

        public void CancelAttack()
        {
            playerMovement.cancelState = CancelState.none;
            currentStep = -1;
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
            optionalHitFunction = actionframe.optionalHitFunction;
            optionalSound = actionframe.optionalSound;
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
        public void setOptionalSound(SoundEffect newSound) {
            optionalSound = newSound;
        }

        public int activeFrame;
        public bool isAttack;
        public bool isMovement;
        public Vector2 movementTranslation;
        public Hitbox hitbox;
        public delegate void voidDel();
        public SoundEffect optionalSound;
        public voidDel optionalFunction, optionalHitFunction;

    }

    struct Hitbox
    {
        public Hitbox( int damage, int chipDamage, int hitstun, int blockstun, Vector2 pushback, 
            Rectangle hitboxBounds, Vector2 positionOffset,  CancelState cancelStrength,HitSpark attackStrength,AttackProperty attackProperty = AttackProperty.Hit, int pushbackDuration =5, int hitStop = -1, bool ignorePushback = false, ThrowType throwType = ThrowType.none)
        {
            this.damage = damage;
            this.chipDamage = chipDamage;
            this.hitstun = hitstun;
            this.blockstun = blockstun;
            this.pushbackDuration = pushbackDuration;
            this.pushback = pushback;
            this.hitboxBounds = hitboxBounds;
            this.positionOffset = positionOffset;
            this.attackProperty = attackProperty;
            this.cancelStrength = cancelStrength;
            this.attackStrength = attackStrength;
            this.ignorePushback = ignorePushback;
            this.throwType = throwType;
            moveMasterID = MasterObjectContainer.GetMoveMasterID();
            moveCurrentUseID = 0;
            if (hitStop == -1)
            {
                if (attackStrength == HitSpark.light)
                {
                    this.hitStop = 5;
                }
                else if (attackStrength == HitSpark.medium)
                {
                    this.hitStop = 7;
                }
                else if (attackStrength == HitSpark.heavy)
                {
                    this.hitStop = 9;
                }
                else
                {
                    this.hitStop = 11;
                }
            }
            else {
                this.hitStop = hitStop;
            }
        }

        
        public int damage;
        public int chipDamage;
        public int hitstun;
        public int blockstun;
        public int hitStop;
        public int pushbackDuration;
        //public int blockStop; // should this exist?
        public Vector2 pushback;
        public Rectangle hitboxBounds;
        public Vector2 positionOffset;
        public CancelState cancelStrength;
        public HitSpark attackStrength;
        public AttackProperty attackProperty;
        public int moveMasterID;
        public int moveCurrentUseID;
        public bool ignorePushback;
        public ThrowType throwType;
    }
    enum AttackProperty {
        Throw,
        Launcher,
        Kockdown,
        Hit
    }
    enum ThrowType {
        chun,
        none

    }
}
