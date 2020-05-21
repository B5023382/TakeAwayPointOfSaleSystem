using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.SqlClient;
using System.Diagnostics;
using System.IO;

namespace TakeAwayPointOfSaleSystem
{
    public partial class frmLogin : Form
    {

        private string connectionString;



        private string productKey = "ABCDEF";

        public frmLogin()
        {
            InitializeComponent();
            
            dockLogin.SubscribeControlToDragEvents(pagLogin);
            dockLogin.SubscribeControlToDragEvents(tabLogin);
            dockLogin.SubscribeControlToDragEvents(tabRegister);

            string workingDriectory = Environment.CurrentDirectory;
            string projectDrictory = Directory.GetParent(workingDriectory).Parent.FullName;
            connectionString = "Data Source=(LocalDB)\\MSSQLLocalDB;AttachDbFilename=" + projectDrictory + "\\PointOfSaleLocalDatabase.mdf;Integrated Security=True";
        }


        private void btnRegister_Click(object sender, EventArgs e)
        {
            cleanInput();
            pagLogin.SetPage(1);
        }

        private void btnBack_Click(object sender, EventArgs e)
        {
            cleanInput();
            pagLogin.SetPage(0);
        }

        private void btnInitalizeAccount_Click(object sender, EventArgs e)
        {
            if (txtProductKey.Text.Trim().Equals(productKey))
            {
                using (SqlConnection sqlCon = new SqlConnection(connectionString))
                {
                    sqlCon.Open();

                    string comText = "SELECT * FROM SoftwareUser WHERE Username = '" + txtNewUserName.Text.Trim() + "'";
                    SqlCommand checkUsername = new SqlCommand(comText, sqlCon);

                    SqlDataReader sqlRead = checkUsername.ExecuteReader();

                    if (sqlRead.HasRows)
                    {
                        MessageBox.Show("This Username has been used, Please enter another Username", "Error", MessageBoxButtons.OK,
                            MessageBoxIcon.Error);
                    }
                    else
                    {
                        string role;

                        if (ckbIsAdmin.Checked)
                        {
                            role = "admin";
                        }
                        else
                        {
                            role = "normal";
                        }

                        sqlRead.Close();
                        SqlCommand sqlCommand = new SqlCommand("AddUser", sqlCon);
                        sqlCommand.CommandType = CommandType.StoredProcedure;
                        sqlCommand.Parameters.AddWithValue("@username", txtNewUserName.Text.Trim());
                        sqlCommand.Parameters.AddWithValue("@password", txtNewPassword.Text.Trim());
                        sqlCommand.Parameters.AddWithValue("@role", role);
                        sqlCommand.ExecuteNonQuery();

                        MessageBox.Show("Your account has create Successfullly. Now you can log in system with your account", "Congrates", MessageBoxButtons.OK,
                            MessageBoxIcon.Information);

                        cleanInput();
                        pagLogin.SetPage(0);
                    }

                    sqlCon.Close();
                }
            }
            else
            {
                MessageBox.Show("Your product key is incorrect", "Error", MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
            }
        }

        private void btnLogin_Click(object sender, EventArgs e)
        {
            using (SqlConnection sqlCon = new SqlConnection(connectionString))
            {
                sqlCon.Open();
                SqlCommand sqlCommand = new SqlCommand("CheckUser", sqlCon);
                sqlCommand.CommandType = CommandType.StoredProcedure;
                sqlCommand.Parameters.AddWithValue("@username", txtUsername.Text.Trim());
                sqlCommand.Parameters.AddWithValue("@password", txtPassword.Text.Trim());

                SqlDataReader dr = sqlCommand.ExecuteReader();
                if (dr.HasRows && dr.Read())
                {

                    string role = dr.GetValue(3).ToString();
                    var mainForm = new FrmMain(txtUsername.Text.Trim(), role);
                    Program.SetMainForm(mainForm);
                    Program.ShowForm();
                    foreach (var process in Process.GetProcessesByName("TabTip"))
                    {
                        process.Kill();
                    }
                    this.Close();
                }
                else
                {
                    MessageBox.Show("Either your username or password is incorrect", "Error", MessageBoxButtons.OK,
                        MessageBoxIcon.Error);
                }

                sqlCon.Close();
            }
        }

        private void cleanInput()
        {
            txtPassword.Text = "";
            txtNewPassword.Text = "";
            txtUsername.Text = "";
            txtNewUserName.Text = "";
            txtProductKey.Text = "";
        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            Environment.Exit(0);
        }

        private void txtUserName_selected(object sender, EventArgs e)
        {
            showKeyboard();
        }

        private void txtPassword_Selected(object sender, EventArgs e)
        {
            showKeyboard();
        }

        private void txtNewUserName_Selected(object sender, EventArgs e)
        {
            showKeyboard();
        }

        private void txtNewPassword_Selected(object sender, EventArgs e)
        {
            showKeyboard();
        }

        private void txtProductKey_Selected(object sender, EventArgs e)
        {
            showKeyboard();
        }

        private void showKeyboard()
        {
            string touchKeyboardPath = @"C:\Program Files\Common Files\Microsoft Shared\Ink\TabTip.exe";
            Process.Start(touchKeyboardPath);
        }
    }
}
