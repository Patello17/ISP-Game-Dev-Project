﻿using ISP_Project.Game_States;
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
            // Globals.GraphicsDeviceManager = _graphics;
            Content.RootDirectory = "Content";
            IsMouseVisible = true;
        }

        protected override void Initialize()
        {
            // TODO: Add your initialization logic here
            IsMouseVisible = true;
            Window.AllowUserResizing = false;
            Window.AllowAltF4 = true;
            Window.Title = "Snail Mail";

            // set and update window size
            WindowManager.InitializeWindow(_graphics);
            WindowManager.SetMainWindowSize(1280, 720);
            // _graphics.ToggleFullScreen
            _graphics.PreferMultiSampling = false;

            base.Initialize();
        }

        protected override void LoadContent()
        {
            // TODO: use this.Content to load your game content here
            
            _spriteBatch = new SpriteBatch(GraphicsDevice);
            Globals.ContentManager = this.Content;
            Globals.SpriteBatch = _spriteBatch;

            AudioManager.LoadAudio();
        }

        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || quit)
                Exit();

            // TODO: Add your update logic here
            Globals.Update(gameTime);
            WindowManager.Update(gameTime);
            InputManager.Update(gameTime);
            StateManager.Update(gameTime);
            AudioManager.Update(gameTime);

            /*if (InputManager.isKey(InputManager.Inputs.LEFT, InputManager.isTriggered))
                WindowManager.SetMainWindowSize(320, 180);
            if (InputManager.isKey(InputManager.Inputs.UP, InputManager.isTriggered))
                WindowManager.SetMainWindowSize(480, 320);
            if (InputManager.isKey(InputManager.Inputs.RIGHT, InputManager.isTriggered))
                WindowManager.SetMainWindowSize(1280, 720);*/
            if (InputManager.isKey(InputManager.Inputs.MAXIMIZESCREEN, InputManager.isTriggered))
            {
                WindowManager.SetMainWindowSize(1280, 720);
                _graphics.ToggleFullScreen();
            }

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
            
            WindowManager.DrawMainWindow(gameTime);

            base.Draw(gameTime);
        }
    }
}
