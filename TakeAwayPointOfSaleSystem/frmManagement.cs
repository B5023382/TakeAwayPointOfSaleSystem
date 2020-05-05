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
    public partial class frmManagement : Form
    {
        private frmMenuEdit menuEdit;
        public frmManagement(frmMenuEdit menuEdit)
        {
            InitializeComponent();
            this.menuEdit = menuEdit;
            Program.SetActiveForm(menuEdit);
        }

        private void btnBack_Click(object sender, EventArgs e)
        {
            Program.ShowMainForm();
            this.Close();
        }

        private void btnEditMenu_Click(object sender, EventArgs e)
        {
            menuEdit.setEditType(0);
            Program.ShowForm();
            this.Close();
        }

        private void EditFoodCategory_Click(object sender, EventArgs e)
        {
            menuEdit.setEditType(1);
            Program.ShowForm();
            this.Close();
        }

        private void btnEditSetMeal_Click(object sender, EventArgs e)
        {
            menuEdit.setEditType(4);
            Program.ShowForm();
            this.Close();
        }

        private void btnEditDishCommon_Click(object sender, EventArgs e)
        {
            menuEdit.setEditType(3);
            Program.ShowForm();
            this.Close();
        }

        private void btnEditCommonCategory_Click(object sender, EventArgs e)
        {
            menuEdit.setEditType(2);
            Program.ShowForm();
            this.Close();
        }
    }
}
