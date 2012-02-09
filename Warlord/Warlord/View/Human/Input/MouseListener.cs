using Microsoft.Xna.Framework;

namespace Warlord.View.Human.Input
{
    interface MouseListener
    {
        bool OnMouseMove(Vector2 prevPosition, Vector2 currentPosition);
        bool OnLButtonDown(Vector2 location);
        bool OnLButtonUp(Vector2 location);
        bool OnRButtonDown(Vector2 location);
        bool OnRButtonUp(Vector2 location);
        bool OnMouseWheel(float deltaWheelMove);
    }
}
