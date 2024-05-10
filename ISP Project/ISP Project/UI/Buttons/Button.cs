using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ISP_Project.Managers;
using System.Diagnostics;
using ISP_Project.Components;

namespace ISP_Project.UI.Buttons
{
    // Code Reference: https://github.com/Oyyou/MonoGame_Tutorials/blob/master/MonoGame_Tutorials/Tutorial013/Controls/Button.cs
    public abstract class Button
    {
        public Sprite Sprite { get; set; } = new Sprite(null, SpriteEffects.None, 0);
        public string texturePath;
        public SpriteFont font;
        public string fontPath; // every button will have this font
        public string Text { get; set; }
        public Color TextColor { get; set; }
        private Vector2 position;
        public Vector2 Position 
        { 
            get 
            { 
                // Why is WindowManager.GetRenderPosition() needed?
                return (position - Sprite.GetSpriteOrigin()) * WindowManager.GetRenderScale() + WindowManager.GetRenderPosition(); 
            }
            set
            {
                position = value; 
            } 
        }
        private float buttonScale;
        public float ButtonScale 
        { 
            get 
            { 
                return buttonScale * WindowManager.GetRenderScale(); 
            } 
            set 
            { 
                buttonScale = value; 
            } 
        }
        public float FontScale { get; set; }
        
        // button "hitbox" ("clickbox," if you will :) )
        public Rectangle Rectangle
        {
            get
            {
                return new Rectangle((int)(Position.X), (int)(Position.Y), 
                    (int)(Sprite.Texture.Width * ButtonScale), (int)(Sprite.Texture.Height * ButtonScale));
            }
        }
        public bool ForceShade { get; set; } = false;
        private bool isHovering;

        public Button(Texture2D texture, SpriteFont font, float buttonScale, float fontScale)
        {
            this.Sprite.Texture = texture;
            this.font = font;

            TextColor = Color.Black;
            ButtonScale = buttonScale;
            FontScale = fontScale;
        }

        public void Update(GameTime gameTime)
        {
            if (InputManager.cursorRectangle.Intersects(Rectangle))
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

        public void Draw(GameTime gameTime)
        {
            // draw button and change color on cursor hover
            var color = Color.White;
            if (isHovering || ForceShade)
                color = Color.Gray;
            else { ForceShade = false; }

            // not relying on Rectangle data here because the Renderer will scale this
            Globals.SpriteBatch.Draw(Sprite.Texture, position, null, color, 0f, Sprite.GetSpriteOrigin(), buttonScale, SpriteEffects.None, 0.5f);

            // draw text
            if (!string.IsNullOrEmpty(Text))
            {
                // find the center of the button and fit text
                var x = (position.X) - (font.MeasureString(Text).X * FontScale / 2);
                var y = (position.Y) - (font.MeasureString(Text).Y * FontScale / 2);

                Globals.SpriteBatch.DrawString(font, Text, new Vector2(x, y), TextColor, 0f, Vector2.Zero, FontScale, SpriteEffects.None, 1f);
            }
        }
    }
}
