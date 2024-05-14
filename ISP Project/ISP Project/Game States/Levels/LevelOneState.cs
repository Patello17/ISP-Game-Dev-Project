using ISP_Project.Managers;
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
        private Snail player = new Snail(new Vector2(2, 18));
        // naming convention: type of box + room # + letter from top down, left right
        private Box starBox = new Box(new Vector2(3, 18), BoxType.STAR);
        private Box leftBox2 = new Box(new Vector2(17, 16), BoxType.LEFT);
        private Box leftBox3 = new Box(new Vector2(30, 17), BoxType.LEFT);
        private Box downBox3 = new Box(new Vector2(29, 17), BoxType.DOWN);


        public LevelOneState()
        {
            boxes = new List<Box>()
            {
                starBox, leftBox2, leftBox3, downBox3
            };

            LoadState();

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
        public override void LoadState()
        {
            // load everything in this state
            buttonTexture = Globals.ContentManager.Load<Texture2D>("UI Elements/Button");
            buttonFont = Globals.ContentManager.Load<SpriteFont>("Fonts/Button Font");
            tileMap.LoadContent();
            player.LoadContent();
            foreach (Box box in boxes)
            {
                box.LoadContent();
                box.UpdatePosition(tileMap.CollisionMap);
            }
        }
        public override void Update(GameTime gameTime)
        {
            // pause
            if (InputManager.isKey(InputManager.Inputs.PAUSE, InputManager.isTriggered))
            {
                AudioManager.PlaySoundEffect("Button Press");
                StateManager.ChangeState(new PauseState());
            }

            // reset level
            if (InputManager.isKey(InputManager.Inputs.RESTART, InputManager.isTriggered))
            {
                StateManager.ChangeState(new LevelOneState());
                // Debug.WriteLine("RESTARTING!");
            }
                
            List<Box> boxUpdateOrder = new List<Box>();
            foreach (Button button in buttons)
            {
                button.Update(gameTime);
            }

            // let actors know of this level's collision map
            foreach (Box box in boxes)
            {
                box.Update(gameTime, tileMap.CollisionMap);
            }
            player.Update(gameTime, tileMap.CollisionMap);
            
            // check for box collisions
            foreach (Box box in boxes)
            {
                // check if player is about to collide with unsunken box
                if (player.GetNextPosition() == box.TileMapPosition && !box.GetSunkState())
                {
                    box.SetNextPosition(player.GetMovementVector(), false);
                    var currentBoxNextPosition = box.GetNextPosition();
                    boxUpdateOrder.Add(box);

                    // handle "chained" boxes
                    while (tileMap.CollisionMap.GetCollision(currentBoxNextPosition) == 3 && 
                        box.GetMovementVector() != Vector2.Zero && box.GetBoxType() != BoxType.STAR)
                    {
                        if (GetBox(currentBoxNextPosition) != box)
                        {
                            boxUpdateOrder.Add(GetBox(currentBoxNextPosition));
                            GetBox(currentBoxNextPosition).SetNextPosition(player.GetMovementVector(), true);
                        }
                        
                        box.SetNextPosition(player.GetMovementVector(), false);
                        currentBoxNextPosition += player.GetMovementVector();
                    }

                }

            }

            // check for win
            if (tileMap.CollisionMap.GetCollision(starBox.GetNextPosition()) == 5)
            {
                // go to win screen
                Debug.WriteLine("YAY! LEVEL COMPLETED.");
                StateManager.ChangeState(new LevelSelectionState());
            }

            // update actor positions
            if (boxUpdateOrder.Count > 0)
            {
                for (int i = boxUpdateOrder.Count; i > 0; i--)
                {
                    var box = boxUpdateOrder[i - 1];
                    box.UpdatePosition(tileMap.CollisionMap);
                }
            }
            
            player.UpdatePosition(tileMap.CollisionMap);
            
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
                if (box.GetCurrentPosition() == tileMapPosition && !box.GetSunkState())
                {
                    return box;
                }
            }
            return null;
        }

    }
}
