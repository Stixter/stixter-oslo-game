namespace Stixter.Plexi.Core
{
    public static class GameTimerHandler
    {
        public static int TotalGameTime { get; set; }
        public static int LastGameStartTime { get; set; }

        public static int CurrentGameTime
        {
            get
            {
                if (TotalGameTime > LastGameStartTime)
                    return (TotalGameTime - LastGameStartTime);// - (ScoreHandler.TotalScore *2);
                else
                {
                    return 0;
                }
            }
        }
    }
}