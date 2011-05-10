using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Stixter.Plexi.Sprites.Helpers;

namespace Stixter.Plexi.Sprites.Sprites
{
    public class Player
    {
        public enum State { Walking, Jumping }
        public enum PlayerDirection {Left, Right, Up, Down, None}

        private readonly ContentManager _contentManager;
        public AnimatedSprite Sprite;
        private readonly GraphicsDevice _graphicsDevice;
        private Rectangle _viewportRect;
        private readonly Random _random;
        public State PlayerState;
        private double _currentTimer;
        private double _startJumpTime;
        const double TimeInAir = 0.5;
        const float MaxEnemyVelocity = 8.0f;
        private bool _jumpInProgress;
        private float _currentVelocity = 1.0f;
        private float _lastPlayerY;
        public bool AllowJump;
        private PlayerDirection _lastplayerDirection;
        private PlayerDirection _currentPlayerDirection;
   
        public Player(ContentManager contentManager, GraphicsDevice graphicsDevice, Random random)
        {
            _contentManager = contentManager;
            _graphicsDevice = graphicsDevice;
            _random = random;

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
            Sprite = new AnimatedSprite(_contentManager.Load<Texture2D>("Sprites\\player_move"));
        }

        public void HitFloor(float posY)
        {
            if (!_jumpInProgress)
            {
                Sprite.Position.Y = posY;
                _lastPlayerY = Sprite.Position.Y;
            }
        }

        public void MoveEnemy(PlayerDirection direction)
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
            if (_currentPlayerDirection == PlayerDirection.Right)
                Sprite.Position.X = Sprite.Position.X + _currentVelocity;

            if (_currentPlayerDirection == PlayerDirection.Left)
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

        private void SetCurrentDirection(PlayerDirection direction)
        {
            _currentPlayerDirection = direction;
        }

        public void UpdatePlayer(GameTime gameTime, int direction)
        {
            Sprite.UpdateSprite(gameTime, direction);
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
