using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace Stixter.Plexi.Sprites.Sprites
{
    public class Cloud : GameComponent
    {
        private readonly ContentManager _contentManager;
        public GameSprite Sprite;
        private Texture2D _texture;
        private readonly GraphicsDevice _graphicsDeviceService;
        private const string SmallCloudTexture = "Sprites\\cloud_small";
        private const string BigCloudTexture = "Sprites\\cloud";

        public Cloud(Game game) : base(game)
        {
            _contentManager = game.Content;
            var graphicsDeviceService = Game.Services.GetService(typeof(IGraphicsDeviceService)) as IGraphicsDeviceService;
            if (graphicsDeviceService != null) _graphicsDeviceService = graphicsDeviceService.GraphicsDevice;

            CreateObject();
            SetCloudVelocity();
        }

        private void SetCloudVelocity()
        {
            var speed = (float)RandomHelper.Instance.NextDouble();
            
            if (speed.Equals(0.0))
                speed = 0.1f;  
  
            Sprite.Velocity = new Vector2(1 + speed, 0);
        }

        private void CreateObject()
        {
            RandomizeTexture();
            Sprite = new GameSprite(_texture) { Alive = true };
            RadomizeSpritePostion();
        }

        private void RadomizeSpritePostion()
        {
            Sprite.Position.X = RandomHelper.Instance.Next(0, _graphicsDeviceService.Viewport.Width + 800);
            Sprite.Position.Y = RandomHelper.Instance.Next(100, _graphicsDeviceService.Viewport.Height - 200);
        }

        private void RandomizeTexture()
        {
            var selectTexture = RandomHelper.Instance.Next(0, 100);
            _texture = _contentManager.Load<Texture2D>(selectTexture > 50 ? BigCloudTexture : SmallCloudTexture);
        }

        public override void Update(GameTime gameTime)
        {
            Sprite.Position.X = Sprite.Position.X - Sprite.Velocity.X;
            if (Sprite.Position.X < -RandomHelper.Instance.Next(100, 300))
                Sprite.Position.X = RandomHelper.Instance.Next(_graphicsDeviceService.Viewport.Width + 100, _graphicsDeviceService.Viewport.Width + 800);

            base.Update(gameTime);
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(Sprite.Sprite, Sprite.Position, null, Color.White, Sprite.Rotation, Sprite.Center, 1.0f, SpriteEffects.None, 0);
        }
    }
}