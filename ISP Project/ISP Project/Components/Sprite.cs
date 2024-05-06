using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ISP_Project.Components
{
    public class Sprite
    {
        public Texture2D Texture {  get; set; }
        public SpriteEffects SpriteEffects { get; set; }
        public int DrawLayer { get; set; }

        public Sprite(Texture2D texture, SpriteEffects spriteEffects, int drawLayer)
        {
            Texture = texture;
            SpriteEffects = spriteEffects;
            DrawLayer = drawLayer;
        }

        public Vector2 GetSpriteOrigin()
        {
            return new Vector2(Texture.Width / 2, Texture.Height / 2);
        }

    }
}
