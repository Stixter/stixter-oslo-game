using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Stixter.Plexi.Sprites.Sprites;

namespace Stixter.Plexi.ScreenManager.Handlers
{
    public interface IEnemiesHandler
    {
        void Update(GameTime gameTime);
        bool CheckIfEnemiesKills(Rectangle objectToKill);
        List<Enemy> GetEnemies();
        void Draw(GameTime gameTime, SpriteBatch spriteBatch);
        void Reset();
        void AddEnemy();
    }
}