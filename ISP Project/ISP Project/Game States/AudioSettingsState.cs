using ISP_Project.Components;
using ISP_Project.Managers;
using ISP_Project.UI.Buttons;
using ISP_Project.UI.Sliders;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace ISP_Project.Game_States
{
    public class AudioSettingsState : State
    {
        private List<Sprite> buttons;
        private Button audioSettingsReturnButton;
        private int selectedButtonCounter = 0;
        private int selectedButton;
        private Texture2D backgroundTexture;
        private Texture2D buttonTexture;
        private Texture2D sliderBarTexture;
        private Texture2D sliderTexture;
        private SpriteFont buttonFont;
        private Texture2D controlsUI;
        private SongSlider songSlider;
        private SoundEffectsSlider sfxSlider;


        public AudioSettingsState()
        {
            LoadState();

            songSlider = new SongSlider(sliderBarTexture, sliderTexture, buttonFont, 1f, 1f, 
                new Vector2(WindowManager.GetMainWindowCenter().X, WindowManager.GetMainWindowCenter().Y - 32), 1f)
            {
                BarSprite = new Sprite(sliderBarTexture, SpriteEffects.None, 0f),
                SliderSprite = new Sprite(sliderTexture, SpriteEffects.None, 0f)
            };

            sfxSlider = new SoundEffectsSlider(sliderBarTexture, sliderTexture, buttonFont, 1f, 1f,
                new Vector2(WindowManager.GetMainWindowCenter().X, WindowManager.GetMainWindowCenter().Y + 32), 1f)
            {
                BarSprite = new Sprite(sliderBarTexture, SpriteEffects.None, 0f),
                SliderSprite = new Sprite(sliderTexture, SpriteEffects.None, 0f)
            };

            audioSettingsReturnButton = new FineControlsReturnButton(buttonTexture, buttonFont, 1, 0.5f)
            {
                Sprite = new Sprite(buttonTexture, SpriteEffects.None, 0),
                Position = new Vector2(WindowManager.GetMainWindowCenter().X, WindowManager.GetMainWindowCenter().Y + 80),
                Text = "Return"
            };
        }

        public override void LoadState()
        {
            backgroundTexture = Globals.ContentManager.Load<Texture2D>("Backgrounds/Title Background");
            buttonTexture = Globals.ContentManager.Load<Texture2D>("UI Elements/Button");
            buttonFont = Globals.ContentManager.Load<SpriteFont>("Fonts/Button Font");
            sliderBarTexture = Globals.ContentManager.Load<Texture2D>("UI Elements/Horizontal Slider Bar");
            sliderTexture = Globals.ContentManager.Load<Texture2D>("Interactables/Envelope");
            controlsUI = Globals.ContentManager.Load<Texture2D>("UI Elements/Controls Display Audio Setting");

            selectedButtonCounter = 0;
        }

        public override void Update()
        {
            if (InputManager.isKey(InputManager.Inputs.PAUSE, InputManager.isTriggered))
            {
                audioSettingsReturnButton.TriggerEvent();
            }

            // keyboard only select
            if (InputManager.isKey(InputManager.Inputs.UP, InputManager.isTriggered))
            {
                AudioManager.PlaySoundEffect("Scroll");
                selectedButtonCounter++;
            }
            if (InputManager.isKey(InputManager.Inputs.DOWN, InputManager.isTriggered))
            {
                AudioManager.PlaySoundEffect("Scroll");
                selectedButtonCounter--;
            }
            if (selectedButtonCounter >= 0)
                selectedButtonCounter = -3;

            selectedButton = Math.Abs(selectedButtonCounter % 3);

            // Debug.WriteLine("SELECTION: " + selectedButton);
            switch (selectedButton)
            {
                case 1:
                    sfxSlider.Update();
                    songSlider.SliderSprite.Color = Color.White;
                    sfxSlider.SliderSprite.Color = Color.Gray;
                    audioSettingsReturnButton.ForceShade = false;
                    break;
                case 2:
                    audioSettingsReturnButton.Update();
                    songSlider.SliderSprite.Color = Color.White;
                    sfxSlider.SliderSprite.Color = Color.White;
                    audioSettingsReturnButton.ForceShade = true;
                    if (InputManager.isKey(InputManager.Inputs.INTERACT, InputManager.isTriggered))
                    {
                        audioSettingsReturnButton.TriggerEvent();
                    }
                    break;
                default:
                    songSlider.Update();
                    songSlider.SliderSprite.Color = Color.Gray;
                    sfxSlider.SliderSprite.Color = Color.White;
                    audioSettingsReturnButton.ForceShade = false;
                    break;
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

            var text = "AUDIO";
            var x = WindowManager.GetMainWindowCenter().X - (buttonFont.MeasureString(text).X * 1f / 2);
            var y = WindowManager.GetMainWindowCenter().Y - 80 - (buttonFont.MeasureString(text).Y * 1f / 2);
            Globals.SpriteBatch.DrawString(buttonFont, text, new Vector2(x, y),
                Color.White, 0f, Vector2.Zero, 1f, SpriteEffects.None, 1f);

            var songText = "SONG VOLUME";
            var songTextX = WindowManager.GetMainWindowCenter().X - (buttonFont.MeasureString(songText).X * 0.5f / 2);
            var songTextY = WindowManager.GetMainWindowCenter().Y - 48 - (buttonFont.MeasureString(songText).Y * 0.5f / 2);
            Globals.SpriteBatch.DrawString(buttonFont, songText, new Vector2(songTextX, songTextY),
                Color.White, 0f, Vector2.Zero, 0.5f, SpriteEffects.None, 1f);

            var songVolumeText = Math.Round(AudioManager.GetMaximumSongVolume() * 100) == -0 ? "OFF" : Math.Round(AudioManager.GetMaximumSongVolume() * 100) + "%";
            var songVolumeTextX = WindowManager.GetMainWindowCenter().X - (buttonFont.MeasureString(songVolumeText).X * 0.5f / 2);
            var songVolumeTextY = WindowManager.GetMainWindowCenter().Y - 16 - (buttonFont.MeasureString(songVolumeText).Y * 0.5f / 2);
            Globals.SpriteBatch.DrawString(buttonFont, songVolumeText, new Vector2(songVolumeTextX, songVolumeTextY),
                Color.White, 0f, Vector2.Zero, 0.5f, SpriteEffects.None, 1f);

            var sfxText = "SOUND EFFECTS VOLUME";
            var sfxTextX = WindowManager.GetMainWindowCenter().X - (buttonFont.MeasureString(sfxText).X * 0.5f / 2);
            var sfxTextY = WindowManager.GetMainWindowCenter().Y + 16 - (buttonFont.MeasureString(sfxText).Y * 0.5f / 2);
            Globals.SpriteBatch.DrawString(buttonFont, sfxText, new Vector2(sfxTextX, sfxTextY),
                Color.White, 0f, Vector2.Zero, 0.5f, SpriteEffects.None, 1f);

            var sfxVolumeText = Math.Round(AudioManager.GetMaximumSFXVolume() * 100) == -0 ? "OFF" : Math.Round(AudioManager.GetMaximumSFXVolume() * 100) + "%";
            var sfxVolumeTextX = WindowManager.GetMainWindowCenter().X - (buttonFont.MeasureString(sfxVolumeText).X * 0.5f / 2);
            var sfxVolumeTextY = WindowManager.GetMainWindowCenter().Y + 48 - (buttonFont.MeasureString(sfxVolumeText).Y * 0.5f / 2);
            Globals.SpriteBatch.DrawString(buttonFont, sfxVolumeText, new Vector2(sfxVolumeTextX, sfxVolumeTextY),
                Color.White, 0f, Vector2.Zero, 0.5f, SpriteEffects.None, 1f);

            songSlider.Draw();
            sfxSlider.Draw();
            audioSettingsReturnButton.Draw();

            if (selectedButton == 2)
            {
                var selectorOrigin = new Vector2(sliderTexture.Width / 2, sliderTexture.Height / 2);
                Globals.SpriteBatch.Draw(sliderTexture,
                    WindowManager.GetMainWindowCenter() + new Vector2(-56, 80),
                    null, Color.White, 0f, selectorOrigin, 1f, SpriteEffects.None, 1f);
            }

            var controlsUIOrigin = new Vector2(controlsUI.Width / 2, controlsUI.Height / 2);
            Globals.SpriteBatch.Draw(controlsUI, WindowManager.GetMainWindowCenter(), null, Color.White, 0f, controlsUIOrigin, 1f, SpriteEffects.None, 1f);
        }

        public override void PlayStateSong()
        {
            // play music
        }
    }
}
