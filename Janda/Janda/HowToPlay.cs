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
    public class HowToPlay : Microsoft.Xna.Framework.DrawableGameComponent
    {
        private SpriteBatch spriteBatch;
        private SpriteFont spriteFont;
        private Vector2 position;
        private string howToPlay; // how to play text
        private string item; // navigation item (go to menu)

        public HowToPlay(Game game, SpriteBatch spriteBatch,
            SpriteFont spriteFont,
            Vector2 position)
            : base(game)
        {
            this.spriteBatch = spriteBatch;
            this.spriteFont = spriteFont;
            this.position = position;
            howToPlay = "To choose between flags\r\n" +
                "use Left/Right arrow keys\r\n" + 
                "respectively. Use arrow and\r\n" + 
                "Enter keys to navigate.";
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
            // position to iterate through draw strings
            Vector2 tempPosition = position;

            spriteBatch.Begin();
            spriteBatch.DrawString(spriteFont, howToPlay, tempPosition, Color.White);
            tempPosition.Y += spriteFont.LineSpacing * 5; // 5 - number of lines to skip
            spriteBatch.DrawString(spriteFont, item, tempPosition, Color.DeepSkyBlue);
            spriteBatch.End();

            base.Draw(gameTime);
        }

        //show and hide how to play screen
        public void Shown(bool action)
        {
            this.Enabled = action;
            this.Visible = action;
        }
    }
}
