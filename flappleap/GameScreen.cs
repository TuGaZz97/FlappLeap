/* 
 * Author : Marin Verstraete
 * Class  : TIS-E2B
 * Date   : 15.01.2018
 * Projet : FlappLeap
 */

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Media;
using System;

namespace FlappLeap
{
    public abstract class GameScreen : FlappLeapDrawableGameComponent
    {
        protected SpriteBatch Sb { get; set; }
        protected SpriteFont Font { get; set; }

        //Music Game
        private Song FlappSong;

        protected Vector2 CenterScreen
        {
            get => new Vector2(
                this.FlappLeapGame.Graphics.PreferredBackBufferWidth / 2, 
                this.FlappLeapGame.Graphics.PreferredBackBufferHeight / 2);
        }

        public GameScreen(FlappLeapGame game) : base(game)
        {
        }

        protected override void LoadContent()
        {
            this.Sb = new SpriteBatch(this.Game.GraphicsDevice);
            this.Font = this.Game.Content.Load<SpriteFont>("FontBig");
            FlappSong = this.Game.Content.Load<Song>("Musiques/FlappLeapMusic");
            MediaPlayer.Play(FlappSong);

            base.LoadContent();
        }

        public override void Draw(GameTime gameTime)
        {
            // Frames per second
            double frameRate = Math.Round(1d / gameTime.ElapsedGameTime.TotalSeconds);

            this.Sb.Begin();
            this.Sb.DrawString(this.Font, frameRate + " FPS", new Vector2(5, 5), Color.White);
            this.Sb.End();

            base.Draw(gameTime);
        }
    }
}
