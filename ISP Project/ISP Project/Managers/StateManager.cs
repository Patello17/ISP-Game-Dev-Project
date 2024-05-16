using ISP_Project.Game_States;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Extended.Sprites;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ISP_Project.Managers
{
    public class StateManager
    {
        /*private static State previousState;
        private static State currentState;
        private static State nextState;

        public static void Update(GameTime gameTime)
        {
            if (nextState != null)
            {
                previousState = currentState;
                currentState = nextState;
                nextState = null;
                currentState.LoadState();
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
        }*/

        /*public static List<State> states = new List<State>()
        {
            new TitleState(),
            new PauseState(),
            new SettingsState(),
            new AudioSettingsState(),

        };*/
        private static List<State> stateStack = new List<State>()
        {
            new TitleState()
        };

        public static void Update(GameTime gameTime)
        {
            if (stateStack.Count > 0)
                GetCurrentState().Update(gameTime);

            // if there are duplicate states, only keep the most recent one
            var newStateStack = new List<State>();
            for (int i = 0; i < stateStack.Count; i++)
            {
                if (!IsStateInStateStack(newStateStack, stateStack[i]))
                    newStateStack.Add(stateStack[i]);
            }
            stateStack = newStateStack;

            // Debug.WriteLine(stateStack.Count);
        }
        public static void Draw(GameTime gameTime)
        {
            if (stateStack.Count > 0)
                GetCurrentState().Draw(gameTime);
        }

        /*public static void AddState(State state)
        {
            stateStack.Insert(0, state);
        }*/

        public static void ChangeState(State nextState)
        {
            if (nextState != null)
            {
                stateStack.Insert(0, nextState);
                // stateStack[0].LoadState();
                // Debug.WriteLine("STATE CHANGE!");
            }
        }
        public static State GetCurrentState()
        {
            if (stateStack.Count > 0)
                return stateStack[0];
            return null;
        }
        public static State GetPreviousState()
        {
            if (stateStack.Count > 1)
                return stateStack[1];
            return null;
        }
        public static State GetRecentState(State state)
        {
            foreach (State _state in stateStack)
            {
                if (_state.GetType() == state.GetType())
                    return _state;
            }
            return null;
        }
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
        private static bool IsStateInStateStack(List<State> stateInstances, State state)
        {
            foreach (State _state in stateInstances)
            {
                if (_state.GetType() == state.GetType())
                    return true;
            }
            return false;
        }
    }
}
