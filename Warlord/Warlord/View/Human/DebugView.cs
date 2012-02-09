using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Warlord.View.Human.Screens;
using Warlord.View.Human.Display;
using GameTools.State;
using Warlord.View.Human.State;

// Flagged for refactoring

namespace Warlord.View.Human
{
    class DebugView : HumanView
    {
        private GameWindow gameWindow;
        private GraphicsDevice graphics;

        private SpriteBatch spriteBatch;

        ContentManager content;

        StateMachine<DebugView> stateMachine;

        public DebugView(GameWindow gameWindow, GraphicsDevice graphics, ContentManager content)
        {
            this.gameWindow = gameWindow;
            this.graphics = graphics;
            this.content = content;

            TextureRepository.BlockTextures = content.Load<Texture2D>("Textures/Blocks/block_textures");
            FontRepository.DebugFont = content.Load<SpriteFont>("Font/DebugFont");

            spriteBatch = new SpriteBatch(graphics);

            stateMachine = new StateMachine<DebugView>(this);
        }
        public void BeginGame()
        {
            stateMachine.ChangeState(new DebugPlayingState(this, gameWindow, graphics, content));
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
        }
    }
}
