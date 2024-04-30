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
    public class MenuState : State
    {
        private List<Button> buttons;
        // private Texture2D buttonTexture; // expandable?: can add multiple button textures here and reference in dictionary
        // private SpriteFont buttonFont;

        /*public Dictionary<Texture2D, string> textures = new Dictionary<Texture2D, string>()
        {
            { buttonTexture, "UI Elements/Button"}
        };
        private Dictionary<SpriteFont, string> fonts = new Dictionary<SpriteFont, string>
        {
            
        };*/

        public MenuState(ContentManager content) 
        {
            var buttonTexture = content.Load<Texture2D>("UI Elements/Button");
            var buttonFont = content.Load<SpriteFont>("Fonts/Button Font");

            var resumeButton = new ResumeButton(buttonTexture, buttonFont)
            {
                Position = new Vector2(200, 200),
                Text = "Resume"
            };

            var settingsButton = new SettingsButton(buttonTexture, buttonFont)
            {
                Position = new Vector2(200, 400),
                Text = "Settings"
            };

            var quitButton = new QuitButton(buttonTexture, buttonFont)
            {
                Position = new Vector2(200, 600),
                Text = "Quit"
            };

            buttons = new List<Button>() 
            { 
                resumeButton, settingsButton, quitButton
            };

            Load(content);
        }
        public override void Load(ContentManager content)
        {
            foreach (Button button in buttons)
            {
                button.texture = content.Load<Texture2D>(button.texturePath);
                button.font = content.Load<SpriteFont>(button.fontPath);
            }
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
