using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Stixter.Plexi.Sprites
{
    public class AnimatedSprite
    {
        public Texture2D Sprite;
        public Vector2 Position;
        public float Rotation;
        public Vector2 Center;
        public Vector2 Velocity;
        public bool Alive;
        public bool Hidden;
        public float Timer;
        public float Interval = 300f / 25f;
        public int FrameCount = 16;
        public int CurrentFrame;
        public int SpriteWidth = 45; 
        public int SpriteHeight = 67;
        public Rectangle SourceRect;
        private int _playerPosition = 0;
        public int Direction = 0;

        public AnimatedSprite(Texture2D loadTexture)
        {
            Sprite = loadTexture;
            Hidden = false;
            Rotation = 0.0f;
            Position = Vector2.Zero;
            Center = new Vector2(SourceRect.Width / 2, SourceRect.Height / 2);
            Velocity = Vector2.Zero;
            Alive = false;
        }

        public Rectangle UpdateSprite(GameTime gameTime, int direction)
        {
            Timer += (float)gameTime.ElapsedGameTime.TotalMilliseconds;
            if (Timer > Interval)
            {
                CurrentFrame++;
                if (CurrentFrame > FrameCount - 1)
                {
                    const int positionOne = 0;
                    const int positionTwo = 50;
                    const int positionThree = 100;
                    const int postionFour = 150;

                    var directionCord = direction.Equals(0) ? 74 : 148;

                    CurrentFrame = 0;

                    if(direction.Equals(2))
                    {
                        SourceRect = new Rectangle(100, 3, SpriteWidth, SpriteHeight);
                    }
                    else
                    {
                        if (_playerPosition.Equals(0))
                        {
                            SourceRect = new Rectangle(positionOne, directionCord, SpriteWidth, SpriteHeight);
                        }
                        if (_playerPosition.Equals(1))
                        {
                            SourceRect = new Rectangle(positionTwo, directionCord, SpriteWidth, SpriteHeight);
                        }
                        if (_playerPosition.Equals(2))
                        {
                            SourceRect = new Rectangle(positionThree, directionCord, SpriteWidth, SpriteHeight);
                        }
                        if (_playerPosition.Equals(3))
                        {
                            SourceRect = new Rectangle(postionFour, directionCord, SpriteWidth, SpriteHeight);
                        }

                        _playerPosition++;

                        if (_playerPosition.Equals(4))
                            _playerPosition = 0;
                    }
                    
                }

                Timer = 0f;
            }
            return SourceRect;
            
        }
    }
}
