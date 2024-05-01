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

        // private Texture2D buttonTexture; // expandable?: can add multiple button textures here and reference in dictionary
        // private SpriteFont buttonFont;

        /*public Dictionary<Texture2D, string> textures = new Dictionary<Texture2D, string>()
        {
            { buttonTexture, "UI Elements/Button"}
        };
        private Dictionary<SpriteFont, string> fonts = new Dictionary<SpriteFont, string>
        {
            
        };*/

        public SettingsState(ContentManager content)
        {
            LoadState(content);

            var resumeButton = new AudioButton(buttonTexture, buttonFont)
            {
                Position = new Vector2(200, 200),
                Text = "Audio"
            };

            var settingsButton = new ControlsButton(buttonTexture, buttonFont)
            {
                texture = buttonTexture,
                Position = new Vector2(200, 400),
                Text = "Controls"
            };

            var quitButton = new SettingsReturnButton(buttonTexture, buttonFont)
            {
                texture = buttonTexture,
                Position = new Vector2(200, 600),
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
