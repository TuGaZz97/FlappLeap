/* 
 * Author : Marin Verstraete
 * Class  : TIS-E2B
 * Date   : 18.12.2017
 * Projet : FlappLeap
 */

using System;
using System.Diagnostics;
using Leap;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace FlappLeap
{
    public static class Constants
    {
        public const int GAME_WIDTH = 1920;
        public const int GAME_HEIGHT = 1080;

        public static Vector2 Screen
        {
            get
            {
                return new Vector2(GAME_WIDTH, GAME_HEIGHT);
            }
        }
    }

    /// <summary>
    /// This is the main type for your game.
    /// </summary>
    public class FlappLeapGame : Game, ILeapEventDelegate
    {
       
        private GameScreen Screen { get; set; }
        public GraphicsDeviceManager Graphics { get; set; }

        // LEAP CODE
        private Controller controller { get; set; }

        private LeapEventListener listener { get; set; }

        public LeapClass lClass { get; set; }

        public bool JumpRequested = false;

        public bool detectClap = false;
        // END LEAP CODE
      
        public FlappLeapGame()
        {
            this.Graphics = new GraphicsDeviceManager(this)
            {
                PreferredBackBufferWidth = Constants.GAME_WIDTH,
                PreferredBackBufferHeight = Constants.GAME_HEIGHT
            };

            // LEAP CODE
            this.controller = new Controller();
            this.listener = new LeapEventListener(this);
            this.lClass = new LeapClass();
            controller.AddListener(listener);
            // END LEAP CODE

            this.Content.RootDirectory = "Content";
            this.IsMouseVisible = true;
        }

        public void ChangeScreen(Type type, int score = -1)
        {
            if (!type.IsSubclassOf(typeof(GameScreen)))
                return;

            if (this.Screen != null)
            {
                this.Components.Remove(this.Screen);
                this.Screen.Dispose();
            }

            if (score > -1)
                this.Screen = Activator.CreateInstance(type, this, score) as GameScreen;
            else
                this.Screen = Activator.CreateInstance(type, this, false) as GameScreen;


            this.Components.Add(this.Screen);
        }

        public void ChangeScreen(Type type, bool multiplayerOn, int score = -1)
        {
            if (!type.IsSubclassOf(typeof(GameScreen)))
                return;

            if (this.Screen != null)
            {
                this.Components.Remove(this.Screen);
                this.Screen.Dispose();
            }

            if (score > -1)
                this.Screen = Activator.CreateInstance(type, this, score) as GameScreen;
            else
                this.Screen = Activator.CreateInstance(type, this, multiplayerOn) as GameScreen;


            this.Components.Add(this.Screen);
        }

        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize()
        {
            this.ChangeScreen(typeof(TitleScreen));

            base.Initialize();
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            base.LoadContent();
        }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// game-specific content.
        /// </summary>
        protected override void UnloadContent()
        {
            base.UnloadContent();
        }

        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                this.Exit();

            base.Update(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            this.GraphicsDevice.Clear(Color.CornflowerBlue);

            base.Draw(gameTime);
        }

        // LEAP CODE
        public void LeapEventNotification(string EventName)
        {
            switch (EventName)
            {
                case "onInit":
                    Debug.WriteLine("Init");
                    break;
                case "onConnect":
                    this.connectHandler();
                    break;
                case "onFrame":
                    if(detectClap == false)
                    {
                        Point mousePos = lClass.mousePosition(this.controller.Frame());
                        try
                        {
                            Mouse.SetPosition(mousePos.X, mousePos.Y);
                        }
                        catch
                        {
                        }
                    }
                    else
                    {
                        if (lClass.detectClap(this.controller.Frame()))
                        {
                            JumpRequested = true;
                        }
                    }
                    break;
            }
        }

        void connectHandler()
        {
            this.controller.Config.SetFloat("Gesture.Circle.MinRadius", 40.0f);
            this.controller.EnableGesture(Gesture.GestureType.TYPE_CIRCLE);
            this.controller.EnableGesture(Gesture.GestureType.TYPE_SWIPE);
            this.controller.EnableGesture(Gesture.GestureType.TYPE_KEY_TAP);
            this.controller.EnableGesture(Gesture.GestureType.TYPE_SCREEN_TAP);
        }
        // END LEAP CODE
    }
}
