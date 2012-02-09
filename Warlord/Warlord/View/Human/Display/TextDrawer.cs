using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace Warlord.View.Human.Display
{
    class TextDrawer
    {
        GraphicsDevice graphics;
        SpriteBatch spriteBatch;

        public TextDrawer(GraphicsDevice graphics)
        {
            this.graphics = graphics;
            spriteBatch = new SpriteBatch(graphics);
        }

        public void Drawstring(string text, SpriteFont font, Vector2 position, Color color)
        {
            RasterizerState rs = graphics.RasterizerState;
            DepthStencilState ds = graphics.DepthStencilState;
            BlendState bs = graphics.BlendState;

            spriteBatch.Begin();
            spriteBatch.DrawString(font, text, position, color);
            spriteBatch.End();

            graphics.RasterizerState = rs;
            graphics.DepthStencilState = ds;
            graphics.BlendState = bs;
        }
        public void Drawstrings(string[] text, SpriteFont font, Vector2[] position, Color color)
        {
            RasterizerState rs = graphics.RasterizerState;
            DepthStencilState ds = graphics.DepthStencilState;
            BlendState bs = graphics.BlendState;

            for(int index = 0; index < text.Length; index++)
            {
                spriteBatch.Begin();
                spriteBatch.DrawString(font, text[index], position[index], color);
                spriteBatch.End();
            }

            graphics.RasterizerState = rs;
            graphics.DepthStencilState = ds;
            graphics.BlendState = bs;
        }
    }
}
