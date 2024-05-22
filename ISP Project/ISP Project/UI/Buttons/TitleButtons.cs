using ISP_Project.Game_States;
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
    public class TitleButtons
    {
        // create NewGame Button
        public class NewGameButton : Button
        {
            private Texture2D buttonTexture;
            private SpriteFont buttonFont;
            private float buttonScale;
            private float fontScale;

            public NewGameButton(Texture2D texture, SpriteFont font, float buttonScale, float fontScale) : base(texture, font, buttonScale, fontScale)
            {
                buttonTexture = texture;
                buttonFont = font;
                this.buttonScale = buttonScale;
                this.fontScale = buttonScale;
            }

            public override void TriggerEvent()
            {
                // create fresh save file
                SaveManager.NewSave();
                SaveManager.NewReadEnvelopesSave();
                // change Game State here!
                AudioManager.PlaySoundEffect("Door Opening");
                StateManager.ChangeState(new HubState(), Transitions.BlackFade, 2f);
            }
        }

        // create LoadGame Button
        public class LoadGameButton : Button
        {
            private Texture2D buttonTexture;
            private SpriteFont buttonFont;
            private float buttonScale;
            private float fontScale;

            public LoadGameButton(Texture2D texture, SpriteFont font, float buttonScale, float fontScale) : base(texture, font, buttonScale, fontScale)
            {
                buttonTexture = texture;
                buttonFont = font;
                this.buttonScale = buttonScale;
                this.fontScale = buttonScale;
            }

            public override void TriggerEvent()
            {
                // change Game State here!
                AudioManager.PlaySoundEffect("Door Opening");
                StateManager.ChangeState(new HubState(), Transitions.BlackFade, 3f);
            }
        }

        // create Quit Button
        public class QuitButton : Button
        {
            private Texture2D buttonTexture;
            private SpriteFont buttonFont;
            private float buttonScale;
            private float fontScale;

            public QuitButton(Texture2D texture, SpriteFont font, float buttonScale, float fontScale) : base(texture, font, buttonScale, fontScale)
            {
                buttonTexture = texture;
                buttonFont = font;
                this.buttonScale = buttonScale;
                this.fontScale = buttonScale;
            }

            public override void TriggerEvent()
            {
                AudioManager.PlaySoundEffect("Button Press");
                StateManager.ChangeState(new QuitState(), Transitions.BlackFade, 1f);
            }
        }
    }
}