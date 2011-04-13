using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Stixter.Plexi.Sprites.Sprites;

namespace Stixter.Plexi.ScreenManager.GameScreens
{
    public class ActionScreen : GameScreen
    {
        private KeyboardState _keyboardState;
        private readonly Texture2D _image;
        private readonly Rectangle _imageRectangle;
        private Player _player;
        private KeyboardState _oldKeyboardState;
        private readonly Random random = new Random();
        private int _playerDirection = 0;

        public ActionScreen(ContentManager contentManager, Game game, SpriteBatch spriteBatch, Texture2D image)
            : base(contentManager, game, spriteBatch)
        {
            _image = image;
            _imageRectangle = new Rectangle(0, 0, Game.Window.ClientBounds.Width, Game.Window.ClientBounds.Height);
            var graphicsService = Game.Services.GetService(typeof(IGraphicsDeviceService)) as IGraphicsDeviceService;
            _player = new Player(contentManager, graphicsService.GraphicsDevice, random);
        }

        public override void Update(GameTime gameTime)
        {
            _keyboardState = Keyboard.GetState();

            bool jumping = false;

            var keys = _keyboardState.GetPressedKeys();

            foreach (Keys keyse in keys)
            {
                if(keyse == Keys.Space)
                {
                    jumping = true;
                }
            }

            if (_keyboardState.IsKeyDown(Keys.Right))
            {
                _player.MoveEnemy(0, jumping);
                _playerDirection = 1;
            }
            else if (_keyboardState.IsKeyDown(Keys.Left))
            {
                _player.MoveEnemy(1, jumping);
                _playerDirection = 0;
            }
            else if (_keyboardState.IsKeyDown(Keys.Up))
            {
                _player.MoveEnemy(2, jumping);
            }
            else if (_keyboardState.IsKeyDown(Keys.Down))
            {
                _player.MoveEnemy(3, jumping);
            }
            else
            {
                _player.MoveEnemy(4, jumping);
                _playerDirection = 2;   
            }
            _oldKeyboardState = _keyboardState;
            base.Update(gameTime);
            
        }

        public override void Draw(GameTime gameTime)
        {
            SpriteBatch.Draw(_image, _imageRectangle, Color.White);
            _player.UpdatePlayer(gameTime, _playerDirection);
            _player.Draw(SpriteBatch);
            base.Draw(gameTime);
        }

    }
}
