using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using Stixter.Plexi.Core;
using Stixter.Plexi.ScreenManager.GameScreens;
using Stixter.Plexi.Service.HighScoreService;

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
        private GameOverScreen _gameOverScreen;
        private ActionScreen _actionScreen;
        private SoundEffect _menuClickSelected;
        private Song _gamePlaySong, _menuSong, _dead;
        private bool _gameIsOn;

        public PlexiGame()
        {
            _graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
        }

        protected override void LoadContent()
        {
            _gamePlaySong = Content.Load<Song>("Sounds\\battle");
            _menuSong = Content.Load<Song>("Sounds\\menu_song2");
            _dead = Content.Load<Song>("Sounds\\defeat");
            MediaPlayer.IsRepeating = true;
            MediaPlayer.Play(_menuSong);
            _menuClickSelected = Content.Load<SoundEffect>("Sounds\\button-25");
            _spriteBatch = new SpriteBatch(GraphicsDevice);
            _gameIsOn = false;
            SetScreenSize();

            _gameOverScreen = new GameOverScreen(this, _spriteBatch, Content.Load<SpriteFont>("menufont"), Content.Load<Texture2D>("gameover_screen"));
            Components.Add(_gameOverScreen);
            _gameOverScreen.Hide();

            _startScreen = new StartScreen(this, _spriteBatch, Content.Load<SpriteFont>("menufont"), Content.Load<Texture2D>("splash_screen"));
            Components.Add(_startScreen);
            _startScreen.Hide();
            
            _actionScreen = new ActionScreen(this, _spriteBatch, Content.Load<Texture2D>("Levels\\level1_background"));
            Components.Add(_actionScreen);
            _actionScreen.Hide();

            //_activeScreen = _gameOverScreen;
            //_gameOverScreen.SetScoreText();

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
          
            if(CheckKey(Keys.F1))
            {
                if (!_gameIsOn)
                {
                    SetActiveScreen(_startScreen);
                    _gameIsOn = false;
                    MediaPlayer.IsRepeating = true;
                    if(!_gameOverScreen.IshighScore)
                        MediaPlayer.Play(_menuSong);
                }
            }
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
                        _gameIsOn = true;
                        _menuClickSelected.Play();
                        _actionScreen.ResetGame();
                        _gameOverScreen.IshighScore = false;
                        GameTimerHandler.TotalGameTime = 0;
                        GameTimerHandler.LastGameStartTime = (int)gameTime.TotalGameTime.TotalSeconds;
                        MediaPlayer.IsRepeating = true;
                        MediaPlayer.Play(_gamePlaySong);
                        SetActiveScreen(_actionScreen);
                    }
                   
                    if (_startScreen.SelectedIndex == 1)
                    {
                        _menuClickSelected.Play();
                        _gameOverScreen.IshighScore = true;
                        _gameOverScreen.GetHighScoreFromLeaderBoard();
                        SetActiveScreen(_gameOverScreen);
                    }
                    if(_startScreen.SelectedIndex == 2)
                    {
                        Exit();
                    }
                }
            }

            if (_actionScreen.CheckIfGameIsOver())
            {
                if (_gameIsOn)
                {
                    _gameIsOn = false;
                    MediaPlayer.IsRepeating = false;
                    MediaPlayer.Play(_dead);
                    
                   
                    _gameOverScreen.GetHighScoreFromLeaderBoard();

                   

                    SetActiveScreen(_gameOverScreen);
                    _gameOverScreen.SetScoreText();
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