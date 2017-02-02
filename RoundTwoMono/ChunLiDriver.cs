using System;
using System.Diagnostics;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using EngineFang;

namespace RoundTwoMono
{
    class ChunLiDriver : Component
    {

        Attack light, medium, heavy, sp1, sp2, sp3, super, throwTry, throwAttack;
        Attack jumpLight, jumpMedium, jumpHeavy;
        Attack jumpMediumFollowup;
        Attack jumpForward, jumpBack, jumpNeutral;
        Attack winAttack, introAttack;
        Attack chunLiThrown;
        Projectile fireball;
        InputManager input;
        SuperMeter superMeter;
        SpriteAnimator<FighterAnimations> animator;
        PlayerMovement playerMovement;
        FighterSound soundPlayer;
        Health health;
        bool showHitboxes;

        bool jumpMovesRemaining = false;

        public ChunLiDriver()
        {
            showHitboxes = true;
        }

        public void load(ContentManager Content, PlayerIndex playerNumber)
        {

            input = new InputManager(playerNumber);
            entity.addComponent(input);

            animator = new SpriteAnimator<FighterAnimations>(new Vector2(-10, 75));
            entity.addComponent(animator);

            playerMovement = new PlayerMovement(input, animator, 5f);
            playerMovement.Load(Content);
            entity.addComponent(playerMovement);

            superMeter = new SuperMeter(playerNumber);
            //superMeter.Load(Content);
            entity.addComponent(superMeter);

            soundPlayer = new FighterSound();
            entity.addComponent(soundPlayer);

            health = new Health(1000, playerMovement, animator, playerNumber, superMeter, soundPlayer);
            entity.addComponent(health);
            health.load(Content);

            // Sound Block
            

            soundPlayer.light = Content.Load<SoundEffect>("3rdStrikeSounds/Chunli/hump");
            soundPlayer.medium = Content.Load<SoundEffect>("3rdStrikeSounds/Chunli/yea");
            soundPlayer.heavy = Content.Load<SoundEffect>("3rdStrikeSounds/Chunli/ehh");

            soundPlayer.sp1 = Content.Load<SoundEffect>("3rdStrikeSounds/Chunli/kikoken");
            soundPlayer.sp2 = Content.Load<SoundEffect>("3rdStrikeSounds/Chunli/sbk");
            soundPlayer.sp3 = Content.Load<SoundEffect>("3rdStrikeSounds/Chunli/hazanshu");

            soundPlayer.win = Content.Load<SoundEffect>("3rdStrikeSounds/Chunli/win");
            soundPlayer.intro = Content.Load<SoundEffect>("3rdStrikeSounds/Chunli/intro");

            soundPlayer.jumpRise = Content.Load<SoundEffect>("3rdStrikeSounds/jumpRise");
            soundPlayer.jumpLand = Content.Load<SoundEffect>("3rdStrikeSounds/jumpLanding");

            soundPlayer.hit = Content.Load<SoundEffect>("3rdStrikeSounds/Chunli/hurt");
            
            // attack block

            light = new Attack(input, transform, playerMovement, FighterState.attackStartup, 10);
            ActionFrame actionFrame = new ActionFrame(3);
            actionFrame.setAttack(new Hitbox(20,1, 9, 7, new Vector2(-7, 0), new Rectangle(0, 0, 70, 10), new Vector2(50, 90), CancelState.light, HitSpark.light));
            light.AddActionFrame(actionFrame, 2);
            actionFrame = new ActionFrame(3);
            actionFrame.optionalSound = soundPlayer.light;
            light.AddActionFrame(actionFrame);

            medium = new Attack(input, transform, playerMovement, FighterState.attackStartup, 22);
            actionFrame = new ActionFrame(2);
            actionFrame.setMovement(new Vector2(10, 0));
            medium.AddActionFrame(actionFrame);
            actionFrame = new ActionFrame(6);
            actionFrame.setAttack(new Hitbox(50, 1, 16, 13, new Vector2(-5, 0), new Rectangle(0, 0, 100, 10), new Vector2(50, 20), CancelState.medium, HitSpark.medium));
            medium.AddActionFrame(actionFrame, 2);
            actionFrame = new ActionFrame(3);
            actionFrame.optionalSound = soundPlayer.medium;
            medium.AddActionFrame(actionFrame);

            heavy = new Attack(input, transform, playerMovement, FighterState.attackStartup, 32);
            actionFrame = new ActionFrame(7);
            actionFrame.setMovement(new Vector2(10, 0));
            heavy.AddActionFrame(actionFrame);
            actionFrame = new ActionFrame(8);
            actionFrame.setMovement(new Vector2(40, 0));
            actionFrame.setAttack(new Hitbox(85, 1, 23, 11, new Vector2(-5, 0), new Rectangle(0, 0, 60, 10), new Vector2(57, 75), CancelState.heavy, HitSpark.heavy));
            heavy.AddActionFrame(actionFrame);
            actionFrame = new ActionFrame(26);
            actionFrame.setMovement(new Vector2(-15, 0));
            heavy.AddActionFrame(actionFrame);
            actionFrame = new ActionFrame(32);
            actionFrame.setMovement(new Vector2(-15, 0));
            heavy.AddActionFrame(actionFrame);
            actionFrame = new ActionFrame(3);
            actionFrame.optionalSound = soundPlayer.heavy;
            heavy.AddActionFrame(actionFrame);

            jumpLight = new Attack(input, transform, playerMovement, FighterState.jumpingAttack, 30);
            jumpLight.isJumpingAttack = true;
            actionFrame = new ActionFrame(3);
            actionFrame.setAttack(new Hitbox(50, 1, 10, 8, new Vector2(-5, 0), new Rectangle(0, 0, 20, 20), new Vector2(55, 70), CancelState.none, HitSpark.light));
            jumpLight.AddActionFrame(actionFrame, 3);

            jumpMediumFollowup = new Attack(input, transform, playerMovement, FighterState.jumpingAttack, 40);
            for (int i = 0; i < 10; i++)
            {
                actionFrame = new ActionFrame(i);
                actionFrame.setMovement(new Vector2(6, i));
                jumpMediumFollowup.AddActionFrame(actionFrame, 1);
            }
            for (int i = 10; i < 20; i++)
            {
                actionFrame = new ActionFrame(i);
                actionFrame.setMovement(new Vector2(6, -i+10));
                jumpMediumFollowup.AddActionFrame(actionFrame, 1);
            }
            actionFrame = new ActionFrame(20);
            actionFrame.setMovement(new Vector2(3, -10));
            jumpMediumFollowup.AddActionFrame(actionFrame, 5);
            actionFrame = new ActionFrame(25);
            actionFrame.setMovement(new Vector2(0, -10));
            jumpMediumFollowup.AddActionFrame(actionFrame, 15);

            jumpMedium = new Attack(input, transform, playerMovement, FighterState.jumpingAttack, 30);
            jumpMedium.isJumpingAttack = true;
            actionFrame = new ActionFrame(3);
            actionFrame.optionalHitFunction = () => { playerMovement.StartAttack(jumpMediumFollowup, FighterAnimations.jumpRising);  playerMovement.CancelJump(); };
            actionFrame.setAttack(new Hitbox(35, 1, 35, 30, new Vector2(-5, 0), new Rectangle(0, 0, 20, 70), new Vector2(0, 40), CancelState.none, HitSpark.medium));
            jumpMedium.AddActionFrame(actionFrame, 3);

            


            jumpHeavy = new Attack(input, transform, playerMovement, FighterState.jumpingAttack,30);
            jumpHeavy.isJumpingAttack = true;
            actionFrame = new ActionFrame(7);
            actionFrame.setAttack(new Hitbox(80, 1, 20, 15, new Vector2(-5, 0), new Rectangle(0, 0, 80, 25), new Vector2(70, 80), CancelState.none, HitSpark.heavy));
            jumpHeavy.AddActionFrame(actionFrame, 6);

            sp1 = new Attack(input, transform, playerMovement, FighterState.attackStartup, 54);
            actionFrame = new ActionFrame(14);
            actionFrame.optionalFunction = ActivateFireabll;
            sp1.AddActionFrame(actionFrame);
            actionFrame = new ActionFrame(3);
            actionFrame.optionalSound = soundPlayer.sp1;
            sp1.AddActionFrame(actionFrame);

            sp2 = new Attack(input, transform, playerMovement, FighterState.attackStartup, 45);
            actionFrame = new ActionFrame(3);
            actionFrame.optionalFunction =  () => { playerMovement.SetState(FighterState.invincible); };
            sp2.AddActionFrame(actionFrame);
            actionFrame = new ActionFrame(8);
            actionFrame.setMovement(new Vector2(3, 1));
            sp2.AddActionFrame(actionFrame, 12);
            actionFrame = new ActionFrame(12);
            actionFrame.setAttack(new Hitbox(15, 1, 16, 10, new Vector2(-5, 0), new Rectangle(0, 0, 100, 10), new Vector2(30, 80), CancelState.special, HitSpark.special));
            sp2.AddActionFrame(actionFrame, 2);
            actionFrame = new ActionFrame(15);
            actionFrame.optionalFunction = () => { playerMovement.SetState(FighterState.attackRecovery); };
            sp2.AddActionFrame(actionFrame);
            actionFrame = new ActionFrame(18);
            actionFrame.setAttack(new Hitbox(15, 1, 16, 10, new Vector2(-5, 0), new Rectangle(0, 0, 100, 10), new Vector2(30, 80), CancelState.special, HitSpark.special));
            sp2.AddActionFrame(actionFrame, 2);
            actionFrame = new ActionFrame(20);
            actionFrame.setMovement(new Vector2(3, -1));
            sp2.AddActionFrame(actionFrame, 12);
            actionFrame = new ActionFrame(22);
            actionFrame.setAttack(new Hitbox(15, 1, 24, 10, new Vector2(-5, 0), new Rectangle(0, 0, 100, 10), new Vector2(30, 80), CancelState.special, HitSpark.special));
            sp2.AddActionFrame(actionFrame, 2);
            actionFrame = new ActionFrame(3);
            actionFrame.optionalSound = soundPlayer.sp2;
            sp2.AddActionFrame(actionFrame);

            sp3 = new Attack(input, transform, playerMovement, FighterState.attackStartup, 40);
            actionFrame = new ActionFrame(0);
            actionFrame.setMovement(new Vector2(7, 10));
            sp3.AddActionFrame(actionFrame, 8);
            actionFrame = new ActionFrame(8);
            actionFrame.optionalFunction = () => { playerMovement.SetState(FighterState.projectileInvincible); };
            actionFrame.setMovement(new Vector2(7, -10));
            sp3.AddActionFrame(actionFrame, 8);
            actionFrame = new ActionFrame(16);
            actionFrame.setAttack(new Hitbox(85, 1, 23, 15, new Vector2(-5, 0), new Rectangle(0, 0, 70, 10), new Vector2(50, 10), CancelState.special, HitSpark.special));
            sp3.AddActionFrame(actionFrame, 3);
            actionFrame = new ActionFrame(19);
            actionFrame.optionalFunction = () => { playerMovement.SetState(FighterState.attackRecovery); };
            sp3.AddActionFrame(actionFrame, 1);
            actionFrame = new ActionFrame(3);
            actionFrame.optionalSound = soundPlayer.sp3;
            sp3.AddActionFrame(actionFrame);

            throwAttack = new Attack(input, transform, playerMovement, FighterState.invincible, 40);
            actionFrame = new ActionFrame(0);
            actionFrame.optionalFunction = () => { playerMovement.EnableMovementCollision(false); };
            throwAttack.AddActionFrame(actionFrame);
            actionFrame = new ActionFrame(0);
            actionFrame.setMovement(new Vector2(7, 0));
            throwAttack.AddActionFrame(actionFrame, 15);
            actionFrame = new ActionFrame(15);
            actionFrame.optionalFunction = () => { playerMovement.EnableMovementCollision(true); };
            throwAttack.AddActionFrame(actionFrame);

            throwTry = new Attack(input, transform, playerMovement, FighterState.attackStartup, 40);
            actionFrame = new ActionFrame(6);
            actionFrame.setAttack(new Hitbox(150, 0, 40, 0, new Vector2(-5, 0), new Rectangle(0, 0, 80, 10), new Vector2(30, 80), CancelState.none, HitSpark.light, AttackProperty.Throw, throwType: ThrowType.chun));
            actionFrame.optionalHitFunction = () => { playerMovement.StartAttack(throwAttack, FighterAnimations.throwComplete); };
            throwTry.AddActionFrame(actionFrame);

            chunLiThrown = new Attack(input, transform, playerMovement, FighterState.invincible, 95);
            actionFrame = new ActionFrame(16);
            actionFrame.setMovement(new Vector2(-20, 0));
            actionFrame.optionalFunction = () => { playerMovement.EnableMovementCollision(false); };
            chunLiThrown.AddActionFrame(actionFrame, 4);
            actionFrame = new ActionFrame(20);
            actionFrame.optionalFunction = () => { animator.PlayAnimation(FighterAnimations.knockdown); };
            chunLiThrown.AddActionFrame(actionFrame);
            actionFrame = new ActionFrame(21);
            actionFrame.setMovement(new Vector2(-8, 0));
           chunLiThrown.AddActionFrame(actionFrame, 10);
            actionFrame = new ActionFrame(31);
            actionFrame.optionalFunction = () => { playerMovement.EnableMovementCollision(true); };
            chunLiThrown.AddActionFrame(actionFrame);

            int legsHitStop = 4;

            super = new Attack(input, transform, playerMovement, FighterState.attackStartup, 90);
            actionFrame = new ActionFrame(2);
            actionFrame.optionalFunction = superHitStopAndEffect;
            //actionFrame.setOptionalSound(MasterSound.super);
            super.AddActionFrame(actionFrame, 1);
            
            actionFrame = new ActionFrame(5);
            actionFrame.setMovement(new Vector2(10, 0));
            super.AddActionFrame(actionFrame, 11);
            actionFrame = new ActionFrame(17);
            actionFrame.setAttack(new Hitbox(20, 1, 16, 10, new Vector2(0, 0), new Rectangle(0, 0, 70, 10), new Vector2(40, 50), CancelState.none, HitSpark.special, hitStop: legsHitStop));
            super.AddActionFrame(actionFrame, 1);
            actionFrame = new ActionFrame(19);
            actionFrame.setAttack(new Hitbox(20, 1, 16, 10, new Vector2(0, 0), new Rectangle(0, 0, 70, 10), new Vector2(40, 50), CancelState.none, HitSpark.special, hitStop: legsHitStop));
            super.AddActionFrame(actionFrame, 1);
            actionFrame = new ActionFrame(21);
            actionFrame.setAttack(new Hitbox(20, 1, 16, 10, new Vector2(0, 0), new Rectangle(0, 0, 70, 10), new Vector2(40, 100), CancelState.none, HitSpark.special, hitStop: legsHitStop));
            super.AddActionFrame(actionFrame, 1);
            actionFrame = new ActionFrame(23);
            actionFrame.setAttack(new Hitbox(20, 1, 16, 10, new Vector2(0, 0), new Rectangle(0, 0, 70, 10), new Vector2(40, 80), CancelState.none, HitSpark.special, hitStop: legsHitStop));
            super.AddActionFrame(actionFrame, 1);            
            actionFrame = new ActionFrame(25);
            actionFrame.setAttack(new Hitbox(20, 1, 16, 10, new Vector2(0, 0), new Rectangle(0, 0, 70, 10), new Vector2(40, 100), CancelState.none, HitSpark.special, hitStop: legsHitStop));
            super.AddActionFrame(actionFrame, 1);
            actionFrame = new ActionFrame(27);
            actionFrame.setAttack(new Hitbox(20, 1, 16, 10, new Vector2(0, 0), new Rectangle(0, 0, 70, 10), new Vector2(40, 100), CancelState.none, HitSpark.special, hitStop: legsHitStop));
            super.AddActionFrame(actionFrame, 1);
            actionFrame = new ActionFrame(29);
            actionFrame.setAttack(new Hitbox(20, 1, 16, 10, new Vector2(0, 0), new Rectangle(0, 0, 70, 10), new Vector2(40, 70), CancelState.none, HitSpark.special, hitStop: legsHitStop));
            super.AddActionFrame(actionFrame, 1);
            actionFrame = new ActionFrame(31);
            actionFrame.setAttack(new Hitbox(20, 1, 16, 10, new Vector2(-10, 0), new Rectangle(0, 0, 70, 10), new Vector2(40, 90), CancelState.none, HitSpark.special, hitStop: legsHitStop));
            super.AddActionFrame(actionFrame, 1);           
            actionFrame = new ActionFrame(35);
            actionFrame.setMovement(new Vector2(10, 0));
            super.AddActionFrame(actionFrame, 6);
            actionFrame = new ActionFrame(41);
            actionFrame.setAttack(new Hitbox(20, 1, 16, 10, new Vector2(0, 0), new Rectangle(0, 0, 70, 10), new Vector2(40, 100), CancelState.none, HitSpark.special, hitStop: legsHitStop));
            super.AddActionFrame(actionFrame, 1);
            actionFrame = new ActionFrame(43);
            actionFrame.setAttack(new Hitbox(20, 1, 16, 10, new Vector2(0, 0), new Rectangle(0, 0, 70, 10), new Vector2(40, 70), CancelState.none, HitSpark.special, hitStop: legsHitStop));
            super.AddActionFrame(actionFrame, 1);
            actionFrame = new ActionFrame(45);
            actionFrame.setAttack(new Hitbox(20, 1, 16, 10, new Vector2(0, 0), new Rectangle(0, 0, 70, 10), new Vector2(40, 80), CancelState.none, HitSpark.special, hitStop: legsHitStop));
            super.AddActionFrame(actionFrame, 1);
            actionFrame = new ActionFrame(47);
            actionFrame.setAttack(new Hitbox(20, 1, 16, 10, new Vector2(0, 0), new Rectangle(0, 0, 70, 10), new Vector2(40, 50), CancelState.none, HitSpark.special, hitStop: legsHitStop));
            super.AddActionFrame(actionFrame, 1);            
            actionFrame = new ActionFrame(49);
            actionFrame.setAttack(new Hitbox(20, 1, 16, 10, new Vector2(0, 0), new Rectangle(0, 0, 70, 10), new Vector2(40, 100), CancelState.none, HitSpark.special, hitStop: legsHitStop));
            super.AddActionFrame(actionFrame, 1);
            actionFrame = new ActionFrame(51);
            actionFrame.setAttack(new Hitbox(20, 1, 16, 10, new Vector2(0, 0), new Rectangle(0, 0, 70, 10), new Vector2(40, 70), CancelState.none, HitSpark.special, hitStop: legsHitStop));
            super.AddActionFrame(actionFrame, 1);
            actionFrame = new ActionFrame(53);
            actionFrame.setAttack(new Hitbox(20, 1, 16, 10, new Vector2(0, 0), new Rectangle(0, 0, 70, 10), new Vector2(40, 80), CancelState.none, HitSpark.special, hitStop: legsHitStop));
            super.AddActionFrame(actionFrame, 1);
            actionFrame = new ActionFrame(55);
            actionFrame.setAttack(new Hitbox(20, 1, 16, 10, new Vector2(-10, 0), new Rectangle(0, 0, 70, 10), new Vector2(40, 50), CancelState.none, HitSpark.special, hitStop: legsHitStop));
            super.AddActionFrame(actionFrame, 1);
            actionFrame = new ActionFrame(62);
            actionFrame.setMovement(new Vector2(10, 0));
            super.AddActionFrame(actionFrame, 7);
            actionFrame = new ActionFrame(68);
            actionFrame.setAttack(new Hitbox(80, 1, 16, 10, new Vector2(-3, 10), new Rectangle(0, 0, 30, 120), new Vector2(10, 80), CancelState.none, HitSpark.special, AttackProperty.Launcher, 15));
            super.AddActionFrame(actionFrame, 1);

            
            jumpForward = new Attack(input, transform, playerMovement, FighterState.jumping, 43);
            //actionFrame = new ActionFrame(3);
            //actionFrame.optionalSound = soundPlayer.jumpRise;
            //jumpForward.AddActionFrame(actionFrame);
            actionFrame = new ActionFrame(3);
            actionFrame.setMovement(new Vector2(4, 7));
            jumpForward.AddActionFrame(actionFrame, 13);
            actionFrame = new ActionFrame(16);
            actionFrame.setMovement(new Vector2(4, 5));
            jumpForward.AddActionFrame(actionFrame, 1);
            actionFrame = new ActionFrame(17);
            actionFrame.setMovement(new Vector2(4, 2));
            jumpForward.AddActionFrame(actionFrame, 1);
            actionFrame = new ActionFrame(18);
            actionFrame.setMovement(new Vector2(4, 1));
            jumpForward.AddActionFrame(actionFrame, 1);
            actionFrame = new ActionFrame(19);
            actionFrame.setMovement(new Vector2(4, 0));
            jumpForward.AddActionFrame(actionFrame, 1);
            actionFrame = new ActionFrame(20);
            actionFrame.setMovement(new Vector2(4, 0));
            jumpForward.AddActionFrame(actionFrame, 1);
            actionFrame = new ActionFrame(21);
            actionFrame.setMovement(new Vector2(4, -1));
            jumpForward.AddActionFrame(actionFrame, 1);
            actionFrame = new ActionFrame(22);
            actionFrame.setMovement(new Vector2(4, -2));
            jumpForward.AddActionFrame(actionFrame, 1);
            actionFrame = new ActionFrame(23);
            actionFrame.setMovement(new Vector2(4, -5));
            jumpForward.AddActionFrame(actionFrame, 1);
            actionFrame = new ActionFrame(24);
            actionFrame.setMovement(new Vector2(4, -7));
            jumpForward.AddActionFrame(actionFrame, 13);
            //actionFrame = new ActionFrame(38);
            //actionFrame.optionalSound = soundPlayer.jumpLand;
            //jumpForward.AddActionFrame(actionFrame);

            jumpBack = new Attack(input, transform, playerMovement, FighterState.jumping, 43);
            //actionFrame = new ActionFrame(3);
            //actionFrame.optionalSound = soundPlayer.jumpRise;
            //jumpBack.AddActionFrame(actionFrame);
            actionFrame = new ActionFrame(3);
            actionFrame.setMovement(new Vector2(-4, 7));            
            jumpBack.AddActionFrame(actionFrame, 13);
            actionFrame = new ActionFrame(16);
            actionFrame.setMovement(new Vector2(-4, 5));
            jumpBack.AddActionFrame(actionFrame, 1);
            actionFrame = new ActionFrame(17);
            actionFrame.setMovement(new Vector2(-4, 2));
            jumpBack.AddActionFrame(actionFrame, 1);
            actionFrame = new ActionFrame(18);
            actionFrame.setMovement(new Vector2(-4, 1));
            jumpBack.AddActionFrame(actionFrame, 1);
            actionFrame = new ActionFrame(19);
            actionFrame.setMovement(new Vector2(-4, 0));
            jumpBack.AddActionFrame(actionFrame, 1);
            actionFrame = new ActionFrame(20);
            actionFrame.setMovement(new Vector2(-4, 0));
            jumpBack.AddActionFrame(actionFrame, 1);
            actionFrame = new ActionFrame(21);
            actionFrame.setMovement(new Vector2(-4, -1));
            jumpBack.AddActionFrame(actionFrame, 1);
            actionFrame = new ActionFrame(22);
            actionFrame.setMovement(new Vector2(-4, -2));
            jumpBack.AddActionFrame(actionFrame, 1);
            actionFrame = new ActionFrame(23);
            actionFrame.setMovement(new Vector2(-4, -5));
            jumpBack.AddActionFrame(actionFrame, 1);
            actionFrame = new ActionFrame(24);
            actionFrame.setMovement(new Vector2(-4, -7));
            jumpBack.AddActionFrame(actionFrame, 13);
            //actionFrame = new ActionFrame(38);
            //actionFrame.optionalSound = soundPlayer.jumpLand;
            //jumpBack.AddActionFrame(actionFrame);

            jumpNeutral = new Attack(input, transform, playerMovement, FighterState.jumping, 43);
            //actionFrame = new ActionFrame(3);
            //actionFrame.optionalSound = soundPlayer.jumpRise;
            //jumpNeutral.AddActionFrame(actionFrame);
            actionFrame = new ActionFrame(3);
            actionFrame.setMovement(new Vector2(0, 7));
            jumpNeutral.AddActionFrame(actionFrame, 13);
            actionFrame = new ActionFrame(16);
            actionFrame.setMovement(new Vector2(0, 5));
            jumpNeutral.AddActionFrame(actionFrame, 1);
            actionFrame = new ActionFrame(17);
            actionFrame.setMovement(new Vector2(0, 2));
            jumpNeutral.AddActionFrame(actionFrame, 1);
            actionFrame = new ActionFrame(18);
            actionFrame.setMovement(new Vector2(0, 1));
            jumpNeutral.AddActionFrame(actionFrame, 1);
            actionFrame = new ActionFrame(19);
            actionFrame.setMovement(new Vector2(0, 0));
            jumpNeutral.AddActionFrame(actionFrame, 1);
            actionFrame = new ActionFrame(20);
            actionFrame.setMovement(new Vector2(0, 0));
            jumpNeutral.AddActionFrame(actionFrame, 1);
            actionFrame = new ActionFrame(21);
            actionFrame.setMovement(new Vector2(0, -1));
            jumpNeutral.AddActionFrame(actionFrame, 1);
            actionFrame = new ActionFrame(22);
            actionFrame.setMovement(new Vector2(0, -2));
            jumpNeutral.AddActionFrame(actionFrame, 1);
            actionFrame = new ActionFrame(23);
            actionFrame.setMovement(new Vector2(0, -5));
            jumpNeutral.AddActionFrame(actionFrame, 1);
            actionFrame = new ActionFrame(24);
            actionFrame.setMovement(new Vector2(0, -7));
            jumpNeutral.AddActionFrame(actionFrame, 13);
            //actionFrame = new ActionFrame(38);
            //actionFrame.optionalSound = soundPlayer.jumpLand;
            //jumpNeutral.AddActionFrame(actionFrame);

            winAttack = new Attack(input, transform, playerMovement, FighterState.invincible, 500);
            introAttack = new Attack(input, transform, playerMovement, FighterState.invincible, 60);

            health.SetThrows(chunLiThrown);

            Animation walkingAnimation = new Animation(animationType.looping);
            walkingAnimation.addFrame(Content.Load<Texture2D>("walkToward/SF3_3rd_Chunli_23072"), 1);
            walkingAnimation.addFrame(Content.Load<Texture2D>("walkToward/SF3_3rd_Chunli_23073"), 1);
            walkingAnimation.addFrame(Content.Load<Texture2D>("walkToward/SF3_3rd_Chunli_23074"), 1);
            walkingAnimation.addFrame(Content.Load<Texture2D>("walkToward/SF3_3rd_Chunli_23075"), 1);
            walkingAnimation.addFrame(Content.Load<Texture2D>("walkToward/SF3_3rd_Chunli_23076"), 1);
            walkingAnimation.addFrame(Content.Load<Texture2D>("walkToward/SF3_3rd_Chunli_23077"), 1);
            walkingAnimation.addFrame(Content.Load<Texture2D>("walkToward/SF3_3rd_Chunli_23078"), 1);
            walkingAnimation.addFrame(Content.Load<Texture2D>("walkToward/SF3_3rd_Chunli_23079"), 1);
            walkingAnimation.addFrame(Content.Load<Texture2D>("walkToward/SF3_3rd_Chunli_23080"), 1);
            walkingAnimation.addFrame(Content.Load<Texture2D>("walkToward/SF3_3rd_Chunli_23081"), 1);
            walkingAnimation.addFrame(Content.Load<Texture2D>("walkToward/SF3_3rd_Chunli_23082"), 1);
            walkingAnimation.addFrame(Content.Load<Texture2D>("walkToward/SF3_3rd_Chunli_23083"), 1);
            walkingAnimation.addFrame(Content.Load<Texture2D>("walkToward/SF3_3rd_Chunli_23084"), 1);
            walkingAnimation.addFrame(Content.Load<Texture2D>("walkToward/SF3_3rd_Chunli_23085"), 1);
            walkingAnimation.addFrame(Content.Load<Texture2D>("walkToward/SF3_3rd_Chunli_23086"), 1);
            walkingAnimation.addFrame(Content.Load<Texture2D>("walkToward/SF3_3rd_Chunli_23087"), 1);


            Animation neutralAnimation = new Animation(animationType.pingPong);
            neutralAnimation.addFrame(Content.Load<Texture2D>("neutral/SF3_3rd_Chunli_23056"), 3);
            neutralAnimation.addFrame(Content.Load<Texture2D>("neutral/SF3_3rd_Chunli_23057"), 3);
            neutralAnimation.addFrame(Content.Load<Texture2D>("neutral/SF3_3rd_Chunli_23058"), 3);
            neutralAnimation.addFrame(Content.Load<Texture2D>("neutral/SF3_3rd_Chunli_23059"), 3);
            neutralAnimation.addFrame(Content.Load<Texture2D>("neutral/SF3_3rd_Chunli_23060"), 3);
            neutralAnimation.addFrame(Content.Load<Texture2D>("neutral/SF3_3rd_Chunli_23061"), 3);

            Animation walkBackAnimation = new Animation(animationType.looping);
            walkBackAnimation.addFrame(Content.Load<Texture2D>("walkBack/SF3_3rd_Chunli_23104"), 2);
            walkBackAnimation.addFrame(Content.Load<Texture2D>("walkBack/SF3_3rd_Chunli_23105"), 2);
            walkBackAnimation.addFrame(Content.Load<Texture2D>("walkBack/SF3_3rd_Chunli_23106"), 2);
            walkBackAnimation.addFrame(Content.Load<Texture2D>("walkBack/SF3_3rd_Chunli_23107"), 2);
            walkBackAnimation.addFrame(Content.Load<Texture2D>("walkBack/SF3_3rd_Chunli_23108"), 2);
            walkBackAnimation.addFrame(Content.Load<Texture2D>("walkBack/SF3_3rd_Chunli_23109"), 2);
            walkBackAnimation.addFrame(Content.Load<Texture2D>("walkBack/SF3_3rd_Chunli_23110"), 2);
            walkBackAnimation.addFrame(Content.Load<Texture2D>("walkBack/SF3_3rd_Chunli_23111"), 2);
            walkBackAnimation.addFrame(Content.Load<Texture2D>("walkBack/SF3_3rd_Chunli_23112"), 2);
            walkBackAnimation.addFrame(Content.Load<Texture2D>("walkBack/SF3_3rd_Chunli_23113"), 2);
            walkBackAnimation.addFrame(Content.Load<Texture2D>("walkBack/SF3_3rd_Chunli_23114"), 2);
            walkBackAnimation.addFrame(Content.Load<Texture2D>("walkBack/SF3_3rd_Chunli_23115"), 2);
            walkBackAnimation.addFrame(Content.Load<Texture2D>("walkBack/SF3_3rd_Chunli_23116"), 2);
            walkBackAnimation.addFrame(Content.Load<Texture2D>("walkBack/SF3_3rd_Chunli_23117"), 2);
            walkBackAnimation.addFrame(Content.Load<Texture2D>("walkBack/SF3_3rd_Chunli_23118"), 2);

            Animation jumpAnimation = new Animation(animationType.oneShot);
            jumpAnimation.addFrame(Content.Load<Texture2D>("neutralJump/SF3_3rd_Chunli_23200"), 2);
            jumpAnimation.addFrame(Content.Load<Texture2D>("neutralJump/SF3_3rd_Chunli_23201"), 2);
            jumpAnimation.addFrame(Content.Load<Texture2D>("neutralJump/SF3_3rd_Chunli_23202"), 2);
            jumpAnimation.addFrame(Content.Load<Texture2D>("neutralJump/SF3_3rd_Chunli_23203"), 2);
            jumpAnimation.addFrame(Content.Load<Texture2D>("neutralJump/SF3_3rd_Chunli_23204"), 2);
            jumpAnimation.addFrame(Content.Load<Texture2D>("neutralJump/SF3_3rd_Chunli_23205"), 2);
            jumpAnimation.addFrame(Content.Load<Texture2D>("neutralJump/SF3_3rd_Chunli_23206"), 2);
            jumpAnimation.addFrame(Content.Load<Texture2D>("neutralJump/SF3_3rd_Chunli_23207"), 2);
            jumpAnimation.addFrame(Content.Load<Texture2D>("neutralJump/SF3_3rd_Chunli_23208"), 2);
            jumpAnimation.addFrame(Content.Load<Texture2D>("neutralJump/SF3_3rd_Chunli_23209"), 2);
            jumpAnimation.addFrame(Content.Load<Texture2D>("neutralJump/SF3_3rd_Chunli_23210"), 2);
            jumpAnimation.addFrame(Content.Load<Texture2D>("neutralJump/SF3_3rd_Chunli_23211"), 2);
            jumpAnimation.addFrame(Content.Load<Texture2D>("neutralJump/SF3_3rd_Chunli_23212"), 2);
            jumpAnimation.addFrame(Content.Load<Texture2D>("neutralJump/SF3_3rd_Chunli_23213"), 2);
            jumpAnimation.addFrame(Content.Load<Texture2D>("neutralJump/SF3_3rd_Chunli_23214"), 2);
            jumpAnimation.addFrame(Content.Load<Texture2D>("neutralJump/SF3_3rd_Chunli_23215"), 2);
            jumpAnimation.addFrame(Content.Load<Texture2D>("neutralJump/SF3_3rd_Chunli_23216"), 2);
            jumpAnimation.addFrame(Content.Load<Texture2D>("neutralJump/SF3_3rd_Chunli_23217"), 2);
            jumpAnimation.addFrame(Content.Load<Texture2D>("neutralJump/SF3_3rd_Chunli_23218"), 2);
            jumpAnimation.addFrame(Content.Load<Texture2D>("neutralJump/SF3_3rd_Chunli_23219"), 2);
            jumpAnimation.addFrame(Content.Load<Texture2D>("neutralJump/SF3_3rd_Chunli_23220"), 2);
            jumpAnimation.addFrame(Content.Load<Texture2D>("neutralJump/SF3_3rd_Chunli_23221"), 2);
            jumpAnimation.addFrame(Content.Load<Texture2D>("neutralJump/SF3_3rd_Chunli_23222"), 2);
            jumpAnimation.addFrame(Content.Load<Texture2D>("neutralJump/SF3_3rd_Chunli_23223"), 2);
            jumpAnimation.addFrame(Content.Load<Texture2D>("neutralJump/SF3_3rd_Chunli_23224"), 2);
            jumpAnimation.addFrame(Content.Load<Texture2D>("neutralJump/SF3_3rd_Chunli_23225"), 2);

            Animation lightAnimation = new Animation(animationType.oneShot);
            lightAnimation.addFrame(Content.Load<Texture2D>("attackLight/SF3_3rd_Chunli_23872"), 1);
            lightAnimation.addFrame(Content.Load<Texture2D>("attackLight/SF3_3rd_Chunli_23873"), 1);
            lightAnimation.addFrame(Content.Load<Texture2D>("attackLight/SF3_3rd_Chunli_23874"), 1);
            lightAnimation.addFrame(Content.Load<Texture2D>("attackLight/SF3_3rd_Chunli_23875"), 1);
            lightAnimation.addFrame(Content.Load<Texture2D>("attackLight/SF3_3rd_Chunli_23876"), 1);

            Animation mediumAnimation = new Animation(animationType.oneShot);
            mediumAnimation.addFrame(Content.Load<Texture2D>("attackMedium/SF3_3rd_Chunli_24176"), 0);
            mediumAnimation.addFrame(Content.Load<Texture2D>("attackMedium/SF3_3rd_Chunli_24177"), 2);
            mediumAnimation.addFrame(Content.Load<Texture2D>("attackMedium/SF3_3rd_Chunli_24178"), 1);
            mediumAnimation.addFrame(Content.Load<Texture2D>("attackMedium/SF3_3rd_Chunli_24179"), 1);
            mediumAnimation.addFrame(Content.Load<Texture2D>("attackMedium/SF3_3rd_Chunli_24180"), 1);
            mediumAnimation.addFrame(Content.Load<Texture2D>("attackMedium/SF3_3rd_Chunli_24181"), 2);
            mediumAnimation.addFrame(Content.Load<Texture2D>("attackMedium/SF3_3rd_Chunli_24182"), 1);
            mediumAnimation.addFrame(Content.Load<Texture2D>("attackMedium/SF3_3rd_Chunli_24183"), 2);
            mediumAnimation.addFrame(Content.Load<Texture2D>("attackMedium/SF3_3rd_Chunli_24184"), 4);

            Animation heavyAnimation = new Animation(animationType.oneShot);
            heavyAnimation.addFrame(Content.Load<Texture2D>("attackHeavy/SF3_3rd_Chunli_23904"), 1);
            heavyAnimation.addFrame(Content.Load<Texture2D>("attackHeavy/SF3_3rd_Chunli_23905"), 2);
            heavyAnimation.addFrame(Content.Load<Texture2D>("attackHeavy/SF3_3rd_Chunli_23906"), 1);
            heavyAnimation.addFrame(Content.Load<Texture2D>("attackHeavy/SF3_3rd_Chunli_23907"), 0);
            heavyAnimation.addFrame(Content.Load<Texture2D>("attackHeavy/SF3_3rd_Chunli_23908"), 1);
            heavyAnimation.addFrame(Content.Load<Texture2D>("attackHeavy/SF3_3rd_Chunli_23909"), 1);
            heavyAnimation.addFrame(Content.Load<Texture2D>("attackHeavy/SF3_3rd_Chunli_23910"), 0);
            heavyAnimation.addFrame(Content.Load<Texture2D>("attackHeavy/SF3_3rd_Chunli_23911"), 0);
            heavyAnimation.addFrame(Content.Load<Texture2D>("attackHeavy/SF3_3rd_Chunli_23912"), 2);
            heavyAnimation.addFrame(Content.Load<Texture2D>("attackHeavy/SF3_3rd_Chunli_23913"), 2);
            heavyAnimation.addFrame(Content.Load<Texture2D>("attackHeavy/SF3_3rd_Chunli_23914"), 2);
            heavyAnimation.addFrame(Content.Load<Texture2D>("attackHeavy/SF3_3rd_Chunli_23915"), 2);
            heavyAnimation.addFrame(Content.Load<Texture2D>("attackHeavy/SF3_3rd_Chunli_23916"), 2);

            Animation jumpLightAnimation = new Animation(animationType.looping);
            jumpLightAnimation.addFrame(Content.Load<Texture2D>("jumpLight/SF3_3rd_Chunli_24256"), 1);
            jumpLightAnimation.addFrame(Content.Load<Texture2D>("jumpLight/SF3_3rd_Chunli_24257"), 1);
            jumpLightAnimation.addFrame(Content.Load<Texture2D>("jumpLight/SF3_3rd_Chunli_24258"), 1);
            jumpLightAnimation.addFrame(Content.Load<Texture2D>("jumpLight/SF3_3rd_Chunli_24259"), 1);

            Animation jumpMediumAnimation = new Animation(animationType.looping);
            jumpMediumAnimation.addFrame(Content.Load<Texture2D>("jumpmedium/SF3_3rd_Chunli_24352"), 1);
            jumpMediumAnimation.addFrame(Content.Load<Texture2D>("jumpmedium/SF3_3rd_Chunli_24353"), 1);

            Animation jumpHeavyAnimation = new Animation(animationType.oneShot);
            jumpHeavyAnimation.addFrame(Content.Load<Texture2D>("jumpheavy/SF3_3rd_Chunli_24304"), 2);
            jumpHeavyAnimation.addFrame(Content.Load<Texture2D>("jumpheavy/SF3_3rd_Chunli_24305"), 2);
            jumpHeavyAnimation.addFrame(Content.Load<Texture2D>("jumpheavy/SF3_3rd_Chunli_24306"), 2);
            jumpHeavyAnimation.addFrame(Content.Load<Texture2D>("jumpheavy/SF3_3rd_Chunli_24307"), 1);
            jumpHeavyAnimation.addFrame(Content.Load<Texture2D>("jumpheavy/SF3_3rd_Chunli_24308"), 1);
            jumpHeavyAnimation.addFrame(Content.Load<Texture2D>("jumpheavy/SF3_3rd_Chunli_24309"), 1);
            jumpHeavyAnimation.addFrame(Content.Load<Texture2D>("jumpheavy/SF3_3rd_Chunli_24310"), 1);
            jumpHeavyAnimation.addFrame(Content.Load<Texture2D>("jumpheavy/SF3_3rd_Chunli_24311"), 1);
            jumpHeavyAnimation.addFrame(Content.Load<Texture2D>("jumpheavy/SF3_3rd_Chunli_24312"), 1);


            Animation sp1Animation = new Animation(animationType.oneShot);
            sp1Animation.addFrame(Content.Load<Texture2D>("sp1/SF3_3rd_Chunli_24528"), 0);
            sp1Animation.addFrame(Content.Load<Texture2D>("sp1/SF3_3rd_Chunli_24529"), 1);
            sp1Animation.addFrame(Content.Load<Texture2D>("sp1/SF3_3rd_Chunli_24530"), 1);
            sp1Animation.addFrame(Content.Load<Texture2D>("sp1/SF3_3rd_Chunli_24531"), 2);
            sp1Animation.addFrame(Content.Load<Texture2D>("sp1/SF3_3rd_Chunli_24532"), 1);
            sp1Animation.addFrame(Content.Load<Texture2D>("sp1/SF3_3rd_Chunli_24533"), 0);
            sp1Animation.addFrame(Content.Load<Texture2D>("sp1/SF3_3rd_Chunli_24534"), 0);
            sp1Animation.addFrame(Content.Load<Texture2D>("sp1/SF3_3rd_Chunli_24535"), 0);
            sp1Animation.addFrame(Content.Load<Texture2D>("sp1/SF3_3rd_Chunli_24536"), 1);
            sp1Animation.addFrame(Content.Load<Texture2D>("sp1/SF3_3rd_Chunli_24537"), 2);
            sp1Animation.addFrame(Content.Load<Texture2D>("sp1/SF3_3rd_Chunli_24538"), 2);
            sp1Animation.addFrame(Content.Load<Texture2D>("sp1/SF3_3rd_Chunli_24539"), 2);
            sp1Animation.addFrame(Content.Load<Texture2D>("sp1/SF3_3rd_Chunli_24540"), 2);
            sp1Animation.addFrame(Content.Load<Texture2D>("sp1/SF3_3rd_Chunli_24541"), 2);
            sp1Animation.addFrame(Content.Load<Texture2D>("sp1/SF3_3rd_Chunli_24542"), 3);
            sp1Animation.addFrame(Content.Load<Texture2D>("sp1/SF3_3rd_Chunli_24543"), 2);
            sp1Animation.addFrame(Content.Load<Texture2D>("sp1/SF3_3rd_Chunli_24544"), 13);
            sp1Animation.addFrame(Content.Load<Texture2D>("sp1/SF3_3rd_Chunli_24545"), 1);
            sp1Animation.addFrame(Content.Load<Texture2D>("sp1/SF3_3rd_Chunli_24546"), 1);
            sp1Animation.addFrame(Content.Load<Texture2D>("sp1/SF3_3rd_Chunli_24547"), 1);
            sp1Animation.addFrame(Content.Load<Texture2D>("sp1/SF3_3rd_Chunli_24548"), 1);

            Animation sp2Animation = new Animation(animationType.oneShot);
            sp2Animation.addFrame(Content.Load<Texture2D>("sp2/SF3_3rd_Chunli_24480"), 1);
            sp2Animation.addFrame(Content.Load<Texture2D>("sp2/SF3_3rd_Chunli_24481"), 0);
            sp2Animation.addFrame(Content.Load<Texture2D>("sp2/SF3_3rd_Chunli_24482"), 0);
            sp2Animation.addFrame(Content.Load<Texture2D>("sp2/SF3_3rd_Chunli_24483"), 0);
            sp2Animation.addFrame(Content.Load<Texture2D>("sp2/SF3_3rd_Chunli_24484"), 1);
            sp2Animation.addFrame(Content.Load<Texture2D>("sp2/SF3_3rd_Chunli_24485"), 1);
            sp2Animation.addFrame(Content.Load<Texture2D>("sp2/SF3_3rd_Chunli_24486"), 0);
            sp2Animation.addFrame(Content.Load<Texture2D>("sp2/SF3_3rd_Chunli_24487"), 0);
            sp2Animation.addFrame(Content.Load<Texture2D>("sp2/SF3_3rd_Chunli_24488"), 0);
            sp2Animation.addFrame(Content.Load<Texture2D>("sp2/SF3_3rd_Chunli_24489"), 0);
            sp2Animation.addFrame(Content.Load<Texture2D>("sp2/SF3_3rd_Chunli_24490"), 0);
            sp2Animation.addFrame(Content.Load<Texture2D>("sp2/SF3_3rd_Chunli_24491"), 0);
            sp2Animation.addFrame(Content.Load<Texture2D>("sp2/SF3_3rd_Chunli_24492"), 1);
            sp2Animation.addFrame(Content.Load<Texture2D>("sp2/SF3_3rd_Chunli_24493"), 0);
            sp2Animation.addFrame(Content.Load<Texture2D>("sp2/SF3_3rd_Chunli_24494"), 0);
            sp2Animation.addFrame(Content.Load<Texture2D>("sp2/SF3_3rd_Chunli_24495"), 0);
            sp2Animation.addFrame(Content.Load<Texture2D>("sp2/SF3_3rd_Chunli_24496"), 1);
            sp2Animation.addFrame(Content.Load<Texture2D>("sp2/SF3_3rd_Chunli_24497"), 0);
            sp2Animation.addFrame(Content.Load<Texture2D>("sp2/SF3_3rd_Chunli_24498"), 0);
            sp2Animation.addFrame(Content.Load<Texture2D>("sp2/SF3_3rd_Chunli_24499"), 2);
            sp2Animation.addFrame(Content.Load<Texture2D>("sp2/SF3_3rd_Chunli_24500"), 2);
            sp2Animation.addFrame(Content.Load<Texture2D>("sp2/SF3_3rd_Chunli_24501"), 1);
            sp2Animation.addFrame(Content.Load<Texture2D>("sp2/SF3_3rd_Chunli_24502"), 0);
            sp2Animation.addFrame(Content.Load<Texture2D>("sp2/SF3_3rd_Chunli_24503"), 0);
            sp2Animation.addFrame(Content.Load<Texture2D>("sp2/SF3_3rd_Chunli_24504"), 0);
            sp2Animation.addFrame(Content.Load<Texture2D>("sp2/SF3_3rd_Chunli_24505"), 0);
            sp2Animation.addFrame(Content.Load<Texture2D>("sp2/SF3_3rd_Chunli_24506"), 0);
            sp2Animation.addFrame(Content.Load<Texture2D>("sp2/SF3_3rd_Chunli_24507"), 0);
            sp2Animation.addFrame(Content.Load<Texture2D>("sp2/SF3_3rd_Chunli_24508"), 0);
            sp2Animation.addFrame(Content.Load<Texture2D>("sp2/SF3_3rd_Chunli_24509"), 1);
            sp2Animation.addFrame(Content.Load<Texture2D>("sp2/SF3_3rd_Chunli_24510"), 0);
            sp2Animation.addFrame(Content.Load<Texture2D>("sp2/SF3_3rd_Chunli_24511"), 1);
            sp2Animation.addFrame(Content.Load<Texture2D>("sp2/SF3_3rd_Chunli_24512"), 1);
            sp2Animation.addFrame(Content.Load<Texture2D>("sp2/SF3_3rd_Chunli_24513"), 1);

            Animation sp3Animation = new Animation(animationType.oneShot);
            sp3Animation.addFrame(Content.Load<Texture2D>("sp3/1"), 0);
            sp3Animation.addFrame(Content.Load<Texture2D>("sp3/2"), 0);
            sp3Animation.addFrame(Content.Load<Texture2D>("sp3/SF3_3rd_Chunli_23526"), 0);
            sp3Animation.addFrame(Content.Load<Texture2D>("sp3/SF3_3rd_Chunli_23527"), 0);
            sp3Animation.addFrame(Content.Load<Texture2D>("sp3/SF3_3rd_Chunli_23528"), 0);
            sp3Animation.addFrame(Content.Load<Texture2D>("sp3/SF3_3rd_Chunli_23529"), 0);
            sp3Animation.addFrame(Content.Load<Texture2D>("sp3/SF3_3rd_Chunli_23530"), 0);
            sp3Animation.addFrame(Content.Load<Texture2D>("sp3/SF3_3rd_Chunli_23531"), 0);
            sp3Animation.addFrame(Content.Load<Texture2D>("sp3/SF3_3rd_Chunli_23532"), 0);
            sp3Animation.addFrame(Content.Load<Texture2D>("sp3/SF3_3rd_Chunli_23533"), 0);
            sp3Animation.addFrame(Content.Load<Texture2D>("sp3/SF3_3rd_Chunli_23534"), 0);
            sp3Animation.addFrame(Content.Load<Texture2D>("sp3/SF3_3rd_Chunli_23535"), 0);
            sp3Animation.addFrame(Content.Load<Texture2D>("sp3/SF3_3rd_Chunli_23543"), 0);
            sp3Animation.addFrame(Content.Load<Texture2D>("sp3/SF3_3rd_Chunli_23544"), 0);
            sp3Animation.addFrame(Content.Load<Texture2D>("sp3/SF3_3rd_Chunli_23545"), 0);
            sp3Animation.addFrame(Content.Load<Texture2D>("sp3/SF3_3rd_Chunli_23546"), 0);
            sp3Animation.addFrame(Content.Load<Texture2D>("sp3/SF3_3rd_Chunli_23547"), 0);
            sp3Animation.addFrame(Content.Load<Texture2D>("sp3/SF3_3rd_Chunli_23548"), 0);
            sp3Animation.addFrame(Content.Load<Texture2D>("sp3/SF3_3rd_Chunli_23549"), 2);
            sp3Animation.addFrame(Content.Load<Texture2D>("sp3/SF3_3rd_Chunli_23550"), 2);
            sp3Animation.addFrame(Content.Load<Texture2D>("sp3/SF3_3rd_Chunli_23551"), 5);
            sp3Animation.addFrame(Content.Load<Texture2D>("sp3/SF3_3rd_Chunli_23562"), 2);
            sp3Animation.addFrame(Content.Load<Texture2D>("sp3/SF3_3rd_Chunli_23563"), 2);

            Animation throwTryAnimation = new Animation(animationType.oneShot);
            throwTryAnimation.addFrame(Content.Load<Texture2D>("throw/SF3_3rd_Chunli_24368"), 1);
            throwTryAnimation.addFrame(Content.Load<Texture2D>("throw/SF3_3rd_Chunli_24369"), 1);
            throwTryAnimation.addFrame(Content.Load<Texture2D>("throw/SF3_3rd_Chunli_24370"), 1);
            throwTryAnimation.addFrame(Content.Load<Texture2D>("throw/SF3_3rd_Chunli_24371"), 1);
            throwTryAnimation.addFrame(Content.Load<Texture2D>("throw/SF3_3rd_Chunli_24372"), 1);

            Animation throwAnimation = new Animation(animationType.oneShot);
            throwAnimation.addFrame(Content.Load<Texture2D>("throw/SF3_3rd_Chunli_24401"), 1);
            throwAnimation.addFrame(Content.Load<Texture2D>("throw/SF3_3rd_Chunli_24402"), 1);
            throwAnimation.addFrame(Content.Load<Texture2D>("throw/SF3_3rd_Chunli_24403"), 1);
            throwAnimation.addFrame(Content.Load<Texture2D>("throw/SF3_3rd_Chunli_24404"), 1);
            throwAnimation.addFrame(Content.Load<Texture2D>("throw/SF3_3rd_Chunli_24405"), 1);
            throwAnimation.addFrame(Content.Load<Texture2D>("throw/SF3_3rd_Chunli_24406"), 1);
            throwAnimation.addFrame(Content.Load<Texture2D>("throw/SF3_3rd_Chunli_24407"), 1);
            throwAnimation.addFrame(Content.Load<Texture2D>("throw/SF3_3rd_Chunli_24408"), 1);
            throwAnimation.addFrame(Content.Load<Texture2D>("throw/SF3_3rd_Chunli_24409"), 1);
            throwAnimation.addFrame(Content.Load<Texture2D>("throw/SF3_3rd_Chunli_24410"), 1);
            throwAnimation.addFrame(Content.Load<Texture2D>("throw/SF3_3rd_Chunli_24411"), 1);
            throwAnimation.addFrame(Content.Load<Texture2D>("throw/SF3_3rd_Chunli_24412"), 1);
            throwAnimation.addFrame(Content.Load<Texture2D>("throw/SF3_3rd_Chunli_24413"), 1);
            throwAnimation.addFrame(Content.Load<Texture2D>("throw/SF3_3rd_Chunli_24414"), 1);
            throwAnimation.addFrame(Content.Load<Texture2D>("throw/SF3_3rd_Chunli_24415"), 1);
            throwAnimation.addFrame(Content.Load<Texture2D>("throw/SF3_3rd_Chunli_24416"), 1);
            throwAnimation.addFrame(Content.Load<Texture2D>("throw/SF3_3rd_Chunli_24417"), 1);
            throwAnimation.addFrame(Content.Load<Texture2D>("throw/SF3_3rd_Chunli_24418"), 1);
            throwAnimation.addFrame(Content.Load<Texture2D>("throw/SF3_3rd_Chunli_24419"), 1);

            Animation superAnimation = new Animation(animationType.oneShot);
            superAnimation.addFrame(Content.Load<Texture2D>("super/SF3_3rd_Chunli_23431"), 2);
            superAnimation.addFrame(Content.Load<Texture2D>("super/SF3_3rd_Chunli_23432"), 1);
            superAnimation.addFrame(Content.Load<Texture2D>("super/SF3_3rd_Chunli_23334"), 1);
            superAnimation.addFrame(Content.Load<Texture2D>("super/SF3_3rd_Chunli_23335"), 1);
            superAnimation.addFrame(Content.Load<Texture2D>("super/SF3_3rd_Chunli_23336"), 1);
            superAnimation.addFrame(Content.Load<Texture2D>("super/SF3_3rd_Chunli_23337"), 1);
            superAnimation.addFrame(Content.Load<Texture2D>("super/SF3_3rd_Chunli_23338"), 1);
            superAnimation.addFrame(Content.Load<Texture2D>("super/SF3_3rd_Chunli_23339"), 1);

            superAnimation.addFrame(Content.Load<Texture2D>("super/SF3_3rd_Chunli_24392"), 0);
            superAnimation.addFrame(Content.Load<Texture2D>("super/SF3_3rd_Chunli_24393"), 0);
            superAnimation.addFrame(Content.Load<Texture2D>("super/SF3_3rd_Chunli_24395"), 0);
            superAnimation.addFrame(Content.Load<Texture2D>("super/SF3_3rd_Chunli_24396"), 0);
            superAnimation.addFrame(Content.Load<Texture2D>("super/SF3_3rd_Chunli_24464"), 0);
            superAnimation.addFrame(Content.Load<Texture2D>("super/SF3_3rd_Chunli_24465"), 0);
            superAnimation.addFrame(Content.Load<Texture2D>("super/SF3_3rd_Chunli_24466"), 0);
            superAnimation.addFrame(Content.Load<Texture2D>("super/SF3_3rd_Chunli_24469"), 0);
            superAnimation.addFrame(Content.Load<Texture2D>("super/SF3_3rd_Chunli_24470"), 0);
            superAnimation.addFrame(Content.Load<Texture2D>("super/SF3_3rd_Chunli_24471"), 0);
            superAnimation.addFrame(Content.Load<Texture2D>("super/SF3_3rd_Chunli_24472"), 0);
            superAnimation.addFrame(Content.Load<Texture2D>("super/SF3_3rd_Chunli_24473"), 0);
            superAnimation.addFrame(Content.Load<Texture2D>("super/SF3_3rd_Chunli_24474"), 0);
            superAnimation.addFrame(Content.Load<Texture2D>("super/SF3_3rd_Chunli_24477"), 0);
            superAnimation.addFrame(Content.Load<Texture2D>("super/SF3_3rd_Chunli_24478"), 0);
            superAnimation.addFrame(Content.Load<Texture2D>("super/SF3_3rd_Chunli_24479"), 0);
        
            superAnimation.addFrame(Content.Load<Texture2D>("super/SF3_3rd_Chunli_23427"), 1);
            superAnimation.addFrame(Content.Load<Texture2D>("super/SF3_3rd_Chunli_23428"), 1);
            superAnimation.addFrame(Content.Load<Texture2D>("super/SF3_3rd_Chunli_23429"), 1);
            superAnimation.addFrame(Content.Load<Texture2D>("super/SF3_3rd_Chunli_23430"), 1);

            superAnimation.addFrame(Content.Load<Texture2D>("super/SF3_3rd_Chunli_23340"), 0);
            superAnimation.addFrame(Content.Load<Texture2D>("super/SF3_3rd_Chunli_23341"), 0);
            superAnimation.addFrame(Content.Load<Texture2D>("super/SF3_3rd_Chunli_23342"), 0);
            superAnimation.addFrame(Content.Load<Texture2D>("super/SF3_3rd_Chunli_23343"), 0);
            superAnimation.addFrame(Content.Load<Texture2D>("super/SF3_3rd_Chunli_23354"), 0);
            superAnimation.addFrame(Content.Load<Texture2D>("super/SF3_3rd_Chunli_23355"), 0);
            superAnimation.addFrame(Content.Load<Texture2D>("super/SF3_3rd_Chunli_23356"), 0);
            superAnimation.addFrame(Content.Load<Texture2D>("super/SF3_3rd_Chunli_23357"), 0);
            superAnimation.addFrame(Content.Load<Texture2D>("super/SF3_3rd_Chunli_23358"), 0);
            superAnimation.addFrame(Content.Load<Texture2D>("super/SF3_3rd_Chunli_23359"), 0);
            superAnimation.addFrame(Content.Load<Texture2D>("super/SF3_3rd_Chunli_23403"), 0);
            superAnimation.addFrame(Content.Load<Texture2D>("super/SF3_3rd_Chunli_23404"), 0);
            superAnimation.addFrame(Content.Load<Texture2D>("super/SF3_3rd_Chunli_23405"), 0);
            superAnimation.addFrame(Content.Load<Texture2D>("super/SF3_3rd_Chunli_23406"), 0);
            superAnimation.addFrame(Content.Load<Texture2D>("super/SF3_3rd_Chunli_23407"), 0);
            superAnimation.addFrame(Content.Load<Texture2D>("super/SF3_3rd_Chunli_23413"), 1);
            superAnimation.addFrame(Content.Load<Texture2D>("super/SF3_3rd_Chunli_23414"), 1);
            superAnimation.addFrame(Content.Load<Texture2D>("super/SF3_3rd_Chunli_23415"), 1);
            superAnimation.addFrame(Content.Load<Texture2D>("super/SF3_3rd_Chunli_23416"), 1);
            superAnimation.addFrame(Content.Load<Texture2D>("super/SF3_3rd_Chunli_23417"), 1);
            superAnimation.addFrame(Content.Load<Texture2D>("super/SF3_3rd_Chunli_23418"), 1);
            superAnimation.addFrame(Content.Load<Texture2D>("super/SF3_3rd_Chunli_23419"), 1);
            superAnimation.addFrame(Content.Load<Texture2D>("super/SF3_3rd_Chunli_23420"), 1);
            superAnimation.addFrame(Content.Load<Texture2D>("super/SF3_3rd_Chunli_23421"), 1);
            superAnimation.addFrame(Content.Load<Texture2D>("super/SF3_3rd_Chunli_23422"), 5);
            superAnimation.addFrame(Content.Load<Texture2D>("super/SF3_3rd_Chunli_23423"), 1);
            superAnimation.addFrame(Content.Load<Texture2D>("super/SF3_3rd_Chunli_23427"), 1);



            Animation jumpRisingAnimation = new Animation(animationType.oneShot);
            jumpRisingAnimation.addFrame(Content.Load<Texture2D>("neutralJump/SF3_3rd_Chunli_23200"), 1);
            jumpRisingAnimation.addFrame(Content.Load<Texture2D>("neutralJump/SF3_3rd_Chunli_23201"), 1);
            jumpRisingAnimation.addFrame(Content.Load<Texture2D>("neutralJump/SF3_3rd_Chunli_23202"), 1);
            jumpRisingAnimation.addFrame(Content.Load<Texture2D>("neutralJump/SF3_3rd_Chunli_23203"), 0);
            jumpRisingAnimation.addFrame(Content.Load<Texture2D>("neutralJump/SF3_3rd_Chunli_23204"), 0);
            jumpRisingAnimation.addFrame(Content.Load<Texture2D>("neutralJump/SF3_3rd_Chunli_23205"), 0);
            jumpRisingAnimation.addFrame(Content.Load<Texture2D>("neutralJump/SF3_3rd_Chunli_23206"), 0);
            jumpRisingAnimation.addFrame(Content.Load<Texture2D>("neutralJump/SF3_3rd_Chunli_23207"), 1);
            jumpRisingAnimation.addFrame(Content.Load<Texture2D>("neutralJump/SF3_3rd_Chunli_23208"), 1);
            jumpRisingAnimation.addFrame(Content.Load<Texture2D>("neutralJump/SF3_3rd_Chunli_23209"), 1);
            jumpRisingAnimation.addFrame(Content.Load<Texture2D>("neutralJump/SF3_3rd_Chunli_23210"), 1);
            jumpRisingAnimation.addFrame(Content.Load<Texture2D>("neutralJump/SF3_3rd_Chunli_23211"), 1);
            jumpRisingAnimation.addFrame(Content.Load<Texture2D>("neutralJump/SF3_3rd_Chunli_23212"), 1);
            jumpRisingAnimation.addFrame(Content.Load<Texture2D>("neutralJump/SF3_3rd_Chunli_23213"), 1);
            jumpRisingAnimation.addFrame(Content.Load<Texture2D>("neutralJump/SF3_3rd_Chunli_23214"), 1);

            //Animation jumpDescendingAnimation = new Animation(animationType.oneShot);
            jumpRisingAnimation.addFrame(Content.Load<Texture2D>("neutralJump/SF3_3rd_Chunli_23215"), 1);
            jumpRisingAnimation.addFrame(Content.Load<Texture2D>("neutralJump/SF3_3rd_Chunli_23216"), 1);
            jumpRisingAnimation.addFrame(Content.Load<Texture2D>("neutralJump/SF3_3rd_Chunli_23217"), 1);
            jumpRisingAnimation.addFrame(Content.Load<Texture2D>("neutralJump/SF3_3rd_Chunli_23218"), 1);
            jumpRisingAnimation.addFrame(Content.Load<Texture2D>("neutralJump/SF3_3rd_Chunli_23219"), 1);

            //Animation jumpLandingAnimation = new Animation(animationType.oneShot);
            jumpRisingAnimation.addFrame(Content.Load<Texture2D>("neutralJump/SF3_3rd_Chunli_23220"), 1);
            jumpRisingAnimation.addFrame(Content.Load<Texture2D>("neutralJump/SF3_3rd_Chunli_23221"), 1);
            jumpRisingAnimation.addFrame(Content.Load<Texture2D>("neutralJump/SF3_3rd_Chunli_23222"), 1);
            jumpRisingAnimation.addFrame(Content.Load<Texture2D>("neutralJump/SF3_3rd_Chunli_23223"), 0);
            jumpRisingAnimation.addFrame(Content.Load<Texture2D>("neutralJump/SF3_3rd_Chunli_23224"), 0);
            jumpRisingAnimation.addFrame(Content.Load<Texture2D>("neutralJump/SF3_3rd_Chunli_23225"), 0);


            Animation hitReel = new Animation(animationType.oneShot);
            hitReel.addFrame(Content.Load<Texture2D>("hit/SF3_3rd_Chunli_23440"), 1);
            hitReel.addFrame(Content.Load<Texture2D>("hit/SF3_3rd_Chunli_23441"), 1);
            hitReel.addFrame(Content.Load<Texture2D>("hit/SF3_3rd_Chunli_23442"), 1);
            hitReel.addFrame(Content.Load<Texture2D>("hit/SF3_3rd_Chunli_23443"), 1);
            hitReel.addFrame(Content.Load<Texture2D>("hit/SF3_3rd_Chunli_23444"), 1);


            Animation blocking = new Animation(animationType.oneShot);
            blocking.addFrame(Content.Load<Texture2D>("blocking/SF3_3rd_Chunli_23264"), 1);
            blocking.addFrame(Content.Load<Texture2D>("blocking/SF3_3rd_Chunli_23265"), 1);
            blocking.addFrame(Content.Load<Texture2D>("blocking/SF3_3rd_Chunli_23266"), 1);
            blocking.addFrame(Content.Load<Texture2D>("blocking/SF3_3rd_Chunli_23267"), 1);
            blocking.addFrame(Content.Load<Texture2D>("blocking/SF3_3rd_Chunli_23268"), 1);
            blocking.addFrame(Content.Load<Texture2D>("blocking/SF3_3rd_Chunli_23269"), 1);

            Animation airHit = new Animation(animationType.oneShot);
            airHit.addFrame(Content.Load<Texture2D>("airhit/SF3_3rd_Chunli_23584"), 1);
            airHit.addFrame(Content.Load<Texture2D>("airhit/SF3_3rd_Chunli_23585"), 1);
            airHit.addFrame(Content.Load<Texture2D>("airhit/SF3_3rd_Chunli_23586"), 1);
            airHit.addFrame(Content.Load<Texture2D>("airhit/SF3_3rd_Chunli_23587"), 1);
            airHit.addFrame(Content.Load<Texture2D>("airhit/SF3_3rd_Chunli_23588"), 1);
            airHit.addFrame(Content.Load<Texture2D>("airhit/SF3_3rd_Chunli_23589"), 1);
            airHit.addFrame(Content.Load<Texture2D>("airhit/SF3_3rd_Chunli_23590"), 1);
            airHit.addFrame(Content.Load<Texture2D>("airhit/SF3_3rd_Chunli_23591"), 1);
            airHit.addFrame(Content.Load<Texture2D>("airhit/SF3_3rd_Chunli_23592"), 1);


            Animation deathKnockdown = new Animation(animationType.oneShot);
            deathKnockdown.addFrame(Content.Load<Texture2D>("knockdown/SF3_3rd_Chunli_23593"), 1);
            deathKnockdown.addFrame(Content.Load<Texture2D>("knockdown/SF3_3rd_Chunli_23594"), 1);
            deathKnockdown.addFrame(Content.Load<Texture2D>("knockdown/SF3_3rd_Chunli_23595"), 1);
            deathKnockdown.addFrame(Content.Load<Texture2D>("knockdown/SF3_3rd_Chunli_23596"), 1);
            deathKnockdown.addFrame(Content.Load<Texture2D>("knockdown/SF3_3rd_Chunli_23597"), 1);
            deathKnockdown.addFrame(Content.Load<Texture2D>("knockdown/SF3_3rd_Chunli_23598"), 1);
            deathKnockdown.addFrame(Content.Load<Texture2D>("knockdown/SF3_3rd_Chunli_23599"), 1);
            deathKnockdown.addFrame(Content.Load<Texture2D>("knockdown/SF3_3rd_Chunli_23600"), 1);
            deathKnockdown.addFrame(Content.Load<Texture2D>("knockdown/SF3_3rd_Chunli_23601"), 1);
            deathKnockdown.addFrame(Content.Load<Texture2D>("knockdown/SF3_3rd_Chunli_23602"), 1);
            deathKnockdown.addFrame(Content.Load<Texture2D>("knockdown/SF3_3rd_Chunli_23603"), 1);
            deathKnockdown.addFrame(Content.Load<Texture2D>("knockdown/SF3_3rd_Chunli_23604"), 1);
            deathKnockdown.addFrame(Content.Load<Texture2D>("knockdown/SF3_3rd_Chunli_23605"), 1);
            deathKnockdown.addFrame(Content.Load<Texture2D>("knockdown/SF3_3rd_Chunli_23606"), 1);
            deathKnockdown.addFrame(Content.Load<Texture2D>("knockdown/SF3_3rd_Chunli_23607"), 1);
            deathKnockdown.addFrame(Content.Load<Texture2D>("knockdown/SF3_3rd_Chunli_23608"), 1);
            deathKnockdown.addFrame(Content.Load<Texture2D>("knockdown/SF3_3rd_Chunli_23609"), 1);

            Animation knockdown = new Animation(animationType.oneShot);
            knockdown.addFrame(Content.Load<Texture2D>("knockdown/SF3_3rd_Chunli_23593"), 1);
            knockdown.addFrame(Content.Load<Texture2D>("knockdown/SF3_3rd_Chunli_23594"), 1);
            knockdown.addFrame(Content.Load<Texture2D>("knockdown/SF3_3rd_Chunli_23595"), 1);
            knockdown.addFrame(Content.Load<Texture2D>("knockdown/SF3_3rd_Chunli_23596"), 1);
            knockdown.addFrame(Content.Load<Texture2D>("knockdown/SF3_3rd_Chunli_23597"), 1);
            knockdown.addFrame(Content.Load<Texture2D>("knockdown/SF3_3rd_Chunli_23598"), 1);
            knockdown.addFrame(Content.Load<Texture2D>("knockdown/SF3_3rd_Chunli_23599"), 1);
            knockdown.addFrame(Content.Load<Texture2D>("knockdown/SF3_3rd_Chunli_23600"), 1);
            knockdown.addFrame(Content.Load<Texture2D>("knockdown/SF3_3rd_Chunli_23601"), 1);
            knockdown.addFrame(Content.Load<Texture2D>("knockdown/SF3_3rd_Chunli_23602"), 1);
            knockdown.addFrame(Content.Load<Texture2D>("knockdown/SF3_3rd_Chunli_23603"), 1);
            knockdown.addFrame(Content.Load<Texture2D>("knockdown/SF3_3rd_Chunli_23604"), 1);
            knockdown.addFrame(Content.Load<Texture2D>("knockdown/SF3_3rd_Chunli_23605"), 1);
            knockdown.addFrame(Content.Load<Texture2D>("knockdown/SF3_3rd_Chunli_23606"), 1);
            knockdown.addFrame(Content.Load<Texture2D>("knockdown/SF3_3rd_Chunli_23607"), 1);
            knockdown.addFrame(Content.Load<Texture2D>("knockdown/SF3_3rd_Chunli_23608"), 1);
            knockdown.addFrame(Content.Load<Texture2D>("knockdown/SF3_3rd_Chunli_23609"), 1);
            // getup portion
            knockdown.addFrame(Content.Load<Texture2D>("Getup/SF3_3rd_Chunli_23808"), 1);
            knockdown.addFrame(Content.Load<Texture2D>("Getup/SF3_3rd_Chunli_23809"), 1);
            knockdown.addFrame(Content.Load<Texture2D>("Getup/SF3_3rd_Chunli_23810"), 1);
            knockdown.addFrame(Content.Load<Texture2D>("Getup/SF3_3rd_Chunli_23811"), 1);
            knockdown.addFrame(Content.Load<Texture2D>("Getup/SF3_3rd_Chunli_23812"), 1);
            knockdown.addFrame(Content.Load<Texture2D>("Getup/SF3_3rd_Chunli_23813"), 1);
            knockdown.addFrame(Content.Load<Texture2D>("Getup/SF3_3rd_Chunli_23814"), 1);
            knockdown.addFrame(Content.Load<Texture2D>("Getup/SF3_3rd_Chunli_23815"), 1);
            knockdown.addFrame(Content.Load<Texture2D>("Getup/SF3_3rd_Chunli_23816"), 1);
            knockdown.addFrame(Content.Load<Texture2D>("Getup/SF3_3rd_Chunli_23817"), 1);
            knockdown.addFrame(Content.Load<Texture2D>("Getup/SF3_3rd_Chunli_23818"), 1);
            knockdown.addFrame(Content.Load<Texture2D>("Getup/SF3_3rd_Chunli_23819"), 1);
            knockdown.addFrame(Content.Load<Texture2D>("Getup/SF3_3rd_Chunli_23820"), 1);
            knockdown.addFrame(Content.Load<Texture2D>("Getup/SF3_3rd_Chunli_23821"), 1);
            knockdown.addFrame(Content.Load<Texture2D>("Getup/SF3_3rd_Chunli_23822"), 1);
            knockdown.addFrame(Content.Load<Texture2D>("Getup/SF3_3rd_Chunli_23823"), 1);
            knockdown.addFrame(Content.Load<Texture2D>("Getup/SF3_3rd_Chunli_23824"), 1);
            knockdown.addFrame(Content.Load<Texture2D>("Getup/SF3_3rd_Chunli_23825"), 1);
            knockdown.addFrame(Content.Load<Texture2D>("Getup/SF3_3rd_Chunli_23826"), 1);
            knockdown.addFrame(Content.Load<Texture2D>("Getup/SF3_3rd_Chunli_23827"), 1);
            knockdown.addFrame(Content.Load<Texture2D>("Getup/SF3_3rd_Chunli_23828"), 1);
            knockdown.addFrame(Content.Load<Texture2D>("Getup/SF3_3rd_Chunli_23829"), 1);
            knockdown.addFrame(Content.Load<Texture2D>("Getup/SF3_3rd_Chunli_23830"), 1);

            Animation intro = new Animation(animationType.oneShot);
            intro.addFrame(Content.Load<Texture2D>("intro/SF3_3rd_Chunli_23248"), 3);
            intro.addFrame(Content.Load<Texture2D>("intro/SF3_3rd_Chunli_23249"), 3);
            intro.addFrame(Content.Load<Texture2D>("intro/SF3_3rd_Chunli_23250"), 3);
            intro.addFrame(Content.Load<Texture2D>("intro/SF3_3rd_Chunli_23251"), 3);
            intro.addFrame(Content.Load<Texture2D>("intro/SF3_3rd_Chunli_23252"), 3);
            intro.addFrame(Content.Load<Texture2D>("intro/SF3_3rd_Chunli_23253"), 3);
            intro.addFrame(Content.Load<Texture2D>("intro/SF3_3rd_Chunli_23254"), 3);
            intro.addFrame(Content.Load<Texture2D>("intro/SF3_3rd_Chunli_23255"), 3);
            intro.addFrame(Content.Load<Texture2D>("intro/SF3_3rd_Chunli_23256"), 3);
            intro.addFrame(Content.Load<Texture2D>("intro/SF3_3rd_Chunli_23257"), 3);
            intro.addFrame(Content.Load<Texture2D>("intro/SF3_3rd_Chunli_23258"), 3);
            intro.addFrame(Content.Load<Texture2D>("intro/SF3_3rd_Chunli_23259"), 3);
            intro.addFrame(Content.Load<Texture2D>("intro/SF3_3rd_Chunli_23260"), 3);
            intro.addFrame(Content.Load<Texture2D>("intro/SF3_3rd_Chunli_23261"), 3);
            intro.addFrame(Content.Load<Texture2D>("intro/SF3_3rd_Chunli_23262"), 3);

            Animation win = new Animation(animationType.oneShot);
            win.addFrame(Content.Load<Texture2D>("Win/SF3_3rd_Chunli_24354"), 3);
            win.addFrame(Content.Load<Texture2D>("Win/SF3_3rd_Chunli_24355"), 3);
            win.addFrame(Content.Load<Texture2D>("Win/SF3_3rd_Chunli_24356"), 3);
            win.addFrame(Content.Load<Texture2D>("Win/SF3_3rd_Chunli_24357"), 3);
            win.addFrame(Content.Load<Texture2D>("Win/SF3_3rd_Chunli_24358"), 3);
            win.addFrame(Content.Load<Texture2D>("Win/SF3_3rd_Chunli_24354"), 3);
            win.addFrame(Content.Load<Texture2D>("Win/SF3_3rd_Chunli_24360"), 3);
            win.addFrame(Content.Load<Texture2D>("Win/SF3_3rd_Chunli_24361"), 3);
            win.addFrame(Content.Load<Texture2D>("Win/SF3_3rd_Chunli_24362"), 3);
            win.addFrame(Content.Load<Texture2D>("Win/SF3_3rd_Chunli_24363"), 3);
            win.addFrame(Content.Load<Texture2D>("Win/SF3_3rd_Chunli_24364"), 3);
            win.addFrame(Content.Load<Texture2D>("Win/SF3_3rd_Chunli_24365"), 3);
            win.addFrame(Content.Load<Texture2D>("Win/SF3_3rd_Chunli_24366"), 3);
            win.addFrame(Content.Load<Texture2D>("Win/SF3_3rd_Chunli_24367"), 3);
            win.addFrame(Content.Load<Texture2D>("Win/SF3_3rd_Chunli_24386"), 3);
            win.addFrame(Content.Load<Texture2D>("Win/SF3_3rd_Chunli_24387"), 3);
            win.addFrame(Content.Load<Texture2D>("Win/SF3_3rd_Chunli_24388"), 3);
            win.addFrame(Content.Load<Texture2D>("Win/SF3_3rd_Chunli_24389"), 3);
            win.addFrame(Content.Load<Texture2D>("Win/SF3_3rd_Chunli_24390"), 3);
            win.addFrame(Content.Load<Texture2D>("Win/SF3_3rd_Chunli_24391"), 3);

            Animation chunliThrow = new Animation(animationType.oneShot);
            chunliThrow.addFrame(Content.Load<Texture2D>("hit/SF3_3rd_Chunli_23440"), 10);
            chunliThrow.addFrame(Content.Load<Texture2D>("hit/SF3_3rd_Chunli_23441"), 10);

            animator.addAnimation(FighterAnimations.walkToward, walkingAnimation);
            animator.addAnimation(FighterAnimations.neutral, neutralAnimation);
            animator.addAnimation(FighterAnimations.walkBack, walkBackAnimation);

            animator.addAnimation(FighterAnimations.light, lightAnimation);
            animator.addAnimation(FighterAnimations.medium, mediumAnimation);
            animator.addAnimation(FighterAnimations.heavy, heavyAnimation);

            animator.addAnimation(FighterAnimations.sp1, sp1Animation);
            animator.addAnimation(FighterAnimations.sp2, sp2Animation);
            animator.addAnimation(FighterAnimations.sp3, sp3Animation);

            animator.addAnimation(FighterAnimations.throwTry, throwTryAnimation);
            animator.addAnimation(FighterAnimations.throwComplete, throwAnimation);
            animator.addAnimation(FighterAnimations.Super, superAnimation);

            animator.addAnimation(FighterAnimations.jumpLight, jumpLightAnimation);
            animator.addAnimation(FighterAnimations.jumpMedium, jumpMediumAnimation);
            animator.addAnimation(FighterAnimations.jumpHeavy, jumpHeavyAnimation);

            animator.addAnimation(FighterAnimations.jumpRising, jumpRisingAnimation);
            animator.addAnimation(FighterAnimations.knockdown, knockdown);
            animator.addAnimation(FighterAnimations.deathKnockdown, deathKnockdown);

            animator.addAnimation(FighterAnimations.airHit, airHit);
            animator.addAnimation(FighterAnimations.hit, hitReel);
            animator.addAnimation(FighterAnimations.blocking, blocking);

            animator.addAnimation(FighterAnimations.intro, intro);
            animator.addAnimation(FighterAnimations.win, win);

            animator.addAnimation(FighterAnimations.chunliThrow, chunliThrow);

            





            //animator.addAnimation(FigherAnimations.jumpDecending, jumpDescendingAnimation);
            //animator.addAnimation(FigherAnimations.jumpLanding, jumpLandingAnimation);


            // attacks
            light.Load(Content);
            medium.Load(Content);
            heavy.Load(Content);

            sp1.Load(Content);
            sp2.Load(Content);
            sp3.Load(Content);

            super.Load(Content);
            throwAttack.Load(Content);
            throwTry.Load(Content);
            chunLiThrown.Load(Content);

            jumpLight.Load(Content);
            jumpMedium.Load(Content);
            jumpMediumFollowup.Load(Content);
            jumpHeavy.Load(Content);

            // inputs
            input.xPress = LightAttack;
            input.yPress = MediumAttack;
            input.rbPress = HeavyAttack;
            input.aPress = sp1Attack;
            input.bPress = sp2Attack;
            input.rtPress = sp3Attack;

            input.XAPress = ThrowAttackFunc;
            input.YBPress = superAttack;

            playerMovement.SetJumps(jumpRightFunc, jumpLeftFunc, jumpNeutralFunc);

            if (playerNumber == PlayerIndex.Two)
            {
                //transform.position = new Vector3(800, 650, 0);
                transform.position = new Vector3(0, 0, 0);

            }
            else {
                transform.position = new Vector3(0, 0, 0);
            }

            // projectile setup
            Entity projectile = new Entity();
            entity.scene.addEntity(projectile);
            fireball = new Projectile(new Hitbox(85, 1, 16, 10, new Vector2(-5, 0), new Rectangle(0, 0, 30, 30), new Vector2(0, 0), CancelState.special, HitSpark.special, ignorePushback: true));
            

            projectile.addComponent(fireball);
            fireball.Load(Content);
            fireball.movementVector = new Vector2(5,0);
            fireball.SpawnPoint = new Vector2(60, 60);
            fireball.optionalFunction = FireballHit;

            Animation active = new Animation(animationType.looping);
            active.addFrame(Content.Load<Texture2D>("kikoken/SF3_3S_Chunli_39400"), 1);
            active.addFrame(Content.Load<Texture2D>("kikoken/SF3_3S_Chunli_39401"), 1);
            active.addFrame(Content.Load<Texture2D>("kikoken/SF3_3S_Chunli_39402"), 1);
            active.addFrame(Content.Load<Texture2D>("kikoken/SF3_3S_Chunli_39403"), 1);
            active.addFrame(Content.Load<Texture2D>("kikoken/SF3_3S_Chunli_39404"), 1);
            active.addFrame(Content.Load<Texture2D>("kikoken/SF3_3S_Chunli_39405"), 1);
            active.addFrame(Content.Load<Texture2D>("kikoken/SF3_3S_Chunli_39406"), 1);
            active.addFrame(Content.Load<Texture2D>("kikoken/SF3_3S_Chunli_39407"), 1);
            active.addFrame(Content.Load<Texture2D>("kikoken/SF3_3S_Chunli_39408"), 1);
            active.addFrame(Content.Load<Texture2D>("kikoken/SF3_3S_Chunli_39409"), 1);
            active.addFrame(Content.Load<Texture2D>("kikoken/SF3_3S_Chunli_39410"), 1);
            active.addFrame(Content.Load<Texture2D>("kikoken/SF3_3S_Chunli_39411"), 1);
            active.addFrame(Content.Load<Texture2D>("kikoken/SF3_3S_Chunli_39412"), 1);
            active.addFrame(Content.Load<Texture2D>("kikoken/SF3_3S_Chunli_39413"), 1);
            active.addFrame(Content.Load<Texture2D>("kikoken/SF3_3S_Chunli_39414"), 1);
            active.addFrame(Content.Load<Texture2D>("kikoken/SF3_3S_Chunli_39415"), 1);
            active.addFrame(Content.Load<Texture2D>("kikoken/SF3_3S_Chunli_39416"), 1);
            active.addFrame(Content.Load<Texture2D>("kikoken/SF3_3S_Chunli_39417"), 1);
            fireball.SetActiveAnimation(active);

            Animation dissipate = new Animation(animationType.oneShot);
            dissipate.addFrame(Content.Load<Texture2D>("kikoken/SF3_3S_Chunli_39418"), 1);
            dissipate.addFrame(Content.Load<Texture2D>("kikoken/SF3_3S_Chunli_39419"), 1);
            dissipate.addFrame(Content.Load<Texture2D>("kikoken/SF3_3S_Chunli_39420"), 1);
            dissipate.addFrame(Content.Load<Texture2D>("kikoken/SF3_3S_Chunli_39421"), 1);
            dissipate.addFrame(Content.Load<Texture2D>("kikoken/SF3_3S_Chunli_39422"), 1);
            dissipate.addFrame(Content.Load<Texture2D>("kikoken/SF3_3S_Chunli_39423"), 1);
            dissipate.addFrame(Content.Load<Texture2D>("kikoken/SF3_3S_Chunli_39424"), 1);
            dissipate.addFrame(Content.Load<Texture2D>("kikoken/SF3_3S_Chunli_39425"), 1);
            dissipate.addFrame(Content.Load<Texture2D>("kikoken/SF3_3S_Chunli_39426"), 1);
            fireball.SetDissipateAnimation(dissipate);

            //playerMovement.StartAttack(introAttack, FigherAnimations.intro);
        
        }


