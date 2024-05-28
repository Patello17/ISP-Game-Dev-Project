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
using ISP_Project.Screen_Management.Transitions;

namespace ISP_Project.Game_States.Levels
{
    public class LevelThreeState : State
    {
        private List<Box> boxes;
        private List<Button> buttons;
        private Texture2D buttonTexture;
        private SpriteFont buttonFont;
        private Texture2D controlsUI;

        // create undo timing variables
        private float undoDasTimer; // DAS stands for "delayed auto shift"
        private float undoDelay = 0.4f;
        private float transitionTimer;
        private float transitionSpeed = 0.1f;

        // create tile map instance
        private LevelThreeTileMap tileMap = new LevelThreeTileMap(WindowManager.GetMainWindowCenter());

        // initialize player
        private Snail player = new Snail(new Vector2(4, 17));

        // intialize box position and type
        // naming convention: type of box + room # + letter from top down, left right
        private Box starBox = new Box(new Vector2(5, 17), BoxType.STAR);
        private Box downBox1a = new Box(new Vector2(5, 14), BoxType.DOWN);
        private Box downBox1b = new Box(new Vector2(6, 14), BoxType.DOWN);
        private Box leftBox1 = new Box(new Vector2(6, 15), BoxType.LEFT);
        private Box rightBox1 = new Box(new Vector2(5, 15), BoxType.RIGHT);
        private Box rightBox2a = new Box(new Vector2(16, 16), BoxType.RIGHT);
        private Box rightBox2b = new Box(new Vector2(23, 14), BoxType.RIGHT);
        private Box downBox2 = new Box(new Vector2(24, 14), BoxType.DOWN);
        private Box upBox2a = new Box(new Vector2(17, 17), BoxType.UP);
        private Box upBox2b = new Box(new Vector2(22, 17), BoxType.UP);
        private Box leftBox2a = new Box(new Vector2(18, 17), BoxType.LEFT);
        private Box leftBox2b = new Box(new Vector2(25, 15), BoxType.LEFT);
        private Box hbox3a = new Box(new Vector2(30, 9), BoxType.HORIZONTAL);
        private Box hbox3b = new Box(new Vector2(30, 10), BoxType.HORIZONTAL);
        private Box rightBox3a = new Box(new Vector2(30, 14), BoxType.RIGHT);
        private Box rightBox3b = new Box(new Vector2(30, 15), BoxType.RIGHT);
        private Box vbox3 = new Box(new Vector2(31, 17), BoxType.VERTICAL);
        private Box leftBox3 = new Box(new Vector2(32, 16), BoxType.LEFT);
        private Box downBox4a = new Box(new Vector2(16, 7), BoxType.DOWN);
        private Box downBox4b = new Box(new Vector2(17, 4), BoxType.DOWN);
        private Box downBox4c = new Box(new Vector2(21, 4), BoxType.DOWN);
        private Box rightBox4 = new Box(new Vector2(16, 8), BoxType.RIGHT);
        private Box leftBox4 = new Box(new Vector2(22, 9), BoxType.LEFT);
        private Box upBox4a = new Box(new Vector2(17, 9), BoxType.UP);
        private Box upBox4b = new Box(new Vector2(21, 9), BoxType.UP);
        private Box hBox4a = new Box(new Vector2(19, 4), BoxType.HORIZONTAL);
        private Box hBox4b = new Box(new Vector2(24, 5), BoxType.HORIZONTAL);


        public LevelThreeState()
        {
            boxes = new List<Box>()
            {
                starBox, downBox1a, downBox1b, leftBox1, rightBox1,
                rightBox2a, rightBox2b, downBox2, upBox2a, upBox2b, leftBox2a, leftBox2b,
                hbox3a, hbox3b, rightBox3a, rightBox3b, vbox3, leftBox3,
                downBox4a, downBox4b, downBox4c, rightBox4, leftBox4, upBox4a, upBox4b, hBox4a, hBox4b
            };

            LoadState();

            var pauseButton = new PauseButton(buttonTexture, buttonFont, 1, 0.5f)
            {
                Position = new Vector2(buttonTexture.Width / 2, buttonTexture.Height / 2),
                Text = "Pause"
            };

            buttons = new List<Button>()
            {
                // pauseButton
            };
        }

        public override void LoadState()
        {
            buttonTexture = Globals.ContentManager.Load<Texture2D>("UI Elements/Button");
            buttonFont = Globals.ContentManager.Load<SpriteFont>("Fonts/Button Font");
            controlsUI = Globals.ContentManager.Load<Texture2D>("UI Elements/Controls Display");

            tileMap.LoadContent();
            player.LoadContent();
            player.Sprite.SpriteEffects = SpriteEffects.FlipHorizontally;

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
                StateManager.ChangeState(new PauseState(), Transitions.BlackFade, 0.1f);
            }

            // reset level
            if (InputManager.isKey(InputManager.Inputs.RESTART, InputManager.isTriggered))
            {
                StateManager.ChangeState(new LevelThreeState(), Transitions.Push, 0.5f);
            }

            // undo moves
            if (InputManager.isKey(InputManager.Inputs.UNDO, InputManager.isTriggered))
            {
                Undo();
            }
            if (InputManager.isKey(InputManager.Inputs.UNDO, InputManager.isPressed))
            {
                undoDasTimer += Globals.Time;
            }
            else
            {
                undoDasTimer = 0f;
                transitionTimer = 0f;
            }
            if (undoDasTimer >= undoDelay)
            {
                if (transitionTimer >= transitionSpeed)
                {
                    transitionTimer = 0f;
                    Undo();
                }
                else
                {
                    transitionTimer += Globals.Time;
                }
            }

