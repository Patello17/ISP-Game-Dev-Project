using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ISP_Project.Screen_Management.Transitions
{
    // Code Reference: https://www.youtube.com/watch?v=oeUE2O6LAEU
    public class PushTransition(RenderTarget2D transitionFrame) : Transition(transitionFrame)
    {
        protected override void Process()
        {
            int mid = (int)(oldScene.Width * percentage);
            Vector2 oldPos = new(mid - oldScene.Width, 0);
            Vector2 newPos = new(mid, 0);

            Globals.SpriteBatch.Draw(oldScene, oldPos, Color.White);
            Globals.SpriteBatch.Draw(newScene, newPos, Color.White);
        }
    }
}