        public void setOtherPlayer(ref Entity other) {
            playerMovement.SetOtherPlayer(other.getComponent<PlayerMovement>());
            light.SetOtherPlayer(ref other);
            medium.SetOtherPlayer(ref other);
            heavy.SetOtherPlayer(ref other);

            jumpLight.SetOtherPlayer(ref other);
            jumpMedium.SetOtherPlayer(ref other);
            jumpHeavy.SetOtherPlayer(ref other);

            sp1.SetOtherPlayer(ref other);
            sp2.SetOtherPlayer(ref other);
            sp3.SetOtherPlayer(ref other);

            super.SetOtherPlayer(ref other);
            throwAttack.SetOtherPlayer(ref other);
            throwTry.SetOtherPlayer(ref other);

            fireball.setOtherPlayer(ref other);

        }
        

        


        public bool LightAttack()
        {
            if (playerMovement.GetState() == FighterState.neutral)
            {
                playerMovement.StartAttack(light, FighterAnimations.light);
                return true;
            } else if (playerMovement.GetState() == FighterState.jumping){
                playerMovement.StartAttack(jumpLight, FighterAnimations.jumpLight, false);
                return true;
            }
            return false;
        }

        public bool MediumAttack()
        {
            if (playerMovement.GetState() == FighterState.neutral || playerMovement.cancelState == CancelState.light)
            {
                playerMovement.StartAttack(medium, FighterAnimations.medium);
                return true;
            }
            else if (playerMovement.GetState() == FighterState.jumping || (playerMovement.GetState() == FighterState.jumpingAttack && jumpMovesRemaining))
            {
                jumpMovesRemaining = false;
                playerMovement.StartAttack(jumpMedium, FighterAnimations.jumpMedium, false);
                return true;
            }
            return false;
        }

