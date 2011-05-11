using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Stixter.Plexi.Sprites.Helpers;

namespace Stixter.Plexi.Sprites.Sprites
{
    public class Player : GameComponent
    {
        public enum State { Walking, Jumping }

        public AnimatedSprite Sprite;
        private readonly GraphicsDevice _graphicsDevice;
        private Rectangle _viewportRect;
        public State PlayerState;
        private double _currentTimer;
        private double _startJumpTime;
        const double TimeInAir = 0.5;
        const float MaxEnemyVelocity = 7.0f;
        private bool _jumpInProgress;
        private float _currentVelocity = 1.0f;
        private float _lastPlayerY;
        public bool AllowJump;
        private AnimatedSprite.PlayerDirection _lastplayerDirection;
        private AnimatedSprite.PlayerDirection _currentPlayerDirection;
        private string _texture;
   
        public Player(Game game, string texture) : base(game)
        {
            _texture = texture;
            
            var graphicsDeviceService = Game.Services.GetService(typeof(IGraphicsDeviceService)) as IGraphicsDeviceService;
            _graphicsDevice = graphicsDeviceService.GraphicsDevice;

            CreateViewportRec();
            CreateGameObject();

            _lastPlayerY = 312f;
            Sprite.Position.X = 400f;
        }

        public Rectangle PlayerFootHit()
        {
            return new Rectangle(
                       (int)Sprite.Position.X + 50,
                       (int)Sprite.Position.Y,
                       Sprite.Sprite.Width - 180,
                       Sprite.Sprite.Height - 200);
        }

        private void CreateViewportRec()
        {
            _viewportRect = new Rectangle(0, 0,
               _graphicsDevice.Viewport.Width,
               _graphicsDevice.Viewport.Height);
        }

        public Rectangle GetEnemyRec()
        {
            return new Rectangle((int)Sprite.Position.X + 10, (int)Sprite.Position.Y + 10,Sprite.SourceRect.Width,
                                            Sprite.SourceRect.Height); 
        }

        private void CreateGameObject()
        {
            Sprite = new AnimatedSprite(Game.Content.Load<Texture2D>(_texture));
        }

        public void HitFloor(float posY)
        {
            if (!_jumpInProgress)
            {
                Sprite.Position.Y = posY;
                _lastPlayerY = Sprite.Position.Y;
            }
        }

        public void MoveEnemy(AnimatedSprite.PlayerDirection direction)
        {
            Sprite.Position.Y = _lastPlayerY;

            SetCurrentDirection(direction);
            SetCorretSpeedOnPlayer();
            MovePlayerLeftOrRight();

            if (PlayerState.Equals(State.Jumping) && AllowJump)
                Jump();
 
            _lastplayerDirection = direction;

            if(!_jumpInProgress)
                Sprite.Position.Y = _lastPlayerY + 5.0f;
            else
                Sprite.Position.Y = _lastPlayerY - 5.0f;

            _lastPlayerY = Sprite.Position.Y;

            PlayerState = State.Walking;
            Sprite.Alive = true;
            
        }

        private void SetCorretSpeedOnPlayer()
        {
            if((_lastplayerDirection == _currentPlayerDirection))
            {
                if(_currentVelocity < MaxEnemyVelocity)
                    _currentVelocity = _currentVelocity + 0.07f;
            }
            else
                _currentVelocity = 0;
        }

        private void MovePlayerLeftOrRight()
        {
            if (_currentPlayerDirection == AnimatedSprite.PlayerDirection.Right)
                Sprite.Position.X = Sprite.Position.X + _currentVelocity;

            if (_currentPlayerDirection == AnimatedSprite.PlayerDirection.Left)
                Sprite.Position.X = Sprite.Position.X - _currentVelocity;
        }

        private void Jump()
        {
            if(!_jumpInProgress)
            {
                _jumpInProgress = true;
                _startJumpTime = _currentTimer;
            }
        }

        private void SetCurrentDirection(AnimatedSprite.PlayerDirection direction)
        {
            _currentPlayerDirection = direction;
        }

        public void UpdatePlayer(GameTime gameTime, int direction)
        {
            Sprite.UpdateSprite(gameTime, _currentPlayerDirection);
            SpritePosition.KeepSpriteOnScreen(Sprite, _graphicsDevice);
            
            _currentTimer = gameTime.TotalGameTime.TotalSeconds;

            if(_jumpInProgress)
                CheckIfStillJumping();
        }

        private void CheckIfStillJumping()
        {
            var timeDiffrent = _currentTimer - _startJumpTime;
            _jumpInProgress = timeDiffrent <= TimeInAir;
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            if (Sprite.Alive)
            {
                spriteBatch.Draw(Sprite.Sprite, Sprite.Position, Sprite.SourceRect, Color.White, Sprite.Rotation, Sprite.Center, 1.0f, SpriteEffects.None, 0);
            }
        }
    }
}
