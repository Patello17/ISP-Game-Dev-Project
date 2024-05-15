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
        private bool isSunken = false;
        private bool sinkLock = false;
        public bool CanMove { get; set; }
        public override Sprite Sprite { get; set; }
        public override Transform Transform { get; set; }
        private Vector2 tileMapPosition;
        public override Vector2 TileMapPosition
        {
            get { return tileMapPosition; }
            set
            {
                tileMapPosition = value;
                /*var centeredTileMapPosition = tileMapPosition - new Vector2(20, 11);
                Transform.Position = new Vector2(
                    (int)(WindowManager.GetMainWindowCenter().X + (centeredTileMapPosition.X * 16)) + 8,
                    (int)(WindowManager.GetMainWindowCenter().Y + (centeredTileMapPosition.Y * (180 / 11))) + 8);*/
            }
        }
        Vector2 newTileMapPosition;
        Vector2 movementVector;
        private bool isSliding = false;

        public Box(Vector2 tileMapPosition, BoxType boxType)
        {
            Transform = new Transform(Vector2.Zero, 1f, 0f);
            TileMapPosition = tileMapPosition;
            var centeredTileMapPosition = tileMapPosition - new Vector2(20, 11);
            Transform.Position = new Vector2(
                    (int)(WindowManager.GetMainWindowCenter().X + (centeredTileMapPosition.X * 16)) + 8,
                    (int)(WindowManager.GetMainWindowCenter().Y + (centeredTileMapPosition.Y * (180 / 11))) + 8);
            newTileMapPosition = TileMapPosition;
            this.boxType = boxType;
        }

        public override void LoadContent()
        {
            Sprite = new Sprite(null, SpriteEffects.None, 1);
            // load the correct texture
            switch (boxType)
            {
                case BoxType.HORIZONTAL:
                    Sprite.Texture = Globals.ContentManager.Load<Texture2D>("Box/Horizontal Box");
                    break;
                case BoxType.VERTICAL:
                    Sprite.Texture = Globals.ContentManager.Load<Texture2D>("Box/Vertical Box");
                    break;
                case BoxType.UP:
                    Sprite.Texture = Globals.ContentManager.Load<Texture2D>("Box/Up Box");
                    break;
                case BoxType.DOWN:
                    Sprite.Texture = Globals.ContentManager.Load<Texture2D>("Box/Down Box");
                    break;
                case BoxType.LEFT:
                    Sprite.Texture = Globals.ContentManager.Load<Texture2D>("Box/Left Box");
                    break;
                case BoxType.RIGHT:
                    Sprite.Texture = Globals.ContentManager.Load<Texture2D>("Box/Right Box");
                    break;
                case BoxType.STAR:
                    Sprite.Texture = Globals.ContentManager.Load<Texture2D>("Box/Star Box");
                    break;
            }
        }
        public override void Update(GameTime gameTime, CollisionMap collisionMap)
        {
            SetTexture();

            // slide
            Slide(GetNextPosition());

            // UpdatePosition(collisionMap);
        }

        public override void Draw()
        {
            Globals.SpriteBatch.Draw(Sprite.Texture, Transform.Position, null, Sprite.Color,
                Transform.Rotation, Sprite.GetSpriteOrigin(), Transform.Scale,
                Sprite.SpriteEffects, Sprite.DrawLayer);
        }
        
        public bool GetSunkState()
        {
            return isSunken;
        }
        public Vector2 GetMovementVector()
        {
            return movementVector;
        }
        public Vector2 GetCurrentPosition()
        {
            return TileMapPosition;
        }
        public Vector2 GetNextPosition()
        {
            return newTileMapPosition;
        }
        public void SetNextPosition(Vector2 movementVector, bool isBoxPushing)
        {
            switch (boxType)
            {
                case BoxType.HORIZONTAL:
                    if (movementVector == new Vector2(1, 0) || movementVector == new Vector2(-1, 0))
                        this.movementVector = movementVector;
                    break;
                case BoxType.VERTICAL:
                    if (movementVector == new Vector2(0, -1) || movementVector == new Vector2(0, 1))
                        this.movementVector = movementVector;
                    break;
                case BoxType.UP:
                    if (movementVector == new Vector2(0, -1))
                        this.movementVector = movementVector;
                    break;
                case BoxType.DOWN:
                    if (movementVector == new Vector2(0, 1))
                        this.movementVector = movementVector;
                    break;
                case BoxType.LEFT:
                    if (movementVector == new Vector2(-1, 0))
                        this.movementVector = movementVector;
                    break;
                case BoxType.RIGHT:
                    if (movementVector == new Vector2(1, 0))
                        this.movementVector = movementVector;
                    break;
                case BoxType.STAR:
                    this.movementVector = movementVector;
                    break;
            }
            if (isBoxPushing)
            {
                this.movementVector = movementVector;
                // Debug.WriteLine("BOX IS PUSHING BOX");
            }
                

            newTileMapPosition = TileMapPosition + this.movementVector;
            // Debug.WriteLine(this.movementVector + " || " + newTileMapPosition);
        }
        public void SetTexture()
        {
            if (isSunken)
            {
                Sprite.Color = new Color(127, 174, 198);
                Sprite.DrawLayer = 0.2f;
                if (!sinkLock)
                {
                    // newTileMapPosition += new Vector2(0, 7 * 11 / 180);
                    sinkLock = true;
                }
            }
            else 
            {
                Sprite.Color = Color.White;
                sinkLock = false;
            }
            /*Sprite.Texture = textureDictionary[keyDown];
            Sprite.SpriteEffects = spriteEffectsDictionary[keyDown];*/
        }
        public void UpdatePosition(CollisionMap collisionMap)
        {
            // check for collisions (1 = solid in the tilesheet; 2 = water; 5 = mailbox goal)
            if (collisionMap.GetCollision(newTileMapPosition) == 2)
            {
                // update position and sink
                isSunken = true;
                CanMove = false;
                // Transform.Position += movementVector * 16; // tiles are 16x16
                collisionMap.SetCollision(TileMapPosition, 0);
                TileMapPosition = newTileMapPosition;
                collisionMap.SetCollision(TileMapPosition, 0); // box acts as a path when sunk!
            }
            else if (collisionMap.GetCollision(newTileMapPosition) != 1 &&
                collisionMap.GetCollision(newTileMapPosition) != 3 &&
                !isSunken)
            {
                // update position
                CanMove = true;
                // Transform.Position += movementVector * 16; // tiles are 16x16
                
                collisionMap.SetCollision(TileMapPosition, 0);
                TileMapPosition = newTileMapPosition;
                collisionMap.SetCollision(TileMapPosition, 3);
                // Debug.WriteLine(collisionMap.GetCollision(TileMapPosition));
                
            }

            movementVector = Vector2.Zero;
            newTileMapPosition = TileMapPosition;

            // collisionMap.SetCollision(TileMapPosition, 3);
        }

        public BoxType GetBoxType()
        {
            return boxType;
        }

        private void Slide(Vector2 targetPosition)
        {
            var centeredTileMapPosition = targetPosition - new Vector2(20, 11);
            targetPosition = new Vector2(
                (int)(WindowManager.GetMainWindowCenter().X + (centeredTileMapPosition.X * 16)) + 8,
                (int)(WindowManager.GetMainWindowCenter().Y + (centeredTileMapPosition.Y * (180 / 11))) + 8);
            /*targetPosition = new Vector2(targetPosition.X * 16 + 8, targetPosition.Y * (180 / 11) + 8);*/
            var clampBound = 1f; // snap to position when within 1 pixels

            Vector2 newPosition = Vector2.Lerp(Transform.Position, targetPosition, 0.3f);
            if (Math.Abs(newPosition.X - targetPosition.X) <= clampBound &&
                    Math.Abs(newPosition.Y - targetPosition.Y) <= clampBound)
            {
                newPosition = targetPosition;
                isSliding = false;
            }
            Transform.Position = newPosition;
        }
    }
}
