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
    public class InputManager
    {
        // create button detection variables
        private static KeyboardState currentKeyState;
        private static KeyboardState previousKeyState;

        // create click detection variables
        private static MouseState mouseState;
        private static ButtonState previousLeftButton;
        private static ButtonState previousRightButton;

        // create rectangle that tracks the mouse position
        // (if this rectangle intersects with another rectangle, we know the mouse is hovering over something)
        public static Rectangle cursorRect;

        // declare key actions
        public enum Inputs
        {
            UP, DOWN, LEFT, RIGHT,
            INTERACT, PAUSE
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
            { Inputs.RIGHT, Keys.D },
            { Inputs.INTERACT, Keys.Z},
            { Inputs.PAUSE, Keys.Escape}

        };
        // assign click inputs to specific clicks
        private static Dictionary<ClickInputs, ButtonState> playerClicks = new Dictionary<ClickInputs, ButtonState>()
        {
            { ClickInputs.INTERACT, Mouse.GetState().LeftButton }
        };

        public static void Update()
        {
            // update key press logic variables
            previousKeyState = currentKeyState;
            currentKeyState = Keyboard.GetState();

            // update click logic variables
            previousLeftButton = mouseState.LeftButton;
            previousRightButton = mouseState.RightButton;
            mouseState = Mouse.GetState();

            // update cursor rectangle
            cursorRect = new Rectangle(mouseState.Position.X, mouseState.Position.Y, 1, 1);
        }

        // get different information from key inputs
        public static Func<bool, bool, bool> isPressed = (currentState, previousState) => currentState;
        public static Func<bool, bool, bool> isTriggered = (currentState, previousState) => currentState && !previousState;
        public static Func<bool, bool, bool> isReleased = (currentState, previousState) => !currentState && previousState;

        // get different information from click inputs
        /*public static Func<bool, bool, bool> isMousePressed = (mouseState, oldMouseState) => mouseState;
        public static Func<bool, bool, bool> isMouseTriggered = (mouseState, oldMouseState) => mouseState && !oldMouseState;
        public static Func<bool, bool, bool> isMouseReleased = (mouseState, oldMouseState) => !mouseState && oldMouseState;
        */

        // specify key to get input information from
        public static bool isKey(Inputs input, Func<bool, bool, bool> checkState)
        {
            return checkState(currentKeyState.IsKeyDown(playerKeys[input]), previousKeyState.IsKeyDown(playerKeys[input]));
        }

        public static bool isClick(ClickInputs input, Func<bool, bool, bool> checkState)
        {

            ButtonState currentClickState =
                playerClicks[input].Equals(Mouse.GetState().LeftButton) ? mouseState.LeftButton :
                playerClicks[input].Equals(Mouse.GetState().RightButton) ? mouseState.RightButton :
                ButtonState.Released;

            ButtonState previousClickState =
                playerClicks[input].Equals(Mouse.GetState().LeftButton) ? previousLeftButton :
                playerClicks[input].Equals(Mouse.GetState().RightButton) ? previousRightButton :
                ButtonState.Released;

            bool currentClick = currentClickState == ButtonState.Pressed;
            bool previousClick = previousClickState == ButtonState.Pressed;

            return checkState(currentClick, previousClick);
        }

        /*private ButtonState getClickState(MouseState mouseState, ButtonState button)
        {
            if (button.Equals(Mouse.GetState().LeftButton))
                return mouseState.LeftButton;
            if (button.Equals(Mouse.GetState().RightButton))
                return mouseState.RightButton;
            return ButtonState.Released;
        }*/

    }
}