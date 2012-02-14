using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using GameTools;
using XNAGraphicsHelper;

namespace GraphicsHelperTest
{
    public class HelperTest : Microsoft.Xna.Framework.Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
        Camera3D camera;
        CameraMovementController movement;
        SpriteFont font;
        BasicEffect effect;

        WireFrameDrawer wireDrawer;
        TextDrawer textDrawer;

        BoundingBox[] testBoxs;

        public HelperTest()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
        }

        protected override void Initialize()
        {
            wireDrawer = new WireFrameDrawer(GraphicsDevice);

            camera = new Camera3D( Window.ClientBounds, new Vector3(0,0,4f), Vector3.Zero, Vector3.Up);
            movement = new CameraMovementController(camera, Window);
            base.Initialize();
        }
        protected override void LoadContent()
        {
            effect = new BasicEffect(GraphicsDevice);
            font = Content.Load<SpriteFont>("debugFont");

            testBoxs = new BoundingBox[3];

            testBoxs[0] = new BoundingBox(-Vector3.One, Vector3.One);
            testBoxs[1] = new BoundingBox(new Vector3(10, 5, 10), new Vector3(11, 6, 11));
            testBoxs[2] = new BoundingBox(new Vector3(10, 10, 10), new Vector3(16, 16, 16));

            textDrawer = new TextDrawer(GraphicsDevice, font);
            
            spriteBatch = new SpriteBatch(GraphicsDevice);
        }

        protected override void UnloadContent()
        {
        }

        protected override void Update(GameTime gameTime)
        {
            DispatchKeyboardInput();
            DispatchMouseInput();

            base.Update(gameTime);
        }
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.Black);

            wireDrawer.DrawBoundingBoxs( testBoxs, camera.View, camera.Projection);

            string[] text = { "I am text", "As am I!", "Hello There!" };
            Vector3[] positions = { Vector3.Zero, Vector3.One, new Vector3(10,30,20)};

            textDrawer.DrawText3D(text, positions, Color.White, camera.Position, camera.View, camera.Projection, 10);

            DrawLocation( );

            base.Draw(gameTime);
        }
        private void DrawLocation( )
        {            
            Vector3 facing = camera.Facing;

            string[] text = { "X: " + String.Format("{0:0.00}", camera.Position.X),
                              "Y: " + String.Format("{0:0.00}", camera.Position.Y),
                              "Z: " + String.Format("{0:0.00}", camera.Position.Z),
                              "X-Facing: " + String.Format("{0:0.00}", facing.X),
                              "Y-Facing: " + String.Format("{0:0.00}", facing.Y),
                              "Z-Facing: " + String.Format("{0:0.00}", facing.Z)};

            Vector2[] positions = {new Vector2(10, 10), new Vector2(10,30), new Vector2(10,50),
                                   new Vector2(10, 100), new Vector2(10,120), new Vector2(10,140)};
            
            Color[] colors = {Color.Red, Color.Blue, Color.Green, Color.Red, Color.Blue, Color.Green};

            textDrawer.DrawText2D(text, positions, colors);
        }
        public void DispatchKeyboardInput()
        {
            currentKeyboardState = Keyboard.GetState();

            if(IsActive)
            {
                KeysUp();
                KeysDown();
            }

            previousKeyboardState = currentKeyboardState;
        }
        public void DispatchMouseInput()
        {
            currentMouseState = Mouse.GetState();
            previousMouseLoc = new Vector2(previousMouseState.X, previousMouseState.Y);
            currentMouseLoc = new Vector2(currentMouseState.X, currentMouseState.Y);

            if(IsActive)
            {
                if(currentMouseLoc != previousMouseLoc)
                    MouseMove();

                previousMouseState = currentMouseState;
            }            
        }
        private void KeysUp()
        {
            Keys[] previousKeysDown = previousKeyboardState.GetPressedKeys();
            
            foreach(Keys key in previousKeysDown)
            {                
                if(currentKeyboardState.IsKeyUp(key))
                {
                    movement.OnKeyUp(key);
                }
            }
        }
        private void KeysDown()
        {
            Keys[] currentKeysDown = currentKeyboardState.GetPressedKeys();

            foreach(Keys key in currentKeysDown)
            {
                if( key == Keys.Escape )
                    Exit( );

                if(previousKeyboardState.IsKeyDown(key))
                {
                    movement.OnKeyHeld(key);                    
                }
                else
                {
                    movement.OnKeyDown(key);
                }
            }
        }  
        private void MouseMove()
        {
            if(previousMouseLoc != currentMouseLoc)
            {
                movement.OnMouseMove(previousMouseLoc, currentMouseLoc);
            }
        }
        public KeyboardState currentKeyboardState { get; set; }

        public KeyboardState previousKeyboardState { get; set; }

        public MouseState currentMouseState { get; set; }

        public Vector2 previousMouseLoc { get; set; }

        public Vector2 currentMouseLoc { get; set; }

        public MouseState previousMouseState { get; set; }
    }
}

