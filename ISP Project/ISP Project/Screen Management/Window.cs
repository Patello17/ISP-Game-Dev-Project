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
        private bool isFullScreen = false; // not full screen by default
        private Renderer renderer;
        private GraphicsDeviceManager graphicsDeviceManager;

        public Window(int width, int height, GraphicsDeviceManager graphicsDeviceManager)
        {
            this.graphicsDeviceManager = graphicsDeviceManager;
            renderer = new Renderer(graphicsDeviceManager.GraphicsDevice); 
            SetWindowSize(width, height);
        }

        /// <summary>
        /// Sets the size of the window.
        /// </summary>
        /// <param name="width"></param>
        /// <param name="height"></param>
        public void SetWindowSize(int width, int height)
        {
            windowSize = new Vector2(width, height);
            graphicsDeviceManager.PreferredBackBufferWidth = width;
            graphicsDeviceManager.PreferredBackBufferHeight = height;

            graphicsDeviceManager.ApplyChanges();
            renderer.SetDestinationRectangle();
        }

        /// <summary>
        /// Sets the window to full screen mode and adjusts render target accordingly.
        /// </summary>
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

        /// <summary>
        /// Draws the render target
        /// </summary>
        /// <param name="gameTime"></param>
        public void Draw(GameTime gameTime)
        {
            renderer.Draw(gameTime);
        }

        /// <summary>
        /// Sets the render target
        /// </summary>
        public void SetRenderTarget()
        {
            renderer.Activate();
        }

        /// <summary>
        /// Gets the render scale.
        /// </summary>
        /// <returns></returns>
        public float GetRenderScale()
        {
            return renderer.GetRenderScale();
        }

        /// <summary>
        /// Gets the position of the render target on the window.
        /// </summary>
        /// <returns></returns>
        public Vector2 GetRenderPosition()
        {
            return renderer.GetRenderPosition();
        }

        /// <summary>
        /// Gets the center of the window.
        /// </summary>
        /// <returns></returns>
        public Vector2 GetCenter()
        {
            return renderer.GetRenderSize() / 2;
        }
    }
}
