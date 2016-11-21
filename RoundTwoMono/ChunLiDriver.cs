using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework;

namespace RoundTwoMono
{
    class ChunLiDriver: Component, Renderable, Updateable
    {

        Attack light, medium, heavy, sp1, sp2, sp3, super, throwAttack;
        Attack currentAttack;
        InputManager input;
        SpriteAnimator<FigherAnimations> animator;
        PlayerMovement playerMovement;
        bool showHitboxes, isAttacking;

       public  ChunLiDriver()
        {
            showHitboxes = true;
            isAttacking = false;

            


            
        }
        
        public void load(ContentManager Content) {

            animator = new SpriteAnimator<FigherAnimations>();
            input = new InputManager(0);
            playerMovement = new PlayerMovement(input, animator, 5f);
            entity.addComponent(input);
            entity.addComponent(animator);
            entity.addComponent(playerMovement);

            light = new Attack(input, transform, 10);
            ActionFrame actionFrame =  new ActionFrame(3);
            actionFrame.setAttack(new Hitbox(15, 10, 8, 2, new Rectangle(0, 0, 80, 20), new Vector2(0, -10)));
            light.AddActionFrame(actionFrame);

            medium = new Attack(input, transform, 34);
            actionFrame = new ActionFrame(6);
            actionFrame.setAttack(new Hitbox(50, 13, 12, 2, new Rectangle(0, 0, 120, 20), new Vector2(0, 50)));
            medium.AddActionFrame(actionFrame);

            heavy = new Attack(input, transform, 52);
            actionFrame = new ActionFrame(20);
            actionFrame.setAttack(new Hitbox(85, 16, 10, 5, new Rectangle(0, 0, 100, 20), new Vector2(0, 0)));
            heavy.AddActionFrame(actionFrame);

            sp1 = new Attack(input, transform, 54);
            actionFrame = new ActionFrame(14);
            actionFrame.setAttack(new Hitbox(85, 16, 10, 5, new Rectangle(0, 0, 100, 20), new Vector2(0, 0)));
            sp1.AddActionFrame(actionFrame);

            sp2 = new Attack(input, transform, 110);
            actionFrame = new ActionFrame(18);
            actionFrame.setAttack(new Hitbox(85, 16, 10, 5, new Rectangle(0, 0, 100, 20), new Vector2(0, 0)));
            sp2.AddActionFrame(actionFrame);

            sp3 = new Attack(input, transform, 74);
            actionFrame = new ActionFrame(22);
            actionFrame.setAttack(new Hitbox(85, 16, 10, 5, new Rectangle(0, 0, 100, 20), new Vector2(0, 0)));
            sp3.AddActionFrame(actionFrame);

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
            neutralAnimation.addFrame(Content.Load<Texture2D>("neutral/SF3_3rd_Chunli_23056"), 2);
            neutralAnimation.addFrame(Content.Load<Texture2D>("neutral/SF3_3rd_Chunli_23057"), 2);
            neutralAnimation.addFrame(Content.Load<Texture2D>("neutral/SF3_3rd_Chunli_23058"), 2);
            neutralAnimation.addFrame(Content.Load<Texture2D>("neutral/SF3_3rd_Chunli_23059"), 2);
            neutralAnimation.addFrame(Content.Load<Texture2D>("neutral/SF3_3rd_Chunli_23060"), 2);
            neutralAnimation.addFrame(Content.Load<Texture2D>("neutral/SF3_3rd_Chunli_23061"), 2);

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
            mediumAnimation.addFrame(Content.Load<Texture2D>("attackMedium/SF3_3rd_Chunli_24176"), 2);
            mediumAnimation.addFrame(Content.Load<Texture2D>("attackMedium/SF3_3rd_Chunli_24177"), 1);
            mediumAnimation.addFrame(Content.Load<Texture2D>("attackMedium/SF3_3rd_Chunli_24178"), 1);
            mediumAnimation.addFrame(Content.Load<Texture2D>("attackMedium/SF3_3rd_Chunli_24179"), 1);
            mediumAnimation.addFrame(Content.Load<Texture2D>("attackMedium/SF3_3rd_Chunli_24180"), 1);
            mediumAnimation.addFrame(Content.Load<Texture2D>("attackMedium/SF3_3rd_Chunli_24181"), 1);
            mediumAnimation.addFrame(Content.Load<Texture2D>("attackMedium/SF3_3rd_Chunli_24182"), 1);
            mediumAnimation.addFrame(Content.Load<Texture2D>("attackMedium/SF3_3rd_Chunli_24183"), 1);
            mediumAnimation.addFrame(Content.Load<Texture2D>("attackMedium/SF3_3rd_Chunli_24184"), 1);

            Animation heavyAnimation = new Animation(animationType.oneShot);
            heavyAnimation.addFrame(Content.Load<Texture2D>("attackHeavy/SF3_3rd_Chunli_24032"), 1);
            heavyAnimation.addFrame(Content.Load<Texture2D>("attackHeavy/SF3_3rd_Chunli_24033"), 1);
            heavyAnimation.addFrame(Content.Load<Texture2D>("attackHeavy/SF3_3rd_Chunli_24034"), 1);
            heavyAnimation.addFrame(Content.Load<Texture2D>("attackHeavy/SF3_3rd_Chunli_24035"), 1);
            heavyAnimation.addFrame(Content.Load<Texture2D>("attackHeavy/SF3_3rd_Chunli_24036"), 1);
            heavyAnimation.addFrame(Content.Load<Texture2D>("attackHeavy/SF3_3rd_Chunli_24037"), 1);
            heavyAnimation.addFrame(Content.Load<Texture2D>("attackHeavy/SF3_3rd_Chunli_24038"), 1);
            heavyAnimation.addFrame(Content.Load<Texture2D>("attackHeavy/SF3_3rd_Chunli_24039"), 1);
            heavyAnimation.addFrame(Content.Load<Texture2D>("attackHeavy/SF3_3rd_Chunli_24040"), 1);
            heavyAnimation.addFrame(Content.Load<Texture2D>("attackHeavy/SF3_3rd_Chunli_24041"), 1);
            heavyAnimation.addFrame(Content.Load<Texture2D>("attackHeavy/SF3_3rd_Chunli_24042"), 1);
            heavyAnimation.addFrame(Content.Load<Texture2D>("attackHeavy/SF3_3rd_Chunli_24043"), 1);
            heavyAnimation.addFrame(Content.Load<Texture2D>("attackHeavy/SF3_3rd_Chunli_24044"), 1);
            heavyAnimation.addFrame(Content.Load<Texture2D>("attackHeavy/SF3_3rd_Chunli_24045"), 1);

            Animation sp1Animation = new Animation(animationType.oneShot);
            sp1Animation.addFrame(Content.Load<Texture2D>("sp1/SF3_3rd_Chunli_24528"), 1);
            sp1Animation.addFrame(Content.Load<Texture2D>("sp1/SF3_3rd_Chunli_24529"), 1);
            sp1Animation.addFrame(Content.Load<Texture2D>("sp1/SF3_3rd_Chunli_24530"), 1);
            sp1Animation.addFrame(Content.Load<Texture2D>("sp1/SF3_3rd_Chunli_24531"), 1);
            sp1Animation.addFrame(Content.Load<Texture2D>("sp1/SF3_3rd_Chunli_24532"), 1);
            sp1Animation.addFrame(Content.Load<Texture2D>("sp1/SF3_3rd_Chunli_24533"), 1);
            sp1Animation.addFrame(Content.Load<Texture2D>("sp1/SF3_3rd_Chunli_24534"), 1);
            sp1Animation.addFrame(Content.Load<Texture2D>("sp1/SF3_3rd_Chunli_24535"), 1);
            sp1Animation.addFrame(Content.Load<Texture2D>("sp1/SF3_3rd_Chunli_24536"), 1);
            sp1Animation.addFrame(Content.Load<Texture2D>("sp1/SF3_3rd_Chunli_24537"), 1);
            sp1Animation.addFrame(Content.Load<Texture2D>("sp1/SF3_3rd_Chunli_24538"), 1);
            sp1Animation.addFrame(Content.Load<Texture2D>("sp1/SF3_3rd_Chunli_24539"), 1);
            sp1Animation.addFrame(Content.Load<Texture2D>("sp1/SF3_3rd_Chunli_24540"), 1);
            sp1Animation.addFrame(Content.Load<Texture2D>("sp1/SF3_3rd_Chunli_24541"), 1);
            sp1Animation.addFrame(Content.Load<Texture2D>("sp1/SF3_3rd_Chunli_24542"), 1);
            sp1Animation.addFrame(Content.Load<Texture2D>("sp1/SF3_3rd_Chunli_24543"), 1);
            sp1Animation.addFrame(Content.Load<Texture2D>("sp1/SF3_3rd_Chunli_24544"), 1);
            sp1Animation.addFrame(Content.Load<Texture2D>("sp1/SF3_3rd_Chunli_24545"), 1);
            sp1Animation.addFrame(Content.Load<Texture2D>("sp1/SF3_3rd_Chunli_24546"), 1);
            sp1Animation.addFrame(Content.Load<Texture2D>("sp1/SF3_3rd_Chunli_24547"), 1);
            sp1Animation.addFrame(Content.Load<Texture2D>("sp1/SF3_3rd_Chunli_24548"), 1);

            Animation sp2Animation = new Animation(animationType.oneShot);
            sp2Animation.addFrame(Content.Load<Texture2D>("sp2/SF3_3rd_Chunli_24480"), 1);
            sp2Animation.addFrame(Content.Load<Texture2D>("sp2/SF3_3rd_Chunli_24481"), 1);
            sp2Animation.addFrame(Content.Load<Texture2D>("sp2/SF3_3rd_Chunli_24482"), 1);
            sp2Animation.addFrame(Content.Load<Texture2D>("sp2/SF3_3rd_Chunli_24483"), 1);
            sp2Animation.addFrame(Content.Load<Texture2D>("sp2/SF3_3rd_Chunli_24484"), 1);
            sp2Animation.addFrame(Content.Load<Texture2D>("sp2/SF3_3rd_Chunli_24485"), 1);
            sp2Animation.addFrame(Content.Load<Texture2D>("sp2/SF3_3rd_Chunli_24486"), 1);
            sp2Animation.addFrame(Content.Load<Texture2D>("sp2/SF3_3rd_Chunli_24487"), 1);
            sp2Animation.addFrame(Content.Load<Texture2D>("sp2/SF3_3rd_Chunli_24488"), 1);
            sp2Animation.addFrame(Content.Load<Texture2D>("sp2/SF3_3rd_Chunli_24489"), 1);
            sp2Animation.addFrame(Content.Load<Texture2D>("sp2/SF3_3rd_Chunli_24490"), 1);
            sp2Animation.addFrame(Content.Load<Texture2D>("sp2/SF3_3rd_Chunli_24491"), 1);
            sp2Animation.addFrame(Content.Load<Texture2D>("sp2/SF3_3rd_Chunli_24492"), 1);
            sp2Animation.addFrame(Content.Load<Texture2D>("sp2/SF3_3rd_Chunli_24493"), 1);
            sp2Animation.addFrame(Content.Load<Texture2D>("sp2/SF3_3rd_Chunli_24494"), 1);
            sp2Animation.addFrame(Content.Load<Texture2D>("sp2/SF3_3rd_Chunli_24495"), 1);
            sp2Animation.addFrame(Content.Load<Texture2D>("sp2/SF3_3rd_Chunli_24496"), 1);
            sp2Animation.addFrame(Content.Load<Texture2D>("sp2/SF3_3rd_Chunli_24497"), 1);
            sp2Animation.addFrame(Content.Load<Texture2D>("sp2/SF3_3rd_Chunli_24498"), 1);
            sp2Animation.addFrame(Content.Load<Texture2D>("sp2/SF3_3rd_Chunli_24499"), 1);
            sp2Animation.addFrame(Content.Load<Texture2D>("sp2/SF3_3rd_Chunli_24500"), 1);
            sp2Animation.addFrame(Content.Load<Texture2D>("sp2/SF3_3rd_Chunli_24501"), 1);
            sp2Animation.addFrame(Content.Load<Texture2D>("sp2/SF3_3rd_Chunli_24502"), 1);
            sp2Animation.addFrame(Content.Load<Texture2D>("sp2/SF3_3rd_Chunli_24503"), 1);
            sp2Animation.addFrame(Content.Load<Texture2D>("sp2/SF3_3rd_Chunli_24504"), 1);
            sp2Animation.addFrame(Content.Load<Texture2D>("sp2/SF3_3rd_Chunli_24505"), 1);
            sp2Animation.addFrame(Content.Load<Texture2D>("sp2/SF3_3rd_Chunli_24506"), 1);
            sp2Animation.addFrame(Content.Load<Texture2D>("sp2/SF3_3rd_Chunli_24507"), 1);
            sp2Animation.addFrame(Content.Load<Texture2D>("sp2/SF3_3rd_Chunli_24508"), 1);
            sp2Animation.addFrame(Content.Load<Texture2D>("sp2/SF3_3rd_Chunli_24509"), 1);
            sp2Animation.addFrame(Content.Load<Texture2D>("sp2/SF3_3rd_Chunli_24510"), 1);
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
            sp3Animation.addFrame(Content.Load<Texture2D>("sp3/SF3_3rd_Chunli_23549"), 4);
            sp3Animation.addFrame(Content.Load<Texture2D>("sp3/SF3_3rd_Chunli_23550"), 4);    
            sp3Animation.addFrame(Content.Load<Texture2D>("sp3/SF3_3rd_Chunli_23551"), 4);
            sp3Animation.addFrame(Content.Load<Texture2D>("sp3/SF3_3rd_Chunli_23562"), 4);
            sp3Animation.addFrame(Content.Load<Texture2D>("sp3/SF3_3rd_Chunli_23563"), 4);



            animator.addAnimation(FigherAnimations.walkToward, walkingAnimation);
            animator.addAnimation(FigherAnimations.neutral, neutralAnimation);
            animator.addAnimation(FigherAnimations.walkBack, walkBackAnimation);
            animator.addAnimation(FigherAnimations.light, lightAnimation);
            animator.addAnimation(FigherAnimations.medium, mediumAnimation);
            animator.addAnimation(FigherAnimations.heavy, heavyAnimation);
            animator.addAnimation(FigherAnimations.sp1, sp1Animation);
            animator.addAnimation(FigherAnimations.sp2, sp2Animation);
            animator.addAnimation(FigherAnimations.sp3, sp3Animation);



            light.Load(Content);
            medium.Load(Content);
            heavy.Load(Content);
            sp1.Load(Content);
            sp2.Load(Content);
            sp3.Load(Content);
            //super.Load(Content);
            //throwAttack.Load(Content);

            input.xPress = LightAttack;
            input.yPress = MediumAttack;
            input.rbPress = HeavyAttack;
            input.aPress = sp1Attack;
            input.bPress = sp2Attack;
            input.rtPress = sp3Attack;
            transform.position = new Vector3(150, 300, 0);

        }

