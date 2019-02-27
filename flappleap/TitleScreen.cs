/* 
 * Author : Marin Verstraete
 * Class  : TIS-E2B
 * Date   : 15.01.2018
 * Projet : FlappLeap
 */

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;

namespace FlappLeap
{
    public class TitleScreen : GameScreen
    {
        public const int CURSOR_SPEED = 3;

        private string spriteFontButton;

        private Button HighscoresButton { get; set; }
        private Button OnePlayerButton { get; set; }
        private Song FlappSong;

        private Button MultiPlayerButton { get; set; }

        private Button RulesButton { get; set; }
        public static int MouseX { get; set; }
        public static int MouseY { get; set; }

        private Texture2D Logo { get; set; }
#if DEBUG
        private Button CheatAdd { get; set; }
#endif
        public TitleScreen(FlappLeapGame game, bool multiplayerOn = false) : base(game)
        {
        }

        public override void Initialize()
        {
            int gameWidth = this.FlappLeapGame.Graphics.PreferredBackBufferWidth;
            int gameHeight = this.FlappLeapGame.Graphics.PreferredBackBufferHeight;
            int buttonsWidth = 250, buttonsSpacing = 150;
            spriteFontButton = "FontSmall";

            // Highscore button disabled for now, play button moved to center
            this.HighscoresButton = new Button(this.Game, "Highscores", (gameWidth / 3) - (buttonsWidth / 2) - buttonsSpacing, (gameHeight / 2) + 200, buttonsWidth, 50, spriteFontButton);
            this.OnePlayerButton = new Button(this.Game, "One Player", (gameWidth / 3) - (buttonsWidth / 2) + buttonsSpacing, (gameHeight / 2) + 200, buttonsWidth, 50, spriteFontButton);
            this.MultiPlayerButton = new Button(this.Game, "Multiplayer", (gameWidth / 3) - (buttonsWidth / 2) + buttonsSpacing * 3, (gameHeight / 2) + 200, buttonsWidth , 50, spriteFontButton);
            this.RulesButton = new Button(this.Game, "Rules", (gameWidth / 3) - (buttonsWidth / 2) + buttonsSpacing * 5, (gameHeight / 2) + 200, buttonsWidth, 50, spriteFontButton);
      

            MouseX = GraphicsDevice.Viewport.Width / 2;
            MouseY = GraphicsDevice.Viewport.Height / 2;



            this.HighscoresButton.Click += (s, e) => this.FlappLeapGame.ChangeScreen(typeof(HighScoreScreen));
            this.OnePlayerButton.Click += (s, e) => this.FlappLeapGame.ChangeScreen(typeof(PlayScreen));
            this.MultiPlayerButton.Click += (s, e) => this.FlappLeapGame.ChangeScreen(typeof(PlayScreen), true);
            this.RulesButton.Click += (s, e) => this.FlappLeapGame.ChangeScreen(typeof(RulesScreen));

            this.Game.Components.Add(this.HighscoresButton);
            this.Game.Components.Add(this.OnePlayerButton);
            this.Game.Components.Add(this.MultiPlayerButton);
            this.Game.Components.Add(this.RulesButton);

            base.Initialize();
        }

        protected override void LoadContent()
        {
            this.Logo = this.Game.Content.Load<Texture2D>(@"Images\flappy_logo");
            FlappSong = this.Game.Content.Load<Song>("Musiques/Shotgun");
            MediaPlayer.Play(FlappSong);
            base.LoadContent();
        }

        public override void Draw(GameTime gameTime)
        {

            KeyboardState state = Keyboard.GetState();

            float scale = 0.25f;
            Vector2 logoCenter = new Vector2(this.Logo.Width / 2 * scale, (this.Logo.Height / 2 * scale) + 100);

            if(state.IsKeyDown(Keys.D))
            {
                MouseX += CURSOR_SPEED;
                Mouse.SetPosition(MouseX, MouseY);
                
            }

            if (state.IsKeyDown(Keys.S))
            {
                MouseY += CURSOR_SPEED;
                Mouse.SetPosition(MouseX, MouseY);
            }

            if (state.IsKeyDown(Keys.A))
            {
                MouseX -= CURSOR_SPEED;
                Mouse.SetPosition(MouseX, MouseY);
            }

            if (state.IsKeyDown(Keys.W))
            {
                MouseY -= CURSOR_SPEED;
                Mouse.SetPosition(MouseX, MouseY);
            }

            this.Sb.Begin();
            this.Sb.Draw(this.Logo, this.CenterScreen - logoCenter, null, Color.White, 0f, Vector2.Zero, scale, SpriteEffects.None, 0f);
            this.Sb.End();

            base.Draw(gameTime);
        }

        protected override void Dispose(bool disposing)
        {
            this.Game.Components.Remove(this.HighscoresButton);
            this.Game.Components.Remove(this.OnePlayerButton);
            this.Game.Components.Remove(this.MultiPlayerButton);
            this.Game.Components.Remove(this.RulesButton);
#if DEBUG
            this.Game.Components.Remove(this.CheatAdd);
#endif
            base.Dispose(disposing);
        }
    }
}
