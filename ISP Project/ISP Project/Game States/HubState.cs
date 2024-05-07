﻿using ISP_Project.UI.Buttons;
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
        private SpriteFont buttonFont;

        private HubTileMap tileMap = new HubTileMap(WindowManager.GetMainWindowCenter());
        private Snail player = new Snail(WindowManager.GetMainWindowCenter() + new Vector2(8, 8), new Vector2(8, 4)); // Vector2(8, 8) is used to align Shelly with the grid since we're drawing from the middle of the sprite now

        public HubState(ContentManager content)
        {
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
        }
        public override void Update(GameTime gameTime)
        {
            foreach (Button button in buttons)
            {
                button.Update(gameTime);
            }
            player.Update(gameTime, tileMap.CollisionMap);
        }

        public override void PostUpdate(GameTime gameTime)
        {
            // unload sprites if they're not needed
        }

        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            tileMap.Draw(gameTime, spriteBatch);

            player.Draw(gameTime, spriteBatch);

            foreach (Button button in buttons)
            {
                button.Draw(gameTime, spriteBatch);
            }           
        }
    }
}
