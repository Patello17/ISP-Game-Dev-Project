﻿using ISP_Project.Components;
using ISP_Project.Managers;
using ISP_Project.UI.Buttons;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ISP_Project.Game_States
{
    public class SettingsState : State
    {
        private List<Button> buttons;
        private int selectedButtonCounter = 0;
        // create variables for the textures and fonts of the buttons (Buttons can share the same texture/font)
        private Texture2D buttonTexture;
        private SpriteFont buttonFont;

        public SettingsState(ContentManager content)
        {
            LoadState(content);

            var audioButton = new AudioButton(buttonTexture, buttonFont, 1, 0.5f)
            {
                Sprite = new Sprite(buttonTexture, SpriteEffects.None, 0),
                Position = new Vector2(WindowManager.GetMainWindowCenter().X, WindowManager.GetMainWindowCenter().Y - 16),
                Text = "Audio"
            };

            var controlsButton = new ControlsButton(buttonTexture, buttonFont, 1, 0.5f)
            {
                Sprite = new Sprite(buttonTexture, SpriteEffects.None, 0),
                Position = new Vector2(WindowManager.GetMainWindowCenter().X, WindowManager.GetMainWindowCenter().Y),
                Text = "Controls"
            };

            var settingsReturnButton = new ReturnButton(buttonTexture, buttonFont, 1, 0.5f)
            {
                Sprite = new Sprite(buttonTexture, SpriteEffects.None, 0),
                Position = new Vector2(WindowManager.GetMainWindowCenter().X, WindowManager.GetMainWindowCenter().Y + 16),
                Text = "Return"
            };

            buttons = new List<Button>()
            {
                audioButton, controlsButton, settingsReturnButton
            };

        }
        public override void LoadState(ContentManager content)
        {
            // load everything in this state
            buttonTexture = content.Load<Texture2D>("UI Elements/Button");
            buttonFont = content.Load<SpriteFont>("Fonts/Button Font");
        }
        public override void Update(GameTime gameTime)
        {
            // keyboard only select
            if (InputManager.isKey(InputManager.Inputs.UP, InputManager.isTriggered))
                selectedButtonCounter++;
            if (InputManager.isKey(InputManager.Inputs.DOWN, InputManager.isTriggered))
                selectedButtonCounter--;
            if (selectedButtonCounter >= 0)
                selectedButtonCounter = -buttons.Count;

            var selectedButton = Math.Abs(selectedButtonCounter % buttons.Count);

            bool isHovering = false;
            foreach (Button button in buttons)
            {
                button.Update(gameTime);
                button.ForceShade = false;
                if (isHovering == false)
                    isHovering = button.GetCursorHover();
            }

            if (!isHovering)
                buttons[selectedButton].ForceShade = true;

            if (InputManager.isKey(InputManager.Inputs.INTERACT, InputManager.isTriggered))
            {
                buttons[selectedButton].TriggerEvent();
            }
        }

        public override void PostUpdate(GameTime gameTime)
        {
            // unload sprites if they're not needed
        }

        public override void Draw(GameTime gameTime)
        {
            foreach (Button button in buttons)
            {
                button.Draw(gameTime);
            }
        }
    }
}
