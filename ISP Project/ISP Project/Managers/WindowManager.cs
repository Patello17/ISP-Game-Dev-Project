using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Schema;
using ISP_Project.Screen_Management;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace ISP_Project.Managers
{
    public class WindowManager
    {
        /*// declare Window variable
        public static Window Window { get; set; }

        public static void Update(GameTime gameTime)
        {
            
        }

        public static void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            Window.Draw(gameTime, spriteBatch);
            StateManager.Draw(gameTime, spriteBatch);
        }*/
        public static Window window;
        public static Texture2D texture;

        public static void InitializeWindow(GraphicsDeviceManager graphicDeviceManager)
        {
            window = new Window(1600, 900, graphicDeviceManager); // default window size
        }

        public static void Update(GameTime gameTime)
        {
            
        }

        public static void DrawMainWindow(GameTime gameTime, SpriteBatch spriteBatch)
        {
            window.SetRenderTarget();
            
            spriteBatch.Begin(SpriteSortMode.Deferred, null, SamplerState.PointClamp, null, null, null, null); // final null should be replaced with camera transformation matrix i think
            spriteBatch.Draw(texture, new Vector2(600, 600), Color.White);
            StateManager.Draw(gameTime, spriteBatch);
            spriteBatch.End();

            window.Draw(gameTime, spriteBatch);
        }

        public static void SetMainWindowResolution(int width, int height)
        {
            window.SetResolution(width, height);
        }
        public static void LoadSampleTexture()
        {
            texture = Globals.ContentManager.Load<Texture2D>("UI Elements/Button");
        }

    }
}
