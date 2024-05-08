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
        private RenderTarget2D renderTarget;
        private GraphicsDevice graphicsDevice;
        private Rectangle destinationRectangle;
        public Vector2 RenderPosition { get; set; }
        public float RenderScale { get; set; }

        public Renderer(GraphicsDevice graphicsDevice, Vector2 renderSize)
        {
            this.graphicsDevice = graphicsDevice;
            renderTarget = new RenderTarget2D(graphicsDevice, (int)renderSize.X, (int)renderSize.Y);
        }

        public void SetDestinationRectangle()
        {
            var screenSize = graphicsDevice.PresentationParameters.Bounds;

            float scaleX = (float)screenSize.Width / renderTarget.Width;
            float scaleY = (float)screenSize.Height / renderTarget.Height;
            RenderScale = Math.Min(scaleX, scaleY);

            int newWidth = (int)(renderTarget.Width * RenderScale);
            int newHeight = (int)(renderTarget.Height * RenderScale);

            RenderPosition = new Vector2((screenSize.Width - newWidth) / 2, (screenSize.Height - newHeight) / 2);

            destinationRectangle = new Rectangle((int)RenderPosition.X, (int)RenderPosition.Y, newWidth, newHeight);
        }

        public void Activate()
        {
            graphicsDevice.SetRenderTarget(renderTarget);
            // set the unused render area this color
            graphicsDevice.Clear(Color.Black);
        }

        public void Draw(GameTime gameTime)
        {
            graphicsDevice.SetRenderTarget(null);
            // set the excess window screen this color (i.e. the "black bars")
            graphicsDevice.Clear(Color.Black);

            Globals.SpriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, SamplerState.PointClamp, DepthStencilState.None, RasterizerState.CullCounterClockwise, null, null);
            Globals.SpriteBatch.Draw(renderTarget, destinationRectangle, Color.White);
            Globals.SpriteBatch.End();
        }

        public float GetRenderScale()
        {
            return RenderScale;
        }
        public Vector2 GetRenderPosition()
        {
            return RenderPosition;
        }
    }
}
