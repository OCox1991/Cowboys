using System;

namespace Cowboys
{
#if WINDOWS || XBOX
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        static void Main(string[] args)
        {
            using (Cowboys game = new Cowboys())
            {
                game.Run();
            }
        }
    }
#endif
}

