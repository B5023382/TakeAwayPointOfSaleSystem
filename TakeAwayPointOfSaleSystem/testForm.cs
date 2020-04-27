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
    public partial class testForm : Form
    {
        public testForm()
        {
            InitializeComponent();
            for (int i = 0; i < 10; i++)
            {
                BunifuButton b = new BunifuButton();
                b.Text = "B" + i;
                b.Size = new Size(110,50);
                b.Click += common_button;
                flowLayoutPanel1.Controls.Add(b);
            }
        }

        private void common_button(object sender, EventArgs e)
        {
            var currentButton = sender as BunifuButton;
            MessageBox.Show("this button is " + currentButton.Text, "Error", MessageBoxButtons.OK,
                MessageBoxIcon.Error);
        }
    }
}
