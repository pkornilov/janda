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
    public class About : Microsoft.Xna.Framework.DrawableGameComponent
    {
        private SpriteBatch spriteBatch;
        private SpriteFont spriteFont;
        private Vector2 position;
        private string about; // about text
        private string item; // navigation item (go to menu)

        public About(Game game, SpriteBatch spriteBatch,
            SpriteFont spriteFont,
            Vector2 position)
            : base(game)
        {
            this.spriteBatch = spriteBatch;
            this.spriteFont = spriteFont;
            this.position = position;
            about = "Janda - Flag Quiz\r\n\r\n" +
                "Made by Philippe Kornilov\r\n" +
                "(c) Copyright, 2015";
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
            spriteBatch.DrawString(spriteFont, about, tempPosition, Color.White);
            tempPosition.Y += spriteFont.LineSpacing * 5; // 5 - number of lines to skip
            spriteBatch.DrawString(spriteFont, item, tempPosition, Color.DeepSkyBlue);
            spriteBatch.End();

            base.Draw(gameTime);
        }

        // Shows and hides the About screen
        public void Shown(bool action)
        {
            this.Enabled = action;
            this.Visible = action;
        }
    }
}
