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
        private DataTable dt;

        //private string connectionString = Properties.Settings.Default.LocalDatabaseConnectionString;
        private string connectionString =
            "Data Source=(LocalDB)\\MSSQLLocalDB;AttachDbFilename=D:\\Homework\\Project\\TakeAwayPointOfSaleSystem\\TakeAwayPointOfSaleSystem\\PointOfSaleLocalDatabase.mdf;Integrated Security=True";
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
            if (!Char.IsDigit(ch) && ch != 8)
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
            Program.mainPage.setCustomerDetail(txtTelephone.Text, 
                txtName.Text, txtHouseNo.Text, txtAddress.Text, txtPostcode.Text, txtDeliverFee.Text, txtDeliverTime.Text);

            if (!string.IsNullOrWhiteSpace(txtTelephone.Text))
            {
                using (SqlConnection sqlCon = new SqlConnection(connectionString))
                {
                    sqlCon.Open();
                    SqlCommand addCustomer = new SqlCommand("AddCustomer", sqlCon);
                    addCustomer.CommandType = CommandType.StoredProcedure;
                    addCustomer.Parameters.AddWithValue("@phone", txtTelephone.Text.Trim());
                    addCustomer.Parameters.AddWithValue("@name", txtName.Text.Trim());
                    addCustomer.Parameters.AddWithValue("@houseNo", txtHouseNo.Text.Trim());
                    addCustomer.Parameters.AddWithValue("@address", txtAddress.Text.Trim());
                    addCustomer.Parameters.AddWithValue("@postcode", txtPostcode.Text.Trim().ToUpper());
                    addCustomer.Parameters.AddWithValue("@deliverFee", txtDeliverFee.Text.Trim());
                    addCustomer.ExecuteNonQuery();
                    sqlCon.Close();
                }
            }

            if (dt != null)
            {
                dt.Clear();
                dgvCustomer.DataSource = dt;
            }
            
            Program.ShowMainForm();
            this.Hide();
        }

        private void btnCloseAddress_Click(object sender, EventArgs e)
        {
            Program.ShowMainForm();
            this.Hide();
        }

        public void SetCustomerDetail(string phone, string name, string houseNo, string address, string postCode, string deliverFee, string deliverTime)
        {
            txtTelephone.Text = phone;
            txtName.Text = name;
            txtHouseNo.Text = houseNo;
            txtAddress.Text = address;
            txtPostcode.Text = postCode;
            txtDeliverFee.Text = deliverFee;
            txtDeliverTime.Text = deliverTime;
        }

        private void btnSearchByPhone_Click(object sender, EventArgs e)
        {
            if (!string.IsNullOrWhiteSpace(txtTelephone.Text))
            {
                using (SqlConnection sqlCon = new SqlConnection(connectionString))
                {
                    sqlCon.Open();
                    SqlDataAdapter getCustomer = new SqlDataAdapter("SELECT * FROM Customer WHERE phoneNumber = " + txtTelephone.Text, sqlCon);
                    //SqlDataAdapter getCustomer = new SqlDataAdapter("SELECT * FROM Customer", sqlCon);
                    DataSet customerDetail = new DataSet();
                    getCustomer.Fill(customerDetail);
                    sqlCon.Close();
                    dt = customerDetail.Tables[0];
                    dgvCustomer.DataSource = dt;
                }
            }
        }

        private void dgvCustomer_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (dgvCustomer.SelectedRows.Count > 0 )
            {
                DataGridViewRow selectedRow = dgvCustomer.SelectedRows[0];

                txtName.Text = selectedRow.Cells["customerName"].Value.ToString();
                txtTelephone.Text = selectedRow.Cells["phoneNumber"].Value.ToString();
                txtHouseNo.Text = selectedRow.Cells["houseNo"].Value.ToString();
                txtAddress.Text = selectedRow.Cells["address"].Value.ToString();
                txtPostcode.Text = selectedRow.Cells["postcode"].Value.ToString();
                txtDeliverFee.Text = selectedRow.Cells["deliverFee"].Value.ToString();
            }
        }
    }
}
