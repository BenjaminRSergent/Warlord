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
using Warlord.View.Human.Input;

// Flagged for refactoring

namespace Warlord.View.Human.Display
{
    class DebugView : HumanView
    {
        GameWindow gameWindow;
        GraphicsDevice graphics;
        Camera3D camera;

        SpriteBatch spriteBatch;
        SpriteFont spriteFont;

        ContentManager content;

        public DebugView(GameWindow gameWindow, GraphicsDevice graphics, ContentManager content)
        {
            this.gameWindow = gameWindow;
            this.graphics = graphics;
            this.content = content;

            TextureRepository.BlockTextures = content.Load<Texture2D>("Textures/Blocks/block_textures");
            spriteBatch = new SpriteBatch(graphics);
            spriteFont = content.Load<SpriteFont>("Font/DebugFont");

            camera = new Camera3D(gameWindow.ClientBounds, new Vector3(0, 12, 0), Vector2.Zero, Vector3.Up);
        }
        public void BeginGame()
        {
            DebugMovementController controller = new DebugMovementController(camera, gameWindow);
            PushScreenElement(new WorldGraphics(graphics, gameWindow, content, camera));
            KeyboardListeners.Add(controller);
            MouseListeners.Add(controller);
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
