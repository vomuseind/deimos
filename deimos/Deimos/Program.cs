using System;

namespace Deimos
{
#if WINDOWS
    static class Program
    {
        public static string PlayerEmail = "test@mail.com";
        public static string PlayerToken = "23601116db467d50c6d8722dffb378b9";

        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        static void Main(string[] args)
        {
            try
            {
                PlayerEmail = args[0];
                PlayerToken = args[1];
            }
            catch (Exception)
            {
                // This is because it hasn't been executed with enough parameters
                // Exiting
                //return;
            }

            using (DeimosGame game = new DeimosGame())
            {
                game.Run();
            }
        }
    }
#endif
}

