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
using ISP_Project.Screen_Management.Transitions;
using System.Diagnostics;

namespace ISP_Project.Game_States
{
    public class HubState : State
    {
        private List<Button> envelopes = new List<Button>();
        private Texture2D buttonTexture;
        private int selectedButtonCounter = 0;
        private Button mapButton;
        private Texture2D mapButtonTexture;
        private Texture2D envelopeTexture;
        private SpriteFont buttonFont;
        private Texture2D controlsUI;
        private Texture2D hubDisplay;

        // create shelf lists
        private List<Button> shelf1 = new List<Button>();
        private List<Button> shelf2 = new List<Button>();
        private List<Button> shelf3 = new List<Button>();
        private List<Button> shelf4 = new List<Button>();

        // create tile map instance
        private HubTileMap tileMap = new HubTileMap(WindowManager.GetMainWindowCenter());

        // initialize player
        private Snail player = new Snail(new Vector2(10, 12)); 

        public HubState()
        {
            LoadState();

            // Debug.WriteLine(SaveManager.Load().LevelsCompleted);

            var pauseButton = new PauseButton(buttonTexture, buttonFont, 1, 0.5f)
            {
                Position = new Vector2(buttonTexture.Width / 2, buttonTexture.Height / 2),
                Text = "Pause"
            };

            mapButton = new MapButton(mapButtonTexture, buttonFont, 1, 0.5f)
            {
                Position = new Vector2(WindowManager.GetMainWindowCenter().X - 48, WindowManager.GetMainWindowCenter().Y - 48),
                Text = ""
            };
        }

        public override void LoadState()
        {
            buttonTexture = Globals.ContentManager.Load<Texture2D>("UI Elements/Button");
            mapButtonTexture = Globals.ContentManager.Load<Texture2D>("Interactables/Map Board");
            envelopeTexture = Globals.ContentManager.Load<Texture2D>("Interactables/Envelope");
            buttonFont = Globals.ContentManager.Load<SpriteFont>("Fonts/Button Font");
            controlsUI = Globals.ContentManager.Load<Texture2D>("UI Elements/Controls Display Hub");
            hubDisplay = Globals.ContentManager.Load<Texture2D>("UI Elements/Hub Display");

            tileMap.LoadContent();
            player.LoadContent();

            envelopes.Clear();
            switch (SaveManager.Load().LevelsCompleted)
            {
                case 1:
                    envelopes.Add(new EnvelopeOneButton(envelopeTexture, buttonFont, 1f, 0f)
                    { Position = WindowManager.GetMainWindowCenter() + new Vector2(48, -21) });
                    break;
                case 2:
                    envelopes.Add(new EnvelopeOneButton(envelopeTexture, buttonFont, 1f, 0f)
                    { Position = WindowManager.GetMainWindowCenter() + new Vector2(48, -21) });
                    envelopes.Add(new EnvelopeTwoButton(envelopeTexture, buttonFont, 1f, 0f)
                    { Position = WindowManager.GetMainWindowCenter() + new Vector2(48, -8) });
                    break;
                default:
                    // no envelopes
                    break;
            }

            // Debug.WriteLine(envelopes.Count);
            shelf1.Clear();
            shelf2.Clear();
            shelf3.Clear();
            shelf4.Clear();
            foreach (Button envelope in envelopes)
            {
                if (envelope is EnvelopeOneButton || envelope is EnvelopeTwoButton)
                {
                    shelf1.Add(envelope);
                }
                // Debug.Write(envelope + ", ");
            }
        }

