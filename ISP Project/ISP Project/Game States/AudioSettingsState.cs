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
        private Texture2D buttonTexture;
        private Texture2D sliderBarTexture;
        private Texture2D sliderTexture;
        private SpriteFont buttonFont;
        private HorizontalSlider songSlider;

        public AudioSettingsState()
        {
            LoadState();

            songSlider = new HorizontalSlider(sliderBarTexture, sliderTexture, buttonFont, 1f, 1f, 0.5f)
            {
                BarSprite = new Sprite(sliderBarTexture, SpriteEffects.None, 0f),
                SliderSprite = new Sprite(sliderTexture, SpriteEffects.None, 0f),
                BarPosition = new Vector2(WindowManager.GetMainWindowCenter().X, WindowManager.GetMainWindowCenter().Y)
            };

            buttons = new List<Button>()
            {
                songSlider.Slider
            };

        }

        public override void LoadState()
        {
            buttonTexture = Globals.ContentManager.Load<Texture2D>("UI Elements/Button");
            buttonFont = Globals.ContentManager.Load<SpriteFont>("Fonts/Button Font");
            sliderBarTexture = Globals.ContentManager.Load<Texture2D>("UI Elements/Horizontal Slider Bar");
            sliderTexture = Globals.ContentManager.Load<Texture2D>("UI Elements/Horizontal Slider");
        }

        public override void Update()
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

            songSlider.Update();

            foreach (Button button in buttons)
            {
                button.Update();
            }
        }

        public override void PostUpdate()
        {
            // unload sprites if they're not needed
        }

        public override void Draw()
        {
            songSlider.Draw();

            foreach (Button button in buttons)
            {
                button.Draw();
            }
        }

        public override void PlayStateSong()
        {
            // play music
        }
    }
}
