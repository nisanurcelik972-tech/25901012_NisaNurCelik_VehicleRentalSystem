
using System;
using System.Windows.Forms;
using VehicleRentalSystem.Forms;

namespace VehicleRentalSystem
{
    internal static class Program
    {
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            Application.Run(new MainForm());
        }
    }
}
