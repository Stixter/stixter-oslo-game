using System;
using System.Collections.Generic;
using System.Threading;
using Microsoft.Xna.Framework;

namespace Stixter.Plexi.ScreenManager.GameScreens
{
    public class LevelHandler : GameComponent
    {
        private readonly List<Platform> _platformsLevel1;
        private readonly List<Platform> _platformsRandom;
        private readonly List<Platform> _platformsStartScreen;
        private Random _random;

        public LevelHandler(Game game) : base(game)
        {
            _platformsLevel1 = new List<Platform>();
            _platformsStartScreen = new List<Platform>();
            _platformsRandom = new List<Platform>();
            _random = new Random();
            CreatePlatformsLevel1();
            CreatePlatformsStartScreen();
            CreateRandomLevel();
        }

        public void CreateRandomLevel()
        {
            _platformsRandom.Clear();
            var numberOfPlatforms = _random.Next(8, 10);
            
            _platformsRandom.Add(new Platform(30, 700, 0, Game));

            var y = 650;
            var x = 0;
            var lastPlatform = CreateRandomPlatform(y, x);
            for(var i = 0; i<numberOfPlatforms; i++)
            {
                y = lastPlatform.GetPostionY() - (50 + _random.Next(0, 20));
                x = x + (lastPlatform.GetLength()*100);
                if (x > 900)
                    x = 0;

                var platform = CreateRandomPlatform(y, x);
                _platformsRandom.Add(platform);
                lastPlatform = platform;
            }
        }

        private Platform CreateRandomPlatform(int y, int x)
        {
            return new Platform(_random.Next(3, 10), _random.Next(y, y + 20), _random.Next(x, x + 100), Game);
        }

        private void CreatePlatformsLevel1()
        {
            _platformsLevel1.Add(new Platform(30, 700, 0, Game));
            _platformsLevel1.Add(new Platform(7, 600, 210, Game));
            _platformsLevel1.Add(new Platform(5, 600, 700, Game));
            _platformsLevel1.Add(new Platform(10, 500, 0, Game));
            _platformsLevel1.Add(new Platform(5, 500, 900, Game));
            _platformsLevel1.Add(new Platform(3, 400, 700, Game));
            _platformsLevel1.Add(new Platform(8, 390, 190, Game));
            _platformsLevel1.Add(new Platform(5, 250, 90, Game));
            _platformsLevel1.Add(new Platform(6, 300, 1000, Game));
            _platformsLevel1.Add(new Platform(7, 240, 520, Game));
        }

        private void CreatePlatformsStartScreen()
        {
            _platformsStartScreen.Add(new Platform(30, 700, 0, Game));
            _platformsStartScreen.Add(new Platform(7, 600, 240, Game));
            _platformsStartScreen.Add(new Platform(5, 600, 700, Game));
            _platformsStartScreen.Add(new Platform(8, 500, 0, Game));
            _platformsStartScreen.Add(new Platform(6, 490, 950, Game));
            _platformsStartScreen.Add(new Platform(3, 390, 100, Game));
            _platformsStartScreen.Add(new Platform(6, 380, 1000, Game));
            _platformsStartScreen.Add(new Platform(5, 280, 890, Game));
            _platformsStartScreen.Add(new Platform(5, 290, 150, Game));
        }

        public List<Platform> GetPlatformLevel1()
        {
            return _platformsLevel1;
        }

        public List<Platform> GetRandomLevel()
        {
            return _platformsRandom;
        }

        public List<Platform> GetPlatformStartScreen()
        {
            return _platformsStartScreen;
        }
    }
}
