using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ISP_Project.Components
{
    public class Transform
    {
        public Vector2 Position { get; set; }
        public float Scale { get; set; }
        public float Rotation { get; set; }

        public Transform(Vector2 position, float scale, float rotation)
        {
            Position = position;
            Scale = scale;
            Rotation = rotation;
        }
    }
}
