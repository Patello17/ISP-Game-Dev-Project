using ISP_Project.Game_States;
using ISP_Project.Game_States.Levels;
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
            List<State> states = new List<State>()
            {
                new HubState(), new LevelSelectionState(),
                new LevelOneState()
            };
            StateManager.ChangeState(StateManager.GetRecentState(StateManager.GetMostRecentState(states)));
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
            StateManager.ChangeState(new SettingsState());
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
            StateManager.ChangeState(new TitleState());
        }
    }
}
