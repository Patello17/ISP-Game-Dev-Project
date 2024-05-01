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

        public AudioButton(Texture2D texture, SpriteFont font) : base(texture, font)
        {
            this.buttonTexture = texture;
            this.buttonFont = font;
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

        public ControlsButton(Texture2D texture, SpriteFont font) : base(texture, font)
        {
            buttonTexture = texture;
            buttonFont = font;
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

        public SettingsReturnButton(Texture2D texture, SpriteFont font) : base(texture, font)
        {
            this.buttonTexture = texture;
            this.buttonFont = font;
        }

        public override void TriggerEvent()
        {
            StateManager.ChangeState(new MenuState(Globals.ContentManager));
        }
    }
}
