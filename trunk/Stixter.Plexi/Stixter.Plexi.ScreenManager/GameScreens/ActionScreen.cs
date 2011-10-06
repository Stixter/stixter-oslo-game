using System;
using System.Linq;
using System.Collections.Generic;
using System.Threading;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Stixter.Plexi.ScreenManager.Handlers;
using Stixter.Plexi.Sprites;
using Stixter.Plexi.Sprites.Sprites;

namespace Stixter.Plexi.ScreenManager.GameScreens
{
    public class ActionScreen : GameScreen
    {
        private KeyboardState _keyboardState;
        private readonly Texture2D _image;
        private readonly Rectangle _imageRectangle;
        private InformationPanel _informationPanel;
        private Player _player;
        private List<Enemy> _enemies;
        private List<PickUpItem> _pickUpItems;
        private const int NumberOfEnemys = 6;
        private DeadCharacter _deadCharacter;
        private List<Platform> _platforms;
        private KeyboardState _oldKeyboardState;

        public ActionScreen(Game game, SpriteBatch spriteBatch, Texture2D image)
            : base(game, spriteBatch)
        {
            _image = image;
            _imageRectangle = new Rectangle(0, 0, Game.Window.ClientBounds.Width, Game.Window.ClientBounds.Height);
            
            _informationPanel = new InformationPanel(game);
            CreatePlayer(game);
            CreateEnemies(game);
            CreatePickUpItems(game);
            BuildPlatforms();
            SetPickUpItemsOnPlatforms();
        }

        private void CreatePlayer(Game game)
        {
            _deadCharacter = new DeadCharacter(game);
            _player = new Player(game, "Sprites\\player_move");
        }

        private void CreatePickUpItems(Game game)
        {
            _pickUpItems = new List<PickUpItem>();
            for (var i = 0; i < 3; i++)
            {
                var light = new PickUpItem(game);
                _pickUpItems.Add(light);
            }
        }

        private void CreateEnemies(Game game)
        {
            _enemies = new List<Enemy>();
            float start = 50;
            for (var i = 0; i < NumberOfEnemys; i++)
            {
                start += 150;
                Thread.Sleep(20);
                _enemies.Add(new Enemy(game, "Sprites\\enemy_move", start, new Random()));
            }
        }

        private void BuildPlatforms()
        {
            var platformHandlerLevel1 = new LevelHandler(CurrentGame);
            _platforms = platformHandlerLevel1.GetPlatformLevel1();
        }

        private void SetPickUpItemsOnPlatforms()
        {
            var lightCount = 0;
            foreach (var pickUpItem in _pickUpItems)
            {
                ItemPositionHandler.PlaceItem(lightCount, _platforms, pickUpItem);
                lightCount++;
            }
        }

        public override void Update(GameTime gameTime)
        {
            var totalSeconds = 30 - gameTime.TotalGameTime.Seconds;
            if (totalSeconds == 0)
            {
                _player.Sprite.Alive = false;
            }

            _informationPanel.CurrentTime = totalSeconds.ToString();
            _keyboardState = Keyboard.GetState();

            CheckIfCharacterIsJumpingAndChangeState();
            SetCharacterMoveDirection();

            foreach (var enemy in _enemies)
                enemy.MoveCharacter();

            if (!_player.Sprite.Alive)
            {
                _deadCharacter.Sprite.Position.Y = _player.Sprite.Position.Y + 30;
                _deadCharacter.Sprite.Position.X = _player.Sprite.Position.X + 20;
            }

            _oldKeyboardState = _keyboardState;

            CheckPlatformHit();
            CheckIfEnemiesKillsPlayer();
            CheckIfPlayerPickUpItemAndCreateNewItems();

            base.Update(gameTime);
        }

        private void CheckIfPlayerPickUpItemAndCreateNewItems()
        {
            var allLightsAreDead = true;
            foreach (var pickUpItem in _pickUpItems)
            {
                if (pickUpItem.GetFloorRec().Intersects(_player.GetPlayerRec()) && pickUpItem.Sprite.Alive)
                {
                    _informationPanel.AddPoint();
                    pickUpItem.Sprite.Alive = false;
                }

                if (pickUpItem.Sprite.Alive)
                    allLightsAreDead = false;
            }

            if (allLightsAreDead)
                SetPickUpItemsOnPlatforms();
        }

        private void CheckIfEnemiesKillsPlayer()
        {
            foreach (var enemy in _enemies)
            {
                if (enemy.CharacterKillingHit().Intersects(_player.CharacterKillingHit()))
                    _player.Sprite.Alive = false;
            }
        }

        private void CheckIfCharacterIsJumpingAndChangeState()
        {
            if (_keyboardState.IsKeyDown(Keys.Space))
                _player.PlayerState = Character.State.Jumping;
        }

        private void SetCharacterMoveDirection()
        {
            if (_keyboardState.IsKeyDown(Keys.Right))
                _player.MoveCharacter(AnimatedSprite.PlayerDirection.Right);
            else if (_keyboardState.IsKeyDown(Keys.Left))
                _player.MoveCharacter(AnimatedSprite.PlayerDirection.Left);
            else
                _player.MoveCharacter(AnimatedSprite.PlayerDirection.None);
        }

        private void CheckPlatformHit()
        {
            var playerHitAnyPlatform = false;

            foreach (var platform in _platforms)
            {
                if (platform.CheckFloorHit(_player) != 0)
                {
                    _player.HitFloor(platform.CheckFloorHit(_player));
                    playerHitAnyPlatform = true;
                }

                foreach (var enemy in _enemies)
                {
                    if (platform.CheckFloorHit(enemy) != 0)
                    {
                        enemy.HitFloor(platform.CheckFloorHit(enemy));
                        enemy.AllowJump = true;
                    }
                }
            }

            _player.AllowJump = playerHitAnyPlatform;
        }

        public override void Draw(GameTime gameTime)
        {
            SpriteBatch.Draw(_image, _imageRectangle, Color.White);
            _informationPanel.Draw(SpriteBatch);
            DrawAllPlatforms();
            DrawPlayerIfAliveElseDrawDeathSprite(gameTime);

            foreach (var enemy in _enemies)
            {
                enemy.UpdatePlayer(gameTime);
                enemy.Draw(SpriteBatch);
            }

            foreach (PickUpItem light in _pickUpItems)
            {
                light.Draw(SpriteBatch);
            }

           
            base.Draw(gameTime);
        }

        private void DrawPlayerIfAliveElseDrawDeathSprite(GameTime gameTime)
        {
            if (_player.Sprite.Alive)
            {
                _player.UpdatePlayer(gameTime);
                _player.Draw(SpriteBatch);
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
