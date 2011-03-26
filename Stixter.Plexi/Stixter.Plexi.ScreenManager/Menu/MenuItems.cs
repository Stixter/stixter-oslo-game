namespace Stixter.Plexi.ScreenManager.Menu
{
    public static class MenuItems
    {
        public enum MenuItem { StartGame, HighScore, Help, EndGame };

        public static string GetLabelFromItem(MenuItem item)
        {
            if (item.Equals(MenuItem.StartGame))
                return "Start Game";
            if (item.Equals(MenuItem.Help))
                return "Help";
            if (item.Equals(MenuItem.HighScore))
                return "High Score";
            if (item.Equals(MenuItem.EndGame))
                return "End Game";

            return string.Empty;
        }
    }

  

}
