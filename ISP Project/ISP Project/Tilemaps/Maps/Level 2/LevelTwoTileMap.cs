using ISP_Project.Components;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ISP_Project.Tilemaps.Maps.Level_1
{
    public class LevelTwoTileMap : Tilemap
    {
        // create tileset variables
        private Texture2D levelTileset;
        private Texture2D collisionsTileset;
        public override Transform Transform { get; set; } = new Transform(Vector2.Zero, 1f, 0f);
        public override Vector2 TileMapDimensions { get; set; } = new Vector2(40, 22);

        // create variables for individual layers
        private Dictionary<Vector2, int> level;
        private Dictionary<Vector2, int> decorations;
        private Dictionary<Vector2, int> collisions;

        // create collision map
        public override CollisionMap CollisionMap { get; set; } = new CollisionMap();

        public LevelTwoTileMap(Vector2 position)
        {
            Transform.Position = position;
            level = LoadTileMap("../../../Tilemaps/Maps/Level 2/Level Two Tilemap_Level Map.csv");
            decorations = LoadTileMap("../../../Tilemaps/Maps/Level 2/Level Two Tilemap_Decorations.csv");
            collisions = LoadTileMap("../../../Tilemaps/Maps/Level 2/Level Two Tilemap_Collisions.csv");
            CollisionMap.SetMap(collisions);
        }

        public override void LoadContent()
        {
            levelTileset = Globals.ContentManager.Load<Texture2D>("Tilesets/Grassy Level Tileset");
            collisionsTileset = Globals.ContentManager.Load<Texture2D>("Tilesets/Collisions Tileset");
        }

        public void Draw()
        {
            Draw(16, 14, 16,
                new Dictionary<Dictionary<Vector2, int>, Texture2D>
                {
                    { level, levelTileset },
                    { decorations, levelTileset }
                    // { collisions, collisionsTileset},
                },
                new Dictionary<Dictionary<Vector2, int>, float>
                {
                    { level, 0f},
                    { decorations, 0.2f},
                    // { collisions, collisionsTileset},
                });
        }
    }
}
