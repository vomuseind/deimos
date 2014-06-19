using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using System.Threading;
using System.Collections;

namespace Deimos
{
    /// <summary>
    /// This is the main type for your game
    /// </summary>
    public partial class DeimosGame : Microsoft.Xna.Framework.Game
    {
        GraphicsDeviceManager Graphics;

        public DeferredRenderer Renderer
        {
            get;
            private set;
        }

       

        public enum PlayingStates
        {
            Normal,
            MiniGame,
            NoClip
        }


        PlayingStates currentPlayingState = PlayingStates.Normal;
        public PlayingStates CurrentPlayingState
        {
            get { return currentPlayingState; }
            private set { currentPlayingState = value; }
        }

        public VideoPlayer VideoPlayer;
        public Video IntroVideo;


        public DeimosGame()
        {
            HelperFacade.Helpers = new Helpers();

            GeneralFacade.Game = this;

            GeneralFacade.TempContent = new ContentManager(Services);

            Graphics = new GraphicsDeviceManager(this);


            VideoPlayer = new VideoPlayer();


            Content.RootDirectory = "Content";
            GeneralFacade.TempContent.RootDirectory = "Content";


            Renderer = new DeferredRenderer(this);
            Components.Add(Renderer);

            DisplayFacade.ParticleManager = new Tranquillity.ParticleManager(this);
            DisplayFacade.ParticleManager.Visible = true;
            Components.Add(DisplayFacade.ParticleManager);



            GeneralFacade.Config = new Config();
            GameplayFacade.Constants = new Constants();
        }

        protected override void Initialize()
        {
            IsMouseVisible = false;

            // Game settings
            //Graphics.PreferredBackBufferWidth = 1680;
            //Graphics.PreferredBackBufferHeight = 1050;
            Graphics.PreferredBackBufferWidth = 1280;
            Graphics.PreferredBackBufferHeight = 850;
            //Graphics.IsFullScreen = true;
            //Graphics.PreferMultiSampling = true; // Anti aliasing - Useless as custom effects
            Graphics.SynchronizeWithVerticalRetrace = false; // VSync
            //IsFixedTimeStep = false; // Call the UPDATE method all the time instead of x time per sec
            Graphics.ApplyChanges();

            if (NetworkFacade.IsMultiplayer)
            {

                // Thread optimization
                // Sending thread
                NetworkFacade.Outgoing.Name = "out";
                NetworkFacade.Outgoing.IsBackground = true;
                NetworkFacade.Outgoing.Priority = ThreadPriority.BelowNormal;
                // Receiving thread
                NetworkFacade.Incoming.Name = "in";
                NetworkFacade.Incoming.IsBackground = true;
                // Interpreting thread
                NetworkFacade.Interpret.Name = "proc";
                NetworkFacade.Interpret.IsBackground = true;
                // World updating thread
                NetworkFacade.World.Name = "world";
                NetworkFacade.World.IsBackground = true;

                NetworkFacade.MovePacket.Name = "move";
                NetworkFacade.MovePacket.IsBackground = true;

                if (NetworkFacade.ServerIsLocal)
                {
                    NetworkFacade.NetworkHandling.SetConnectivity(
                        "127.0.0.1", 1518, "127.0.0.1", 1564);
                }
            }

            base.Initialize();
        }


