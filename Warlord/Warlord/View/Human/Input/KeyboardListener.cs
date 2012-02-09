using Microsoft.Xna.Framework.Input;

namespace Warlord.View.Human.Input
{
    interface KeyboardListener
    {
        bool OnKeyDown(Keys key);
        bool OnKeyUp(Keys key);
    }
}
