using System;
using System.Windows.Forms;

namespace Keeker.Client.Gui
{
    internal class Program
    {
        public static Program Instance { get; private set; }

        private Program()
        {
            
        }

        public void SaveSettings(AppSettings appSettings)
        {
            throw new NotImplementedException();
        }

        private void Run()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new MainForm());
        }

        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        private static void Main()
        {
            var program = new Program();
            Instance = program;
            program.Run();
        }
    }
}
