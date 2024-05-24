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
            HLONGUP, HLONGDOWN, HLONGLEFT, HLONGRIGHT,
            VLONGUP, VLONGDOWN, VLONGLEFT, VLONGRIGHT,
            STAR
        }
        private BoxType boxType;

        // create sinking logic variables
        public bool IsSunken { get; set; } = false;
        private bool sinkLock = false;

        // reference Actor properties
        public override Sprite Sprite { get; set; }
        public override Transform Transform { get; set; }
        public override Vector2 TileMapPosition { get; set; }

        // create movement-related properties and fields
        public override List<Vector2> PastPositions { get; set; }
        public List<bool> PastSinkState { get; set; }
        public bool IsColliding { get; set; }
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
            PastPositions = new List<Vector2>() { TileMapPosition };
            PastSinkState = new List<bool>() { IsSunken };
            nextTileMapPosition = TileMapPosition;
            this.boxType = boxType;
        }

        public override void LoadContent()
        {
            Sprite = new Sprite(null, SpriteEffects.None, 0.8f);
            Sprite.Color = Color.White;

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
                case BoxType.HLONGUP:
                    Sprite.Texture = Globals.ContentManager.Load<Texture2D>("Box/H Long Box Up");
                    break;
                case BoxType.HLONGDOWN:
                    Sprite.Texture = Globals.ContentManager.Load<Texture2D>("Box/H Long Box Down");
                    break;
                case BoxType.HLONGLEFT:
                    Sprite.Texture = Globals.ContentManager.Load<Texture2D>("Box/H Long Box Left");
                    break;
                case BoxType.HLONGRIGHT:
                    Sprite.Texture = Globals.ContentManager.Load<Texture2D>("Box/H Long Box Right");
                    break;
                case BoxType.VLONGUP:
                    Sprite.Texture = Globals.ContentManager.Load<Texture2D>("Box/V Long Box Up");
                    break;
                case BoxType.VLONGDOWN:
                    Sprite.Texture = Globals.ContentManager.Load<Texture2D>("Box/V Long Box Down");
                    break;
                case BoxType.VLONGLEFT:
                    Sprite.Texture = Globals.ContentManager.Load<Texture2D>("Box/V Long Box Left");
                    break;
                case BoxType.VLONGRIGHT:
                    Sprite.Texture = Globals.ContentManager.Load<Texture2D>("Box/V Long Box Right");
                    break;
                case BoxType.STAR:
                    Sprite.Texture = Globals.ContentManager.Load<Texture2D>("Box/Star Box");
                    break;
            }
        }

        public override void Update()
        {
            // reset movement variables
            movementVector = Vector2.Zero;
            nextTileMapPosition = TileMapPosition;

            SetTexture();
            Slide(GetNextPosition());
        }

        public override void Draw()
        {
            // draw the correct texture
            if (boxType == BoxType.HLONGUP || boxType == BoxType.HLONGDOWN || boxType == BoxType.HLONGLEFT || boxType == BoxType.HLONGRIGHT)
            {
                Globals.SpriteBatch.Draw(Sprite.Texture, Transform.Position, null, Sprite.Color,
                Transform.Rotation, Sprite.GetSpriteOrigin() - new Vector2(8, 0), Transform.Scale,
                Sprite.SpriteEffects, Sprite.DrawLayer);
            }
            else if (boxType == BoxType.VLONGUP || boxType == BoxType.VLONGDOWN || boxType == BoxType.VLONGLEFT || boxType == BoxType.VLONGRIGHT)
            {
                Globals.SpriteBatch.Draw(Sprite.Texture, Transform.Position, null, Sprite.Color,
                Transform.Rotation, Sprite.GetSpriteOrigin() - new Vector2(0, 8), Transform.Scale,
                Sprite.SpriteEffects, Sprite.DrawLayer);
            }
            else
            {
                Globals.SpriteBatch.Draw(Sprite.Texture, Transform.Position, null, Sprite.Color,
                Transform.Rotation, Sprite.GetSpriteOrigin(), Transform.Scale,
                Sprite.SpriteEffects, Sprite.DrawLayer);
            }
        }
        
        /// <summary>
        /// Gets whether this box has sunk or not.
        /// </summary>
        /// <returns></returns>
        public bool GetSinkState()
        {
            return IsSunken;
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
                case BoxType.HLONGUP:
                    if (movementVector == new Vector2(0, -1))
                        this.movementVector = movementVector;
                    break;
                case BoxType.HLONGDOWN:
                    if (movementVector == new Vector2(0, 1))
                        this.movementVector = movementVector;
                    break;
                case BoxType.HLONGLEFT:
                    if (movementVector == new Vector2(-1, 0))
                        this.movementVector = movementVector;
                    break;
                case BoxType.HLONGRIGHT:
                    if (movementVector == new Vector2(1, 0))
                        this.movementVector = movementVector;
                    break;
                case BoxType.VLONGUP:
                    if (movementVector == new Vector2(0, -1))
                        this.movementVector = movementVector;
                    break;
                case BoxType.VLONGDOWN:
                    if (movementVector == new Vector2(0, 1))
                        this.movementVector = movementVector;
                    break;
                case BoxType.VLONGLEFT:
                    if (movementVector == new Vector2(-1, 0))
                        this.movementVector = movementVector;
                    break;
                case BoxType.VLONGRIGHT:
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
            if (IsSunken)
            {
                Sprite.Color = new Color(127, 174, 198);
                Sprite.DrawLayer = 0.2f;
            }
            else 
            {
                Sprite.Color = Color.White;
                Sprite.DrawLayer = 0.8f;
            }
        }

        public override void UpdatePosition(CollisionMap collisionMap)
        {
            // check for collisions
            /*if (boxType == BoxType.VLONGUP || boxType == BoxType.VLONGDOWN || boxType == BoxType.VLONGLEFT || boxType == BoxType.VLONGRIGHT)
            {
                switch (collisionMap.GetCollision(nextTileMapPosition) && collisionMap.GetCollision(nextTileMapPosition + new Vector2(0, 1)))
                {
                    case 1: // solids
                        IsColliding = true;
                        collisionMap.SetCollision(TileMapPosition, 3);
                        collisionMap.SetCollision(TileMapPosition + new Vector2(0, 1), 3);
                        break;
                    case 2: // water
                        IsColliding = false;
                        IsSunken = true;
                        collisionMap.SetCollision(TileMapPosition, 0);
                        collisionMap.SetCollision(TileMapPosition + new Vector2(0, 1), 0);
                        TileMapPosition = nextTileMapPosition;
                        collisionMap.SetCollision(TileMapPosition, 0); // box acts as path when sunk!
                        collisionMap.SetCollision(TileMapPosition + new Vector2(0, 1), 0);
                        AudioManager.PlaySoundEffect("Box Splash");
                        break;
                    case 3: // boxes
                        IsColliding = true;
                        collisionMap.SetCollision(TileMapPosition, 3);
                        collisionMap.SetCollision(TileMapPosition + new Vector2(0, 1), 3);
                        break;
                    case 5: // mailbox
                        IsColliding = true;
                        collisionMap.SetCollision(TileMapPosition, 0);
                        collisionMap.SetCollision(TileMapPosition + new Vector2(0, 1), 0);
                        TileMapPosition = nextTileMapPosition;
                        collisionMap.SetCollision(TileMapPosition, 3);
                        collisionMap.SetCollision(TileMapPosition + new Vector2(0, 1), 3);
                        break;
                    default:
                        if (!IsSunken)
                        {
                            IsColliding = false;
                            collisionMap.SetCollision(TileMapPosition, 0);
                            collisionMap.SetCollision(TileMapPosition + new Vector2(0, 1), 0);
                            TileMapPosition = nextTileMapPosition;
                            collisionMap.SetCollision(TileMapPosition + new Vector2(0, 1), 3);
                        }
                        break;
                }
            }
            else
            {
                switch (collisionMap.GetCollision(nextTileMapPosition))
                {
                    case 1: // solids
                        IsColliding = true;
                        collisionMap.SetCollision(TileMapPosition, 3);
                        break;
                    case 2: // water
                        IsColliding = false;
                        IsSunken = true;
                        collisionMap.SetCollision(TileMapPosition, 0);
                        TileMapPosition = nextTileMapPosition;
                        collisionMap.SetCollision(TileMapPosition, 0); // box acts as path when sunk!
                        AudioManager.PlaySoundEffect("Box Splash");
                        break;
                    case 3: // boxes
                        IsColliding = true;
                        collisionMap.SetCollision(TileMapPosition, 3);
                        break;
                    case 5: // mailbox
                        IsColliding = true;
                        collisionMap.SetCollision(TileMapPosition, 0);
                        TileMapPosition = nextTileMapPosition;
                        collisionMap.SetCollision(TileMapPosition, 3);
                        break;
                    default:
                        if (!IsSunken)
                        {
                            IsColliding = false;
                            collisionMap.SetCollision(TileMapPosition, 0);
                            TileMapPosition = nextTileMapPosition;
                        }
                        break;
                }*/

            if (boxType == BoxType.VLONGUP || boxType == BoxType.VLONGDOWN || boxType == BoxType.VLONGLEFT || boxType == BoxType.VLONGRIGHT)
            {
                var otherHalfCanMove = true;
                switch (collisionMap.GetCollision(nextTileMapPosition + new Vector2(0, 1)))
                {
                    case 1: // solids
                        otherHalfCanMove = false;
                        break;
                    case 3:
                        break;
                    default:
                        break;
                }
                switch (collisionMap.GetCollision(nextTileMapPosition))
                {
                    case 1: // solids
                        if (otherHalfCanMove)
                        {
                            IsColliding = true;
                            collisionMap.SetCollision(TileMapPosition, 3);
                            collisionMap.SetCollision(TileMapPosition + new Vector2(0, 1), 3);
                        }
                        break;
                    case 2: // water
                        if (otherHalfCanMove && collisionMap.GetCollision(nextTileMapPosition + new Vector2(0, 1)) != 0)
                        {
                            IsColliding = false;
                            IsSunken = true;
                            collisionMap.SetCollision(TileMapPosition, 0);
                            collisionMap.SetCollision(TileMapPosition + new Vector2(0, 1), 0);
                            TileMapPosition = nextTileMapPosition;
                            collisionMap.SetCollision(TileMapPosition, 0); // box acts as path when sunk!
                            collisionMap.SetCollision(TileMapPosition + new Vector2(0, 1), 0);
                            AudioManager.PlaySoundEffect("Box Splash");
                        }
                        else
                        {
                            collisionMap.SetCollision(TileMapPosition, 0);
                            collisionMap.SetCollision(TileMapPosition + new Vector2(0, 1), 0);
                            TileMapPosition = nextTileMapPosition;
                            collisionMap.SetCollision(TileMapPosition, 3);
                            collisionMap.SetCollision(TileMapPosition + new Vector2(0, 1), 3);
                        }
                        
                        break;
                    case 3: // boxes
                        if (otherHalfCanMove)
                        {
                            IsColliding = true;
                            collisionMap.SetCollision(TileMapPosition, 3);
                            collisionMap.SetCollision(TileMapPosition + new Vector2(0, 1), 3);
                        }
                        break;
                    case 5: // mailbox
                        if (otherHalfCanMove)
                        {
                            IsColliding = true;
                            collisionMap.SetCollision(TileMapPosition, 0);
                            collisionMap.SetCollision(TileMapPosition + new Vector2(0, 1), 0);
                            TileMapPosition = nextTileMapPosition;
                            collisionMap.SetCollision(TileMapPosition, 3);
                            collisionMap.SetCollision(TileMapPosition + new Vector2(0, 1), 3);
                        }
                        break;
                    default:
                        if (!IsSunken && otherHalfCanMove)
                        {
                            IsColliding = false;
                            collisionMap.SetCollision(TileMapPosition, 0);
                            collisionMap.SetCollision(TileMapPosition + new Vector2(0, 1), 0);
                            TileMapPosition = nextTileMapPosition;
                            collisionMap.SetCollision(TileMapPosition, 3);
                            collisionMap.SetCollision(TileMapPosition + new Vector2(0, 1), 3);
                        }
                        break;
                }
            }
            else
            {
                switch (collisionMap.GetCollision(nextTileMapPosition))
                {
                    case 1: // solids
                        IsColliding = true;
                        collisionMap.SetCollision(TileMapPosition, 3);
                        break;
                    case 2: // water
                        IsColliding = false;
                        IsSunken = true;
                        collisionMap.SetCollision(TileMapPosition, 0);
                        TileMapPosition = nextTileMapPosition;
                        collisionMap.SetCollision(TileMapPosition, 0); // box acts as path when sunk!
                        AudioManager.PlaySoundEffect("Box Splash");
                        break;
                    case 3: // boxes
                        IsColliding = true;
                        collisionMap.SetCollision(TileMapPosition, 3);
                        break;
                    case 5: // mailbox
                        IsColliding = true;
                        collisionMap.SetCollision(TileMapPosition, 0);
                        TileMapPosition = nextTileMapPosition;
                        collisionMap.SetCollision(TileMapPosition, 3);
                        break;
                    default:
                        if (!IsSunken)
                        {
                            IsColliding = false;
                            collisionMap.SetCollision(TileMapPosition, 0);
                            TileMapPosition = nextTileMapPosition;
                        }
                        break;
                }
            }
            


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
