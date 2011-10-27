using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using Stixter.Plexi.Sprites.Helpers;

namespace Stixter.Plexi.Sprites.Sprites
{
    public class Character : GameComponent
    {
        public enum State { Walking, Jumping }
        public State PlayerState;
        public AnimatedSprite Sprite;

        private readonly GraphicsDevice _graphicsDevice;
        private double _currentTimer;
        private double _startJumpTime;
        const double TimeInAir = 0.52;
        protected float MaxCharacterVelocity = 7.0f;
        protected bool JumpInProgress;
        protected float CurrentVelocity = 1.0f;
        protected float LastPlayerY;
        protected bool EnableJumpSound;
        public bool AllowJump;
        protected float FallingVelocity = 1.0f;
        protected float JumpingVelocity = 1.0f;
        protected AnimatedSprite.PlayerDirection LastplayerDirection;
        protected AnimatedSprite.PlayerDirection CurrentPlayerDirection;
        private readonly string _texture;
        private readonly SoundEffect _jumpSound;
        private readonly ScreenText _screenText;
        protected string DebugText = string.Empty;
        
   
        public Character(Game game, string texture) : base(game)
        {
            _texture = texture;
            _screenText = new ScreenText(Game);
            var graphicsDeviceService = Game.Services.GetService(typeof(IGraphicsDeviceService)) as IGraphicsDeviceService;
            _graphicsDevice = graphicsDeviceService.GraphicsDevice;
            _jumpSound = game.Content.Load<SoundEffect>("Sounds\\jump");
            CreateGameObject();
        }

        public virtual Rectangle PlayerFootHit()
        {
            return new Rectangle(
                       (int)Sprite.Position.X + 50,
                       (int)Sprite.Position.Y -1,
                       Sprite.Sprite.Width - 180,
                       Sprite.Sprite.Height - 220);
        }

        public Rectangle CharacterKillingHit()
        {
            return new Rectangle(
                       (int)Sprite.Position.X + 50,
                       (int)Sprite.Position.Y - 10,
                       Sprite.Sprite.Width - 180,
                       Sprite.Sprite.Height - 220);
        }

        public Rectangle GetPlayerRec()
        {
            return new Rectangle(
                (int)Sprite.Position.X + 25, 
                (int)Sprite.Position.Y + 10,
                Sprite.SourceRect.Width - 20, 
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
                Sprite.Position.Y = posY - 2;
                LastPlayerY = Sprite.Position.Y;
                FallingVelocity = 4.5f;
                JumpingVelocity = 4.5f;
            }
        }

        protected void SetCorretSpeedOnPlayer()
        {
            if((LastplayerDirection == CurrentPlayerDirection))
            {
                if(CurrentVelocity < MaxCharacterVelocity)
                    CurrentVelocity = CurrentVelocity + 0.11f;
            }
            else
                CurrentVelocity = 0;
        }

        protected void Jump()
        {
            if(!JumpInProgress)
            {
                if (EnableJumpSound)
                    _jumpSound.Play();
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

            _screenText.SetPosition((int)Sprite.Position.X, (int)Sprite.Position.Y - 50);
            if (JumpInProgress)
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
                //_screenText.Draw(spriteBatch, DebugText);
                spriteBatch.Draw(Sprite.Sprite, Sprite.Position, Sprite.SourceRect, Color.White, Sprite.Rotation, Sprite.Center, 1.0f, SpriteEffects.None, 0);
            }
        }
    }
}
