using ISP_Project.Game_States;
using ISP_Project.Game_States.Levels;
using ISP_Project.Managers;
using ISP_Project.Screen_Management.Transitions;
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
            AudioManager.PlaySoundEffect("Button Press");
            // change Game State here!
            List<State> states = new List<State>()
            {
                new HubState(), new LevelSelectionState(),
                new LevelOneState(), new LevelTwoState(), new LevelThreeState()
            };
            StateManager.ChangeState(StateManager.GetRecentState(StateManager.GetMostRecentState(states)), Transitions.BlackFade, 0f);
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
            AudioManager.PlaySoundEffect("Button Press");
            StateManager.ChangeState(new SettingsState(), Transitions.BlackFade, 0f);
        }
    }

    // create Quit To Title Button
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
            AudioManager.PlaySoundEffect("Button Press");
            StateManager.ChangeState(new TitleState(), Transitions.BlackFade, 0f);
        }
    }
}
