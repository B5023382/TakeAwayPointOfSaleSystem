using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Timers;
using System.Windows.Forms;
using Bunifu.UI.WinForms.BunifuButton;
using Timer = System.Windows.Forms.Timer;

namespace TakeAwayPointOfSaleSystem
{
    public partial class FrmMain : Form
    {
        
        //private string connectionString = Properties.Settings.Default.LocalDatabaseConnectionString;
        private string connectionString =
            "Data Source=(LocalDB)\\MSSQLLocalDB;AttachDbFilename=D:\\Homework\\Project\\TakeAwayPointOfSaleSystem\\TakeAwayPointOfSaleSystem\\PointOfSaleLocalDatabase.mdf;Integrated Security=True";
        Timer myTimer = new Timer{Interval = 1000};
        private frmAddress addressForm = new frmAddress();
        private frmMenuEdit menuEdition = new frmMenuEdit();

        public FrmMain(string username, string role)
        {
            InitializeComponent();

            for (int i = 0; i < 20; i++)
            {
                BunifuButton b = new BunifuButton();
                b.Text = "Type " + i;
                b.Size = new Size(110, 50);
                b.Click += common_button_click;
                flpCommonCategory.Controls.Add(b);
            }

            for (int i = 0; i < 20; i++)
            {
                BunifuButton b = new BunifuButton();
                b.Text = "Type " + i;
                b.Size = new Size(200, 90);
                b.Click += dishButton_click;
                flpDishMenu.Controls.Add(b);
            }

            lblUsername.Text = username;
            lblRole.Text = role;
            lblDate.Text = DateTime.Now.Date.ToString("yyyy-M-d dddd");

        }

        private void Form_load(object sender, EventArgs e)
        {
            myTimer.Tick += (o, args) => { lblTime.Text = DateTime.Now.ToString("h:mm:ss tt"); };
            myTimer.Start();
        }

        private void common_button_click(object sender, EventArgs e)
        {

        }

        private void btnAllDish_Click(object sender, EventArgs e)
        {

        }

        private void dishButton_click(object sender, EventArgs e)
        {

        }

        private void btnMinusQTY_Click(object sender, EventArgs e)
        {

        }

        private void btnAddQTY_Click(object sender, EventArgs e)
        {

        }

        private void btnChangePrice_Click(object sender, EventArgs e)
        {
            using (dialogChangePrice d = new dialogChangePrice('.', "Change Price", null))
            {
                if (d.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                {
                    lblTotal.Text = d.newData;
                }
            }
        }

        private void btnSetTime_Click(object sender, EventArgs e)
        {
            using (dialogChangePrice d = new dialogChangePrice(':', "Set Time", "Clear"))
            {
                if (d.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                {
                    lblDeliverTime.Text = d.newData;
                }
            }
        }

        private void btnSearch_Click(object sender, EventArgs e)
        {
            using (dialogChangePrice d = new dialogChangePrice(' ', "Search Dish", "Clear"))
            {
                if (d.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                {
                   
                }
            }
        }

        private void btnCancelOrder_Click(object sender, EventArgs e)
        {

        }

        private void btnViewAll_Click(object sender, EventArgs e)
        {

        }

        private void btnSetMeal_Click(object sender, EventArgs e)
        {

        }

        private void btnDishCommon_Click(object sender, EventArgs e)
        {
            pagMenuPage.SetPage(1);
        }

        private void btnDishLess_Click(object sender, EventArgs e)
        {

        }

        private void btnDishAdd_Click(object sender, EventArgs e)
        {

        }

        private void btnDishNone_Click(object sender, EventArgs e)
        {

        }

        private void btnDishSwap_Click(object sender, EventArgs e)
        {

        }

        private void btnCloseCommon_Click(object sender, EventArgs e)
        {
            pagMenuPage.SetPage(0);
        }

        private void panAddressBar_click(object sender, EventArgs e)
        {
            addressForm.SetCustomerDetail(lblTelphone.Text, lblName.Text, lblHouseNo.Text, lblAddress.Text, lblPostcode.Text, lblDeliverFee.Text, lblDeliverTime.Text);
            Program.SetActiveForm(addressForm);
            Program.ShowForm();
            this.Hide();
        }

        private void btnManagement_Click(object sender, EventArgs e)
        {
            if (lblRole.Text.Equals("admin"))
            {
                var managementForm = new frmManagement(menuEdition);
                Program.SetActiveForm(managementForm);
                Program.ShowForm();
                this.Hide();
            }
            else
            {
                MessageBox.Show("Your don't have management permission, Please change to admin account", "Error", MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
            }
        }

        private void btnPayment_Click(object sender, EventArgs e)
        {
            using (dialogPayment p = new dialogPayment())
            {
                if (p.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                {

                }
            }
        }

        public void setCustomerDetail(string phone, string name, string houseNo, string address, string postCode, string deliverFee, string deliverTime)
        {
            lblTelphone.Text = phone;
            lblName.Text = name;
            lblHouseNo.Text = houseNo;
            lblAddress.Text = address;
            lblPostcode.Text = postCode;
            lblDeliverFee.Text = deliverFee;
            lblDeliverTime.Text = deliverTime;

            lblAddress.Location = new Point(lblHouseNo.Right + 5, lblHouseNo.Location.Y );
        }


    }
}
