global using System;
global using System.Linq;

namespace ProjectLoot
{
    /// <summary>
    /// The main class.
    /// </summary>
    public static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main(string[] args)
        {
            using (var game = new Game1())
            {
                var byEditor = args.Contains("LaunchedByEditor");

                if (byEditor)
                {
                    try
                    {
                        game.Run();
                    }
                    catch (Exception e)
                    {
                        System.IO.File.WriteAllText("CrashInfo.txt", e.ToString());
                        throw;
                    }
                }
                else
                {
                    game.Run();
                }

            }
        }
    }
}
