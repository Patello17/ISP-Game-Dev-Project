using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Extended.Tiled;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ISP_Project.Tilemaps
{
    // Code Reference: https://www.youtube.com/watch?v=LViEJPIu76E
    public abstract class Tilemap
    {
        public abstract CollisionMap CollisionMap { get; set; }
        public Dictionary<Vector2, int> LoadTileMap(string filePath)
        {
            Dictionary<Vector2, int> result = new Dictionary<Vector2, int>(); 
            StreamReader reader = new StreamReader(filePath);

            int y = 0;
            string line;
            while ((line = reader.ReadLine()) != null)
            {
                string[] items = line.Split(",");

                for (int x = 0; x < items.Length; x++)
                {
                    if (int.TryParse(items[x], out int value))
                    {
                        if (value > -1)
                        {
                            result[new Vector2(x, y)] = value;
                        }
                    }
                }
                
                y++;
            }

            return result;
        }

        // load textures
        public abstract void LoadContent(ContentManager content);

        // draw textures
        public void Draw(GameTime gameTime, SpriteBatch spriteBatch,
            int displayTileSize, int numTilesPerRow, int pixelTileSize,
            Dictionary<Dictionary<Vector2, int>, Texture2D> textureMapDictionary,
            Vector2 position)
        {
            foreach (var layer in textureMapDictionary.Keys)
            {
                foreach (var item in layer)
                {
                    Rectangle destinationRectangle = new Rectangle(
                        (int)item.Key.X * displayTileSize + (int)position.X,
                        (int)item.Key.Y * displayTileSize + (int)position.Y,
                        displayTileSize, displayTileSize);

                    int x = item.Value % numTilesPerRow;
                    int y = item.Value / numTilesPerRow;

                    Rectangle sourceRectangle = new Rectangle(
                        x * pixelTileSize,
                        y * pixelTileSize,
                        pixelTileSize, pixelTileSize
                    );

                    spriteBatch.Draw(textureMapDictionary[layer], destinationRectangle, sourceRectangle, Color.White);
                }
            }
        }
    }
}
