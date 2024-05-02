using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace ISP_Project.Components
{
    public class Sprite
    {
        public Texture2D Texture {  get; set; }
        public Vector2 Position { get; set; }
        public float Scale { get; set; }

        public Sprite(Texture2D texture, Vector2 position, float scale)
        {
            Texture = texture;
            Position = position; ;
            Scale = scale;
        }

    }
}
