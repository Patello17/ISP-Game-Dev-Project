using Microsoft.Xna.Framework;
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

        public void SetMap(Dictionary<Vector2, int> collisions)
        {
            var mapWidth = collisions.ElementAt(collisions.Count - 1).Key.X + 1;
            var mapHeight = collisions.ElementAt(collisions.Count - 1).Key.Y + 1;

            map = new int[(int)mapWidth, (int)mapHeight];

            // Debug.WriteLine(collisions.Count + " / " + map.Length);


            for (int i = 0; i < collisions.Count; i++)
            {
                map[(int)collisions.ElementAt(i).Key.X, (int)collisions.ElementAt(i).Key.Y] =
                    collisions.ElementAt(i).Value;
            }

            /*for (int y = 0; y < mapHeight; y++)
            {
                for (int x = 0; x < mapWidth; x++)
                {
                    Debug.WriteLine(x + ", " + y);
                    Debug.WriteLine(mapWidth + ", " + mapHeight);
                    map[x, y] = collisions.ElementAt((Index)((mapWidth * y) + x)).Value;
                    Debug.WriteLine(map);
                }
            }*/
            
        }

        public bool isCollision(Vector2 position)
        {
            if (map[(int)position.X, (int)position.Y] != -1)
                return true;
            return false;
        }

    }
}
