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
    public partial class frmMenuEdit : Form
    {
        //private string connectionString = Properties.Settings.Default.LocalDatabaseConnectionString;
        private string connectionString =
            "Data Source=(LocalDB)\\MSSQLLocalDB;AttachDbFilename=D:\\Homework\\Project\\TakeAwayPointOfSaleSystem\\TakeAwayPointOfSaleSystem\\PointOfSaleLocalDatabase.mdf;Integrated Security=True";

        private gridSetting  gridSet = new gridSetting();
        onScreenKeyboard onKeyboard;
        public frmMenuEdit()
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

        private void btnBackToManagement_Click(object sender, EventArgs e)
        {
            var managementForm = new frmManagement(this);
            Program.SetActiveForm(managementForm);
            Program.ShowForm();
            this.Hide();
        }

        private void BtnClearCategory_Click(object sender, EventArgs e)
        {
            txtCategoryName.Text = "";
            txtOtherName.Text = "";
            dgvCatogory.ClearSelection();
        }

        public void setEditType(int page)
        {
            switch (page)
            {
                case 0:
                    lblTitle.Text = "Edit Menu";
                    gridSet.setCategoryTable("FoodCategory");
                    dgvFoodCateGory.Columns[2].DataPropertyName = "categoryName";
                    dgvFoodCateGory.Columns[3].DataPropertyName = "categoryOtherName";
                    pagEditType.SetPage(1);
                    fillGirdOfPage2();
                    break;

                case 1:
                    lblTitle.Text = "Edit Food Category";
                    gridSet.setCategoryTable("FoodCategory");
                    gridSet.setProprety("AddFoodCategory", "DeleteFromFoodCategory");
                    dgvCatogory.Columns[1].DataPropertyName = "categoryName";
                    dgvCatogory.Columns[2].DataPropertyName = "categoryOtherName";
                    pagEditType.SetPage(0);
                    fillGird();
                    break;

                case 2:
                    lblTitle.Text = "Edit Food Common";
                    gridSet.setCategoryTable("CommonCategory");
                    dgvFoodCateGory.Columns[2].DataPropertyName = "commonCategory";
                    dgvFoodCateGory.Columns[3].DataPropertyName = "commonCategoryOther";
                    lblCategoryTwo.Visible = false;
                    pagEditType.SetPage(1);
                    fillGirdOfPage2();
                    break;

                case 3:
                    lblTitle.Text = "Edit Food Common Category";
                    gridSet.setCategoryTable("CommonCategory");
                    gridSet.setProprety("AddCommonCategory", "DeleteFromCommonCategory");
                    dgvCatogory.Columns[1].DataPropertyName = "commonCategory";
                    dgvCatogory.Columns[2].DataPropertyName = "commonCategoryOther";
                    pagEditType.SetPage(0);
                    fillGird();
                    break;

                case 4:
                    lblTitle.Text = "Edit Set Meal";
                    pagEditType.SetPage(2);
                    break;
            }


        }

        private void btnClearFood_Click(object sender, EventArgs e)
        {
            dgvFoodCateGory.ClearSelection();
            txtDishNo.Text = "";
            txtDishName.Text = "";
            txtDishOther.Text = "";
            txtDishPrice.Text = "";
        }

        private void btnClearSet_Click(object sender, EventArgs e)
        {
            dgvCatogory.ClearSelection();
            txtSetDish.Text = "";
            txtSetDishOther.Text = "";
            txtSetName.Text = "";
            txtSetPrice.Text = "";
            txtQTY.Text = "";
        }

        private void btnGetDish_Click(object sender, EventArgs e)
        {
            using (dialogSelectSetMealDish d = new dialogSelectSetMealDish() )
            {
                
            }
        }

        private void btnPickColor_Click(object sender, EventArgs e)
        {
            using (ColorDialog c = new ColorDialog())
            {
                if (c.ShowDialog() == DialogResult.OK)
                {
                    btnPickColor.BackColor = c.Color;
                }
            }
        }

        public void fillGird()
        {
            using (SqlConnection sqlCon = new SqlConnection(connectionString))
            {
                sqlCon.Open();
                SqlDataAdapter getCategory = new SqlDataAdapter("SELECT * FROM " + gridSet.getTable(), sqlCon);
                DataSet categoryDataSet = new DataSet();
                getCategory.Fill(categoryDataSet);
                sqlCon.Close();
                dgvCatogory.Columns[0].DataPropertyName = "Id";
                dgvCatogory.DataSource = categoryDataSet.Tables[0];
                foreach (DataGridViewRow row in dgvCatogory.Rows)
                {
                    row.DefaultCellStyle.BackColor = Color.FromArgb((int)row.Cells[3].Value);
                }
                dgvCatogory.Columns["buttonColor"].Visible = false;
                dgvCatogory.ClearSelection();
            }
        }

        public void fillGirdOfPage2()
        {
            using (SqlConnection sqlCon = new SqlConnection(connectionString))
            {
                sqlCon.Open();
                SqlDataAdapter getCategory = new SqlDataAdapter("SELECT * FROM " + gridSet.getTable(), sqlCon);
                DataSet categoryDataSet = new DataSet();
                getCategory.Fill(categoryDataSet);
                sqlCon.Close();
                dgvFoodCateGory.Columns[1].DataPropertyName = "Id";
                dgvFoodCateGory.DataSource = categoryDataSet.Tables[0];
                dgvFoodCateGory.Columns["buttonColor"].Visible = false;
                dgvFoodCateGory.ClearSelection();
            }
        }

        private void btnSaveCategory_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtCategoryName.Text))
            {
                MessageBox.Show("Your need enter categoryName", "Error", MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
            }
            else
            {
                using (SqlConnection sqlCon = new SqlConnection(connectionString))
                {
                    sqlCon.Open();
                    SqlCommand addCustomer = new SqlCommand(gridSet.getProcedureName(), sqlCon);
                    addCustomer.CommandType = CommandType.StoredProcedure;
                    addCustomer.Parameters.AddWithValue("@categoryName", txtCategoryName.Text.Trim());
                    addCustomer.Parameters.AddWithValue("@categoryOtherName", txtOtherName.Text.Trim());
                    addCustomer.Parameters.AddWithValue("@buttonColor", btnPickColor.BackColor.ToArgb());
                    addCustomer.Parameters.AddWithValue("@id", 0);
                    addCustomer.ExecuteNonQuery();
                    sqlCon.Close();
                    fillGird();
                }
            }
        }

        private void btnDeleteCatogory_Click(object sender, EventArgs e)
        {
            if (dgvCatogory.SelectedRows.Count > 0)
            {
                DataGridViewRow selectedRow = dgvCatogory.SelectedRows[0];

                using (SqlConnection sqlCon = new SqlConnection(connectionString))
                {
                    sqlCon.Open();
                    SqlCommand addCustomer = new SqlCommand(gridSet.getProcedureDelete(), sqlCon);
                    addCustomer.CommandType = CommandType.StoredProcedure;
                    addCustomer.Parameters.AddWithValue("@id", selectedRow.Cells[0].Value);
                    addCustomer.ExecuteNonQuery();
                    sqlCon.Close();
                    fillGird();
                }
            }
        }

        private void txtDishPrice_KeyPress(object sender, KeyPressEventArgs e)
        {
            char ch = e.KeyChar;
            if (ch == 46 && txtDishPrice.Text.IndexOf('.') != -1)
            {
                e.Handled = true;
                return;
            }

            if (!Char.IsDigit(ch) && ch != 8 && ch != 46)
            {
                e.Handled = true;
            }
        }

        private void dgvCatogory_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (dgvCatogory.SelectedRows.Count > 0)
            {
                DataGridViewRow selectedRow = dgvCatogory.SelectedRows[0];

                txtCategoryName.Text = selectedRow.Cells[1].Value.ToString();
                txtOtherName.Text = selectedRow.Cells[2].Value.ToString();
                btnPickColor.BackColor = Color.FromArgb((int)selectedRow.Cells[3].Value);
            }
        }
    }
}
