using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using Stixter.Plexi.Core;
using Stixter.Plexi.ScreenManager.GameScreens;
using Stixter.Plexi.Sprites.Sprites;

namespace Stixter.Plexi.ScreenManager.Handlers
{
    public class CollectItemHandler : ICollectItemHandler
    {
        private List<PickUpItem> _pickUpItems;
        private readonly int _numberOfPickUpItems;
        private Game _game;
        private SoundEffect _soundEffect;

        public CollectItemHandler()
        {
            _numberOfPickUpItems = 3;
        }

        public void Init(Game game)
        {
            _game = game;
            _soundEffect = game.Content.Load<SoundEffect>("Sounds\\coin-drop-4");
        }

        public void CreatePickUpItems()
        {
            if (_game != null)
            {
                _pickUpItems = new List<PickUpItem>();
                for (var i = 0; i < _numberOfPickUpItems; i++)
                {
                    var pickUpItem = new PickUpItem(_game);
                    _pickUpItems.Add(pickUpItem);
                }
            }
        }

        public void SetPickUpItemsOnPlatforms(List<Platform> platforms)
        {
            var itemCount = 0;
            foreach (var pickUpItem in _pickUpItems)
            {
                ItemPositionHandler.PlaceItem(itemCount, platforms, pickUpItem);
                itemCount++;
            }
        }

        public void CheckIfPlayerPickUpItemAndCreateNewItems(Rectangle targetRectangel, List<Platform> platforms)
        {
            if (_pickUpItems.Aggregate(true, (current, pickUpItem) => CheckIfPlayerPickUpItem(pickUpItem, targetRectangel, current)))
                SetPickUpItemsOnPlatforms(platforms);
        }

        private bool CheckIfPlayerPickUpItem(PickUpItem pickUpItem, Rectangle targetRectangel, bool allItemIsCollected)
        {
            ItemIsPickedUp(pickUpItem, targetRectangel);

            if (pickUpItem.Sprite.Alive)
                allItemIsCollected = false;
            return allItemIsCollected;
        }

        private void ItemIsPickedUp(PickUpItem pickUpItem, Rectangle targetRectangel)
        {
            if (pickUpItem.GetFloorRec().Intersects(targetRectangel) && pickUpItem.Sprite.Alive)
                PickUpAction(pickUpItem);
        }

        private void PickUpAction(PickUpItem pickUpItem)
        {
            _soundEffect.Play();
            ScoreHandler.AddPoint();
            pickUpItem.Sprite.Alive = false;
        }

        public void DrawAllItems(SpriteBatch batch)
        {
            foreach (var item in _pickUpItems)
                item.Draw(batch);
        }
    }
}