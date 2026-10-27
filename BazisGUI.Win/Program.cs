using System;
using System.Globalization;
using System.Threading;
using System.Windows.Forms;
using BazisGUI.Win.AvaloniaUI.Hosting;

namespace BazisGUI.Win
{
    static class Program
    {
        /// <summary>
        /// Главная точка входа для приложения.
        /// </summary>
        [STAThread]
        static void Main(string[] args)
        {
            //Thread.CurrentThread.CurrentUICulture = new CultureInfo("en");
            Thread.CurrentThread.CurrentCulture = CultureInfo.InvariantCulture;
            //Application.EnableVisualStyles();
            //Application.SetCompatibleTextRenderingDefault(false);
            ApplicationConfiguration.Initialize();
            AvaloniaHost.Initialize();
            Application.Run(new BaseForm(args));
        }
    }
}
