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
using Bunifu.UI.WinForms;
using TakeAwayPointOfSaleSystem.Properties;

namespace TakeAwayPointOfSaleSystem
{
    public partial class frmMenuEdit : Form
    {
        //private string connectionString = Properties.Settings.Default.LocalDatabaseConnectionString;
        private string connectionString =
            "Data Source=(LocalDB)\\MSSQLLocalDB;AttachDbFilename=D:\\Homework\\Project\\TakeAwayPointOfSaleSystem\\TakeAwayPointOfSaleSystem\\PointOfSaleLocalDatabase.mdf;Integrated Security=True";

        private gridSetting  gridSet = new gridSetting();
        private int category1 = 0;
        private int category2 = 0;

        private dish food;
        private DataTable dt;
        onScreenKeyboard onKeyboard;
        private BunifuTextBox textBox;

        public frmMenuEdit()
        {
            InitializeComponent();
            onKeyboard = new onScreenKeyboard(btnLeftShift, btnRightShift);
            lblCategoryOne.Text = "";
            lblCategoryTwo.Text = "";
            lblDishName.Text = "";
            lblDishOther.Text = "";
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
            textBox = (BunifuTextBox) sender;
            textBox.SelectAll();
            onKeyboard.setCotrol((Control)sender);
        }

        private void btnBackToManagement_Click(object sender, EventArgs e)
        {
            var managementForm = new frmManagement(this);
            Program.SetActiveForm(managementForm);
            Program.ShowForm();
            this.Hide();
            category1 = 0;
            category2 = 0;
            lblCategoryOne.Text = "";
            lblCategoryTwo.Text = "";
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
                    gridSet.setTable("FoodMenu", "GetFoodTable");
                    gridSet.setProprety("FoodCategory", "AddNewDish", "DeleteDish");
                    dgvFoodCategory.Columns[2].DataPropertyName = "categoryName";
                    dgvFoodCategory.Columns[3].DataPropertyName = "categoryOtherName";
                    dgvFood.Columns[1].DataPropertyName = "foodName";
                    dgvFood.Columns[2].DataPropertyName = "foodOtherName";
                    lblCategoryTwo.Visible = true;
                    pagEditType.SetPage(1);
                    fillGirdOfPage2();
                    break;

                case 1:
                    lblTitle.Text = "Edit Food Category";
                    gridSet.setProprety("FoodCategory", "AddFoodCategory", "DeleteFromFoodCategory");
                    dgvCatogory.Columns[1].DataPropertyName = "categoryName";
                    dgvCatogory.Columns[2].DataPropertyName = "categoryOtherName";
                    pagEditType.SetPage(0);
                    fillGird();
                    break;

                case 2:
                    lblTitle.Text = "Edit Food Common";
                    gridSet.setTable("FoodCommon", "GetCommonTable");
                    gridSet.setProprety("CommonCategory", "AddNewCommon", "DeleteCommon");
                    dgvFoodCategory.Columns[2].DataPropertyName = "commonCategory";
                    dgvFoodCategory.Columns[3].DataPropertyName = "commonCategoryOther";
                    dgvFood.Columns[1].DataPropertyName = "commonName";
                    dgvFood.Columns[2].DataPropertyName = "commonOtherName";
                    lblCategoryTwo.Visible = false;
                    pagEditType.SetPage(1);
                    fillGirdOfPage2();
                    break;

                case 3:
                    lblTitle.Text = "Edit Food Common Category";
                    gridSet.setProprety("CommonCategory","AddCommonCategory", "DeleteFromCommonCategory");
                    dgvCatogory.Columns[1].DataPropertyName = "commonCategory";
                    dgvCatogory.Columns[2].DataPropertyName = "commonCategoryOther";
                    pagEditType.SetPage(0);
                    fillGird();
                    break;

                case 4:
                    lblTitle.Text = "Edit Set Meal";
                    pagEditType.SetPage(2);
                    fill_SetTable();
                    break;
            }


        }

        private void btnClearFood_Click(object sender, EventArgs e)
        {
            clearPage();
        }

        private void btnClearSet_Click(object sender, EventArgs e)
        {
            clearSetEnter();
        }

