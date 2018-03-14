using System;

namespace Irbis
{
    /// <summary>
    /// The main class.
    /// </summary>
    public static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        //[STAThread]
        static void Main()
        {
            try
            {
                using (var game = new Irbis())
                { game.Run(); }
            }
            catch (Exception e)
            {
                Irbis.WriteLine("Exception: " + e.Message);
                Console.WriteLine("Exception: " + e.Message);
            }
            finally
            { if (Irbis.Crash) { Irbis.ExportConsole(); } }
        }
    }
}
