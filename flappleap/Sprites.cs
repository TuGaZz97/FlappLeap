using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;

namespace FlappLeap
{
    public class Background : Sprite
    {
        //private Viewport Viewport;      //Our game viewport
        private Vector2 offset;         //Offset to start drawing our image
        public Vector2 Speed;           //Speed of movement of our parallax effect

        public Background(Texture2D texture, Vector2 speed, bool hasCollision = false)
            : base(texture, 1, hasCollision)
        {
            offset = Vector2.Zero;
            Speed = speed;
        }

        public void Update(GameTime gametime)
        {
            float elapsed = (float)gametime.ElapsedGameTime.TotalSeconds;
            Vector2 direction = new Vector2(1, 0);

            //Zoom
            base.Zoom = Texture.Height / Constants.GAME_HEIGHT;

            //Calculate the distance to move our image, based on speed
            Vector2 distance = direction * Speed * elapsed;

            //Update our offset
            Position += distance;
        }

        public override void Draw(SpriteBatch spriteBatch, GraphicsDeviceManager graphics)
        {
            spriteBatch.Draw(Texture, new Vector2(0, 0), Bounds, Color.White, 0, Vector2.Zero, Zoom, SpriteEffects.None, 1);
        }
    }

    public class Obstacle : Sprite
    {
        static public int RespawnRange = 3000;
        static public int GapSize = 300;

        private Vector2 speed;
        private Vector2 direction;
        private Viewport Viewport;      //Our game viewport

        public bool OutOfScreen
        {
            get
            {
                return (Position.X + Bounds.Width) < 0;
            }
        }

        public Obstacle(Texture2D Texture, Vector2 Position, float Zoom)
            : base(Texture, Zoom, true)
        {
            speed = new Vector2(500 * Zoom, 500 * Zoom);
            direction = new Vector2(1, 0);
            this.Position = Position;
        }

        public void Update(GameTime gametime, Viewport viewport, float difficultyMultiplicator)
        {
            float elapsed = (float)gametime.ElapsedGameTime.TotalSeconds;

            Viewport = viewport;

            //Calculate the distance to move our image, based on speed
            Vector2 distance =  direction * speed * difficultyMultiplicator * elapsed;

            Position -= distance;
        }

        public override void Draw(SpriteBatch spriteBatch, GraphicsDeviceManager graphics)
        {
            spriteBatch.Draw(Texture, Bounds, Color.White);
            //spriteBatch.Draw(Texture, new Vector2(1280, 720), Bounds, Color.White, 0, Vector2.Zero, 1, SpriteEffects.None, 1);
        }
    }

    public class Floor : Sprite
    {
        private Vector2 offset;         //Offset to start drawing our image
        public Vector2 Speed;           //Speed of movement of our parallax effect

        public Floor(Texture2D texture, float Zoom)
            : base(texture, Zoom, true)
        {
            offset = Vector2.Zero;
            Speed = new Vector2(500 * Zoom, 500 * Zoom);
            Position = new Vector2(0, Constants.GAME_HEIGHT - Bounds.Height);
        }

        public void Update(GameTime gametime, float Zoom, float difficultyMultiplicator)
        {
            float elapsed = (float)gametime.ElapsedGameTime.TotalSeconds;
            Vector2 direction = new Vector2(1, 0);

            base.Zoom = Zoom;

            //Calculate the distance to move our image, based on speed
            Vector2 distance = direction * Speed * difficultyMultiplicator * elapsed;

            //Update our offset, the floor's position doesn't actually ever moves
            offset -= distance;
        }

        public override void Draw(SpriteBatch spriteBatch, GraphicsDeviceManager graphics)
        {
            spriteBatch.Draw(
                Texture, new Vector2(Position.X + (offset.X % Constants.GAME_WIDTH),
                    Position.Y), new Rectangle(0, 0, Texture.Width, Texture.Height),
                Color.White, 0, Vector2.Zero, Zoom, SpriteEffects.None, 1);
            spriteBatch.Draw(
                Texture, new Vector2(Constants.GAME_WIDTH + Position.X + (offset.X % Constants.GAME_WIDTH),
                    Position.Y), new Rectangle(0, 0, Texture.Width, Texture.Height),
                Color.White, 0, Vector2.Zero, Zoom, SpriteEffects.None, 1);
        }
    }

