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
using System.Xml;
using DataTypes;

namespace Janda
{
    public class Game1 : Microsoft.Xna.Framework.Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
        SpriteFont spriteFont; // regular text font, 18pt
        SpriteFont scoreFont; // result score text font, 46pt
        Color background = Color.DarkGray; //game background
        GameState gs = GameState.Menu; //current game state
        EnterReleased er = EnterReleased.Yes; //enter key state (described in DataTypes.cs)

        // sound effects for correct/incorrect answers and navigation
        SoundEffect correct;
        SoundEffect incorrect;
        SoundEffect select;

        Texture2D tex; //flags texture
        Texture2D hiTex;//highlight texture (1x1 white, just because of xna..)
        Rectangle rect1; //rectangle for left flag
        Rectangle rect2; //rectangle for right flag

        Country[] countries; //array of countries (contains name and rectangle location data)
        Random r = new Random(); //randomizer
        Choose choose; //choice indicator; to be randomized (described in DataTypes.cs)

        MainMenu menu;
        Quiz quiz;
        Result result;
        Help help;
        HowToPlay howToPlay;
        About about;
        FlagScroll scroll;

        const float GAMEDURATION = 59; // seconds
        const float TIMER = 1000; // milliseconds between questions
        const int ONESCORE = 100; // points for single question

        const int FLAGWIDTH = 119; // pixels
        const int FLAGHEIGHT = 59; // pixels
        const int WINDOWWIDTH = 480; // pixels
        const int WINDOWHEIGHT = 300; // pixels

        const int SCROLLX = 340; // position; pixels
        const int COUNTRYTEXTY = 200; // position.Y; pixels
        const int MENUINDENT = 50; // position.X, left indent; pixels
        const int ITEMINDENT = 30; // position.X, left indent; pixels

        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            graphics.IsFullScreen = false;
            graphics.PreferredBackBufferWidth = WINDOWWIDTH; //sets window width
            graphics.PreferredBackBufferHeight = WINDOWHEIGHT; //sets window height

