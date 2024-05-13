using ISP_Project.UI.Buttons;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ISP_Project.Tilemaps;
using ISP_Project.Gameplay;
using ISP_Project.Managers;

namespace ISP_Project.Game_States
{
    public class HubState : State
    {
        private List<Button> buttons;
        // create variables for the textures and fonts of the buttons (Buttons can share the same texture/font)
        private Texture2D buttonTexture;
        private Texture2D mapButtonTexture;
        private SpriteFont buttonFont;

        private HubTileMap tileMap = new HubTileMap(WindowManager.GetMainWindowCenter());
        // + Vector2(x, y) is used to align Shelly with the grid since we're drawing from the middle of the sprite now; y can be adjusted for a more 3D effect
        private Snail player = new Snail(WindowManager.GetMainWindowCenter() + new Vector2(-152, 24), new Vector2(1, 6)); 

        public HubState(ContentManager content)
        {
            LoadState(content);

            var pauseButton = new PauseButton(buttonTexture, buttonFont, 1, 0.5f)
            {
                Position = new Vector2(buttonTexture.Width / 2, buttonTexture.Height / 2),
                Text = "Pause"
            };

            var mapButton = new MapButton(mapButtonTexture, buttonFont, 1, 0.5f)
            {
                Position = new Vector2(WindowManager.GetMainWindowCenter().X - 48, WindowManager.GetMainWindowCenter().Y - 48),
                Text = ""
            };

            buttons = new List<Button>()
            {
                pauseButton, mapButton
            };
        }
        public override void LoadState(ContentManager content)
        {
            // load everything in this state
            buttonTexture = content.Load<Texture2D>("UI Elements/Button");
            mapButtonTexture = content.Load<Texture2D>("Interactables/Map Board");
            buttonFont = content.Load<SpriteFont>("Fonts/Button Font");
            tileMap.LoadContent(content);
            player.LoadContent(content);
        }
        public override void Update(GameTime gameTime)
        {
            // pause
            if (InputManager.isKey(InputManager.Inputs.PAUSE, InputManager.isTriggered))
            {
                StateManager.ChangeState(new MenuState(Globals.ContentManager));
            }

            var mapButton = buttons[1];

            foreach (Button button in buttons)
            {
                button.Update(gameTime);
            }

            // let player know of the collision map
            player.Update(gameTime, tileMap.CollisionMap);

            // these vectors represent the position of the doorway
            if (player.GetNextPosition() == new Vector2(0, 5) || player.GetNextPosition() == new Vector2(0, 6) || player.GetNextPosition() == new Vector2(0, 7))
            {
                StateManager.ChangeState(new TitleState(Globals.ContentManager));
            }
            // these vectors represent the positions right below the map board
            if (player.TileMapPosition == new Vector2(6, 4) || player.TileMapPosition == new Vector2(7, 4) ||
                player.TileMapPosition == new Vector2(8, 4) || player.TileMapPosition == new Vector2(9, 4))
            {

                MapButton.isClickable = true;
                mapButton.ForceShade = true;
            }
            else
            {
                MapButton.isClickable = false;
                mapButton.ForceShade = false;
            }
            // detect mapButton interaction
            if (InputManager.isKey(InputManager.Inputs.INTERACT, InputManager.isTriggered))
            {
                mapButton.TriggerEvent();
                AudioManager.PlaySoundEffect(AudioManager.soundEffects[0]);
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

            player.Draw(gameTime);

            foreach (Button button in buttons)
            {
                button.Draw(gameTime);
            }           
        }
    }
}
