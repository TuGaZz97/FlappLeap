/* 
 * Author : Pin Guillaume
 * Class  : TIS-E1B
 * Date   : 15.01.2018
 * Projet : FlappLeap
 */
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;

namespace FlappLeap
{
    public class AddHighScoreScreen : GameScreen
    {
        private const int MAX_CHAR_PSEUDO = 3;
        private const int NB_TOUCH_LINE = 9;
        private HighScoreManagement highScoreManager;
        private SpriteFont FlappyFont { get; set; }
        private Button AddScore { get; set; }
        private Button BackButton { get; set; }
        private Button Backspace { get; set; }
        private int MouseX { get; set; }
        private int MouseY { get; set; }

        public string Pseudo
        {
            get
            {
                return this.pseudo;
            }
            set
            {
                if (pseudo.Length < MAX_CHAR_PSEUDO || value.Length == 0)
                    this.pseudo = value;
            }
        }

        private List<Button> Touch = new List<Button>();
        private string[] TableauTouch = new string[] { "Q", "W", "E", "R", "T", "Z", "U", "I", "O", "P", "A", "S", "D", "F", "G", "H", "J", "K", "L", "Y", "X", "C", "V", "B", "N", "M" };

        private string pseudo = "";
        private int score = -1;
        private int level = 1;

        private int gameWidth;
        private int gameHeight;
        private int sizeButtonTouch;

        private string spriteFont;
        private string spriteFontButton;

        public AddHighScoreScreen(FlappLeapGame game, int score) : base(game)
        {
            if (score > -1)
                this.score = score;

            gameWidth = game.Graphics.PreferredBackBufferWidth; // 1920
            gameHeight = game.Graphics.PreferredBackBufferHeight; // 1080
            sizeButtonTouch = gameHeight / 10;

            spriteFont = "FontBig";
            spriteFontButton = "FontSmall";
        }

        public AddHighScoreScreen(FlappLeapGame game)
            :this (game, -1)
        {

        }

        public override void Initialize()
        {
            highScoreManager = new HighScoreManagement();
            this.FlappyFont = this.Game.Content.Load<SpriteFont>(spriteFont);

            // Create all buttons
            this.BackButton = new Button(this.Game, "BACK", gameWidth / 2, Convert.ToInt32((gameHeight) / 1.2), sizeButtonTouch * 2, sizeButtonTouch, spriteFontButton);
            this.AddScore = new Button(this.Game, "AJOUTER", gameWidth / 4, Convert.ToInt32((gameHeight) / 1.2), sizeButtonTouch * 3, sizeButtonTouch, spriteFontButton);
            this.Backspace = new Button(this.Game, "DEL", gameWidth / NB_TOUCH_LINE + (Convert.ToInt32(sizeButtonTouch * 1.5) * Convert.ToInt32(TableauTouch.Length - 18.5)), Convert.ToInt32(gameHeight / 1.6), Convert.ToInt32(sizeButtonTouch * 1.5), sizeButtonTouch, spriteFontButton);
            for (int i = 0; i < TableauTouch.Length; i++)
            {
                if (i <= NB_TOUCH_LINE)
                {
                    Touch.Add(new Button(this.Game, TableauTouch[i], gameWidth / NB_TOUCH_LINE + (Convert.ToInt32(sizeButtonTouch * 1.5) * i), Convert.ToInt32(gameHeight / 3), sizeButtonTouch, sizeButtonTouch, spriteFontButton));
                }
                else if (i > NB_TOUCH_LINE && i <= NB_TOUCH_LINE + NB_TOUCH_LINE)
                {
                    Touch.Add(new Button(this.Game, TableauTouch[i], gameWidth / NB_TOUCH_LINE + Convert.ToInt32(Convert.ToInt32(sizeButtonTouch * 1.5) * (i - NB_TOUCH_LINE)), Convert.ToInt32(gameHeight / 2.1), sizeButtonTouch, sizeButtonTouch, spriteFontButton));
                }
                else
                {
                    Touch.Add(new Button(this.Game, TableauTouch[i], gameWidth / NB_TOUCH_LINE + Convert.ToInt32(Convert.ToInt32(sizeButtonTouch * 1.5) * (i - NB_TOUCH_LINE * 2)), Convert.ToInt32(gameHeight / 1.6), sizeButtonTouch, sizeButtonTouch, spriteFontButton));
                }

            }

            this.MouseX = PlayScreen.MouseX;
            this.MouseY = PlayScreen.MouseY;

            // Event for all buttons
            this.AddScore.Click += this.Add_Click;
            this.BackButton.Click += (s, e) => this.FlappLeapGame.ChangeScreen(typeof(TitleScreen));
            this.Backspace.Click += this.Del_Click;
            foreach (var item in Touch)
            {
                item.Click += (s, e) => Pseudo += item.Text;
            }

            // Display all buttons
            this.Game.Components.Add(this.Backspace);
            this.Game.Components.Add(this.AddScore);
            this.Game.Components.Add(this.BackButton);
            for (int i = 0; i < Touch.Count; i++)
            {
                this.Game.Components.Add(this.Touch[i]);
            }

            base.Initialize();
        }

