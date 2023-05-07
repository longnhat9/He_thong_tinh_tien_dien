using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
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
using Microsoft.SqlServer.Server;
using System.Net.Mail;
using System.Net;
using Microsoft.Win32;
using OfficeOpenXml;
using System.IO;
using iTextSharp.text.pdf;
using iTextSharp.text;

namespace App_tien_dien
{
    /// <summary>
    /// Interaction logic for admin.xaml
    /// </summary>
    public partial class admin : Window
    {
        public admin()
        {
            InitializeComponent();
            Loadgrid();
            loadgrid_hoadon();
            set_cb_list_makh();
            set_cb_status_thanhtoan();
            set_cb_don_gia();
            set_cb_status_hoadon();
            WindowStartupLocation = System.Windows.WindowStartupLocation.CenterScreen;
            //send_email_qua_han();
        }

        string sqlConnect = @"Data Source=DESKTOP-173K518;Initial Catalog=BTL_tien_dien;User ID=sa;Password=admin";

        private void set_cb_status_thanhtoan()
        {
            cb_status_thanhtoan.Items.Add("Chưa thanh toán");
            cb_status_thanhtoan.Items.Add("Đã thanh toán");
        }

        private void set_cb_don_gia()
        {
            cb_don_gia.Items.Add("0 - 50kwh là 1.678 đ/kwh");
            cb_don_gia.Items.Add("51 - 100kwh là 1.734 đ/kwh");
            cb_don_gia.Items.Add("101 - 200kwh là 2.014 đ/kwh");
            cb_don_gia.Items.Add("201 - 300kwh là 2.536 đ/kwh");
            cb_don_gia.Items.Add("301 - 400kwh là 2.834 đ/kwh");
            cb_don_gia.Items.Add("Từ 400kwh trở lên là 2.927 đ/kwh");
            cb_don_gia.Text = cb_don_gia.Items[0].ToString();
        }

        private void set_cb_status_hoadon()
        {
            cb_status_hoadon.Items.Add("Hóa đơn đã thanh toán");
            cb_status_hoadon.Items.Add("Hóa đơn chưa thanh toán");
            cb_status_hoadon.Items.Add("Tất cả các hóa đơn");
            cb_status_hoadon.Text = cb_status_hoadon.Items[2].ToString();
        }

