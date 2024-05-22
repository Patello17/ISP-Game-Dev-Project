using ISP_Project.UI.Buttons;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static ISP_Project.UI.Buttons.TitleButtons;
using ISP_Project.Components;
using ISP_Project.Managers;
using System.Diagnostics;

namespace ISP_Project.Game_States
{
    public class TitleState : State
    {
        private List<Button> buttons;
        private int selectedButtonCounter = 0;
        private int selectedButton;
        private Texture2D selectorTexture;
        private Texture2D buttonTexture;
        private Texture2D backgroundTexture;
        private Texture2D titleTexture;
        private SpriteFont buttonFont;
        private Texture2D controlsUI;

        public TitleState()
        {
            LoadState();

            var newGameButton = new NewGameButton(buttonTexture, buttonFont, 1, 0.5f)
            {
                Sprite = new Sprite(buttonTexture, SpriteEffects.None, 0),
                Position = new Vector2(WindowManager.GetMainWindowCenter().X, WindowManager.GetMainWindowCenter().Y - 32),
                Text = "New Game"
            };

            var loadGameButton = new LoadGameButton(buttonTexture, buttonFont, 1, 0.5f)
            {
                Sprite = new Sprite(buttonTexture, SpriteEffects.None, 0),
                Position = new Vector2(WindowManager.GetMainWindowCenter().X, WindowManager.GetMainWindowCenter().Y - 16),
                Text = "Load Game"
            };

            var settingsButton = new SettingsButton(buttonTexture, buttonFont, 1, 0.5f)
            {
                Sprite = new Sprite(buttonTexture, SpriteEffects.None, 0),
                Position = new Vector2(WindowManager.GetMainWindowCenter().X, WindowManager.GetMainWindowCenter().Y),
                Text = "Settings"
            };

            var quitButton = new QuitButton(buttonTexture, buttonFont, 1, 0.5f)
            {
                Sprite = new Sprite(buttonTexture, SpriteEffects.None, 0),
                Position = new Vector2(WindowManager.GetMainWindowCenter().X, WindowManager.GetMainWindowCenter().Y + 16),
                Text = "Quit"
            };

            buttons = new List<Button>()
            {
                newGameButton, loadGameButton, settingsButton, quitButton
            };
        }

        public override void LoadState()
        {
            selectorTexture = Globals.ContentManager.Load<Texture2D>("Interactables/Envelope");
            buttonTexture = Globals.ContentManager.Load<Texture2D>("UI Elements/Button");
            backgroundTexture = Globals.ContentManager.Load<Texture2D>("Backgrounds/Title Background");
            titleTexture = Globals.ContentManager.Load<Texture2D>("Backgrounds/Title");
            buttonFont = Globals.ContentManager.Load<SpriteFont>("Fonts/Button Font");
            controlsUI = Globals.ContentManager.Load<Texture2D>("UI Elements/Controls Display Title");

            selectedButtonCounter = 0;
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
            var titleOrigin = new Vector2(titleTexture.Width / 2, titleTexture.Height / 2);
            Globals.SpriteBatch.Draw(titleTexture, WindowManager.GetMainWindowCenter() - new Vector2(0, 80), null, Color.White, 0f, titleOrigin, 1f, SpriteEffects.None, 0f);

            foreach (Button button in buttons)
            {
                button.Draw();
            }

            var selectorOrigin = new Vector2(selectorTexture.Width / 2, selectorTexture.Height / 2);
            Globals.SpriteBatch.Draw(selectorTexture, 
                WindowManager.GetMainWindowCenter() - new Vector2(56, 32) + (new Vector2(0, 16 * selectedButton)), 
                null, Color.White, 0f, selectorOrigin, 1f, SpriteEffects.None, 1f);

            var controlsUIOrigin = new Vector2(controlsUI.Width / 2, controlsUI.Height / 2);
            Globals.SpriteBatch.Draw(controlsUI, WindowManager.GetMainWindowCenter(), null, Color.White, 0f, controlsUIOrigin, 1f, SpriteEffects.None, 1f);
        }

        public override void PlayStateSong()
        {
            // play music
            AudioManager.ForcePlaySong("Title Theme");
        }
    }
}
