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
    public class Renderer
    {
        private RenderTarget2D renderTarget;
        private GraphicsDevice graphicsDevice;
        private Rectangle destinationRectangle;
        public Vector2 RenderPosition { get; set; }
        public float RenderScale { get; set; }

        public Renderer(GraphicsDevice graphicsDevice, int width, int height)
        {
            this.graphicsDevice = graphicsDevice;
            renderTarget = new RenderTarget2D(graphicsDevice, width, height);
        }

        public void SetDestinationRectangle()
        {
            var screenSize = graphicsDevice.PresentationParameters.Bounds;

            float scaleX = (float)screenSize.Width / renderTarget.Width;
            float scaleY = (float)screenSize.Height / renderTarget.Height;
            RenderScale = Math.Min(scaleX, scaleY);
            Debug.WriteLine(screenSize + " || " + renderTarget.Bounds);

            int newWidth = (int)(renderTarget.Width * RenderScale);
            int newHeight = (int)(renderTarget.Height * RenderScale);

            RenderPosition = new Vector2((screenSize.Width - newWidth) / 2, (screenSize.Height - newHeight) / 2);

            destinationRectangle = new Rectangle((int)RenderPosition.X, (int)RenderPosition.Y, newWidth, newHeight);
        }

        public void Activate()
        {
            graphicsDevice.SetRenderTarget(renderTarget);
            graphicsDevice.Clear(Color.DarkSlateGray);
        }

        public void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            graphicsDevice.SetRenderTarget(null);
            graphicsDevice.Clear(Color.Black);

            spriteBatch.Begin(SpriteSortMode.Deferred, null, SamplerState.PointClamp, null, null, null, null);
            spriteBatch.Draw(renderTarget, destinationRectangle, Color.White);
            spriteBatch.End();
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
