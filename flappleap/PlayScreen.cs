/* 
 * Author : Marin Verstraete
 * Class  : TIS-E2B
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
    public class PlayScreen : GameScreen
    {
        private Button BackButton { get; set; }
        private Button HighScoreButton{ get; set; }
        public bool MultiplayerOn { get; set; }
        private SpriteFont gameFontBig;
        private SpriteFont gameFontSmall;

        // All the sprites
        private List<Background> Background;
        private List<Obstacle[]> Obstacles;
        private Floor Floor;
        private Player PlayerOne;
        private Player PlayerTwo;

        // Textures are designed for 720p, screen is upsclaed with the ScreenRation property
        private const int PlayScreen_WIDTH = 1280;
        private const int PlayScreen_HEIGHT = 720;
        public float ScreenRatio
        {
            get
            {
                return (float)Constants.GAME_HEIGHT / (float)PlayScreen_HEIGHT;
            }
        }

        // Time values used to generate new obstacles
        private double TotalPlayTime;
        private double NextGenerated;
        private string spriteFontButton;
        private double WaitingFor = 0;

        // the player's score
        private int playerOneScore;
        private int playerTwoScore;

        private GameStates GameState;
        private enum GameStates
        {
            Waiting,
            Playing,
            Dead
        };

        public PlayScreen(FlappLeapGame game, bool multiplayerOn) : base(game)
        {
            GameState = GameStates.Waiting;
            playerOneScore = 0;
            playerTwoScore = 0;
            this.MultiplayerOn = multiplayerOn;
        }

        public override void Initialize()
        {
            TotalPlayTime = 0;
            NextGenerated = 0;

            spriteFontButton = "FontSmall";

            gameFontBig = Game.Content.Load<SpriteFont>("FontBig");
            gameFontSmall = Game.Content.Load<SpriteFont>("FontSmall");

            // Moving background
            Background = new List<Background>
            {
                new Background(Game.Content.Load<Texture2D>(@"Images\Backgrounds\layer_05"), new Vector2(50, 50)),
                new Background(Game.Content.Load<Texture2D>(@"Images\Backgrounds\layer_04"), new Vector2(75, 75)),
                new Background(Game.Content.Load<Texture2D>(@"Images\Backgrounds\layer_03"), new Vector2(100, 100)),
            };

            Obstacles = new List<Obstacle[]>(); // array of 2 obstacles, Top and Bottom
            Floor = new Floor(Game.Content.Load<Texture2D>(@"Images\Obstacles\floor"), ScreenRatio);
            PlayerOne = new Player(Game.Content.Load<Texture2D>(@"Images\doge"), ScreenRatio);
            if(this.MultiplayerOn) PlayerTwo = new Player(Game.Content.Load<Texture2D>(@"Images\LilSleepy"), ScreenRatio, true);

            // Back button
            this.BackButton = new Button(this.Game, "Back", 20, Constants.GAME_HEIGHT - 100, 150, 50, spriteFontButton);
            this.BackButton.Click += BackButton_Click;

            // Back button
            string highscoreText = "Save your score";
            this.HighScoreButton = new Button(this.Game, highscoreText, (Constants.GAME_WIDTH / 2) - 150, 50, 300, 50, spriteFontButton);
            this.HighScoreButton.Click += HighScoreButton_Click;

            this.Game.Components.Add(this.BackButton);

            base.Initialize();
        }

        private void HighScoreButton_Click(object sender, MouseState e)
        {
            OnClose();
            this.FlappLeapGame.ChangeScreen(typeof(AddHighScoreScreen), playerOneScore);
        }

        private void BackButton_Click(object sender, MouseState e)
        {
            OnClose();
            this.FlappLeapGame.ChangeScreen(typeof(TitleScreen));
        }

        public override void Update(GameTime gameTime)
          {
            // Keyboard input to jump
            KeyboardState state = Keyboard.GetState();
            TotalPlayTime += gameTime.ElapsedGameTime.TotalMilliseconds;

            if (this.FlappLeapGame.JumpRequested)
            {
                this.FlappLeapGame.JumpRequested = false;
                PlayerOne.Jump();
            }



            if(this.MultiplayerOn)
            {
                if(PlayerOne.Dead && PlayerTwo.Dead)
                {
                    bool found = false;
                    foreach (var xD in Game.Components)
                        found = (xD == this.HighScoreButton);

                    if (!found)
                        // Adds the hightscore button
                        this.Game.Components.Add(this.HighScoreButton);

                    GameState = GameStates.Dead;
                    Obstacles.Clear();

                    Sprite.WithCollision.Add(Floor);

                    if (state.IsKeyDown(Keys.Space) || this.FlappLeapGame.JumpRequested)
                    {
                        this.FlappLeapGame.JumpRequested = false;
                        PlayerOne.Reset();
                        PlayerTwo.Reset(true);
                        playerOneScore = 0;
                        playerTwoScore = 0;
                        GameState = GameStates.Waiting;
                        WaitingFor = TotalPlayTime;

                        this.Game.Components.Remove(this.HighScoreButton);

                        // TODO Opens the add score menu when the game is over
                        // this.FlappLeapGame.ChangeScreen(typeof(AddHighScoreScreen), playerScore);
                    }
                }
            }
            else
            {
                if(PlayerOne.Dead)
                {
                    bool found = false;
                    foreach (var xD in Game.Components)
                        found = (xD == this.HighScoreButton);

                    if (!found)
                        // Adds the hightscore button
                        this.Game.Components.Add(this.HighScoreButton);

                    GameState = GameStates.Dead;
                    Obstacles.Clear();

                    Sprite.WithCollision.Add(Floor);

                    if (state.IsKeyDown(Keys.Space) || this.FlappLeapGame.JumpRequested)
                    {
                        this.FlappLeapGame.JumpRequested = false;
                        PlayerOne.Reset();
                        playerOneScore = 0;
                        GameState = GameStates.Waiting;
                        WaitingFor = TotalPlayTime;

                        this.Game.Components.Remove(this.HighScoreButton);

                        // TODO Opens the add score menu when the game is over
                        // this.FlappLeapGame.ChangeScreen(typeof(AddHighScoreScreen), playerScore);
                    }
                }
            }
            

            // Scrolling Backgrounds
            foreach (Background bg in Background)
                bg.Update(gameTime);

            // updates the obstacles
            foreach (Obstacle[] obs in Obstacles)
            {
                obs[0].Update(gameTime, GraphicsDevice.Viewport);
                obs[1].Update(gameTime, GraphicsDevice.Viewport);
            }


            if (TotalPlayTime >= NextGenerated && GameState == GameStates.Playing)
            {
                int MaxGap = (Constants.GAME_HEIGHT - Floor.Bounds.Height) - (int)(Obstacle.GapSize * ScreenRatio);
                int GapPosition = new Random().Next(0, MaxGap);
                Obstacle Top = new Obstacle(Game.Content.Load<Texture2D>(@"Images/Obstacles/wall"), Vector2.Zero, ScreenRatio);
                Obstacle Bottom = new Obstacle(Game.Content.Load<Texture2D>(@"Images/Obstacles/wall"), Vector2.Zero, ScreenRatio);

                Top.Position.X = Bottom.Position.X = Constants.GAME_WIDTH + Top.Texture.Width;
                Top.Position.Y = GapPosition - Top.Bounds.Height;
                Bottom.Position.Y = GapPosition + (int)(Obstacle.GapSize * ScreenRatio);

                Obstacles.Add(new Obstacle[]
                {
                    Top,
                    Bottom
                });

                NextGenerated = TotalPlayTime + new Random().Next(1000, 1000 + Obstacle.RespawnRange);
            }

            foreach (Obstacle[] obs in Obstacles)
            {
                // Just check for the top since its aligned with the bottom anyway
                if (obs[0].OutOfScreen && !PlayerOne.Dead)
                {
                    // The obstacles got out of the screen thus the player survived it
                    playerOneScore++;
                }
            }

            // Removes all the obstacles that are out of the screen
            Obstacles.RemoveAll(obs => obs[0].OutOfScreen);

            // updates the floor
            Floor.Update(gameTime, ScreenRatio);


            // If the game is waiting, the player isn't updated
            if (GameState == GameStates.Waiting)
            {
                if ((state.IsKeyDown(Keys.Space) || this.FlappLeapGame.JumpRequested) && TotalPlayTime > WaitingFor + 1000)
                {
                    this.FlappLeapGame.JumpRequested = false;
                    // The player started playing
                    GameState = GameStates.Playing;
                }
            }
            else
            {
                if(this.MultiplayerOn)
                {
                    PlayerTwo.Update(gameTime, ScreenRatio);
                }
                PlayerOne.Update(gameTime, ScreenRatio);
            }

            base.Update(gameTime);
        }

        public override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            Sb.Begin(SpriteSortMode.Deferred, null, SamplerState.LinearWrap, null, null);

            // Draws the backgrounds
            foreach (Background bg in Background)
                bg.Draw(Sb, FlappLeapGame.Graphics);

            // Draws the obstacles
            foreach (Obstacle[] obs in Obstacles)
            {
                obs[0].Draw(Sb, FlappLeapGame.Graphics);
                obs[1].Draw(Sb, FlappLeapGame.Graphics);
            }

            // Draws the floor and the player
            Floor.Draw(Sb, FlappLeapGame.Graphics);
            PlayerOne.Draw(Sb, FlappLeapGame.Graphics);
            PlayerTwo.Draw(Sb, FlappLeapGame.Graphics);

            if (GameState == GameStates.Waiting)
            {
                this.FlappLeapGame.detectClap = true;

                // Shows a tutorial
                string tutorialJump = "Press space to start !";
                Vector2 FontOrigin = (Constants.Screen / 2) - gameFontBig.MeasureString(tutorialJump) / 2;
                Sb.DrawString(gameFontBig, tutorialJump, FontOrigin + new Vector2(2, 2), Color.White);
                Sb.DrawString(gameFontBig, tutorialJump, FontOrigin, Color.Red);
            }

            // Generate the score string
            string scoreString = string.Format("Score : {0}", playerOneScore.ToString());
            Vector2 ScoreOrigin = (Constants.Screen / 2) - gameFontBig.MeasureString(scoreString) / 2;

            if (GameState == GameStates.Dead)
            {
                this.FlappLeapGame.detectClap = false;
                // Black screen background
                Texture2D pixel = new Texture2D(GraphicsDevice, 1, 1);
                Color[] colorData = {
                        Color.White,
                    };
                pixel.SetData(colorData);
                Sb.Draw(pixel, new Rectangle(0, 0, Constants.GAME_WIDTH, Constants.GAME_HEIGHT), Color.Black * 0.5f);

                // You died message
                string youDied = "You Died! Press space to try again.";
                Vector2 youDiedOrigin = (Constants.Screen / 2) - gameFontBig.MeasureString(youDied) / 2;
                Sb.DrawString(gameFontBig, youDied, youDiedOrigin + new Vector2(2, 2), Color.White);
                Sb.DrawString(gameFontBig, youDied, youDiedOrigin, Color.Red);

                // The score
                Sb.DrawString(gameFontBig, scoreString, ScoreOrigin + new Vector2(0, 100 * ScreenRatio) + new Vector2(2, 2), Color.White);
                Sb.DrawString(gameFontBig, scoreString, ScoreOrigin + new Vector2(0, 100 * ScreenRatio), Color.Red);
            }
            else
            {
                // The score
                Sb.DrawString(gameFontBig, scoreString, new Vector2(0, 300 * ScreenRatio) + ScoreOrigin + new Vector2(2, 2), Color.White);
                Sb.DrawString(gameFontBig, scoreString, new Vector2(0, 300 * ScreenRatio) + ScoreOrigin, Color.Red);
            }

            Sb.End();
            base.Draw(gameTime);
        }

        protected override void Dispose(bool disposing)
        {
            OnClose();
            this.Game.Components.Remove(this.BackButton);

            bool found = false;
            foreach (var lmao in Game.Components)
                found = (lmao == this.HighScoreButton);

            if (found)
                this.Game.Components.Remove(this.HighScoreButton);

            base.Dispose(disposing);
        }

        private void OnClose()
        {
            Background.Clear();
            Obstacles.Clear();
            PlayerOne.Reset();
            Sprite.WithCollision.Clear();
        }
    }
}
