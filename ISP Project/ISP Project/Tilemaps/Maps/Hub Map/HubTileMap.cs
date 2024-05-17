using ISP_Project.Components;
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
        // create tileset variables
        private Texture2D roomTileset;
        private Texture2D decorationsTileset;
        private Texture2D collisionsTileset;
        public override Transform Transform { get; set; } = new Transform(Vector2.Zero, 1f, 0f);
        public override Vector2 TileMapDimensions { get; set; } = new Vector2(40, 22);

        // create variables for individual layers
        private Dictionary<Vector2, int> room;
        private Dictionary<Vector2, int> decorations;
        private Dictionary<Vector2, int> collisions;

        // create collision map
        public override CollisionMap CollisionMap { get; set; } = new CollisionMap();

        public HubTileMap(Vector2 position)
        {
            Transform.Position = position;
            room = LoadTileMap("../../../Tilemaps/Maps/Hub Map/Hub Tilemap_Room.csv");
            decorations = LoadTileMap("../../../Tilemaps/Maps/Hub Map/Hub Tilemap_Decorations.csv");
            collisions = LoadTileMap("../../../Tilemaps/Maps/Hub Map/Hub Tilemap_Collisions.csv");
            CollisionMap.SetMap(collisions);
        }

        public override void LoadContent()
        {
            roomTileset = Globals.ContentManager.Load<Texture2D>("Tilesets/Hub Tileset");
            decorationsTileset = Globals.ContentManager.Load<Texture2D>("Tilesets/Hub Tileset");
            collisionsTileset = Globals.ContentManager.Load<Texture2D>("Tilesets/Collisions Tileset");
        }

        public void Draw()
        {
            Draw(16, 16, 16,
                new Dictionary<Dictionary<Vector2, int>, Texture2D>
                {
                    { room, roomTileset},
                    { decorations, decorationsTileset},
                    // { collisions, collisionsTileset},
                },
                new Dictionary<Dictionary<Vector2, int>, float>
                {
                    { room, 0f},
                    { decorations, 0.2f},
                    // { collisions, collisionsTileset},
                });
        }
    }
}
