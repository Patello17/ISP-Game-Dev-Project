using ISP_Project.Components;
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
        private int selectedButton;
        private Texture2D selectorTexture;
        private Texture2D backgroundTexture;
        private Texture2D buttonTexture;
        private SpriteFont buttonFont;

        public SettingsState()
        {
            LoadState();

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

        public override void LoadState()
        {
            selectorTexture = Globals.ContentManager.Load<Texture2D>("Snail/Snail");
            backgroundTexture = Globals.ContentManager.Load<Texture2D>("Backgrounds/Title Background");
            buttonTexture = Globals.ContentManager.Load<Texture2D>("UI Elements/Button");
            buttonFont = Globals.ContentManager.Load<SpriteFont>("Fonts/Button Font");
        }

        public override void Update()
        {
            // keyboard only select
            if (InputManager.isKey(InputManager.Inputs.UP, InputManager.isTriggered))
            {
                selectedButtonCounter++;
                AudioManager.PlaySoundEffect("Scroll");
            }
            if (InputManager.isKey(InputManager.Inputs.DOWN, InputManager.isTriggered))
            {
                selectedButtonCounter--;
                AudioManager.PlaySoundEffect("Scroll");
            }
            if (selectedButtonCounter >= 0)
                selectedButtonCounter = -buttons.Count;

            selectedButton = Math.Abs(selectedButtonCounter % buttons.Count);

            bool isHovering = false;
            foreach (Button button in buttons)
            {
                button.Update();
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

        public override void PostUpdate()
        {
            // unload sprites if they're not needed
        }

        public override void Draw()
        {
            var backgroundOrigin = new Vector2(backgroundTexture.Width / 2, backgroundTexture.Height / 2);
            Globals.SpriteBatch.Draw(backgroundTexture, WindowManager.GetMainWindowCenter(), null, Color.White, 0f, backgroundOrigin, 1f, SpriteEffects.None, 0f);

            var movesText = "SETTINGS";
            var movesX = WindowManager.GetMainWindowCenter().X - (buttonFont.MeasureString(movesText).X * 1f / 2);
            var movesY = WindowManager.GetMainWindowCenter().Y - 80 - (buttonFont.MeasureString(movesText).Y * 1f / 2);
            Globals.SpriteBatch.DrawString(buttonFont, movesText, new Vector2(movesX, movesY),
                Color.White, 0f, Vector2.Zero, 1f, SpriteEffects.None, 1f);

            foreach (Button button in buttons)
            {
                button.Draw();
            }

            var selectorOrigin = new Vector2(selectorTexture.Width / 2, selectorTexture.Height / 2);
            Globals.SpriteBatch.Draw(selectorTexture,
                WindowManager.GetMainWindowCenter() - new Vector2(56, 16) + (new Vector2(0, 16 * selectedButton)),
                null, Color.White, 0f, selectorOrigin, 1f, SpriteEffects.None, 1f);
        }

        public override void PlayStateSong()
        {
            // play music
        }
    }
}
