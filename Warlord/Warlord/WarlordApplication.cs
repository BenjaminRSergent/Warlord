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

namespace Warlord
{
    internal class WarlordApplication : Game, GameApplication
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;

        WarlordLogic logic;
        DebugDisplay debugDisplay;
        DebugInput debugInput;

        ErrorLogger errorLogger;

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

            debugDisplay = new DebugDisplay(Window, GraphicsDevice, Content);
            debugInput = new DebugInput(Window);

            debugDisplay.BeginGame( );
            logic.BeginGame( );
        }

        protected override void UnloadContent()
        {            
            logic.EndGame( );
        }

        protected override void Update(GameTime gameTime)
        {
            Active = IsActive;

            Mouse.WindowHandle = Window.Handle;

            eventManager.SendEvent(new GameEvent(new GameTools.Optional<object>(this), "read_input", null, 0));
            eventManager.SendEvent(new GameEvent(new GameTools.Optional<object>(this), "update", gameTime, 0));

            if(Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {           
            eventManager.SendEvent(new GameEvent(new GameTools.Optional<object>(this), "draw", gameTime, 0));

            base.Draw(gameTime);
        }       
        
        public void ReportError(string errorReport)
        {
            errorLogger.Write(errorReport);
        }
        public EventManager GameEventManager
        {
            get { return WarlordApplication.eventManager; }
        }
        public EntityManager EntityManager
        {
            get { return logic.EntityManager; }
        }

        public bool Active { get; private set; }
    }
}
