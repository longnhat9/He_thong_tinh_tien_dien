using System;
using System.Collections.Generic;
using System.Data.SqlClient;
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
    /// Interaction logic for FormLoginCustomer.xaml
    /// </summary>
    public partial class FormLoginCustomer : Window
    {
        private static string Makh;
        public FormLoginCustomer()
        {
            InitializeComponent();
            WindowStartupLocation = System.Windows.WindowStartupLocation.CenterScreen;
        }

        public static string getMakh()
        {
            return Makh;
        }

        private void btnloginCustomer_Click(object sender, RoutedEventArgs e)
        {
            Makh = inputmakh.Text;
            if (string.IsNullOrWhiteSpace(inputmakh.Text) || string.IsNullOrWhiteSpace(inputpassword_customer.Password))
            {
                MessageBox.Show("Mã khách hàng hoặc mật khẩu không được để trống!");
                return;
            }

            try
            {
                string strConnectDB = @"Data Source=DESKTOP-173K518;Initial Catalog=BTL_tien_dien;User ID=sa;Password=admin";
                SqlConnection conn = new SqlConnection(strConnectDB);
                conn.Open();
                String sqlSelect = "SELECT user_customer, pass_customer FROM tablelogin_customer WHERE user_customer = @user AND pass_customer = @password";
                SqlCommand cmd = new SqlCommand(sqlSelect, conn);
                cmd.Parameters.AddWithValue("@user", inputmakh.Text);
                cmd.Parameters.AddWithValue("@password", inputpassword_customer.Password);
                var sqlreader = cmd.ExecuteReader();
                if (sqlreader.HasRows)
                {
                    Customer formCustomer = new Customer();
                    this.Close();
                    formCustomer.ShowDialog();
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

        private void btn_login_admin_Click(object sender, RoutedEventArgs e)
        {
            MessageBoxResult result = MessageBox.Show("Bạn có muốn truy cập vào trang đăng nhập quyền quản trị không?", "Xác nhận truy cập", MessageBoxButton.YesNo);
            switch (result)
            {
                case MessageBoxResult.Yes:
                    FormLoginAdmin loginAdmin = new FormLoginAdmin();
                    this.Close();
                    loginAdmin.ShowDialog();
                    break;
                case MessageBoxResult.No:
                    break;
            }
        }
    }
}