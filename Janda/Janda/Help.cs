using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;

namespace Janda
{
    public class Help : Microsoft.Xna.Framework.DrawableGameComponent
    {
        private SpriteBatch spriteBatch;
        private SpriteFont spriteFont;
        private Vector2 position;
        private string help; // help text
        private string item; // navigation item (go to menu)

        public Help(Game game, SpriteBatch spriteBatch,
            SpriteFont spriteFont,
            Vector2 position)
            : base(game)
        {
            this.spriteBatch = spriteBatch;
            this.spriteFont = spriteFont;
            this.position = position;
            help = "Janda is an educational\r\n" + 
                   "quiz game. The goal is\r\n" +
                   "to choose the correct\r\n" + 
                   "one between two flags.";
            item = "Go to Menu";
        }

        public override void Initialize()
        {
            base.Initialize();
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
        }

        public override void Draw(GameTime gameTime)
        {
            // position to iterate through draw string
            Vector2 tempPosition = position;

            spriteBatch.Begin();
            spriteBatch.DrawString(spriteFont, help, tempPosition, Color.White);
            tempPosition.Y += spriteFont.LineSpacing * 5; // 5 - number of lines to skip
            spriteBatch.DrawString(spriteFont, item, tempPosition, Color.DeepSkyBlue);
            spriteBatch.End();

            base.Draw(gameTime);
        }

        // Show and hide help
        public void Shown(bool action)
        {
            this.Enabled = action;
            this.Visible = action;
        }
    }
}
