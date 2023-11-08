using System;
using System.Windows.Forms;

namespace Invertex
{
    class Program
    {
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new IceStreamForm());
        }
    }
}