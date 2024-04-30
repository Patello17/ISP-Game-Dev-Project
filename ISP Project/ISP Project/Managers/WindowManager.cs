using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;

namespace ISP_Project.Managers
{
    // create Window class
    public class Window
    {
        private Vector2 windowSize;
        public Window(int x, int y)
        {
            setWindowSize(x, y);
        }
        
        public void setWindowSize(int x, int y)
        {
            windowSize.X = x;
            windowSize.Y = y;
        }
        public void updateWindowSize(GraphicsDeviceManager graphics)
        {
            graphics.PreferredBackBufferWidth = getIntWindowSizeX();
            graphics.PreferredBackBufferHeight = getIntWindowSizeY();
            graphics.ApplyChanges();
        }
        public Vector2 getWindowSize() { return windowSize; }
        public int getIntWindowSizeX() { return (int)windowSize.X; }
        public int getIntWindowSizeY() { return (int)windowSize.Y; }
    }

    public class WindowManager
    {

        // declare WindowManager variables
        private static Window window;

        // unused constructor
        public WindowManager() { }

        // singleton pattern
        public static Window getWindow()
        {
            if (window == null)
                window = new Window(1600, 900); // default screen size
            return window;
        }

    }
}
