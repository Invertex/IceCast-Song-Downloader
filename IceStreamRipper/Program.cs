using System;
using System.Windows.Forms;

namespace Invertex
{
	//Do thing
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