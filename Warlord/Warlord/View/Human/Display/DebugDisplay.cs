using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Warlord.Interfaces.Subsystems;
using GameTools;
using Microsoft.Xna.Framework;
using Warlord.Event;
using Warlord.Logic.Data;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;
using GameTools.Graph;
using GameTools.State;
using Warlord.View.Sates;

// Flagged for refactoring

namespace Warlord.View.Human.Display
{
    class DebugDisplay : GameView
    {
        GameWindow gameWindow;
        GraphicsDevice graphics;

        SpriteBatch spriteBatch;
        SpriteFont spriteFont;

        ContentManager content;

        StateMachine<DebugDisplay> stateMachine;               

        public DebugDisplay(GameWindow gameWindow, GraphicsDevice graphics, ContentManager content)
        {
            this.gameWindow = gameWindow;
            this.graphics = graphics;           
            this.content = content;
            
            TextureRepository.BlockTextures = content.Load<Texture2D>("Textures/Blocks/block_textures");

            GlobalApplication.Application.GameEventManager.Subscribe(Draw, "draw");
            spriteBatch = new SpriteBatch(graphics);
            spriteFont = content.Load<SpriteFont>("Font/DebugFont");

            stateMachine = new StateMachine<DebugDisplay>( this );
        }
        public void BeginGame( )
        {
            stateMachine.ChangeState( new DebugDisplayPlayState(this, gameWindow, graphics, spriteBatch, content) );
        }
        private void Draw(object sender, object gameTimeObject)
        {           
            stateMachine.Update( );
        }                
    }
}
