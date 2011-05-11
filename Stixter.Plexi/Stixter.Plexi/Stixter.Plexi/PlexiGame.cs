using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Stixter.Plexi.ScreenManager.GameScreens;

namespace Stixter.Plexi
{
    public class PlexiGame : Game
    {
        private readonly GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;
        private KeyboardState _oldKeyboardState;
        private KeyboardState _keyboardState;
        private GameScreen _activeScreen;
        private StartScreen _startScreen;
        private ActionScreen _actionScreen;

        public PlexiGame()
        {
            _graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
        }

        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);
       
            SetScreenSize();

            _startScreen = new StartScreen(this, _spriteBatch, Content.Load<SpriteFont>("menufont"), Content.Load<Texture2D>("splash_screen"));
            Components.Add(_startScreen);
            _startScreen.Hide();
            
            _actionScreen = new ActionScreen(this, _spriteBatch, Content.Load<Texture2D>("Levels\\level1_background"));
            Components.Add(_actionScreen);
            _actionScreen.Hide();

            _activeScreen = _startScreen;
            _activeScreen.Show();
        }

        private void SetScreenSize()
        {
            _graphics.PreferredBackBufferWidth = 1280;
            _graphics.PreferredBackBufferHeight = 720;
            _graphics.ApplyChanges();
            //_graphics.ToggleFullScreen();
        }

        protected override void UnloadContent()
        {
            Content.Unload();
        }

        protected override void Update(GameTime gameTime)
        {
            _keyboardState = Keyboard.GetState();
          
            if (CheckKey(Keys.Escape))
                Exit();

            if (CheckKey(Keys.Back))
            {
                SetActiveScreen(_startScreen);
            }

            if (_activeScreen == _startScreen)
            {
                if (CheckKey(Keys.Enter))
                {
                    if (_startScreen.SelectedIndex == 0)
                    {
                        SetActiveScreen(_actionScreen);
                    }
                    if (_startScreen.SelectedIndex == 1)
                    {
                        SetActiveScreen(_actionScreen);
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

        private void SetActiveScreen(GameScreen gameScreen)
        {
            _activeScreen.Hide();
            _activeScreen = gameScreen;
            _activeScreen.Show();
        }

        private bool CheckKey(Keys theKey)
        {
            return _keyboardState.IsKeyUp(theKey) && _oldKeyboardState.IsKeyDown(theKey);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.Black);
            _spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend);
            base.Draw(gameTime);
            _spriteBatch.End();
        }
    }
}
