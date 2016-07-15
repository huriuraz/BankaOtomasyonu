using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

namespace _13253065P_
{
    static class Program
    {
        public static string girisyapilan;
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new Formlogin());
        }
    }
}
