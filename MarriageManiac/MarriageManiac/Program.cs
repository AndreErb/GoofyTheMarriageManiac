using System;

namespace MarriageManiac
{
#if WINDOWS || XBOX
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        static void Main(string[] args)
        {
            using (GoofyGame game = new GoofyGame())
            {
                game.Window.Title = "Goofy The Marriage Maniac";
                game.Run();
            }
        }
    }
#endif
}

