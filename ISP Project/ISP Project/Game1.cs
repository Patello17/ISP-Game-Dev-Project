using ISP_Project.Game_States;
using ISP_Project.Game_States.Levels;
using ISP_Project.Managers;
using ISP_Project.Screen_Management;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using MonoGame.Extended;
using System;
using System.Diagnostics;
using System.Xml;

namespace ISP_Project
{
    public class Game1 : Game
    {
        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;

        public static bool quit = false;

        public Game1()
        {
            _graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;
        }

        protected override void Initialize()
        {
            // TODO: Add your initialization logic here
            IsMouseVisible = false; // not using mouse
            Window.AllowUserResizing = false;
            Window.AllowAltF4 = true;
            Window.Title = "Snail Mail";

            // _graphics.ToggleFullScreen
            _graphics.PreferMultiSampling = false;

            base.Initialize();
        }

        protected override void LoadContent()
        {
            // TODO: use this.Content to load your game content here
            
            Globals.ContentManager = Content;
            Globals.GraphicsDevice = GraphicsDevice;
            // Globals.StateGraphicsDevice = new GraphicsDevice(this.GraphicsDevice.Adapter, GraphicsProfile.Reach, new PresentationParameters());
            // Debug.WriteLine(Globals.GraphicsDevice);
            _spriteBatch = new SpriteBatch(GraphicsDevice);
            Globals.SpriteBatch = _spriteBatch;

            // set and update window size
            WindowManager.InitializeWindow(_graphics);
            WindowManager.SetMainWindowSize(1280, 720);


            AudioManager.LoadAudio();
        }

        protected override void Update(GameTime gameTime)
        {
            // quit game
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || quit)
                Exit();

            // maximize the screen
            if (InputManager.isKey(InputManager.Inputs.MAXIMIZESCREEN, InputManager.isTriggered))
            {
                WindowManager.SetMainWindowSize(1280, 720);
                _graphics.ToggleFullScreen();
            }

            // update managers
            Globals.Update(gameTime);
            WindowManager.Update();
            StateManager.Update();
            InputManager.Update();
            AudioManager.Update();

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            Globals.GraphicsDevice.Clear(Color.Black);

            // TODO: Add your drawing code here


            // I don't know why I should be using SpriteSortMode.Immediate. If I don't, the button text gets all funky
            
            WindowManager.DrawMainWindow(gameTime);

            base.Draw(gameTime);
        }
    }
}
