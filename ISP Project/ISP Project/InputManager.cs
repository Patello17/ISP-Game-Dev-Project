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
        private static KeyboardState currentKeyState;
        private static KeyboardState previousKeyState;

        // create click detection variables
        private static MouseState currentMouseState;
        private static MouseState previousMouseState;

        // declare key actions
        public enum Inputs
        {
            UP, DOWN, LEFT, RIGHT
        }
        // declare click actions
        public enum ClickInputs
        {
            INTERACT
        }

        // assign button inputs to specific keys
        private static Dictionary<Inputs, Keys> playerKeys = new Dictionary<Inputs, Keys>()
        {
            { Inputs.UP, Keys.W},
            { Inputs.LEFT, Keys.A },
            { Inputs.DOWN, Keys.S },
            { Inputs.RIGHT, Keys.D }
        };
        // assign click inputs to specific clicks
        private static Dictionary<ClickInputs, ButtonState> playerClicks = new Dictionary<ClickInputs, ButtonState>()
        {
            { ClickInputs.INTERACT, currentMouseState.LeftButton}
        };

        public static void Update()
        {
            // update key press logic variables
            previousKeyState = currentKeyState;
            currentKeyState = Keyboard.GetState();

            // update click logic variables
            previousMouseState = currentMouseState;
            currentMouseState = Mouse.GetState();
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
            return checkState(currentKeyState.IsKeyDown(playerKeys[input]), previousKeyState.IsKeyDown(playerKeys[input]));
        }
    }
}