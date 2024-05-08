using ISP_Project.Game_States;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Extended.Sprites;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ISP_Project.Managers
{
    public class StateManager
    {
        private static State previousState;
        private static State currentState;
        private static State nextState;

        public static void Update(GameTime gameTime)
        {
            if (nextState != null)
            {
                previousState = currentState;
                currentState = nextState;
                nextState = null;
                currentState.LoadState(Globals.ContentManager);
            }
            currentState.Update(gameTime);
        }

        public static void Draw(GameTime gameTime)
        {
            currentState.Draw(gameTime);
        }
        public static void ChangeState(State state)
        {
            nextState = state;
        }
        public static State GetCurrentState()
        {
            return currentState;
        }
        public static State GetPreviousState()
        {
            return previousState;
        }
    }
}
