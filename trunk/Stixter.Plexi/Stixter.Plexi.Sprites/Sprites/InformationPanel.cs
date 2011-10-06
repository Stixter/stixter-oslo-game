using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace Stixter.Plexi.Sprites.Sprites
{
    public class InformationPanel : GameComponent
    {
        private readonly ContentManager _contentManager;
        public GameSprite Sprite;
        private Texture2D _texture;
        private ScreenText _timeText;
        private ScreenText _pointsText;
        public string CurrentTime = "notime";
        public int Points = 0;

        public InformationPanel(Game game)
            : base(game)
        {
            _contentManager = game.Content;
            _timeText = new ScreenText(game);
            _timeText.SetPosition(1000, 50);

            _pointsText = new ScreenText(game);
            _pointsText.SetPosition(1000, 20);

            _contentManager = game.Content;
            CreateObject();
            Sprite.Position.X = 1100;
            Sprite.Position.Y = 50;
        }

        public Rectangle GetFloorRec()
        {
            return new Rectangle(
                (int)Sprite.Position.X,
                (int)Sprite.Position.Y,
                Sprite.Sprite.Width,
                Sprite.Sprite.Height);
        }

        private void CreateObject()
        {
            _texture = _contentManager.Load<Texture2D>("Sprites\\information_panel");
            Sprite = new GameSprite(_texture) { Alive = true };
        }

        public void AddPoint()
        {
            Points++;
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            if (Sprite.Alive)
                spriteBatch.Draw(Sprite.Sprite, Sprite.Position, null, Color.White, Sprite.Rotation, Sprite.Center, 1.0f, SpriteEffects.None, 0);
            _timeText.Draw(spriteBatch, string.Format("Total time: {0}", CurrentTime));
            _pointsText.Draw(spriteBatch, string.Format("Points: {0}", Points));
        }
    }
}