        private void clearSetEnter()
        {
            dgvSetMenu.ClearSelection();
            lblDishName.Text = "";
            lblDishOther.Text = "";
            txtSetName.Text = "";
            txtSetOtherName.Text = "";
            txtSetPrice.Text = "0.00";
            txtSetDishPrice.Text = "0.00";
            txtQTY.Text = "0";
            txtSetNo.Text = "";
            if(dt != null)
            {
                dt.Clear();
                dgvSetDish.DataSource = dt;
            }
            else
            {
                dgvSetDish.Rows.Clear();
            }
        }

        private void btnGetDish_Click(object sender, EventArgs e)
        {
            using (dialogSelectSetMealDish d = new dialogSelectSetMealDish() )
            {
                if (d.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                {
                    food = d.getDish();
                    lblDishName.Text = food.getName();
                    lblDishOther.Text = food.getOtherName();
                }
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
                SqlDataAdapter getCategory = new SqlDataAdapter("SELECT * FROM " + gridSet.getCategoryTable(), sqlCon);
                DataSet categoryDataSet = new DataSet();
                getCategory.Fill(categoryDataSet);
                sqlCon.Close();
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
                SqlDataAdapter getCategory = new SqlDataAdapter("SELECT * FROM " + gridSet.getCategoryTable(), sqlCon);
                DataSet categoryDataSet = new DataSet();
                getCategory.Fill(categoryDataSet);

                SqlCommand getCommand = new SqlCommand(gridSet.getProcedureGet(), sqlCon);
                getCommand.CommandType = CommandType.StoredProcedure;
                SqlDataAdapter getMenu = new SqlDataAdapter();
                getMenu.SelectCommand = getCommand;
                DataSet menuDataSet = new DataSet();
                getMenu.Fill(menuDataSet);

                sqlCon.Close();

                dgvFoodCategory.DataSource = categoryDataSet.Tables[0];
                dgvFoodCategory.Columns["buttonColor"].Visible = false;

                dgvFood.DataSource = menuDataSet.Tables[0];
                dgvFood.Columns["category"].Visible = false;
                dgvFood.Columns["categoryName"].Visible = false;

                if (gridSet.getTable().Equals("FoodMenu"))
                {
                    dgvFood.Columns["category2"].Visible = false;
                    dgvFood.Columns["category2Name"].Visible = false;
                }

                dgvFoodCategory.ClearSelection();
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

            txtCategoryName.Text = "";
            txtOtherName.Text = "";
            dgvCatogory.ClearSelection();
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
                txtCategoryName.Text = "";
                txtOtherName.Text = "";
                dgvCatogory.ClearSelection();
            }
        }

        private void txtDishPrice_KeyPress(object sender, KeyPressEventArgs e)
        {
            char ch = e.KeyChar;
            if (ch == 46 && textBox.Text.IndexOf('.') != -1)
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

        private void dgvFoodCateGory_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (string.IsNullOrWhiteSpace((string) dgvFoodCategory.SelectedRows[0].Cells[0].Value))
            {
                if (string.IsNullOrWhiteSpace(lblCategoryOne.Text))
                {
                    dgvFoodCategory.SelectedRows[0].Cells[0].Value = "Yes";
                    dgvFoodCategory.SelectedRows[0].DefaultCellStyle.BackColor = Color.Chartreuse;
                    lblCategoryOne.Text = (string)dgvFoodCategory.SelectedRows[0].Cells[2].Value;
                    lblCategoryOne.Location = new Point(lblcate.Right + 20, lblcate.Location.Y);
                    category1 = (int) dgvFoodCategory.SelectedRows[0].Cells[1].Value;
                }
                else if(gridSet.getCategoryTable().Equals("FoodCategory") && string.IsNullOrWhiteSpace(lblCategoryTwo.Text))
                {
                    dgvFoodCategory.SelectedRows[0].Cells[0].Value = "Yes";
                    dgvFoodCategory.SelectedRows[0].DefaultCellStyle.BackColor = Color.Chartreuse;
                    lblCategoryTwo.Text = (string)dgvFoodCategory.SelectedRows[0].Cells[2].Value;
                    lblCategoryTwo.Location = new Point(lblCategoryOne.Right + 20, lblCategoryOne.Location.Y);
                    category2 = (int)dgvFoodCategory.SelectedRows[0].Cells[1].Value;
                }

            }
            else
            {
                dgvFoodCategory.SelectedRows[0].Cells[0].Value = "";
                dgvFoodCategory.SelectedRows[0].DefaultCellStyle.BackColor = Color.Empty;
                if (category1 == (int) dgvFoodCategory.SelectedRows[0].Cells[1].Value)
                {
                    if (category2 != 0)
                    {
                        category1 = category2;
                        lblCategoryOne.Text = lblCategoryTwo.Text;
                    }
                    else
                    {
                        category1 = 0;
                        lblCategoryOne.Text = "";
                    }
                }

                category2 = 0;
                lblCategoryTwo.Text = "";

            }
            
        }

        private void frmMenuEdit_Shown(object sender, EventArgs e)
        {
            category1 = 0;
            category2 = 0;
            lblCategoryOne.Text = "";
            lblCategoryTwo.Text = "";
        }

        private void clearPage()
        {
            dgvFoodCategory.ClearSelection();
            dgvFood.ClearSelection();
            txtDishNo.Text = "";
            txtDishName.Text = "";
            txtDishOther.Text = "";
            txtDishPrice.Text = "0.00";
            lblCategoryOne.Text = "";
            lblCategoryTwo.Text = "";
            category2 = 0;
            category1 = 0;
            clearCategoryTable();
        }

        private void clearCategoryTable()
        {
            foreach (DataGridViewRow row in dgvFoodCategory.Rows)
            {
                row.Cells[0].Value = "";
                row.DefaultCellStyle.BackColor = Color.Empty;
            }
        }

        private void btnDeleteFood_Click(object sender, EventArgs e)
        {
            if (dgvFood.SelectedRows.Count > 0)
            {
                DataGridViewRow selectedRow = dgvFood.SelectedRows[0];

                using (SqlConnection sqlCon = new SqlConnection(connectionString))
                {
                    sqlCon.Open();
                    SqlCommand addCustomer = new SqlCommand(gridSet.getProcedureDelete(), sqlCon);
                    addCustomer.CommandType = CommandType.StoredProcedure;
                    addCustomer.Parameters.AddWithValue("@id", selectedRow.Cells[0].Value);
                    addCustomer.ExecuteNonQuery();
                    sqlCon.Close();
                    fillGirdOfPage2();
                }
            }
            clearPage();
        }

        private void dgvFood_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (dgvFood.SelectedRows.Count > 0)
            {
                clearCategoryTable();
                DataGridViewRow selectedRow = dgvFood.SelectedRows[0];

                txtDishNo.Text = selectedRow.Cells[0].Value.ToString();
                txtDishName.Text = selectedRow.Cells[1].Value.ToString();
                txtDishOther.Text = selectedRow.Cells[2].Value.ToString();
                txtDishPrice.Text = selectedRow.Cells[3].Value.ToString();
                category1 = (int)selectedRow.Cells[4].Value;
                lblCategoryOne.Text = selectedRow.Cells[5].Value.ToString();
                
                if (gridSet.getTable().Equals("FoodMenu") && !string.IsNullOrWhiteSpace(selectedRow.Cells[6].Value.ToString()) )
                {
                    category2 = (int)selectedRow.Cells[6].Value;
                    lblCategoryTwo.Text = selectedRow.Cells[7].Value.ToString();
                }
                else
                {
                    category2 = 0;
                    lblCategoryTwo.Text = "";
                }

                foreach (DataGridViewRow row in dgvFoodCategory.Rows)
                {
                    if ((int) row.Cells[1].Value == category1 || (int) row.Cells[1].Value == category2)
                    {
                        row.Cells[0].Value = "Yes";
                        row.DefaultCellStyle.BackColor = Color.Chartreuse;
                    }
                }
            }
        }

