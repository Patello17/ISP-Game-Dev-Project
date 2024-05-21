using ISP_Project.Components;
using ISP_Project.Managers;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Diagnostics.Metrics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ISP_Project.Screen_Management.Transitions
{
    // Code Reference: https://www.youtube.com/watch?v=oeUE2O6LAEU
    public class EnvelopeOpenTransition(RenderTarget2D transitionFrame) : Transition(transitionFrame)
    {
        Sprite closedEnvelope = new Sprite(null, SpriteEffects.None, 0.5f);
        Sprite frontEnvelope = new Sprite(null, SpriteEffects.None, 0.3f);
        Sprite backEnvelope = new Sprite(null, SpriteEffects.None, 0f);
        protected override void Process()
        {
            closedEnvelope.Color = Color.White;
            closedEnvelope.Texture = Globals.ContentManager.Load<Texture2D>("UI Elements/Closed Envelope");
            frontEnvelope.Color = Color.White;
            frontEnvelope.Texture = Globals.ContentManager.Load<Texture2D>("UI Elements/Open Envelope Front");
            backEnvelope.Color = Color.White;
            backEnvelope.Texture = Globals.ContentManager.Load<Texture2D>("UI Elements/Open Envelope Back");
            // Globals.SpriteBatch.Draw(oldScene, Vector2.Zero, Color.White * percentage);
            // Globals.SpriteBatch.Draw(oldScene, Vector2.Zero, Color.White * percentage);
            var envelopeOffset = new Vector2(0, 800) * (1 - (float)Math.Sin((Math.PI / 2) * percentage));
            /*Globals.SpriteBatch.Draw(backEnvelope.Texture, WindowManager.GetMainWindowCenter() + envelopeOffset, null, 
                backEnvelope.Color * (percentage), 0f, backEnvelope.GetSpriteOrigin(), 1f, backEnvelope.SpriteEffects, backEnvelope.DrawLayer);*/
            Globals.SpriteBatch.Draw(closedEnvelope.Texture, WindowManager.GetMainWindowCenter() + envelopeOffset, null,
                closedEnvelope.Color * (percentage), 0f, closedEnvelope.GetSpriteOrigin(), 1f, closedEnvelope.SpriteEffects, closedEnvelope.DrawLayer);
            Globals.SpriteBatch.Draw(newScene, Vector2.Zero, null, Color.White, 0f, Vector2.Zero, 1f, SpriteEffects.None, 0.2f);
/*            Globals.SpriteBatch.Draw(frontEnvelope.Texture, WindowManager.GetMainWindowCenter() + envelopeOffset, null, 
                frontEnvelope.Color * (percentage), 0f, frontEnvelope.GetSpriteOrigin(), 1f, frontEnvelope.SpriteEffects, frontEnvelope.DrawLayer);*/
            // Globals.SpriteBatch.Draw(newScene, Vector2.Zero, Color.White * (float)Math.Cos(Math.PI * percentage));
            

        }
    }
}
