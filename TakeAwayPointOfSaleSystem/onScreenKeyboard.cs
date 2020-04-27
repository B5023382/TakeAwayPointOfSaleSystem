using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Bunifu.UI.WinForms.BunifuButton;

namespace TakeAwayPointOfSaleSystem
{
    class onScreenKeyboard
    {
        private Control activedTextbox;

        public void setCotrol(Control activedControl)
        {
            activedTextbox = activedControl;
        }
        
       public void keyboard_click (object sender, EventArgs e)
       {
           if (activedTextbox != null)
           {
               BunifuButton btn = (BunifuButton)sender;
               activedTextbox.Focus();
               SendKeys.Send(btn.Text);
            }
       }

    }
}