        private void btnSaveFood_Click(object sender, EventArgs e)
        {
            if ((string.IsNullOrWhiteSpace(txtDishNo.Text))|| string.IsNullOrWhiteSpace(txtDishName.Text)|| string.IsNullOrWhiteSpace(txtDishPrice.Text) || category1 == 0)
            {
                MessageBox.Show("Your didn't complete all necessary section", "Error", MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
            }
            else
            {
                using (SqlConnection sqlCon = new SqlConnection(connectionString))
                {
                    sqlCon.Open();
                    SqlCommand addCustomer = new SqlCommand(gridSet.getProcedureName(), sqlCon);
                    addCustomer.CommandType = CommandType.StoredProcedure;
                    addCustomer.Parameters.AddWithValue("@foodName", txtDishName.Text.Trim());
                    addCustomer.Parameters.AddWithValue("@foodOtherName", txtDishOther.Text.Trim());
                    addCustomer.Parameters.AddWithValue("@price", txtDishPrice.Text.Trim());
                    addCustomer.Parameters.AddWithValue("@id", txtDishNo.Text);
                    addCustomer.Parameters.AddWithValue("@category1", category1);
                    if (gridSet.getTable().Equals("FoodMenu") )
                    {
                        addCustomer.Parameters.AddWithValue("@category2", category2);
                    }

                    addCustomer.ExecuteNonQuery();
                    sqlCon.Close();
                    fillGirdOfPage2();
                }
            }
            clearPage();
        }

        private void btnAddDish_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(lblDishName.Text))
            {
                MessageBox.Show("Please select a dish use Get Dish button", "Error", MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
            }
            else if (Convert.ToInt32(txtQTY.Text) < 1)
            {
                MessageBox.Show("Please set Quantity of dish", "Error", MessageBoxButtons.OK,
                     MessageBoxIcon.Error);
            }
            else
            {
                bool repeate = false;
                foreach (DataGridViewRow row in dgvSetDish.Rows)
                {
                    if ((int) row.Cells[0].Value == food.getId())
                    {
                        row.Cells[3].Value = txtSetDishPrice.Text;
                        row.Cells[4].Value = txtQTY.Text;
                        repeate = true;
                        break;
                    }
                }

                if (!repeate)
                {
                    if(dgvSetDish.DataSource == null)
                    {
                        dgvSetDish.Rows.Add(food.getId(), lblDishName.Text, lblDishOther.Text, txtSetDishPrice.Text, txtQTY.Text);
                    }
                    else
                    {
                        DataRow r = dt.NewRow();
                        r["dishNo"] = food.getId();
                        r["foodName"] = lblDishName.Text;
                        r["foodOtherName"] = lblDishOther.Text;
                        r["price"] = txtSetDishPrice.Text;
                        r["QTY"] = txtQTY.Text;
                        dt.Rows.Add(r);
                    }

                }
            }
        }