        public bool HeavyAttack()
        {
            if (playerMovement.GetState() == FighterState.neutral || playerMovement.cancelState == CancelState.medium)
            {
                playerMovement.StartAttack(heavy, FighterAnimations.heavy);
                return true;
            }
            else if (playerMovement.GetState() == FighterState.jumping)
            {
                playerMovement.StartAttack(jumpHeavy, FighterAnimations.jumpHeavy, false);
                return true;
            }
            return false;
        }
        public bool sp1Attack()
        {
            if (!fireball.isActive && (playerMovement.GetState() == FighterState.neutral || playerMovement.cancelState == CancelState.light || playerMovement.cancelState == CancelState.medium || playerMovement.cancelState == CancelState.heavy))
            {
                superMeter.AddMeter(50);
                playerMovement.StartAttack(sp1, FighterAnimations.sp1);
                return true;
            }
            return false;
        }
        public void ActivateFireabll() {
            
            fireball.Activate(transform.position, playerMovement.isFacingLeft);
            MasterSound.fireball.Play();
        }
        public void FireballHit() {
            if (playerMovement.GetState() == FighterState.attackStartup) {
                playerMovement.cancelState = CancelState.special;
            }
        }
        public bool sp2Attack()
        {
            if (playerMovement.GetState() == FighterState.neutral || playerMovement.cancelState == CancelState.light || playerMovement.cancelState == CancelState.medium || playerMovement.cancelState == CancelState.heavy)
            {
                superMeter.AddMeter(75);
                playerMovement.StartAttack(sp2, FighterAnimations.sp2);
                return true;
            }
            return false;
        }
        public bool sp3Attack()
        {
            if (playerMovement.GetState() == FighterState.neutral || playerMovement.cancelState == CancelState.light || playerMovement.cancelState == CancelState.medium || playerMovement.cancelState == CancelState.heavy)
            {
                superMeter.AddMeter(75);
                playerMovement.StartAttack(sp3, FighterAnimations.sp3);
                return true;
            }
            return false;
        }
        public bool superAttack()
        {
            if ((playerMovement.GetState() == FighterState.neutral || playerMovement.cancelState == CancelState.light || playerMovement.cancelState == CancelState.medium || playerMovement.cancelState == CancelState.heavy || playerMovement.cancelState == CancelState.special) && superMeter.GetMeter() >= 1000)
            {
                superMeter.EmptyMeter();
                playerMovement.transform.position.Y = playerMovement.groundBound;
                playerMovement.StartAttack(super, FighterAnimations.Super);
                return true;
            }
            return false;
        }
        public void superHitStopAndEffect() {
            MasterObjectContainer.superEffect.transform.position = transform.position;
            MasterObjectContainer.superEffect.PlayAnimation(superFlash.super, true);
            MasterObjectContainer.hitstopRemaining = 60;
            MasterSound.super.Play();
        }
        public bool ThrowAttackFunc()
        {
            if (playerMovement.GetState() == FighterState.neutral )
            {
                playerMovement.StartAttack(throwTry, FighterAnimations.throwTry);
                return true;
            }
            return false;
        }
        public void jumpNeutralFunc() {
            if (playerMovement.GetState() == FighterState.neutral)
            {
                playerMovement.StartJump(jumpNeutral, FighterAnimations.jumpRising);
                jumpMovesRemaining = false;
            }
        }
        public void jumpRightFunc()
        {
            if (playerMovement.GetState() == FighterState.neutral)
            {
                if (playerMovement.isFacingLeft)
                {
                    playerMovement.StartJump(jumpBack, FighterAnimations.jumpRising);
                }
                else {
                    playerMovement.StartJump(jumpForward, FighterAnimations.jumpRising);
                }
                jumpMovesRemaining = false;
            }
        }
        public void jumpLeftFunc()
        {
            if (playerMovement.GetState() == FighterState.neutral)
            {
                if (playerMovement.isFacingLeft)
                {
                    playerMovement.StartJump(jumpForward, FighterAnimations.jumpRising);
                }
                else {
                    playerMovement.StartJump(jumpBack, FighterAnimations.jumpRising);
                }
                jumpMovesRemaining = false;
            }
        }
        


    }
}
