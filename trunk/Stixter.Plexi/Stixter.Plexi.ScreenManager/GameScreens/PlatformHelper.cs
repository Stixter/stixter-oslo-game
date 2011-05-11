using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Stixter.Plexi.Sprites.Sprites;

namespace Stixter.Plexi.ScreenManager.GameScreens
{
    public class LevelHandler : GameComponent
    {
        private readonly List<Platform> _platforms;

        public LevelHandler(Game game) : base(game)
        {
            _platforms = new List<Platform>();
            CreatePlatforms();
        }

        private void CreatePlatforms()
        {
            _platforms.Add(new Platform(30, 700, 0, Game));
            _platforms.Add(new Platform(5, 500, 200, Game));
            _platforms.Add(new Platform(10, 300, 0, Game));
            _platforms.Add(new Platform(5, 200, 100, Game));
            _platforms.Add(new Platform(5, 500, 700, Game));
        }

        public List<Platform> GetPlatformLevel1()
        {
            return _platforms;
        }
    }

    public class Platform : GameComponent
    {
        private readonly int _lenght;
        private readonly int _postitionY;
        private readonly int _postitionX;
        private readonly List<Floor> _floorList;

        public Platform(int length, int positionY, int positionX, Game game) : base(game)
        {
            _lenght = length;
            _postitionY = positionY;
            _postitionX = positionX;
            _floorList = new List<Floor>();
            CreateFloor();
        }

        public float CheckHit(Player player)
        {
            var playerAimRect = player.PlayerFootHit();

            foreach (var floor in _floorList)
            {
                var enemyRec = floor.GetFloorRec();
                if (playerAimRect.Intersects(enemyRec))
                {
                    player.AllowJump = true;
                    return floor.Sprite.Position.Y - 85;
                }
            }

            return 0;
        }

        private void CreateFloor()
        {
            for (var i = 0; i < _lenght; i++)
            {
                var floorItem = new Floor(Game);
                _floorList.Add(floorItem);
            }
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            var countingPositionX = _postitionX;
            
            foreach (var item in _floorList)
            {
                item.Sprite.Position.Y = _postitionY;
                item.Sprite.Position.X = countingPositionX;
                item.Draw(spriteBatch);
                countingPositionX += 50;
            }
        }
    } 
}
