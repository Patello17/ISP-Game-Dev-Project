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
        private Texture2D buttonTexture;
        private Texture2D mapButtonTexture;
        private SpriteFont buttonFont;

        // create tile map instance
        private HubTileMap tileMap = new HubTileMap(WindowManager.GetMainWindowCenter());

        // initialize player
        private Snail player = new Snail(new Vector2(10, 12)); 

        public HubState()
        {
            LoadState();

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

        public override void LoadState()
        {
            buttonTexture = Globals.ContentManager.Load<Texture2D>("UI Elements/Button");
            mapButtonTexture = Globals.ContentManager.Load<Texture2D>("Interactables/Map Board");
            buttonFont = Globals.ContentManager.Load<SpriteFont>("Fonts/Button Font");
            tileMap.LoadContent();
            player.LoadContent();

            // play music
            AudioManager.ForcePlaySong("Hub Theme");
        }

        public override void Update()
        {
            // pause
            if (InputManager.isKey(InputManager.Inputs.PAUSE, InputManager.isTriggered))
            {
                AudioManager.PlaySoundEffect("Button Press");
                StateManager.ChangeState(new PauseState());
            }

            var mapButton = buttons[1];

            foreach (Button button in buttons)
            {
                button.Update();
            }

            // let player know of the collision map
            player.Update(tileMap.CollisionMap);

            // these vectors represent the position of the doorway
            if (player.GetNextPosition() == new Vector2(9, 11) || 
                player.GetNextPosition() == new Vector2(9, 12) || 
                player.GetNextPosition() == new Vector2(9, 13))
            {
                StateManager.ChangeState(new TitleState());
            }
            // these vectors represent the positions right below the map board
            if (player.TileMapPosition == new Vector2(15, 10) || player.TileMapPosition == new Vector2(16, 10) ||
                player.TileMapPosition == new Vector2(17, 10) || player.TileMapPosition == new Vector2(18, 10))
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
            } 

            // update Actor positions
            player.UpdatePosition(tileMap.CollisionMap);
        }

        public override void PostUpdate()
        {
            // unload sprites if they're not needed
        }

        public override void Draw()
        {
            tileMap.Draw();

            player.Draw();

            foreach (Button button in buttons)
            {
                button.Draw();
            }           
        }
    }
}
