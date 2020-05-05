using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace TakeAwayPointOfSaleSystem
{
    static class Program
    {
        private static ApplicationContext mainContext = new ApplicationContext();
        public static FrmMain mainPage;
        
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            mainContext.MainForm = new FrmMain("Wei", "admin");
            mainPage = (FrmMain)mainContext.MainForm;
            //mainContext.MainForm = new frmLogin();
            Application.Run(mainContext);
        }

        public static void SetMainForm(FrmMain mainForm)
        {
            mainPage = mainForm;
            mainContext.MainForm = mainPage;
        }

        public static void SetActiveForm(Form form)
        {
            mainContext.MainForm = form;
        }

        public static void ShowForm()
        {
            mainContext.MainForm.Show();
        }

        public static void ShowMainForm()
        {
            mainContext.MainForm = mainPage;
            mainContext.MainForm.Show();
        }
    }
}