        /// <summary>
        /// Remove the last char of the pseudo
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Del_Click(object sender, MouseState e)
        {
            if (pseudo.Length >= 1)
            {
                pseudo = pseudo.Remove(pseudo.Length - 1);
            }
        }
        /// <summary>
        /// Add the score
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Add_Click(object sender, MouseState e)
        {
            if (pseudo.Length == MAX_CHAR_PSEUDO)
            {
                highScoreManager.AddHighScore(this.pseudo, this.score, this.level);
                this.FlappLeapGame.ChangeScreen(typeof(TitleScreen));
            }
        }

        public override void Update(GameTime gameTime)
        {
            this.AddScore.Enabled = pseudo.Length == MAX_CHAR_PSEUDO;
            if (pseudo.Length == 3)
            {
                foreach (var item in Touch)
                {
                    item.Enabled = false;
                }
            }
            else
            {
                foreach (var item in Touch)
                {
                    item.Enabled = true;
                }
            }
            base.Update(gameTime);
        }

        public override void Draw(GameTime gameTime)
        {
            this.Sb.Begin();
            this.Sb.DrawString(this.FlappyFont, "SCORE  :", new Vector2(Convert.ToInt32(gameWidth / 4), Convert.ToInt32(gameHeight / 20)), Color.White);
            this.Sb.DrawString(this.FlappyFont, score.ToString(), new Vector2(Convert.ToInt32(gameWidth / 2), Convert.ToInt32(gameHeight / 20)), Color.White);
            this.Sb.DrawString(this.FlappyFont, "NIVEAU :", new Vector2(Convert.ToInt32(gameWidth / 4), Convert.ToInt32(gameHeight / 7)), Color.White);
            this.Sb.DrawString(this.FlappyFont, level.ToString(), new Vector2(Convert.ToInt32(gameWidth / 2), Convert.ToInt32(gameHeight / 7)), Color.White);
            this.Sb.DrawString(this.FlappyFont, "PSEUDO :", new Vector2(Convert.ToInt32(gameWidth / 4), Convert.ToInt32(gameHeight / 4.5)), Color.White);
            this.Sb.DrawString(this.FlappyFont, Pseudo, new Vector2(Convert.ToInt32(gameWidth / 2), Convert.ToInt32(gameHeight / 4.5)), Color.White);
            this.Sb.End();

            KeyboardState state = Keyboard.GetState();

            if (state.IsKeyDown(Keys.D))
            {
                this.MouseX += TitleScreen.CURSOR_SPEED;
                Mouse.SetPosition(this.MouseX, this.MouseY);

            }

            if (state.IsKeyDown(Keys.S))
            {
                this.MouseY += TitleScreen.CURSOR_SPEED;
                Mouse.SetPosition(this.MouseX, this.MouseY);
            }

            if (state.IsKeyDown(Keys.A))
            {
                this.MouseX -= TitleScreen.CURSOR_SPEED;
                Mouse.SetPosition(this.MouseX, this.MouseY);
            }

            if (state.IsKeyDown(Keys.W))
            {
                this.MouseY -= TitleScreen.CURSOR_SPEED;
                Mouse.SetPosition(this.MouseX, this.MouseY);
            }

            base.Draw(gameTime);
        }

        protected override void Dispose(bool disposing)
        {
            // Remove all buttons
            this.Game.Components.Remove(this.AddScore);
            this.Game.Components.Remove(this.BackButton);
            this.Game.Components.Remove(this.Backspace);
            for (int i = 0; i < Touch.Count; i++)
            {
                this.Game.Components.Remove(this.Touch[i]);
            }
            base.Dispose(disposing);
        }
    }
}
