using Microsoft.SqlServer.Server;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Net.Mail;
using System.Net;
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
using Microsoft.Win32;
using OfficeOpenXml;
using System.IO;
using System.Xml.Linq;
using iTextSharp.text.pdf;
using iTextSharp.text;

namespace App_tien_dien
{
    /// <summary>
    /// Interaction logic for Customer.xaml
    /// </summary>
    public partial class Customer : Window
    {
        public Customer()
        {
            InitializeComponent();
            Load_page_infor();
            Load_page_hoadon();
            set_cb_don_gia();
            set_cb_status_hoadon();
        }
        string sqlConnect = @"Data Source=DESKTOP-173K518;Initial Catalog=BTL_tien_dien;User ID=sa;Password=admin";
        string Makh = FormLoginCustomer.getMakh();

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
        private void Load_page_infor()
        {
            string Makh = FormLoginCustomer.getMakh();
            DataTable table = new DataTable();
            try
            {
                SqlConnection conn = new SqlConnection(sqlConnect);
                conn.Open();
                string sqlSelect = "SELECT * FROM tablecustomer WHERE makh = @makh";
                SqlDataAdapter adapter = new SqlDataAdapter();
                adapter.SelectCommand = new SqlCommand(sqlSelect, conn);
                adapter.SelectCommand.Parameters.AddWithValue("makh", Makh);
                adapter.Fill(table);
                conn.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }

            makh_infor.Text = table.Rows[0]["makh"].ToString();
            hoten_infor.Text = table.Rows[0]["tenkh"].ToString();
            sodt_infor.Text = table.Rows[0]["sodt"].ToString();
            email_infor.Text = table.Rows[0]["email"].ToString();
            diachi_infor.Text = table.Rows[0]["diachi"].ToString();
            macongto_infor.Text = table.Rows[0]["macongto"].ToString();
        }

