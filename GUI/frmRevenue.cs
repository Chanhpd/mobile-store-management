using DAL;
using DocumentFormat.OpenXml;
using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Spreadsheet;
using MySql.Data.MySqlClient;
using System;
using System.Data;
using System.Linq;
using System.Windows.Forms;

namespace GUI
{
    public partial class frmRevenue : Form
    {

        public frmRevenue()
        {
            InitializeComponent();
        }

        private void dateTimePicker2_ValueChanged(object sender, EventArgs e)
        {

        }

        private void guna2DateTimePicker1_ValueChanged(object sender, EventArgs e)
        {

        }

        private void frmRevenue_Load(object sender, EventArgs e)
        {

        }
        public void ExportToExcel(DataGridView dataGridView)
        {
            // Tạo file excel mới
            DateTime ngayBD = guna2DateTimePicker1.Value;
            DateTime ngayKT = guna2DateTimePicker2.Value;
            // Định dạng lại theo chuẩn của SQL Server
            string ngayBDSQL = ngayBD.ToString("yyyy-MM-dd");
            string ngayKTSQL = ngayKT.ToString("yyyy-MM-dd");
            string fileName = "DanhSachDonHang.xlsx";
            SpreadsheetDocument document = SpreadsheetDocument.Create(fileName, SpreadsheetDocumentType.Workbook);

            // Tạo các sheet và cấu hình file excel
            WorkbookPart workbookPart = document.AddWorkbookPart();
            workbookPart.Workbook = new Workbook();

            WorksheetPart worksheetPart = workbookPart.AddNewPart<WorksheetPart>();
            worksheetPart.Worksheet = new Worksheet(new SheetData());

            Sheets sheets = workbookPart.Workbook.AppendChild(new Sheets());
            Sheet sheet = new Sheet() { Id = workbookPart.GetIdOfPart(worksheetPart), SheetId = 1, Name = "Sheet 1" };
            sheets.Append(sheet);

            // Đổ data từ datagridview vào file excel
            SheetData sheetData = worksheetPart.Worksheet.Elements<SheetData>().First();

            Row headerRow = new Row();

            foreach (DataGridViewColumn column in dataGridView.Columns)
            {
                headerRow.AppendChild(CreateCell(column.HeaderText));
            }

            sheetData.AppendChild(headerRow);

            foreach (DataGridViewRow dataGridViewRow in dataGridView.Rows)
            {
                Row row = new Row();

                foreach (DataGridViewCell cell in dataGridViewRow.Cells)
                {
                    string cellValue = cell.Value != null ? cell.Value.ToString() : "";
                    row.AppendChild(CreateCell(cellValue));
                }
                sheetData.AppendChild(row);
            }
            Row totalRow = new Row();
            totalRow.AppendChild(CreateCell("Doanh thu " + ngayBDSQL + " -> " + ngayKTSQL + ""));
            totalRow.AppendChild(CreateCell(label6.Text));
            sheetData.Append(totalRow);

            worksheetPart.Worksheet.Save();

            workbookPart.Workbook.Save();

            document.Close();

            // Mở file excel sau khi export thành công
            System.Diagnostics.Process.Start(fileName);
        }

        // Hàm tạo một cell với giá trị được cung cấp
        private Cell CreateCell(string text)
        {
            if (int.TryParse(text, out int result))
            {
                return new Cell(new CellValue(result.ToString())) { DataType = CellValues.Number };
            }
            else
            {
                return new Cell(new CellValue(text ?? string.Empty)) { DataType = CellValues.String };
            }
        }
        private void guna2GradientButton2_Click(object sender, EventArgs e)
        {
            // Lấy giá trị ngày tháng từ DateTimePicker
            DateTime ngayBD = guna2DateTimePicker1.Value;
            DateTime ngayKT = guna2DateTimePicker2.Value;
            // Định dạng lại theo chuẩn của SQL Server
            string ngayBDSQL = ngayBD.ToString("yyyy-MM-dd");
            string ngayKTSQL = ngayKT.ToString("yyyy-MM-dd");
            string query = "SELECT HDX.HDX_Ma,KH.TEN_KH, KH.DIACHI,KH.SDT, HDX.HDX_NgayLap, HDX.HDX_TongTien\r\n" +
                "FROM HOA_DON_XUAT HDX\r\nJOIN KHACH_HANG KH " +
                "ON HDX.MA_KH = KH.MA_KH\r\nWHERE HDX.HDX_NgayLap BETWEEN '" + ngayBDSQL + "' AND '" + ngayKTSQL + "'";
            Console.WriteLine(query);
            //MessageBox.Show(query);
            MySqlConnection conn = new MySqlConnection(DbConnect.stringConnect);
            MySqlDataAdapter dt = new MySqlDataAdapter(query, conn);
            DataSet dase = new DataSet();
            dt.Fill(dase, "a");
            dtgvRevenue.DataSource = dase;
            dtgvRevenue.DataMember = "a";

            // Tính tổng doanh thu
            DataTable dtt = new DataTable();
            using (MySqlConnection connection = new MySqlConnection(DbConnect.stringConnect))
            {
                MySqlCommand command = new MySqlCommand(query, connection);
                MySqlDataAdapter adapter = new MySqlDataAdapter(command);
                adapter.Fill(dtt);
            }
            double tongDoanhThu = 0;
            foreach (DataRow row in dtt.Rows)
            {
                double giaTri;
                if (double.TryParse(row["HDX_TongTien"].ToString(), out giaTri))
                {
                    tongDoanhThu += giaTri;
                }
            }
            label6.Text = tongDoanhThu.ToString();

        }

        private void guna2GradientButton1_Click(object sender, EventArgs e)
        {
            if (label6.Text == ".")
            {
                MessageBox.Show("Vui lòng thống kê trước khi lập danh sách");
            }
            else
            {
                ExportToExcel(dtgvRevenue);
            }
        }

        private void dtgvRevenue_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }
    }
}
