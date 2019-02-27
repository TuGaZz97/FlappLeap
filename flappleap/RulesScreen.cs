/*
 * Micael Rodrigues
 * T.IS-E2
 */
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;

namespace FlappLeap
{
    class RulesScreen : GameScreen
    {
        private Button BackButton { get; set; }

        private SpriteFont FlappyFont { get; set; }
        private int MouseX { get; set; }
        private int MouseY { get; set; }

        private int maxDisplay;
        private int startDisplay = 0;

        private int gameWidth;
        private int gameHeight;
        private int sizeButtonTouch;

        private string text_But = "BUT\r\n Le but de FlappLeap est d'effectuer le score le plus eleve en traverssant les differents obstacles.";
        private string text_Touches = "TOUCHES\r\n Les differentes touchent du jeu: \r\n S pour demarrer la partie  \r\n R pour recommencer apres la mort,  \r\n Space pour voler \r\n En mode multijoueur la fleche du haut pour faire sauter le deuxieme personnage.";
        private string text_Difficulte = "DIFFICULTE\r\n La difficulte du jeu est crescendo par rapport au score dans le jeu, la vitesse et les obstacles sont multiplies";

        private string spriteFontButton;

        public RulesScreen(FlappLeapGame game, bool multiplayer = false) : base(game)
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

            this.MouseX = TitleScreen.MouseX;
            this.MouseY = TitleScreen.MouseY;

            base.Initialize();
        }

        public override void Draw(GameTime gameTime)
        {
            this.Sb.Begin();
            this.Sb.DrawString(this.FlappyFont, "Bienvenue dans FlappLeap", new Vector2(gameWidth / 70, gameHeight / 18), Color.White);
            this.Sb.DrawString(this.FlappyFont, text_But + "\r\n" + "\r\n" + text_Touches + "\r\n" + "\r\n" + text_Difficulte, new Vector2(gameWidth / 70, gameHeight / 9), Color.White);
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
            // Remove all buttons;
            this.Game.Components.Remove(this.BackButton);
            base.Dispose(disposing);
        }
    }
}