            // update Buttons
            foreach (Button button in buttons)
            {
                button.Update();
            }

            // update Actors
            foreach (Box box in boxes)
            {
                box.Update();
            }
            player.Update();

            // check for box collisions
            List<Box> boxUpdateOrder = new List<Box>();
            foreach (Box box in boxes)
            {
                // check if player is about to collide with unsunken box
                if (player.GetNextPosition() == box.TileMapPosition && !box.GetSinkState())
                {
                    box.SetNextPosition(player.GetMovementVector(), false);
                    var currentBoxNextPosition = box.GetNextPosition();
                    boxUpdateOrder.Add(box);

                    // handle "chained" boxes
                    while (tileMap.CollisionMap.GetCollision(currentBoxNextPosition) == 3 && // another box
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
            if (tileMap.CollisionMap.GetCollision(starBox.GetNextPosition()) == 5) // mailbox
            {
                // save new save file
                if (SaveManager.Load().LevelsCompleted < 3)
                {
                    SaveFile saveFile = new SaveFile()
                    {
                        LevelsCompleted = SaveManager.Load().LevelsCompleted + 1,
                        LevelOneFewestMoves = SaveManager.Load().LevelOneFewestMoves,
                        LevelTwoFewestMoves = SaveManager.Load().LevelTwoFewestMoves,
                        LevelThreeFewestMoves = SaveManager.Load().LevelsCompleted == 2 ? player.PastPositions.Count - 1 : SaveManager.Load().LevelThreeFewestMoves < player.PastPositions.Count - 1 ? SaveManager.Load().LevelThreeFewestMoves : player.PastPositions.Count - 1,
                    };
                    SaveManager.Save(saveFile);
                }

                // go to win screen
                AudioManager.PlaySoundEffect("Victory Jingle");
                StateManager.ChangeState(new HubState(), Transitions.BlackFade, 0.1f);
            }

            // add new moves to the undo lists
            if (!player.IsColliding && player.PastPositions[0] != player.TileMapPosition)
            {
                player.PastPositions.Insert(0, player.TileMapPosition);
                player.PastSprites.Insert(0, new Sprite(player.Sprite.Texture, player.Sprite.SpriteEffects, 0.8f));

                foreach (Box box in boxes)
                {
                    box.PastPositions.Insert(0, box.TileMapPosition);
                    box.PastSinkState.Insert(0, box.IsSunken);
                }
                // Debug.WriteLine(player.PastSprites.Count + " : " + player.PastSprites[0].ToString());
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

            // update collision map to reflect box positions
            foreach (Box box in boxes)
            {
                if (!box.IsSunken)
                    tileMap.CollisionMap.SetCollision(box.TileMapPosition, 3);
            }
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

            var controlsUIOrigin = new Vector2(controlsUI.Width / 2, controlsUI.Height / 2);
            Globals.SpriteBatch.Draw(controlsUI, WindowManager.GetMainWindowCenter(), null, Color.White, 0f, controlsUIOrigin, 1f, SpriteEffects.None, 1f);

            string movesText = "Moves: " + (player.PastPositions.Count - 1);
            float movesTextSize = 0.5f;
            var x = WindowManager.GetMainWindowCenter().X + 107 - (buttonFont.MeasureString(movesText).X * movesTextSize / 2);
            var y = WindowManager.GetMainWindowCenter().Y - 160 - (buttonFont.MeasureString(movesText).Y * movesTextSize / 2);
            Globals.SpriteBatch.DrawString(buttonFont, movesText, new Vector2(x, y), Color.White, 0f, Vector2.Zero, movesTextSize, SpriteEffects.None, 1f);
        }

        public override void PlayStateSong()
        {
            // play music
            AudioManager.ForcePlaySong("Level 3 Theme");
        }

        private Box GetBox(Vector2 tileMapPosition)
        {
            foreach (Box box in boxes)
            {
                if (box.GetCurrentPosition() == tileMapPosition && !box.GetSinkState())
                {
                    return box;
                }
            }
            return null;
        }

        private void Undo()
        {
            if (player.PastPositions.Count > 1 && player.PastSprites.Count > 1)
            {
                player.TileMapPosition = player.PastPositions[1];
                player.PastPositions.RemoveAt(0);
                player.Sprite = player.PastSprites[0];
                player.PastSprites.RemoveAt(0);

                foreach (Box box in boxes)
                {
                    if (box.TileMapPosition != box.PastPositions[1])
                    {
                        if (box.PastSinkState[0])
                        {
                            tileMap.CollisionMap.SetCollision(box.TileMapPosition, 2);
                        }
                        else if (GetBox(box.TileMapPosition) != null)
                        {
                            tileMap.CollisionMap.SetCollision(box.TileMapPosition, 0);
                        }
                    }

                    box.TileMapPosition = box.PastPositions[1];
                    box.PastPositions.RemoveAt(0);
                    box.IsSunken = box.PastSinkState[1];
                    box.PastSinkState.RemoveAt(0);
                }
                // Debug.WriteLine(starBox.PastSinkState[0]);
            }
        }
    }
}