        private void check_thanhtoan()
        {
            try
            {
                SqlConnection conn = new SqlConnection(sqlConnect);
                conn.Open();
                string sqlSelect = "SELECT * FROM hoadon WHERE mahoadon = @mahd AND makh = @Makh AND status_thanhtoan = @status_thanh_toan";
                SqlCommand cmd = new SqlCommand(sqlSelect, conn);
                cmd.Parameters.AddWithValue("@mahd", mahd_hoadon.Text);
                cmd.Parameters.AddWithValue("@Makh", cb_list_makh.Text);
                cmd.Parameters.AddWithValue("@status_thanh_toan", cb_status_thanhtoan.Items[1]);
                var check = cmd.ExecuteReader();
                if (check.HasRows)
                {
                    cb_status_thanhtoan.IsEnabled = false;
                }
                else
                {
                    cb_status_thanhtoan.IsEnabled = true;
                }
                conn.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void Loadgrid()
        {
            try
            {
                SqlConnection conn = new SqlConnection(sqlConnect);
                conn.Open();
                string sqlSelect = "SELECT * FROM tablecustomer";
                SqlCommand cmd = new SqlCommand(sqlSelect, conn);
                SqlDataReader reader = cmd.ExecuteReader();
                DataTable dataTable = new DataTable();
                dataTable.Load(reader);
                table_grid.ItemsSource = dataTable.DefaultView;
                conn.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private DataTable dttable()
        {
            try
            {
                SqlConnection conn = new SqlConnection(sqlConnect);
                conn.Open();
                string sqlSelect = "SELECT * FROM tablecustomer";
                SqlCommand cmd = new SqlCommand(sqlSelect, conn);
                SqlDataReader reader = cmd.ExecuteReader();
                DataTable dataTable = new DataTable();
                dataTable.Load(reader);
                conn.Close();
                return dataTable;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            return null;
        }

        private void Resetform()
        {
            makh.Text = "";
            hoten.Text = "";
            sodt.Text = "";
            email.Text = "";
            diachi.Text = "";
            macongto.Text = "";
            Loadgrid();
        }

        private bool CheckValid()
        {
            if (string.IsNullOrWhiteSpace(makh.Text) || string.IsNullOrWhiteSpace(hoten.Text) || string.IsNullOrWhiteSpace(sodt.Text) || string.IsNullOrWhiteSpace(email.Text) || string.IsNullOrWhiteSpace(diachi.Text) || string.IsNullOrWhiteSpace(macongto.Text))
            {
                return false;
            }
            else
            {
                return true;
            }
        }

        private bool check_makh()
        {
            try
            {
                SqlConnection conn = new SqlConnection(sqlConnect);
                conn.Open();
                string sqlSelect = @"SELECT * FROM tablecustomer WHERE makh = @makh";
                SqlCommand cmd = new SqlCommand(sqlSelect, conn);
                cmd.Parameters.AddWithValue("@makh", makh.Text);
                var check = cmd.ExecuteReader();
                if (check.HasRows)
                {
                    conn.Close();
                    return true;
                }
                conn.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            return false;
        }

        private bool check_sdt()
        {
            try
            {
                SqlConnection conn = new SqlConnection(sqlConnect);
                conn.Open();
                string sqlSelect = @"SELECT * FROM tablecustomer WHERE sodt = @sodt";
                SqlCommand cmd = new SqlCommand(sqlSelect, conn);
                cmd.Parameters.AddWithValue("@sodt", sodt.Text);
                var check = cmd.ExecuteReader();
                if (check.HasRows)
                {
                    conn.Close();
                    return true;
                }
                conn.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            return false;
        }

        private bool check_macongto()
        {
            try
            {
                SqlConnection conn = new SqlConnection(sqlConnect);
                conn.Open();
                string sqlSelect = @"SELECT * FROM tablecustomer WHERE macongto = @macongto";
                SqlCommand cmd = new SqlCommand(sqlSelect, conn);
                cmd.Parameters.AddWithValue("@macongto", macongto.Text);
                var check = cmd.ExecuteReader();
                if (check.HasRows)
                {
                    conn.Close();
                    return true;
                }
                conn.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            return false;
        }

        private bool check_edit()
        {
            try
            {
                SqlConnection conn = new SqlConnection(sqlConnect);
                conn.Open();
                string sqlSelect = @"SELECT * FROM tablecustomer WHERE makh = @makh AND tenkh = @tenkh AND sodt = @sodt AND email = @email AND diachi = @diachi AND macongto = @macongto";
                SqlCommand cmd = new SqlCommand(sqlSelect, conn);
                cmd.Parameters.AddWithValue("@makh", makh.Text);
                cmd.Parameters.AddWithValue("@tenkh", hoten.Text);
                cmd.Parameters.AddWithValue("@sodt", sodt.Text);
                cmd.Parameters.AddWithValue("@email", email.Text);
                cmd.Parameters.AddWithValue("@diachi", diachi.Text);
                cmd.Parameters.AddWithValue("@macongto", macongto.Text);
                var check = cmd.ExecuteReader();
                if (check.HasRows)
                {
                    conn.Close();
                    return true;
                }
                conn.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            return false;
        }

        private void add_user()
        {
            try
            {
                SqlConnection conn = new SqlConnection(sqlConnect);
                conn.Open();
                string sqlupdate = @"INSERT INTO tablelogin_customer VALUES (@usercustomer, @passcustomer)";
                SqlCommand cmd = new SqlCommand(sqlupdate, conn);
                cmd.Parameters.AddWithValue("@usercustomer", makh.Text);
                cmd.Parameters.AddWithValue("@passcustomer", makh.Text);
                cmd.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void btnadd_Click(object sender, RoutedEventArgs e)
        {
            if (CheckValid() == false)
            {
                MessageBox.Show("Bạn chưa nhập đủ thông tin của khách hàng!");
                return;
            }

            if (sodt.Text.Length != 10)
            {
                MessageBox.Show("Số điện thoại phải đủ 10 số");
                return;
            }

            if (check_makh() == true)
            {
                MessageBox.Show("Mã khách hàng đã tồn tại trong hệ thống!");
                return;
            }

            if (check_sdt() == true)
            {
                MessageBox.Show("Số điện thoại đã tồn tại trong hệ thống!");
                return;
            }

            if (check_macongto() == true)
            {
                MessageBox.Show("Mã công tơ đã tồn tại trong hệ thống!");
                return;
            }

            try
            {
                SqlConnection conn = new SqlConnection(sqlConnect);
                conn.Open();
                String sqlinsert = "INSERT INTO tablecustomer VALUES (@makh, @tenkh, @sodt, @email, @diachi, @macongto)";
                SqlCommand cmd = new SqlCommand(sqlinsert, conn);
                cmd.Parameters.AddWithValue("@makh", makh.Text);
                cmd.Parameters.AddWithValue("@tenkh", hoten.Text);
                cmd.Parameters.AddWithValue("@sodt", sodt.Text);
                cmd.Parameters.AddWithValue("@email", email.Text);
                cmd.Parameters.AddWithValue("@diachi", diachi.Text);
                cmd.Parameters.AddWithValue("@macongto", macongto.Text);
                cmd.ExecuteNonQuery();
                MessageBox.Show("Đã thêm thành công!");
                add_user();
                Resetform();
                conn.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private bool check_edit_customer()
        {
            try
            {
                SqlConnection conn = new SqlConnection(sqlConnect);
                conn.Open();
                string sqlSelect = @"SELECT * FROM tablecustomer WHERE makh = @makh AND tenkh = @tenkh AND sodt = @sodt AND email = @email AND diachi = @diachi AND macongto = @macongto";
                SqlCommand cmd = new SqlCommand(sqlSelect, conn);
                cmd.Parameters.AddWithValue("@makh", makh.Text);
                cmd.Parameters.AddWithValue("@tenkh", hoten.Text);
                cmd.Parameters.AddWithValue("@sodt", sodt.Text);
                cmd.Parameters.AddWithValue("@email", email.Text);
                cmd.Parameters.AddWithValue("@diachi", diachi.Text);
                cmd.Parameters.AddWithValue("@macongto", macongto.Text);
                var check = cmd.ExecuteReader();
                if (check.HasRows)
                {
                    conn.Close();
                    return true;
                }
                conn.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            return false;
        }

        private void btn_modify_Click(object sender, RoutedEventArgs e)
        {
            if (CheckValid() == false)
            {
                MessageBox.Show("Bạn chưa nhập đủ thông tin của khách hàng!");
                return;
            }

            if (check_edit_customer() == true)
            {
                MessageBox.Show("Các thông tin của khách hàng chưa được chỉnh sửa!");
                return;
            }

            if (check_makh() == false)
            {
                MessageBox.Show("Mã khách hàng không tồn tại trong hệ thống!");
                return;
            }

            try
            {
                SqlConnection conn = new SqlConnection(sqlConnect);
                conn.Open();
                String sqlinsert = "SELECT sodt FROM tablecustomer WHERE makh = @makh";
                SqlDataAdapter adapter = new SqlDataAdapter(sqlinsert, conn);
                adapter.SelectCommand.Parameters.AddWithValue("@makh", makh.Text);
                DataTable table = new DataTable();
                adapter.Fill(table);
                conn.Close();

                int rows = table.Rows.Count;
                for (int i = 0; i < rows; i++)
                {
                    if (sodt.Text.Equals(table.Rows[i]["sodt"]) == false)
                    {
                        MessageBox.Show("Số điện thoại đã tồn tại trong hệ thống!");
                        return;
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }

            try
            {
                SqlConnection conn = new SqlConnection(sqlConnect);
                conn.Open();
                String sqlinsert = "SELECT macongto FROM tablecustomer WHERE makh = @makh";
                SqlDataAdapter adapter = new SqlDataAdapter(sqlinsert, conn);
                adapter.SelectCommand.Parameters.AddWithValue("@makh", makh.Text);
                DataTable table = new DataTable();
                adapter.Fill(table);
                conn.Close();

                int rows = table.Rows.Count;
                for (int i = 0; i < rows; i++)
                {
                    if (macongto.Text.Equals(table.Rows[i]["macongto"]) == false)
                    {
                        MessageBox.Show("Mã công tơ đã tồn tại trong hệ thống!");
                        return;
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }

            MessageBoxResult result = MessageBox.Show("Bạn có muốn sửa thông tin khách hàng này không?", "Xác nhận chỉnh sửa", MessageBoxButton.YesNo);
            switch (result)
            {
                case MessageBoxResult.Yes:
                    if (check_sdt() == false)
                    {
                        try
                        {
                            SqlConnection conn = new SqlConnection(sqlConnect);
                            conn.Open();
                            String sqlinsert = "UPDATE tablecustomer SET sodt = @sodt WHERE makh = @makh";
                            SqlCommand cmd = new SqlCommand(sqlinsert, conn);
                            cmd.Parameters.AddWithValue("@makh", makh.Text);
                            cmd.Parameters.AddWithValue("@sodt", sodt.Text);
                            cmd.ExecuteNonQuery();
                            conn.Close();
                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show(ex.Message);
                        }
                    }

                    if (check_macongto() == false)
                    {
                        try
                        {
                            SqlConnection conn = new SqlConnection(sqlConnect);
                            conn.Open();
                            String sqlinsert = "UPDATE tablecustomer SET macongto = @macongto WHERE makh = @makh";
                            SqlCommand cmd = new SqlCommand(sqlinsert, conn);
                            cmd.Parameters.AddWithValue("@makh", makh.Text);
                            cmd.Parameters.AddWithValue("@macongto", macongto.Text);
                            cmd.ExecuteNonQuery();
                            conn.Close();
                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show(ex.Message);
                        }
                    }

                    try
                    {
                        SqlConnection conn = new SqlConnection(sqlConnect);
                        conn.Open();
                        String sqlinsert = "UPDATE tablecustomer SET tenkh = @tenkh, email = @email, diachi = @diachi WHERE makh = @makh";
                        SqlCommand cmd = new SqlCommand(sqlinsert, conn);
                        cmd.Parameters.AddWithValue("@makh", makh.Text);
                        cmd.Parameters.AddWithValue("@tenkh", hoten.Text);
                        cmd.Parameters.AddWithValue("@email", email.Text);
                        cmd.Parameters.AddWithValue("@diachi", diachi.Text);
                        cmd.ExecuteNonQuery();
                        MessageBox.Show("Đã Sửa thông tin thành công!");
                        Resetform();
                        conn.Close();
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message);
                    }
                    break;
                case MessageBoxResult.No:
                    break;
            }
        }

        private void btnremove_Click(object sender, RoutedEventArgs e)
        {
            if (CheckValid() == false)
            {
                MessageBox.Show("Bạn chưa chọn thông tin khách hàng để xóa!");
                return;
            }

            if (check_makh() == false)
            {
                MessageBox.Show("Mã khách hàng chưa đúng để xóa!");
                return;
            }

            if (check_edit() == false)
            {
                MessageBox.Show("Các thông tin không đúng so với thông tin của khách hàng!");
                return;
            }

            MessageBoxResult result = MessageBox.Show("Bạn có muốn xóa thông tin khách hàng này không?", "Xác nhận xóa", MessageBoxButton.YesNo);
            string tmp = "";
            switch (result)
            {
                case MessageBoxResult.Yes:
                    try
                    {
                        SqlConnection conn = new SqlConnection(sqlConnect);
                        conn.Open();
                        string sql_delete = "DELETE FROM tablecustomer WHERE makh = @makh";
                        SqlCommand cmd = new SqlCommand(sql_delete, conn);
                        tmp = makh.Text;
                        cmd.Parameters.AddWithValue("@makh", makh.Text);
                        cmd.ExecuteNonQuery();
                        MessageBox.Show("Đã xóa thành công!");
                        Resetform();
                        conn.Close();
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message);
                    }

                    try
                    {
                        SqlConnection conn = new SqlConnection(sqlConnect);
                        conn.Open();
                        string sql_delete = "DELETE FROM tablelogin_customer WHERE user_customer = @makh";
                        SqlCommand cmd = new SqlCommand(sql_delete, conn);
                        cmd.Parameters.AddWithValue("@makh", tmp);
                        cmd.ExecuteNonQuery();
                        conn.Close();
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message);
                    }
                    break;
                case MessageBoxResult.No:
                    break;
            }
        }

        private void btnreset_Click(object sender, RoutedEventArgs e)
        {
            Resetform();
        }

        private void table_grid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (table_grid.SelectedIndex.ToString() != null)
            {
                DataRowView dtrv = (DataRowView)table_grid.SelectedItem;
                if (dtrv != null)
                {
                    makh.Text = dtrv[0].ToString();
                    hoten.Text = dtrv[1].ToString();
                    sodt.Text = dtrv[2].ToString();
                    email.Text = dtrv[3].ToString();
                    diachi.Text = dtrv[4].ToString();
                    macongto.Text = dtrv[5].ToString();
                }
            }
        }

        private void btnsearch_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(inputsearch.Text))
            {
                MessageBox.Show("Chưa nhập mã khách hàng để tìm kiếm");
                return;
            }

            try
            {
                SqlConnection conn = new SqlConnection(sqlConnect);
                conn.Open();
                string sqlSelect = "SELECT * FROM tablecustomer WHERE makh = @makh";
                SqlCommand cmd = new SqlCommand(sqlSelect, conn);
                cmd.Parameters.AddWithValue("@makh", inputsearch.Text);
                var check = cmd.ExecuteNonQuery();
                SqlDataReader reader = cmd.ExecuteReader();
                DataTable dataTable = new DataTable();
                dataTable.Load(reader);
                table_grid.ItemsSource = dataTable.DefaultView;
                conn.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        // hóa đơn
        private void loadgrid_hoadon()
        {
            try
            {
                SqlConnection conn = new SqlConnection(sqlConnect);
                conn.Open();
                string sqlselect = "SELECT * FROM hoadon";
                SqlCommand cmd = new SqlCommand(sqlselect, conn);
                SqlDataReader reader = cmd.ExecuteReader();
                DataTable dataTable = new DataTable();
                dataTable.Load(reader);
                grid_list_hoadon.ItemsSource = dataTable.DefaultView;
                conn.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private bool checkform_hoadon()
        {
            if (string.IsNullOrWhiteSpace(mahd_hoadon.Text) || string.IsNullOrWhiteSpace(macongto_hoadon.Text) || string.IsNullOrWhiteSpace(cb_list_makh.Text) || string.IsNullOrWhiteSpace(tenkh_hoadon.Text) || string.IsNullOrWhiteSpace(thang_hoadon.Text) || string.IsNullOrWhiteSpace(chisocu_hoadon.Text) || string.IsNullOrWhiteSpace(chisomoi_hoadon.Text) || string.IsNullOrWhiteSpace(thanhtien_hoadon.Text) || string.IsNullOrWhiteSpace(cb_status_thanhtoan.Text) || string.IsNullOrWhiteSpace(inputemail_hoadon.Text))
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        private void resetform_hoadon()
        {
            mahd_hoadon.Text = "";
            macongto_hoadon.Text = "";
            cb_list_makh.Text = "";
            tenkh_hoadon.Text = "";
            cb_don_gia.Text = cb_don_gia.Items[0].ToString();
            thang_hoadon.Text = "";
            chisocu_hoadon.Text = "";
            chisomoi_hoadon.Text = "";
            thanhtien_hoadon.Text = "";
            inputemail_hoadon.Text = "";
            inputsearch_hoadon.Text = "";
            radio_mahd.IsChecked = false;
            radio_makh.IsChecked = false;
            radio_chuathanhtoan.IsChecked = false;
            radio_dathanhtoan.IsChecked = false;
            cb_status_thanhtoan.Text = "";
            cb_status_hoadon.Text = cb_status_hoadon.Items[2].ToString();
            loadgrid_hoadon();
            cb_status_thanhtoan.IsEnabled = true;
        }

        private void get_hoadon(string mahd)
        {
            try
            {
                SqlConnection conn = new SqlConnection(sqlConnect);
                conn.Open();
                string sqlSelect = "SELECT * FROM hoadon WHERE mahoadon = @mahd";
                SqlCommand cmd = new SqlCommand(sqlSelect, conn);
                cmd.Parameters.AddWithValue("@mahd", mahd);
                SqlDataReader reader = cmd.ExecuteReader();
                DataTable dt = new DataTable();
                dt.Load(reader);
                conn.Close();
                string filePath = @"D:\App_Tien_Dien\App_tien_dien\PDF\" + mahd + "_Chua_Thanh_Toan.pdf";
                SaveFileDialog saveFileDialog = new SaveFileDialog();
                saveFileDialog.InitialDirectory = filePath;
                saveFileDialog.FileName = filePath;
                ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
                ExportToPdf(dt, filePath);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void ExportToPdf(DataTable dt, string strFilePath)
        {
            Document document = new Document();
            PdfWriter writer = PdfWriter.GetInstance(document, new FileStream(strFilePath, FileMode.Create));
            document.Open();
            iTextSharp.text.Font font5 = iTextSharp.text.FontFactory.GetFont(FontFactory.HELVETICA, 5);

            PdfPTable table = new PdfPTable(dt.Columns.Count);
            float[] widths = new float[dt.Columns.Count];
            for (int i = 0; i < dt.Columns.Count; i++)
                widths[i] = 4f;

            table.SetWidths(widths);

            table.WidthPercentage = 100;
            PdfPCell cell = new PdfPCell(new Phrase("Products"));

            cell.Colspan = dt.Columns.Count;

            foreach (DataColumn c in dt.Columns)
            {
                table.AddCell(new Phrase(c.ColumnName, font5));
            }

            foreach (DataRow r in dt.Rows)
            {
                if (dt.Rows.Count > 0)
                {
                    for (int h = 0; h < dt.Columns.Count; h++)
                    {
                        table.AddCell(new Phrase(r[h].ToString(), font5));
                    }
                }
            }
            document.Add(table);
            document.Close();
        }

        private bool check_hoadon_qua_han(string date)
        {
            int day = int.Parse(date.Substring(0, 2));
            int month = int.Parse(date.Substring(3, 2));
            int year = int.Parse(date.Substring(6, 4));
            DateTime date_now = DateTime.Parse(DateTime.Now.ToString("dd/MM/yyyy"));
            if(year < date_now.Year)
            {
                return true;
            }
            else if (year == date_now.Year && month + 1 < date_now.Month)
            {
                return true;
            }
            else if (year == date_now.Year && month + 1 == date_now.Month && day < date_now.Day)
            {
                return true;
            }
            return false;
        }

        private void send_email_qua_han()
        {
            DataTable dataTable = getDatatable_all_hoadon();
            int rows = dataTable.Rows.Count;
            if(rows == 0)
            {
                MessageBox.Show("Chưa có dữ liệu hóa đơn!");
                return;
            }

            for(int i = 0; i < rows; i++)
            {
                string check_thanhtoan = dataTable.Rows[i]["status_thanhtoan"].ToString();
                if(check_thanhtoan.Equals("Chưa thanh toán"))
                {
                    bool check_quahan = check_hoadon_qua_han(dataTable.Rows[i]["thang"].ToString());
                    if(check_quahan == true)
                    {
                        string email = dataTable.Rows[i]["email"].ToString();
                        string tenkh = dataTable.Rows[i]["tenkh"].ToString();
                        get_hoadon(dataTable.Rows[i]["mahoadon"].ToString());
                        string message_email = "Xin kính chào quý khách: " + tenkh;
                        MailAddress myemail = new MailAddress("20010974@st.phenikaa-uni.edu.vn", "Quản lý tiền điện");
                        MailAddress mail_to = new MailAddress(email, tenkh);

                        string password = "Longnhat20089";
                        SmtpClient client_smtp = new SmtpClient("smtp.gmail.com", 587);
                        client_smtp.EnableSsl = true;
                        client_smtp.DeliveryMethod = SmtpDeliveryMethod.Network;
                        client_smtp.UseDefaultCredentials = false;
                        client_smtp.Credentials = new NetworkCredential(myemail.Address, password);
                        MailMessage message = new MailMessage(myemail, mail_to);
                        message.Subject = "Hóa đơn tiền điện";
                        message.Body = message_email;
                        string file_pdf_path = @"D:\App_Tien_Dien\App_tien_dien\PDF\" + dataTable.Rows[i]["mahoadon"].ToString() + "_Chua_Thanh_Toan.pdf";
                        Attachment attachment = new Attachment(file_pdf_path);
                        message.Attachments.Add(attachment);
                        try
                        {
                            client_smtp.Send(message);
                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show(ex.Message);
                        }
                    }
                }
            }
        }

        private bool check_mahd_hoadon()
        {
            try
            {
                SqlConnection conn = new SqlConnection(sqlConnect);
                conn.Open();
                string sqlselect = "SELECT mahoadon FROM hoadon WHERE mahoadon = @mahd";
                SqlCommand cmd = new SqlCommand(sqlselect, conn);
                cmd.Parameters.AddWithValue("@mahd", mahd_hoadon.Text);
                var check = cmd.ExecuteReader();
                if (check.HasRows)
                {
                    conn.Close();
                    return true;
                }
                conn.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            return false;
        }

        private bool checkvalid_email()
        {
            return true;
        }

        private void set_cb_list_makh()
        {
            try
            {
                SqlConnection conn = new SqlConnection(sqlConnect);
                conn.Open();
                string sqlSelect = "SELECT makh FROM tablecustomer";
                SqlCommand cmd = new SqlCommand(sqlSelect, conn);
                var list_makh = cmd.ExecuteReader();
                if (list_makh.HasRows)
                {
                    while (list_makh.Read())
                    {
                        cb_list_makh.Items.Add(list_makh.GetString(0));
                    }
                }

                conn.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        bool chiso_hople = true;

        private void btn_add_Click(object sender, RoutedEventArgs e)
        {
            if (cb_list_makh.SelectedItem != null)
            {
                if (cb_list_makh.SelectedItem.ToString() == "")
                {
                    MessageBox.Show("Bạn chưa chọn mã khách hàng!");
                    return;
                }
            }

            if (checkform_hoadon() == true)
            {
                MessageBox.Show("Bạn chưa nhập đủ thông tin của hóa đơn!");
                return;
            }

            if(chiso_hople == false) {
                MessageBox.Show("Chỉ số mới phải lớn hơn chỉ số cũ!");
                return;
            }

            if (double.TryParse(chisocu_hoadon.Text, out double l) == false)
            {
                MessageBox.Show("Chỉ số cũ phải là số!");
                return;
            }

            if (double.TryParse(chisomoi_hoadon.Text, out double k) == false)
            {
                MessageBox.Show("Chỉ số mới phải là số!");
                return;
            }

            try
            {
                SqlConnection conn = new SqlConnection(sqlConnect);
                conn.Open();
                string sqlselect = "SELECT mahoadon FROM hoadon WHERE mahoadon = @mahd";
                SqlCommand cmd = new SqlCommand(sqlselect, conn);
                cmd.Parameters.AddWithValue("@mahd", mahd_hoadon.Text);
                var check = cmd.ExecuteReader();
                if (check.HasRows)
                {
                    MessageBox.Show("Mã hóa đơn đã tồn tại trong hệ thống!");
                    conn.Close();
                    return;
                }

                SqlConnection sqlconn = new SqlConnection(sqlConnect);
                sqlconn.Open();
                string sqlInsert = "INSERT INTO hoadon VALUES (@mahd, @macongto, @makh, @tenkh, @thang, @chisocu, @chisomoi, @thanhtien, @status_thanhtoan, @email)";
                using (SqlCommand cmd_insert = new SqlCommand(sqlInsert, sqlconn))
                {
                    cmd_insert.Parameters.AddWithValue("@mahd", mahd_hoadon.Text);
                    cmd_insert.Parameters.AddWithValue("@macongto", macongto_hoadon.Text);
                    if (cb_list_makh.SelectedItem != null)
                    {
                        cmd_insert.Parameters.AddWithValue("@makh", cb_list_makh.SelectedItem.ToString());
                    }
                    cmd_insert.Parameters.AddWithValue("@tenkh", tenkh_hoadon.Text);
                    cmd_insert.Parameters.AddWithValue("@thang", thang_hoadon.Text);
                    cmd_insert.Parameters.AddWithValue("@chisocu", chisocu_hoadon.Text);
                    cmd_insert.Parameters.AddWithValue("@chisomoi", chisomoi_hoadon.Text);
                    cmd_insert.Parameters.AddWithValue("@thanhtien", thanhtien_hoadon.Text);
                    cmd_insert.Parameters.AddWithValue("@status_thanhtoan", cb_status_thanhtoan.Text);
                    cmd_insert.Parameters.AddWithValue("@email", inputemail_hoadon.Text);
                    cmd_insert.ExecuteNonQuery();
                    MessageBox.Show("Đã thêm hóa đơn thành công!");
                    send_email_qua_han();
                    resetform_hoadon();
                }
                sqlconn.Close();
                conn.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private bool check_edit_form_hoadon()
        {
            try
            {
                SqlConnection conn = new SqlConnection(sqlConnect);
                conn.Open();
                string sqlselect = "SELECT * FROM hoadon WHERE mahoadon = @mahd AND macongto = @macongto AND makh = @makh AND tenkh = @tenkh AND thang = @thang AND chisocu = @chisocu AND chisomoi = @chisomoi AND thanhtien = @thanhtien AND status_thanhtoan = @status_thanhtoan AND email = @email";
                SqlCommand cmd = new SqlCommand(sqlselect, conn);
                cmd.Parameters.AddWithValue("@mahd", mahd_hoadon.Text);
                cmd.Parameters.AddWithValue("@macongto", macongto_hoadon.Text);
                cmd.Parameters.AddWithValue("@makh", cb_list_makh.Text);
                cmd.Parameters.AddWithValue("@tenkh", tenkh_hoadon.Text);
                cmd.Parameters.AddWithValue("@thang", thang_hoadon.Text);
                cmd.Parameters.AddWithValue("@chisocu", chisocu_hoadon.Text);
                cmd.Parameters.AddWithValue("@chisomoi", chisomoi_hoadon.Text);
                cmd.Parameters.AddWithValue("@thanhtien", thanhtien_hoadon.Text);
                cmd.Parameters.AddWithValue("@status_thanhtoan", cb_status_thanhtoan.Text);
                cmd.Parameters.AddWithValue("@email", inputemail_hoadon.Text);
                var check = cmd.ExecuteReader();
                if (check.HasRows)
                {
                    conn.Close();
                    return true;
                }
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            return false;
        }

        private void btn_update_Click(object sender, RoutedEventArgs e)
        {
            if (checkform_hoadon() == true)
            {
                MessageBox.Show("Bạn chưa nhập đủ thông tin của hóa đơn!");
                return;
            }

            if(check_edit_form_hoadon() == true)
            {
                MessageBox.Show("Các trường thông tin chưa được chỉnh sửa!");
                return;
            }

            if (check_mahd_hoadon() == false)
            {
                MessageBox.Show("Mã hóa đơn không tồn tại trong hệ thống!");
                return;
            }

            if (chiso_hople == false)
            {
                MessageBox.Show("Chỉ số mới phải lớn hơn chỉ số cũ!");
                return;
            }

            if (double.TryParse(chisocu_hoadon.Text, out double l) == false)
            {
                MessageBox.Show("Chỉ số cũ phải là số!");
                return;
            }

            if (double.TryParse(chisomoi_hoadon.Text, out double k) == false)
            {
                MessageBox.Show("Chỉ số mới phải là số!");
                return;
            }

            try
            {
                SqlConnection conn = new SqlConnection(sqlConnect);
                conn.Open();
                String sqlinsert = "SELECT mahoadon FROM hoadon";
                SqlDataAdapter adapter = new SqlDataAdapter(sqlinsert, conn);
                DataTable table = new DataTable();
                adapter.Fill(table);
                conn.Close();

                int rows = table.Rows.Count;
                for (int i = 0; i < rows; i++)
                {
                    if (mahd_hoadon.Text.Equals(table.Rows[i]["mahoadon"]) == false)
                    {
                        MessageBox.Show("Mã hóa đơn không được chỉnh sửa!");
                        return;
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }

            MessageBoxResult result = MessageBox.Show("Bạn có muốn chỉnh sửa hóa đơn này không?", "Xác nhận sửa", MessageBoxButton.YesNo);
            switch (result)
            {
                case MessageBoxResult.Yes:
                    try
                    {
                        SqlConnection conn = new SqlConnection(sqlConnect);
                        conn.Open();
                        string sqlselect_mahd = @"UPDATE hoadon SET thang = @thang, chisocu = @chisocu, chisomoi = @chisomoi, thanhtien = @thanhtien, status_thanhtoan = @statusthanhtoan WHERE mahoadon = @mahd";
                        SqlCommand cmd_insert = new SqlCommand(sqlselect_mahd, conn);
                        cmd_insert.Parameters.AddWithValue("@thang", thang_hoadon.Text);
                        cmd_insert.Parameters.AddWithValue("@chisocu", chisocu_hoadon.Text);
                        cmd_insert.Parameters.AddWithValue("@chisomoi", chisomoi_hoadon.Text);
                        cmd_insert.Parameters.AddWithValue("@thanhtien", thanhtien_hoadon.Text);
                        cmd_insert.Parameters.AddWithValue("@statusthanhtoan", cb_status_thanhtoan.Text);
                        cmd_insert.Parameters.AddWithValue("@mahd", mahd_hoadon.Text);
                        cmd_insert.ExecuteNonQuery();
                        MessageBox.Show("Đã chỉnh sửa thành công");
                        resetform_hoadon();
                        conn.Close();
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message);
                    }
                    break;
                case MessageBoxResult.No:
                    break;
            }
        }

        private void grid_list_hoadon_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (grid_list_hoadon.SelectedIndex.ToString() != null)
            {
                DataRowView dtrv = (DataRowView)grid_list_hoadon.SelectedItem;
                if (dtrv != null)
                {
                    mahd_hoadon.Text = dtrv[0].ToString();
                    macongto_hoadon.Text = dtrv[1].ToString();
                    cb_list_makh.Text = dtrv[2].ToString();
                    tenkh_hoadon.Text = dtrv[3].ToString();
                    thang_hoadon.Text = dtrv[4].ToString();
                    chisocu_hoadon.Text = dtrv[5].ToString();
                    chisomoi_hoadon.Text = dtrv[6].ToString();
                    thanhtien_hoadon.Text = dtrv[7].ToString();
                    cb_status_thanhtoan.Text = dtrv[8].ToString();
                    inputemail_hoadon.Text = dtrv[9].ToString();
                    check_thanhtoan();
                }
            }
        }

        private void btn_delete_Click(object sender, RoutedEventArgs e)
        {
            if (checkform_hoadon() == true)
            {
                MessageBox.Show("Bạn chưa nhập đủ thông tin của hóa đơn để xóa!");
                return;
            }

            if (check_edit_form_hoadon() == false)
            {
                MessageBox.Show("Các trường thông tin không chính xác để xóa!");
                return;
            }

            MessageBoxResult result = MessageBox.Show("Bạn có muốn xóa hóa đơn này không?", "Xác nhận xóa", MessageBoxButton.YesNo);
            switch (result)
            {
                case MessageBoxResult.Yes:
                    try
                    {
                        SqlConnection conn = new SqlConnection(sqlConnect);
                        conn.Open();
                        string sql_delete = "DELETE FROM hoadon WHERE mahoadon = @mahd";
                        SqlCommand cmd = new SqlCommand(sql_delete, conn);
                        cmd.Parameters.AddWithValue("@mahd", mahd_hoadon.Text);
                        cmd.ExecuteNonQuery();
                        MessageBox.Show("Đã xóa thành công!");
                        resetform_hoadon();
                        conn.Close();
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message);
                    }
                    break;
                case MessageBoxResult.No:
                    break;
            }
        }

        private void send_email()
        {
            string message_email = "Xin kính chào quý khách: " + tenkh_hoadon.Text;
            MailAddress myemail = new MailAddress("20010974@st.phenikaa-uni.edu.vn", "Quản lý tiền điện");
            MailAddress mail_to = new MailAddress(inputemail_hoadon.Text, tenkh_hoadon.Text);

            string password = "Longnhat20089";

            SmtpClient client_smtp = new SmtpClient("smtp.gmail.com", 587);
            client_smtp.EnableSsl = true;
            client_smtp.DeliveryMethod = SmtpDeliveryMethod.Network;
            client_smtp.UseDefaultCredentials = false;
            client_smtp.Credentials = new NetworkCredential(myemail.Address, password);

            MailMessage message = new MailMessage(myemail, mail_to);
            message.Subject = "Hóa đơn tiền điện";
            message.Body = message_email;

            try
            {
                client_smtp.Send(message);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void btnsend_email_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(inputemail_hoadon.Text) == true)
            {
                MessageBox.Show("Bạn chưa nhập email của khách hàng để gửi thông tin!");
                return;
            }

            if (checkvalid_email() == false)
            {
                MessageBox.Show("Email của khách hàng chưa đúng định dạng!");
                return;
            }

            MessageBoxResult result = MessageBox.Show("Bạn có muốn gửi mail thông báo đến khách hàng này không?", "Xác nhận gửi mail", MessageBoxButton.YesNo);
            switch (result)
            {
                case MessageBoxResult.Yes:
                    send_email();
                    break;
                case MessageBoxResult.No:
                    break;
            }
        }

        private void btn_search_Click(object sender, RoutedEventArgs e)
        {
            if (radio_mahd.IsChecked == true && radio_chuathanhtoan.IsChecked == false && radio_dathanhtoan.IsChecked == false)
            {
                if (string.IsNullOrWhiteSpace(inputsearch_hoadon.Text) == true)
                {
                    MessageBox.Show("Vui lòng nhập Mã Hóa Đơn để tìm kiếm!");
                    return;
                }

                SqlConnection conn = new SqlConnection(sqlConnect);
                conn.Open();
                string sql_search = @"SELECT * FROM hoadon WHERE mahoadon = @mahd";
                SqlCommand cmd = new SqlCommand(sql_search, conn);
                cmd.Parameters.AddWithValue("@mahd", inputsearch_hoadon.Text);
                SqlDataReader reader = cmd.ExecuteReader();
                DataTable dataTable = new DataTable();
                dataTable.Load(reader);
                grid_list_hoadon.ItemsSource = dataTable.DefaultView;
                conn.Close();
            }
            else if (radio_makh.IsChecked == true && radio_chuathanhtoan.IsChecked == false && radio_dathanhtoan.IsChecked == false)
            {
                if (string.IsNullOrWhiteSpace(inputsearch_hoadon.Text) == true)
                {
                    MessageBox.Show("Vui lòng nhập Mã Khách Hàng để tìm kiếm!");
                    return;
                }

                SqlConnection conn = new SqlConnection(sqlConnect);
                conn.Open();
                string sql_search = @"SELECT * FROM hoadon WHERE makh = @makh";
                SqlCommand cmd = new SqlCommand(sql_search, conn);
                cmd.Parameters.AddWithValue("@makh", inputsearch_hoadon.Text);
                SqlDataReader reader = cmd.ExecuteReader();
                DataTable dataTable = new DataTable();
                dataTable.Load(reader);
                grid_list_hoadon.ItemsSource = dataTable.DefaultView;
                conn.Close();
            }
            else if (radio_mahd.IsChecked == true && radio_chuathanhtoan.IsChecked == true)
            {
                if (string.IsNullOrWhiteSpace(inputsearch_hoadon.Text) == true)
                {
                    MessageBox.Show("Vui lòng nhập Mã Hóa Đơn để tìm kiếm!");
                    return;
                }

                SqlConnection conn = new SqlConnection(sqlConnect);
                conn.Open();
                string sql_search = @"SELECT * FROM hoadon WHERE mahoadon = @mahd AND status_thanhtoan = @statusthanhtoan";
                SqlCommand cmd = new SqlCommand(sql_search, conn);
                cmd.Parameters.AddWithValue("@mahd", inputsearch_hoadon.Text);
                cmd.Parameters.AddWithValue("@statusthanhtoan", "Chưa thanh toán");
                SqlDataReader reader = cmd.ExecuteReader();
                DataTable dataTable = new DataTable();
                dataTable.Load(reader);
                grid_list_hoadon.ItemsSource = dataTable.DefaultView;
                conn.Close();
            }
            else if (radio_mahd.IsChecked == true && radio_dathanhtoan.IsChecked == true)
            {
                if (string.IsNullOrWhiteSpace(inputsearch_hoadon.Text) == true)
                {
                    MessageBox.Show("Vui lòng nhập Mã Hóa Đơn để tìm kiếm!");
                    return;
                }

                SqlConnection conn = new SqlConnection(sqlConnect);
                conn.Open();
                string sql_search = @"SELECT * FROM hoadon WHERE mahoadon = @mahd AND status_thanhtoan = @statusthanhtoan";
                SqlCommand cmd = new SqlCommand(sql_search, conn);
                cmd.Parameters.AddWithValue("@mahd", inputsearch_hoadon.Text);
                cmd.Parameters.AddWithValue("@statusthanhtoan", "Đã thanh toán");
                SqlDataReader reader = cmd.ExecuteReader();
                DataTable dataTable = new DataTable();
                dataTable.Load(reader);
                grid_list_hoadon.ItemsSource = dataTable.DefaultView;
                conn.Close();
            }
            else if (radio_makh.IsChecked == true && radio_chuathanhtoan.IsChecked == true)
            {
                if (string.IsNullOrWhiteSpace(inputsearch_hoadon.Text) == true)
                {
                    MessageBox.Show("Vui lòng nhập Mã Khách Hàng để tìm kiếm!");
                    return;
                }

                SqlConnection conn = new SqlConnection(sqlConnect);
                conn.Open();
                string sql_search = @"SELECT * FROM hoadon WHERE makh = @makh AND status_thanhtoan = @statusthanhtoan";
                SqlCommand cmd = new SqlCommand(sql_search, conn);
                cmd.Parameters.AddWithValue("@makh", inputsearch_hoadon.Text);
                cmd.Parameters.AddWithValue("@statusthanhtoan", "Chưa thanh toán");
                SqlDataReader reader = cmd.ExecuteReader();
                DataTable dataTable = new DataTable();
                dataTable.Load(reader);
                grid_list_hoadon.ItemsSource = dataTable.DefaultView;
                conn.Close();
            }
            else if (radio_makh.IsChecked == true && radio_dathanhtoan.IsChecked == true)
            {
                if (string.IsNullOrWhiteSpace(inputsearch_hoadon.Text) == true)
                {
                    MessageBox.Show("Vui lòng nhập Mã Khách Hàng để tìm kiếm!");
                    return;
                }

                SqlConnection conn = new SqlConnection(sqlConnect);
                conn.Open();
                string sql_search = @"SELECT * FROM hoadon WHERE makh = @makh AND status_thanhtoan = @statusthanhtoan";
                SqlCommand cmd = new SqlCommand(sql_search, conn);
                cmd.Parameters.AddWithValue("@makh", inputsearch_hoadon.Text);
                cmd.Parameters.AddWithValue("@statusthanhtoan", "Đã thanh toán");
                SqlDataReader reader = cmd.ExecuteReader();
                DataTable dataTable = new DataTable();
                dataTable.Load(reader);
                grid_list_hoadon.ItemsSource = dataTable.DefaultView;
                conn.Close();
            }
            else if (radio_dathanhtoan.IsChecked == true)
            {
                SqlConnection conn = new SqlConnection(sqlConnect);
                conn.Open();
                string sql_search = @"SELECT * FROM hoadon WHERE status_thanhtoan = @statusthanhtoan";
                SqlCommand cmd = new SqlCommand(sql_search, conn);
                cmd.Parameters.AddWithValue("@statusthanhtoan", "Đã thanh toán");
                SqlDataReader reader = cmd.ExecuteReader();
                DataTable dataTable = new DataTable();
                dataTable.Load(reader);
                grid_list_hoadon.ItemsSource = dataTable.DefaultView;
                conn.Close();
            }
            else if (radio_chuathanhtoan.IsChecked == true)
            {
                SqlConnection conn = new SqlConnection(sqlConnect);
                conn.Open();
                string sql_search = @"SELECT * FROM hoadon WHERE status_thanhtoan = @statusthanhtoan";
                SqlCommand cmd = new SqlCommand(sql_search, conn);
                cmd.Parameters.AddWithValue("@statusthanhtoan", "Chưa thanh toán");
                SqlDataReader reader = cmd.ExecuteReader();
                DataTable dataTable = new DataTable();
                dataTable.Load(reader);
                grid_list_hoadon.ItemsSource = dataTable.DefaultView;
                conn.Close();
            }
            else
            {
                MessageBox.Show("Hãy chọn điều kiện để tìm kiếm!");
                return;
            }
        }

        private void btn_reset_Click(object sender, RoutedEventArgs e)
        {
            resetform_hoadon();
            cb_list_makh.Items.Clear();
            set_cb_list_makh();
        }

        private void btn_logout_hoadon_Click(object sender, RoutedEventArgs e)
        {
            MessageBoxResult result = MessageBox.Show("Bạn có muốn đăng xuất không?", "Xác nhận đăng xuất", MessageBoxButton.YesNo);
            switch (result)
            {
                case MessageBoxResult.Yes:
                    FormLoginAdmin formLoginAdmin = new FormLoginAdmin();
                    this.Close();
                    formLoginAdmin.ShowDialog();
                    break;
                case MessageBoxResult.No:
                    break;
            }
        }

        private void btn_logout_infor_Click(object sender, RoutedEventArgs e)
        {
            MessageBoxResult result = MessageBox.Show("Bạn có muốn đăng xuất không?", "Xác nhận đăng xuất", MessageBoxButton.YesNo);
            switch (result)
            {
                case MessageBoxResult.Yes:
                    FormLoginAdmin formLoginAdmin = new FormLoginAdmin();
                    this.Close();
                    formLoginAdmin.ShowDialog();
                    break;
                case MessageBoxResult.No:
                    break;
            }
        }

        private void cb_list_makh_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (cb_list_makh.SelectedItem != null)
            {
                if (cb_list_makh.SelectedItem.ToString() != "")
                {
                    try
                    {
                        SqlConnection conn = new SqlConnection(sqlConnect);
                        conn.Open();
                        string sqlselect = "SELECT makh, tenkh, email, macongto FROM tablecustomer WHERE makh = @makh";
                        SqlDataAdapter adapter = new SqlDataAdapter();
                        adapter.SelectCommand = new SqlCommand(sqlselect, conn);
                        adapter.SelectCommand.Parameters.AddWithValue("@makh", cb_list_makh.SelectedItem.ToString());
                        DataTable table = new DataTable();
                        adapter.Fill(table);
                        cb_list_makh.Text = table.Rows[0]["makh"].ToString();
                        tenkh_hoadon.Text = table.Rows[0]["tenkh"].ToString();
                        inputemail_hoadon.Text = table.Rows[0]["email"].ToString();
                        macongto_hoadon.Text = table.Rows[0]["macongto"].ToString();
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message);
                    }
                }
            }
        }

        private void chisomoi_hoadon_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (double.TryParse(chisocu_hoadon.Text, out double l) == false)
            {
                thanhtien_hoadon.Text = "Chỉ số cũ phải là số";
                return;
            }

            if (double.TryParse(chisomoi_hoadon.Text, out double k) == false)
            {
                thanhtien_hoadon.Text = "Chỉ số mới phải là số";
                return;
            }

            double so_dien_tieu_thu = double.Parse(chisomoi_hoadon.Text) - double.Parse(chisocu_hoadon.Text);
            if (so_dien_tieu_thu <= 0)
            {
                thanhtien_hoadon.Text = "Chỉ số mới phải cao hơn chỉ số cũ!";
                chiso_hople = false;
                return;
            }

            chiso_hople = true;

            double thanh_tien = 0;
            double tmp = 0;
            if (so_dien_tieu_thu <= 50)
            {
                thanh_tien = thanh_tien + so_dien_tieu_thu * 1.678;
            }
            else
            {
                thanh_tien = thanh_tien + 50 * 1.678;
                tmp = tmp + 50;
                if (so_dien_tieu_thu >= 50.000000001 && so_dien_tieu_thu <= 100)
                {
                    thanh_tien = thanh_tien + (so_dien_tieu_thu - tmp) * 1.734;
                }
                else
                {
                    thanh_tien = thanh_tien + 50 * 1.734;
                    tmp = tmp + 50;

                    if (so_dien_tieu_thu >= 100.0000000001 && so_dien_tieu_thu <= 200)
                    {
                        thanh_tien = thanh_tien + (so_dien_tieu_thu - tmp) * 2.014;
                    }
                    else
                    {
                        thanh_tien = thanh_tien + 100 * 2.014;
                        tmp = tmp + 100;

                        if (so_dien_tieu_thu >= 200.000000001 && so_dien_tieu_thu <= 300)
                        {
                            thanh_tien = thanh_tien + (so_dien_tieu_thu - tmp) * 2.536;
                        }
                        else
                        {
                            thanh_tien = thanh_tien + 100 * 2.536;
                            tmp = tmp + 100;
                            if (so_dien_tieu_thu >= 300.000000001 && so_dien_tieu_thu <= 400)
                            {
                                thanh_tien = thanh_tien + (so_dien_tieu_thu - tmp) * 2.834;
                            }
                            else
                            {
                                thanh_tien = thanh_tien + 100 * 2.834;
                                tmp = tmp + 100;
                                if (so_dien_tieu_thu > 400)
                                {
                                    thanh_tien = thanh_tien + (so_dien_tieu_thu - tmp) * 2.927;
                                }
                            }
                        }
                    }
                }
            }

            double thue_vat = thanh_tien * 0.1;
            double tong_tien = (thanh_tien + thue_vat) * 1000;
            string tongtien = tong_tien.ToString();
            thanhtien_hoadon.Text = tongtien + " đ";
        }

        private void chisocu_hoadon_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (double.TryParse(chisocu_hoadon.Text, out double k) == false)
            {
                thanhtien_hoadon.Text = "Chỉ số cũ phải là số";
                return;
            }
        }

        private void btn_exportToExcel_Click(object sender, RoutedEventArgs e)
        {
            string filePath = "";
            SaveFileDialog fileDialog = new SaveFileDialog();
            fileDialog.Filter = "Excel | *.xlsx";
            if (fileDialog.ShowDialog() == true)
            {
                filePath = fileDialog.FileName;
            }

            if (string.IsNullOrEmpty(filePath))
            {
                MessageBox.Show("Đường dẫn file không hợp lệ!");
                return;
            }

            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;

            try
            {
                using (ExcelPackage p = new ExcelPackage())
                {
                    p.Workbook.Properties.Author = "Admin";
                    p.Workbook.Properties.Title = "Danh sách khách hàng";
                    p.Workbook.Worksheets.Add("Test Sheet");

                    ExcelWorksheet ws = p.Workbook.Worksheets[0];
                    ws.Name = "Test Sheet";
                    ws.Cells.Style.Font.Size = 12;
                    ws.Cells.Style.Font.Name = "Times New Roman";

                    string[] arrColumnHeader =
                    {
                        "Mã khách hàng",
                        "Họ và tên",
                        "Số điện thoại",
                        "Email",
                        "Địa chỉ",
                        "Mã công tơ"
                    };

                    var countColumnHeader = arrColumnHeader.Count();
                    ws.Cells[1, 1].Value = "Danh sách khách hàng";
                    ws.Cells[1, 1, 1, countColumnHeader].Merge = true;
                    ws.Cells[1, 1, 1, countColumnHeader].Style.Font.Bold = true;
                    ws.Cells[1, 1, 1, countColumnHeader].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
                    int columnIndex = 1;
                    int rowIndex = 2;

                    foreach (var item in arrColumnHeader)
                    {
                        var cell = ws.Cells[rowIndex, columnIndex];
                        var fill = cell.Style.Fill;
                        fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
                        fill.BackgroundColor.SetColor(System.Drawing.Color.LightBlue);
                        var border = cell.Style.Border;
                        border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;
                        cell.Value = item;
                        columnIndex++;
                    }
                    DataTable dt = new DataTable();
                    dt = dttable();

                    foreach (DataRow dr in dt.Rows)
                    {
                        columnIndex = 1;
                        rowIndex++;
                        ws.Cells[rowIndex, columnIndex++].Value = dr[0].ToString();
                        ws.Cells[rowIndex, columnIndex++].Value = dr[1].ToString();
                        ws.Cells[rowIndex, columnIndex++].Value = dr[2].ToString();
                        ws.Cells[rowIndex, columnIndex++].Value = dr[3].ToString();
                        ws.Cells[rowIndex, columnIndex++].Value = dr[4].ToString();
                        ws.Cells[rowIndex, columnIndex++].Value = dr[5].ToString();
                    }
                    Byte[] bin = p.GetAsByteArray();
                    File.WriteAllBytes(filePath, bin);
                }
                MessageBox.Show("Đã xuất file Excel thành công!");
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private DataTable getDatatable_HoaDonChuathanhtoan()
        {
            try
            {
                SqlConnection conn = new SqlConnection(sqlConnect);
                conn.Open();
                string sqlSelect = "SELECT * FROM hoadon WHERE status_thanhtoan = @status_thanhtoan";
                SqlCommand cmd = new SqlCommand(sqlSelect, conn);
                cmd.Parameters.AddWithValue("@status_thanhtoan", "Chưa thanh toán");
                SqlDataReader reader = cmd.ExecuteReader();
                DataTable dataTable = new DataTable();
                dataTable.Load(reader);
                conn.Close();
                return dataTable;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            return null;
        }

        private DataTable getDatatable_HoaDonDathanhtoan()
        {
            try
            {
                SqlConnection conn = new SqlConnection(sqlConnect);
                conn.Open();
                string sqlSelect = "SELECT * FROM hoadon WHERE status_thanhtoan = @status_thanhtoan";
                SqlCommand cmd = new SqlCommand(sqlSelect, conn);
                cmd.Parameters.AddWithValue("@status_thanhtoan", "Đã thanh toán");
                SqlDataReader reader = cmd.ExecuteReader();
                DataTable dataTable = new DataTable();
                dataTable.Load(reader);
                conn.Close();
                return dataTable;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            return null;
        }

        private DataTable getDatatable_all_hoadon()
        {
            try
            {
                SqlConnection conn = new SqlConnection(sqlConnect);
                conn.Open();
                string sqlSelect = "SELECT * FROM hoadon";
                SqlCommand cmd = new SqlCommand(sqlSelect, conn);
                SqlDataReader reader = cmd.ExecuteReader();
                DataTable dataTable = new DataTable();
                dataTable.Load(reader);
                conn.Close();
                return dataTable;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            return null;
        }

        private void Export_hoadon(string status_hoadon, string status_thanhtoan)
        {
            string question = "Bạn có muốn xuất danh sách " + status_hoadon + " hay không?";
            MessageBoxResult result = MessageBox.Show(question, "Xác nhận đăng xuất", MessageBoxButton.YesNo);
            switch (result)
            {
                case MessageBoxResult.Yes:
                    string filePath = "";
                    SaveFileDialog fileDialog = new SaveFileDialog();
                    fileDialog.Filter = "Excel | *.xlsx";

                    if (fileDialog.ShowDialog() == true)
                    {
                        filePath = fileDialog.FileName;
                    }
                    MessageBox.Show(filePath);

                    if (string.IsNullOrEmpty(filePath))
                    {
                        MessageBox.Show("Đường dẫn file không hợp lệ!");
                        return;
                    }

                    ExcelPackage.LicenseContext = LicenseContext.NonCommercial;

                    try
                    {
                        using (ExcelPackage p = new ExcelPackage())
                        {
                            p.Workbook.Properties.Author = "Customer";
                            p.Workbook.Properties.Title = "Danh sách hóa đơn";
                            p.Workbook.Worksheets.Add("Test Sheet");

                            ExcelWorksheet ws = p.Workbook.Worksheets[0];
                            ws.Cells.Style.Locked = true;
                            ws.Name = "Test Sheet";
                            ws.Cells.Style.Font.Size = 12;
                            ws.Cells.Style.Font.Name = "Times New Roman";

                            string[] arrColumnHeader =
                            {
                                    "Mã hóa đơn",
                                    "Mã công tơ",
                                    "Mã khách hàng",
                                    "Tên khách hàng",
                                    "Tháng",
                                    "Chỉ số cũ",
                                    "Chỉ số mới",
                                    "Thành tiền",
                                    "Trạng thái",
                                    "Email"
                                };

                            var countColumnHeader = arrColumnHeader.Count();
                            ws.Cells[1, 1].Value = "Danh sách " + status_hoadon;
                            ws.Cells[1, 1, 1, countColumnHeader].Merge = true;
                            ws.Cells[1, 1, 1, countColumnHeader].Style.Font.Bold = true;
                            ws.Cells[1, 1, 1, countColumnHeader].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
                            int columnIndex = 1;
                            int rowIndex = 2;

                            foreach (var item in arrColumnHeader)
                            {
                                var cell = ws.Cells[rowIndex, columnIndex];
                                var fill = cell.Style.Fill;
                                fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
                                fill.BackgroundColor.SetColor(System.Drawing.Color.LightBlue);
                                var border = cell.Style.Border;
                                border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;
                                cell.Value = item;
                                columnIndex++;
                            }

                            DataTable dt = new DataTable();
                            if(status_thanhtoan.Equals("Đã thanh toán"))
                            {
                                dt = getDatatable_HoaDonDathanhtoan();
                            }
                            else if (status_thanhtoan.Equals("Chưa thanh toán"))
                            {
                                dt = getDatatable_HoaDonChuathanhtoan();
                            }
                            else
                            {
                                dt = getDatatable_all_hoadon();
                            }

                            if (dt == null)
                            {
                                MessageBox.Show("Không có thông tin hóa đơn để xuất danh sách!");
                                return;
                            }

                            foreach (DataRow dr in dt.Rows)
                            {
                                columnIndex = 1;
                                rowIndex++;
                                ws.Cells[rowIndex, columnIndex++].Value = dr[0].ToString();
                                ws.Cells[rowIndex, columnIndex++].Value = dr[1].ToString();
                                ws.Cells[rowIndex, columnIndex++].Value = dr[2].ToString();
                                ws.Cells[rowIndex, columnIndex++].Value = dr[3].ToString();
                                ws.Cells[rowIndex, columnIndex++].Value = dr[4].ToString();
                                ws.Cells[rowIndex, columnIndex++].Value = dr[5].ToString();
                                ws.Cells[rowIndex, columnIndex++].Value = dr[6].ToString();
                                ws.Cells[rowIndex, columnIndex++].Value = dr[7].ToString();
                                ws.Cells[rowIndex, columnIndex++].Value = dr[8].ToString();
                                ws.Cells[rowIndex, columnIndex++].Value = dr[9].ToString();
                            }
                            Byte[] bin = p.GetAsByteArray();
                            File.WriteAllBytes(filePath, bin);
                        }
                        MessageBox.Show("Đã xuất file Excel thành công!");
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message);
                    }
                    break;
                case MessageBoxResult.No:
                    break;
            }
        }

        private void btn_export_hoadon_Click_1(object sender, RoutedEventArgs e)
        {
            if (cb_status_hoadon.Text.Equals("Hóa đơn đã thanh toán"))
            {
                Export_hoadon(cb_status_hoadon.Text, "Đã thanh toán");
            }
            else if (cb_status_hoadon.Text.Equals("Hóa đơn chưa thanh toán"))
            {
                Export_hoadon(cb_status_hoadon.Text, "Chưa thanh toán");
            }
            else if (cb_status_hoadon.Text.Equals("Tất cả các hóa đơn"))
            {
                Export_hoadon(cb_status_hoadon.Text, "");
            }
        }

        private void btn_inhoadon_Click(object sender, RoutedEventArgs e)
        {
            if (checkform_hoadon() == true)
            {
                MessageBox.Show("Bạn chưa chọn hóa đơn!");
                return;
            }
            Form1 form_inhoadon = new Form1();
            form_inhoadon.setData(mahd_hoadon.Text, macongto_hoadon.Text, cb_list_makh.Text, tenkh_hoadon.Text, thang_hoadon.Text, chisocu_hoadon.Text, chisomoi_hoadon.Text, thanhtien_hoadon.Text, cb_status_thanhtoan.Text, inputemail_hoadon.Text);
            form_inhoadon.ShowDialog();
        }
    }
}