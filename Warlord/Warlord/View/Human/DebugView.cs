using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Warlord.View.Human.Screens;
using Warlord.View.Human.Display;
using GameTools.State;
using Warlord.View.Human.State;

// Flagged for refentitying

namespace Warlord.View.Human
{
    class DebugView : HumanView
    {
        private GameWindow gameWindow;
        private GraphicsDevice graphics;

        private SpriteBatch spriteBatch;

        private ContentManager content;
        private StateMachine<DebugView> stateMachine;
        private DebugPlayingState debugState;

        public DebugView(GameWindow gameWindow, GraphicsDevice graphics, ContentManager content)
        {
            this.gameWindow = gameWindow;
            this.graphics = graphics;
            this.content = content;            

            spriteBatch = new SpriteBatch(graphics);

            stateMachine = new StateMachine<DebugView>(this);
        }
        public void BeginGame()
        {
            debugState = new DebugPlayingState(this, gameWindow, graphics, content);
            stateMachine.ChangeState(debugState);
        }
        public void SetCurrentScreen(Screen screen)
        {
            CurrentScreen = screen;
        }
        public override void Draw(GameTime gameTime)
        {
            graphics.Clear(Color.SkyBlue);
            base.Draw(gameTime);
        }
        public override void HandleInput()
        {
            base.HandleInput();
        }
        public override void Update(GameTime gameTime)
        {
            debugState.Update();
        }
    }
}
