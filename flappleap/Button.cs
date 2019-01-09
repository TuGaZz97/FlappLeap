/* 
 * Author : Marin Verstraete
 * Class  : TIS-E2B
 * Date   : 18.12.2017
 * Projet : FlappLeap
 */

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Diagnostics;

namespace FlappLeap
{
    public class Button : DrawableGameComponent
    {
        private const int DEFAULT_HOVER_TRIGGER_MS = 2000;
        private const string DEFAULT_FONT = "Font";

        private SpriteBatch Sb { get; set; }
        private SpriteFont Font { get; set; }
        private Texture2D Texture { get; set; }
        private MouseState OldState { get; set; }
        private int HoveredMs { get; set; }
        private string FontName { get; set; }

        public event EventHandler<MouseState> Click;

        public Rectangle Bounds { get; set; }
        public string Text { get; set; }
        public int HoverMsTrigger { get; set; }

        public bool IsHovered { get; private set; }
        public bool IsPressed { get; private set; }

        public Button(Game game, string text, int x, int y, int width, int height, string fontName = DEFAULT_FONT) : base(game)
        {
            this.Bounds = new Rectangle(x, y, width, height);
            this.Text = text;
            this.HoverMsTrigger = DEFAULT_HOVER_TRIGGER_MS;
            this.FontName = fontName;

            // Buttons always on top
            this.DrawOrder = int.MaxValue;

#if DEBUG
            this.Click += (o, e) => Debug.WriteLine("Click on button {0}@{1} at {2},{3}", this.ToString(), this.GetHashCode(), e.X, e.Y);
#endif
        }

        protected override void LoadContent()
        {
            this.Sb = new SpriteBatch(this.Game.GraphicsDevice);
            this.Font = this.Game.Content.Load<SpriteFont>(this.FontName);

            this.Texture = new Texture2D(this.Game.GraphicsDevice, 1, 1);
            this.Texture.SetData(new[] { Color.White });

            base.LoadContent();
        }

        public override void Update(GameTime gameTime)
        {
            MouseState newState = Mouse.GetState();

            // Mouse is inside the button
            this.IsHovered =
                newState.X >= this.Bounds.Left &&
                newState.X <= this.Bounds.Right &&
                newState.Y >= this.Bounds.Top &&
                newState.Y <= this.Bounds.Bottom;

            // Mouse inside the button and left click down
            this.IsPressed = this.IsHovered && newState.LeftButton == ButtonState.Pressed;

            // Store how much time the mouse is hovering the button
            this.HoveredMs = this.IsHovered ? this.HoveredMs + gameTime.ElapsedGameTime.Milliseconds : 0;

            // Hover trigger has been reached or mouse click with debouncing
            if (this.HoveredMs >= this.HoverMsTrigger || (this.IsPressed && this.OldState.LeftButton == ButtonState.Released))
            {
                // Fire click event
                this.Click?.Invoke(this, newState);

                // Reset hover counter
                this.HoveredMs = 0;
            }

            // Store state
            this.OldState = newState;

            base.Update(gameTime);
        }

        public override void Draw(GameTime gameTime)
        {
            // Compute text size and pos
            Vector2 textSize = this.Font.MeasureString(this.Text);
            float textX = this.Bounds.X + (this.Bounds.Width / 2) - (textSize.X / 2);
            float textY = this.Bounds.Y + (this.Bounds.Height / 2) - (textSize.Y / 2);

            Rectangle hoverStateRect = this.Bounds;
            hoverStateRect.Y += 10;
            hoverStateRect.Height -= 20;
            hoverStateRect.Width = (int)((this.HoveredMs / (float)this.HoverMsTrigger) * this.Bounds.Width);

            this.Sb.Begin();

            // Button background
            if (this.Enabled)
            {
                this.Sb.Draw(this.Texture, this.Bounds, this.IsPressed ? Color.DarkGray : this.IsHovered ? Color.LightGray : Color.White);
            }
            else
            {
                // Disabled style
                this.Sb.Draw(this.Texture, this.Bounds, Color.DarkGray);
            }

            // Button hover state
            this.Sb.Draw(this.Texture, hoverStateRect, Color.LightGreen);

            // Button text
            this.Sb.DrawString(this.Font, this.Text, new Vector2(textX, textY), this.IsPressed ? Color.White : Color.Black);

            this.Sb.End();

            base.Draw(gameTime);
        }
    }
}
