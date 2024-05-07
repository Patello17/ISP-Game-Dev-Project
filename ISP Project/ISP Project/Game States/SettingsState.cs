using ISP_Project.Components;
using ISP_Project.UI.Buttons;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ISP_Project.Game_States
{
    public class SettingsState : State
    {
        private List<Button> buttons;
        // create variables for the textures and fonts of the buttons (Buttons can share the same texture/font)
        private Texture2D buttonTexture;
        private SpriteFont buttonFont;

        public SettingsState(ContentManager content)
        {
            LoadState(content);

            var resumeButton = new AudioButton(buttonTexture, buttonFont, 1, 0.5f)
            {
                Sprite = new Sprite(buttonTexture, SpriteEffects.None, 0),
                Position = new Vector2(80, 16),
                Text = "Audio"
            };

            var settingsButton = new ControlsButton(buttonTexture, buttonFont, 1, 0.5f)
            {
                Sprite = new Sprite(buttonTexture, SpriteEffects.None, 0),
                Position = new Vector2(80, 32),
                Text = "Controls"
            };

            var quitButton = new SettingsReturnButton(buttonTexture, buttonFont, 1, 0.5f)
            {
                Sprite = new Sprite(buttonTexture, SpriteEffects.None, 0),
                Position = new Vector2(80, 48),
                Text = "Return"
            };

            buttons = new List<Button>()
            {
                resumeButton, settingsButton, quitButton
            };

        }
        public override void LoadState(ContentManager content)
        {
            // load everything in this state
            buttonTexture = content.Load<Texture2D>("UI Elements/Button");
            buttonFont = content.Load<SpriteFont>("Fonts/Button Font");
        }
        public override void Update(GameTime gameTime)
        {
            foreach (Button button in buttons)
            {
                button.Update(gameTime);
            }
        }

        public override void PostUpdate(GameTime gameTime)
        {
            // unload sprites if they're not needed
        }

        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            foreach (Button button in buttons)
            {
                button.Draw(gameTime, spriteBatch);
            }
        }
    }
}
