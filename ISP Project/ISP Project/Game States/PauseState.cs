using ISP_Project.Components;
using ISP_Project.Game_States.Letters;
using ISP_Project.Game_States.Levels;
using ISP_Project.Managers;
using ISP_Project.UI.Buttons;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ISP_Project.Game_States
{
    public class PauseState : State
    {
        private List<Button> buttons;
        private int selectedButtonCounter = 0;
        private int selectedButton;
        private Texture2D selectorTexture;
        private Texture2D backgroundTexture;
        private Texture2D buttonTexture;
        private SpriteFont buttonFont;
        private Texture2D controlsUI;

        private ResumeButton resumeButton;
        private SettingsButton settingsButton;
        private QuitToTitleButton quitToTitleButton;
        private HubReturnButton quitToHubButton;

        public PauseState() 
        {
            LoadState();
            
            resumeButton = new ResumeButton(buttonTexture, buttonFont, 1, 0.5f)
            {
                Sprite = new Sprite(buttonTexture, SpriteEffects.None, 0),
                Position = new Vector2(WindowManager.GetMainWindowCenter().X, WindowManager.GetMainWindowCenter().Y - 16),
                Text = "Resume"
            };

            settingsButton = new SettingsButton(buttonTexture, buttonFont, 1, 0.5f)
            {
                Sprite = new Sprite(buttonTexture, SpriteEffects.None, 0),
                Position = new Vector2(WindowManager.GetMainWindowCenter().X, WindowManager.GetMainWindowCenter().Y),
                Text = "Settings"
            };

            quitToTitleButton = new QuitToTitleButton(buttonTexture, buttonFont, 1, 0.5f)
            {
                Sprite = new Sprite(buttonTexture, SpriteEffects.None, 0),
                Position = new Vector2(WindowManager.GetMainWindowCenter().X, WindowManager.GetMainWindowCenter().Y + 16),
                Text = "Exit"
            };

            quitToHubButton = new HubReturnButton(buttonTexture, buttonFont, 1, 0.5f)
            {
                Sprite = new Sprite(buttonTexture, SpriteEffects.None, 0),
                Position = new Vector2(WindowManager.GetMainWindowCenter().X, WindowManager.GetMainWindowCenter().Y + 16),
                Text = "To Hub"
            };

            // change exit button between hub and title
            List<State> states = new List<State>()
            {
                new TitleState(), new HubState(), new LevelOneState(), new LevelSelectionState()
            };

            if (StateManager.GetMostRecentState(states) is HubState)
            {
                buttons = new List<Button>()
                {
                    resumeButton, settingsButton, quitToTitleButton
                };
            }
            else
            {
                buttons = new List<Button>()
                {
                    resumeButton, settingsButton, quitToHubButton
                };
            }
        }

        public override void LoadState()
        {
            selectorTexture = Globals.ContentManager.Load<Texture2D>("Snail/Snail");
            backgroundTexture = Globals.ContentManager.Load<Texture2D>("Backgrounds/Title Background");
            buttonTexture = Globals.ContentManager.Load<Texture2D>("UI Elements/Button");
            buttonFont = Globals.ContentManager.Load<SpriteFont>("Fonts/Button Font");
            controlsUI = Globals.ContentManager.Load<Texture2D>("UI Elements/Controls Display Pause");

            selectedButton = 1;
        }

        public override void Update()
        {
            // unpause
            if (InputManager.isKey(InputManager.Inputs.PAUSE, InputManager.isTriggered))
            {
                resumeButton.TriggerEvent();
            }

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

            var text = "PAUSED";
            var testX = WindowManager.GetMainWindowCenter().X - (buttonFont.MeasureString(text).X * 1f / 2);
            var textY = WindowManager.GetMainWindowCenter().Y - 80 - (buttonFont.MeasureString(text).Y * 1f / 2);
            Globals.SpriteBatch.DrawString(buttonFont, text, new Vector2(testX, textY),
                Color.White, 0f, Vector2.Zero, 1f, SpriteEffects.None, 1f);

            foreach (Button button in buttons)
            {
                button.Draw();
            }

            var selectorOrigin = new Vector2(selectorTexture.Width / 2, selectorTexture.Height / 2);
            Globals.SpriteBatch.Draw(selectorTexture,
                WindowManager.GetMainWindowCenter() - new Vector2(56, 16) + (new Vector2(0, 16 * selectedButton)),
                null, Color.White, 0f, selectorOrigin, 1f, SpriteEffects.None, 1f);

            var controlsUIOrigin = new Vector2(controlsUI.Width / 2, controlsUI.Height / 2);
            Globals.SpriteBatch.Draw(controlsUI, WindowManager.GetMainWindowCenter(), null, Color.White, 0f, controlsUIOrigin, 1f, SpriteEffects.None, 1f);
        }

        public override void PlayStateSong()
        {
            // play music
        }
    }
}