        public override void Update()
        {
            // pause
            if (InputManager.isKey(InputManager.Inputs.PAUSE, InputManager.isTriggered))
            {
                AudioManager.PlaySoundEffect("Button Press");
                StateManager.ChangeState(new PauseState(), Transitions.BlackFade, 0f);
            }

            player.Update();

            // these vectors represent the position of the doorway
            if (player.GetNextPosition() == new Vector2(9, 11) || 
                player.GetNextPosition() == new Vector2(9, 12) || 
                player.GetNextPosition() == new Vector2(9, 13))
            {
                StateManager.ChangeState(new TitleState(), Transitions.BlackFade, 0.1f);
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
            var currentShelf = new List<Button>();
            // these vectors represent the positions under the first inventory shelf
            if (player.TileMapPosition == new Vector2(22, 12) || player.TileMapPosition == new Vector2(23, 12))
            {
                player.isSelectingEnvelope = true;
                currentShelf = shelf1;
            }
            else if (player.TileMapPosition == new Vector2(24, 12) || player.TileMapPosition == new Vector2(25, 12))
            {
                player.isSelectingEnvelope = true;
                currentShelf = shelf2;
            }
            else if (player.TileMapPosition == new Vector2(26, 12) || player.TileMapPosition == new Vector2(27, 12))
            {
                player.isSelectingEnvelope = true;
                currentShelf = shelf3;

            }
            else if (player.TileMapPosition == new Vector2(28, 12) || player.TileMapPosition == new Vector2(29, 12))
            {
                player.isSelectingEnvelope = true;
                currentShelf = shelf4;
            }
            else 
            { 
                player.isSelectingEnvelope = false;

                foreach (Button envelope in envelopes)
                {
                    envelope.ForceShade = false;
                }
                // Debug.WriteLine("NOT SELECTING");
            }
            // Debug.WriteLine(player.TileMapPosition);

            if (player.isSelectingEnvelope && currentShelf.Count > 1)
            {
                // keyboard select for envelopes
                if (InputManager.isKey(InputManager.Inputs.UP, InputManager.isTriggered))
                {
                    selectedButtonCounter++;
                    AudioManager.PlaySoundEffect("Scroll");
                }
                if (InputManager.isKey(InputManager.Inputs.DOWN, InputManager.isTriggered))
                {
                    selectedButtonCounter--;
                    AudioManager.PlaySoundEffect("Scroll");
                }
                if (selectedButtonCounter >= 0)
                    selectedButtonCounter = -currentShelf.Count;
            }

            if (currentShelf.Count > 0)
            {
                var selectedEnvelope = Math.Abs(selectedButtonCounter % currentShelf.Count);
                if (currentShelf.Count == 1)
                    selectedEnvelope = 0;

                foreach (Button envelope in currentShelf)
                {
                    // envelope.Update();
                    envelope.ForceShade = false;
                }
                currentShelf[selectedEnvelope].ForceShade = true;
                // Debug.WriteLine(currentShelf.Count);

                if (InputManager.isKey(InputManager.Inputs.INTERACT, InputManager.isTriggered))
                {
                    // currentShelf[selectedEnvelope].Update();
                    currentShelf[selectedEnvelope].TriggerEvent();
                }
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

            foreach (Button envelope in envelopes)
            {
                envelope.Draw();
            }
            mapButton.Draw();

            var controlsUIOrigin = new Vector2(controlsUI.Width / 2, controlsUI.Height / 2);
            Globals.SpriteBatch.Draw(controlsUI, WindowManager.GetMainWindowCenter(), null, Color.White, 0f, controlsUIOrigin, 1f, SpriteEffects.None, 1f);
            var hubDisplayOrigin = new Vector2(hubDisplay.Width / 2, hubDisplay.Height / 2);
            Globals.SpriteBatch.Draw(hubDisplay, WindowManager.GetMainWindowCenter(), null, Color.White, 0f, controlsUIOrigin, 1f, SpriteEffects.None, 1f);
        }

        public override void PlayStateSong()
        {
            // play music
            switch (SaveManager.Load().LevelsCompleted)
            {
                case 1:
                    AudioManager.ForcePlaySong("Hub Theme 1");
                    break;
                case 2:
                    AudioManager.ForcePlaySong("Hub Theme 2");
                    break;
                default:
                    AudioManager.ForcePlaySong("Hub Theme");
                    break;
            }
        }
    }
}
