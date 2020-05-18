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
                        b.Size = new Size(200, 90);
                        b.Click += dishButton_click;
                        flpDishMenu.Controls.Add(b);
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
                        flpCommonCategory.Controls.Add(b);
                    }
                }

                sqlCon.Close();
            }

            delete.Text = "Delete";
            delete.Size = new Size(110, 50);
            delete.Click += deleteCommon_click;
            flpCommonCategory.Controls.Add(delete);

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

            dgvFood.DefaultCellStyle.WrapMode = DataGridViewTriState.True;
        }

        private void deleteCommon_click(object sender, EventArgs e)
        {
            if (dgvOrder.SelectedRows.Count > 0)
            {
                string commonId = (string) dgvOrder.SelectedRows[0].Cells[6].Value;
                string commons = (string) dgvOrder.SelectedRows[0].Cells[4].Value;
                dgvOrder.SelectedRows[0].Cells[4].Value = "";

                string[] commonList = commons.Split('/');
                string[] commonPrice = commonId.Split('/');

                if(commonList.Length > 0 && (commonList[commonList.Length - 1].Contains("Add") || commonList[commonList.Length - 1].Contains("Swap"))){

                    dgvOrder.SelectedRows[0].Cells[6].Value = "";

                    decimal single = Convert.ToDecimal(dgvOrder.SelectedRows[0].Cells[5].Value) / Convert.ToDecimal(dgvOrder.SelectedRows[0].Cells[1].Value);
                    dgvOrder.SelectedRows[0].Cells[5].Value = (single - Convert.ToDecimal(commonPrice[commonPrice.Length - 1]))
                        * Convert.ToDecimal(dgvOrder.SelectedRows[0].Cells[1].Value);

                    for (int i = 0; i < commonPrice.Length - 1; i++)
                    {
                        if (!string.IsNullOrEmpty(commonPrice[i]))
                        {
                            dgvOrder.SelectedRows[0].Cells[6].Value = (string)dgvOrder.SelectedRows[0].Cells[6].Value + "/" + commonPrice[i];
                        }
                    }
                }

                for (int i = 0; i < commonList.Length - 1; i++)
                {
                    if (!string.IsNullOrEmpty(commonList[i]))
                    {
                        dgvOrder.SelectedRows[0].Cells[4].Value = (string)dgvOrder.SelectedRows[0].Cells[4].Value + "/" + commonList[i] ;
                    }
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
                    new SqlDataAdapter("SELECT f.Id, f.commonName, f.commonOtherName, f.price FROM FoodCommon f " +
                    "LEFT JOIN CommonCategory c ON f.category = c.Id WHERE c.commonCategory = '" + b.Text +"'", sqlCon);
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
                SqlDataAdapter getCategory = new SqlDataAdapter("SELECT Id, foodName, foodOtherName, price FROM FoodMenu", sqlCon);
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
                    new SqlDataAdapter("SELECT f.Id, f.foodName, f.foodOtherName, f.price FROM FoodMenu f " +
                    "LEFT JOIN FoodCategory c ON f.category1 = c.Id " +
                    "LEFT JOIN FoodCategory c2 ON f.category2 = c2.Id" +
                    " WHERE c.categoryName = '" + b.Text + "' OR c2.categoryName = '" + b.Text + "'", sqlCon);
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
               if(Convert.ToDecimal(dgvOrder.SelectedRows[0].Cells[1].Value) - 1 < 1)
               {
                    //if(dgvOrder.Rows[dgvOrder.SelectedRows[0].Index].DefaultCellStyle.BackColor != Color.Empty)
                    //{
                    //    dgvOrder.Rows[dgvOrder.SelectedRows[0].Index].DefaultCellStyle.BackColor = Color.Empty;
                    //}
                    dgvOrder.Rows.RemoveAt(dgvOrder.SelectedRows[0].Index);
               }
               else
               {
                    decimal qty = Convert.ToDecimal(dgvOrder.SelectedRows[0].Cells[1].Value);
                    decimal dishPrice = Convert.ToDecimal(dgvOrder.SelectedRows[0].Cells[5].Value) / qty;
                    dgvOrder.SelectedRows[0].Cells[1].Value = qty - 1;
                    dgvOrder.SelectedRows[0].Cells[5].Value = dishPrice * (qty - 1);
               }
                lblTotal.Text = "£ " + calculateTotal().ToString();
            }
        }

        private void btnAddQTY_Click(object sender, EventArgs e)
        {
            if (dgvOrder.SelectedRows.Count > 0 && dgvOrder.SelectedRows[0].DefaultCellStyle.BackColor != Color.GreenYellow)
            {
                decimal temp = Convert.ToDecimal(dgvOrder.SelectedRows[0].Cells[5].Value) / Convert.ToDecimal(dgvOrder.SelectedRows[0].Cells[1].Value);
                decimal qty = Convert.ToDecimal(dgvOrder.SelectedRows[0].Cells[1].Value) + 1;
                dgvOrder.SelectedRows[0].Cells[1].Value = qty;
                dgvOrder.SelectedRows[0].Cells[5].Value = temp * qty;
                lblTotal.Text = "£ " + calculateTotal().ToString();
            }
        }

        private void btnChangePrice_Click(object sender, EventArgs e)
        {
            if (dgvOrder.SelectedRows.Count > 0)
            {
                decimal price = Convert.ToDecimal(dgvOrder.SelectedRows[0].Cells[5].Value) / Convert.ToDecimal(dgvOrder.SelectedRows[0].Cells[1].Value);
                using (dialogChangePrice d = new dialogChangePrice('.', "Change Price", null, price.ToString()))
                {
                    if (d.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                    {
                        dgvOrder.SelectedRows[0].Cells[5].Value = Convert.ToDecimal(d.newData) * Convert.ToDecimal(dgvOrder.SelectedRows[0].Cells[1].Value);
                        lblTotal.Text = "£ " + calculateTotal().ToString();
                    }
                }
            }
        }

        private void btnSetTime_Click(object sender, EventArgs e)
        {
            using (dialogChangePrice d = new dialogChangePrice(':', "Set Time", "Clear", lblDeliverTime.Text))
            {
                if (d.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                {
                    lblDeliverTime.Text = d.newData;
                }
            }
        }

        private void btnSearch_Click(object sender, EventArgs e)
        {
            using (dialogChangePrice d = new dialogChangePrice(' ', "Search Dish", "Clear", ""))
            {
                if (d.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                {
                    using (SqlConnection sqlCon = new SqlConnection(connectionString))
                    {
                        sqlCon.Open();
                        SqlDataAdapter getCategory = new SqlDataAdapter("SELECT Id, foodName, foodOtherName, price FROM FoodMenu WHERE Id LIKE '" + d.newData + "%'", sqlCon);
                        DataSet categoryDataSet = new DataSet();
                        getCategory.Fill(categoryDataSet);
                        sqlCon.Close();
                        dgvFood.DataSource = categoryDataSet.Tables[0];
                    }
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
            using (dialogGetSetMeal d = new dialogGetSetMeal())
            {
                if (d.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                {
                    dish set = d.getDish();

                    dgvOrder.Rows.Add(set.getId(), 1, set.getName(), set.getOtherName(), "", set.getPrice());
                    dgvOrder.Rows[dgvOrder.Rows.Count - 1].DefaultCellStyle.BackColor = Color.GreenYellow;

                    using (SqlConnection sqlCon = new SqlConnection(connectionString))
                    {
                        sqlCon.Open();
                        SqlCommand getSetDish = new SqlCommand("GetSetDish", sqlCon);
                        getSetDish.CommandType = CommandType.StoredProcedure;
                        getSetDish.Parameters.AddWithValue("@setId", set.getId());
                        using (SqlDataReader reader = getSetDish.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                dgvOrder.Rows.Add(reader["dishNo"].ToString(), reader["QTY"].ToString(), reader["foodName"].ToString(), 
                                    reader["foodOtherName"].ToString(), "", reader["price"].ToString());
                                dgvOrder.Rows[dgvOrder.Rows.Count - 1].DefaultCellStyle.BackColor = Color.GreenYellow;
                            }
                        }
                        sqlCon.Close();
                    }
                    lblTotal.Text = "£ " + calculateTotal();
                }
            }
        }

        private void btnDishCommon_Click(object sender, EventArgs e)
        {
            pagMenuPage.SetPage(1);
        }

        private void btnDishLess_Click(object sender, EventArgs e)
        {
            if (dgvCommon.SelectedRows.Count > 0 && dgvOrder.SelectedRows.Count > 0)
            {
                dgvOrder.SelectedRows[0].Cells[4].Value = (string) dgvOrder.SelectedRows[0].Cells[4].Value
                                                          + "/" + "Less " +
                                                          (string) dgvCommon.SelectedRows[0].Cells["sideName"].Value;
            }
        }

        private void btnDishAdd_Click(object sender, EventArgs e)
        {
            if (dgvCommon.SelectedRows.Count > 0 && dgvOrder.SelectedRows.Count > 0)
            {
                dgvOrder.SelectedRows[0].Cells[4].Value = (string)dgvOrder.SelectedRows[0].Cells[4].Value
                                                          + "/" + "Add " +
                                                          (string)dgvCommon.SelectedRows[0].Cells["sideName"].Value;

                    dgvOrder.SelectedRows[0].Cells[6].Value = (string)dgvOrder.SelectedRows[0].Cells[6].Value
                          + "/" + dgvCommon.SelectedRows[0].Cells["Price"].Value.ToString();
                

                decimal single = Convert.ToDecimal(dgvOrder.SelectedRows[0].Cells[5].Value) / Convert.ToDecimal(dgvOrder.SelectedRows[0].Cells[1].Value);
                dgvOrder.SelectedRows[0].Cells[5].Value = (single + Convert.ToDecimal(dgvCommon.SelectedRows[0].Cells["Price"].Value))
                    * Convert.ToDecimal(dgvOrder.SelectedRows[0].Cells[1].Value);

                lblTotal.Text = "£ " + calculateTotal();
            }
        }

        private void btnDishNone_Click(object sender, EventArgs e)
        {
            if (dgvCommon.SelectedRows.Count > 0 && dgvOrder.SelectedRows.Count > 0)
            {
                dgvOrder.SelectedRows[0].Cells[4].Value = (string)dgvOrder.SelectedRows[0].Cells[4].Value
                                                          + "/" + "No " +
                                                          (string)dgvCommon.SelectedRows[0].Cells["sideName"].Value;
            }
        }

        private void btnDishSwap_Click(object sender, EventArgs e)
        {
            if (dgvCommon.SelectedRows.Count > 0 && dgvOrder.SelectedRows.Count > 0)
            {
                dgvOrder.SelectedRows[0].Cells[4].Value = (string)dgvOrder.SelectedRows[0].Cells[4].Value
                                                          + "/" + "Swap to" +
                                                          (string)dgvCommon.SelectedRows[0].Cells["sideName"].Value;

                dgvOrder.SelectedRows[0].Cells[6].Value = (string)dgvOrder.SelectedRows[0].Cells[6].Value
                          + "/" + dgvCommon.SelectedRows[0].Cells["Price"].Value.ToString();

                decimal single = Convert.ToDecimal(dgvOrder.SelectedRows[0].Cells[5].Value) / Convert.ToDecimal(dgvOrder.SelectedRows[0].Cells[1].Value);
                dgvOrder.SelectedRows[0].Cells[5].Value = (single + Convert.ToDecimal(dgvCommon.SelectedRows[0].Cells["Price"].Value))
                    * Convert.ToDecimal(dgvOrder.SelectedRows[0].Cells[1].Value);

                lblTotal.Text = "£ " + calculateTotal();
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

        private void dgvFood_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            
            if(dgvFood.SelectedRows.Count > 0 && pagMenuPage.PageIndex == 0)
            {
                bool repeat = false;
                foreach(DataGridViewRow row in dgvOrder.Rows)
                {
                    if(row.Cells[0].Value.ToString() == dgvFood.SelectedRows[0].Cells[0].Value.ToString() && string.IsNullOrEmpty(row.Cells[4].Value.ToString()) && row.DefaultCellStyle.BackColor != Color.GreenYellow)
                    {
                        row.Cells[1].Value = Convert.ToDecimal(row.Cells[1].Value) + 1;
                        row.Cells[5].Value = Convert.ToDecimal(row.Cells[1].Value) * Convert.ToDecimal(dgvFood.SelectedRows[0].Cells[3].Value);
                        repeat = true;
                        break;
                    }
                }
                if (!repeat)
                {
                    dgvOrder.Rows.Add(dgvFood.SelectedRows[0].Cells[0].Value, 1, dgvFood.SelectedRows[0].Cells[1].Value,
                        dgvFood.SelectedRows[0].Cells[2].Value, "", dgvFood.SelectedRows[0].Cells[3].Value);
                }
            }

            lblTotal.Text = "£ " + calculateTotal().ToString();
        }

        private decimal calculateTotal()
        {
            decimal total = 0;

            foreach(DataGridViewRow row in dgvOrder.Rows)
            {
                total = total + Convert.ToDecimal(row.Cells[5].Value);
            }
            return total + Convert.ToDecimal(lblDeliverFee.Text);
        }
    }
}