            Content.RootDirectory = "Content";
        }

        protected override void Initialize()
        {
            base.Initialize();
            IsMouseVisible = true;
            ShowMenu(); //shows menu as game opens
        }

        protected override void LoadContent()
        {
            spriteBatch = new SpriteBatch(GraphicsDevice);

            //load fonts, country data, textures and sounds
            spriteFont = Content.Load<SpriteFont>("fonts/arial-bold-18pt");
            scoreFont = Content.Load<SpriteFont>("fonts/arial-bold-46pt");
            countries = Content.Load<Country[]>("data/countries-data");
            tex = Content.Load<Texture2D>("images/world-flags");
            hiTex = Content.Load<Texture2D>("images/white");
            correct = Content.Load<SoundEffect>("sounds/correct");
            incorrect = Content.Load<SoundEffect>("sounds/incorrect");
            select = Content.Load<SoundEffect>("sounds/select");

            rect1 = new Rectangle(0, 0, FLAGWIDTH, FLAGHEIGHT); //left flag rect
            rect2 = new Rectangle(0, 0, FLAGWIDTH, FLAGHEIGHT); //right flag rect
            quiz = new Quiz(this, spriteBatch, spriteFont, tex, hiTex, rect1, rect2);
            quiz.ScoreColor = Color.White;
            quiz.Duration = GAMEDURATION; //duration of quiz

            result = new Result(this, spriteBatch, spriteFont, scoreFont, new Vector2(ITEMINDENT,
                graphics.PreferredBackBufferHeight / 2 - (spriteFont.LineSpacing * 5 +
                scoreFont.LineSpacing) / 2), quiz.Score, quiz.Correct, quiz.Incorrect);

            menu = new MainMenu(this, spriteBatch, spriteFont, new Vector2(MENUINDENT,
                graphics.PreferredBackBufferHeight / 2 - ((spriteFont.LineSpacing + 5) * 5 / 2)));

            scroll = new FlagScroll(this, spriteBatch, tex, new Rectangle(0, 0, FLAGWIDTH, tex.Height),
                new Rectangle(FLAGWIDTH + 1, 0, FLAGWIDTH, tex.Height), new Vector2(SCROLLX, 0),
                new Vector2(SCROLLX, tex.Height), new Vector2(0, -1));

            help = new Help(this, spriteBatch, spriteFont, new Vector2(ITEMINDENT,
                graphics.PreferredBackBufferHeight / 2 - spriteFont.LineSpacing * 6 / 2));

            howToPlay = new HowToPlay(this, spriteBatch, spriteFont, new Vector2(ITEMINDENT,
                graphics.PreferredBackBufferHeight / 2 - spriteFont.LineSpacing * 6 / 2));

            about = new About(this, spriteBatch, spriteFont, new Vector2(ITEMINDENT,
                graphics.PreferredBackBufferHeight / 2 - spriteFont.LineSpacing * 6 / 2));

            Components.Add(quiz);
            Components.Add(result);
            Components.Add(menu);
            Components.Add(scroll);
            Components.Add(help);
            Components.Add(howToPlay);
            Components.Add(about);
        }

        protected override void UnloadContent() { }

        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed)
                this.Exit();

            KeyboardState ks = Keyboard.GetState();

            //if quiz is in progress, update timer and its location constantly
            // (transition - pause between questions)
            if (gs == GameState.Quiz || gs == GameState.Transition)
            {
                quiz.Duration -= (float)gameTime.ElapsedGameTime.TotalSeconds;
                quiz.ScorePosition = new Vector2(WINDOWWIDTH -
                    spriteFont.MeasureString(quiz.Score.ToString() + " ").X, 0);
            }

            //counts milliseconds during transition (between questions)
            if (gs == GameState.Transition)
                quiz.Timer -= (float)gameTime.ElapsedGameTime.TotalMilliseconds;

            //if quiz is in progress
            if (gs == GameState.Quiz)
            {
                if (ks.IsKeyDown(Keys.Left)) //if left arrow key pressed
                {
                    if (choose == Choose.Left)
                    {
                        quiz.Score += ONESCORE; //update score (+)
                        quiz.HiColor1 = Color.LimeGreen; //highlight correct flag
                        quiz.ScoreColor = Color.LimeGreen; //highlight score
                        quiz.Correct++; //update correct answer count
                        correct.Play(); //play correct sound
                    }
                    else
                    {
                        quiz.Score -= ONESCORE; //update score (-)
                        quiz.HiColor1 = Color.OrangeRed;//highlight incorrect flag
                        quiz.ScoreColor = Color.OrangeRed; //highlight score
                        quiz.Incorrect++;//update incorrect answer count
                        incorrect.Play();//play incorrect sound
                    }
                    gs = GameState.Transition; //enter transition (pause between questions)
                    quiz.Timer = TIMER; //set transition timer
                }

                if (ks.IsKeyDown(Keys.Right)) //if right arrow key pressed
                {
                    if (choose == Choose.Right)
                    {
                        quiz.Score += ONESCORE;//update score (+)
                        quiz.HiColor2 = Color.LimeGreen; //highlight correct flag
                        quiz.ScoreColor = Color.LimeGreen;//highlight score
                        quiz.Correct++;//update correct answer count
                        correct.Play(); //play correct sound
                    }
                    else
                    {
                        quiz.Score -= ONESCORE;//update score (-)
                        quiz.HiColor2 = Color.OrangeRed;//highlight incorrect flag
                        quiz.ScoreColor = Color.OrangeRed;//highlight score
                        quiz.Incorrect++;//update incorrect answer count
                        incorrect.Play();//play incorrect sound
                    }
                    gs = GameState.Transition;//enter transition (pause between questions)
                    quiz.Timer = TIMER; //set transition timer
                }
            }

            // if gamestate set to menu, show it
            if (gs == GameState.Menu)
            {
                ShowMenu();
            }

            // if quiz/transition (pause between questions) in progress and time is up,
            // show results (score, correct, incorrect) and play sound
            if ((gs == GameState.Quiz || gs == GameState.Transition) && quiz.Duration <= 0)
            {
                ShowResult();
                select.Play();
            }

            //if results screen is up
            if (gs == GameState.Result)
            {
                //make navigation selection for play again
                if (result.Index == 0 && ks.IsKeyDown(Keys.Enter))
                {
                    StartQuiz();
                    select.Play();
                }
                //make navigation selection for go to menu
                if (result.Index == 1 && ks.IsKeyDown(Keys.Enter))
                {
                    ShowMenu();
                    select.Play();
                    er = EnterReleased.No; //set er to No until Enter key is released
                }
            }

            // if help / how to play / about screen is up
            if (gs == GameState.Help || gs==GameState.HowToPlay || gs==GameState.About)
            {
                if (ks.IsKeyUp(Keys.Enter)) //as Enter key is released, set er so
                {
                    er = EnterReleased.Yes;
                }
                //make navigation selection for go to menu
                if (er == EnterReleased.Yes && ks.IsKeyDown(Keys.Enter))
                {
                    ShowMenu();
                    select.Play();
                    er = EnterReleased.No; //set er to No until Enter key is released
                }
            }

            // if main menu screen is up
            if (gs == GameState.Menu)
            {
                if (ks.IsKeyUp(Keys.Enter))
                {
                    er = EnterReleased.Yes;//as Enter key is released, set er so
                }
                //make navigation selection for play (start quiz)
                if (menu.Index == 0 && er == EnterReleased.Yes && ks.IsKeyDown(Keys.Enter))
                {
                    StartQuiz();
                    select.Play();
                }
                //make navigation selection for show help
                if (menu.Index == 1 && er == EnterReleased.Yes && ks.IsKeyDown(Keys.Enter))
                {
                    ShowHelp();
                    select.Play();
                    er = EnterReleased.No; //set er to No until Enter key is released
                }
                //make navigation selection for show how to play
                if (menu.Index == 2 && er == EnterReleased.Yes && ks.IsKeyDown(Keys.Enter))
                {
                    ShowHowToPlay();
                    select.Play();
                    er = EnterReleased.No; //set er to No until Enter key is released
                }
                //make navigation selection for about info
                if (menu.Index == 3 && er == EnterReleased.Yes && ks.IsKeyDown(Keys.Enter))
                {
                    ShowAbout();
                    select.Play();
                    er = EnterReleased.No; //set er to No until Enter key is released
                }
                //make navigation selection for application exit
                if (menu.Index == 4 && ks.IsKeyDown(Keys.Enter))
                {
                    this.Exit();
                }
            }

            //if game is in transition state (pause between questions), or it's the first
            // question in quiz
            if ((gs == GameState.Transition && quiz.Timer <= 0) || gs == GameState.QuizFirst)
            {
                int country1 = r.Next(0, countries.Count()); //left flag randomized
                int country2 = country1; //right flag equals to left flag
                // while right flag equals to left flag, randomize right flag to it doesn't
                while (country2 == country1) { country2 = r.Next(0, countries.Count()); }

                int select = r.Next(0, 2); //make a random choice between left and right flag
                int chooseText; //to remember ID (int) of the random choice above
                //set global 'choose' in accordance with randomized choice (left/right),
                // as well as the ID ('chooseText') to draw the flag
                if (select == 0) { choose = Choose.Left; chooseText = country1; }
                else { choose = Choose.Right; chooseText = country2; }

                //locations of rectangles of left and right flags are set
                quiz.Rect1 = new Rectangle(countries[country1].X, countries[country1].Y, FLAGWIDTH, FLAGHEIGHT);
                quiz.Rect2 = new Rectangle(countries[country2].X, countries[country2].Y, FLAGWIDTH, FLAGHEIGHT);

                //set country name text (seen under the two flags on the quiz screen)
                quiz.Text = countries[chooseText].Name;
                Vector2 countryTextDim = spriteFont.MeasureString(quiz.Text);
                double countryTextPosX = graphics.PreferredBackBufferWidth / 2 - countryTextDim.X / 2;
                quiz.TextPosition = new Vector2(Convert.ToInt32(countryTextPosX), COUNTRYTEXTY);

                //highlight for left and right flags is to default; score text colour to white
                quiz.HiColor1 = Color.DarkGray;
                quiz.HiColor2 = Color.DarkGray;
                quiz.ScoreColor = Color.White;

                gs = GameState.Quiz; //enter quiz game state
            }

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(background);

            base.Draw(gameTime);
        }

        //start quiz method
        protected void StartQuiz()
        {
            gs = GameState.QuizFirst;
            background = Color.DarkGray;

            result.Shown(false);
            menu.Shown(false);
            scroll.Shown(false);
            howToPlay.Shown(false);
            about.Shown(false);

            quiz.Shown(true);
            quiz.Duration = GAMEDURATION;
            quiz.Score = new int();
            quiz.Correct = new int();
            quiz.Incorrect = new int();
            quiz.HiColor1 = Color.DarkGray;
            quiz.HiColor2 = Color.DarkGray;
            quiz.ScoreColor = Color.White;
        }

        //show results screen method
        protected void ShowResult()
        {
            gs = GameState.Result;
            background = Color.DimGray;

            quiz.Shown(false);
            menu.Shown(false);
            scroll.Shown(true);
            help.Shown(false);
            howToPlay.Shown(false);
            about.Shown(false);

            result.Shown(true);
            result.Score = quiz.Score;
            result.Correct = quiz.Correct;
            result.Incorrect = quiz.Incorrect;
            result.Index = 0;
        }

        //show main menu screen method
        protected void ShowMenu()
        {
            gs = GameState.Menu;
            background = Color.DimGray;

            quiz.Shown(false);
            result.Shown(false);
            scroll.Shown(true);
            help.Shown(false);
            howToPlay.Shown(false);
            about.Shown(false);

            menu.Shown(true);
        }

        //show help screen method
        protected void ShowHelp()
        {
            gs = GameState.Help;
            background = Color.DimGray;

            quiz.Shown(false);
            result.Shown(false);
            scroll.Shown(true);
            menu.Shown(false);
            howToPlay.Shown(false);
            about.Shown(false);

            help.Shown(true);
        }

        //show how to play screen method
        protected void ShowHowToPlay()
        {
            gs = GameState.HowToPlay;
            background = Color.DimGray;

            quiz.Shown(false);
            result.Shown(false);
            scroll.Shown(true);
            menu.Shown(false);
            help.Shown(false);
            about.Shown(false);

            howToPlay.Shown(true);
        }

        //show about screen method
        protected void ShowAbout()
        {
            gs = GameState.About;
            background = Color.DimGray;

            quiz.Shown(false);
            result.Shown(false);
            scroll.Shown(true);
            menu.Shown(false);
            help.Shown(false);
            howToPlay.Shown(false);

            about.Shown(true);
        }
    }
}