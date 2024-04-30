﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace ISP_Project.Gameplay
{
    internal class Actor
    {
        public Texture2D Sprite;
        public Vector2 Position;
        public int DrawLayer;

        public void Update(GameTime gameTime) { }
        public void Draw(GameTime gameTime, SpriteBatch spriteBatch) { }
    }
}