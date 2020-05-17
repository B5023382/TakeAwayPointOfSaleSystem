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
        BunifuButton delete = new BunifuButton();

        public FrmMain(string username, string role)
        {
            InitializeComponent();

            using (SqlConnection sqlCon = new SqlConnection(connectionString))
            {
                sqlCon.Open();
                SqlCommand addCustomer = new SqlCommand("SELECT categoryName FROM FoodCategory", sqlCon);
                addCustomer.CommandType = CommandType.Text;

                using (SqlDataReader reader = addCustomer.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        BunifuButton b = new BunifuButton();
                        b.Text = reader["categoryName"].ToString();
                        b.Size = new Size(110, 50);
                        b.Click += dishButton_click;
                        flpCommonCategory.Controls.Add(b);
                    }
                }

                SqlCommand getCommonCategory = new SqlCommand("SELECT commonCategory FROM CommonCategory", sqlCon);
                getCommonCategory.CommandType = CommandType.Text;

                using (SqlDataReader reader = getCommonCategory.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        BunifuButton b = new BunifuButton();
                        b.Text = reader["commonCategory"].ToString();
                        b.Size = new Size(110, 50);
                        b.Click += common_button_click;
                        flpDishMenu.Controls.Add(b);
                    }
                }

                sqlCon.Close();
            }

            delete.Text = "Delete";
            delete.Size = new Size(110, 50);
            delete.Click += deleteCommon_click;
            flpDishMenu.Controls.Add(delete);

            lblUsername.Text = username;
            lblRole.Text = role;
            lblDate.Text = DateTime.Now.Date.ToString("yyyy-M-d dddd");

            lblHouseNo.Text = "";
            lblAddress.Text = "";
            lblDeliverTime.Text = "";
            lblTelphone.Text = "";
            lblDeliverFee.Text = "1";
            lblPostcode.Text = "";
            lblName.Text = "";
            lblTotal.Text = "0.00";
            lblNote.Text = "";
        }

        private void deleteCommon_click(object sender, EventArgs e)
        {
            if (dgvOrder.SelectedRows.Count > 0)
            {
                string commonId = (string) dgvOrder.SelectedRows[0].Cells[5].Value;
                string commons = (string) dgvOrder.SelectedRows[0].Cells[3].Value;
                dgvOrder.SelectedRows[0].Cells[3].Value = "";
                string[] commonList = commons.Split(',');
                for (int i = 0; i < commonList.Length - 1; i++)
                {
                    dgvOrder.SelectedRows[0].Cells[3].Value = (string)dgvOrder.SelectedRows[0].Cells[3].Value + commonList[i];
                }
            }
        }

        private void Form_load(object sender, EventArgs e)
        {
            myTimer.Tick += (o, args) => { lblTime.Text = DateTime.Now.ToString("h:mm:ss tt"); };
            myTimer.Start();
        }

        private void common_button_click(object sender, EventArgs e)
        {
            BunifuButton b = (BunifuButton)sender;
            using (SqlConnection sqlCon = new SqlConnection(connectionString))
            {
                sqlCon.Open();
                SqlDataAdapter getCategory =
                    new SqlDataAdapter("SELECT Id, commonName, price FROM FoodCommon WHERE category = '" + b.Text +"'", sqlCon);
                DataSet categoryDataSet = new DataSet();
                getCategory.Fill(categoryDataSet);
                sqlCon.Close();
                dgvCommon.DataSource = categoryDataSet.Tables[0];
            }
        }

        private void btnAllDish_Click(object sender, EventArgs e)
        {
            using (SqlConnection sqlCon = new SqlConnection(connectionString))
            {
                sqlCon.Open();
                SqlDataAdapter getCategory = new SqlDataAdapter("SELECT Id, foodName, price FROM FoodMenu", sqlCon);
                DataSet categoryDataSet = new DataSet();
                getCategory.Fill(categoryDataSet);
                sqlCon.Close();
                dgvFood.DataSource = categoryDataSet.Tables[0];
            }
        }

        private void dishButton_click(object sender, EventArgs e)
        {
            BunifuButton b = (BunifuButton) sender;
            using (SqlConnection sqlCon = new SqlConnection(connectionString))
            {
                sqlCon.Open();
                SqlDataAdapter getCategory = 
                    new SqlDataAdapter("SELECT Id, foodName, price FROM FoodMenu WHERE category1 = " + b.Text + " OR category2 = " + b.Text, sqlCon);
                DataSet categoryDataSet = new DataSet();
                getCategory.Fill(categoryDataSet);
                sqlCon.Close();
                dgvFood.DataSource = categoryDataSet.Tables[0];
            }
        }

        private void btnMinusQTY_Click(object sender, EventArgs e)
        {
            if (dgvOrder.SelectedRows.Count > 0)
            {
               if((int)dgvOrder.SelectedRows[0].Cells[1].Value - 1 < 1)
               {
                    dgvOrder.Rows.RemoveAt(dgvOrder.SelectedRows[0].Index);
               }
               else
               {
                   dgvOrder.SelectedRows[0].Cells[1].Value = (int) dgvOrder.SelectedRows[0].Cells[1].Value - 1;
               }
            }
        }

        private void btnAddQTY_Click(object sender, EventArgs e)
        {
            if (dgvOrder.SelectedRows.Count > 0)
            {
                dgvOrder.SelectedRows[0].Cells[1].Value = (int)dgvOrder.SelectedRows[0].Cells[1].Value + 1;
            }
        }

        private void btnChangePrice_Click(object sender, EventArgs e)
        {
            if (dgvOrder.SelectedRows.Count > 0)
            {
                using (dialogChangePrice d = new dialogChangePrice('.', "Change Price", null))
                {
                    if (d.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                    {
                        lblTotal.Text = d.newData;
                    }
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
            lblHouseNo.Text = "";
            lblAddress.Text = "";
            lblDeliverTime.Text = "";
            lblTelphone.Text = "";
            lblDeliverFee.Text = "1";
            lblPostcode.Text = "";
            lblName.Text = "";
            lblTotal.Text = "0.00";
            lblNote.Text = "";

            dgvOrder.Rows.Clear();
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
            if (dgvCommon.SelectedRows.Count > 0)
            {
                dgvOrder.SelectedRows[0].Cells[3].Value = (string) dgvOrder.SelectedRows[0].Cells[3].Value
                                                          + "," + "Less " +
                                                          (string) dgvCommon.SelectedRows[0].Cells[0].Value;
            }
        }

        private void btnDishAdd_Click(object sender, EventArgs e)
        {
            if (dgvCommon.SelectedRows.Count > 0)
            {
                dgvOrder.SelectedRows[0].Cells[3].Value = (string)dgvOrder.SelectedRows[0].Cells[3].Value
                                                          + "," + "Add " +
                                                          (string)dgvCommon.SelectedRows[0].Cells[0].Value;
                dgvOrder.SelectedRows[0].Cells[4].Value = (float)dgvOrder.SelectedRows[0].Cells[4].Value + 
                                                          (float)dgvCommon.SelectedRows[0].Cells[1].Value;
            }
        }

        private void btnDishNone_Click(object sender, EventArgs e)
        {
            if (dgvCommon.SelectedRows.Count > 0)
            {
                dgvOrder.SelectedRows[0].Cells[3].Value = (string)dgvOrder.SelectedRows[0].Cells[3].Value
                                                          + "," + "No " +
                                                          (string)dgvCommon.SelectedRows[0].Cells[0].Value;
            }
        }

        private void btnDishSwap_Click(object sender, EventArgs e)
        {
            if (dgvCommon.SelectedRows.Count > 0)
            {
                dgvOrder.SelectedRows[0].Cells[3].Value = (string)dgvOrder.SelectedRows[0].Cells[3].Value
                                                          + "," + "Swap to" +
                                                          (string)dgvCommon.SelectedRows[0].Cells[0].Value;
                dgvOrder.SelectedRows[0].Cells[4].Value = (float)dgvOrder.SelectedRows[0].Cells[4].Value +
                                                          (float)dgvCommon.SelectedRows[0].Cells[1].Value;
            }
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
            if (lblRole.Text.Trim() == "admin")
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
