using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Janda;

namespace Janda
{
    public class Quiz : Microsoft.Xna.Framework.DrawableGameComponent
    {
        private SpriteBatch spriteBatch;
        private SpriteFont spriteFont;
        private Texture2D tex; // flags textures
        private Rectangle rect1; // rectangle for left flag
        private Rectangle rect2; // rectangle for right flag
        public Rectangle Rect1 { get { return rect1; } set { rect1 = value; } }
        public Rectangle Rect2 { get { return rect2; } set { rect2 = value; } }
        protected Vector2 position1 = new Vector2(90, 90); // constant position of left flag
        protected Vector2 position2 = new Vector2(270, 90); // constant position of right flag

        private Rectangle hiRect1 = new Rectangle(70, 70, 159, 99); // highlight left flag rect
        private Rectangle hiRect2 = new Rectangle(250, 70, 159, 99);// highlight right flag rect
        private Texture2D hiTex; // highlight texture (white 1x1 image, just because of xna..)
        private Color hiColor1 = Color.DimGray;//default left flag highlight colour - same as background
        private Color hiColor2 = Color.DimGray;//default right flag highlight colour - same as background
        public Color HiColor1 { get { return hiColor1; } set { hiColor1 = value; } }
        public Color HiColor2 { get { return hiColor2; } set { hiColor2 = value; } }

        private Vector2 textPosition; // country name location
        private string text = ""; // country name text
        public string Text { get { return text; } set { text = value; } }
        public Vector2 TextPosition { get { return textPosition; } set { textPosition = value; } }

        private float timer; // timer for transition between questions
        private float duration; // duration of the quiz
        private int score; //score quiz
        private int correct; // number of correct answers
        private int incorrect; //number of incorrect answers
        private Color scoreColor; //score drawstring colour
        private Vector2 scorePosition; //score drawstring location
        public float Timer { get { return timer; } set { timer = value; } }
        public float Duration { get { return duration; } set { duration = value; } }
        public int Score { get { return score; } set { score = value; } }
        public int Correct { get { return correct; } set { correct = value; } }
        public int Incorrect { get { return incorrect; } set { incorrect = value; } }
        public Color ScoreColor { get { return scoreColor; } set { scoreColor = value; } }
        public Vector2 ScorePosition { get { return scorePosition; } set { scorePosition = value; } }
        
        public Quiz(Game game, SpriteBatch spriteBatch,
            SpriteFont spriteFont,
            Texture2D tex,
            Texture2D hiTex,
            Rectangle rect1,
            Rectangle rect2)
            : base(game)
        {
            this.spriteBatch = spriteBatch;
            this.spriteFont = spriteFont;
            this.tex = tex;
            this.hiTex = hiTex;
            this.rect1 = rect1;
            this.rect2 = rect2;
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
            spriteBatch.Begin();
            spriteBatch.Draw(hiTex, hiRect1, hiColor1);
            spriteBatch.Draw(hiTex, hiRect2, hiColor2);
            spriteBatch.Draw(tex, position1, rect1, Color.White);
            spriteBatch.Draw(tex, position2, rect2, Color.White);
            spriteBatch.DrawString(spriteFont, text, textPosition, Color.White);
            spriteBatch.DrawString(spriteFont, duration.ToString(" 0:00"), Vector2.Zero, Color.White);
            spriteBatch.DrawString(spriteFont, score.ToString(), scorePosition, scoreColor);
            spriteBatch.End();

            base.Draw(gameTime);
        }

        public void Shown(bool action)
        {
            this.Enabled = action;
            this.Visible = action;
        }
    }
}
