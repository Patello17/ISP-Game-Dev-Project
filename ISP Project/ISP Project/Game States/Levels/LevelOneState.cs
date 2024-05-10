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
        private Snail player = new Snail(WindowManager.GetMainWindowCenter() + new Vector2(-280, 120), new Vector2(2, 18));
        private Box starBox = new Box(WindowManager.GetMainWindowCenter() + new Vector2(-264, 120), new Vector2(3, 18), BoxType.STAR);
        private Box leftBox = new Box(WindowManager.GetMainWindowCenter() + new Vector2(-248, 120), new Vector2(4, 18), BoxType.LEFT);

        public LevelOneState(ContentManager content)
        {
            boxes = new List<Box>()
            {
                starBox, leftBox
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
            }
        }
        public override void Update(GameTime gameTime)
        {
            foreach (Button button in buttons)
            {
                button.Update(gameTime);
            }
            // let player know of the collision map
            // Debug.WriteLine(player.GetMovementVector());
            player.Update(gameTime, tileMap.CollisionMap);
            // Debug.WriteLine(player.GetMovementVector());
            foreach (Box box in boxes)
            {
                if (player.GetNextPosition() == box.TileMapPosition && !box.GetSunkState())
                {
                    box.SetNextPosition(player.GetMovementVector());
                    Debug.WriteLine("PUSHING BOX...");
                }
            }
            foreach (Box box in boxes)
            {
                box.Update(gameTime, tileMap.CollisionMap);
            }
            player.UpdatePosition(tileMap.CollisionMap);

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

    }
}
