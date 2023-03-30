using iTextSharp.text.pdf;
using iTextSharp.text;
using Microsoft.Win32;
using OfficeOpenXml;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace App_tien_dien
{
    /// <summary>
    /// Interaction logic for FormLoginAdmin.xaml
    /// </summary>
    public partial class FormLoginAdmin : Window
    {

        public FormLoginAdmin()
        {
            InitializeComponent();
            WindowStartupLocation = System.Windows.WindowStartupLocation.CenterScreen;
        }
        private void btnlogin_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(inputuser.Text) || string.IsNullOrWhiteSpace(inputpassword.Password))
            {
                MessageBox.Show("Tên đăng nhập hoặc mật khẩu không được để trống");
                return;
            }

            try
            {
                string strConnectDB = @"Data Source=DESKTOP-173K518;Initial Catalog=BTL_tien_dien;User ID=sa;Password=admin";
                SqlConnection conn = new SqlConnection(strConnectDB);
                conn.Open();
                String sqlSelect = "SELECT username, password FROM tablelogin_admin WHERE username = @user AND password = @password";
                SqlCommand cmd = new SqlCommand(sqlSelect, conn);
                cmd.Parameters.AddWithValue("@user", inputuser.Text);
                cmd.Parameters.AddWithValue("@password", inputpassword.Password);

                var sqlreader = cmd.ExecuteReader();
                if (sqlreader.HasRows)
                {
                    admin ad = new admin();
                    this.Close();
                    ad.ShowDialog();
                }
                else
                {
                    MessageBox.Show("Tài khoản hoặc mật khẩu không đúng");
                }
                conn.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void btnloginCustomer_Click(object sender, RoutedEventArgs e)
        {
            FormLoginCustomer loginCustomer = new FormLoginCustomer();
            this.Close();
            loginCustomer.ShowDialog();
        }
    }
}