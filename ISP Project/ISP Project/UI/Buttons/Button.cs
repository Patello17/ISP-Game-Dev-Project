using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ISP_Project.Managers;
using System.Diagnostics;

namespace ISP_Project.UI.Buttons
{
    // Code Reference: https://github.com/Oyyou/MonoGame_Tutorials/blob/master/MonoGame_Tutorials/Tutorial013/Controls/Button.cs
    public abstract class Button
    {
        public Texture2D texture;
        public string texturePath;
        public SpriteFont font;
        public string fontPath; // every button will have this font
        public string Text { get; set; }
        public Color TextColor { get; set; }
        public Vector2 Position { get; set; }
        public int ButtonScale { get; set; }
        public int FontScale { get; set; }
        public Rectangle Rectangle
        {
            get
            {
                return new Rectangle((int)Position.X, (int)Position.Y, texture.Width * ButtonScale, texture.Height * ButtonScale);
            }
        }
        private bool isHovering;

        public Button(Texture2D texture, SpriteFont font)
        {
            this.texture = texture;
            this.font = font;

            TextColor = Color.Black;
            ButtonScale = 6;
            FontScale = 3;
        }

        public void Update(GameTime gameTime)
        {
            if (InputManager.cursorRect.Intersects(Rectangle))
            {
                isHovering = true;

                if (InputManager.isClick(InputManager.ClickInputs.INTERACT, InputManager.isTriggered) ||
                    InputManager.isKey(InputManager.Inputs.INTERACT, InputManager.isTriggered))
                {
                    TriggerEvent();
                }
            }
            else { isHovering = false; }
        }

        public abstract void TriggerEvent();

        public void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {

            // draw button and change color on cursor hover
            var color = Color.White;

            if (isHovering)
                color = Color.Gray;

            spriteBatch.Draw(texture, Rectangle, color);

            // draw text
            if (!string.IsNullOrEmpty(Text))
            {
                // find the center of the button and fit text
                var x = (Rectangle.X + Rectangle.Width / 2) - (font.MeasureString(Text).X * FontScale / 2);
                var y = (Rectangle.Y + Rectangle.Height / 2) - (font.MeasureString(Text).Y * FontScale / 2);

                spriteBatch.DrawString(font, Text, new Vector2(x, y), TextColor, 0.0f, Vector2.Zero, FontScale, SpriteEffects.None, 0f);
            }

        }
    }
}
