using ISP_Project.Components;
using ISP_Project.Game_States.Levels;
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
    public class PauseState : State
    {
        private List<Button> buttons;
        private int selectedButtonCounter = 0;
        private Texture2D buttonTexture;
        private SpriteFont buttonFont;

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

            buttons = new List<Button>()
            {
                resumeButton, settingsButton, quitToTitleButton
            };
        }

        public override void LoadState()
        {
            buttonTexture = Globals.ContentManager.Load<Texture2D>("UI Elements/Button");
            buttonFont = Globals.ContentManager.Load<SpriteFont>("Fonts/Button Font");
        }

        public override void Update()
        {
            // unpause
            if (InputManager.isKey(InputManager.Inputs.PAUSE, InputManager.isTriggered))
            {
                resumeButton.TriggerEvent();
            }

            // change exit button between hub and title
            if (StateManager.GetPreviousState() is HubState)
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
            foreach (Button button in buttons)
            {
                button.Draw();
            }
        }
    }
}
