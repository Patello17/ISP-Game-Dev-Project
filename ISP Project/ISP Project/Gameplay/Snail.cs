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
        private Texture2D frontTexture;
        private Texture2D backTexture;
        private Texture2D sideTexture;
        public override Sprite Sprite { get; set; }
        public override Transform Transform { get; set; }
        private Vector2 tileMapPosition;
        public override Vector2 TileMapPosition
        {
            get { return tileMapPosition; }
            set
            {
                tileMapPosition = value;
            }
        }
        Vector2 newTileMapPosition;
        Vector2 movementVector;
        private Inputs previousKeyDown;
        private Inputs keyDown;
        private float dasTimer; // DAS stands for "delayed auto shift"
        private float autoShiftDelay = 0.8f;
        private float transitionTimer;
        private float transitionSpeed = 0.4f;
        private bool isSliding = false;

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
            newTileMapPosition = TileMapPosition;
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

            textureDictionary = new Dictionary<Inputs, Texture2D>()
            {
                { Inputs.UP, backTexture },
                { Inputs.DOWN, frontTexture },
                { Inputs.LEFT, sideTexture },
                { Inputs.RIGHT, sideTexture }
            };
        }
        public override void Update(GameTime gameTime, CollisionMap collisionMap)
        {
            previousKeyDown = keyDown;

            GetKeyDown();
            ApplyDAS();
            SetTexture();

            // slide
            Slide(GetNextPosition());

            // Debug.WriteLine("Collision Map is colliding with " + newTileMapPosition + "? " + collisionMap.isColliding(newTileMapPosition));
            // UpdatePosition(collisionMap);
        }

        public override void Draw()
        {
            Globals.SpriteBatch.Draw(Sprite.Texture, Transform.Position, null, Color.White,
                Transform.Rotation, Sprite.GetSpriteOrigin(), Transform.Scale,
                Sprite.SpriteEffects, Sprite.DrawLayer);
        }
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
        public Vector2 GetMovementVector()
        {
            return movementVector;
        }
        public Vector2 GetNextPosition()
        {
            return newTileMapPosition;
        }
        public void ApplyDAS()
        {
            if (isKey(keyDown, isReleased) || keyDown != previousKeyDown)
            {
                dasTimer = 0f;
                transitionTimer = 0f;
            }
            if (isKey(keyDown, isTriggered))
            {
                SetNextPosition();
                isSliding = true;
            }
            if (isKey(keyDown, isPressed))
            {
                dasTimer += Globals.Time;
            }
            if (dasTimer >= autoShiftDelay && isSliding == false)
            {
                if (transitionTimer >= transitionSpeed && isSliding == false)
                {
                    SetNextPosition();
                    isSliding = true;
                }
                else
                {
                    transitionTimer += Globals.Time;
                }
            }
        }
        public void SetNextPosition()
        {
            movementVector = movementDictionary[keyDown];
            newTileMapPosition = TileMapPosition + movementVector;
            // Debug.WriteLine(movementVector);
        }
        public void SetTexture()
        {
            if (isSliding)
            {
                Sprite.Texture = textureDictionary[keyDown];
                Sprite.SpriteEffects = spriteEffectsDictionary[keyDown];
            }
        }
        public void UpdatePosition(CollisionMap collisionMap)
        {
            // Debug.WriteLine(movementVector + " || " + newTileMapPosition);
            // check for collisions (1 = solid in the tilesheet; 5 = mailbox goal)
            if (collisionMap.GetCollision(newTileMapPosition) != 1 &&
                collisionMap.GetCollision(newTileMapPosition) != 5 &&
                collisionMap.GetCollision(newTileMapPosition) != 3)
            {
                // update position
                // Transform.Position += movementVector * 16; // tiles are 16x16
                TileMapPosition = newTileMapPosition;
            }

            movementVector = Vector2.Zero;
            newTileMapPosition = TileMapPosition;

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
