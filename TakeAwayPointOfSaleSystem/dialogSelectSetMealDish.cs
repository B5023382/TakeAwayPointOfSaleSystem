using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace TakeAwayPointOfSaleSystem
{
    public partial class dialogSelectSetMealDish : Form
    {
        private dish d;
        //private string connectionString = Properties.Settings.Default.LocalDatabaseConnectionString;
        private string connectionString;
          
        public dialogSelectSetMealDish()
        {
            string workingDriectory = Environment.CurrentDirectory;
            string projectDrictory = Directory.GetParent(workingDriectory).Parent.FullName;
            connectionString = "Data Source=(LocalDB)\\MSSQLLocalDB;AttachDbFilename=" + projectDrictory + "\\PointOfSaleLocalDatabase.mdf;Integrated Security=True";

            InitializeComponent();
            using (SqlConnection sqlCon = new SqlConnection(connectionString))
            {
                sqlCon.Open();
                SqlDataAdapter getCategory = new SqlDataAdapter("SELECT * FROM FoodCategory", sqlCon);
                DataSet categoryDataSet = new DataSet();
                getCategory.Fill(categoryDataSet);
                sqlCon.Close();
                dgvCategory.DataSource = categoryDataSet.Tables[0];
                dgvCategory.Columns["buttonColor"].Visible = false;
                dgvCategory.Columns["Id"].Visible = false;
            }
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnSelect_Click(object sender, EventArgs e)
        {
            if (dgvDish.SelectedRows.Count > 0)
            {
                DataGridViewRow row = dgvDish.SelectedRows[0];
                d = new dish((int)row.Cells[0].Value, (string)row.Cells[1].Value, (string)row.Cells[2].Value, "");
                this.DialogResult = DialogResult.OK;
            }
            else
            {
                MessageBox.Show("Please select one dish from grid on right", "Error", MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
            }
        }

        private void dgvCategory_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (dgvCategory.SelectedRows.Count > 0)
            {
                using (SqlConnection sqlCon = new SqlConnection(connectionString))
                {
                    sqlCon.Open();

                    SqlCommand getCommand = new SqlCommand("GetDishByCategory", sqlCon);
                    getCommand.CommandType = CommandType.StoredProcedure;
                    getCommand.Parameters.AddWithValue("@categoryId", (int)dgvCategory.SelectedRows[0].Cells[0].Value);
                    SqlDataAdapter getMenu = new SqlDataAdapter();
                    getMenu.SelectCommand = getCommand;
                    DataSet menuDataSet = new DataSet();
                    getMenu.Fill(menuDataSet);

                    sqlCon.Close();
                    dgvDish.DataSource = menuDataSet.Tables[0];
                }
            }
        }

        public dish getDish()
        {
            return d;
        }
    }
}
