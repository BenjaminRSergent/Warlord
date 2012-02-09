using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using GameTools.State;
using Warlord.View.Human.Screens;

namespace Warlord.View.Human.State
{
    class DebugPlayingState : State<DebugView>
    {
        GameWindow gameWindow;
        GraphicsDevice graphics;
        ContentManager content;

        public DebugPlayingState(DebugView owner, GameWindow gameWindow, GraphicsDevice graphics, ContentManager content)
            : base(owner)
        {
            this.gameWindow = gameWindow;
            this.graphics = graphics;
            this.content = content;
        }

        public override void EnterState()
        {
            owner.SetCurrentScreen(new DebugPlayScreen(graphics, gameWindow, content));
        }

        public override void Update()
        {
            // Nothing to do
        }

        public override void ExitState()
        {
            // Nothing to do
        }
    }
}
