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
    // create Resume Button
    public class ResumeButton : Button
    {
        private Texture2D buttonTexture;
        private SpriteFont buttonFont;
        private int buttonScale;
        private int fontScale;

        public ResumeButton(Texture2D texture, SpriteFont font, int buttonScale, int fontScale) : base(texture, font, buttonScale, fontScale)
        {
            buttonTexture = texture;
            buttonFont = font;
            this.buttonScale = buttonScale;
            this.fontScale = buttonScale;
        }

        public override void TriggerEvent()
        {
            // change Game State here!
            Debug.WriteLine("Resuming...");
        }
    }
    // create Settings Button
    public class SettingsButton : Button
    {
        private Texture2D buttonTexture;
        private SpriteFont buttonFont;
        private int buttonScale;
        private int fontScale;

        public SettingsButton(Texture2D texture, SpriteFont font, int buttonScale, int fontScale) : base(texture, font, buttonScale, fontScale)
        {
            buttonTexture = texture;
            buttonFont = font;
            this.buttonScale = buttonScale;
            this.fontScale = buttonScale;
        }

        public override void TriggerEvent()
        {
            StateManager.ChangeState(new SettingsState(Globals.ContentManager));
        }
    }
    // create Quit Button
    public class QuitButton : Button
    {
        private Texture2D buttonTexture;
        private SpriteFont buttonFont;
        private int buttonScale;
        private int fontScale;

        public QuitButton(Texture2D texture, SpriteFont font, int buttonScale, int fontScale) : base(texture, font, buttonScale, fontScale)
        {
            buttonTexture = texture;
            buttonFont = font;
            this.buttonScale = buttonScale;
            this.fontScale = buttonScale;
        }

        public override void TriggerEvent()
        {
            Debug.WriteLine("Quitting...");
        }
    }
}
