using ISP_Project.UI.Buttons;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ISP_Project.Managers;
using ISP_Project.Components;
using ISP_Project.Screen_Management.Transitions;
using Microsoft.Xna.Framework.Input;
using System.Diagnostics;
using System.IO;

namespace ISP_Project.Game_States
{
    public class LevelSelectionState : State
    {
        private Button[,] buttons = new Button[3, 4];
        // private List<Button> mapButtons;
        private int selectedButtonCounterY = 0;
        private int selectedButtonCounterX = 0;
        private int selectedButtonX;
        private int selectedButtonY;
        private Texture2D mapTexture;
        private Texture2D buttonTexture;
        private SpriteFont buttonFont;
        private Texture2D whiteNoteTexture;
        private Texture2D yellowNoteTexture;
        private Texture2D selectionTexture;
        private Texture2D pinTexture;
        private Texture2D controlsUI;

        public LevelSelectionState()
        {
            LoadState();

            var pauseButton = new PauseButton(buttonTexture, buttonFont, 1, 0.5f)
            {
                Position = new Vector2(buttonTexture.Width / 2, buttonTexture.Height / 2),
                Text = "Pause"
            };

            var hubReturnButton = new HubReturnButton(whiteNoteTexture, buttonFont, 1, 0.5f)
            {
                Sprite = new Sprite(whiteNoteTexture, SpriteEffects.None, 0),
                Position = new Vector2(WindowManager.GetMainWindowCenter().X + 226, WindowManager.GetMainWindowCenter().Y + 75),
                Text = "Back\nTo\nHub"
            };

            var levelOnePin = new LevelOneSelectButton(pinTexture, buttonFont, 1, 0.5f)
            {
                Sprite = new Sprite(pinTexture, SpriteEffects.None, 0),
                Position = new Vector2(WindowManager.GetMainWindowCenter().X - 131, WindowManager.GetMainWindowCenter().Y + 32),
                Text = ""
            };
            var levelOneButton = new LevelOneSelectButton(pinTexture, buttonFont, 1, 0.5f)
            {
                Sprite = new Sprite(yellowNoteTexture, SpriteEffects.None, 0),
                Position = new Vector2(WindowManager.GetMainWindowCenter().X + 114, WindowManager.GetMainWindowCenter().Y - 81),
                Text = "LVL.\n 1"
            };

            var levelTwoPin = new LevelTwoSelectButton(pinTexture, buttonFont, 1, 0.5f)
            {
                Sprite = new Sprite(pinTexture, SpriteEffects.None, 0),
                Position = new Vector2(WindowManager.GetMainWindowCenter().X - 170, WindowManager.GetMainWindowCenter().Y - 32),
                Text = ""
            };
            var levelTwoButton = new LevelTwoSelectButton(pinTexture, buttonFont, 1, 0.5f)
            {
                Sprite = new Sprite(yellowNoteTexture, SpriteEffects.None, 0),
                Position = new Vector2(WindowManager.GetMainWindowCenter().X + 170, WindowManager.GetMainWindowCenter().Y - 81),
                Text = "LVL.\n 2"
            };

            buttons[2, 3] = hubReturnButton;

            switch (SaveManager.Load().LevelsCompleted)
            {
                case 1:
                    LevelOneSelectButton.isClickable = true;
                    LevelTwoSelectButton.isClickable = true;
                    buttons[0, 0] = levelOneButton;
                    buttons[1, 0] = levelTwoButton;
                    // third level unlocked
                    break;
                case 2:
                    LevelOneSelectButton.isClickable = true;
                    LevelTwoSelectButton.isClickable = true;
                    buttons[0, 0] = levelOneButton;
                    buttons[1, 0] = levelTwoButton;
                    break;
                default:
                    LevelOneSelectButton.isClickable = true;
                    buttons[0, 0] = levelOneButton;
                    break;
            }

            /*LevelOneSelectButton.isClickable = true;
            LevelTwoSelectButton.isClickable = true;*/

            /*buttons = new List<Button>()
            {
                hubReturnButton, levelOneButton, levelTwoButton
            };*/
            /*mapButtons = new List<Button>()
            {
                hubReturnButton, levelOneButton, levelTwoButton
            };*/
        }

