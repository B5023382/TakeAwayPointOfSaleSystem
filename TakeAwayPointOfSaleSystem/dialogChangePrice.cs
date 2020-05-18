using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Bunifu.UI.WinForms.BunifuButton;

namespace TakeAwayPointOfSaleSystem
{
    public partial class dialogChangePrice : Form
    {
        public string newData { get; set; }

        private char symbol;
        private bool changePrice = false;
        private string defultPrice;
        public dialogChangePrice(char s, string titleText, string clearText, string defultText)
        {
            InitializeComponent();
            symbol = s;
            btnSideDot.Text = s.ToString();
            defultPrice = defultText;
            lblTitle.Text = titleText;
            txtNewPrice.Text = defultText;
            txtNewPrice.SelectAll();
            if(clearText != null)
            {
                btnNormal.Text = clearText;
            }
            else
            {
                changePrice = true;
            }

            if (s == ' ')
            {
                btnSideDot.Enabled = false;
            }
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnComfirm_Click(object sender, EventArgs e)
        {
            newData = txtNewPrice.Text;
        }

        private void txtDeliverFee_KeyPress(object sender, KeyPressEventArgs e)
        {
            char ch = e.KeyChar;
            if (ch == symbol && txtNewPrice.Text.IndexOf(symbol) != -1)
            {
                e.Handled = true;
                return;
            }

            if (!Char.IsDigit(ch) && ch != 8 && ch != symbol )
            {
                e.Handled = true;
            }
        }

        private void digitButton_click(object sender, EventArgs e)
        {
            txtNewPrice.Focus();
            BunifuButton btn = (BunifuButton)sender;
            SendKeys.Send(btn.Text);
        }

        private void btnBack_Click(object sender, EventArgs e)
        {
            txtNewPrice.Focus();
            SendKeys.Send("{BACKSPACE}");
        }

        private void btnNormal_Click(object sender, EventArgs e)
        {
            BunifuButton btn = (BunifuButton) sender;
            if (changePrice)
            {
                txtNewPrice.Text = defultPrice;
            }
            else
            {
                txtNewPrice.Text = "";
            }

        }
    }
}
