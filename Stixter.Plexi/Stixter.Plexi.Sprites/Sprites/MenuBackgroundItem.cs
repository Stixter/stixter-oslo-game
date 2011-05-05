using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace Stixter.Plexi.Sprites.Sprites
{
    public class MenuBackgroundItem
    {
        private readonly ContentManager _contentManager;
        public GameSprite Sprite;
        private Texture2D _selectedTexture;
        private Texture2D _notSelectedTexture;

        public MenuBackgroundItem(ContentManager contentManager)
        {
            _contentManager = contentManager;
            CreateObject();
        }

        private void CreateObject()
        {
            _selectedTexture = _contentManager.Load<Texture2D>("Sprites\\menu_back_selected");
            _notSelectedTexture = _contentManager.Load<Texture2D>("Sprites\\menu_back_notselected");
            Sprite = new GameSprite(_notSelectedTexture) { Alive = true };
        }

        public void SetSelected()
        {
            Sprite.Sprite = _selectedTexture;
        }

        public void SetNotSelected()
        {
            Sprite.Sprite = _notSelectedTexture;
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(Sprite.Sprite, Sprite.Position, null, Color.White, Sprite.Rotation, Sprite.Center, 1.0f, SpriteEffects.None, 0);
        }
    }
}
