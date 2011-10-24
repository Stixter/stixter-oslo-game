using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using Stixter.Plexi.Core;
using Stixter.Plexi.ScreenManager.GameScreens;
using Stixter.Plexi.Sprites.Sprites;


namespace Stixter.Plexi.ScreenManager.Handlers
{
    public interface ICollectItemHandler
    {
        void Init(Game game);
        void CreatePickUpItems();
        void SetPickUpItemsOnPlatforms(List<Platform> platforms);
        void CheckIfPlayerPickUpItemAndCreateNewItems(Rectangle targetRectangel, List<Platform> platforms);
        void DrawAllItems(SpriteBatch batch);
    }

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
            var allItemIsCollected = true;
            foreach (var pickUpItem in _pickUpItems)
            {
                if (pickUpItem.GetFloorRec().Intersects(targetRectangel) && pickUpItem.Sprite.Alive)
                {
                    _soundEffect.Play();
                    ScoreHandler.AddPoint();
                    pickUpItem.Sprite.Alive = false;
                }

                if (pickUpItem.Sprite.Alive)
                    allItemIsCollected = false;
            }

            if (allItemIsCollected)
                SetPickUpItemsOnPlatforms(platforms);
        }

        public void DrawAllItems(SpriteBatch batch)
        {
            foreach (var item in _pickUpItems)
            {
                item.Draw(batch);
            }
        }
    }
}
