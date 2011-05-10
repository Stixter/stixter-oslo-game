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
        private List<Floor> _floorItems;
        private List<Floor> _platformItems;
        private KeyboardState _oldKeyboardState;
        private readonly Random random = new Random();
        private int _playerDirection = 0;
        private float _hitFloat = 0f;

        public ActionScreen(ContentManager contentManager, Game game, SpriteBatch spriteBatch, Texture2D image)
            : base(contentManager, game, spriteBatch)
        {
            _image = image;
            _imageRectangle = new Rectangle(0, 0, Game.Window.ClientBounds.Width, Game.Window.ClientBounds.Height);
            var graphicsService = Game.Services.GetService(typeof(IGraphicsDeviceService)) as IGraphicsDeviceService;
            _player = new Player(contentManager, graphicsService.GraphicsDevice, random);

            _floorItems = new List<Floor>();

            for (var i = 0; i < 30; i++)
            {
                var floor = new Floor(contentManager);
                floor.Sprite.Position.Y = 700;
                _floorItems.Add(floor);
            }

            _platformItems = new List<Floor>();

            for (var i = 0; i < 10; i++)
            {
                var floor = new Floor(contentManager);
                floor.Sprite.Position.Y = 550;
                _platformItems.Add(floor);
            }
        }

        public override void Update(GameTime gameTime)
        {
            _keyboardState = Keyboard.GetState();

            var keys = _keyboardState.GetPressedKeys();
            
            foreach (Keys keyse in keys)
            {
                if(keyse == Keys.Space)
                {
                    _player.Jumping = true;
                }
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
            else if (_keyboardState.IsKeyDown(Keys.Up))
            {
                _player.MoveEnemy(Player.PlayerDirection.Up);
            }
            else if (_keyboardState.IsKeyDown(Keys.Down))
            {
                _player.MoveEnemy(Player.PlayerDirection.Down);
            }
            else
            {
                _player.MoveEnemy(Player.PlayerDirection.None);
                _playerDirection = 2;   
            }
            _oldKeyboardState = _keyboardState;

            if(CheckHit(_floorItems))
            {
                _player.HitFloor(_hitFloat);
     
            }
            else
            {

            }

            //if (CheckHit(_platformItems))
            //{
            //    _player.HitFloor(_hitFloat);
            //}
            //else
            //{
        
            //}
            base.Update(gameTime);
            
        }

        public bool CheckHit(IList<Floor> floors)
        {
            var playerAimRect = _player.GetPlayerHit();

            foreach (var floor in floors)
            {
                var enemyRec = floor.GetFloorRec();
                if (playerAimRect.Intersects(enemyRec))
                {
                    _hitFloat = floor.Sprite.Position.Y - 85;
                    return true;
                }
            }

            return false;
        }

        public override void Draw(GameTime gameTime)
        {
            SpriteBatch.Draw(_image, _imageRectangle, Color.White);

            var floorPosition = 0;
            foreach (var floorItem in _floorItems)
            {
                floorItem.Sprite.Position.X = floorPosition;
                floorItem.Draw(SpriteBatch);
                floorPosition += 50;
            }

            var platformPosition = 400;
            foreach (var item in _platformItems)
            {
                item.Sprite.Position.X = platformPosition;
                item.Draw(SpriteBatch);
                platformPosition += 50;
            }

            _player.UpdatePlayer(gameTime, _playerDirection);
            _player.Draw(SpriteBatch);
            base.Draw(gameTime);
        }

    }
}