        public void Update() {
            if (isAttacking)
            {
               bool finished = currentAttack.NextStep();
                if (finished) {
                    isAttacking = false;
                    playerMovement.SetState(FighterState.neutral);
                }
            }
        }

        public void Draw(SpriteBatch spritebatch) {
            if (showHitboxes) {
                light.Draw(spritebatch);
                medium.Draw(spritebatch);
                heavy.Draw(spritebatch);
                sp1.Draw(spritebatch);
                sp2.Draw(spritebatch);
                sp3.Draw(spritebatch);
                //super.Draw(spritebatch);
                //throwAttack.Draw(spritebatch);
            }
        }


        public void LightAttack()
        {
            if (playerMovement.GetState() == FighterState.neutral)
            {
                StartAttack(light, FigherAnimations.light);
            }
        }

        public void MediumAttack()
        {
            if (playerMovement.GetState() == FighterState.neutral)
            {
                StartAttack(medium, FigherAnimations.medium);
            }
        }

        public void HeavyAttack()
        {
            if (playerMovement.GetState() == FighterState.neutral)
            {
                StartAttack(heavy, FigherAnimations.heavy);
            }
        }
        public void sp1Attack()
        {
            if (playerMovement.GetState() == FighterState.neutral)
            {
                StartAttack(sp1, FigherAnimations.sp1);
            }
        }

        public void sp2Attack()
        {
            if (playerMovement.GetState() == FighterState.neutral)
            {
                StartAttack(sp2, FigherAnimations.sp2);
            }
        }
        public void sp3Attack()
        {
            if (playerMovement.GetState() == FighterState.neutral)
            {
                StartAttack(sp3, FigherAnimations.sp3);
            }
        }
        void StartAttack(Attack newAttack, FigherAnimations newAnimation) {
            playerMovement.SetState(FighterState.attackStartup);
            animator.PlayAnimation(newAnimation);
            currentAttack = newAttack;
            currentAttack.Start();
            isAttacking = true;
        }


    }
}
