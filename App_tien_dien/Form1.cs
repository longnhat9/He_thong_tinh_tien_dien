using Microsoft.Reporting.WinForms;
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
namespace App_tien_dien
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        string mahdon; string macongto; string makh; string tenkh; string thang; string chisocu; string chisomoi; string thanhtien; string status_thanhtoan; string email;

        public void setData(string mahd, string macongto, string makh, string tenkh, string thang, string chisocu, string chisomoi, string thanhtien, string status_thanhtoan, string email)
        {
            this.mahdon = mahd;
            this.macongto = macongto;
            this.makh = makh;
            this.tenkh = tenkh;
            this.thang = thang;
            this.chisocu = chisocu;
            this.chisomoi = chisomoi;
            this.thang = thang;
            this.thanhtien = thanhtien;
            this.status_thanhtoan = status_thanhtoan;
            this.email = email;
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            string strconnect = @"Data Source=DESKTOP-173K518;Initial Catalog=BTL_tien_dien;User ID=sa;Password=admin";
            DataSet ds = new DataSet();
            try
            {
                SqlConnection conn = new SqlConnection(strconnect);
                conn.Open();
                string sql = "SELECT * FROM hoadon WHERE mahoadon = @mahd";
                SqlDataAdapter dataAdapter = new SqlDataAdapter(sql, conn);
                dataAdapter.SelectCommand.Parameters.AddWithValue("@mahd", mahdon);
                dataAdapter.Fill(ds, "ThongTinHoaDon");
                conn.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }

            this.reportViewer.LocalReport.ReportEmbeddedResource = "App_tien_dien.Report_hoadon.rdlc";
            ReportDataSource rds = new ReportDataSource();
            rds.Name = "DataSet_hoadon";
            rds.Value = ds.Tables["ThongTinHoaDon"];
            this.reportViewer.LocalReport.DataSources.Add(rds);
            this.reportViewer.RefreshReport();
        }
    }
}