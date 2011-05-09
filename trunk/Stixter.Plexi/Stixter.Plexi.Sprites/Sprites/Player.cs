using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

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

        const float MaxEnemyHeight = 0.1f;
        const float MinEnemyHeight = 0.8f;
        const float MaxEnemyVelocity = 6.0f;
        private float _currentVelocity = 1.0f;
        private float _jumpingVelocity = 0.0f;
        private float _jumpingSpeed = 8.0f;
        private bool _hitMaxJump = false;
        private bool _hitLowJump = true;

        private PlayerDirection _lastplayerDirection;
        private PlayerDirection _currentPlayerDirection;
   
        public int Direction = 0;

        public Player(ContentManager contentManager, GraphicsDevice graphicsDevice, Random random)
        {
            this._contentManager = contentManager;
            this._graphicsDevice = graphicsDevice;
            this._random = random;

            CreateViewportRec();
            CreateGameObject();
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

        public void MoveEnemy(PlayerDirection direction)
        {
            Sprite.Position.Y = 612f;

            SetCurrentDirection(direction);

            if((_lastplayerDirection == _currentPlayerDirection))
            {
                if(_currentVelocity < MaxEnemyVelocity)
                    _currentVelocity = _currentVelocity + 0.05f;
            }
            else
            {
                _currentVelocity = 0;
            }

            if (_currentPlayerDirection == PlayerDirection.Right)
            {
                Sprite.Position.X = Sprite.Position.X + _currentVelocity;
            }
            if (_currentPlayerDirection == PlayerDirection.Left)
            {
                Sprite.Position.X = Sprite.Position.X - _currentVelocity;
            }

            if(Jumping)
                JumpingHandler();

           

            if (Jumping)
            {
                Sprite.Position.Y = Sprite.Position.Y - _jumpingVelocity;
            }
            else
            {
                _jumpingVelocity = _jumpingVelocity - _jumpingSpeed;
                if (_jumpingVelocity < 0)
                {
                    _jumpingVelocity = 0;
                }
                Sprite.Position.Y = Sprite.Position.Y - _jumpingVelocity - 1.0f;
            }


            _lastplayerDirection = direction;


            Jumping = false;
            Sprite.Alive = true;
        }

        private void JumpingHandler()
        {
            if(_hitLowJump)
                _jumpingVelocity = _jumpingVelocity + _jumpingSpeed;
                    
            if (_hitMaxJump)
                _jumpingVelocity = _jumpingVelocity - _jumpingSpeed + 1.0f;

            if (_jumpingVelocity > 170.0f)
            {
                _jumpingSpeed = 4.0f;
            }
            else
            {
                _jumpingSpeed = 8.0f;
            }

            if (_jumpingVelocity > 180.0f)
            {
                _hitMaxJump = true;
                _hitLowJump = false;
            }
            if(_jumpingVelocity < 0)
            {
                _hitLowJump = true;
                _hitMaxJump = false;
            }

            
        }

        private void SetCurrentDirection(PlayerDirection direction)
        {
            _currentPlayerDirection = direction;
        }

        public void UpdatePlayer(GameTime gameTime, int direction)
        {
            Sprite.UpdateSprite(gameTime, direction);
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
