using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Warlord.View.Human.Screens;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework;

namespace Warlord.View.Human.Input
{
    class DebugFogController : KeyboardListener
    {
        private WorldGraphics worldGraphics;

        public DebugFogController(WorldGraphics worldGraphics)
        {
            this.worldGraphics = worldGraphics;
        }
        public bool OnKeyDown(Keys key)
        {
            Vector2 fogChange = new Vector2(8, 8);

            if(key == Keys.OemOpenBrackets)
            {
                worldGraphics.Fog = new Vector2(worldGraphics.Fog.X - fogChange.X, worldGraphics.Fog.Y);
                return true;
            }
            if(key == Keys.OemCloseBrackets)
            {
                worldGraphics.Fog = new Vector2(worldGraphics.Fog.X + fogChange.X, worldGraphics.Fog.Y);
                return true;
            }
            if(key == Keys.OemMinus)
            {
                worldGraphics.Fog = new Vector2(worldGraphics.Fog.X, worldGraphics.Fog.Y - fogChange.Y);
                return true;
            }
            if(key == Keys.OemPlus)
            {
                worldGraphics.Fog = new Vector2(worldGraphics.Fog.X, worldGraphics.Fog.Y + fogChange.Y);
                return true;
            }

            return false;
        }
        public bool OnKeyHeld(Keys key)
        {
            return false;
        }
        public bool OnKeyUp(Keys key)
        {
            return false;
        }
    }
}
