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
        public static Window window;

        public static void InitializeWindow(GraphicsDeviceManager graphicDeviceManager)
        {
            window = new Window(640, 360, graphicDeviceManager); // default window size
        }

        public static void Update(GameTime gameTime)
        {
            
        }

        public static void DrawMainWindow(GameTime gameTime)
        {
            window.SetRenderTarget();

            Globals.SpriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, SamplerState.PointClamp, DepthStencilState.None, RasterizerState.CullCounterClockwise, null, null); // final null should be replaced with camera transformation matrix i think
            StateManager.Draw(gameTime);
            Globals.SpriteBatch.End();

            window.Draw(gameTime);
        }

        public static float GetRenderScale()
        {
            return window.GetRenderScale();
        }
        public static Vector2 GetRenderPosition()
        {
            return window.GetRenderPosition();
        }
        public static Vector2 GetMainWindowCenter()
        {
            return window.GetCenter();
        }

        public static void SetMainWindowSize(int width, int height)
        {
            window.SetWindowSize(width, height);
        }
        public static void SetMainWindowFullScreen()
        {
            window.SetFullScreen();
        }
        

    }
}
