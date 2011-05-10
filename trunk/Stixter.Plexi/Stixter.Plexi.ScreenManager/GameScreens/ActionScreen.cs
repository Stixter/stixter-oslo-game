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
        private readonly Player _enemy;
    
        private List<Floor> _floorItems;
        private List<Floor> _platformItems;
        private List<Floor> _platFormItems2;
        private List<Floor> _platFormItems3;
        private List<Floor> _platFormItems4;
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
            _player = new Player(contentManager, graphicsService.GraphicsDevice, "Sprites\\player_move");
            _enemy = new Player(contentManager, graphicsService.GraphicsDevice, "Sprites\\enemy_move");

            BuildPlatforms(contentManager);
        }

        private void BuildPlatforms(ContentManager contentManager)
        {
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
            
            _platFormItems2 = new List<Floor>();
            for (var i = 0; i < 6; i++)
            {
                var floor = new Floor(contentManager);
                floor.Sprite.Position.Y = 400;
                _platFormItems2.Add(floor);
            }

            _platFormItems3 = new List<Floor>();
            for (var i = 0; i < 6; i++)
            {
                var floor = new Floor(contentManager);
                floor.Sprite.Position.Y = 340;
                _platFormItems3.Add(floor);
            }

            _platFormItems4 = new List<Floor>();
            for (var i = 0; i < 6; i++)
            {
                var floor = new Floor(contentManager);
                floor.Sprite.Position.Y = 420;
                _platFormItems4.Add(floor);
            }
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
                _enemy.MoveEnemy(Player.PlayerDirection.Right);
                _playerDirection = 1;
            }
            else if (_keyboardState.IsKeyDown(Keys.Left))
            {
                _player.MoveEnemy(Player.PlayerDirection.Left);
                _enemy.MoveEnemy(Player.PlayerDirection.Left);
                _playerDirection = 0;
            }
            //else if (_keyboardState.IsKeyDown(Keys.Space))
            //{
            //    _player.MoveEnemy(Player.PlayerDirection.Up);
            //}
            //else if (_keyboardState.IsKeyDown(Keys.Down))
            //{
            //    _player.MoveEnemy(Player.PlayerDirection.Down);
            //}
            else
            {
                _player.MoveEnemy(Player.PlayerDirection.None);
                _enemy.MoveEnemy(Player.PlayerDirection.None);
                _playerDirection = 2;   
            }

            _oldKeyboardState = _keyboardState;

            CheckPlatformHit();

            base.Update(gameTime);
            
        }

        private void CheckPlatformHit()
        {
            if (CheckHit(_floorItems, _player))
            {
                _player.HitFloor(_hitFloat);
            }
            else if (CheckHit(_platformItems, _player))
            {
                _player.HitFloor(_hitFloat);
            }
            else if (CheckHit(_platFormItems2, _player))
            {
                _player.HitFloor(_hitFloat);
            }
            else if (CheckHit(_platFormItems3, _player))
            {
                _player.HitFloor(_hitFloat);
            }
            else if (CheckHit(_platFormItems4, _player))
            {
                _player.HitFloor(_hitFloat);
            }
            else
            {
                _player.AllowJump = false;
            }
        }

        public bool CheckHit(IList<Floor> floors, Player player)
        {
            var playerAimRect = player.PlayerFootHit();

            foreach (var floor in floors)
            {
                var enemyRec = floor.GetFloorRec();
                if (playerAimRect.Intersects(enemyRec))
                {
                    _hitFloat = floor.Sprite.Position.Y - 85;
                    player.AllowJump = true;
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

            var platformPosition2 = 0;
            foreach (var item in _platFormItems2)
            {
                item.Sprite.Position.X = platformPosition2;
                item.Draw(SpriteBatch);
                platformPosition2 += 50;
            }

            var platformPosition3 = 500;
            foreach (var item in _platFormItems3)
            {
                item.Sprite.Position.X = platformPosition3;
                item.Draw(SpriteBatch);
                platformPosition3 += 50;
            }

            var platformPosition4 = 1000;
            foreach (var item in _platFormItems4)
            {
                item.Sprite.Position.X = platformPosition4;
                item.Draw(SpriteBatch);
                platformPosition4 += 50;
            }

            _player.UpdatePlayer(gameTime, _playerDirection);
            _player.Draw(SpriteBatch);

            //_enemy.UpdatePlayer(gameTime, _playerDirection);
            //_enemy.Draw(SpriteBatch);
            base.Draw(gameTime);
        }

    }
}
