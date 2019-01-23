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
    public class HighScoreScreen : GameScreen
    {
        private HighScoreManagement highScoreManager;
        private SpriteFont FlappyFont { get; set; }

        private Button Up { get; set; }
        private Button Down { get; set; }
        private Button BackButton { get; set; }

        private int maxDisplay;
        private int startDisplay = 0;
        int floorDistance = 50;

        private int gameWidth;
        private int gameHeight;
        private int sizeButtonTouch;

        private string spriteFontButton;

        List<HighScore> ReadHighScores = new List<HighScore>();
        public HighScoreScreen(FlappLeapGame game, bool multiplayer = false) : base(game)
        {
            gameWidth = game.Graphics.PreferredBackBufferWidth; //1920
            gameHeight = game.Graphics.PreferredBackBufferHeight; // 1080
            sizeButtonTouch = gameHeight / 10;
            // Select font according to resolution
            if (gameWidth == Constants.GAME_WIDTH)
            {
                spriteFontButton = "FontBig";
                maxDisplay = 15;
            }
            else
            {
                spriteFontButton = "FontSmall";
                maxDisplay = 10;
            }
        }

        public override void Initialize()
        {
            highScoreManager = new HighScoreManagement();
            this.FlappyFont = this.Game.Content.Load<SpriteFont>(spriteFontButton);
            // Fills the hight score list
            foreach (var Score in highScoreManager.ReadHighScores())
            {
                this.ReadHighScores.Add(Score);
            }
            // Add all buttons
            this.BackButton = new Button(this.Game, "BACK", Convert.ToInt32(gameWidth/4.5), Convert.ToInt32(gameHeight/1.2), sizeButtonTouch*3, sizeButtonTouch, spriteFontButton);
            this.Up = new Button(this.Game, "UP", Convert.ToInt32(gameWidth / 2.5), Convert.ToInt32(gameHeight / 1.2), sizeButtonTouch*3, sizeButtonTouch, spriteFontButton);
            this.Down = new Button(this.Game, "DOWN", Convert.ToInt32(gameWidth / 1.727), Convert.ToInt32(gameHeight / 1.2), sizeButtonTouch*3, sizeButtonTouch, spriteFontButton);
            // Event for all buttons
            this.Up.Click += this.Up_Click;
            this.Down.Click += this.Down_Click;
            this.BackButton.Click += (s, e) => this.FlappLeapGame.ChangeScreen(typeof(TitleScreen));
            // Display all buttons
            this.Game.Components.Add(this.BackButton);
            this.Game.Components.Add(this.Up);
            this.Game.Components.Add(this.Down);

            base.Initialize();
        }
        /// <summary>
        /// Go down in the list of the best scores
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Down_Click(object sender, MouseState e)
        {
            if (startDisplay < (highScoreManager.ReadHighScores().Count - maxDisplay))
            {
                startDisplay += 5;
            }
        }

        /// <summary>
        /// Go up in the list of the best scores
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Up_Click(object sender, MouseState e)
        {
            if (startDisplay > 0)
                startDisplay -=5; 
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
        }

        public override void Draw(GameTime gameTime)
        {
            this.Sb.Begin();
            this.Sb.DrawString(this.FlappyFont, "POS", new Vector2(gameWidth/70, gameHeight/18), Color.White);
            this.Sb.DrawString(this.FlappyFont, "PSEUDO", new Vector2(gameWidth / 9, gameHeight / 18), Color.White);
            this.Sb.DrawString(this.FlappyFont, "SCORE", new Vector2(Convert.ToInt32(gameWidth / 3.5), gameHeight / 18), Color.White);
            this.Sb.DrawString(this.FlappyFont, "NIVEAU", new Vector2(Convert.ToInt32(gameWidth / 2.2), gameHeight / 18), Color.White);
            this.Sb.DrawString(this.FlappyFont, "DATE", new Vector2(Convert.ToInt32(gameWidth / 1.63), gameHeight / 18), Color.White);

            for (int i = startDisplay; i < (startDisplay + maxDisplay); i++)
            {
                if (i < ReadHighScores.Count)
                {
                    this.Sb.DrawString(this.FlappyFont, (i + 1).ToString(), new Vector2(gameWidth / 70, gameHeight/10 + (floorDistance * (i - startDisplay))), Color.White);
                    this.Sb.DrawString(this.FlappyFont, this.ReadHighScores[i].Name, new Vector2(gameWidth / 9, gameHeight / 10 + (floorDistance * (i - startDisplay))), Color.White);
                    this.Sb.DrawString(this.FlappyFont, this.ReadHighScores[i].Score.ToString(), new Vector2(Convert.ToInt32(gameWidth / 3.5), gameHeight / 10 + (floorDistance * (i - startDisplay))), Color.White);
                    this.Sb.DrawString(this.FlappyFont, this.ReadHighScores[i].Difficulty.ToString(), new Vector2(Convert.ToInt32(gameWidth / 2.2), gameHeight / 10 + (floorDistance * (i - startDisplay))), Color.White);
                    this.Sb.DrawString(this.FlappyFont, this.ReadHighScores[i].Created, new Vector2(Convert.ToInt32(gameWidth / 1.63), gameHeight / 10 + (floorDistance * (i - startDisplay))), Color.White);
                }
            }

            this.Sb.End();
            base.Draw(gameTime);
        }

        protected override void Dispose(bool disposing)
        {
            // Remove all buttons
            this.Game.Components.Remove(this.Up);
            this.Game.Components.Remove(this.Down);
            this.Game.Components.Remove(this.BackButton);
            base.Dispose(disposing);
        }
    }
}
