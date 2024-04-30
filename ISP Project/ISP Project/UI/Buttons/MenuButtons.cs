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

        public ResumeButton(Texture2D texture, SpriteFont font) : base(texture, font)
        {
            this.buttonTexture = texture;
            this.buttonFont = font;
        }

        public override void TriggerEvent()
        {
            Debug.WriteLine("Resuming...");
        }
    }
    // create Settings Button
    public class SettingsButton : Button
    {
        private Texture2D buttonTexture;
        private SpriteFont buttonFont;

        public SettingsButton(Texture2D texture, SpriteFont font) : base(texture, font)
        {
            buttonTexture = texture;
            buttonFont = font;
        }

        public override void TriggerEvent()
        {
            Debug.WriteLine("Entering Settings...");
        }
    }
    // create Quit Button
    public class QuitButton : Button
    {
        private Texture2D buttonTexture;
        private SpriteFont buttonFont;

        public QuitButton(Texture2D texture, SpriteFont font) : base(texture, font)
        {
            this.buttonTexture = texture;
            this.buttonFont = font;
        }

        public override void TriggerEvent()
        {
            Debug.WriteLine("Quitting...");
        }
    }
}
