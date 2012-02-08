using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Input;

namespace Warlord.View.Human.Input
{
    interface KeyboardListener
    {
        bool OnKeyDown(Keys key);
        bool OnKeyUp(Keys key);
    }
}
