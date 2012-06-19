using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Stixter.Plexi.Sprites.Sprites
{
    public class BlinkText
    {
        private readonly SpriteFont _font;
        private readonly int _blinkTime;
        private int _currentBlinkTime;
        private Vector2 _fontPos;
        private bool _blink;

        public BlinkText(SpriteFont font, int blinkTime)
        {
            _font = font;
            _blinkTime = blinkTime;
            _blink = false;
            _fontPos = new Vector2();
        }

        public void Update(int elapsedMs)
        {
            _currentBlinkTime += elapsedMs;
            if (_currentBlinkTime > _blinkTime)
            {
                _blink = !_blink;
                _currentBlinkTime -= _blinkTime;
            }
        }

        public void SetPosition(int x, int y)
        {
            _fontPos.X = x;
            _fontPos.Y = y;
        }

        public void SetSpacing(int spacing)
        {
            _font.Spacing = spacing;
        }

        public void Draw(SpriteBatch spriteBatch, string text)  
        {  
            if (_blink)
                spriteBatch.DrawString(_font, text, _fontPos, Color.Red, 0, new Vector2(0, 0), 1.0f, SpriteEffects.None, 0.5f);
            else
            {
                spriteBatch.DrawString(_font, text, _fontPos, Color.Green, 0, new Vector2(0, 0), 1.0f, SpriteEffects.None, 0.5f);  
            }
        }
    }
}