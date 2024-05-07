using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ISP_Project.Screen_Management
{
    // Code Reference: https://www.youtube.com/watch?v=lpFCseClnA4
    public class Window
    {
        private Vector2 windowSize;
        private Vector2 renderSize = new Vector2(640, 360); // default render size
        private bool isFullScreen;
        private Renderer renderer;
        private GraphicsDeviceManager graphicsDeviceManager;

        public Window(int width, int height, GraphicsDeviceManager graphicsDeviceManager)
        {
            this.graphicsDeviceManager = graphicsDeviceManager;
            renderer = new Renderer(graphicsDeviceManager.GraphicsDevice, renderSize); 
            isFullScreen = false; // not full screen by default
            SetWindowSize(width, height);
        }

        public void SetWindowSize(int width, int height)
        {
            windowSize = new Vector2(width, height);
            graphicsDeviceManager.PreferredBackBufferWidth = width;
            graphicsDeviceManager.PreferredBackBufferHeight = height;

            graphicsDeviceManager.ApplyChanges();
            renderer.SetDestinationRectangle();
        }

        public void SetFullScreen()
        {
            var screenSizeWidth = GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Width;
            var screenSizeHeight = GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Height;

            windowSize = new Vector2(screenSizeWidth, screenSizeHeight);
            graphicsDeviceManager.PreferredBackBufferWidth = screenSizeWidth;
            graphicsDeviceManager.PreferredBackBufferHeight = screenSizeHeight;

            graphicsDeviceManager.ApplyChanges();
            renderer.SetDestinationRectangle();
        }

        public void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            // renderer.Activate();
            renderer.Draw(gameTime, spriteBatch);
        }
        public void SetRenderTarget()
        {
            renderer.Activate();
        }

        public float GetRenderScale()
        {
            return renderer.GetRenderScale();
        }
        public Vector2 GetRenderPosition()
        {
            return renderer.GetRenderPosition();
        }
        public Vector2 GetCenter()
        {
            return renderSize / 2;
        }
    }
}
