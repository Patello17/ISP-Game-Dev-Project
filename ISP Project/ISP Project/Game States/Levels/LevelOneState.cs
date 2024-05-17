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
        private List<Box> boxes;
        private List<Button> buttons;
        private Texture2D buttonTexture;
        private SpriteFont buttonFont;
        
        // create tile map instance
        private LevelOneTileMap tileMap = new LevelOneTileMap(WindowManager.GetMainWindowCenter());
        
        // initialize player
        private Snail player = new Snail(new Vector2(4, 16));
        
        // intialize box position and type
        // naming convention: type of box + room # + letter from top down, left right
        private Box starBox = new Box(new Vector2(5, 16), BoxType.STAR);
        private Box leftBox2 = new Box(new Vector2(20, 14), BoxType.LEFT);
        private Box leftBox3 = new Box(new Vector2(33, 15), BoxType.LEFT);
        private Box upBox3 = new Box(new Vector2(32, 15), BoxType.UP);
        private Box leftBox4 = new Box(new Vector2(34, 6), BoxType.LEFT);
        private Box downBox4 = new Box(new Vector2(33, 6), BoxType.DOWN);
        private Box rightBox4 = new Box(new Vector2(33, 7), BoxType.RIGHT);
        private Box leftBox5a = new Box(new Vector2(20, 7), BoxType.LEFT);
        private Box leftBox5b = new Box(new Vector2(24, 7), BoxType.LEFT);
        private Box leftBox5c = new Box(new Vector2(25, 7), BoxType.LEFT);
        private Box leftBox5d = new Box(new Vector2(27, 7), BoxType.LEFT);
        private Box upBox5a = new Box(new Vector2(21, 9), BoxType.UP);
        private Box upBox5b = new Box(new Vector2(24, 9), BoxType.UP);
        private Box upBox5c = new Box(new Vector2(26, 8), BoxType.UP);
        private Box downBox5a = new Box(new Vector2(19, 4), BoxType.DOWN);
        private Box downBox5b = new Box(new Vector2(25, 5), BoxType.DOWN);

        public LevelOneState()
        {
            boxes = new List<Box>()
            {
                starBox, 
                leftBox2, 
                leftBox3, upBox3,
                leftBox4, downBox4, rightBox4,
                leftBox5a, leftBox5b, leftBox5c, leftBox5d, upBox5a, upBox5b, upBox5c, downBox5a, downBox5b
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

        public override void Update()
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
            }

            // update buttons
            foreach (Button button in buttons)
            {
                button.Update();
            }

            // let actors know of this level's collision map
            foreach (Box box in boxes)
            {
                box.Update(tileMap.CollisionMap);
            }
            player.Update(tileMap.CollisionMap);

            // check for box collisions
            List<Box> boxUpdateOrder = new List<Box>();
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
                AudioManager.PlaySoundEffect("Victory Jingle");
                StateManager.ChangeState(new LevelSelectionState());
            }

            // update Actor positions
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

        public override void PostUpdate()
        {
            // unload sprites if they're not needed
        }

        public override void Draw()
        {
            tileMap.Draw();
            foreach (Box box in boxes)
            {
                box.Draw();
            }
            player.Draw();

            foreach (Button button in buttons)
            {
                button.Draw();
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
