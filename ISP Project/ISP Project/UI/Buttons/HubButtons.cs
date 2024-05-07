﻿using ISP_Project.Game_States;
using ISP_Project.Managers;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ISP_Project.UI.Buttons
{
    // create Map Button
    public class MapButton : Button
    {
        private Texture2D buttonTexture;
        private SpriteFont buttonFont;
        private float buttonScale;
        private float fontScale;
        public static bool isMapUsable = false;

        public MapButton(Texture2D texture, SpriteFont font, float buttonScale, float fontScale) : base(texture, font, buttonScale, fontScale)
        {
            buttonTexture = texture;
            buttonFont = font;
            this.buttonScale = buttonScale;
            this.fontScale = buttonScale;
        }

        public override void TriggerEvent()
        {
            // change Game State here!
            if (isMapUsable)
            {
                StateManager.ChangeState(new LevelSelectionState(Globals.ContentManager));
            }
        }
    }
}