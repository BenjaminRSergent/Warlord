using System.Diagnostics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using GameTools;
using Warlord.View.Human.Display;
using System;

namespace Warlord.Application
{
    class DebugFpsHelper
    {
        private Stopwatch stopWatch;

        private TextDrawer textDrawer;
        private FloatSmoother floatSmoother;

        public DebugFpsHelper(GraphicsDevice graphics)
        {
            stopWatch = new Stopwatch();
            textDrawer = new TextDrawer(graphics);

            floatSmoother = new FloatSmoother(60);

            stopWatch.Start();
        }

        public void CalcFPS()
        {
            if(stopWatch.ElapsedMilliseconds != 0)
                floatSmoother.AddValue(1000 / stopWatch.ElapsedMilliseconds);

            stopWatch.Restart();
        }
        public void DrawFPS()
        {
            int smoothFps = (int)floatSmoother.SmoothedValue;

            if(FontRepository.DebugFont != null)
            {
                string output = "Current FPS: " + smoothFps;
                Vector2 position = new Vector2(20, 20);

                textDrawer.Drawstring(output, FontRepository.DebugFont, position, Color.Yellow);
            }
        }
    }
}
