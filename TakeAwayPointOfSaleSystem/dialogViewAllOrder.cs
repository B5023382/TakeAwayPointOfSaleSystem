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
    public partial class dialogViewAllOrder : Form
    {
        public int orderNo;
        private string connectionString = Properties.Settings.Default.LocalDatabaseConnectionString;
        // private string connectionString =
        // "Data Source=(LocalDB)\\MSSQLLocalDB;AttachDbFilename=D:\\Homework\\Project\\TakeAwayPointOfSaleSystem\\TakeAwayPointOfSaleSystem\\PointOfSaleLocalDatabase.mdf;Integrated Security=True";

        public dialogViewAllOrder()
        {
            InitializeComponent();
            using (SqlConnection sqlCon = new SqlConnection(connectionString))
            {
                sqlCon.Open();
                SqlDataAdapter getCategory = new SqlDataAdapter("SELECT CO.Id, CO.orderTime, C.phoneNumber, C.customerName, C.houseNo, C.address FROM CustomerOrder CO LEFT JOIN Customer C ON CO.customerId = C.Id", sqlCon);
                DataSet categoryDataSet = new DataSet();
                getCategory.Fill(categoryDataSet);
                sqlCon.Close();
                dgvCategory.DataSource = categoryDataSet.Tables[0];
            }
        }

        private void dgvCategory_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (dgvCategory.SelectedRows.Count > 0)
            {
                using (SqlConnection sqlCon = new SqlConnection(connectionString))
                {
                    sqlCon.Open();

                    SqlCommand getCommand = new SqlCommand("SELECT * FROM OrderItem WHERE orderId = " + (int)dgvCategory.SelectedRows[0].Cells[0].Value, sqlCon);
                    getCommand.CommandType = CommandType.Text;

                    List<string> itemId = new List<string>();
                    using (SqlDataReader reader = getCommand.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            itemId.Add(reader["Id"].ToString());
                        }
                        reader.Close();
                    }

                   foreach(string s in itemId)
                    {
                        SqlCommand getSetDish = new SqlCommand("GetOrderItem", sqlCon);
                        getSetDish.CommandType = CommandType.StoredProcedure;
                        getSetDish.Parameters.AddWithValue("@orderId", s);
                        using (SqlDataReader reader = getSetDish.ExecuteReader())
                        {
                            if(reader.Read())
                            {
                                dgvDish.Rows.Add(reader["dishId"].ToString(), reader["dishName"].ToString(), reader["dishOtherName"].ToString(),
                                    reader["price"].ToString(), reader["qty"].ToString());
                                if(reader["isSet"].ToString() == "1")
                                {
                                    dgvDish.Rows[dgvDish.Rows.Count - 1].DefaultCellStyle.BackColor = Color.Gold;
                                }
                                else if (reader["isSet"].ToString() == "2")
                                {
                                    dgvDish.Rows[dgvDish.Rows.Count - 1].DefaultCellStyle.BackColor = Color.GreenYellow;
                                }
                            }
                            reader.Close();
                        }
                    }
                }

                decimal total = 0;

                foreach (DataGridViewRow row in dgvDish.Rows)
                {
                    total = total + Convert.ToDecimal(row.Cells[3].Value) * Convert.ToDecimal(row.Cells[4].Value);
                }

                lblTotal.Text = "£ " + total.ToString();
            }
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnSelect_Click(object sender, EventArgs e)
        {
            if(dgvCategory.SelectedRows.Count > 0)
            {
                orderNo = (int)dgvCategory.SelectedRows[0].Cells[0].Value;
                this.DialogResult = DialogResult.OK;
            }
            else
            {
                MessageBox.Show("Please select an Order before you alter it", "Error", MessageBoxButtons.OK,
    MessageBoxIcon.Error);
            }
        }
    }
    
}
