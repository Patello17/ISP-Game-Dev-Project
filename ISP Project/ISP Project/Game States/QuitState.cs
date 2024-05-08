using ISP_Project.Tilemaps;
using ISP_Project.UI.Buttons;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ISP_Project.Game_States
{
    public class QuitState : State
    {
        public override void LoadState(ContentManager content) { }
        public override void Update(GameTime gameTime) 
        {
            Game1.quit = true;
        }

        public override void PostUpdate(GameTime gameTime) { }

        public override void Draw(GameTime gameTime) { }
    }
}
