﻿using ISP_Project.Game_States;
using ISP_Project.Game_States.Levels;
using ISP_Project.Managers;
using ISP_Project.Screen_Management.Transitions;
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
        public static bool isClickable = false;

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
            if (isClickable)
            {
                AudioManager.PlaySoundEffect("Envelope");
                StateManager.ChangeState(new LevelSelectionState(), Transitions.BlackFade, 0.1f);
            }
            else
            {
                isClickable = false;
            }
        }
    }

    // create Level One Select Button
    public class LevelOneSelectButton : Button
    {
        private Texture2D buttonTexture;
        private SpriteFont buttonFont;
        private float buttonScale;
        private float fontScale;
        public static bool isClickable = false;

        public LevelOneSelectButton(Texture2D texture, SpriteFont font, float buttonScale, float fontScale) : base(texture, font, buttonScale, fontScale)
        {
            buttonTexture = texture;
            buttonFont = font;
            this.buttonScale = buttonScale;
            this.fontScale = buttonScale;
        }

        public override void TriggerEvent()
        {
            // change Game State here!
            if (isClickable)
            {
                AudioManager.PlaySoundEffect("Button Press");
                StateManager.ChangeState(new LevelOneState(), Transitions.BlackFade, 0.1f);
            }
            else
            {
                isClickable = false;
            }
        }
    }
    
    // create Hub Return Button
    public class HubReturnButton : Button
    {
        private Texture2D buttonTexture;
        private SpriteFont buttonFont;
        private float buttonScale;
        private float fontScale;
        public HubReturnButton(Texture2D texture, SpriteFont font, float buttonScale, float fontScale) : base(texture, font, buttonScale, fontScale)
        {
            buttonTexture = texture;
            buttonFont = font;
            this.buttonScale = buttonScale;
            this.fontScale = buttonScale;
        }

        public override void TriggerEvent()
        {
            AudioManager.PlaySoundEffect("Button Press");
            StateManager.ChangeState(StateManager.GetRecentState(new HubState()), Transitions.BlackFade, 0.1f);
        }
    }
}
