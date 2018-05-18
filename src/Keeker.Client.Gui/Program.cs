using Newtonsoft.Json;
using System;
using System.IO;
using System.Reflection;
using System.Text;
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
            var json = JsonConvert.SerializeObject(appSettings, Formatting.Indented);
            var filePath = this.CreateSettingsFilePath();
            File.WriteAllText(filePath, json, Encoding.UTF8);
        }

        public AppSettings LoadSettings()
        {
            try
            {
                var filePath = this.CreateSettingsFilePath();
                var json = File.ReadAllText(filePath, Encoding.UTF8);
                var appSettings = JsonConvert.DeserializeObject<AppSettings>(json);
                return appSettings;
            }
            catch
            {
                // dismiss for now
                return null;
            }
        }

        private string CreateSettingsFilePath()
        {
            var exePath = Assembly.GetEntryAssembly().Location;
            var exeDirPath = Path.GetDirectoryName(exePath);
            var path = Path.Combine(exeDirPath, "AppSettings.json");
            return path;
        }

        private void Run()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new ClientForm());
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
