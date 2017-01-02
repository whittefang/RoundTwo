using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using RoundTwoMono.EngineFang;
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
        Entity objectPools;
        DebugTools debugTools;
       

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
            Camera.Init(GraphicsDevice.Viewport);
            Camera.Zoom = 4;
            
            chunli = new Entity();
            chunli2 = new Entity();
            objectPools = new Entity();
            mainScene = new Scene();
            noHitstopScene = new Scene();
            debugTools = new DebugTools();
            
            base.Initialize();
        }

        
        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);

            mainScene.addEntity(chunli);
            mainScene.addEntity(chunli2);
            noHitstopScene.addEntity(objectPools);

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

            // player one
            var chun = new ChunLiDriver();
            chunli.addComponent(chun);
            chun.load(this.Content, PlayerIndex.Three);

            // player two
            var chun2 = new ChunLiDriver();
            chunli2.addComponent(chun2);
            chun2.load(this.Content, PlayerIndex.Two);

            chun.setOtherPlayer(ref chunli2);
            chun2.setOtherPlayer(ref chunli);

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
            GraphicsDevice.Clear(Color.CornflowerBlue);

            // TODO: Add your drawing code here
            spriteBatch.Begin(samplerState: SamplerState.PointClamp,transformMatrix: Camera.GetViewMatrix());
         
            mainScene.Draw(spriteBatch);
            noHitstopScene.Draw(spriteBatch);
            spriteBatch.End();


            base.Draw(gameTime);
        }
        
    }
}
