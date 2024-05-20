﻿using System;
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
    internal class Snail : Actor
    {
        // create texture variables
        private Texture2D frontTexture;
        private Texture2D backTexture;
        private Texture2D sideTexture;

        // reference Actor properties
        public override Sprite Sprite { get; set; }
        public override Transform Transform { get; set; }
        public override Vector2 TileMapPosition { get; set; }

        // create movement-related properties and fields
        public override List<Vector2> PastPositions { get; set; }
        public List<Sprite> PastSprites { get; set; }
        public bool IsColliding { get; set; } = false;
        Vector2 nextTileMapPosition;
        Vector2 movementVector;

        // create fine-control movement variables
        private Inputs previousKeyDown;
        private Inputs keyDown;
        private float dasTimer; // DAS stands for "delayed auto shift"
        private float autoShiftDelay = 0.1f;
        private float transitionTimer;
        private float transitionSpeed = 0.4f;
        private bool isSliding = false;
        private float slideSpeed = 0.3f;

        // create dictionaries for commonly referenced information
        private Dictionary<Inputs, Vector2> movementDictionary = new Dictionary<Inputs, Vector2>();
        private Dictionary<Inputs, Texture2D> textureDictionary = new Dictionary<Inputs, Texture2D>();
        private Dictionary<Inputs, SpriteEffects> spriteEffectsDictionary = new Dictionary<Inputs, SpriteEffects>();

        public Snail(Vector2 tileMapPosition)
        {
            Transform = new Transform(Vector2.Zero, 1f, 0f);
            TileMapPosition = tileMapPosition;
            var centeredTileMapPosition = tileMapPosition - new Vector2(20, 11);
            Transform.Position = new Vector2(
                (int)(WindowManager.GetMainWindowCenter().X + (centeredTileMapPosition.X * 16)) + 8,
                (int)(WindowManager.GetMainWindowCenter().Y + (centeredTileMapPosition.Y * (180 / 11))) + 8);
            PastPositions = new List<Vector2>() { TileMapPosition };
            nextTileMapPosition = TileMapPosition;
            keyDown = Inputs.RIGHT;

            movementDictionary = new Dictionary<Inputs, Vector2>()
            {
                { Inputs.UP, new Vector2(0, -1) },
                { Inputs.DOWN, new Vector2(0, 1) },
                { Inputs.LEFT, new Vector2(-1, 0) },
                { Inputs.RIGHT, new Vector2(1, 0) }
            };

            spriteEffectsDictionary = new Dictionary<Inputs, SpriteEffects>()
            {
                { Inputs.UP, SpriteEffects.None },
                { Inputs.DOWN, SpriteEffects.None },
                { Inputs.LEFT, SpriteEffects.FlipHorizontally },
                { Inputs.RIGHT, SpriteEffects.None },
            };
        }
        
        public override void LoadContent()
        {
            Sprite = new Sprite(null, SpriteEffects.None, 0.8f);
            sideTexture = Globals.ContentManager.Load<Texture2D>("Snail/Snail");
            frontTexture = Globals.ContentManager.Load<Texture2D>("Snail/Snail Front");
            backTexture = Globals.ContentManager.Load<Texture2D>("Snail/Snail Back");
            Sprite.Texture = sideTexture;
            PastSprites = new List<Sprite>() { Sprite };

            textureDictionary = new Dictionary<Inputs, Texture2D>()
            {
                { Inputs.UP, backTexture },
                { Inputs.DOWN, frontTexture },
                { Inputs.LEFT, sideTexture },
                { Inputs.RIGHT, sideTexture }
            };
        }

        public override void Update()
        {
            previousKeyDown = keyDown;

            // reset movement variables
            movementVector = Vector2.Zero;
            nextTileMapPosition = TileMapPosition;

            GetKeyDown();
            ApplyDAS();
            SetTexture();
            Slide(GetNextPosition());

            // Debug.WriteLine(PastPositions.Count);
            // Debug.WriteLine("Collision Map is colliding with " + newTileMapPosition + "? " + collisionMap.isColliding(newTileMapPosition));
            // UpdatePosition(collisionMap);
        }

        public override void Draw()
        {
            Globals.SpriteBatch.Draw(Sprite.Texture, Transform.Position, null, Color.White,
                Transform.Rotation, Sprite.GetSpriteOrigin(), Transform.Scale,
                Sprite.SpriteEffects, Sprite.DrawLayer);
        }

        /// <summary>
        /// Gets the most recent movement key pressed.
        /// </summary>
        public void GetKeyDown()
        {
            switch (true)
            {
                case bool when isKey(Inputs.UP, isTriggered):
                    keyDown = Inputs.UP;
                    break;
                case bool when isKey(Inputs.DOWN, isTriggered):
                    keyDown = Inputs.DOWN;
                    break;
                case bool when isKey(Inputs.LEFT, isTriggered):
                    keyDown = Inputs.LEFT;
                    break;
                case bool when isKey(Inputs.RIGHT, isTriggered):
                    keyDown = Inputs.RIGHT;
                    break;
            }
        }

        /// <summary>
        /// Gets the player's movement vector.
        /// </summary>
        /// <returns></returns>
        public Vector2 GetMovementVector()
        {
            return movementVector;
        }

        /// <summary>
        /// Gets the player's next position.
        /// </summary>
        /// <returns></returns>
        public Vector2 GetNextPosition()
        {
            return nextTileMapPosition;
        }

        /// <summary>
        /// Applies a Delayed Auto Shift and updates movement.
        /// </summary>
        public void ApplyDAS()
        {
            if (isKey(keyDown, isReleased) || keyDown != previousKeyDown)
            {
                dasTimer = 0f;
                transitionTimer = 0f;
            }
            if (isKey(keyDown, isTriggered))
            {
                isSliding = true;
                SetNextPosition();
            }
            if (isKey(keyDown, isPressed))
            {
                dasTimer += Globals.Time;
            }
            else
            {
                dasTimer = 0f;
                transitionTimer = 0f;
            }
            if (dasTimer >= autoShiftDelay && !isSliding)
            {
                if (transitionTimer >= transitionSpeed && !isSliding)
                {
                    isSliding = true;
                    SetNextPosition();
                }
                else
                {
                    transitionTimer += Globals.Time;
                }
            }
        }

        /// <summary>
        /// Sets the player's next position.
        /// </summary>
        public void SetNextPosition()
        {
            movementVector = movementDictionary[keyDown];
            nextTileMapPosition = TileMapPosition + movementVector;
        }

        /// <summary>
        /// Sets the player's texture
        /// </summary>
        public void SetTexture()
        {
            if (isSliding)
            {
                Sprite.Texture = textureDictionary[keyDown];
                Sprite.SpriteEffects = spriteEffectsDictionary[keyDown];
            }
        }

        public override void UpdatePosition(CollisionMap collisionMap)
        {
            // check for collisions
            if (nextTileMapPosition != TileMapPosition)
            {
                switch(collisionMap.GetCollision(nextTileMapPosition))
                {
                    case 1: // solids
                        IsColliding = true;
                        AudioManager.PlaySoundEffect("Collision Not Permitted");
                        break;
                    case 2: // water
                        IsColliding = false;
                        AudioManager.PlaySoundEffect("Box Splash");
                        TileMapPosition = nextTileMapPosition; // update position
                        break;
                    case 3: // boxes
                        AudioManager.PlaySoundEffect("Player Movement");
                        break;
                    case 5: // mailbox
                        IsColliding = true;
                        AudioManager.PlaySoundEffect("Collision Not Permitted");
                        break;
                    default:
                        IsColliding = false;
                        AudioManager.PlaySoundEffect("Player Movement");
                        TileMapPosition = nextTileMapPosition; // update position
                        break;
                }
                /*if (collisionMap.GetCollision(nextTileMapPosition) != 1 && // solids
                collisionMap.GetCollision(nextTileMapPosition) != 3 && // boxes
                collisionMap.GetCollision(nextTileMapPosition) != 5) // mailbox
                {
                    AudioManager.PlaySoundEffect("Player Movement");

                    // update position
                    TileMapPosition = nextTileMapPosition;
                }
                if (collisionMap.GetCollision(nextTileMapPosition) == 1 || // solids
                    collisionMap.GetCollision(nextTileMapPosition) == 5) // mailbox
                {
                    AudioManager.PlaySoundEffect("Collision Not Permitted");
                    // update position
                    TileMapPosition = nextTileMapPosition;
                }*/
            }
            
            // Debug.WriteLine(nextTileMapPosition);
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

            // smoothly transition from the current tile to the next tile.
            Vector2 newPosition = Vector2.Lerp(Transform.Position, targetPosition, slideSpeed);
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
