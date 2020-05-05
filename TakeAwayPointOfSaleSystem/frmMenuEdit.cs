using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace TakeAwayPointOfSaleSystem
{
    public partial class frmMenuEdit : Form
    {

        public frmMenuEdit()
        {
            InitializeComponent();
            
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
        }

        public void setEditType(int page)
        {
            switch (page)
            {
                case 0:
                    lblTitle.Text = "Edit Menu"; 
                    pagEditType.SetPage(1);
                    break;
                case 1:
                    lblTitle.Text = "Edit Food Category";
                    pagEditType.SetPage(0);
                    break;
                case 2:
                    lblTitle.Text = "Edit Food Common";
                    pagEditType.SetPage(1);
                    break;
                case 3:
                    lblTitle.Text = "Edit Food Common Category";
                    lblCategoryTwo.Text = "";
                    pagEditType.SetPage(0);
                    break;
                case 4:
                    lblTitle.Text = "Edit Set Meal";
                    pagEditType.SetPage(2);
                    break;
            }
        }

        private void btnClearFood_Click(object sender, EventArgs e)
        {
            txtDishNo.Text = "";
            txtDishName.Text = "";
            txtDishOther.Text = "";
            txtDishPrice.Text = "";
        }

        private void btnClearSet_Click(object sender, EventArgs e)
        {
            txtSetDish.Text = "";
            txtSetDishOther.Text = "";
            txtSetName.Text = "";
            txtSetPrice.Text = "";
            txtQTY.Text = "";
        }
    }
}
