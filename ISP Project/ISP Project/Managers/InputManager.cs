using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework;

namespace ISP_Project.Managers
{
    // Code Reference: My brother made this InputManager class a while back, but I added the click functionality
    public class InputManager
    {
        // create button detection variables
        private static KeyboardState currentKeyState;
        private static KeyboardState previousKeyState;

        // create click detection variables
        private static MouseState mouseState;
        private static bool previousLeftButtonState;
        private static bool previousRightButtonState;

        // create rectangle that tracks the mouse position
        public static Rectangle cursorRectangle;

        // declare key actions
        public enum Inputs
        {
            UP, DOWN, LEFT, RIGHT,
            INTERACT, PAUSE,
            MAXIMIZESCREEN,
            RESTART
        }
        // declare click actions
        public enum ClickInputs
        {
            INTERACT
        }

        // assign button inputs to specific keys
        private static Dictionary<Inputs, Keys> playerKeys = new Dictionary<Inputs, Keys>()
        {
            { Inputs.UP, Keys.Up},
            { Inputs.LEFT, Keys.Left },
            { Inputs.DOWN, Keys.Down },
            { Inputs.RIGHT, Keys.Right },
            { Inputs.INTERACT, Keys.Z},
            { Inputs.PAUSE, Keys.Escape},
            { Inputs.MAXIMIZESCREEN, Keys.F11},
            { Inputs.RESTART, Keys.R}

        };

        // create click dictionaries
        private static Dictionary<ClickInputs, ButtonState> currentPlayerClicks;
        private static Dictionary<ClickInputs, bool> previousPlayerClicks;

        public static void Update(GameTime gameTime)
        {
            // update key press logic variables
            previousKeyState = currentKeyState;
            currentKeyState = Keyboard.GetState();

            // update click logic variables
            previousLeftButtonState = mouseState.LeftButton == ButtonState.Pressed ? true: false;
            previousRightButtonState = mouseState.RightButton == ButtonState.Pressed ? true : false;
            previousPlayerClicks = new Dictionary<ClickInputs, bool>()
            {
                { ClickInputs.INTERACT, previousLeftButtonState}
            };

            // update click dictionary with mouseState (there's has to be a better way to do this...)
            mouseState = Mouse.GetState();
            currentPlayerClicks = new Dictionary<ClickInputs, ButtonState>()
            {
                { ClickInputs.INTERACT, mouseState.LeftButton}
            };

            // update cursor rectangle
            cursorRectangle = new Rectangle(mouseState.Position.X, mouseState.Position.Y, 1, 1);
        }

        // get different information from key inputs
        public static Func<bool, bool, bool> isPressed = (currentState, previousState) => currentState;
        public static Func<bool, bool, bool> isTriggered = (currentState, previousState) => currentState && !previousState;
        public static Func<bool, bool, bool> isReleased = (currentState, previousState) => !currentState && previousState;

        // specify key to get input information from
        public static bool isKey(Inputs input, Func<bool, bool, bool> checkState)
        {
            return checkState(currentKeyState.IsKeyDown(playerKeys[input]), previousKeyState.IsKeyDown(playerKeys[input]));
        }

        public static bool isClick(ClickInputs input, Func<bool, bool, bool> checkState)
        {
            var currentClickState = false;
            var previousClickState = previousPlayerClicks[input];

            if (currentPlayerClicks[input] == ButtonState.Pressed)
                currentClickState = true;

            return checkState(currentClickState, previousClickState);
        }
    }
}