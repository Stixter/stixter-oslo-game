using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Stixter.Plexi.Core;
using Stixter.Plexi.ScreenManager.Menu;
using Stixter.Plexi.Sprites.Sprites;

namespace Stixter.Plexi.ScreenManager.GameScreens
{
    public class StartScreen : GameScreen
    {
        private readonly MenuComponent _menuComponent;
        private readonly Texture2D _image;
        private readonly Rectangle _imageRectangle;
        private List<Platform> _platforms;
        private List<Enemy> _enemies;
        private ScreenText _resultText;
        private string _result;

        public StartScreen(Game game, SpriteBatch spriteBatch, SpriteFont spriteFont, Texture2D image)
            : base(game, spriteBatch)
        {
            var menuItems = new List<MenuItems.MenuItem>
                                    {
                                        MenuItems.MenuItem.StartGame, 
                                        MenuItems.MenuItem.Help, 
                                        MenuItems.MenuItem.HighScore, 
                                        MenuItems.MenuItem.EndGame
                                    };

            _menuComponent = new MenuComponent(game, spriteBatch, spriteFont, menuItems);

            _resultText = new ScreenText(game);
            _resultText.SetPosition(500, 150);
            Components.Add(_menuComponent);
            BuildPlatforms();
            CreateEnemies(game);
            _image = image;
            _imageRectangle = new Rectangle(0, 0, Game.Window.ClientBounds.Width, Game.Window.ClientBounds.Height);
        }

        private void CreateEnemies(Game game)
        {
            _enemies = new List<Enemy>();
            var random = new Random();
            float start = 50;
            for (var i = 0; i < random.Next(6, 15); i++)
            {
                start += 150;
                Thread.Sleep(20);
                _enemies.Add(new Enemy(game, "Sprites\\enemy_move", start, new Random()));
            }
        }

        private void BuildPlatforms()
        {
            var platformHandlerLevel1 = new LevelHandler(CurrentGame);
            _platforms = platformHandlerLevel1.GetPlatformStartScreen();
        }

        public int SelectedIndex
        {
            get { return _menuComponent.SelectedIndex; }
            set { _menuComponent.SelectedIndex = value; }
        }

        public override void Update(GameTime gameTime)
        {
            foreach (var enemy in _enemies)
                enemy.MoveCharacter();

            var dictionary = new Dictionary<int, bool>();
            foreach (var platform in _platforms)
            {
                for (int index = 0; index < _enemies.Count; index++)
                {
                    var enemy = _enemies[index];
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
                _enemies[keyValuePair.Key].AllowJump = keyValuePair.Value;
            }

            base.Update(gameTime);
        }

        public void SetScoreText()
        {
            _result = "Last game result: " + ScoreHandler.TotalScore;
        }

        public override void Draw(GameTime gameTime)
        {
            SpriteBatch.Draw(_image, _imageRectangle, Color.White);
            DrawAllPlatforms();

            if (!string.IsNullOrEmpty(_result))
            {
                _resultText.Draw(SpriteBatch, _result);
            }

            foreach (var enemy in _enemies)
            {
                enemy.UpdatePlayer(gameTime);
                enemy.Draw(SpriteBatch);
            }
            base.Draw(gameTime);
        }

        private void DrawAllPlatforms()
        {
            foreach (var platform in _platforms)
                platform.Draw(SpriteBatch);
        }
    }
}
