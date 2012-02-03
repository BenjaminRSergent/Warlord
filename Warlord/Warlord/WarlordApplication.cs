using System;
using System.Collections.Generic;
using System.Linq;
using GameTools.Graph;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using Warlord.Event;
using Warlord.Logic;
using Warlord.Logic.Data;
using Warlord.Logic.Data.World;
using Warlord.View;
using GameTools.Noise3D;

namespace Warlord
{
    internal class WarlordApplication : Game, GameApplication
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
        WarlordLogic logic;
        ErrorLogger errorLogger;
        FlyView flyView;
        FlyInput input;

        GlobalApplication globalAccess;

        static WarlordEventManager eventManager;

        public WarlordApplication()
        {
            globalAccess = new GlobalApplication(this);

            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";

            eventManager = new WarlordEventManager();

            errorLogger = new ErrorLogger( );
            errorLogger.Init("Error.log", true);

            GameStaticInitalizer.InitalizeStatics();

            RegionArrayMaps.Init();

            Vector2i VectorOne = new Vector2i(-4, -4);

            logic = new WarlordLogic();
        }

        protected override void Initialize()
        {
            base.Initialize();
        }

        protected override void LoadContent()
        {
            spriteBatch = new SpriteBatch(GraphicsDevice);

            flyView = new FlyView(Window, GraphicsDevice, Content);
            input = new FlyInput(Window);

            logic.BeginGame( );
        }

        protected override void UnloadContent()
        {            
            logic.ShutDown( );
        }

        protected override void Update(GameTime gameTime)
        {
            Active = IsActive;
            eventManager.SendEvent(new GameEvent(new GameTools.Optional<object>(this), "update", gameTime, 0));

            if(Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.SkyBlue);

            eventManager.SendEvent(new GameEvent(new GameTools.Optional<object>(this), "draw", gameTime, 0));

            base.Draw(gameTime);
        }
        public void ReportError(string errorReport)
        {
            errorLogger.Write(errorReport);
        }
        public bool Active { get; private set; }
        public EventManager GameEventManager
        {
            get { return WarlordApplication.eventManager; }
        }
        public EntityManager EntityManager
        {
            get { return logic.EntityManager; }
        }
        public ProcessManager ProcessManager
        {
            get
            {
                return logic.ProcessManager;
            }
        }        
    }
}
