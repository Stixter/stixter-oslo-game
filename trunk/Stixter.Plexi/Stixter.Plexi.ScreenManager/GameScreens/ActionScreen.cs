using System;
using System.Collections.Generic;
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
        private readonly Player _player;

        private List<Platform> _platforms;
        private KeyboardState _oldKeyboardState;
        private readonly Random random = new Random();
        private int _playerDirection = 0;
        private float _hitFloat = 0f;

        public ActionScreen(Game game, SpriteBatch spriteBatch, Texture2D image)
            : base(game, spriteBatch)
        {
            _image = image;
            _imageRectangle = new Rectangle(0, 0, Game.Window.ClientBounds.Width, Game.Window.ClientBounds.Height);

            _player = new Player(game, "Sprites\\player_move");

            BuildPlatforms();
        }

        private void BuildPlatforms()
        {
            var platformHandlerLevel1 = new LevelHandler(CurrentGame);
            _platforms = platformHandlerLevel1.GetPlatformLevel1();
        }

        public override void Update(GameTime gameTime)
        {
            _keyboardState = Keyboard.GetState();

            foreach (var key in _keyboardState.GetPressedKeys())
            {
                if (key == Keys.Space)
                    _player.PlayerState = Player.State.Jumping;
            }

            if (_keyboardState.IsKeyDown(Keys.Right))
            {
                _player.MoveEnemy(Player.PlayerDirection.Right);
                _playerDirection = 1;
            }
            else if (_keyboardState.IsKeyDown(Keys.Left))
            {
                _player.MoveEnemy(Player.PlayerDirection.Left);
                _playerDirection = 0;
            }
            else
            {
                _player.MoveEnemy(Player.PlayerDirection.None);
                _playerDirection = 2;   
            }

            _oldKeyboardState = _keyboardState;

            CheckPlatformHit();

            base.Update(gameTime);
            
        }

        private void CheckPlatformHit()
        {
            bool playerHitAnyPlatform = false;
            foreach (var platform in _platforms)
            {
                if(platform.CheckHit(_player) != 0)
                {
                    _player.HitFloor(platform.CheckHit(_player));
                    playerHitAnyPlatform = true;
                }
            }

            _player.AllowJump = playerHitAnyPlatform;
        }

        public override void Draw(GameTime gameTime)
        {
            SpriteBatch.Draw(_image, _imageRectangle, Color.White);

            foreach (Platform platform in _platforms)
            {
                platform.Draw(SpriteBatch);
            }

            _player.UpdatePlayer(gameTime, _playerDirection);
            _player.Draw(SpriteBatch);

            base.Draw(gameTime);
        }

    }
}
