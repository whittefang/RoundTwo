﻿using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Media;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using EngineFang;
namespace RoundTwoMono
{
    /// <summary>
    /// This is the main type for your game.
    /// </summary>
    public class Game1 : Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
        Scene mainScene, noHitstopScene;
        Entity chunli, chunli2;
        Entity stageEntity;
        Entity objectPools;
        DebugTools debugTools;
        ChunLiDriver chunDriverOne, chunDriverTwo;
        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            graphics.PreferredBackBufferWidth = 1920;
            graphics.PreferredBackBufferHeight = 1080;
            Content.RootDirectory = "Content";
        }

        
        protected override void Initialize()
        {
            // TODO: Add your initialization logic here
            Camera.Init(GraphicsDevice.Viewport, 4.5f);
            
            chunli = new Entity();
            chunli2 = new Entity();
            objectPools = new Entity();
            stageEntity = new Entity();
            mainScene = new Scene();
            noHitstopScene = new Scene();
            debugTools = new DebugTools();

            // player one
            chunDriverOne = new ChunLiDriver();
            chunli.addComponent(chunDriverOne);

            // player two
            chunDriverTwo = new ChunLiDriver();
            chunli2.addComponent(chunDriverTwo);

            mainScene.addEntity(stageEntity);
            mainScene.addEntity(chunli);
            mainScene.addEntity(chunli2);
            noHitstopScene.addEntity(objectPools);

            base.Initialize();
        }

        
        protected override void LoadContent()
        {

            chunDriverOne.InitializeComponents(PlayerIndex.One);
            chunDriverTwo.InitializeComponents(PlayerIndex.Two);

            Camera.SetPosition(new Vector2(0, 110));
            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);

           

            // set up object pools
            ObjectPool tmpHitSparks = new ObjectPool(HitSpark.block, Content);
            objectPools.addComponent(tmpHitSparks);

            tmpHitSparks = new ObjectPool(HitSpark.light, Content);
            objectPools.addComponent(tmpHitSparks);

            tmpHitSparks = new ObjectPool(HitSpark.medium, Content);
            objectPools.addComponent(tmpHitSparks);

            tmpHitSparks = new ObjectPool(HitSpark.heavy, Content);
            objectPools.addComponent(tmpHitSparks);

            tmpHitSparks = new ObjectPool(HitSpark.special, Content);
            objectPools.addComponent(tmpHitSparks);

            SpriteAnimator<superFlash> superEffect = new SpriteAnimator<superFlash>(new Vector2(50,100));
            objectPools.addComponent(superEffect);

            
            UIMatch.Load(Content);

            Stage stage = new Stage();
            Texture2D stagePic = Content.Load<Texture2D>("stages/gouki/gouki_0-0");
            StagePart akumaMoon = new StagePart(Transform.GetCustomRenderPosition(stagePic, new Vector2(0, 260), TransformOriginPoint.center), stagePic, .4f);
            stage.AddStagePart(akumaMoon);

            stagePic = Content.Load<Texture2D>("stages/gouki/gouki_1-0");
            StagePart akumaMountain = new StagePart(Transform.GetCustomRenderPosition(stagePic, new Vector2(0, 250), TransformOriginPoint.center), stagePic, .35f);
            stage.AddStagePart(akumaMountain);

            stagePic = Content.Load<Texture2D>("stages/gouki/gouki_2-0");
            StagePart akumaLeftFore = new StagePart(Transform.GetCustomRenderPosition(stagePic, new Vector2(0, 220), TransformOriginPoint.right), stagePic, 0);
            stage.AddStagePart(akumaLeftFore);

            stagePic = Content.Load<Texture2D>("stages/gouki/gouki_2-1");
            StagePart akumaRightFore = new StagePart(Transform.GetCustomRenderPosition(stagePic, new Vector2(0, 220), TransformOriginPoint.left), stagePic, 0);
            stage.AddStagePart(akumaRightFore);

           

            stageEntity.addComponent(stage);


            Animation superAnim = new Animation(animationType.oneShot);
            superAnim.renderOneshotAfterCompletion = false;
            superAnim.addFrame(this.Content.Load<Texture2D>("SuperEffect/30265"), 1);
            superAnim.addFrame(this.Content.Load<Texture2D>("SuperEffect/30266"), 1);
            superAnim.addFrame(this.Content.Load<Texture2D>("SuperEffect/30267"), 1);
            superAnim.addFrame(this.Content.Load<Texture2D>("SuperEffect/30268"), 1);
            superAnim.addFrame(this.Content.Load<Texture2D>("SuperEffect/30269"), 1);
            superAnim.addFrame(this.Content.Load<Texture2D>("SuperEffect/30270"), 1);
            superAnim.addFrame(this.Content.Load<Texture2D>("SuperEffect/30271"), 1);
            superAnim.addFrame(this.Content.Load<Texture2D>("SuperEffect/30272"), 1);
            superAnim.addFrame(this.Content.Load<Texture2D>("SuperEffect/30273"), 1);
            superAnim.addFrame(this.Content.Load<Texture2D>("SuperEffect/30274"), 1);
            superAnim.addFrame(this.Content.Load<Texture2D>("SuperEffect/30275"), 1);
            superAnim.addFrame(this.Content.Load<Texture2D>("SuperEffect/30276"), 1);
            superAnim.addFrame(this.Content.Load<Texture2D>("SuperEffect/30277"), 1);
            superAnim.addFrame(this.Content.Load<Texture2D>("SuperEffect/30278"), 1);
            superAnim.addFrame(this.Content.Load<Texture2D>("SuperEffect/30279"), 1);
            superAnim.addFrame(this.Content.Load<Texture2D>("SuperEffect/30280"), 1);
            superAnim.addFrame(this.Content.Load<Texture2D>("SuperEffect/30281"), 1);
            superAnim.addFrame(this.Content.Load<Texture2D>("SuperEffect/30282"), 1);
            superAnim.addFrame(this.Content.Load<Texture2D>("SuperEffect/30283"), 1);
            superAnim.addFrame(this.Content.Load<Texture2D>("SuperEffect/30284"), 1);
            superAnim.addFrame(this.Content.Load<Texture2D>("SuperEffect/30285"), 1);
            superAnim.addFrame(this.Content.Load<Texture2D>("SuperEffect/30286"), 1);
            superAnim.addFrame(this.Content.Load<Texture2D>("SuperEffect/30287"), 1);
            superAnim.addFrame(this.Content.Load<Texture2D>("SuperEffect/30288"), 1);
            superAnim.addFrame(this.Content.Load<Texture2D>("SuperEffect/30289"), 1);
            superAnim.addFrame(this.Content.Load<Texture2D>("SuperEffect/30290"), 1);
            superAnim.addFrame(this.Content.Load<Texture2D>("SuperEffect/30291"), 1);
            superAnim.addFrame(this.Content.Load<Texture2D>("SuperEffect/30292"), 1);
            superAnim.addFrame(this.Content.Load<Texture2D>("SuperEffect/30293"), 1);
            superAnim.addFrame(this.Content.Load<Texture2D>("SuperEffect/30294"), 1);
            superAnim.addFrame(this.Content.Load<Texture2D>("SuperEffect/30295"), 1);
            superAnim.addFrame(this.Content.Load<Texture2D>("SuperEffect/30296"), 1);
            superAnim.addFrame(this.Content.Load<Texture2D>("SuperEffect/30308"), 1);
            superAnim.addFrame(this.Content.Load<Texture2D>("SuperEffect/30309"), 1);
            superAnim.addFrame(this.Content.Load<Texture2D>("SuperEffect/30310"), 1);
            superAnim.addFrame(this.Content.Load<Texture2D>("SuperEffect/30311"), 1);
            superAnim.addFrame(this.Content.Load<Texture2D>("SuperEffect/30312"), 1);
            superAnim.addFrame(this.Content.Load<Texture2D>("SuperEffect/30313"), 1);
            superAnim.addFrame(this.Content.Load<Texture2D>("SuperEffect/30314"), 1);
            superAnim.addFrame(this.Content.Load<Texture2D>("SuperEffect/30315"), 1);
            superAnim.addFrame(this.Content.Load<Texture2D>("SuperEffect/30316"), 1);
            superAnim.addFrame(this.Content.Load<Texture2D>("SuperEffect/30317"), 1);
            superAnim.addFrame(this.Content.Load<Texture2D>("SuperEffect/30318"), 1);

            superEffect.addAnimation(superFlash.super, superAnim);

            MasterObjectContainer.superEffect = superEffect;
            MasterObjectContainer.hitSparkHolder = objectPools;

            

            

            mainScene.Load(Content);
            MasterSound.Load(Content);

            chunDriverOne.setOtherPlayer(ref chunli2);
            chunDriverTwo.setOtherPlayer(ref chunli);

            MasterObjectContainer.backgroundMusic = Content.Load<Song>("Music/ChunLiStage");

            MasterObjectContainer.playerOneMovement = chunli.getComponent<PlayerMovement>();
            MasterObjectContainer.playerTwoMovement = chunli2.getComponent<PlayerMovement>();
            MasterObjectContainer.NextRound();
            //chunAnim.PlayAnimation(FigherAnimations.neutral);
            // TODO: use this.Content to load your game content here
        }

      
        protected override void UnloadContent()
        {
            // TODO: Unload any non ContentManager content here
        }

       
        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();
            debugTools.Update();

            // TODO: Add your update logic here
            if (!MasterObjectContainer.paused || MasterObjectContainer.advanceOneFrame)
            {
                if (MasterObjectContainer.hitstopRemaining <= 0)
                {

                    mainScene.Update();
                    MasterObjectContainer.Update();
                }
                else
                {
                    MasterObjectContainer.hitstopRemaining--;
                }
            }

            noHitstopScene.Update();

            if (MasterObjectContainer.advanceOneFrame) {
                MasterObjectContainer.advanceOneFrame = false;
            }
            base.Update(gameTime);
        }

        
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.Black);

            // TODO: Add your drawing code here
            spriteBatch.Begin(samplerState: SamplerState.PointClamp,transformMatrix: Camera.GetViewMatrix());
         
            mainScene.Draw(spriteBatch);
            noHitstopScene.Draw(spriteBatch);

            spriteBatch.End();

            spriteBatch.Begin(samplerState: SamplerState.PointClamp, transformMatrix: Camera.GetUIMatrix());
            UIMatch.Draw(spriteBatch);

            spriteBatch.End();



            base.Draw(gameTime);
        }
        
    }
}
