using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace ISP_Project.Screen_Management
{
    // Code Reference: https://www.youtube.com/watch?v=lpFCseClnA4
    public class Renderer
    {
        // create render target variables
        private RenderTarget2D renderTarget;
        private Rectangle destinationRectangle;
        public Vector2 RenderPosition { get; set; }
        public float RenderScale { get; set; }
        private Vector2 renderSize = new Vector2(640, 360); // default resolution


        public Renderer()
        {
            renderTarget = new RenderTarget2D(Globals.GraphicsDevice, (int)renderSize.X, (int)renderSize.Y);
        }

        /// <summary>
        /// Adjusts the destination rectangle of the render target to fit the window dimensions.
        /// </summary>
        public void SetDestinationRectangle()
        {
            var screenSize = Globals.GraphicsDevice.PresentationParameters.Bounds;

            float scaleX = (float)screenSize.Width / renderTarget.Width;
            float scaleY = (float)screenSize.Height / renderTarget.Height;
            RenderScale = Math.Min(scaleX, scaleY);

            int newWidth = (int)(renderTarget.Width * RenderScale);
            int newHeight = (int)(renderTarget.Height * RenderScale);

            RenderPosition = new Vector2((screenSize.Width - newWidth) / 2, (screenSize.Height - newHeight) / 2);

            destinationRectangle = new Rectangle((int)RenderPosition.X, (int)RenderPosition.Y, newWidth, newHeight);
        }
        
        /// <summary>
        /// Activate the render target.
        /// </summary>
        public void Activate()
        {
            Globals.GraphicsDevice.SetRenderTarget(renderTarget);
            // set the unused render area to this color
            Globals.GraphicsDevice.Clear(Color.Coral);
        }

        /// <summary>
        /// Draw the render target.
        /// </summary>
        /// <param name="gameTime"></param>
        public void Draw(GameTime gameTime)
        {
            Globals.GraphicsDevice.SetRenderTarget(null);
            // set the excess window screen this color (i.e. the "black bars")
            Globals.GraphicsDevice.Clear(Color.LightSlateGray);

            Globals.SpriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend,
                SamplerState.PointClamp, DepthStencilState.None, RasterizerState.CullCounterClockwise,
                null, null);
            Globals.SpriteBatch.Draw(renderTarget, destinationRectangle, Color.White);
            Globals.SpriteBatch.End();
        }

        /// <summary>
        /// Gets the render scale.
        /// </summary>
        /// <returns></returns>
        public float GetRenderScale()
        {
            return RenderScale;
        }

        /// <summary>
        /// Gets the position of the render target on the window.
        /// </summary>
        /// <returns></returns>
        public Vector2 GetRenderPosition()
        {
            return RenderPosition;
        }

        /// <summary>
        /// Gets the render dimensions.
        /// </summary>
        /// <returns></returns>
        public Vector2 GetRenderSize()
        {
            return renderSize;
        }

        public RenderTarget2D GetRenderTarget()
        {
            return renderTarget;
        }
    }
}
