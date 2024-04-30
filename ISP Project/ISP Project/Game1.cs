using ISP_Project.Game_States;
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

        private State currentState;

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
            // TODO: use this.Content to load your game content here
            
            _spriteBatch = new SpriteBatch(GraphicsDevice);

            currentState = new MenuState(this.Content);
        }

        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            // TODO: Add your update logic here
            InputManager.Update();
            WindowManager.getWindow().updateWindowSize(_graphics);

            /*if (InputManager.isKey(InputManager.Inputs.UP, InputManager.isTriggered))
                Debug.WriteLine("UP");*/

            currentState.Update(gameTime);

            /*if (InputManager.isClick(InputManager.ClickInputs.INTERACT, InputManager.isTriggered))
                Debug.WriteLine("TRIGGERED");
            if (InputManager.isClick(InputManager.ClickInputs.INTERACT, InputManager.isPressed))
                Debug.WriteLine("PRESSED");
            if (InputManager.isClick(InputManager.ClickInputs.INTERACT, InputManager.isReleased))
                Debug.WriteLine("RELEASED");*/



            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.Black);

            // TODO: Add your drawing code here

            // I don't know why I should be using SpriteSortMode.Immediate. If I don't, the button text gets all funky
            _spriteBatch.Begin(SpriteSortMode.Immediate, null, SamplerState.PointClamp, null, null, null, null); // final null should be replaced with camera transformation matrix i think

            currentState.Draw(gameTime, _spriteBatch);

            _spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}