        protected override void LoadContent()
        {
            IntroVideo = Content.Load<Video>("Videos/Intro");
            DisplayFacade.BackgroundMenu = Content.Load<Texture2D>("Images/Menu/backgroundMenu");

            DisplayFacade.MenuImages.Add("StartMenuPlay", Content.Load<Texture2D>("Images/Menu/StartMenu/play"));
            DisplayFacade.MenuImages.Add("StartMenuPlayHover", Content.Load<Texture2D>("Images/Menu/StartMenu/playHover"));
            DisplayFacade.MenuImages.Add("StartMenuServers", Content.Load<Texture2D>("Images/Menu/StartMenu/servers"));
            DisplayFacade.MenuImages.Add("StartMenuServersHover", Content.Load<Texture2D>("Images/Menu/StartMenu/serversHover"));
            DisplayFacade.MenuImages.Add("StartMenuQuit", Content.Load<Texture2D>("Images/Menu/StartMenu/quit"));
            DisplayFacade.MenuImages.Add("StartMenuQuitHover", Content.Load<Texture2D>("Images/Menu/StartMenu/quitHover"));

            DisplayFacade.MenuImages.Add("PauseMenuResume", Content.Load<Texture2D>("Images/Menu/PauseMenu/resume"));
            DisplayFacade.MenuImages.Add("PauseMenuResumeHover", Content.Load<Texture2D>("Images/Menu/PauseMenu/resumeHover"));
            DisplayFacade.MenuImages.Add("PauseMenuMain", Content.Load<Texture2D>("Images/Menu/PauseMenu/main"));
            DisplayFacade.MenuImages.Add("PauseMenuMainHover", Content.Load<Texture2D>("Images/Menu/PauseMenu/mainHover"));

            DisplayFacade.ButtonsImages.Add("DeadScreenGood", Content.Load<Texture2D>("Images/Buttons/DeadScreen/good"));
            DisplayFacade.ButtonsImages.Add("DeadScreenGoodHover", Content.Load<Texture2D>("Images/Buttons/DeadScreen/goodHover"));
            DisplayFacade.ButtonsImages.Add("DeadScreenBad", Content.Load<Texture2D>("Images/Buttons/DeadScreen/bad"));
            DisplayFacade.ButtonsImages.Add("DeadScreenBadHover", Content.Load<Texture2D>("Images/Buttons/DeadScreen/badHover"));
            DisplayFacade.ButtonsImages.Add("DeadScreenUgly", Content.Load<Texture2D>("Images/Buttons/DeadScreen/ugly"));
            DisplayFacade.ButtonsImages.Add("DeadScreenUglyHover", Content.Load<Texture2D>("Images/Buttons/DeadScreen/uglyHover"));

            DisplayFacade.DebugFont = Content.Load<SpriteFont>("Fonts/debug");
            DisplayFacade.TableFont = Content.Load<SpriteFont>("Fonts/table");
            DisplayFacade.TitleFont = Content.Load<SpriteFont>("Fonts/title");
            DisplayFacade.UIFont = Content.Load<SpriteFont>("Fonts/ui");

            DisplayFacade.ExplosionParticleSystem = new ExplosionParticleSystem(100, Content.Load<Texture2D>("Textures/Particles/explosion"));
            DisplayFacade.ExplosionParticleSystem.AddAffector(new VelocityAffector(Vector3.Down));
            DisplayFacade.ParticleManager.AddParticleSystem(DisplayFacade.ExplosionParticleSystem, BlendState.Additive);

            DisplayFacade.SmokeParticleSystem = new SmokeParticleSystem(100, Content.Load<Texture2D>("Textures/Particles/smoke"));
            DisplayFacade.SmokeParticleSystem.AddAffector(new VelocityAffector(Vector3.Down));
            DisplayFacade.ParticleManager.AddParticleSystem(DisplayFacade.SmokeParticleSystem, BlendState.Additive);

            DisplayFacade.SpriteBatch = new SpriteBatch(GraphicsDevice);

            GeneralFacade.GameStateManager = new GameStateManager();
            GeneralFacade.GameStateManager.Set(new IntroStartingGS());

            DisplayFacade.ScreenElementManager = new ScreenElementManager();

            DisplayFacade.DebugScreen = new DebugScreen();

            GeneralFacade.SceneManager = new SceneManager(GeneralFacade.TempContent);

            DisplayFacade.ModelAnimationManager = new ModelAnimationManager();

            DisplayFacade.Camera = new Camera(
                Vector3.Zero,
                Vector3.Zero
            );


            GeneralFacade.SceneManager.SetScene<SceneStartMenu>();
        }

         
        protected override void UnloadContent()
        {
            // TODO: Unload any non ContentManager content here
        }


