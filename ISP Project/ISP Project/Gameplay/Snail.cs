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
    internal class Snail : Actor
    {
        private Texture2D frontTexture;
        private Texture2D backTexture;
        private Texture2D sideTexture;
        public override Sprite Sprite { get; set; }
        public override Transform Transform { get; set; }
        public override Vector2 TileMapPosition { get; set; }
        Vector2 newTileMapPosition;
        Vector2 movementVector;
        private Inputs previousKeyDown;
        private Inputs keyDown;
        private float dasTimer; // DAS stands for "delayed auto shift"
        private float autoShiftDelay = 0.8f;
        private float transitionTimer;
        private float transitionSpeed = 0.2f;

        private Dictionary<Inputs, Vector2> movementDictionary = new Dictionary<Inputs, Vector2>();
        private Dictionary<Inputs, Texture2D> textureDictionary = new Dictionary<Inputs, Texture2D>();
        private Dictionary<Inputs, SpriteEffects> spriteEffectsDictionary = new Dictionary<Inputs, SpriteEffects>();

        public Snail(Vector2 position, Vector2 tileMapPosition)
        {
            Transform = new Transform(position, 1f, 0f);
            TileMapPosition = tileMapPosition;
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
        
        public override void LoadContent(ContentManager content)
        {
            Sprite = new Sprite(null, SpriteEffects.None, 0);
            sideTexture = content.Load<Texture2D>("Snail/Snail");
            frontTexture = content.Load<Texture2D>("Snail/Snail Front");
            backTexture = content.Load<Texture2D>("Snail/Snail Back");
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
            
            movementVector = Vector2.Zero;
            newTileMapPosition = TileMapPosition;

            previousKeyDown = keyDown;

            GetKeyDown();
            ApplyDAS();
            SetTexture();

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
        public void ApplyDAS()
        {
            // Debug.WriteLine(keyDown);
            if (isKey(keyDown, isReleased) || keyDown != previousKeyDown)
            {
                dasTimer = 0f;
                transitionTimer = 0f;
                // Debug.WriteLine("KEY CHANGE");
            }
            if (isKey(keyDown, isTriggered))
            {
                SetNextPosition();
            }
            if (isKey(keyDown, isPressed))
            {
                dasTimer += Globals.Time;
                // Debug.WriteLine(dasTimer);
            }
            if (dasTimer >= autoShiftDelay)
            {
                if (transitionTimer >= transitionSpeed)
                {
                    SetNextPosition();
                    transitionTimer = 0f;
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
        }
        public void SetTexture()
        {
            Sprite.Texture = textureDictionary[keyDown];
            Sprite.SpriteEffects = spriteEffectsDictionary[keyDown];
        }
        public void UpdatePosition(CollisionMap collisionMap)
        {
            // check for collisions (1 = solid in the tilesheet; 5 = mailbox goal)
            if (collisionMap.GetCollision(newTileMapPosition) != 1 &&
                collisionMap.GetCollision(newTileMapPosition) != 5)
            {
                // update position
                Transform.Position += movementVector * 16; // tiles are 16x16
                TileMapPosition = newTileMapPosition;
            }
        }
    }
}
