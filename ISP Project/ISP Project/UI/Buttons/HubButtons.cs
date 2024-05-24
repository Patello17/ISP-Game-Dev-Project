using Autofac.Core.Activators;
using ISP_Project.Game_States;
using ISP_Project.Game_States.Letters;
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

    // create Level Two Select Button
    public class LevelTwoSelectButton : Button
    {
        private Texture2D buttonTexture;
        private SpriteFont buttonFont;
        private float buttonScale;
        private float fontScale;
        public static bool isClickable = false;

        public LevelTwoSelectButton(Texture2D texture, SpriteFont font, float buttonScale, float fontScale) : base(texture, font, buttonScale, fontScale)
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
                StateManager.ChangeState(new LevelTwoState(), Transitions.BlackFade, 0.1f);
            }
            else
            {
                isClickable = false;
            }
        }
    }

    // create Level Three Select Button
    public class LevelThreeSelectButton : Button
    {
        private Texture2D buttonTexture;
        private SpriteFont buttonFont;
        private float buttonScale;
        private float fontScale;
        public static bool isClickable = false;

        public LevelThreeSelectButton(Texture2D texture, SpriteFont font, float buttonScale, float fontScale) : base(texture, font, buttonScale, fontScale)
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
                StateManager.ChangeState(new LevelThreeState(), Transitions.BlackFade, 0.1f);
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

    // create Envelope 1
    public class EnvelopeOneButton : Envelope
    {
        private Texture2D buttonTexture;
        private SpriteFont buttonFont;
        private float buttonScale;
        private float fontScale;

        public EnvelopeOneButton(Texture2D texture, SpriteFont font, float buttonScale, float fontScale) : base(texture, font, buttonScale, fontScale)
        {
            buttonTexture = texture;
            buttonFont = font;
            this.buttonScale = buttonScale;
            this.fontScale = buttonScale;
        }

        public override void TriggerEvent()
        {
            AudioManager.PlaySoundEffect("Envelope");
            Opened = true;
            var newSave = new ReadEnvelopesFile
            {
                ReadOne = true,
                ReadTwo = SaveManager.LoadReadEnvelopes().ReadTwo,
                ReadThree = SaveManager.LoadReadEnvelopes().ReadThree,
                ReadFour = SaveManager.LoadReadEnvelopes().ReadFour
            };
            SaveManager.SaveReadEnvelopes(newSave);
            StateManager.ChangeState(new LetterOne(), Transitions.EnvelopeOpen, 1f);
        }
    }

    // create Envelope 2
    public class EnvelopeTwoButton : Envelope
    {
        private Texture2D buttonTexture;
        private SpriteFont buttonFont;
        private float buttonScale;
        private float fontScale;

        public EnvelopeTwoButton(Texture2D texture, SpriteFont font, float buttonScale, float fontScale) : base(texture, font, buttonScale, fontScale)
        {
            buttonTexture = texture;
            buttonFont = font;
            this.buttonScale = buttonScale;
            this.fontScale = buttonScale;
        }

        public override void TriggerEvent()
        {
            AudioManager.PlaySoundEffect("Envelope");
            Opened = true;
            var newSave = new ReadEnvelopesFile
            {
                ReadOne = SaveManager.LoadReadEnvelopes().ReadOne,
                ReadTwo = true,
                ReadThree = SaveManager.LoadReadEnvelopes().ReadThree,
                ReadFour = SaveManager.LoadReadEnvelopes().ReadFour
            };
            SaveManager.SaveReadEnvelopes(newSave);
            StateManager.ChangeState(new LetterTwo(), Transitions.EnvelopeOpen, 1f);
        }
    }

    // create Envelope 3
    public class EnvelopeThreeButton : Envelope
    {
        private Texture2D buttonTexture;
        private SpriteFont buttonFont;
        private float buttonScale;
        private float fontScale;

        public EnvelopeThreeButton(Texture2D texture, SpriteFont font, float buttonScale, float fontScale) : base(texture, font, buttonScale, fontScale)
        {
            buttonTexture = texture;
            buttonFont = font;
            this.buttonScale = buttonScale;
            this.fontScale = buttonScale;
        }

        public override void TriggerEvent()
        {
            AudioManager.PlaySoundEffect("Envelope");
            Opened = true;
            var newSave = new ReadEnvelopesFile
            {
                ReadOne = SaveManager.LoadReadEnvelopes().ReadOne,
                ReadTwo = SaveManager.LoadReadEnvelopes().ReadTwo,
                ReadThree = true,
                ReadFour = SaveManager.LoadReadEnvelopes().ReadFour
            };
            SaveManager.SaveReadEnvelopes(newSave);
            StateManager.ChangeState(new LetterThree(), Transitions.EnvelopeOpen, 1f);
        }
    }

    // create Envelope 4
    public class EnvelopeFourButton : Envelope
    {
        private Texture2D buttonTexture;
        private SpriteFont buttonFont;
        private float buttonScale;
        private float fontScale;

        public EnvelopeFourButton(Texture2D texture, SpriteFont font, float buttonScale, float fontScale) : base(texture, font, buttonScale, fontScale)
        {
            buttonTexture = texture;
            buttonFont = font;
            this.buttonScale = buttonScale;
            this.fontScale = buttonScale;
        }

        public override void TriggerEvent()
        {
            AudioManager.PlaySoundEffect("Envelope");
            Opened = true;
            var newSave = new ReadEnvelopesFile
            {
                ReadOne = SaveManager.LoadReadEnvelopes().ReadOne,
                ReadTwo = SaveManager.LoadReadEnvelopes().ReadTwo,
                ReadThree = SaveManager.LoadReadEnvelopes().ReadThree,
                ReadFour = true
            };
            SaveManager.SaveReadEnvelopes(newSave);
            StateManager.ChangeState(new LetterFour(), Transitions.EnvelopeOpen, 1f);
        }
    }
}
