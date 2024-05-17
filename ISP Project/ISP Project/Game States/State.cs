using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ISP_Project.Game_States
{
    // Code Reference: https://github.com/Oyyou/MonoGame_Tutorials/blob/master/MonoGame_Tutorials/Tutorial013/States/State.cs
    public abstract class State
    {

        RenderTarget2D target;
        public State()
        {
            target = Globals.GetNewRenderTarget();
        }
        /// <summary>
        /// Loads everything in this State.
        /// </summary>
        public abstract void LoadState();

        /// <summary>
        /// Updates everything in this State.
        /// </summary>
        public abstract void Update();

        /// <summary>
        /// Updates everything in this State one last time.
        /// </summary>
        public abstract void PostUpdate();

        /// <summary>
        /// Draws everything in this State.
        /// </summary>
        public abstract void Draw();

        public virtual RenderTarget2D GetFrame()
        {
            Globals.GraphicsDevice.SetRenderTarget(target);
            Globals.GraphicsDevice.Clear(Color.Black);

            Globals.SpriteBatch.Begin(SpriteSortMode.FrontToBack, BlendState.AlphaBlend,
                SamplerState.PointClamp, DepthStencilState.None, RasterizerState.CullCounterClockwise,
                null, null);
            Draw();
            Globals.SpriteBatch.End();

            // Draw();

            Globals.GraphicsDevice.SetRenderTarget(null);
            return target;
        }
    }
}
