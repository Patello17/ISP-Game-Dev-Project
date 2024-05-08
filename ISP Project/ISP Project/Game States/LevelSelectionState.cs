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
using ISP_Project.Components;

namespace ISP_Project.Game_States
{
    public class LevelSelectionState : State
    {
        private List<Button> buttons;
        private Texture2D mapTexture;
        // create variables for the textures and fonts of the buttons (Buttons can share the same texture/font)
        private Texture2D buttonTexture;
        private SpriteFont buttonFont;
        private Texture2D whiteNoteTexture;
        private Texture2D yellowNoteTexture;
        private Texture2D pinTexture;

        public LevelSelectionState(ContentManager content)
        {
            LoadState(content);

            var pauseButton = new PauseButton(buttonTexture, buttonFont, 1, 0.5f)
            {
                Position = new Vector2(buttonTexture.Width / 2, buttonTexture.Height / 2),
                Text = "Pause"
            };

            var hubReturnButton = new HubReturnButton(whiteNoteTexture, buttonFont, 1, 0.5f)
            {
                Sprite = new Sprite(whiteNoteTexture, SpriteEffects.None, 0),
                Position = new Vector2(WindowManager.GetMainWindowCenter().X + 221, WindowManager.GetMainWindowCenter().Y + 80),
                Text = "Back\nTo\nHub"
            };

            var levelOneButton = new LevelOneSelectButton(pinTexture, buttonFont, 1, 0.5f)
            {
                Sprite = new Sprite(pinTexture, SpriteEffects.None, 0),
                Position = new Vector2(WindowManager.GetMainWindowCenter().X - 132, WindowManager.GetMainWindowCenter().Y + 32),
                Text = ""
            };

            buttons = new List<Button>()
            {
                pauseButton, hubReturnButton, levelOneButton
            };

        }
        public override void LoadState(ContentManager content)
        {
            // load everything in this state
            mapTexture = content.Load<Texture2D>("Backgrounds/Map");
            whiteNoteTexture = content.Load<Texture2D>("Interactables/White Note");
            yellowNoteTexture = content.Load<Texture2D>("Interactables/Yellow Note");
            pinTexture = content.Load<Texture2D>("Interactables/Pin");
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

        public override void Draw(GameTime gameTime)
        {
            Globals.SpriteBatch.Draw(mapTexture, WindowManager.GetMainWindowCenter(), null, Color.White, 0f,
                new Vector2(mapTexture.Width / 2, mapTexture.Height / 2), 1f, SpriteEffects.None, 0f);
            foreach (Button button in buttons)
            {
                button.Draw(gameTime);
            }
        }
    }
}
