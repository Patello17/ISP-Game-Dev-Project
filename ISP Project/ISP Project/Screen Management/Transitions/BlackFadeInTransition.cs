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
    public class BlackFadeInTransition(RenderTarget2D transitionFrame) : Transition(transitionFrame)
    {
        protected override void Process()
        {
            Globals.SpriteBatch.Draw(oldScene, Vector2.Zero, Color.Black * percentage);
            Globals.SpriteBatch.Draw(newScene, Vector2.Zero, Color.White * (1 - percentage));
        }
    }
}
