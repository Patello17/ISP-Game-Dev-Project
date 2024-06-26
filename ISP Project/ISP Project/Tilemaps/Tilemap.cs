﻿using ISP_Project.Components;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Extended.Tiled;
using System;
using System.Collections.Generic;
using System.Diagnostics;
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
        public abstract Transform Transform { get; set; }
        public abstract Vector2 TileMapDimensions { get; set; }
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

        /// <summary>
        /// Loads the tilesets for this Tile Map.
        /// </summary>
        public abstract void LoadContent();

        /// <summary>
        /// Draws this Tile Map using the given tileset(s).
        /// </summary>
        /// <param name="displayTileSize"></param>
        /// <param name="numTilesPerRow"></param>
        /// <param name="pixelTileSize"></param>
        /// <param name="textureMapDictionary"></param>
        /// <param name="depthDictionary"></param>
        public void Draw(int displayTileSize, int numTilesPerRow, int pixelTileSize,
            Dictionary<Dictionary<Vector2, int>, Texture2D> textureMapDictionary, 
            Dictionary<Dictionary<Vector2, int>, float> depthDictionary)
        {
            foreach (var layer in textureMapDictionary.Keys)
            {
                foreach (var item in layer)
                {
                    Rectangle destinationRectangle = new Rectangle(
                        (int)item.Key.X * displayTileSize * (int)Transform.Scale + (int)Transform.Position.X,
                        (int)item.Key.Y * displayTileSize * (int)Transform.Scale + (int)Transform.Position.Y,
                        displayTileSize * (int)Transform.Scale, displayTileSize * (int)Transform.Scale);

                    int x = item.Value % numTilesPerRow;
                    int y = item.Value / numTilesPerRow;

                    Rectangle sourceRectangle = new Rectangle(
                        x * pixelTileSize,
                        y * pixelTileSize,
                        pixelTileSize, pixelTileSize
                    );

                    Globals.SpriteBatch.Draw(textureMapDictionary[layer], destinationRectangle, sourceRectangle, Color.White, Transform.Rotation,
                        new Vector2(TileMapDimensions.X * pixelTileSize / 2, TileMapDimensions.Y * pixelTileSize / 2), 
                        SpriteEffects.None, depthDictionary[layer]);
                }
            }
        }
    }
}
