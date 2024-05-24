using ISP_Project.Components;
using ISP_Project.Managers;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ISP_Project.Game_States.Letters
{
    public class LetterFour : State
    {
        private Sprite letter;
        private SpriteFont letterFont;
        private float letterFontSize = 0.5f;
        private Texture2D portraitTexture;
        private Texture2D controlsUI;
        public bool Opened { get; set; } = false;
        private const string letterText =
            "Dear Player,\n" +
            "Thank you for playing\n" +
            "our game, Snail Mail.\n" +
            "We hope you enjoyed\n" +
            "our ISP as much as we\n" +
            "enjoyed making it.\n\n" +
            "From,\n" +
            "Jacob and Prayag <3\n\n" +
            "Music and Sound Effects - Jacob\n" +
            "Programming and Puzzle Design - Prayag\n" +
            "Game Design, Writing, Artwork - BOTH";

        public LetterFour()
        {
            letter = new Sprite(null, SpriteEffects.None, 0.2f);

            LoadState();
        }

        public override void LoadState()
        {
            letter.Texture = Globals.ContentManager.Load<Texture2D>("Interactables/Blank Letter");
            letter.Color = Color.White;
            letterFont = Globals.ContentManager.Load<SpriteFont>("Fonts/Button Font");
            portraitTexture = Globals.ContentManager.Load<Texture2D>("Portraits/Developer Portrait");
            controlsUI = Globals.ContentManager.Load<Texture2D>("UI Elements/Controls Display Envelope");
        }

        public override void Update()
        {
            if (InputManager.isKey(InputManager.Inputs.PAUSE, InputManager.isTriggered))
            {
                StateManager.ChangeState(StateManager.GetRecentState(new HubState()), Screen_Management.Transitions.Transitions.BlackFade, 0.1f);
            }
        }

        public override void PostUpdate()
        {

        }

        public override void Draw()
        {
            Globals.SpriteBatch.Draw(letter.Texture, WindowManager.GetMainWindowCenter(), null, letter.Color, 0f,
                letter.GetSpriteOrigin(), 1f, letter.SpriteEffects, letter.DrawLayer);
            var textX = WindowManager.GetMainWindowCenter().X - (letterFont.MeasureString(letterText).X * letterFontSize / 2);
            var textY = WindowManager.GetMainWindowCenter().Y - (letterFont.MeasureString(letterText).Y * letterFontSize / 2);
            Globals.SpriteBatch.DrawString(letterFont, letterText, new Vector2(textX, textY),
                Color.Black, 0f, Vector2.Zero, letterFontSize, SpriteEffects.None, 0.8f);

            var portraitOrigin = new Vector2(portraitTexture.Width / 2, portraitTexture.Height / 2);
            Globals.SpriteBatch.Draw(portraitTexture, WindowManager.GetMainWindowCenter() + new Vector2(105, -30), null, Color.White, 0f, portraitOrigin, 1f, SpriteEffects.None, 1f);

            var controlsUIOrigin = new Vector2(controlsUI.Width / 2, controlsUI.Height / 2);
            Globals.SpriteBatch.Draw(controlsUI, WindowManager.GetMainWindowCenter(), null, Color.White, 0f, controlsUIOrigin, 1f, SpriteEffects.None, 1f);
        }

        public override void PlayStateSong()
        {

        }
    }
}
