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
            _platforms.Add(new Platform(7, 600, 210, Game));
            _platforms.Add(new Platform(5, 600, 700, Game));
            _platforms.Add(new Platform(10, 500, 0, Game));
            _platforms.Add(new Platform(5, 500, 900, Game));
            _platforms.Add(new Platform(3, 400, 700, Game));
            _platforms.Add(new Platform(8, 390, 190, Game));
            _platforms.Add(new Platform(5, 250, 90, Game));
            _platforms.Add(new Platform(6, 300, 1000, Game));
            _platforms.Add(new Platform(7, 240, 520, Game));
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

        public float CheckHit(Character character)
        {
            var playerAimRect = character.PlayerFootHit();

            foreach (var floor in _floorList)
            {
                var enemyRec = floor.GetFloorRec();
                if (playerAimRect.Intersects(enemyRec))
                {
                    character.AllowJump = true;
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
