
using System.Collections.Generic;


namespace Oracle.Native
{


    public class OracleClient
    {

        protected static string RegexDirSeparator
        {

            get
            {
                // http://www.mikesdotnetting.com/Article/46/CSharp-Regular-Expressions-Cheat-Sheet
                return System.Text.RegularExpressions.Regex.Escape(System.IO.Path.DirectorySeparatorChar.ToString());
            }

        } // End Property RegexDirSeparator


        // http://stackoverflow.com/questions/281145/asp-net-hostingenvironment-shadowcopybinassemblies
        public static void EnsureDllsLoaded()
        {
            int iBitNess = System.IntPtr.Size * 8;

            string strTargetDirectory = System.Reflection.Assembly.GetExecutingAssembly().Location;
            strTargetDirectory = System.IO.Path.GetDirectoryName(strTargetDirectory);

            string strSourcePath = "~/bin/dependencies/InstantClient/";

            if (System.Environment.OSVersion.Platform == System.PlatformID.Unix)
            {
                strSourcePath += "Linux" + iBitNess.ToString();
            }
            else
            {
                strSourcePath += "Win" + iBitNess.ToString();
            }

            strSourcePath = System.Web.HttpContext.Current.Server.MapPath(strSourcePath);

            string[] astrAllFiles = System.IO.Directory.GetFiles(strSourcePath, "*.dll");

            //string strDebug = string.Join(" ", astrAllFiles);

            foreach (string strSourceFile in astrAllFiles)
            {
                string strTargetFile = System.IO.Path.GetFileName(strSourceFile);
                strTargetFile = System.IO.Path.Combine(strTargetDirectory, strTargetFile);
                if (!System.IO.File.Exists(strTargetFile))
                    System.IO.File.Copy(strSourceFile, strTargetFile, true);

                // http://stackoverflow.com/questions/4571088/how-to-programatically-read-native-dll-imports-in-c
                //if(strTargetFile.EndsWith("orannzsbb11.dll", StringComparison.OrdinalIgnoreCase))
                //    continue; // Unneeded exception thrower

                if (System.Text.RegularExpressions.Regex.IsMatch(strTargetFile, @"^(.*" + RegexDirSeparator + @")?orannzsbb11\.(dll|so|dylib)$", System.Text.RegularExpressions.RegexOptions.IgnoreCase))
                    continue; // Unneeded exception thrower

                try
                {
                    Platform.SharedLibrary.Load(strTargetFile);
                }
                catch (System.Exception ex)
                {
                    System.Console.WriteLine(ex.Message);
                }

            } // Next strSourceFile

        } // End Sub EnsureDllsLoaded


    } // End Class OracleClient


} // End Namespace Oracle
