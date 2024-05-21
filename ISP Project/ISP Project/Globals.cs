using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ISP_Project.Managers;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using System.Diagnostics;

namespace ISP_Project
{
    /// <summary>
    /// This class holds references to frequently accessed global variables.
    /// </summary>
    public static class Globals
    {
        public static float Time { get; set; }
        public static ContentManager ContentManager { get; set; }
        // public static ContentManager CurrentStateTextureContentManager { get; set; }
        public static GraphicsDevice GraphicsDevice { get; set; }
        public static GraphicsDevice StateGraphicsDevice { get; set; }
        public static GraphicsDeviceManager GraphicsDeviceManager { get; set; }
        public static SpriteBatch SpriteBatch { get; set; }

        public static void Update(GameTime gameTime)
        {
            Time = (float)gameTime.ElapsedGameTime.TotalSeconds; // should be 60 fps (1 / 60 = 0.0167...)
        }

        public static RenderTarget2D GetNewRenderTarget()
        {
            return new RenderTarget2D(GraphicsDevice, 640, 360);
        }

    }
}
