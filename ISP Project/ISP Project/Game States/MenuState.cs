using ISP_Project.Components;
using ISP_Project.Managers;
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
        // create variables for the textures and fonts of the buttons (Buttons can share the same texture/font)
        private Texture2D buttonTexture;
        private SpriteFont buttonFont;

        public MenuState(ContentManager content) 
        {
            LoadState(content);

            Debug.WriteLine(WindowManager.GetMainWindowCenter());
            var resumeButton = new ResumeButton(buttonTexture, buttonFont, 1, 0.5f)
            {
                Sprite = new Sprite(buttonTexture, SpriteEffects.None, 0),
                Position = new Vector2(WindowManager.GetMainWindowCenter().X, WindowManager.GetMainWindowCenter().Y - 16),
                Text = "Resume"
            };

            var settingsButton = new SettingsButton(buttonTexture, buttonFont, 1, 0.5f)
            {
                Sprite = new Sprite(buttonTexture, SpriteEffects.None, 0),
                Position = new Vector2(WindowManager.GetMainWindowCenter().X, WindowManager.GetMainWindowCenter().Y),
                Text = "Settings"
            };

            var quitButton = new QuitToTitleButton(buttonTexture, buttonFont, 1, 0.5f)
            {
                Sprite = new Sprite(buttonTexture, SpriteEffects.None, 0),
                Position = new Vector2(WindowManager.GetMainWindowCenter().X, WindowManager.GetMainWindowCenter().Y + 16),
                Text = "To Title"
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
