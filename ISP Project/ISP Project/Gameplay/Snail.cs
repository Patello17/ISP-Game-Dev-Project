using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using ISP_Project.Components;
using ISP_Project.Game_States;
using ISP_Project.Managers;
using ISP_Project.Tilemaps;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using MonoGame.Extended.Content;

namespace ISP_Project.Gameplay
{
    internal class Snail : Actor
    {
        private Texture2D frontTexture;
        private Texture2D backTexture;
        private Texture2D sideTexture;
        public override Sprite Sprite { get; set; }
        public override Transform Transform { get; set; }
        public override Vector2 TileMapPosition { get; set; }

        public Snail(Vector2 position, Vector2 tileMapPosition)
        {
            Transform = new Transform(position, 1f, 0f);
            TileMapPosition = tileMapPosition;
        }
        public override void LoadContent(ContentManager content)
        {
            Sprite = new Sprite(null, SpriteEffects.None, 0);
            sideTexture = content.Load<Texture2D>("Snail/Snail");
            frontTexture = content.Load<Texture2D>("Snail/SnailFront");
            backTexture = content.Load<Texture2D>("Snail/SnailBack");
            Sprite.Texture = sideTexture;
        }
        public override void Update(GameTime gameTime, CollisionMap collisionMap)
        {
            
            Vector2 velocity = Vector2.Zero;
            Vector2 newTileMapPosition = TileMapPosition;

            if (InputManager.isKey(InputManager.Inputs.UP, InputManager.isTriggered))
            {
                velocity = new Vector2(0, -16);
                newTileMapPosition = new Vector2(TileMapPosition.X, TileMapPosition.Y - 1);
                Sprite.Texture = backTexture;
            }
            else if (InputManager.isKey(InputManager.Inputs.DOWN, InputManager.isTriggered))
            {
                velocity = new Vector2(0, 16);
                newTileMapPosition = new Vector2(TileMapPosition.X, TileMapPosition.Y + 1);
                Sprite.Texture = frontTexture;
            }
            else if (InputManager.isKey(InputManager.Inputs.LEFT, InputManager.isTriggered))
            {
                velocity = new Vector2(-16, 0);
                newTileMapPosition = new Vector2(TileMapPosition.X - 1, TileMapPosition.Y);
                Sprite.SpriteEffects = SpriteEffects.FlipHorizontally;
                Sprite.Texture = sideTexture;
            }
            else if (InputManager.isKey(InputManager.Inputs.RIGHT, InputManager.isTriggered))
            {
                velocity = new Vector2(16, 0);
                newTileMapPosition = new Vector2(TileMapPosition.X + 1, TileMapPosition.Y);
                Sprite.SpriteEffects = SpriteEffects.None;
                Sprite.Texture = sideTexture;
            }

            // Debug.WriteLine("Collision Map is colliding with " + newTileMapPosition + "? " + collisionMap.isColliding(newTileMapPosition));

            if (StateManager.GetCurrentState() is HubState)
            {
                // these vectors represent the position of the doorway
                if (newTileMapPosition == new Vector2(0, 5) || newTileMapPosition == new Vector2(0, 6))
                {
                    StateManager.ChangeState(new TitleState(Globals.ContentManager));
                }
            }

            if (collisionMap.GetCollision(newTileMapPosition) == 0)
            {
                Transform.Position += velocity;
                TileMapPosition = newTileMapPosition;
            }

            
            
        }

        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(Sprite.Texture, Transform.Position, null, Color.White,
                Transform.Rotation, Sprite.GetSpriteOrigin(), Transform.Scale,
                Sprite.SpriteEffects, Sprite.DrawLayer);
        }



        /*public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw();
        }*/

    }
}