        public override void LoadState()
        {
            mapTexture = Globals.ContentManager.Load<Texture2D>("Backgrounds/Map");
            whiteNoteTexture = Globals.ContentManager.Load<Texture2D>("Interactables/White Note");
            yellowNoteTexture = Globals.ContentManager.Load<Texture2D>("Interactables/Yellow Note");
            selectionTexture = Globals.ContentManager.Load<Texture2D>("Interactables/Hover Note");
            pinTexture = Globals.ContentManager.Load<Texture2D>("Interactables/Pin");
            buttonTexture = Globals.ContentManager.Load<Texture2D>("UI Elements/Button");
            buttonFont = Globals.ContentManager.Load<SpriteFont>("Fonts/Button Font");
            controlsUI = Globals.ContentManager.Load<Texture2D>("UI Elements/Controls Display Map");

            selectedButtonCounterX = -2;
            selectedButtonCounterY = -3;
        }

        public override void Update()
        {
            // pause
            if (InputManager.isKey(InputManager.Inputs.PAUSE, InputManager.isTriggered))
            {
                AudioManager.PlaySoundEffect("Button Press");
                StateManager.ChangeState(new PauseState(), Transitions.BlackFade, 0.1f);
            }

            // keyboard only select
            if (InputManager.isKey(InputManager.Inputs.LEFT, InputManager.isTriggered))
            {
                selectedButtonCounterX++;
                AudioManager.PlaySoundEffect("Scroll");
            }  
            if (InputManager.isKey(InputManager.Inputs.RIGHT, InputManager.isTriggered))
            {
                selectedButtonCounterX--;
                AudioManager.PlaySoundEffect("Scroll");
            }
            if (InputManager.isKey(InputManager.Inputs.UP, InputManager.isTriggered))
            {
                selectedButtonCounterY++;
                AudioManager.PlaySoundEffect("Scroll");
            }
            if (InputManager.isKey(InputManager.Inputs.DOWN, InputManager.isTriggered))
            {
                selectedButtonCounterY--;
                AudioManager.PlaySoundEffect("Scroll");
            }

            /*var lengthX = buttons ? buttons.GetLength(1) = 0;
            var lengthY = buttons[0, 1]*/
            if (selectedButtonCounterX >= 0)
                selectedButtonCounterX = -buttons.GetLength(0);
            if (selectedButtonCounterY >= 0)
                selectedButtonCounterY = -buttons.GetLength(1);

            selectedButtonY = Math.Abs(selectedButtonCounterY % buttons.GetLength(1));
            selectedButtonX = Math.Abs(selectedButtonCounterX % buttons.GetLength(0));

            // Debug.WriteLine(selectedButtonCounterX + ", " + selectedButtonCounterY);
            foreach (Button button in buttons)
            {
                if (button != null)
                {
                    button.Update();
                    button.ForceShade = false;
                    button.Sprite.Texture = yellowNoteTexture;
                }
            }

            // Debug.WriteLine(selectedButtonCounterX + ", " + selectedButtonCounterY
            if (buttons[selectedButtonX, selectedButtonY] != null)
            {
                buttons[selectedButtonX, selectedButtonY].Sprite.Texture = whiteNoteTexture;

                if (InputManager.isKey(InputManager.Inputs.INTERACT, InputManager.isTriggered))
                {
                    buttons[selectedButtonX, selectedButtonY].TriggerEvent();
                }
            }
            

            // update buttons
            /*foreach(Button button in buttons)
            {
                if (button != null)
                button.Update();
            }*/
        }

        public override void PostUpdate()
        {
            // unload sprites if they're not needed
        }

        public override void Draw()
        {
            Globals.SpriteBatch.Draw(mapTexture, WindowManager.GetMainWindowCenter(), null, Color.White, 0f,
                new Vector2(mapTexture.Width / 2, mapTexture.Height / 2), 1f, SpriteEffects.None, 0f);
            
            foreach (Button button in buttons)
            {
                if (button != null)
                    button.Draw();
            }

            var selectionOrigin = new Vector2(selectionTexture.Width / 2, selectionTexture.Height / 2);
            Globals.SpriteBatch.Draw(selectionTexture, WindowManager.GetMainWindowCenter() +
                new Vector2(114, -81) + new Vector2(56 * selectedButtonX, 52 * selectedButtonY), null, Color.White,
                0f, selectionOrigin, 1f, SpriteEffects.None, 1f);

            var controlsUIOrigin = new Vector2(controlsUI.Width / 2, controlsUI.Height / 2);
            Globals.SpriteBatch.Draw(controlsUI, WindowManager.GetMainWindowCenter(), null, Color.White, 0f, controlsUIOrigin, 1f, SpriteEffects.None, 1f);
        }

        public override void PlayStateSong()
        {
            // play music
        }
    }
}