        private bool CheckValid()
        {
            if (string.IsNullOrWhiteSpace(mahd_hoadon.Text) || string.IsNullOrWhiteSpace(macongto_hoadon.Text) || string.IsNullOrWhiteSpace(makh_hoadon.Text) || string.IsNullOrWhiteSpace(tenkh_hoadon.Text) || string.IsNullOrWhiteSpace(thang_hoadon.Text) || string.IsNullOrWhiteSpace(chisocu_hoadon.Text) || string.IsNullOrWhiteSpace(chisomoi_hoadon.Text) || string.IsNullOrWhiteSpace(thanhtien_hoadon.Text) || string.IsNullOrWhiteSpace(status_thanhtoan.Text) || string.IsNullOrWhiteSpace(inputemail_hoadon.Text))
            {
                return false;
            }
            else
            {
                return true;
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

        private void Load_page_hoadon()
        {
            try
            {
                SqlConnection conn = new SqlConnection(sqlConnect);
                conn.Open();
                string sqlSelect = "SELECT * FROM hoadon WHERE makh = @Makh";
                SqlCommand cmd = new SqlCommand(sqlSelect, conn);
                cmd.Parameters.AddWithValue("@Makh", Makh);
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

        private void resetForm()
        {
            mahd_hoadon.Text = "";
            tenkh_hoadon.Text = "";
            macongto_hoadon.Text = "";
            makh_hoadon.Text = "";
            cb_don_gia.Text = cb_don_gia.Items[0].ToString();
            thang_hoadon.Text = "";
            chisocu_hoadon.Text = "";
            chisomoi_hoadon.Text = "";
            thanhtien_hoadon.Text = "";
            inputemail_hoadon.Text = "";
            radio_mahd.IsChecked = false;
            radio_chuathanhtoan.IsChecked = false;
            radio_dathanhtoan.IsChecked = false;
            status_thanhtoan.Text = "";
            cb_status_hoadon.Text = cb_status_hoadon.Items[2].ToString();
            Load_page_hoadon();
        }

        private void btn_reset_Click(object sender, RoutedEventArgs e)
        {
            resetForm();
        }

        private void get_hoadon()
        {
            try
            {
                SqlConnection conn = new SqlConnection(@"Data Source=DESKTOP-173K518;Initial Catalog=BTL_tien_dien;Persist Security Info=True;User ID=sa;Password=admin");
                conn.Open();
                string sqlSelect = "SELECT * FROM hoadon WHERE mahoadon = @mahd";
                SqlCommand cmd = new SqlCommand(sqlSelect, conn);
                cmd.Parameters.AddWithValue("@mahd", mahd_hoadon.Text);
                SqlDataReader reader = cmd.ExecuteReader();
                DataTable dt = new DataTable();
                dt.Load(reader);
                conn.Close();
                string mahd = mahd_hoadon.Text;
                string trangthai = status_thanhtoan.Text;
                string filePath = @"D:\App_Tien_Dien\App_tien_dien\PDF\" + mahd + "_Da_Thanh_Toan.pdf";
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

        private void send_email()
        {
            string message_email = "Xin kính chào quý khách: " + tenkh_hoadon.Text;

            MailAddress myemail = new MailAddress("20010974@st.phenikaa-uni.edu.vn", "Thông tin hóa đơn tiền điện");
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
            string file_pdf_path = @"D:\App_Tien_Dien\App_tien_dien\PDF\" + mahd_hoadon.Text + "_Da_Thanh_Toan.pdf";
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

        private void btn_thanhtoan_Click(object sender, RoutedEventArgs e)
        {
            if (CheckValid() == false)
            {
                MessageBox.Show("Chưa đủ trường thông tin để thanh toán");
                return;
            }

            if (status_thanhtoan.Text.Equals("Chưa thanh toán") == true)
            {
                MessageBoxResult result = MessageBox.Show("Bạn có muốn thanh toán hóa đơn này không?", "Xác nhận thanh toán", MessageBoxButton.YesNo);
                switch (result)
                {
                    case MessageBoxResult.Yes:
                        try
                        {
                            SqlConnection conn = new SqlConnection(sqlConnect);
                            conn.Open();
                            string sqlSelect = "UPDATE hoadon SET status_thanhtoan = @status WHERE makh = @Makh AND mahoadon = @Mahd";
                            SqlCommand cmd = new SqlCommand(sqlSelect, conn);
                            cmd.Parameters.AddWithValue("@status", "Đã thanh toán");
                            cmd.Parameters.AddWithValue("@Makh", Makh);
                            cmd.Parameters.AddWithValue("@Mahd", mahd_hoadon.Text);
                            cmd.ExecuteNonQuery();
                            MessageBox.Show("Thanh toán hóa đơn thành công!");
                            get_hoadon();
                            send_email();
                            resetForm();
                            conn.Close();
                        }
                        catch(Exception ex)
                        {
                            MessageBox.Show(ex.Message);
                        }
                        break;
                    case MessageBoxResult.No:
                        break;
                }
            }
            else
            {
                MessageBox.Show("Quý khách đã thanh toán hóa đơn này rồi!");
                return;
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
                    makh_hoadon.Text = dtrv[2].ToString();
                    tenkh_hoadon.Text = dtrv[3].ToString();
                    thang_hoadon.Text = dtrv[4].ToString();
                    chisocu_hoadon.Text = dtrv[5].ToString();
                    chisomoi_hoadon.Text = dtrv[6].ToString();
                    thanhtien_hoadon.Text = dtrv[7].ToString();
                    status_thanhtoan.Text = dtrv[8].ToString();
                    inputemail_hoadon.Text = dtrv[9].ToString();
                }
            }
        }

        private void btn_logout_infor_Click(object sender, RoutedEventArgs e)
        {
            MessageBoxResult result = MessageBox.Show("Bạn có muốn đăng xuất không?", "Xác nhận đăng xuất", MessageBoxButton.YesNo);
            switch(result)
            {
                case MessageBoxResult.Yes:
                    FormLoginCustomer formLoginCustomer = new FormLoginCustomer();
                    this.Close();
                    formLoginCustomer.ShowDialog();
                    break;
                case MessageBoxResult.No:
                    break;
            }
        }

        private void btn_logout_hoadon_Click(object sender, RoutedEventArgs e)
        {
            MessageBoxResult result = MessageBox.Show("Bạn có muốn đăng xuất không?", "Xác nhận đăng xuất", MessageBoxButton.YesNo);
            switch (result)
            {
                case MessageBoxResult.Yes:
                    FormLoginCustomer formLoginCustomer = new FormLoginCustomer();
                    this.Close();
                    formLoginCustomer.ShowDialog();
                    break;
                case MessageBoxResult.No:
                    break;
            }
        }

        private DataTable getDatatable_HoaDonChuathanhtoan()
        {
            try
            {
                SqlConnection conn = new SqlConnection(sqlConnect);
                conn.Open();
                string sqlSelect = "SELECT * FROM hoadon WHERE makh = @makh AND status_thanhtoan = @status_thanhtoan";
                SqlCommand cmd = new SqlCommand(sqlSelect, conn);
                cmd.Parameters.AddWithValue("@makh", makh_infor.Text);
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
                string sqlSelect = "SELECT * FROM hoadon WHERE makh = @makh AND status_thanhtoan = @status_thanhtoan";
                SqlCommand cmd = new SqlCommand(sqlSelect, conn);
                cmd.Parameters.AddWithValue("@makh", makh_infor.Text);
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
                string sqlSelect = "SELECT * FROM hoadon WHERE makh = @makh";
                SqlCommand cmd = new SqlCommand(sqlSelect, conn);
                cmd.Parameters.AddWithValue("@makh", makh_infor.Text);
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

        private void btn_export_hoadon_Click(object sender, RoutedEventArgs e)
        {
            if (cb_status_hoadon.Text.Equals("Hóa đơn đã thanh toán"))
            {
                string question = "Bạn có muốn xuất danh sách " + cb_status_hoadon.Text + " hay không?";
                MessageBoxResult result = MessageBox.Show(question, "Xác nhận đăng xuất", MessageBoxButton.YesNo);
                switch (result)
                {
                    case MessageBoxResult.Yes:
                        string filePath = "";
                        SaveFileDialog fileDialog = new SaveFileDialog();
                        fileDialog.Filter = "Excel | *.xlsx | Excel 2019 | *.xls";
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
                                p.Workbook.Properties.Author = "Customer";
                                p.Workbook.Properties.Title = "Danh sách khách hàng";
                                p.Workbook.Worksheets.Add("Test Sheet");

                                ExcelWorksheet ws = p.Workbook.Worksheets[0];
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
                                ws.Cells[1, 1].Value = "Danh sách " + cb_status_hoadon.Text;
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
                                dt = getDatatable_HoaDonDathanhtoan();

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
            else if (cb_status_hoadon.Text.Equals("Hóa đơn chưa thanh toán"))
            {
                string question = "Bạn có muốn xuất danh sách " + cb_status_hoadon.Text + " hay không?";
                MessageBoxResult result = MessageBox.Show(question, "Xác nhận đăng xuất", MessageBoxButton.YesNo);
                switch (result)
                {
                    case MessageBoxResult.Yes:
                        string filePath = "";
                        SaveFileDialog fileDialog = new SaveFileDialog();
                        fileDialog.Filter = "Excel | *.xlsx | Excel 2019 | *.xls";
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
                                p.Workbook.Properties.Author = "Customer";
                                p.Workbook.Properties.Title = "Danh sách các hóa đơn " + cb_status_hoadon.Text;
                                p.Workbook.Worksheets.Add("Test Sheet");

                                ExcelWorksheet ws = p.Workbook.Worksheets[0];
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
                                ws.Cells[1, 1].Value = "Danh sách " + cb_status_hoadon.Text;
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
                                dt = getDatatable_HoaDonChuathanhtoan();

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
            else if (cb_status_hoadon.Text.Equals("Tất cả các hóa đơn"))
            {
                string question = "Bạn có muốn xuất danh sách " + cb_status_hoadon.Text + " hay không?";
                MessageBoxResult result = MessageBox.Show(question, "Xác nhận đăng xuất", MessageBoxButton.YesNo);
                switch (result)
                {
                    case MessageBoxResult.Yes:
                        string filePath = "";
                        SaveFileDialog fileDialog = new SaveFileDialog();
                        fileDialog.Filter = "Excel | *.xlsx | Excel 2019 | *.xls";
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
                                p.Workbook.Properties.Author = "Customer";
                                p.Workbook.Properties.Title = "Danh sách khách hàng " + cb_status_hoadon.Text;
                                p.Workbook.Worksheets.Add("Test Sheet");

                                ExcelWorksheet ws = p.Workbook.Worksheets[0];
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
                                ws.Cells[1, 1].Value = "Danh sách " + cb_status_hoadon.Text;
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
                                dt = getDatatable_all_hoadon();

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
        }
    }
}