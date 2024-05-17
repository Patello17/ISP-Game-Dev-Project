using ISP_Project.Game_States;
using ISP_Project.Screen_Management.Transitions;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Extended.Sprites;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Schema;

namespace ISP_Project.Managers
{
    // Code Reference (for transitions): https://www.youtube.com/watch?v=oeUE2O6LAEU
    public class StateManager
    {
        // create a stack of states
        private static List<State> stateStack = new List<State>()
        {
            new TitleState()
        };

        // create transition variables
        private static RenderTarget2D transitionFrame = Globals.GetNewRenderTarget();
        private static Dictionary<Transitions, Transition> transitions = new Dictionary<Transitions, Transition>()
        {
            { Transitions.BlackFade, new BlackFadeTransition(transitionFrame) }
        };
        private static Transition transition;
        public static bool IsTransitioning { get; set; } = false;

        /// <summary>
        /// Updates the states.
        /// </summary>
        /// <param name="gameTime"></param>
        public static void Update()
        {
            // update the current state
            if (stateStack.Count > 0 && !IsTransitioning)
            {
                GetCurrentState().Update();
            }
            else
            {
                // Debug.WriteLine("TRANSITIONING!");
                IsTransitioning = transition.Update();
            }
                

            // if there are duplicate states, only keep the most recent one
            var newStateStack = new List<State>();
            for (int i = 0; i < stateStack.Count; i++)
            {
                if (!IsStateInStateStack(newStateStack, stateStack[i]))
                    newStateStack.Add(stateStack[i]);
            }
            stateStack = newStateStack;
        }

        /// <summary>
        /// Draws the states.
        /// </summary>
        /// <param name="gameTime"></param>
        public static void Draw()
        {
            // draws the current state
            if (stateStack.Count > 0)
            {

                var frame = GetCurrentState().GetFrame();
                Debug.WriteLine(frame);
                
                Globals.SpriteBatch.Begin(SpriteSortMode.FrontToBack, BlendState.AlphaBlend,
                    SamplerState.PointClamp, DepthStencilState.None, RasterizerState.CullCounterClockwise,
                    null, null); // final null should be replaced with camera transformation matrix i think

                Globals.SpriteBatch.Draw(frame, Vector2.Zero, Color.White);
                // GetCurrentState().Draw();
                Globals.SpriteBatch.End();
                // Globals.SpriteBatch.Draw(frame, Vector2.Zero, Color.White);
            }
                
        }

        /// <summary>
        /// Changes the current state by sending it to the top of the state stack.
        /// </summary>
        /// <param name="nextState"></param>
        public static void ChangeState(State nextState)
        {
            if (nextState != null)
            {
                var oldScene = stateStack[0].GetFrame();
                var newScene = nextState.GetFrame();
                stateStack.Insert(0, nextState);

                transition = transitions[Transitions.BlackFade];
                transition.Start(oldScene, newScene, 2f);
                IsTransitioning = true;
            }
        }

        /// <summary>
        /// Gets the current state.
        /// </summary>
        /// <returns></returns>
        public static State GetCurrentState()
        {
            if (stateStack.Count > 0)
                return stateStack[0];
            return null;
        }

        /// <summary>
        /// Gets the previous state.
        /// </summary>
        /// <returns></returns>
        public static State GetPreviousState()
        {
            if (stateStack.Count > 1)
                return stateStack[1];
            return null;
        }

        /// <summary>
        /// Gets the most recent instance of the given state.
        /// </summary>
        /// <param name="state"></param>
        /// <returns></returns>
        public static State GetRecentState(State state)
        {
            foreach (State _state in stateStack)
            {
                if (_state.GetType() == state.GetType())
                    return _state;
            }
            return null;
        }

        /// <summary>
        /// Compares types of states and returns the most recent one in the state stack.
        /// </summary>
        /// <param name="states"></param>
        /// <returns></returns>
        public static State GetMostRecentState(List<State> states)
        {
            Dictionary<int, State> stateDictionary = new Dictionary<int, State>();
            List<int> indexList = new List<int>();

            foreach (State _state in states)
            {
                if (IsStateInStateStack(stateStack, _state))
                {
                    var index = stateStack.FindIndex(s => s.GetType() == _state.GetType());
                    stateDictionary.Add(index, _state);
                    indexList.Add(index);
                }
            }
            indexList.Sort();

            return stateDictionary[indexList[0]]; 
        }

        /// <summary>
        /// Checks if an instance of the given state is already in the state stack.
        /// </summary>
        /// <param name="stateInstances"></param>
        /// <param name="state"></param>
        /// <returns></returns>
        private static bool IsStateInStateStack(List<State> stateInstances, State state)
        {
            foreach (State _state in stateInstances)
            {
                if (_state.GetType() == state.GetType())
                    return true;
            }
            return false;
        }

        private static RenderTarget2D GetFrame()
        {
            return IsTransitioning ? transition.GetFrame() : stateStack[0].GetFrame();
        }
    }
}
