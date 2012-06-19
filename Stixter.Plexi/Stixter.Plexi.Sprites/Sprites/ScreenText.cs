using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Stixter.Plexi.Sprites.Sprites
{
    public class ScreenText : GameComponent
    {
        private readonly SpriteFont _font;
        private Vector2 _fontPos;

        public ScreenText(Game game)
            : base(game)
        {
            _font = game.Content.Load<SpriteFont>("Fonts\\GameFont");
            _fontPos = new Vector2();
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
            spriteBatch.DrawString(_font, text, _fontPos, Color.WhiteSmoke, 0, new Vector2(0,0), 1.0f, SpriteEffects.None, 0.5f);
        }
    }
}