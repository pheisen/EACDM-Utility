using System;
using System.Linq;
using System.Collections.Generic;
using System.Windows.Forms;
using System.Xml;
using System.Xml.Linq;
using System.IO;

namespace EACDMLinqUtiliy
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
#if (mil)
            //Skybound.Gecko.Xpcom.Initialize(@"C:\Program Files\Mozilla Firefox"); //C:\utility\xulrunner
            
            string path = Path.GetDirectoryName(Application.ExecutablePath);
            path = path + @"\xulrunner";
            Skybound.Gecko.Xpcom.Initialize(path);
#endif
            Application.EnableVisualStyles();
            Application.Run(new Form1());
           //Application.Run(new MoodleCats());
        }
    }
}