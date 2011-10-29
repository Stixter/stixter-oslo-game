using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Stixter.Plexi.Sprites.Sprites;

namespace Stixter.Plexi.ScreenManager.GameScreens
{
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

        public int GetPostionX()
        {
            return _postitionX;
        }

        public int GetPostionY()
        {
            return _postitionY;
        }

        public int GetLength()
        {
            return _lenght;
        }

        public float CheckFloorHit(Character character)
        {
            var playerAimRect = character.PlayerFootHit();

            foreach (var floor in _floorList)
            {
                var floorRec = floor.GetFloorRec();
                if (playerAimRect.Intersects(floorRec))
                {
                    character.AllowJump = true;
                    return floor.Sprite.Position.Y - 85;
                }
            }

            return 0;
        }

        public List<Rectangle> GetFloor()
        {
            return _floorList.Select(item => item.GetCompleteFloorRec()).ToList();
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
                countingPositionX += item.GetCompleteFloorRec().Width;
            }
        }
    }
}