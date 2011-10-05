using System;
using System.Collections.Generic;
using System.Threading;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Stixter.Plexi.Sprites;
using Stixter.Plexi.Sprites.Sprites;

namespace Stixter.Plexi.ScreenManager.GameScreens
{
    public class ActionScreen : GameScreen
    {
        private KeyboardState _keyboardState;
        private readonly Texture2D _image;
        private readonly Rectangle _imageRectangle;
        private readonly Player _character;
        private readonly List<Enemy> _enemys;
        private const int _numberOfEnemys = 4;
        private DeadCharacter _deadCharacter;

        private List<Platform> _platforms;
        private KeyboardState _oldKeyboardState;

        public ActionScreen(Game game, SpriteBatch spriteBatch, Texture2D image)
            : base(game, spriteBatch)
        {
            _image = image;
            _imageRectangle = new Rectangle(0, 0, Game.Window.ClientBounds.Width, Game.Window.ClientBounds.Height);
            _deadCharacter = new DeadCharacter(game);

            _character = new Player(game, "Sprites\\player_move");

            _enemys = new List<Enemy>();
            float start = 50;
            for (var i = 0; i < _numberOfEnemys; i++)
            {
                start += 50;
                Thread.Sleep(20);
                _enemys.Add(new Enemy(game, "Sprites\\enemy_move", start, new Random()));
            }
                

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

            CheckIfCharacterIsJumpingAndChangeState();

            SetCharacterMoveDirection();

            foreach (var enemy in _enemys)
            {
                enemy.MoveCharacter();
            }

            if (!_character.Sprite.Alive)
            {
                _deadCharacter.Sprite.Position.Y = _character.Sprite.Position.Y + 30;
                _deadCharacter.Sprite.Position.X = _character.Sprite.Position.X + 20;
            }
            _oldKeyboardState = _keyboardState;

            CheckPlatformHit();

            base.Update(gameTime);
        }

        private void CheckIfCharacterIsJumpingAndChangeState()
        {
            if (_keyboardState.IsKeyDown(Keys.Space))
                _character.PlayerState = Character.State.Jumping;
        }

        private void SetCharacterMoveDirection()
        {
            if (_keyboardState.IsKeyDown(Keys.Right))
                _character.MoveCharacter(AnimatedSprite.PlayerDirection.Right);
            else if (_keyboardState.IsKeyDown(Keys.Left))
                _character.MoveCharacter(AnimatedSprite.PlayerDirection.Left);
            else
                _character.MoveCharacter(AnimatedSprite.PlayerDirection.None);
        }

        private void CheckPlatformHit()
        {
            bool playerHitAnyPlatform = false;
            foreach (var platform in _platforms)
            {
                if(platform.CheckHit(_character) != 0)
                {
                    _character.HitFloor(platform.CheckHit(_character));
                    playerHitAnyPlatform = true;
                }

                foreach (var enemy in _enemys)
                {
                    if (platform.CheckHit(enemy) != 0)
                    {
                        enemy.HitFloor(platform.CheckHit(enemy));
                        if (enemy.CharacterKillingHit().Intersects(_character.CharacterKillingHit()))
                        {
                            _character.Sprite.Alive = false;
                        }
                    }
                }
                
            }

            _character.AllowJump = playerHitAnyPlatform;
        }

        public override void Draw(GameTime gameTime)
        {
            SpriteBatch.Draw(_image, _imageRectangle, Color.White);
            DrawAllPlatforms();
            DrawPlayerIfAliveElseDrawDeathSprite(gameTime);

            foreach (Enemy enemy in _enemys)
            {
                enemy.UpdatePlayer(gameTime);
                enemy.Draw(SpriteBatch);
            }
           
            base.Draw(gameTime);
        }

        private void DrawPlayerIfAliveElseDrawDeathSprite(GameTime gameTime)
        {
            if (_character.Sprite.Alive)
            {
                _character.UpdatePlayer(gameTime);
                _character.Draw(SpriteBatch);
            }
            else
                _deadCharacter.Draw(SpriteBatch);
        }

        private void DrawAllPlatforms()
        {
            foreach (var platform in _platforms)
                platform.Draw(SpriteBatch);
        }
    }
}
