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
    public class FlagScroll : Microsoft.Xna.Framework.DrawableGameComponent
    {
        private SpriteBatch spriteBatch;
        private Texture2D tex;
        private Rectangle rect1; // Rectangle for first column of texture
        private Rectangle rect2; // Rectangle for second column of texture
        private Vector2 position1; // Position for rect1
        private Vector2 position2; // Position for rect2
        private Vector2 speed;

        public FlagScroll(Game game, SpriteBatch spriteBatch,
            Texture2D tex,
            Rectangle rect1,
            Rectangle rect2,
            Vector2 position1,
            Vector2 position2,
            Vector2 speed)
            : base(game)
        {
            this.spriteBatch = spriteBatch;
            this.tex = tex;
            this.rect1 = rect1;
            this.rect2 = rect2;
            this.position1 = position1;
            this.position2 = position2;
            this.speed = speed;
        }

        public override void Initialize()
        {
            base.Initialize();
        }

        public override void Update(GameTime gameTime)
        {
            // Speeds up both rectangles
            position1 += speed;
            position2 += speed;

            // if rect1 is out of boundries of the screen, put rect2 after rect1
            if (position1.Y < -tex.Height / 2)
                position2.Y = position1.Y + tex.Height;
            // if rect2 is out of boundries of the screen, put rect1 after rect2
            if (position2.Y < -tex.Height / 2)
                position1.Y = position2.Y + tex.Height;
            
            base.Update(gameTime);
        }

        public override void Draw(GameTime gameTime)
        {
            spriteBatch.Begin();
            spriteBatch.Draw(tex, position1, rect1, Color.DarkGray);
            spriteBatch.Draw(tex, position2, rect2, Color.DarkGray);
            spriteBatch.End();

            base.Draw(gameTime);
        }

        // Shows and hides flag scroll
        public void Shown(bool action)
        {
            this.Enabled = action;
            this.Visible = action;
        }
    }
}
