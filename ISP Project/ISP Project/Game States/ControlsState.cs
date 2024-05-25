using ISP_Project.Tilemaps;
using ISP_Project.UI.Buttons;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ISP_Project.Managers;
using ISP_Project.Screen_Management.Transitions;
using ISP_Project.Components;

namespace ISP_Project.Game_States
{
    public class ControlsState : State
    {
        private Texture2D backgroundTexture;
        private Texture2D controlsTexture;
        private Texture2D selectorTexture;
        private SpriteFont buttonFont;
        private Button controlsSettingsReturnButton;

        public ControlsState()
        {
            LoadState();
        }

        public override void LoadState()
        {
            backgroundTexture = Globals.ContentManager.Load<Texture2D>("Backgrounds/Title Background");
            controlsTexture = Globals.ContentManager.Load<Texture2D>("UI Elements/Controls Panel");
            buttonFont = Globals.ContentManager.Load<SpriteFont>("Fonts/Button Font");
        }

        public override void Update()
        {
            if (InputManager.isKey(InputManager.Inputs.PAUSE, InputManager.isTriggered))
            {
                StateManager.ChangeState(new SettingsState(), Transitions.BlackFade, 0f);
            }
        }

        public override void PostUpdate() { }

        public override void Draw()
        {
            var backgroundOrigin = new Vector2(backgroundTexture.Width / 2, backgroundTexture.Height / 2);
            Globals.SpriteBatch.Draw(backgroundTexture, WindowManager.GetMainWindowCenter(), null, Color.White, 0f, backgroundOrigin, 1f, SpriteEffects.None, 0f);

            var text = "CONTROLS";
            var x = WindowManager.GetMainWindowCenter().X - (buttonFont.MeasureString(text).X * 1f / 2);
            var y = WindowManager.GetMainWindowCenter().Y - 100 - (buttonFont.MeasureString(text).Y * 1f / 2);
            Globals.SpriteBatch.DrawString(buttonFont, text, new Vector2(x, y),
                Color.White, 0f, Vector2.Zero, 1f, SpriteEffects.None, 1f);

            var controlsPanelOrigin = new Vector2(controlsTexture.Width / 2, controlsTexture.Height / 2);
            Globals.SpriteBatch.Draw(controlsTexture, WindowManager.GetMainWindowCenter(), null, Color.White, 0f, controlsPanelOrigin, 1f, SpriteEffects.None, 1f);
        }

        public override void PlayStateSong() { }
    }
}
