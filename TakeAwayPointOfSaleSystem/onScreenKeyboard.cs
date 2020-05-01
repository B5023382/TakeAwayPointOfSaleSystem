using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Bunifu.UI.WinForms;
using Bunifu.UI.WinForms.BunifuButton;

namespace TakeAwayPointOfSaleSystem
{
    class onScreenKeyboard
    {
        private Control activedTextbox;
        private Control leftShift;
        private Control rightShift;

        public onScreenKeyboard(Control leftS, Control rightS)
        {
            leftShift = leftS;
            rightShift = rightS;
        }

        private bool shiftPressed = false;
        private bool capsPressed = false;

        public void setCotrol(Control activedControl)
        {
            activedTextbox = activedControl;
            activedTextbox.Focus();
        }
        
       public void keyboard_click (object sender, EventArgs e)
       {
           if (activedTextbox != null)
           {
               BunifuButton btn = (BunifuButton)sender;
               activedTextbox.Focus();
               if (shiftPressed)
               {
                   SendKeys.Send('+' + btn.Text);
                   press_shift();
               }
               else
               {
                   SendKeys.Send(btn.Text);
               }
               
            }
       }

       public void press_enter()
       {
           if (activedTextbox != null)
           {
               activedTextbox.Focus();
               SendKeys.Send("{ENTER}");
           }
        }

       public void press_space()
       {
           if (activedTextbox != null)
           {
               activedTextbox.Focus();
               SendKeys.Send(" ");
           }
       }

        public void press_backspace()
        {
            if (activedTextbox != null)
            {
                activedTextbox.Focus();
                SendKeys.Send("{BACKSPACE}");
            }
        }

        public void press_capsLock(object sender, EventArgs e)
        {
            if (activedTextbox != null)
            {
                activedTextbox.Focus();
                SendKeys.Send("{CAPSLOCK}");
                capsPressed = !capsPressed;
                BunifuButton capsLock = (BunifuButton)sender;
                if (capsPressed)
                {
                    capsLock.IdleBorderThickness = 5;
                }
                else
                {
                    capsLock.IdleBorderThickness = 1;
                }
                

            }
        }

        public void press_shift()
        {
            shiftPressed = !shiftPressed;

            BunifuButton leftShiftButton = (BunifuButton)leftShift;
            BunifuButton rightShiftButton = (BunifuButton)rightShift;
            if (shiftPressed)
            {
                leftShiftButton.IdleBorderThickness = 5;
                rightShiftButton.IdleBorderThickness = 5;
            }
            else
            {
                leftShiftButton.IdleBorderThickness = 1;
                rightShiftButton.IdleBorderThickness = 1;
            }

        }

        public void press_clear()
        {
            if (activedTextbox != null)
            {
                BunifuTextBox textBox = (BunifuTextBox) activedTextbox;
                activedTextbox.Focus();
                textBox.Text = "";
            }
        }

    }
}
