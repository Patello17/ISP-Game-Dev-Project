using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Security;
using System.Runtime.CompilerServices;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;
using ISP_Project.Components;
using ISP_Project.Game_States;
using ISP_Project.Managers;
using ISP_Project.Tilemaps;
using ISP_Project.UI.Buttons;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using MonoGame.Extended.Content;
using static ISP_Project.Managers.InputManager;

namespace ISP_Project.Gameplay
{
    internal class Box : Actor
    {
        public enum BoxType
        {
            HORIZONTAL, VERTICAL,
            UP, DOWN, LEFT, RIGHT,
            STAR
        }
        private BoxType boxType = BoxType.RIGHT; // right by default
        public override Sprite Sprite { get; set; }
        public override Transform Transform { get; set; }
        public override Vector2 TileMapPosition { get; set; }
        Vector2 newTileMapPosition;
        Vector2 movementVector;
        
        public Box(Vector2 position, Vector2 tileMapPosition, BoxType boxType)
        {
            Transform = new Transform(position, 1f, 0f);
            TileMapPosition = tileMapPosition;
            newTileMapPosition = TileMapPosition;
            this.boxType = boxType;
        }

        public override void LoadContent(ContentManager content)
        {
            Sprite = new Sprite(null, SpriteEffects.None, 0);
            // load the correct texture
            switch (boxType)
            {
                case BoxType.HORIZONTAL:
                    Sprite.Texture = content.Load<Texture2D>("Box/Horizontal Box");
                    break;
                case BoxType.VERTICAL:
                    Sprite.Texture = content.Load<Texture2D>("Box/Vertical Box");
                    break;
                case BoxType.UP:
                    Sprite.Texture = content.Load<Texture2D>("Box/Up Box");
                    break;
                case BoxType.DOWN:
                    Sprite.Texture = content.Load<Texture2D>("Box/Down Box");
                    break;
                case BoxType.LEFT:
                    Sprite.Texture = content.Load<Texture2D>("Box/Left Box");
                    break;
                case BoxType.RIGHT:
                    Sprite.Texture = content.Load<Texture2D>("Box/Right Box");
                    break;
                case BoxType.STAR:
                    Sprite.Texture = content.Load<Texture2D>("Box/Star Box");
                    break;
            }
        }
        public override void Update(GameTime gameTime, CollisionMap collisionMap)
        {
            

            // Debug.WriteLine("Collision Map is colliding with " + newTileMapPosition + "? " + collisionMap.isColliding(newTileMapPosition));

            if (StateManager.GetCurrentState() is HubState)
            {
                // these vectors represent the position of the doorway
                if (newTileMapPosition == new Vector2(0, 5) || newTileMapPosition == new Vector2(0, 6) || newTileMapPosition == new Vector2(0, 7))
                {
                    StateManager.ChangeState(new TitleState(Globals.ContentManager));
                }
                // these vectors represent the positions right below the map board
                if (TileMapPosition == new Vector2(6, 4) || TileMapPosition == new Vector2(7, 4) ||
                    TileMapPosition == new Vector2(8, 4) || TileMapPosition == new Vector2(9, 4))
                {
                    MapButton.isClickable = true;
                }
                else { MapButton.isClickable = false; }
            }

            UpdatePosition(collisionMap);

        }

        public override void Draw(GameTime gameTime)
        {
            Globals.SpriteBatch.Draw(Sprite.Texture, Transform.Position, null, Color.White,
                Transform.Rotation, Sprite.GetSpriteOrigin(), Transform.Scale,
                Sprite.SpriteEffects, Sprite.DrawLayer);
        }
        
        public void SetNextPosition(Vector2 movementVector)
        {
            this.movementVector = movementVector;
            newTileMapPosition = TileMapPosition + this.movementVector;
            Debug.WriteLine(this.movementVector + " || " + newTileMapPosition);
        }
        public void SetTexture()
        {
            /*Sprite.Texture = textureDictionary[keyDown];
            Sprite.SpriteEffects = spriteEffectsDictionary[keyDown];*/
        }
        public void UpdatePosition(CollisionMap collisionMap)
        {
            // check for collisions (1 = solid in the tilesheet; 5 = mailbox goal)
            if (collisionMap.GetCollision(newTileMapPosition) != 1 &&
                collisionMap.GetCollision(newTileMapPosition) != 2)
            {
                // update position
                Transform.Position += movementVector * 16; // tiles are 16x16
                collisionMap.SetCollision(TileMapPosition, 0);
                TileMapPosition = newTileMapPosition;
                collisionMap.SetCollision(TileMapPosition, 3);
                Debug.WriteLine(collisionMap.GetCollision(TileMapPosition));
                // Debug.WriteLine(TileMapPosition);
            }

            movementVector = Vector2.Zero;
            newTileMapPosition = TileMapPosition;
            // collisionMap.SetCollision(TileMapPosition, 3);
        }

        public BoxType GetBoxType()
        {
            return boxType;
        }
    }
}
