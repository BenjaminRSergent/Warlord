using GameTools.Graph;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Warlord.Event;
using Warlord.Logic;
using Warlord.Logic.Data.Entity;
using Warlord.Logic.Data.World;
using Warlord.View;
using Warlord.View.Human.Input;
using Warlord.View.Human.Display;
using System;

namespace Warlord
{
    internal class WarlordApplication : Game
    {
        private const long MEMORY_THRESHOLD = 1536000000;

        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;

        WarlordLogic logic;
        DebugView debugView;

        ErrorLogger errorLogger;

        WarlordEventManager eventManager;        

        public WarlordApplication()
        {
            GlobalSystems.SetCurrentApplication(this);

            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";

            eventManager = new WarlordEventManager();

            errorLogger = new ErrorLogger();
            errorLogger.Init("Error.log", true);

            GameStaticInitalizer.InitalizeStatics();

            RegionArrayMaps.Init();

            Vector2i VectorOne = new Vector2i(-4, -4);

            IsFixedTimeStep = false;

            logic = new WarlordLogic();
        }

        protected override void Initialize()
        {
            base.Initialize();
        }

        protected override void LoadContent()
        {
            spriteBatch = new SpriteBatch(GraphicsDevice);

            debugView = new DebugView(Window, GraphicsDevice, Content);

            debugView.BeginGame();
            logic.BeginGame();
        }

        protected override void UnloadContent()
        {
            logic.EndGame();
        }

        protected override void Update(GameTime gameTime)
        {
            Mouse.WindowHandle = Window.Handle;

            logic.Update(gameTime);
            debugView.HandleInput();

            if( GC.GetTotalMemory(false) > MEMORY_THRESHOLD )
                GC.Collect( );


            if(Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            debugView.Draw(gameTime);

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
    }
}
