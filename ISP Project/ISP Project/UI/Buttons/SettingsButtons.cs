using ISP_Project.Game_States;
using ISP_Project.Managers;
using ISP_Project.States;
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
            Debug.WriteLine("Entering Audio Settings...");
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
            StateManager.ChangeState(new MenuState(Globals.ContentManager));
        }
    }
}
