using ISP_Project.Components;
using ISP_Project.Managers;
using ISP_Project.UI.Buttons;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection.PortableExecutable;
using System.Text;
using System.Threading.Tasks;

namespace ISP_Project.UI.Sliders
{
    public abstract class HorizontalSlider
    {
        public Sprite BarSprite { get; set; } = new Sprite(null, SpriteEffects.None, 0);
        public Sprite SliderSprite { get; set; }
        public Transform SliderTransform { get; set; }
        public float Value { get; set; }
        public Vector2 BarPosition { get; set; }
        private float buttonScale;
        private float volumeIncrement = 0.05f;
        /*public Vector2 Position
        {
            get
            {
                // Why is WindowManager.GetRenderPosition() needed?
                return (position - SliderSprite.GetSpriteOrigin()) * WindowManager.GetRenderScale() + WindowManager.GetRenderPosition();
            }
            set
            {
                position = value;
            }
        }
        private float buttonScale;
        public float ButtonScale
        {
            get
            {
                return buttonScale * WindowManager.GetRenderScale();
            }
            set
            {
                buttonScale = value;
            }
        }
        public Rectangle Rectangle
        {
            get
            {
                return new Rectangle((int)(Position.X), (int)(Position.Y),
                    (int)(SliderSprite.Texture.Width * ButtonScale), (int)(SliderSprite.Texture.Height * ButtonScale));
            }
        }
        public bool ForceShade { get; set; } = false;
        private bool isHovering;*/


        public HorizontalSlider(Texture2D barTexture, Texture2D sliderTexture, SpriteFont buttonFont, float buttonScale, float fontScale, Vector2 position, float value)
        {
            BarSprite.Texture = barTexture;
            SliderSprite = new Sprite(sliderTexture, SpriteEffects.None, 0.8f);
            BarPosition = position;
            SliderTransform = new Transform(BarPosition, buttonScale, 0f);
            this.buttonScale = buttonScale;
            Value = value;
        }

        /// <summary>
        /// Updates this Horizontal Slider.
        /// </summary>
        public void Update()
        {

            /*if (InputManager.cursorRectangle.Intersects(Slider.Rectangle) &&
                InputManager.isClick(InputManager.ClickInputs.INTERACT, InputManager.isPressed))
            {
                Slider.Position = new Vector2(InputManager.cursorRectangle.X + InputManager.cursorRectangle.Width / 2, Slider.Position.Y);
            }*/

            var slideIncrement = BarSprite.Texture.Width * buttonScale / (1 / volumeIncrement);
            if (InputManager.isKey(InputManager.Inputs.LEFT, InputManager.isTriggered) && Value > 0)
            {
                AudioManager.PlaySoundEffect("Scroll");
                SliderTransform.Position = new Vector2(SliderTransform.Position.X - slideIncrement, SliderTransform.Position.Y);
                Value -= volumeIncrement;
                SetVolume();
            }
            if (InputManager.isKey(InputManager.Inputs.RIGHT, InputManager.isTriggered) && Value < 1)
            {
                AudioManager.PlaySoundEffect("Scroll");
                SliderTransform.Position = new Vector2(SliderTransform.Position.X + slideIncrement, SliderTransform.Position.Y);
                Value += volumeIncrement;
                SetVolume();
            }
            // Debug.WriteLine(BarPosition);
        }

        /// <summary>
        /// Draws this Horizontal Slider.
        /// </summary>
        public void Draw()
        {
            Globals.SpriteBatch.Draw(BarSprite.Texture, BarPosition, null, Color.White, 0f, BarSprite.GetSpriteOrigin(), 
                buttonScale, BarSprite.SpriteEffects, 0.6f);
            Globals.SpriteBatch.Draw(SliderSprite.Texture, SliderTransform.Position + new Vector2(BarSprite.Texture.Width / 2, 0), null, SliderSprite.Color, 0f, SliderSprite.GetSpriteOrigin(), 
                buttonScale, SliderSprite.SpriteEffects, 0.8f);
        }

        public abstract void SetVolume();
    }
}
