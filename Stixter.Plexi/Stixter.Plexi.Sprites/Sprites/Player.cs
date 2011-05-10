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
        public bool Jumping;
        private readonly GraphicsDevice _graphicsDevice;
        private Rectangle _viewportRect;
        private readonly Random _random;
        private State _currentState;

        const float MaxEnemyHeight = 0.1f;
        const float MinEnemyHeight = 0.8f;
        const float MaxEnemyVelocity = 6.0f;
        private float _currentVelocity = 1.0f;
        private float _jumpingVelocity;
        private float _jumpingSpeed = 7.0f;
        private bool _hitMaxJump;
        private bool _hitLowJump = true;

       

        private float _lastPlayerY;

        private PlayerDirection _lastplayerDirection;
        private PlayerDirection _currentPlayerDirection;
   
        public Player(ContentManager contentManager, GraphicsDevice graphicsDevice, Random random)
        {
            this._contentManager = contentManager;
            this._graphicsDevice = graphicsDevice;
            this._random = random;

            CreateViewportRec();
            CreateGameObject();

            _lastPlayerY = 312f;
            Sprite.Position.X = 400f;
        }

        public Rectangle GetPlayerHit()
        {
            return new Rectangle(
                       (int)Sprite.Position.X,
                       (int)Sprite.Position.Y,
                       Sprite.Sprite.Width - 100,
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
            //_currentState = State.Walking;
            Sprite.Position.Y = posY;
        }

        public void MoveEnemy(PlayerDirection direction)
        {
            Sprite.Position.Y = _lastPlayerY;

            SetCurrentDirection(direction);
            SetCorretSpeedOnPlayer();
            MovePlayerLeftOrRight();
            JumpingHandler();

            _lastplayerDirection = direction;

            
            if(!_currentState.Equals(State.Jumping))
                _lastPlayerY = Sprite.Position.Y;

            Jumping = false;
            Sprite.Alive = true;
            
        }

        private void SetCorretSpeedOnPlayer()
        {
            if((_lastplayerDirection == _currentPlayerDirection))
            {
                if(_currentVelocity < MaxEnemyVelocity)
                    _currentVelocity = _currentVelocity + 0.05f;
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

        private void JumpingHandler()
        {
            if (Jumping || (_currentState == State.Jumping))
            {
                DoJump();
            }
            else
            {
                _jumpingVelocity = _jumpingVelocity - _jumpingSpeed;

                if (_jumpingVelocity < 0)
                    _jumpingVelocity = 0;


                Sprite.Position.Y = _lastPlayerY + 2.0f;
                
                SpritePosition.KeepSpriteOnScreen(Sprite, _graphicsDevice);
            }
        }

        private void DoJump()
        {
            _currentState = State.Jumping;

            if(_hitLowJump)
                _jumpingVelocity = _jumpingVelocity + _jumpingSpeed;
                    
            if (_hitMaxJump)
                _jumpingVelocity = _jumpingVelocity - _jumpingSpeed + 1.0f;

            if (_jumpingVelocity > 230.0f)
            {
                _jumpingSpeed = 4.0f;
            }
            else if (_jumpingVelocity < 230.0f)
            {
                _jumpingSpeed = 7.0f;
            }

            if (_jumpingVelocity > 250.0f)
            {
                _hitMaxJump = true;
                _hitLowJump = false;
            }
            if(_jumpingVelocity < 0)
            {
                _hitLowJump = true;
                _currentState = State.Walking;
                _hitMaxJump = false;
            }

            Sprite.Position.Y = Sprite.Position.Y - _jumpingVelocity;
            SpritePosition.KeepSpriteOnScreen(Sprite, _graphicsDevice);
        }

        private void SetCurrentDirection(PlayerDirection direction)
        {
            _currentPlayerDirection = direction;
        }

        public void UpdatePlayer(GameTime gameTime, int direction)
        {
            Sprite.UpdateSprite(gameTime, direction);
            SpritePosition.KeepSpriteOnScreen(Sprite, _graphicsDevice);
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
