using Microsoft.Xna.Framework;
using MonoGame.Extended;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ISP_Project.Tilemaps
{
    public class CollisionMap
    {
        private int[,] map = { };

        /// <summary>
        /// Parses the Collision Map .csv file and maps values onto a C# array.
        /// </summary>
        /// <param name="collisions"></param>
        public void SetMap(Dictionary<Vector2, int> collisions)
        {
            var mapWidth = collisions.ElementAt(collisions.Count - 1).Key.X + 1;
            var mapHeight = collisions.ElementAt(collisions.Count - 1).Key.Y + 1;

            map = new int[(int)mapWidth, (int)mapHeight];

            for (int i = 0; i < collisions.Count; i++)
            {
                map[(int)collisions.ElementAt(i).Key.X, (int)collisions.ElementAt(i).Key.Y] =
                    collisions.ElementAt(i).Value;
            }
        }

        /// <summary>
        /// Detects if a tile is not an air tile.
        /// </summary>
        /// <param name="position"></param>
        /// <returns></returns>
        public bool isColliding(Vector2 position)
        {
            try
            {
                // 0 is designated as "air" in the tilesheet
                if (map[(int)position.X, (int)position.Y] > 0) 
                    return true;
            }
            catch
            {
                return false;
            }
            return false;
        }

        /// <summary>
        /// Gets the collision tile at the given position coordinate.
        /// </summary>
        /// <param name="position"></param>
        /// <returns></returns>
        public int GetCollision(Vector2 position)
        {
            try
            {
                return map[(int)position.X, (int)position.Y];
            }
            catch
            {
                return 0;
            }
        }

        /// <summary>
        /// Changes a collision tile at a specific position coordinate.
        /// </summary>
        /// <param name="position"></param>
        /// <param name="tile"></param>
        public void SetCollision(Vector2 position, int tile)
        {
            try
            {
                map[(int)position.X, (int)position.Y] = tile;
            }
            catch
            {
                Debug.WriteLine("Could not change collision map.");
                // do nothing
            }
        }

        /// <summary>
        /// Draws the Collision Map (purely for debugging purposes).
        /// </summary>
        public void DrawMap()
        {
            Debug.WriteLine("------------------------------");
            for (int i = 0; i < map.GetLength(1); i++)
            {
                for (int j = 0; j < map.GetLength(0); j++)
                {
                    Debug.Write(map[j, i] + " ");
                }
                Debug.WriteLine("");
            }
        }

    }
}
