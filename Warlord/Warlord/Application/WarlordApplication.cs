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
using System.Diagnostics;
using Warlord.GameTools.Statistics;
using GameTools.Process;

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
            eventManager = new WarlordEventManager();
            threadManager = new ThreadManager( );

            errorLogger = new ErrorLogger();
            errorLogger.Init("Error.log", true);

            GameStaticInitalizer.InitalizeStatics();

            RegionArrayMaps.Init();

            logic = new WarlordLogic();

            fpsHelper = new DebugFpsHelper( GraphicsDevice, Content );

            base.Initialize();
        }

        protected override void LoadContent()
        {
            debugView = new DebugView(Window, GraphicsDevice, Content);

            debugView.BeginGame();
            logic.BeginGame();
        }

        protected override void UnloadContent()
        {
            logic.EndGame();
            threadManager.ShutDown( );
        }

        protected override void Update(GameTime gameTime)
        {
            Mouse.WindowHandle = Window.Handle;

            logic.Update(gameTime);
            debugView.HandleInput();

            fpsHelper.CalcFPS( );

            if(Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            base.Update(gameTime);
        }
        protected override void Draw(GameTime gameTime)
        {           
            debugView.Draw(gameTime);          
  
            fpsHelper.DrawFPS( );

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
    }
}
