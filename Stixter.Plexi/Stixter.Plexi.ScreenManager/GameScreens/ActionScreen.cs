using System;
using System.Linq;
using System.Collections.Generic;
using System.Threading;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Stixter.Plexi.Core;
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
        private readonly InformationPanel _informationPanel;
        private Player _player;
        private DeadCharacter _deadCharacter;
        private List<Platform> _platforms;
        private KeyboardState _oldKeyboardState;
        private readonly SoundEffect _playerDies;
        private readonly ICollectItemHandler _collectItemHandler;
        private readonly ICloudHandler _cloudHandler;
        private readonly IEnemiesHandler _enemiesHandler;
        
        public ActionScreen(Game game, SpriteBatch spriteBatch, Texture2D image) : base(game, spriteBatch)
        {
            _image = image;
            _imageRectangle = new Rectangle(0, 0, Game.Window.ClientBounds.Width, Game.Window.ClientBounds.Height);
     
            _informationPanel = new InformationPanel(game);
            _informationPanel.Reset();

            _cloudHandler = new CloudHandler(game);
     
            _playerDies = game.Content.Load<SoundEffect>("Sounds\\pain");

            _collectItemHandler =  new CollectItemHandler();
            _collectItemHandler.Init(game);
            _collectItemHandler.CreatePickUpItems();

            CreatePlayer(game);
            _enemiesHandler = new EnemiesHandler(game);
            BuildPlatforms();
            _collectItemHandler.SetPickUpItemsOnPlatforms(_platforms);
        }

        public bool CheckIfGameIsOver()
        {
            return !_player.Sprite.Alive;
        }

        private void CreatePlayer(Game game)
        {
            _deadCharacter = new DeadCharacter(game);
            _player = new Player(game, "Sprites\\player_move");
        }

        private void BuildPlatforms()
        {
            var platformHandlerLevel1 = new LevelHandler(CurrentGame);
            _platforms = platformHandlerLevel1.GetRandomLevel();
        }

        public override void Update(GameTime gameTime)
        {
            GameTimerHandler.TotalGameTime = (int)gameTime.TotalGameTime.TotalSeconds;

           
            _cloudHandler.Update(gameTime);
     
            _keyboardState = Keyboard.GetState();

            CheckIfCharacterIsJumpingAndChangeState();
            SetCharacterMoveDirection();

            _enemiesHandler.Update(gameTime);

            if (!_player.Sprite.Alive)
            {
                _deadCharacter.Sprite.Position.Y = _player.Sprite.Position.Y + 30;
                _deadCharacter.Sprite.Position.X = _player.Sprite.Position.X + 20;
            }

            CheckPlatformHit();
            CheckIfEnemiesKillsPlayer();
            _collectItemHandler.CheckIfPlayerPickUpItemAndCreateNewItems(_player.GetPlayerRec(), _platforms);
            _oldKeyboardState = _keyboardState;
            base.Update(gameTime);
        }

        private void CheckIfEnemiesKillsPlayer()
        {
            if(_enemiesHandler.CheckIfEnemiesKills(_player.CharacterKillingHit()))
            {
                if (_player.Sprite.Alive)
                    _playerDies.Play();
                _player.Sprite.Alive = false;
            }
        }

        private void CheckIfCharacterIsJumpingAndChangeState()
        {
            if (_keyboardState.IsKeyDown(Keys.Space) && !_oldKeyboardState.IsKeyDown(Keys.Space))
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
            var dictionary = new Dictionary<int, bool>();
            var enemies = _enemiesHandler.GetEnemies();
            foreach (var platform in _platforms)
            {
                if (platform.CheckFloorHit(_player) != 0)
                {
                    _player.HitFloor(platform.CheckFloorHit(_player));
                    playerHitAnyPlatform = true;
                }

                for (int index = 0; index < enemies.Count; index++)
                {
                    var enemy = enemies[index];
                    enemy.AllowJump = false;
                    if (platform.CheckFloorHit(enemy) != 0)
                    {
                        enemy.HitFloor(platform.CheckFloorHit(enemy));
                        dictionary.Add(index, true);
                    }
                }
            }

            foreach (KeyValuePair<int, bool> keyValuePair in dictionary)
            {
                enemies[keyValuePair.Key].AllowJump = keyValuePair.Value;
            }

            _player.AllowJump = playerHitAnyPlatform;
        }

        public override void Draw(GameTime gameTime)
        {
            SpriteBatch.Draw(_image, _imageRectangle, Color.White);
            
            _informationPanel.Draw(SpriteBatch);
            _cloudHandler.Draw(SpriteBatch);

            DrawAllPlatforms();
            DrawPlayerIfAliveElseDrawDeathSprite(gameTime);

            _enemiesHandler.Draw(gameTime, SpriteBatch);
            _collectItemHandler.DrawAllItems(SpriteBatch);
           
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

        public void ResetGame()
        {
            GameTimerHandler.TotalGameTime = 0;
            _player.Reset();

            _enemiesHandler.Reset();

            BuildPlatforms();

            _collectItemHandler.SetPickUpItemsOnPlatforms(_platforms);
            _cloudHandler.Reset();
            _informationPanel.Reset();
        }
    }
}
