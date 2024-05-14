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
    public class LevelOneTileMap : Tilemap
    {
        private Texture2D levelTileset;
        private Texture2D collisionsTileset;
        public override Transform Transform { get; set; } = new Transform(Vector2.Zero, 1f, 0f);
        public override Vector2 TileMapDimensions { get; set; } = new Vector2(40, 22);
        private Dictionary<Vector2, int> level;
        private Dictionary<Vector2, int> collisions;
        public override CollisionMap CollisionMap { get; set; } = new CollisionMap();

        public LevelOneTileMap(Vector2 position)
        {
            Transform.Position = position;
            level = LoadTileMap("../../../Tilemaps/Maps/Level 1/Level One Tilemap_Level Map.csv");
            collisions = LoadTileMap("../../../Tilemaps/Maps/Level 1/Level One Tilemap_Collisions.csv");
            CollisionMap.SetMap(collisions);
        }

        public override void LoadContent()
        {
            levelTileset = Globals.ContentManager.Load<Texture2D>("Tilesets/Grassy Level Tileset");
            collisionsTileset = Globals.ContentManager.Load<Texture2D>("Tilesets/Collisions Tileset");
        }

        public void Draw(GameTime gameTime)
        {
            Draw(gameTime, 16, 10, 16,
                new Dictionary<Dictionary<Vector2, int>, Texture2D>
                {
                    { level, levelTileset},
                    // { collisions, collisionsTileset},
                },
                new Dictionary<Dictionary<Vector2, int>, float>
                {
                    { level, 0f}
                    // { collisions, collisionsTileset},
                });
        }
    }
}
