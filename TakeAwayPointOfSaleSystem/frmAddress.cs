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

namespace TakeAwayPointOfSaleSystem
{
    public partial class frmAddress : Form
    {
        onScreenKeyboard onKeyboard;

        //private string connectionString = Properties.Settings.Default.LocalDatabaseConnectionString;
        private string connectionString =
            "Data Source=(LocalDB)\\MSSQLLocalDB;AttachDbFilename=D:\\Homework\\Project\\TakeAwayPointOfSaleSystem\\TakeAwayPointOfSaleSystem\\LocalDatabase.mdf;Integrated Security=True";
        public frmAddress()
        {
            InitializeComponent();
            onKeyboard = new onScreenKeyboard(btnLeftShift, btnRightShift);
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
            onKeyboard.setCotrol((Control)sender);
        }

        private void txtDeliverFee_KeyPress(object sender, KeyPressEventArgs e)
        {
            char ch = e.KeyChar;
            if (ch == 46 && txtDeliverFee.Text.IndexOf('.') != -1)
            {
                e.Handled = true;
                return;
            }

            if (!Char.IsDigit(ch) && ch != 8 && ch != 46)
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

        private void txtDeliverTime_KeyPress(object sender, KeyPressEventArgs e)
        {
            char ch = e.KeyChar;
            if (ch == 58 && txtDeliverFee.Text.IndexOf(':') != -1)
            {
                e.Handled = true;
                return;
            }

            if (!Char.IsDigit(ch) && ch != 8 && ch != 58)
            {
                e.Handled = true;
            }
        }

        private void btnSaveOrder_Click(object sender, EventArgs e)
        {
            //lblAddress.Text = txtHouseNo.Text + " " + txtAddress.Text;
            //lblDeliverFee.Text = txtDeliverFee.Text;
            //lblDeliverTime.Text = txtDeliverTime.Text;
            //lblName.Text = txtName.Text;
            //lblNote.Text = txtNote.Text;
            //lblPostcode.Text = txtPostcode.Text;
            //lblTelphone.Text = txtTelephone.Text;

            using (SqlConnection sqlCon = new SqlConnection(connectionString))
            {
                sqlCon.Open();
                SqlCommand addCustomer = new SqlCommand("AddCustomer", sqlCon);
                addCustomer.CommandType = CommandType.StoredProcedure;
                addCustomer.Parameters.AddWithValue("@telephone", txtTelephone.Text.Trim());
                addCustomer.Parameters.AddWithValue("@name", txtName.Text.Trim());
                addCustomer.Parameters.AddWithValue("@houseNo", txtHouseNo.Text.Trim());
                addCustomer.Parameters.AddWithValue("@address", txtAddress.Text.Trim());
                addCustomer.Parameters.AddWithValue("@postcode", txtPostcode.Text.Trim());
                addCustomer.Parameters.AddWithValue("@deliverFee", txtDeliverFee.Text.Trim());
                addCustomer.Parameters.AddWithValue("@id", 0);
                addCustomer.ExecuteNonQuery();
            }
        }

        private void btnCloseAddress_Click(object sender, EventArgs e)
        {

        }
    }
}
