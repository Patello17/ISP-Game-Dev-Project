using ISP_Project.Game_States;
using ISP_Project.Game_States.Levels;
using ISP_Project.Managers;
using ISP_Project.Screen_Management.Transitions;
using ISP_Project.UI.Sliders;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ISP_Project.UI.Buttons
{
    // create Audio Button
    public class AudioButton : Button
    {
        private Texture2D buttonTexture;
        private SpriteFont buttonFont;
        private float buttonScale;
        private float fontScale;

        public AudioButton(Texture2D texture, SpriteFont font, float buttonScale, float fontScale) : base(texture, font, buttonScale, fontScale)
        {
            buttonTexture = texture;
            buttonFont = font;
            this.buttonScale = buttonScale;
            this.fontScale = buttonScale;
        }

        public override void TriggerEvent()
        {
            // change Game State here!
            AudioManager.PlaySoundEffect("Button Press");
            if (StateManager.GetRecentState(new AudioSettingsState()) == null)
                StateManager.ChangeState(new AudioSettingsState(), Transitions.BlackFade, 0.1f);
            else
                StateManager.ChangeState(StateManager.GetRecentState(new AudioSettingsState()), Transitions.BlackFade, 0.1f);
        }
    }

    // create Controls Button
    public class ControlsButton : Button
    {
        private Texture2D buttonTexture;
        private SpriteFont buttonFont;
        private float buttonScale;
        private float fontScale;

        public ControlsButton(Texture2D texture, SpriteFont font, float buttonScale, float fontScale) : base(texture, font, buttonScale, fontScale)
        {
            buttonTexture = texture;
            buttonFont = font;
            this.buttonScale = buttonScale;
            this.fontScale = buttonScale;
        }

        public override void TriggerEvent()
        {
            AudioManager.PlaySoundEffect("Button Press");
            Debug.WriteLine("Entering Controls Settings...");
        }
    }

    // create Settings Return Button
    public class SettingsReturnButton : Button
    {
        private Texture2D buttonTexture;
        private SpriteFont buttonFont;
        private float buttonScale;
        private float fontScale;

        public SettingsReturnButton(Texture2D texture, SpriteFont font, float buttonScale, float fontScale) : base(texture, font, buttonScale, fontScale)
        {
            buttonTexture = texture;
            buttonFont = font;
            this.buttonScale = buttonScale;
            this.fontScale = buttonScale;
        }

        public override void TriggerEvent()
        {
            AudioManager.PlaySoundEffect("Button Press");
            List<State> states = new List<State>()
            {
                new PauseState(), new TitleState()
            };
            StateManager.ChangeState(StateManager.GetRecentState(StateManager.GetMostRecentState(states)), Transitions.BlackFade, 0.1f);
        }
    }

    public class FineControlsReturnButton : Button
    {
        private Texture2D buttonTexture;
        private SpriteFont buttonFont;
        private float buttonScale;
        private float fontScale;

        public FineControlsReturnButton(Texture2D texture, SpriteFont font, float buttonScale, float fontScale) : base(texture, font, buttonScale, fontScale)
        {
            buttonTexture = texture;
            buttonFont = font;
            this.buttonScale = buttonScale;
            this.fontScale = buttonScale;
        }

        public override void TriggerEvent()
        {
            AudioManager.PlaySoundEffect("Button Press");
            StateManager.ChangeState(new SettingsState(), Transitions.BlackFade, 0.1f);
        }
    }

    // create Song Slider Button
    public class SongSlider : HorizontalSlider
    {
        private Texture2D barTexture;
        private Texture2D sliderTexture;
        private SpriteFont buttonFont;
        private float buttonScale;
        private float fontScale;
        private Vector2 position;
        private float value;

        public SongSlider(
            Texture2D barTexture,
            Texture2D sliderTexture,
            SpriteFont font,
            float buttonScale,
            float fontScale,
            Vector2 position,
            float value) :
            base(barTexture, sliderTexture, font, buttonScale, fontScale, position, value)
        {
            this.barTexture = barTexture;
            this.sliderTexture = sliderTexture;
            buttonFont = font;
            this.buttonScale = buttonScale;
            this.fontScale = buttonScale;
            this.position = position;
            this.value = value;
        }

        public override void SetVolume()
        {
            // change Game State here!
            AudioManager.SetSongVolume(Value);
        }
    }

    // create Sound Effects Slider Button
    public class SoundEffectsSlider : HorizontalSlider
    {
        private Texture2D barTexture;
        private Texture2D sliderTexture;
        private SpriteFont buttonFont;
        private float buttonScale;
        private float fontScale;
        private Vector2 position;
        private float value;

        public SoundEffectsSlider(
            Texture2D barTexture, 
            Texture2D sliderTexture, 
            SpriteFont font, 
            float buttonScale, 
            float fontScale, 
            Vector2 position,
            float value) : 
            base(barTexture, sliderTexture, font, buttonScale, fontScale, position, value)
        {
            this.barTexture = barTexture;
            this.sliderTexture = sliderTexture;
            buttonFont = font;
            this.buttonScale = buttonScale;
            this.fontScale = buttonScale;
            this.position = position;
            this.value = value;
        }

        public override void SetVolume()
        {
            // change Game State here!
            AudioManager.SetSoundEffectVolume(Value);
        }
    }
}
