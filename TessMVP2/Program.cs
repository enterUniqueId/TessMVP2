using System;
using System.Windows.Forms;
using TessMVP2.Presenter;
using TessMVP2.View.Interfaces;

namespace TessMVP2
{
    static class Program
    {
        
        /// <summary>
        /// Der Haupteinstiegspunkt für die Anwendung.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            var cont = new TessPresenter();

            //form.btnClick += new FormStart.btnClickHandler(cont.startProgramm);
            Application.Run();
            
        }
    }
}
