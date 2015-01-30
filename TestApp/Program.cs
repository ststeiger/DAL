using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace TestApp
{
    static class Program
    {
        /// <summary>
        /// Der Haupteinstiegspunkt für die Anwendung.
        /// </summary>
        [STAThread]
        static void Main()
        {
            // http://stackoverflow.com/questions/10409576/pass-table-valued-parameter-using-ado-net
            // var x = System.Data.SqlDbType.Structured;


            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new frmMain());
        }
    }
}
