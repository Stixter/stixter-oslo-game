using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Stixter.Plexi.Core;
using Stixter.Plexi.Sprites.Sprites;

namespace Stixter.Plexi.ScreenManager.Handlers
{
    public class EnemiesHandler : GameComponent, IEnemiesHandler
    {
        private readonly List<Enemy> _enemies;
        private const int NumberOfEnemies = 3;
        private bool _isOkToAddNewEnemy = false;
        private int _lastEnemyAdded = 0;
        private const int NewEnemyRate = 10;

        public EnemiesHandler(Game game) : base(game)
        {
            _enemies = new List<Enemy>();
            CreateEnemies();
            _isOkToAddNewEnemy = false;
        }

        public List<Enemy> GetEnemies()
        {
            return _enemies;
        }

        public void AddEnemy()
        {
            _enemies.Add(new Enemy(Game, "Sprites\\enemy_move", 10, new Random()));
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
            foreach (var enemy in _enemies)
                enemy.MoveCharacter();
        }

        public void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {

            if (GameTimerHandler.TotalGameTime % NewEnemyRate == 0)
            {
                if (_isOkToAddNewEnemy)
                {
                    AddEnemy();
                    
                    _lastEnemyAdded = GameTimerHandler.TotalGameTime;
                    _isOkToAddNewEnemy = false;
                }
                if (GameTimerHandler.TotalGameTime != _lastEnemyAdded)
                    _isOkToAddNewEnemy = true;

            }

            foreach (var enemy in _enemies)
            {
                enemy.UpdatePlayer(gameTime);
                enemy.Draw(spriteBatch);
            }
        }

        public void Reset()
        {
            _lastEnemyAdded = 0;
            _enemies.Clear();
            CreateEnemies();
            //foreach (var enemy in _enemies)
            //    enemy.Reset();
            
        }

        public bool CheckIfEnemiesKills(Rectangle objectToKill)
        {
            return _enemies.Any(enemy => enemy.CharacterKillingHit().Intersects(objectToKill));
        }

        private void CreateEnemies()
        {
            float start = 50;
            for (var i = 0; i < NumberOfEnemies; i++)
            {
                start += 150;
                Thread.Sleep(20);
                _enemies.Add(new Enemy(Game, "Sprites\\enemy_move", start, new Random()));
            }
        }
    }
}