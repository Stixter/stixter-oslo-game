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
        private Random _random;

        public Cloud(Game game, Random random) : base(game)
        {
            _random = random;
            
            _contentManager = game.Content;
            CreateObject();

            var speed = (float) _random.NextDouble();
            if (speed == 0.0)
                speed = 0.1f;    
            Sprite.Velocity = new Vector2(1 + speed, 0);

        }

        private int GetRandomNumber(int nLow, int nHigh)
        {
            return _random.Next(nLow, nHigh);
            return (_random.Next() % (nHigh - nLow + 1)) + nLow;
        } 

        private void CreateObject()
        {
            var selectTexture = GetRandomNumber(0, 1);
            if (selectTexture.Equals(1))
                _texture = _contentManager.Load<Texture2D>("Sprites\\cloud");
            else
            {
                _texture = _contentManager.Load<Texture2D>("Sprites\\cloud_small");
            }
            Sprite = new GameSprite(_texture) { Alive = true };

            Sprite.Position.X = GetRandomNumber(1280, 1800);
            Sprite.Position.Y = GetRandomNumber(100, 600);
        }

        public override void Update(GameTime gameTime)
        {
            Sprite.Position.X = Sprite.Position.X - Sprite.Velocity.X;
            if (Sprite.Position.X < -_random.Next(100, 300))
            {
                Random r = new Random();
                Sprite.Position.X = r.Next(1280, 1500);
            }
            base.Update(gameTime);
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(Sprite.Sprite, Sprite.Position, null, Color.White, Sprite.Rotation, Sprite.Center, 1.0f, SpriteEffects.None, 0);
        }
    }
}