using System;
using System.Collections.Generic;
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

        private List<Platform> _platforms;
        private KeyboardState _oldKeyboardState;

        private int _playerDirection = 0;

        public ActionScreen(Game game, SpriteBatch spriteBatch, Texture2D image)
            : base(game, spriteBatch)
        {
            _image = image;
            _imageRectangle = new Rectangle(0, 0, Game.Window.ClientBounds.Width, Game.Window.ClientBounds.Height);

            _character = new Player(game, "Sprites\\player_move");

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

            if (_keyboardState.IsKeyDown(Keys.Space))
                _character.PlayerState = Character.State.Jumping;

            if (_keyboardState.IsKeyDown(Keys.Right))
                _character.MoveCharacter(AnimatedSprite.PlayerDirection.Right);
            else if (_keyboardState.IsKeyDown(Keys.Left))
                _character.MoveCharacter(AnimatedSprite.PlayerDirection.Left);
            else
                _character.MoveCharacter(AnimatedSprite.PlayerDirection.None);

            _oldKeyboardState = _keyboardState;

            CheckPlatformHit();

            base.Update(gameTime);
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
            }

            _character.AllowJump = playerHitAnyPlatform;
        }

        public override void Draw(GameTime gameTime)
        {
            SpriteBatch.Draw(_image, _imageRectangle, Color.White);

            foreach (Platform platform in _platforms)
            {
                platform.Draw(SpriteBatch);
            }

            _character.UpdatePlayer(gameTime);
            _character.Draw(SpriteBatch);

            base.Draw(gameTime);
        }

    }
}
