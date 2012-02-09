using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;
using Microsoft.Xna.Framework.Graphics;
using Warlord.GameTools.Statistics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;

namespace Warlord.Application
{
    class DebugFpsHelper
    {
        private SpriteBatch debugSpriteBatch;
        private SpriteFont debugFont;
        private Stopwatch stopWatch;        

        private double[] fps = new double[60];
        private int advFps = 60;
        private int fpsIndex;
        private GraphicsDevice graphics;

        public DebugFpsHelper( GraphicsDevice graphics, ContentManager content )
        {
            stopWatch = new Stopwatch( );

            this.graphics = graphics;

            stopWatch.Start( );
            debugSpriteBatch = new SpriteBatch(graphics);
            debugFont = content.Load<SpriteFont>("Font/DebugFont");
        }

        public void CalcFPS( )
        {
            if(fpsIndex > fps.Length-1)
                fpsIndex = 0;

            long realDeltaTime = stopWatch.ElapsedMilliseconds;
            if( realDeltaTime > 0)
                fps[fpsIndex] = 1000/realDeltaTime;
            stopWatch.Restart( );

            fpsIndex++;

        }        
        public void DrawFPS( )
        {
            RasterizerState rs = graphics.RasterizerState;
            DepthStencilState ds = graphics.DepthStencilState;
            BlendState bs = graphics.BlendState;           

            int smoothFps = (int)Statistics.Adverage(fps);
            advFps += smoothFps;
            advFps /= 2;

            debugSpriteBatch.Begin( );
            debugSpriteBatch.DrawString( debugFont, "Current FPS: " + smoothFps, new Vector2( 20, 20), Color.Yellow);
            debugSpriteBatch.DrawString( debugFont, "Adverage FPS: " + advFps, new Vector2( 20, 40), Color.Yellow);
            debugSpriteBatch.End( );

            graphics.RasterizerState = rs;
            graphics.DepthStencilState = ds;
            graphics.BlendState = bs;
        }
    }
}
