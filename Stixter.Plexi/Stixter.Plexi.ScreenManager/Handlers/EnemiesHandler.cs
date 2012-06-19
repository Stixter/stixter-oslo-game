using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using Stixter.Plexi.Core;
using Stixter.Plexi.Sprites.Sprites;

namespace Stixter.Plexi.ScreenManager.Handlers
{
    public class EnemiesHandler : GameComponent, IEnemiesHandler
    {
        private readonly List<Enemy> _enemies;
        private const int NumberOfEnemies = 1;
        private bool _isOkToAddNewEnemy = false;
        private int _lastEnemyAdded = 0;
        private const int NewEnemyRate = 10;
        private readonly SoundEffect _newEnemySound;

        public EnemiesHandler(Game game) : base(game)
        {
            _newEnemySound = game.Content.Load<SoundEffect>("Sounds\\newenemy");
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
            if (GameTimerHandler.CurrentGameTime % NewEnemyRate == 0)
            {
                if (_isOkToAddNewEnemy)
                {
                    AddEnemy();
                    _newEnemySound.Play();
                    _lastEnemyAdded = GameTimerHandler.CurrentGameTime;
                    _isOkToAddNewEnemy = false;
                }
                if (GameTimerHandler.CurrentGameTime != _lastEnemyAdded)
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
        }

        public bool CheckIfEnemiesKills(Rectangle objectToKill)
        {
            return _enemies.Any(enemy => enemy.CharacterKillingHit().Intersects(objectToKill) && enemy.IsDeadly);
        }

        private void CreateEnemies()
        {
            float start = 50;
            for (var i = 0; i < NumberOfEnemies; i++)
            {
                start += 150;
                _enemies.Add(new Enemy(Game, "Sprites\\enemy_move", start, new Random()));
            }
        }
    }
}