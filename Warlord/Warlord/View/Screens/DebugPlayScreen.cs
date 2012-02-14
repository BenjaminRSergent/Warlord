using GameTools;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Warlord.View.Human.Input;

namespace Warlord.View.Human.Screens
{
    class DebugPlayScreen : Screen
    {
        private WorldGraphics worldGraphics;
        private DebugMovementController movementController;
        private DebugFogController fogController;
        private Camera3D camera;

        public DebugPlayScreen(GraphicsDevice graphics, GameWindow gameWindow, ContentManager content)
        {
            camera = new Camera3D(gameWindow.ClientBounds, new Vector3(0, 13, 0), new Vector3(0, 13, 1), Vector3.Up);
            worldGraphics = new WorldGraphics(graphics, gameWindow, content, camera);

            movementController = new DebugMovementController(camera, gameWindow);
            fogController = new DebugFogController(worldGraphics);

            PushScreenElement(worldGraphics);
            PushMouseListener(movementController);
            PushKeyboardListener(movementController);
            PushKeyboardListener(fogController);
        }
    }
}

