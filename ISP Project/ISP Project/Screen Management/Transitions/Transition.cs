using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ISP_Project.Screen_Management.Transitions
{
    public abstract class Transition(RenderTarget2D transitionFrame)
    {
        protected RenderTarget2D frame = transitionFrame;
        protected RenderTarget2D oldScene;
        protected RenderTarget2D newScene;
        protected float duration;
        protected float durationLeft;
        protected float percentage;

        protected abstract void Process();

        public void Start(RenderTarget2D oldScene, RenderTarget2D newScene, float length)
        {
            this.oldScene = oldScene;
            this.newScene = newScene;
            duration = length;
            durationLeft = duration;
        }

        public virtual bool Update()
        {
            durationLeft -= Globals.Time;
            percentage = durationLeft / duration;
            return durationLeft < 0f;
        }

        /*public RenderTarget2D GetFrame()
        {
            Globals.GraphicsDevice
        }*/

    }
}
