using BUS;
using DAL;
using DocumentFormat.OpenXml;
using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Spreadsheet;
using MySql.Data.MySqlClient;
using System;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace GUI
{
    public partial class frmLoaiVatTu : Form
    {
        BUS_LoaiHang bus_LoaiHang = new BUS_LoaiHang();
        public MySqlConnection conn;
        public void Connnect()
        {
            conn = new MySqlConnection();

            conn.ConnectionString = DbConnect.stringConnect;
            conn.Open();
        }
        public frmLoaiVatTu()
        {
            InitializeComponent();
        }

        private void frmLoaiVatTu_Load(object sender, EventArgs e)
        {
            foreach (DataGridViewColumn item in dtgvLoai.Columns)
                item.DividerWidth = 1;
            dtgvLoai.DefaultCellStyle.Font = new System.Drawing.Font("Arial", 10);
            dtgvLoai.ColumnHeadersDefaultCellStyle.Font = new System.Drawing.Font("Arial", 11, FontStyle.Bold);
            dtgvLoai.RowTemplate.Height = 30;
            bus_LoaiHang.loadTableLoaiHang(dtgvLoai);

        }
        public void ExportToExcel(DataGridView dataGridView)
        {

            // Định dạng lại theo chuẩn của SQL Server
            string fileName = "DanhSachKhachhang.xlsx";
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
        private void guna2GradientButton1_Click(object sender, EventArgs e)
        {
            ExportToExcel(dtgvLoai);
        }

        private void btnInsert_Click(object sender, EventArgs e)
        {

            if (txtName.Text == "" || txtMoTa.Text == "")
            {
                MessageBox.Show("Xin hãy vui lòng nhập đầy đủ thông tin");

            }
            else
            {
                if (bus_LoaiHang.insertLoaiHang(txtID.Text, txtName.Text, txtMoTa.Text))
                {
                    bus_LoaiHang.loadTableLoaiHang(dtgvLoai);
                    MessageBox.Show("Thêm danh mục thành công");
                    txtID.Text = "";
                    txtName.Text = "";
                    txtMoTa.Text = "";
                }
                else
                {
                    MessageBox.Show("Thêm danh mục thất bại");
                }
            }

        }

        private void btnRefresh_Click(object sender, EventArgs e)
        {

            txtID.Text = "";
            txtName.Text = "";
            txtMoTa.Text = "";
        }

        private void label5_Click(object sender, EventArgs e)
        {

        }

        private void txtAddress_TextChanged(object sender, EventArgs e)
        {
        }

        private void dtgvLoai_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            //DAL_LoaiHang loaiHang = new DAL_LoaiHang();
            txtID.Text = dtgvLoai.CurrentRow.Cells[0].Value.ToString();
            txtName.Text = dtgvLoai.CurrentRow.Cells[1].Value.ToString();
            txtMoTa.Text = dtgvLoai.CurrentRow.Cells[2].Value.ToString();

        }

        private void btnUpdate_Click(object sender, EventArgs e)
        {
            if (txtID.Text != "")
            {
                if (bus_LoaiHang.updateLoaiHang(txtID.Text, txtName.Text, txtMoTa.Text))
                {
                    bus_LoaiHang.loadTableLoaiHang(dtgvLoai);
                    MessageBox.Show("Cập nhật danh mục thành công");
                    txtName.Text = "";
                    txtMoTa.Text = "";
                }
                else
                {
                    MessageBox.Show("Cập nhật danh mục thất bại");
                }
            }
            else
            {
                MessageBox.Show("Vui lòng chọn sản phẩm muốn thay đổi");
            }
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            if (txtID.Text == "")
            {
                MessageBox.Show("Vui lòng chọn sản phẩm muốn xóa");
            }
            else
            {
                bus_LoaiHang.deleteLoaiHang(txtID.Text);
                bus_LoaiHang.loadTableLoaiHang(dtgvLoai);
                txtID.Text = "";
                txtName.Text = "";
                txtMoTa.Text = "";
            }
        }

        private void btnSearch_Click(object sender, EventArgs e)
        {
            //DAL_LoaiHang loaiHang = new DAL_LoaiHang();
            //string sql = "select * from LOAI_HANG where LH_Ten like '%" + txtFind.Text + "%'";
            //loaiHang.HienThiDG(sql, dtgvLoai, conn);

            bus_LoaiHang.loadTableLoaiHang(dtgvLoai, txtFind.Text);
        }
    }
}
