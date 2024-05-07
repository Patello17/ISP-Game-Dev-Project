using ISP_Project.Game_States;
using ISP_Project.Managers;
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
        private float buttonScale;
        private float fontScale;

        public ResumeButton(Texture2D texture, SpriteFont font, float buttonScale, float fontScale) : base(texture, font, buttonScale, fontScale)
        {
            buttonTexture = texture;
            buttonFont = font;
            this.buttonScale = buttonScale;
            this.fontScale = buttonScale;
        }

        public override void TriggerEvent()
        {
            // change Game State here!
            StateManager.ChangeState(new HubState(Globals.ContentManager));
        }
    }
    // create Settings Button
    public class SettingsButton : Button
    {
        private Texture2D buttonTexture;
        private SpriteFont buttonFont;
        private float buttonScale;
        private float fontScale;

        public SettingsButton(Texture2D texture, SpriteFont font, float buttonScale, float fontScale) : base(texture, font, buttonScale, fontScale)
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
    // create QuitToTitle Button
    public class QuitToTitleButton : Button
    {
        private Texture2D buttonTexture;
        private SpriteFont buttonFont;
        private float buttonScale;
        private float fontScale;

        public QuitToTitleButton(Texture2D texture, SpriteFont font, float buttonScale, float fontScale) : base(texture, font, buttonScale, fontScale)
        {
            buttonTexture = texture;
            buttonFont = font;
            this.buttonScale = buttonScale;
            this.fontScale = buttonScale;
        }

        public override void TriggerEvent()
        {
            Debug.WriteLine("Quitting...");
            StateManager.ChangeState(new TitleState(Globals.ContentManager));
        }
    }
}
