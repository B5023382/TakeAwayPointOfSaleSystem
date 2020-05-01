using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Bunifu.UI.WinForms.BunifuButton;

namespace TakeAwayPointOfSaleSystem
{
    public partial class FrmMain : Form
    {
        onScreenKeyboard onKeyboard;
        //private string connectionString = Properties.Settings.Default.LocalDatabaseConnectionString;
        private string connectionString =
            "Data Source=(LocalDB)\\MSSQLLocalDB;AttachDbFilename=D:\\Homework\\Project\\TakeAwayPointOfSaleSystem\\TakeAwayPointOfSaleSystem\\LocalDatabase.mdf;Integrated Security=True";

 
        public FrmMain()
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

            onKeyboard = new onScreenKeyboard(btnLeftShift, btnRightShift);
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

        }

        private void btnCancelOrder_Click(object sender, EventArgs e)
        {

        }

        private void btnViewAll_Click(object sender, EventArgs e)
        {

        }

        private void btnSearch_Click(object sender, EventArgs e)
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

        private void common_button_click(object sender, EventArgs e)
        {

        }

        private void btnCloseCommon_Click(object sender, EventArgs e)
        {
            pagMenuPage.SetPage(0);
        }

        private void panAddressBar_click(object sender, EventArgs e)
        {
            pageMain.SetPage(1);
        }

        private void keyboard_click(object sender, EventArgs e)
        {
            onKeyboard.keyboard_click(sender, e);
        }

        private void btnSpace_Click(object sender, EventArgs e)
        {
            onKeyboard.press_space();
        }

        private void btnEnter_Click(object sender, EventArgs e)
        {
            onKeyboard.press_enter();
        }

        private void btnBackspace_Click(object sender, EventArgs e)
        {
            onKeyboard.press_backspace();
        }

        private void btnCap_Click(object sender, EventArgs e)
        {
            onKeyboard.press_capsLock(sender, e);
        }

        private void btnLeftShift_Click(object sender, EventArgs e)
        {
            onKeyboard.press_shift();
        }

        private void btnRightShift_Click(object sender, EventArgs e)
        {
            onKeyboard.press_shift();
        }

        private void btnClear_Click(object sender, EventArgs e)
        {
            onKeyboard.press_clear();
        }

        private void textbox_select(object sender, EventArgs e)
        {
            onKeyboard.setCotrol((Control) sender);
        }

        private void btnSaveOrder_Click(object sender, EventArgs e)
        {
            lblAddress.Text = txtHouseNo.Text + " " + txtAddress.Text;
            lblDeliverFee.Text = txtDeliverFee.Text;
            lblDeliverTime.Text = txtDeliverTime.Text;
            lblName.Text = txtName.Text;
            lblNote.Text = txtNote.Text;
            lblPostcode.Text = txtPostcode.Text;
            lblTelphone.Text = txtTelephone.Text;

            using (SqlConnection sqlCon = new SqlConnection(connectionString))
            {
                sqlCon.Open();
                SqlCommand addCustomer = new SqlCommand("AddCustomer", sqlCon);
                addCustomer.CommandType = CommandType.StoredProcedure;
                addCustomer.Parameters.AddWithValue("@telephone", txtTelephone.Text.Trim());
                addCustomer.Parameters.AddWithValue("@name", txtName.Text.Trim());
                addCustomer.Parameters.AddWithValue("@houseNo", txtHouseNo.Text.Trim());
                addCustomer.Parameters.AddWithValue("@address", txtAddress.Text.Trim());
                addCustomer.Parameters.AddWithValue("@postcode", txtPostcode.Text.Trim().ToUpper());
                addCustomer.Parameters.AddWithValue("@id", 0);
                addCustomer.ExecuteNonQuery();
                customerData.Customer.AcceptChanges();
                dgvCustomer.Update();

            }
            pageMain.SetPage(0);
        }

        private void btnCloseAddress_Click(object sender, EventArgs e)
        {
            pageMain.SetPage(0);
        }

        private void btnManagement_Click(object sender, EventArgs e)
        {

        }

        private void FrmMain_Load(object sender, EventArgs e)
        {
            // TODO: This line of code loads data into the 'customerData.Customer' table. You can move, or remove it, as needed.
            this.customerTableAdapter.Fill(this.customerData.Customer);

        }

        private void txtDeliverFee_KeyPress(object sender, KeyPressEventArgs e)
        {
            char ch = e.KeyChar;
            if(ch == 46 && txtDeliverFee.Text.IndexOf('.') != -1)
            {
                e.Handled = true;
                return;
            }

            if(!Char.IsDigit(ch) && ch != 8 && ch != 46)
            {
                e.Handled = true;
            }
        }

        private void txtTelephone_KeyPress(object sender, KeyPressEventArgs e)
        {
            char ch = e.KeyChar;
            if (!Char.IsDigit(ch))
            {
                e.Handled = true;
            }
        }
    }
}
