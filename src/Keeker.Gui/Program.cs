using System;
using System.IO;
using System.Windows.Forms;

namespace Keeker.Gui
{
    internal class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        private static void Main()
        {
            Instance = new Program();
            Instance.Run();
        }

        internal static Program Instance { get; private set; }

        internal TextWriter LogWriter { get; set; }

        private void Run()
        {
            LogWriter = TextWriter.Null;

            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new MainForm());
            //Application.Run(new RaceChartForm());
            //Application.Run(new LogForm());
        }
    }
}
