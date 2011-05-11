using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Stixter.Plexi.Sprites.Helpers;

namespace Stixter.Plexi.Sprites.Sprites
{
    public class Enemy : Character
    {
        public Enemy(Game game, string texture) : base(game, texture)
        {
            MaxCharacterVelocity = 3.0f;
            LastPlayerY = 10f;
            Sprite.Position.X = 100f;
        }

        public void MoveCharacter()
        {
            Sprite.Position.Y = LastPlayerY;

            SetCorretSpeedOnPlayer();

            if (Sprite.Position.X >= 1000 && CurrentPlayerDirection == AnimatedSprite.PlayerDirection.Right)
            {
                CurrentPlayerDirection = AnimatedSprite.PlayerDirection.Left;
            }
            else if (Sprite.Position.X <= 10 && CurrentPlayerDirection == AnimatedSprite.PlayerDirection.Left)
            {
                CurrentPlayerDirection = AnimatedSprite.PlayerDirection.Right;
            }

            SetCorretSpeedOnPlayer();
            MovePlayerLeftOrRight();
            LastplayerDirection = CurrentPlayerDirection;

            Sprite.Position.Y = LastPlayerY + 5.0f;
            LastPlayerY = Sprite.Position.Y;
            PlayerState = State.Walking;
            Sprite.Alive = true;
        }
    }
    public class Player : Character
    {
        public Player(Game game, string texture) : base(game, texture)
        {
            MaxCharacterVelocity = 7.0f;
            LastPlayerY = 312f;
            Sprite.Position.X = 400f;
        }

        public void MoveCharacter(AnimatedSprite.PlayerDirection direction)
        {
            Sprite.Position.Y = LastPlayerY;

            SetCurrentDirection(direction);
            SetCorretSpeedOnPlayer();
            MovePlayerLeftOrRight();

            if (PlayerState.Equals(State.Jumping) && AllowJump)
                Jump();

            LastplayerDirection = direction;

            if (!JumpInProgress)
                Sprite.Position.Y = LastPlayerY + 5.0f;
            else
                Sprite.Position.Y = LastPlayerY - 5.0f;

            LastPlayerY = Sprite.Position.Y;

            PlayerState = State.Walking;
            Sprite.Alive = true;
        }
    }

    public class Character : GameComponent
    {
        public enum State { Walking, Jumping }
        public State PlayerState;
        public AnimatedSprite Sprite;

        private readonly GraphicsDevice _graphicsDevice;
        private Rectangle _viewportRect;
        private double _currentTimer;
        private double _startJumpTime;
        const double TimeInAir = 0.5;
        protected float MaxCharacterVelocity = 7.0f;
        protected bool JumpInProgress;
        protected float CurrentVelocity = 1.0f;
        protected float LastPlayerY;
        public bool AllowJump;
        protected AnimatedSprite.PlayerDirection LastplayerDirection;
        protected AnimatedSprite.PlayerDirection CurrentPlayerDirection;
        private readonly string _texture;
   
        public Character(Game game, string texture) : base(game)
        {
            _texture = texture;
            
            var graphicsDeviceService = Game.Services.GetService(typeof(IGraphicsDeviceService)) as IGraphicsDeviceService;
            _graphicsDevice = graphicsDeviceService.GraphicsDevice;

            CreateGameObject();
        }

        public Rectangle PlayerFootHit()
        {
            return new Rectangle(
                       (int)Sprite.Position.X + 50,
                       (int)Sprite.Position.Y,
                       Sprite.Sprite.Width - 180,
                       Sprite.Sprite.Height - 200);
        }

        public Rectangle CharacterKillingHit()
        {
            return new Rectangle(
                       (int)Sprite.Position.X + 50,
                       (int)Sprite.Position.Y,
                       Sprite.Sprite.Width - 180,
                       Sprite.Sprite.Height - 200);
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
            if (!JumpInProgress)
            {
                Sprite.Position.Y = posY;
                LastPlayerY = Sprite.Position.Y;
            }
        }

        protected void SetCorretSpeedOnPlayer()
        {
            if((LastplayerDirection == CurrentPlayerDirection))
            {
                if(CurrentVelocity < MaxCharacterVelocity)
                    CurrentVelocity = CurrentVelocity + 0.07f;
            }
            else
                CurrentVelocity = 0;
        }

        protected void Jump()
        {
            if(!JumpInProgress)
            {
                JumpInProgress = true;
                _startJumpTime = _currentTimer;
            }
        }

        protected void MovePlayerLeftOrRight()
        {
            if (CurrentPlayerDirection == AnimatedSprite.PlayerDirection.Right)
                Sprite.Position.X = Sprite.Position.X + CurrentVelocity;

            if (CurrentPlayerDirection == AnimatedSprite.PlayerDirection.Left)
                Sprite.Position.X = Sprite.Position.X - CurrentVelocity;
        }

        protected void SetCurrentDirection(AnimatedSprite.PlayerDirection direction)
        {
            CurrentPlayerDirection = direction;
        }

        public void UpdatePlayer(GameTime gameTime)
        {
            Sprite.UpdateSprite(gameTime, CurrentPlayerDirection);
            SpritePosition.KeepSpriteOnScreen(Sprite, _graphicsDevice);
            
            _currentTimer = gameTime.TotalGameTime.TotalSeconds;

            if(JumpInProgress)
                CheckIfStillJumping();
        }

        protected void CheckIfStillJumping()
        {
            var timeDiffrent = _currentTimer - _startJumpTime;
            JumpInProgress = timeDiffrent <= TimeInAir;
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