        private void btnDeleteSetDish_Click(object sender, EventArgs e)
        {
            if (dgvSetDish.SelectedRows.Count > 0)
            {
                if (!string.IsNullOrEmpty(txtSetNo.Text))
                {
                    using (SqlConnection sqlCon = new SqlConnection(connectionString))
                    {
                        sqlCon.Open();
                        SqlCommand addCustomer = new SqlCommand("DeleteSetDish", sqlCon);
                        addCustomer.CommandType = CommandType.StoredProcedure;
                        addCustomer.Parameters.AddWithValue("@setId", txtSetNo.Text);
                        addCustomer.Parameters.AddWithValue("@dishId", dgvSetDish.SelectedRows[0].Cells[0].Value);
                        addCustomer.ExecuteNonQuery();
                        sqlCon.Close();
                    }
                }
                dgvSetDish.Rows.RemoveAt(dgvSetDish.SelectedRows[0].Index);
            }
        }

        private void txtQTY_KeyPress(object sender, KeyPressEventArgs e)
        {
            char ch = e.KeyChar;
            if (!Char.IsDigit(ch) && ch != 8)
            {
                e.Handled = true;
            }
        }

        private void dgvSetMenu_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (dgvSetMenu.SelectedRows.Count > 0)
            {
                txtSetName.Text = dgvSetMenu.SelectedRows[0].Cells[1].Value.ToString();
                txtSetOtherName.Text = dgvSetMenu.SelectedRows[0].Cells[2].Value.ToString();
                txtSetPrice.Text = dgvSetMenu.SelectedRows[0].Cells[3].Value.ToString();
                txtSetNo.Text = dgvSetMenu.SelectedRows[0].Cells[0].Value.ToString();
            }
        }

        private void btnSaveSet_Click(object sender, EventArgs e)
        {
            if ((string.IsNullOrWhiteSpace(txtSetNo.Text)) || string.IsNullOrWhiteSpace(txtSetName.Text) || string.IsNullOrWhiteSpace(txtSetPrice.Text) || dgvSetDish.RowCount < 1)
            {
                MessageBox.Show("Your didn't complete all necessary section", "Error", MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
            }
            else
            {
                using (SqlConnection sqlCon = new SqlConnection(connectionString))
                {
                    sqlCon.Open();
                    SqlCommand addCustomer = new SqlCommand("AddSetMeal", sqlCon);
                    addCustomer.CommandType = CommandType.StoredProcedure;
                    addCustomer.Parameters.AddWithValue("@setMealName", txtSetName.Text.Trim());
                    addCustomer.Parameters.AddWithValue("@setMealOther", txtSetOtherName.Text.Trim());
                    addCustomer.Parameters.AddWithValue("@price", txtSetPrice.Text.Trim());
                    addCustomer.Parameters.AddWithValue("@id", txtSetNo.Text);

                    addCustomer.ExecuteNonQuery();

                    for (int row = 0; row < dgvSetDish.RowCount; row++)
                    {
                        SqlCommand addSetDish = new SqlCommand("AddSetDish", sqlCon);
                        addSetDish.CommandType = CommandType.StoredProcedure;
                        addSetDish.Parameters.AddWithValue("@dishNo", (int)dgvSetDish.Rows[row].Cells[0].Value);
                        addSetDish.Parameters.AddWithValue("@qty", Convert.ToInt32(dgvSetDish.Rows[row].Cells[4].Value));
                        addSetDish.Parameters.AddWithValue("@price", Convert.ToDecimal(dgvSetDish.Rows[row].Cells[3].Value));
                        addSetDish.Parameters.AddWithValue("@setId", txtSetNo.Text);
                        addSetDish.ExecuteNonQuery();
                    }
                    sqlCon.Close();
                    fill_SetTable();
                }
            }
            clearSetEnter();
        }

        private void btnDeleteSet_Click(object sender, EventArgs e)
        {
            if (dgvSetMenu.SelectedRows.Count > 0)
            {
                using (SqlConnection sqlCon = new SqlConnection(connectionString))
                {
                    sqlCon.Open();
                    SqlCommand addCustomer = new SqlCommand("DeleteSetMeal", sqlCon);
                    addCustomer.CommandType = CommandType.StoredProcedure;
                    addCustomer.Parameters.AddWithValue("@id", dgvSetMenu.SelectedRows[0].Cells[0].Value);
                    addCustomer.ExecuteNonQuery();
                    sqlCon.Close();
                    fill_SetTable();
                }

                clearSetEnter();
            }
        }

        private void fill_SetTable()
        {
            using (SqlConnection sqlCon = new SqlConnection(connectionString))
            {
                sqlCon.Open();
                SqlDataAdapter getCategory = new SqlDataAdapter("SELECT * FROM SetMeal", sqlCon);
                DataSet categoryDataSet = new DataSet();
                getCategory.Fill(categoryDataSet);
                sqlCon.Close();
                dgvSetMenu.DataSource = categoryDataSet.Tables[0];
            }
        }

        private void txtSetNo_TextChange(object sender, EventArgs e)
        {
            using (SqlConnection sqlCon = new SqlConnection(connectionString))
            {
                sqlCon.Open();
                SqlCommand getSetDish = new SqlCommand("GetSetDish", sqlCon);
                getSetDish.CommandType = CommandType.StoredProcedure;
                getSetDish.Parameters.AddWithValue("@setId", txtSetNo.Text);
                SqlDataAdapter getDish = new SqlDataAdapter();
                getDish.SelectCommand = getSetDish;
                DataSet menuDataSet = new DataSet();
                getDish.Fill(menuDataSet);
                sqlCon.Close();
                dt = menuDataSet.Tables[0];
                dgvSetDish.DataSource = dt;
            }
            dgvSetMenu.ClearSelection();
        }
    }
}
