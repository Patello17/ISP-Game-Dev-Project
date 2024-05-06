using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using ISP_Project.Components;
using ISP_Project.Tilemaps;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace ISP_Project.Gameplay
{
    public abstract class Actor
    {
        public Sprite sprite;
        public Transform transform;
        public Vector2 tileMapPosition;

        public abstract void LoadContent(ContentManager content);
        public abstract void Update(GameTime gameTime, CollisionMap collisionMap);
        public abstract void Draw(GameTime gameTime, SpriteBatch spriteBatch);
    }
}
