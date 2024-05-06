using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ISP_Project.Components;
using ISP_Project.Managers;
using ISP_Project.Tilemaps;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace ISP_Project.Gameplay
{
    internal class Snail : Actor
    {
        Sprite sprite;
        Transform transform;
        Vector2[,] levelMap;
        public Snail()
        {

        }
        public override void LoadContent(ContentManager content)
        {
            sprite = new Sprite(null, SpriteEffects.None, 0);
            sprite.Texture = content.Load<Texture2D>("Snail");
            transform = new Transform(new Vector2(160, 208), 1f, 0f);
        }
        public override void Update(GameTime gameTime, CollisionMap collisionMap)
        {
            
            Vector2 velocity = Vector2.Zero;

            if (InputManager.isKey(InputManager.Inputs.UP, InputManager.isTriggered))
            {
                velocity = new Vector2(0, -16);
            }
            else if (InputManager.isKey(InputManager.Inputs.DOWN, InputManager.isTriggered))
            {
                velocity = new Vector2(0, 16);
            }
            else if (InputManager.isKey(InputManager.Inputs.LEFT, InputManager.isTriggered))
            {
                velocity = new Vector2(-16, 0);
            }
            else if (InputManager.isKey(InputManager.Inputs.RIGHT, InputManager.isTriggered))
            {
                velocity = new Vector2(16, 0);
            }

            transform.Position += velocity;
        }

        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(sprite.Texture, transform.Position, null, Color.White,
                transform.Rotation, sprite.GetSpriteOrigin(), transform.Scale,
                sprite.SpriteEffects, sprite.DrawLayer);
        }



        /*public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw();
        }*/

    }
}
