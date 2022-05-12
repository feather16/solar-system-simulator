using System;
using System.Windows.Forms;

namespace SolarSystemSimulator
{
    static class SolarSystemSimulator
    {
        public readonly static int INTERVAL = 5;
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new MainWindow());
        }
    }
}