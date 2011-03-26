using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Stixter.Plexi.ScreenManager.GameScreens;

namespace Stixter.Plexi
{
    public class PlexiGame : Game
    {
        GraphicsDeviceManager _graphics;
        SpriteBatch _spriteBatch;
        KeyboardState _oldKeyboardState;
        GameScreen _activeScreen;
        StartScreen _startScreen;
        ActionScreen _actionScreen;
        private KeyboardState _keyboardState;

        public PlexiGame()
        {
            _graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
        }

        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize()
        {
            // TODO: Add your initialization logic here

            base.Initialize();
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);
            _startScreen = new StartScreen(this, _spriteBatch, Content.Load<SpriteFont>("menufont"), Content.Load<Texture2D>("wateronglass"));
            Components.Add(_startScreen);
            _startScreen.Hide();
            _actionScreen = new ActionScreen( this, _spriteBatch, Content.Load<Texture2D>("greenmetal"));
            Components.Add(_actionScreen);
            _actionScreen.Hide();
            _activeScreen = _startScreen;
            _activeScreen.Show();
        }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// all content.
        /// </summary>
        protected override void UnloadContent()
        {
            // TODO: Unload any non ContentManager content here
        }

        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime)
        {
            _keyboardState = Keyboard.GetState();
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed)
                Exit();

            _keyboardState = Keyboard.GetState();
            if (_keyboardState.IsKeyDown(Keys.Escape))
                Exit();

            if(_keyboardState.GetPressedKeys().Contains(Keys.Back))
            {
                _activeScreen.Hide();
                _activeScreen = _startScreen;
                _activeScreen.Show();
            }

            if (_activeScreen == _startScreen)
            {
                if (CheckKey(Keys.Enter))
                {
                    if (_startScreen.SelectedIndex == 0)
                    {
                        _activeScreen.Hide();
                        _activeScreen = _actionScreen;
                        _activeScreen.Show();
                    }
                    if (_startScreen.SelectedIndex == 1)
                    {
                        _activeScreen.Hide();
                        _activeScreen = _actionScreen;
                        _activeScreen.Show();
                    }
                    if(_startScreen.SelectedIndex == 3)
                    {
                        Exit();
                    }
                }
            }
            base.Update(gameTime);
            _oldKeyboardState = _keyboardState;
        }

        private bool CheckKey(Keys theKey)
        {
            return _keyboardState.IsKeyUp(theKey) &&
            _oldKeyboardState.IsKeyDown(theKey);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.Black);
            _spriteBatch.Begin(SpriteSortMode.Texture, BlendState.AlphaBlend);
            base.Draw(gameTime);
            _spriteBatch.End();

        }
    }
}
