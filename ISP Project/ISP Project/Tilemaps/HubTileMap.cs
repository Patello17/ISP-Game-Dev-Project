using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ISP_Project.Tilemaps
{
    public class HubTileMap : Tilemap
    {
        private Texture2D tileset;
        private Dictionary<Vector2, int> room;
        private Dictionary<Vector2, int> decorations;
        private Dictionary<Vector2, int> collisions;

        public HubTileMap()
        {
            room = LoadTileMap("../../../Tilemaps/Maps/Hub Map/Hub Tilemap_Room.csv");
            decorations = LoadTileMap("../../../Tilemaps/Maps/Hub Map/Hub Tilemap_Decorations.csv");
            collisions = LoadTileMap("../../../Tilemaps/Maps/Hub Map/Hub Tilemap_Collisions.csv");
        }

        public override void LoadContent(ContentManager content)
        {
            tileset = content.Load<Texture2D>("Tilesets/Hub Tileset");
        }

        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {

            int display_tileSize = 16;
            int num_tiles_per_row = 16; // as specified in the tileset
            int pixel_tileSize = 16; // the sprite resolution of our game is 16x16

            foreach (var item in room)
            {
                Rectangle destinationRectangle = new Rectangle(
                    (int)item.Key.X * display_tileSize, 
                    (int)item.Key.Y * display_tileSize,
                    display_tileSize, display_tileSize);

                int x = item.Value % num_tiles_per_row;
                int y = item.Value / num_tiles_per_row;

                Rectangle sourceRectangle = new Rectangle(
                    x * pixel_tileSize,
                    y * pixel_tileSize,
                    pixel_tileSize, pixel_tileSize
                    );

                spriteBatch.Draw(tileset, destinationRectangle, sourceRectangle, Color.White);

            }
        }
    }
}
