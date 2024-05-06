﻿using Microsoft.Xna.Framework.Graphics;
using ISP_Project.Managers;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.Design;
using ISP_Project.Game_States;

namespace ISP_Project.UI.Buttons
{
    public class GameplayButtons
    {
        // create Audio Button
        public class PauseButton : Button
        {
            private Texture2D buttonTexture;
            private SpriteFont buttonFont;
            private float buttonScale;
            private float fontScale;

            public PauseButton(Texture2D texture, SpriteFont font, float buttonScale, float fontScale) : base(texture, font, buttonScale, fontScale)
            {
                buttonTexture = texture;
                buttonFont = font;
                this.buttonScale = buttonScale;
                this.fontScale = buttonScale;
            }

            public override void TriggerEvent()
            {
                // change Game State here!
                StateManager.ChangeState(new MenuState(Globals.ContentManager));
            }
        }
    }
}