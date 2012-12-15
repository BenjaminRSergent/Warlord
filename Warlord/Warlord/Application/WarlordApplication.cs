using GameTools.Process;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Warlord.Event;
using Warlord.Logic;
using Warlord.Logic.Data.Entity;
using Warlord.Logic.Data.World;
using Warlord.View.Human;
using GameTools.Graph;
using Warlord.View.Human.Display.EntityView;
using Warlord.View.Human.Display;
using Microsoft.Xna.Framework.Graphics;
using Warlord.Interfaces;

namespace Warlord.Application
{
    internal class WarlordApplication : Game
    {
        GraphicsDeviceManager graphics;

        WarlordLogic logic;
        DebugView debugView;
        DebugFpsHelper fpsHelper;

        ErrorLogger errorLogger;
        WarlordEventManager eventManager;
        ThreadManager threadManager;

        public WarlordApplication()
        {
            GlobalSystems.SetCurrentApplication(this);

            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
        }

        protected override void Initialize()
        {
            graphics.PreferredBackBufferWidth = 1280;
            graphics.PreferredBackBufferHeight = 900;

            graphics.ApplyChanges();
            
            eventManager = new WarlordEventManager();
            threadManager = new ThreadManager();

            TextureRepository.BlockTextures = Content.Load<Texture2D>("Textures/Blocks/block_textures");            
            TextureRepository.BearTexture = Content.Load<Texture2D>("Textures/Models/bear_texture");
            FontRepository.DebugFont = Content.Load<SpriteFont>("Font/DebugFont");

            ModelRepository.Initialize(Content);

            errorLogger = new ErrorLogger();
            errorLogger.Init("Error.log", true);

            GameStaticInitalizer.InitalizeStatics();

            logic = new WarlordLogic();

            fpsHelper = new DebugFpsHelper(GraphicsDevice);

            base.Initialize();
        }

        protected override void LoadContent()
        {
            debugView = new DebugView(Window, GraphicsDevice, Content);

            logic.BeginGame(new Vector3( 16, 128, 16), 8, 10);
            debugView.BeginGame();            
        }

        protected override void UnloadContent()
        {
            logic.EndGame();
            threadManager.ShutDown();
            errorLogger.Dispose( );
        }

        protected override void Update(GameTime gameTime)
        {
            Mouse.WindowHandle = Window.Handle;

            logic.Update(gameTime);
            debugView.HandleInput();
            debugView.Update(gameTime);

            fpsHelper.CalcFPS();

            if(Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            base.Update(gameTime);
        }
        protected override void Draw(GameTime gameTime)
        {
            debugView.Draw(gameTime);

            fpsHelper.DrawFPS();

            base.Draw(gameTime);
        }

        public void ReportError(string errorReport)
        {
            errorLogger.Write(errorReport);
        }
        public WarlordEventManager EventManager
        {
            get { return eventManager; }
        }
        public WarlordEntityManager EntityManager
        {
            get { return logic.EntityManager; }
        }
        public ThreadManager ThreadManager
        {
            get { return threadManager; }
        }

        public BlockAccess Blocks { get{ return logic;} }
    }
}
