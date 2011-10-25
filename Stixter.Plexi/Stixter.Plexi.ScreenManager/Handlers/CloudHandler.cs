using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Stixter.Plexi.Sprites.Sprites;

namespace Stixter.Plexi.ScreenManager.Handlers
{
    public class CloudHandler : GameComponent, ICloudHandler
    {
        private readonly List<Cloud> _clouds;
        private readonly int _numberOfClouds;

        public CloudHandler(Game game) : base(game)
        {
            _clouds = new List<Cloud>();
            _numberOfClouds = new Random().Next(10, 20);
            Reset();
        }

        public void Reset()
        {
            CreateClouds(Game);
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
            foreach (var cloud in _clouds)
                cloud.Update(gameTime);
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            foreach (var cloud in _clouds)
                cloud.Draw(spriteBatch);
        }

        private void CreateClouds(Game game)
        {
            _clouds.Clear();
            for (var i = 0; i < _numberOfClouds; i++)
                _clouds.Add(new Cloud(game));
        } 
    }
}