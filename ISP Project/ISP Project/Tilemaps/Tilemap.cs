using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ISP_Project.Tilemaps
{
    public abstract class Tilemap
    {
        public Texture2D Texture { get; set; }
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
        public abstract void Draw(GameTime gameTime, SpriteBatch spriteBatch);
    }
}
