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
        public abstract Sprite Sprite { get; set; }
        public abstract Transform Transform { get; set; }
        public abstract Vector2 TileMapPosition { get; set; }
        public abstract List<Vector2> PastPositions { get; set; }

        /// <summary>
        /// Loads this Actor.
        /// </summary>
        public abstract void LoadContent();

        /// <summary>
        /// Updates this Actor.
        /// </summary>
        /// <param name="collisionMap"></param>
        public abstract void Update();

        /// <summary>
        /// Updates the position of this Actor; Actors need a reference to a collision map to interact with the scene and determine collisions.
        /// </summary>
        /// <param name="collisionMap"></param>
        public abstract void UpdatePosition(CollisionMap collisionMap);

        /// <summary>
        /// Draws this Actor.
        /// </summary>
        public abstract void Draw();
    }
}
