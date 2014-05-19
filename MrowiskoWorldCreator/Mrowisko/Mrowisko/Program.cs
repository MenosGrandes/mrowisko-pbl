using System;

namespace AntHill
{
#if WINDOWS || XBOX
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        static void Main(string[] args)
        {
            Form1 mainForm = new Form1();
            mainForm.Show();
            using (Game1 game = new Game1(mainForm))
            {
                game.Run();
            }
        }
    }
#endif
}

