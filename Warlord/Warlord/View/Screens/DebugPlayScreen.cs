using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using GameTools;
using Warlord.View.Human.Input;

namespace Warlord.View.Human.Screens
{
    class DebugPlayScreen : Screen
    {
        private WorldGraphics worldGraphics;
        private DebugMovementController movementController;
        private Camera3D camera;

        public DebugPlayScreen(GraphicsDevice graphics, GameWindow gameWindow, ContentManager content)
        {
            camera = new Camera3D(gameWindow.ClientBounds, new Vector3(0, 13, 0), Vector2.Zero, Vector3.Up);
            worldGraphics = new WorldGraphics(graphics, gameWindow, content, camera);

            movementController = new DebugMovementController(camera, gameWindow);

            PushScreenElement(worldGraphics);
            PushMouseListener(movementController);
            PushKeyboardListener(movementController);
        }
    }
}

