using Bunifu.UI.WinForms;
using Bunifu.UI.WinForms.BunifuButton;
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
    public partial class dialogPayment : Form
    {
        private decimal total;
        private decimal discount = 0;
        private bool deliver = false;
        private BunifuTextBox textBox;
        public dialogPayment(string deliverFee, string total, string deliverTime)
        {
            InitializeComponent();
            this.total = Convert.ToDecimal(total);
            if (!string.IsNullOrEmpty(deliverFee))
            {
                btnDeliver.IdleBorderThickness = 4;
                this.deliver = true;
            }
            else
            {
                btnCollection.IdleBorderThickness = 4;
            }
            txtDeliverFee.Text = deliverFee;
            txtDeliverTime.Text = deliverTime;
            lblTotal.Text = calculateTotal().ToString();
        }

        private decimal calculateTotal()
        {
            decimal totalToPay = 0;

            decimal otherFee = 0;
            if (!string.IsNullOrEmpty(txtOtherFee.Text))
            {
                otherFee = Convert.ToDecimal(txtOtherFee.Text);
            }
            if (deliver)
            {
                totalToPay = total + Convert.ToDecimal(txtDeliverFee.Text) - discount + otherFee;
            }
            else
            {
                totalToPay = total - discount + otherFee;
            }

            return totalToPay;
        }

        private void btnShop_Click(object sender, EventArgs e)
        {
            deliver = false;
            btnShop.IdleBorderThickness = 4;

            btnDeliver.IdleBorderThickness = 1;

            btnCollection.IdleBorderThickness = 1;
            lblTotal.Text = calculateTotal().ToString();
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnDeliver_Click(object sender, EventArgs e)
        {
            deliver = true;
            if (string.IsNullOrEmpty(txtDeliverFee.Text))
            {
                txtDeliverFee.Text = "1";
            }
            btnShop.IdleBorderThickness = 1;

            btnDeliver.IdleBorderThickness = 4;

            btnCollection.IdleBorderThickness = 1;

            lblTotal.Text = calculateTotal().ToString();
        }

        private void btnCollection_Click(object sender, EventArgs e)
        {
            deliver = false;
            btnShop.IdleBorderThickness = 1;

            btnDeliver.IdleBorderThickness = 1;

            btnCollection.IdleBorderThickness = 4;

            lblTotal.Text = calculateTotal().ToString();
        }

        private void txtDeliverTime_Click(object sender, EventArgs e)
        {
            using (dialogChangePrice d = new dialogChangePrice(':', "Set Time", "Clear", txtDeliverTime.Text))
            {
                if (d.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                {
                    txtDeliverTime.Text = d.newData;
                }
            }
        }

        private void textbox_select(object sender, EventArgs e)
        {
            textBox = (BunifuTextBox)sender;
            textBox.SelectAll();
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

        private void btnDisCountP_Click(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(txtDiscount.Text))
            {
                if (deliver)
                {
                    discount = (total + Convert.ToDecimal(txtDeliverFee.Text)) * Convert.ToDecimal(txtDiscount.Text) / 100;
                }
                else
                {
                    discount = total * Convert.ToDecimal(txtDiscount.Text) / 100;
                }
            }

            lblTotal.Text = calculateTotal().ToString();
        }

        private void btnDiscountN_Click(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(txtDiscount.Text))
            {
                discount =Convert.ToDecimal(txtDiscount.Text);
            }
            lblTotal.Text = calculateTotal().ToString();
        }

        private void btnNormal_Click(object sender, EventArgs e)
        {
            txtNewPrice.Text = "";
        }

        private void digitButton_click(object sender, EventArgs e)
        {
            if(textBox != null)
            {
                textBox.Focus();
                BunifuButton btn = (BunifuButton)sender;
                SendKeys.Send(btn.Text);
            }
        }

        private void btnBack_Click(object sender, EventArgs e)
        {
            if (textBox != null)
            {
                textBox.Focus();
                SendKeys.Send("{BACKSPACE}");
            }
        }

        private void btnReset_Click(object sender, EventArgs e)
        {
            lblPaid.Text = "0.00";
            lblBalance.Text = "0.00";
            lblChange.Text = "0.00";
            discount = 0;
            txtDiscount.Text = "";
            txtOtherFee.Text = "";
            txtNewPrice.Text = "";

            lblTotal.Text = calculateTotal().ToString();
        }

        private void btn5Pound_Click(object sender, EventArgs e)
        {
            txtNewPrice.Text = "5";
        }

        private void btn10Pound_Click(object sender, EventArgs e)
        {
            txtNewPrice.Text = "10";
        }

        private void btn20Pound_Click(object sender, EventArgs e)
        {
            txtNewPrice.Text = "20";
        }

        private void btn50Pound_Click(object sender, EventArgs e)
        {
            txtNewPrice.Text = "50";
        }

        private void btnPay_Click(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(txtNewPrice.Text))
            {
                decimal totalPirce = Convert.ToDecimal(lblTotal.Text);
                decimal? paid = Convert.ToDecimal(lblPaid.Text);
                decimal? cash = Convert.ToDecimal(txtNewPrice.Text);
                if (paid == null || paid == 0)
                {
                    paid = cash;
                    lblPaid.Text = cash.ToString();
                }
                else
                {
                    paid = paid + cash;
                }

                decimal change = (decimal)paid - totalPirce;

                if (change < 0)
                {
                    lblBalance.Text = change.ToString();
                    lblChange.Text = "0.00";
                }
                else
                {
                    lblBalance.Text = "0.00";
                    lblChange.Text = change.ToString();
                }
            }

        }

        private void txtDeliverFee_Click(object sender, EventArgs e)
        {
            using (dialogChangePrice d = new dialogChangePrice('.', "Change Delivery Fee", null, txtDeliverFee.Text))
            {
                if (d.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                {
                    txtDeliverFee.Text = d.newData;
                }
            }

            if (!string.IsNullOrEmpty(txtDeliverFee.Text))
            {
                lblTotal.Text = calculateTotal().ToString();
            }
        }
    }
}
