using ISP_Project.Components;
using ISP_Project.Game_States.Letters;
using ISP_Project.Managers;
using ISP_Project.Screen_Management.Transitions;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Color = Microsoft.Xna.Framework.Color;

namespace ISP_Project.UI.Buttons
{
    public class Envelope : Button
    {
        private Texture2D buttonTexture;
        private SpriteFont buttonFont;
        private float buttonScale;
        private float fontScale;
        private bool forceShadeLock = false;
        public Vector2 Position { get; set; } = Vector2.Zero;
        /*private Color color = Color.White;
        public Color Color { get { return color; } set { color = value; } }*/
        public override bool ForceShade { get; set; }
        public bool Opened { get; set; }

        public Envelope(Texture2D texture, SpriteFont font, float buttonScale, float fontScale) : base(texture, font, buttonScale, fontScale)
        {
            buttonTexture = texture;
            buttonFont = font;
            this.buttonScale = buttonScale;
            this.fontScale = buttonScale;
        }

        public override void TriggerEvent()
        {

        }

        public override void Draw()
        {
            var color = Color.White;
            switch (Opened)
            {
                case (false):
                    if (ForceShade)
                    {
                        color = new Color(71, 0, 0);
                    }
                    else
                    {
                        color = new Color(198, 110, 110);
                    }
                    break;

                default:
                    if (ForceShade)
                    {
                        color = Color.Gray;
                    }
                    else
                    {
                        color = Color.White;
                    }
                    break;
            }

            // not relying on Rectangle data here because the Renderer will scale this
            Globals.SpriteBatch.Draw(Sprite.Texture, Position, null, color, 0f, Sprite.GetSpriteOrigin(), buttonScale, SpriteEffects.None, 0.5f);
        }

    }
}
