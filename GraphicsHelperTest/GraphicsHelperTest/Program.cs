using System;

namespace GraphicsHelperTest
{
#if WINDOWS || XBOX
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        static void Main(string[] args)
        {
            using (HelperTest game = new HelperTest())
            {
                game.Run();
            }
        }
    }
#endif
}

