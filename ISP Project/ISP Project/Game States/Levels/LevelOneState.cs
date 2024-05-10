﻿using ISP_Project.Managers;
using ISP_Project.UI.Buttons;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ISP_Project.Components;
using ISP_Project.Gameplay;
using ISP_Project.Tilemaps;
using ISP_Project.Tilemaps.Maps.Level_1;
using static ISP_Project.Gameplay.Box;
using System.Diagnostics;
using MonoGame.Extended.Timers;

namespace ISP_Project.Game_States.Levels
{
    public class LevelOneState : State
    {
        private List<Button> buttons;
        private List<Box> boxes;
        // create variables for the textures and fonts of the buttons (Buttons can share the same texture/font)
        private Texture2D buttonTexture;
        private SpriteFont buttonFont;
        private LevelOneTileMap tileMap = new LevelOneTileMap(WindowManager.GetMainWindowCenter());
        // + Vector2(x, y) is used to align Shelly with the grid since we're drawing from the middle of the sprite now; y can be adjusted for a more 3D effect
        private Snail player = new Snail(WindowManager.GetMainWindowCenter() + new Vector2(-280, 120), new Vector2(2, 18));
        // naming convention: type of box + room # + letter from top down, left right
        private Box starBox = new Box(WindowManager.GetMainWindowCenter() + new Vector2(-264, 120), new Vector2(3, 18), BoxType.STAR);
        private Box leftBox2 = new Box(WindowManager.GetMainWindowCenter() + new Vector2(-40, 88), new Vector2(17, 16), BoxType.LEFT);
        private Box leftBox3 = new Box(WindowManager.GetMainWindowCenter() + new Vector2(136, 104), new Vector2(28, 17), BoxType.LEFT);
        private Box downBox3 = new Box(WindowManager.GetMainWindowCenter() + new Vector2(120, 104), new Vector2(27, 17), BoxType.DOWN);


        public LevelOneState(ContentManager content)
        {
            boxes = new List<Box>()
            {
                starBox, leftBox2, leftBox3, downBox3
            };

            LoadState(content);

            var pauseButton = new PauseButton(buttonTexture, buttonFont, 1, 0.5f)
            {
                Position = new Vector2(buttonTexture.Width / 2, buttonTexture.Height / 2),
                Text = "Pause"
            };

            buttons = new List<Button>()
            {
                pauseButton
            };
        }
        public override void LoadState(ContentManager content)
        {
            // load everything in this state
            buttonTexture = content.Load<Texture2D>("UI Elements/Button");
            buttonFont = content.Load<SpriteFont>("Fonts/Button Font");
            tileMap.LoadContent(content);
            player.LoadContent(content);
            foreach (Box box in boxes)
            {
                box.LoadContent(content);
                box.UpdatePosition(tileMap.CollisionMap);
            }
        }
        public override void Update(GameTime gameTime)
        {
            List<Box> boxUpdateOrder = new List<Box>();
            foreach (Button button in buttons)
            {
                button.Update(gameTime);
            }
            // let player know of the collision map
            // Debug.WriteLine(player.GetMovementVector());
            foreach (Box box in boxes)
            {
                box.Update(gameTime, tileMap.CollisionMap);
            }
            player.Update(gameTime, tileMap.CollisionMap);
            // Debug.WriteLine(player.GetMovementVector());
            /*foreach (Box box in boxes)
            {
                bool canPush = false;
                // push the box
                if (player.GetNextPosition() == box.TileMapPosition && !box.GetSunkState())
                {
                    // check interaction with every other box
                    foreach (Box _box in boxes)
                    {
                        box.SetNextPosition(player.GetMovementVector());
                        if (_box != box && tileMap.CollisionMap.GetCollision(box.GetNextPosition()) == 3 && _box.CanMove)
                        {
                            _box.SetNextPosition(box.GetMovementVector());
                            Debug.WriteLine(tileMap.CollisionMap.GetCollision(_box.GetNextPosition()));
                            if (tileMap.CollisionMap.GetCollision(_box.GetNextPosition()) == 0 ||
                                tileMap.CollisionMap.GetCollision(_box.GetNextPosition()) == 2 ||
                                tileMap.CollisionMap.GetCollision(_box.GetNextPosition()) == 3)
                            {
                                canPush = true;
                            }
                            if (canPush)
                            {
                                box.SetNextPosition(player.GetMovementVector());
                                _box.SetNextPosition(box.GetMovementVector());
                            }
                            else
                            {
                                box.SetNextPosition(Vector2.Zero);
                                _box.SetNextPosition(Vector2.Zero);
                            }
                        }

                    }

                    // Debug.WriteLine("PUSHING BOX...");
                }

            }*/
            foreach (Box box in boxes)
            {
                // check if player is about to collide with unsunken box
                if (player.GetNextPosition() == box.TileMapPosition && !box.GetSunkState())
                {
                    box.SetNextPosition(player.GetMovementVector(), false);
                    var currentBoxNextPosition = box.GetNextPosition();
                    // var nextBoxNextPosition = currentBoxNextPosition + player.GetMovementVector();
                    // var nextBoxNextPosition = currentBoxNextPosition;
                    // handle "chained" boxes
                    while (tileMap.CollisionMap.GetCollision(currentBoxNextPosition) == 3 && box.GetMovementVector() != Vector2.Zero)
                    {
                        if (GetBox(currentBoxNextPosition) != box)
                        {
                            boxUpdateOrder.Add(GetBox(currentBoxNextPosition));
                            GetBox(currentBoxNextPosition).SetNextPosition(player.GetMovementVector(), true);
                        }
                        
                        box.SetNextPosition(player.GetMovementVector(), false);
                        //nextBoxNextPosition = currentBoxNextPosition + player.GetMovementVector();
                        currentBoxNextPosition += player.GetMovementVector();
                        // nextBoxNextPosition += player.GetMovementVector();
                    }
                    boxUpdateOrder.Add(box);

                }
                
            }

            /*foreach (Box box in boxes)
            {
                box.UpdatePosition(tileMap.CollisionMap);
            }*/

            
            if (boxUpdateOrder.Count > 0)
            {
                Debug.WriteLine(boxUpdateOrder.Count);
                foreach (Box box in boxUpdateOrder)
                {
                    // Debug.WriteLine(box.GetMovementVector());
                }
                /*for (int i = boxOrder.Count; i > 0; i--)
                {
                    var box = boxOrder[i - 1];
                    box.UpdatePosition(tileMap.CollisionMap);
                }*/
                for (int i = 0; i < boxUpdateOrder.Count; i++)
                {
                    var box = boxUpdateOrder[i];
                    box.UpdatePosition(tileMap.CollisionMap);
                }
            }
            
            player.UpdatePosition(tileMap.CollisionMap);

            // TODO: Goal Tile should check for GetBoxType() starBox
            if (tileMap.CollisionMap.GetCollision(starBox.TileMapPosition) == 5)
            {
                StateManager.ChangeState(new LevelSelectionState(Globals.ContentManager));
            }
        }

        public override void PostUpdate(GameTime gameTime)
        {
            // unload sprites if they're not needed
        }

        public override void Draw(GameTime gameTime)
        {
            tileMap.Draw(gameTime);
            foreach (Box box in boxes)
            {
                box.Draw(gameTime);
            }
            player.Draw(gameTime);

            foreach (Button button in buttons)
            {
                button.Draw(gameTime);
            }
        }

        private Box GetBox(Vector2 tileMapPosition)
        {
            foreach (Box box in boxes)
            {
                if (box.GetCurrentPosition() == tileMapPosition)
                {
                    return box;
                }
            }
            return null;
        }

    }
}
