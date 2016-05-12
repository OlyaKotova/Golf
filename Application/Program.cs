using System;
using System.Windows.Forms;

using System.Text;
using System.Net;
using System.Net.Sockets;

namespace SceneEditor
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new MainForm());
            Application.Run(new MainForm1());
            Application.Run(new MainForm2());
        }

        
    }
}

