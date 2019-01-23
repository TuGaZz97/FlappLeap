using System;
using System.Diagnostics;
using Leap;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace FlappLeap
{
    class RulesScreen : GameScreen
    {
        private Button BackButton { get; set; }

        private SpriteFont FlappyFont { get; set; }

        private int maxDisplay;
        private int startDisplay = 0;

        private int gameWidth;
        private int gameHeight;
        private int sizeButtonTouch;

        private string spriteFontButton;

        public RulesScreen(FlappLeapGame game) : base(game)
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
            this.FlappyFont = this.Game.Content.Load<SpriteFont>(spriteFontButton);
            // Add all buttons
            this.BackButton = new Button(this.Game, "BACK", Convert.ToInt32(gameWidth / 4.5), Convert.ToInt32(gameHeight / 1.2), sizeButtonTouch * 3, sizeButtonTouch, spriteFontButton);            
            // Event for all buttons
            this.BackButton.Click += (s, e) => this.FlappLeapGame.ChangeScreen(typeof(TitleScreen));
            // Display all buttons
            this.Game.Components.Add(this.BackButton);

            base.Initialize();
        }

        public override void Draw(GameTime gameTime)
        {
            this.Sb.Begin();
            this.Sb.DrawString(this.FlappyFont, "Bienvenue dans Flapp", new Vector2(gameWidth / 70, gameHeight / 18), Color.White);
            this.Sb.DrawString(this.FlappyFont, "Voici les règles", new Vector2(gameWidth / 70, gameHeight / 9), Color.White);

            this.Sb.End();
            base.Draw(gameTime);
        }

        protected override void Dispose(bool disposing)
        {
            // Remove all buttons;
            this.Game.Components.Remove(this.BackButton);
            base.Dispose(disposing);
        }
    }
}
