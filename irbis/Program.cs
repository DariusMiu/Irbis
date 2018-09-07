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
            using (var game = new Irbis())
            {
                try
                { game.Run(); }
                catch (Exception e)
                {
                    Irbis.WriteLine("Exception: " + e.Message);
                    Irbis.WriteLine("Data: " + e.Data);
                    Irbis.WriteLine("StackTrace:\n" + e.StackTrace);
                    Console.WriteLine("Exception: " + e.Message);
                }
                finally
                { if (Irbis.Crash) { Irbis.ExportConsole(); } }
            }
        }
    }
}
