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
    internal class HorizontalSlider
    {
        public Sprite BarSprite { get; set; } = new Sprite(null, SpriteEffects.None, 0);
        public Sprite SliderSprite
        {
            get { return Slider.Sprite; }
            set { Slider.Sprite = value; } 
        }
        public SongSliderButton Slider { get; set; }
        private float sliderValue;
        public Vector2 BarPosition { get; set; }
        private float buttonScale;
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


        public HorizontalSlider(Texture2D barTexture, Texture2D sliderTexture, SpriteFont buttonFont, float buttonScale, float fontScale, float value)
        {
            BarSprite.Texture = barTexture;
            Slider = new SongSliderButton(sliderTexture, buttonFont, buttonScale, fontScale);
            Slider.Position = BarPosition;
            this.buttonScale = buttonScale;
            sliderValue = value;
        }
        public void Update(GameTime gameTime)
        {
            Slider.Update(gameTime);
            
            if (InputManager.cursorRectangle.Intersects(Slider.Rectangle) &&
                InputManager.isClick(InputManager.ClickInputs.INTERACT, InputManager.isPressed))
            {
                Slider.Position = new Vector2(InputManager.cursorRectangle.X + InputManager.cursorRectangle.Width / 2, Slider.Position.Y);
            }
            Debug.WriteLine(BarPosition);
        }
        public void Draw(GameTime gameTime)
        {
            Globals.SpriteBatch.Draw(BarSprite.Texture, BarPosition, null, Color.White, 0f, BarSprite.GetSpriteOrigin(), buttonScale, BarSprite.SpriteEffects, BarSprite.DrawLayer);
            Slider.Draw(gameTime);
        }
    }
}
