using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace TakeAwayPointOfSaleSystem
{
    static class Program
    {
        static ApplicationContext mainContext = new ApplicationContext();
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);


            //mainContext.MainForm = new frmLogin();
            mainContext.MainForm = new FrmMain();
            //mainContext.MainForm = new testForm();
            Application.Run(mainContext);
        }

        public static void SetMainForm(Form mainForm)
        {
            mainContext.MainForm = mainForm;
        }

        public static void ShowMainForm()
        {
            mainContext.MainForm.Show();
        }
    }
}
