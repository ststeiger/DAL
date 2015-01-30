
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
            RandomizeFileTimestamps();
            
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new frmMain());
        }


        // Don't show in git when a file was worked on...
        public static void RandomizeFileTimestamps()
        {
            string path = @"d:\test.txt";

            if (System.IO.File.Exists(path))
            {
                Random r = new Random();
                int iMins = r.Next(-120, 120);
                int iSecs = r.Next(0, 60);

                System.DateTime creationTime = System.DateTime.Today
                    .AddDays(-1)
                    .AddHours(22)
                    .AddMinutes(iMins)
                    .AddSeconds(iSecs)
                ;
                System.IO.File.SetCreationTime(path, creationTime);



                iMins = r.Next(0, 120);
                iSecs = r.Next(0, 60);
                System.DateTime lastWriteTime = creationTime
                    .AddMinutes(iMins)
                    .AddSeconds(iSecs)
                ;

                System.IO.File.SetLastWriteTime(path, lastWriteTime);
                iMins = r.Next(0, 120);
                iSecs = r.Next(0, 60);
                System.DateTime lastAccessTime = lastWriteTime
                    .AddMinutes(iMins)
                    .AddSeconds(iSecs)
                ;

                System.IO.File.SetLastAccessTime(path, lastAccessTime);
            } // End if (System.IO.File.Exists(path))
        } // End Sub RandomizeFileTimestamps


    }


}
