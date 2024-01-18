using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework.Input;

namespace ISP_Project
{
    internal class InputManager
    {
        // create button detection variables
        private static KeyboardState currentPress;
        private static KeyboardState previousPress;

        // create click detection variables
        private static MouseState currentClick;
        private static MouseState previousClick;

        // declare key actions
        public enum Inputs
        {
            Up, Down, Left, Right
        }
        // declare click actions
        public enum ClickInputs
        {
            Interact
        }

        // assign button inputs to specific keys
        private static Dictionary<Inputs, Keys> playerKeys = new Dictionary<Inputs, Keys>()
        {
            { Inputs.Up, Keys.W},
            { Inputs.Left, Keys.A },
            { Inputs.Down, Keys.S },
            { Inputs.Right, Keys.D }
        };
        // assign click inputs to specific clicks
        private static Dictionary<ClickInputs, ButtonState> playerClicks = new Dictionary<ClickInputs, ButtonState>()
        {
            { ClickInputs.Interact, currentClick.LeftButton}
        };

        public static void Update()
        {
            // update key press logic variables
            previousPress = currentPress;
            currentPress = Keyboard.GetState();

            // update click logic variables
            previousClick = currentClick;
            currentClick = Mouse.GetState();
        }

        // get different information from key inputs
        public static Func<bool, bool, bool> isPressed = (keyState, oldState) => keyState;
        public static Func<bool, bool, bool> isTriggered = (keyState, oldState) => keyState && !oldState;
        public static Func<bool, bool, bool> isReleased = (keyState, oldState) => !keyState && oldState;

        // get different information from click inputs
        public static Func<bool, bool, bool> isMousePressed = (mouseState, oldMouseState) => mouseState;
        public static Func<bool, bool, bool> isMouseTriggered = (mouseState, oldMouseState) => mouseState && !oldMouseState;
        public static Func<bool, bool, bool> isMouseReleased = (mouseState, oldMouseState) => !mouseState && oldMouseState;

        // specify key to get input information from
        public static bool isKey(Inputs input, Func<bool, bool, bool> checkState)
        {
            return checkState(currentPress.IsKeyDown(playerKeys[input]), previousPress.IsKeyDown(playerKeys[input]));
        }
    }
}