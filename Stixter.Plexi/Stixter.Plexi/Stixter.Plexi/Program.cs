namespace Stixter.Plexi
{
#if WINDOWS || XBOX
    static class Program
    {
        static void Main()
        {
            using (var game = new PlexiGame())
            {
                game.Run();
            }


        }
    }
#endif
}

