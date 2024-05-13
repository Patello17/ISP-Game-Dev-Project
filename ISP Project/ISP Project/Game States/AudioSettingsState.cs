using ISP_Project.Components;
using ISP_Project.Managers;
using ISP_Project.UI.Buttons;
using ISP_Project.UI.Sliders;
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
    public class AudioSettingsState : State
    {
        private List<Button> buttons;
        private int selectedButtonCounter = 0;
        // create variables for the textures and fonts of the buttons (Buttons can share the same texture/font)
        private Texture2D buttonTexture;
        private Texture2D sliderBarTexture;
        private Texture2D sliderTexture;
        private SpriteFont buttonFont;
        private HorizontalSlider songSlider;

        public AudioSettingsState(ContentManager content)
        {
            LoadState(content);

            songSlider = new HorizontalSlider(sliderBarTexture, sliderTexture, buttonFont, 1f, 1f, 0.5f)
            {
                BarSprite = new Sprite(sliderBarTexture, SpriteEffects.None, 0f),
                SliderSprite = new Sprite(sliderTexture, SpriteEffects.None, 0f),
                BarPosition = new Vector2(WindowManager.GetMainWindowCenter().X, WindowManager.GetMainWindowCenter().Y)
            };

            /*var audioButton = new AudioButton(buttonTexture, buttonFont, 1, 0.5f)
            {
                Sprite = new Sprite(buttonTexture, SpriteEffects.None, 0),
                Position = new Vector2(WindowManager.GetMainWindowCenter().X, WindowManager.GetMainWindowCenter().Y - 16),
                Text = "Audio"
            };*/

            buttons = new List<Button>()
            {
                songSlider.Slider
            };

        }
        public override void LoadState(ContentManager content)
        {
            // load everything in this state
            buttonTexture = content.Load<Texture2D>("UI Elements/Button");
            buttonFont = content.Load<SpriteFont>("Fonts/Button Font");
            sliderBarTexture = content.Load<Texture2D>("UI Elements/Horizontal Slider Bar");
            sliderTexture = content.Load<Texture2D>("UI Elements/Horizontal Slider");
        }
        public override void Update(GameTime gameTime)
        {
            // keyboard only select
            /*if (InputManager.isKey(InputManager.Inputs.UP, InputManager.isTriggered))
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
            }*/

            songSlider.Update(gameTime);
            foreach (Button button in buttons)
            {
                button.Update(gameTime);
            }
        }

        public override void PostUpdate(GameTime gameTime)
        {
            // unload sprites if they're not needed
        }

        public override void Draw(GameTime gameTime)
        {
            songSlider.Draw(gameTime);
            foreach (Button button in buttons)
            {
                button.Draw(gameTime);
            }
        }
    }
}
