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
    public class Window
    {
        private Vector2 windowSize;
        private bool isFullScreen;
        private Renderer renderer;
        private GraphicsDeviceManager graphicsDeviceManager;

        public Window(int width, int height, GraphicsDeviceManager graphicsDeviceManager)
        {
            this.graphicsDeviceManager = graphicsDeviceManager;
            renderer = new Renderer(graphicsDeviceManager.GraphicsDevice, width, height);
            isFullScreen = false; // not full screen by default
            SetResolution(width, height);
        }

        public void SetResolution(int width, int height)
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
            renderer.Activate();
            renderer.Draw(gameTime, spriteBatch);
        }
        public void SetRenderTarget()
        {
            renderer.Activate();
        }
    }
}
