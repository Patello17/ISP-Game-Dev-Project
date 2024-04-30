using ISP_Project.Managers;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Diagnostics;
using System.Xml;

namespace ISP_Project
{
    public class Game1 : Game
    {
        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;

        public Game1()
        {
            _graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;
        }

        protected override void Initialize()
        {
            // TODO: Add your initialization logic here
            IsMouseVisible = true;
            Window.AllowUserResizing = true;
            Window.AllowAltF4 = true;
            Window.Title = "Snail Mail";

            // set and update window size
            WindowManager.getWindow().setWindowSize(1600, 900);
            WindowManager.getWindow().updateWindowSize(_graphics);

            base.Initialize();
        }

        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);

            // TODO: use this.Content to load your game content here
        }

        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            // TODO: Add your update logic here
            InputManager.Update();
            WindowManager.getWindow().updateWindowSize(_graphics);

            if (InputManager.isKey(InputManager.Inputs.UP, InputManager.isTriggered))
                Debug.WriteLine("UP");

            if (InputManager.isClick(InputManager.ClickInputs.INTERACT, InputManager.isTriggered))
                Debug.WriteLine("TRIGGERED");
            if (InputManager.isClick(InputManager.ClickInputs.INTERACT, InputManager.isPressed))
                Debug.WriteLine("PRESSED");
            if (InputManager.isClick(InputManager.ClickInputs.INTERACT, InputManager.isReleased))
                Debug.WriteLine("RELEASED");

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.Black);

            // TODO: Add your drawing code here

            base.Draw(gameTime);
        }
    }
}
