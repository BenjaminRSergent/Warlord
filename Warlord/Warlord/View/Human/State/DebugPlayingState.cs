using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using GameTools.State;
using Warlord.View.Human.Screens;
using Warlord.Application;

namespace Warlord.View.Human.State
{
    class DebugPlayingState : State<DebugView>
    {
        private GameWindow gameWindow;
        private GraphicsDevice graphics;
        private ContentManager content;
        private DebugPlayScreen debugScreen;

        public DebugPlayingState(DebugView owner, GameWindow gameWindow, GraphicsDevice graphics, ContentManager content)
            : base(owner)
        {
            this.gameWindow = gameWindow;
            this.graphics = graphics;
            this.content = content;
        }

        public override void EnterState()
        {
            debugScreen = new DebugPlayScreen(graphics, gameWindow, content);
            owner.SetCurrentScreen(debugScreen);
        }

        public override void Update()
        {   
            GlobalSystems.EntityManager.Player.Teleport(debugScreen.Camera.Position);
        }

        public override void ExitState()
        {
            // Nothing to do
        }
    }
}
