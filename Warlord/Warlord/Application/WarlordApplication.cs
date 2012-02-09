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

namespace Warlord
{
    internal class WarlordApplication : Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch debugSpriteBatch;

        WarlordLogic logic;
        DebugView debugView;

        ErrorLogger errorLogger;

        WarlordEventManager eventManager;        

        Stopwatch stopWatch;
        double[] fps = new double[60];
        int advFps = 60;
        int fpsIndex;
        private SpriteFont debugFont;

        public WarlordApplication()
        {
            GlobalSystems.SetCurrentApplication(this);

            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";            
        }

        protected override void Initialize()
        {
            eventManager = new WarlordEventManager();

            errorLogger = new ErrorLogger();
            errorLogger.Init("Error.log", true);

            GameStaticInitalizer.InitalizeStatics();

            RegionArrayMaps.Init();

            logic = new WarlordLogic();

            stopWatch = new Stopwatch( );

            stopWatch.Start( );

            base.Initialize();
        }

        protected override void LoadContent()
        {
            debugSpriteBatch = new SpriteBatch(GraphicsDevice);

            debugView = new DebugView(Window, GraphicsDevice, Content);

            debugFont = Content.Load<SpriteFont>("Font/DebugFont");

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

            CalcFPS( );

            if(Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            base.Update(gameTime);
        }
        protected override void Draw(GameTime gameTime)
        {           
            debugView.Draw(gameTime);            
            
            DrawFPS( );

            base.Draw(gameTime);
        }
        protected void CalcFPS( )
        {
            if(fpsIndex > fps.Length-1)
                fpsIndex = 0;

            long realDeltaTime = stopWatch.ElapsedMilliseconds;
            if( realDeltaTime > 0)
                fps[fpsIndex] = 1000/realDeltaTime;
            stopWatch.Restart( );

            fpsIndex++;

        }        
        private void DrawFPS( )
        {
            RasterizerState rs = GraphicsDevice.RasterizerState;
            DepthStencilState ds = GraphicsDevice.DepthStencilState;
            BlendState bs = GraphicsDevice.BlendState;

            int smoothFps = (int)Statistics.Adverage(fps);
            advFps += smoothFps;
            advFps /= 2;

            debugSpriteBatch.Begin( );
            debugSpriteBatch.DrawString( debugFont, "Current FPS: " + smoothFps, new Vector2( 20, 20), Color.Yellow);
            debugSpriteBatch.DrawString( debugFont, "Adverage FPS: " + advFps, new Vector2( 20, 40), Color.Yellow);
            debugSpriteBatch.End( );

            GraphicsDevice.RasterizerState = rs;
            GraphicsDevice.DepthStencilState = ds;
            GraphicsDevice.BlendState = bs;
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