    public class Player : Sprite
    {
        private Vector2 speed;
        private bool dead;
        public bool IsPlayerTwo { get; set; }
        public bool Dead
        {
            get
            {
                return dead;
            }
            private set
            {
                dead = value;
            }
        }

        public Player(Texture2D Texture, float Zoom, bool isPlayerTwo = false)
            : base(Texture, Zoom, true)
        {
            this.IsPlayerTwo = isPlayerTwo;

            if(this.IsPlayerTwo)
            {
                Position = new Vector2((Constants.GAME_WIDTH / 2) - (Bounds.Width / 2), 200 * Zoom);
            }
            else
            {
                Position = new Vector2((Constants.GAME_WIDTH / 3) - (Bounds.Width / 2), 200 * Zoom);
            }
            
            Dead = false;
        }

        public void Reset()
        {
            if (this.IsPlayerTwo)
            {
                Position = new Vector2((Constants.GAME_WIDTH / 2) - (Bounds.Width / 2), 200 * Zoom);
            }
            else
            {
                Position = new Vector2((Constants.GAME_WIDTH / 3) - (Bounds.Width / 2), 200 * Zoom);
            }
            Dead = false;
            speed = Vector2.Zero;
        }

        public void Update(GameTime gametime, float Zoom)
        {
            base.Zoom = Zoom;
            float elapsed = (float)gametime.ElapsedGameTime.TotalSeconds;
            Vector2 direction = new Vector2(0, 1);

            //Calculate the distance to move our image, based on speed
            Vector2 distance = direction * new Vector2(30, 30) * elapsed;
            speed += distance;

            Position += speed;

            // Keyboard input to jump
            KeyboardState state = Keyboard.GetState();

            
            if(this.IsPlayerTwo)
            {
                if (state.IsKeyDown(Keys.Up) && !Dead)
                {
                    Jump();
                }
            }
            else
            {
                if (state.IsKeyDown(Keys.Space) && !Dead)
                {
                    Jump();
                }
            }

            

            if ((this.IsCollidingWithAny() || Position.Y < - (Bounds.Height * 2)) && !Dead)
            {
                DeathJump();
                Dead = true;
                Sprite.WithCollision.Remove(this);
                
            }
        }

        public void Jump()
        {
            speed = new Vector2(0, -8 * Zoom);
        }

        private void DeathJump()
        {
            speed = new Vector2(0, -16 * Zoom);
        }

        public override void Draw(SpriteBatch spriteBatch, GraphicsDeviceManager graphics)
        {
            spriteBatch.Draw(
                Texture, new Vector2(
                    Position.X,
                    Position.Y), new Rectangle(0, 0, Texture.Width, Texture.Height),
                Color.White, 0, Vector2.Zero, Zoom, SpriteEffects.None, 1);
        }
    }


    public abstract class Sprite
    {
        // List of all sprites
        public static List<Sprite> WithCollision;
        public Texture2D Texture;
        public float Zoom = 1;
        //public Point Position;
        public Vector2 Position;
        private Rectangle bounds = Rectangle.Empty;
        private bool hasCollision;

        public virtual Rectangle Bounds
        {
            get
            {
                return new Rectangle(
                    (int)Position.X,
                    (int)Position.Y,
                    (int)(Texture.Width * Zoom),
                    (int)(Texture.Height * Zoom));
            }
        }

        public Sprite(Texture2D me, float Zoom, bool hasCollision = true)
        {
            this.Texture = me;
            this.Zoom = Zoom;
            this.hasCollision = hasCollision;

            // Creates the list if null
            if (WithCollision == null)
                WithCollision = new List<Sprite>();

            // Adds outself to the list (if hasCollision)
            if (hasCollision)
                WithCollision.Add(this);
        }

        ~Sprite()
        {
            if (hasCollision)
                WithCollision.Remove(this);
        }

        public bool IsCollidingWithAny()
        {
            if (hasCollision)
            {
                var memes = WithCollision.Where((v, i) => v != this).ToList();
                foreach (Sprite meme in memes)
                {
                    if (meme.Bounds.Intersects(this.Bounds))
                    {
                        return true;
                    }
                }
                return false;
            }
            else
                return false;

        }

        public abstract void Draw(SpriteBatch spriteBatch, GraphicsDeviceManager graphics);
    }
}
