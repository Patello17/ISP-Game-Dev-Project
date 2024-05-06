using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ISP_Project.Tilemaps
{
    // Code Reference: https://www.youtube.com/watch?v=LViEJPIu76E
    public class HubTileMap : Tilemap
    {
        private Texture2D roomTileset;
        private Texture2D decorationsTileset;
        private Texture2D collisionsTileset;
        private Vector2 position;
        public new Vector2 Position { get { return position; } set { position = value; } }
        private Dictionary<Vector2, int> room;
        private Dictionary<Vector2, int> decorations;
        private Dictionary<Vector2, int> collisions;
        public override CollisionMap CollisionMap { get; set; } = new CollisionMap();

        public HubTileMap(Vector2 position)
        {
            Position = position;
            room = LoadTileMap("../../../Tilemaps/Maps/Hub Map/Hub Tilemap_Room.csv");
            decorations = LoadTileMap("../../../Tilemaps/Maps/Hub Map/Hub Tilemap_Decorations.csv");
            collisions = LoadTileMap("../../../Tilemaps/Maps/Hub Map/Hub Tilemap_Collisions.csv");
            CollisionMap.SetMap(collisions);
        }

        public override void LoadContent(ContentManager content)
        {
            roomTileset = content.Load<Texture2D>("Tilesets/Hub Tileset");
            decorationsTileset = content.Load<Texture2D>("Tilesets/Hub Tileset");
            collisionsTileset = content.Load<Texture2D>("Tilesets/Collisions Tileset");
        }

        public void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            Draw(gameTime, spriteBatch, 16, 16, 16,
                new Dictionary<Dictionary<Vector2, int>, Texture2D>
                {
                    { room, roomTileset},
                    { decorations, decorationsTileset},
                    // { collisions, collisionsTileset},
                }, 
                Position);
        }
    }
}
