namespace Stixter.Plexi.Core
{
    public static class ScoreHandler
    {
        public static int TotalScore { get; private set; }

        public static void AddPoint()
        {
            TotalScore = TotalScore + 1;
        }

        public static void Reset()
        {
            TotalScore = 0;
        }
    }
}