        protected override void Update(GameTime gameTime)
        {
            if (Keyboard.GetState().IsKeyDown(Keys.Escape)
                && GeneralFacade.GameStateManager.CurrentGameState == GameStates.Playing)
            {
                GeneralFacade.GameStateManager.Set(new PauseGS());
            }

            switch (GeneralFacade.GameStateManager.CurrentGameState)
            {
                case GameStates.IntroStarting:
                    GeneralFacade.GameStateManager.Set(new IntroGS());
                    break;

                case GameStates.Intro:
                    if (Keyboard.GetState().IsKeyDown(Keys.Escape))
                    {
                        VideoPlayer.Stop();
                    }
                    break;

                case GameStates.LoadingLevel:
                    //
                    break;

                case GameStates.StartMenu:
                case GameStates.Pause:
                case GameStates.ServerListMenu:
                case GameStates.GraphicOptions:
                case GameStates.ErrorScreen:
                case GameStates.DeadScreen:
                    DisplayFacade.ModelAnimationManager.Animate((float)gameTime.ElapsedGameTime.TotalMilliseconds / 1000);
                    DisplayFacade.ScreenElementManager.HandleMouse((float)gameTime.ElapsedGameTime.TotalMilliseconds / 1000);
                    GeneralFacade.SceneManager.Update((float)gameTime.ElapsedGameTime.TotalMilliseconds / 1000);
                    break;

                case GameStates.Playing:
                default:
                    if (GameplayFacade.ThisPlayer.IsAlive())
                    {
                        GameplayFacade.ThisPlayer.HandleInput(gameTime);
                        GameplayFacade.ThisPlayerDisplay.UpdateDisplay();
                    }
                    DisplayFacade.ModelAnimationManager.Animate((float)gameTime.ElapsedGameTime.TotalMilliseconds / 1000);
                    GeneralFacade.SceneManager.Update((float)gameTime.ElapsedGameTime.TotalMilliseconds / 1000);
                    GameplayFacade.BulletManager.Update(gameTime);

                    TestBindings(gameTime);
                    break;
            }

            DisplayFacade.DebugScreen.Update(gameTime);

            base.Update(gameTime);
        }



        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.Black);

            base.Draw(gameTime);

            if (GeneralFacade.GameStateManager.CurrentGameState == GameStates.Intro)
            {
                Texture2D videoTexture = null;
                if (VideoPlayer.State != MediaState.Stopped)
                {
                    videoTexture = VideoPlayer.GetTexture();
                }

                // Draw the video, if we have a texture to draw.
                if (videoTexture != null)
                {
                    ScreenImage sImage = DisplayFacade.ScreenElementManager.GetImage("Intro");
                    sImage.Image = videoTexture;
                    float height = GraphicsDevice.Viewport.Height;
                    float width = GraphicsDevice.Viewport.Width;
                    float vHeight = videoTexture.Height;
                    float vWidth = videoTexture.Width;
                    sImage.ScaleX = width / vWidth;
                    sImage.ScaleY = height / vHeight;
                }
                else
                {
                    VideoPlayer.Dispose();
                    GeneralFacade.GameStateManager.Set(new StartMenuGS());
                }
            }

            DisplayFacade.DebugScreen.Draw(gameTime);
            DisplayFacade.ScreenElementManager.DrawElements(DisplayFacade.SpriteBatch);
        }

        private void TestBindings(GameTime gameTime)
        {
            // Testing purposes: switching clip/noclip
            if (Keyboard.GetState().IsKeyDown(Keys.N))
            {
                CurrentPlayingState = DeimosGame.PlayingStates.NoClip;
                GameplayFacade.ThisPlayerPhysics.timer_gravity = 0;
                GameplayFacade.ThisPlayerPhysics.acceleration = Vector3.Zero;
                GameplayFacade.ThisPlayerPhysics.initial_velocity = 0;
            }

            if (Keyboard.GetState().IsKeyDown(Keys.M))
            {
                CurrentPlayingState = DeimosGame.PlayingStates.Normal;
            }

            // for testing: player death and respawning
            if (Keyboard.GetState().IsKeyDown(Keys.J))
            {
                GameplayFacade.ThisPlayer.PlayerRespawn(new Vector3(18, 10, 90), Vector3.Zero, "main");
                GameplayFacade.ThisPlayer.InitializeInventory(GameplayFacade.ThisPlayer.Class);
                CurrentPlayingState = PlayingStates.Normal;
            }

            if (Keyboard.GetState().IsKeyDown(Keys.K))
            {
                GameplayFacade.ThisPlayer.PlayerKill();
                GameplayFacade.ThisPlayer.Inventory.Flush();
            }

            if (Keyboard.GetState().IsKeyDown(Keys.L))
            {
                GameplayFacade.ThisPlayer.Health--;

                if (GameplayFacade.ThisPlayer.Health == 0)
                {
                    GameplayFacade.ThisPlayer.PlayerKill();
                }
            }

            if (Keyboard.GetState().IsKeyDown(Keys.U))
            {
                GameplayFacade.ThisPlayer.IsMG = true;
                GameplayFacade.ThisPlayer.MGNumber = 0x00;
                GameplayFacade.Minigames.knife.Load();
            }
            if (Keyboard.GetState().IsKeyDown(Keys.I))
            {
                GameplayFacade.Minigames.knife.Terminate();
            }
        }
    }
}
