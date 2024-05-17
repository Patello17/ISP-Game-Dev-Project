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
        // define box types
        public enum BoxType
        {
            HORIZONTAL, VERTICAL,
            UP, DOWN, LEFT, RIGHT,
            STAR
        }
        private BoxType boxType;

        // create sinking logic variables
        private bool isSunken = false;
        private bool sinkLock = false;

        // reference Actor properties
        public override Sprite Sprite { get; set; }
        public override Transform Transform { get; set; }
        public override Vector2 TileMapPosition { get; set; }

        // create movement-related properties and fields
        public override List<Vector2> PastPositions { get; set; }
        Vector2 nextTileMapPosition;
        Vector2 movementVector;

        public Box(Vector2 tileMapPosition, BoxType boxType)
        {
            Transform = new Transform(Vector2.Zero, 1f, 0f);
            TileMapPosition = tileMapPosition;
            
            // convert tile position coordinates to screen resolution coordinates
            var centeredTileMapPosition = tileMapPosition - new Vector2(20, 11);
            Transform.Position = new Vector2(
                    (int)(WindowManager.GetMainWindowCenter().X + (centeredTileMapPosition.X * 16)) + 8,
                    (int)(WindowManager.GetMainWindowCenter().Y + (centeredTileMapPosition.Y * (180 / 11))) + 8);

            PastPositions = new List<Vector2>();
            nextTileMapPosition = TileMapPosition;
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

        public override void Update()
        {
            SetTexture();
            Slide(GetNextPosition());
        }

        public override void Draw()
        {
            Globals.SpriteBatch.Draw(Sprite.Texture, Transform.Position, null, Sprite.Color,
                Transform.Rotation, Sprite.GetSpriteOrigin(), Transform.Scale,
                Sprite.SpriteEffects, Sprite.DrawLayer);
        }
        
        /// <summary>
        /// Gets whether this box has sunk or not.
        /// </summary>
        /// <returns></returns>
        public bool GetSunkState()
        {
            return isSunken;
        }

        /// <summary>
        /// Gets this box's movement vector.
        /// </summary>
        /// <returns></returns>
        public Vector2 GetMovementVector()
        {
            return movementVector;
        }

        /// <summary>
        /// Gets this box's current position.
        /// </summary>
        /// <returns></returns>
        public Vector2 GetCurrentPosition()
        {
            return TileMapPosition;
        }

        /// <summary>
        /// Gets this box's next position.
        /// </summary>
        /// <returns></returns>
        public Vector2 GetNextPosition()
        {
            return nextTileMapPosition;
        }

        /// <summary>
        /// Sets next position if movement vector is valid.
        /// </summary>
        /// <param name="movementVector"></param>
        /// <param name="isBoxPushing"></param>
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
            }

            nextTileMapPosition = TileMapPosition + this.movementVector;
        }

        /// <summary>
        /// Sets this box's texture.
        /// </summary>
        public void SetTexture()
        {
            if (isSunken)
            {
                Sprite.Color = new Color(127, 174, 198);
                Sprite.DrawLayer = 0.2f;
            }
            else 
            {
                Sprite.Color = Color.White;
            }
        }

        public override void UpdatePosition(CollisionMap collisionMap)
        {
            // check for collisions
            if (collisionMap.GetCollision(nextTileMapPosition) == 2) // water
            {
                // play sound effect
                AudioManager.PlaySoundEffect("Box Splash");

                // sink
                isSunken = true;

                // update position
                collisionMap.SetCollision(TileMapPosition, 0);
                TileMapPosition = nextTileMapPosition;
                collisionMap.SetCollision(TileMapPosition, 0); // box acts as a path when sunk!
            }
            else if (collisionMap.GetCollision(nextTileMapPosition) != 1 && // solids
                collisionMap.GetCollision(nextTileMapPosition) != 3 && // other boxes
                !isSunken)
            {
                // update position
                collisionMap.SetCollision(TileMapPosition, 0);
                TileMapPosition = nextTileMapPosition;
                collisionMap.SetCollision(TileMapPosition, 3);
            }

            movementVector = Vector2.Zero;
            nextTileMapPosition = TileMapPosition;
        }

        /// <summary>
        /// Gets what type of box this box is.
        /// </summary>
        /// <returns></returns>
        public BoxType GetBoxType()
        {
            return boxType;
        }

        /// <summary>
        /// Slides the box to its next position.
        /// </summary>
        /// <param name="targetPosition"></param>
        private void Slide(Vector2 targetPosition)
        {
            // convert tile position coordinates to screen resolution coordinates
            var centeredTileMapPosition = targetPosition - new Vector2(20, 11);
            targetPosition = new Vector2(
                (int)(WindowManager.GetMainWindowCenter().X + (centeredTileMapPosition.X * 16)) + 8,
                (int)(WindowManager.GetMainWindowCenter().Y + (centeredTileMapPosition.Y * (180 / 11))) + 8);

            var clampBound = 1f; // snap to position when within 1 pixel

            // smoothly transition from current tile to next tile.
            Vector2 newPosition = Vector2.Lerp(Transform.Position, targetPosition, 0.3f);
            if (Math.Abs(newPosition.X - targetPosition.X) <= clampBound &&
                    Math.Abs(newPosition.Y - targetPosition.Y) <= clampBound)
            {
                newPosition = targetPosition;
            }
            Transform.Position = newPosition;
        }
    }
}
