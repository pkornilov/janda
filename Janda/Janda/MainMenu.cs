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
    public class MainMenu : Microsoft.Xna.Framework.DrawableGameComponent
    {
        private SpriteBatch spriteBatch;
        private SpriteFont spriteFont;
        private Vector2 position;

        private const int LINESPACE = 5; // additional spacing between lines

        // list of navigation menu items
        List<string> items = new List<string>();
        int index = 0; //selected index (0 - Play, 1 - Help, etc.)
        public int Index { get { return index; } set { index = value; } }

        KeyboardState kso = Keyboard.GetState(); //old keyboard state

        public MainMenu(Game game, SpriteBatch spriteBatch,
            SpriteFont spriteFont,
            Vector2 position)
            : base(game)
        {
            this.spriteBatch = spriteBatch;
            this.spriteFont = spriteFont;
            this.position = position;
            items.Add("Play");
            items.Add("Help");
            items.Add("How to Play");
            items.Add("About");
            items.Add("Exit");
        }

        public override void Initialize()
        {
            base.Initialize();
        }

        public override void Update(GameTime gameTime)
        {
            KeyboardState ks = Keyboard.GetState();
            if (ks.IsKeyDown(Keys.Down) && kso.IsKeyUp(Keys.Down))
            {
                index++;
                if (index == items.Count)
                    index = 0;
            }
            if (ks.IsKeyDown(Keys.Up) && kso.IsKeyUp(Keys.Up))
            {
                index--;
                if (index == -1)
                    index = items.Count - 1;
            }
            kso = ks;

            base.Update(gameTime);
        }

        public override void Draw(GameTime gameTime)
        {
            spriteBatch.Begin();

            Vector2 itemPosition = position; //position of each item; to be iterated
            Color itemColor = Color.White; //colour of each item; selected to be blue

            for (int i = 0; i < items.Count; i++)
            {
                if (index == i)
                    itemColor = Color.DeepSkyBlue; //selected item
                else
                    itemColor = Color.White; //unselected items

                if (i > 0) // if not first item (Play), add line spacing
                    itemPosition.Y += spriteFont.LineSpacing + LINESPACE;

                spriteBatch.DrawString(spriteFont, items[i], itemPosition, itemColor);
            }

            spriteBatch.End();

            base.Draw(gameTime);
        }

        //show and hide main menu screen
        public void Shown(bool action)
        {
            this.Enabled = action;
            this.Visible = action;
        }
        
    }
}
