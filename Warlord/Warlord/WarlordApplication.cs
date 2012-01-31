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

namespace Warlord
{
    public class WarlordApplication : Microsoft.Xna.Framework.Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
        FlyView flyView;
        FlyInput input;

        InfiniteWorld world;

        static WarlordEventManager eventManager;

        public WarlordApplication()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";

            eventManager = new WarlordEventManager();

            GameStaticInitalizer.InitalizeStatics();

            RegionArrayMaps.Init();

            Vector2i VectorOne = new Vector2i(-4,-4);
        }

        protected override void Initialize()
        {
            base.Initialize();
        }

        private void GenerateWorld()
        {
            world = new InfiniteWorld(27, 8, new Vector3i(16, 128, 16));
            world.Initalize();
        }

        protected override void LoadContent()
        {
            spriteBatch = new SpriteBatch(GraphicsDevice);

            flyView = new FlyView(Window, GraphicsDevice, Content);
            input = new FlyInput(Window);

            GenerateWorld();
        }

        protected override void UnloadContent()
        {
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
        static public bool Active { get; private set; }
        static internal EventManager GameEventManager
        {
            get { return WarlordApplication.eventManager; }
        }
    }
}
