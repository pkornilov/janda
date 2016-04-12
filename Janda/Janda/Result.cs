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
    public class Result : Microsoft.Xna.Framework.DrawableGameComponent
    {
        private SpriteBatch spriteBatch;
        private SpriteFont spriteFont; //regular text font (18pt)
        private SpriteFont scoreFont; //result screen - score font (48pt)
        private Vector2 position; //position of whole block
        private int score;
        private int correct;
        private int incorrect;

        public int Score { get { return score; } set { score = value; } }
        public int Correct { get { return correct; } set { correct = value; } }
        public int Incorrect { get { return incorrect; } set { incorrect = value; } }

        //navigation items (Play Again, Go to Menu)
        List<string> items = new List<string>();
        int index = 0; //selected index (0 - Play Again)
        public int Index { get { return index; } set { index = value; } }
        private const int BUTTONINDENT = 25; //indention between navigation items

        public Result(Game game, SpriteBatch spriteBatch,
            SpriteFont spriteFont,
            SpriteFont scoreFont,
            Vector2 position,
            int score,
            int correct,
            int incorrect)
            : base(game)
        {
            this.spriteBatch = spriteBatch;
            this.spriteFont = spriteFont;
            this.scoreFont = scoreFont;
            this.position = position;
            this.score = score;
            this.correct = correct;
            this.incorrect = incorrect;
            items.Add("Play Again");
            items.Add("Go to Menu");
        }

        public override void Initialize()
        {
            base.Initialize();
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);

            KeyboardState ks = Keyboard.GetState();
            if (ks.IsKeyDown(Keys.Left))
            {
                index--;
                if (index == -1)
                    index = 0;
            }
            if (ks.IsKeyDown(Keys.Right))
            {
                index++;
                if (index == items.Count)
                    index = 1;
            }
        }

        public override void Draw(GameTime gameTime)
        {
            //position to iterate through draw strings (incl. navigation items)
            Vector2 tempPosition = position;

            spriteBatch.Begin();

            spriteBatch.DrawString(spriteFont, "Score:", tempPosition, Color.White);
            tempPosition.Y += spriteFont.LineSpacing; //next line, regular spacing
            spriteBatch.DrawString(scoreFont, score.ToString(), tempPosition, Color.White);
            tempPosition.Y += scoreFont.LineSpacing; //next line, score (big) spacing
            spriteBatch.DrawString(spriteFont, "Correct: " + correct.ToString(), tempPosition, Color.White);
            tempPosition.Y += spriteFont.LineSpacing; //next line, regular spacing
            spriteBatch.DrawString(spriteFont, "Incorrect: " + incorrect.ToString(), tempPosition, Color.White);
            tempPosition.Y += spriteFont.LineSpacing * 2; //skip one line, regular spacing

            //navigation selection logic; selected - blue, unselected - white
            for (int i = 0; i < items.Count; i++)
                if (index == i)
                {
                    spriteBatch.DrawString(spriteFont, items[i], tempPosition, Color.DeepSkyBlue);
                    tempPosition.X += (float)spriteFont.MeasureString(items[i].ToString()).X + BUTTONINDENT;
                }
                else
                {
                    spriteBatch.DrawString(spriteFont, items[i], tempPosition, Color.White);
                    tempPosition.X += (float)spriteFont.MeasureString(items[i].ToString()).X + BUTTONINDENT;
                }

            spriteBatch.End();

            base.Draw(gameTime);
        }

        //show and hide result screen
        public void Shown(bool action)
        {
            this.Enabled = action;
            this.Visible = action;
        }
    }
}
