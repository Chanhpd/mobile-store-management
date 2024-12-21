using BUS;
using DAL;
using DocumentFormat.OpenXml;
using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Spreadsheet;
using DTO;
using System;
using System.Data;
using System.Linq;
using System.Windows.Forms;

namespace GUI
{
    public partial class frmCustomers : Form
    {
        BUS_KhachHang buskhachhang = new BUS_KhachHang();
        DTO_KhachHang dtokhachhang = new DTO_KhachHang();
        public frmCustomers()
        {
            InitializeComponent();
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }
        private void LoadGridView()
        {
            dtgvCustomer.Columns[0].HeaderText = "Mã khách hàng";
            dtgvCustomer.Columns[1].HeaderText = "Tên khách hàng";
            dtgvCustomer.Columns[2].HeaderText = "Địa chỉ";
            dtgvCustomer.Columns[3].HeaderText = "Số điện thoại";
            foreach (DataGridViewColumn item in dtgvCustomer.Columns)
                item.DividerWidth = 1;
        }
        private void MessBox(string message, bool isError = false)
        {
            if (isError)
                MessageBox.Show(message, "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Error);
            else
                MessageBox.Show(message, "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }
        private void frmCustomers_Load(object sender, EventArgs e)
        {

            dtgvCustomer.DataSource = buskhachhang.ListCustomer();
            LoadGridView();
        }

        private void btnInsert_Click(object sender, EventArgs e)
        {
            if (txtID.Text != "" && txtName.Text != "" && txtAddress.Text != "" && txtPhone.Text != "")
            {
                dtokhachhang = new DTO_KhachHang
                (
                    txtID.Text,
                    txtName.Text,
                    txtAddress.Text,
                    txtPhone.Text
                );
                if (buskhachhang.InsertCustomer(dtokhachhang))
                {
                    dtgvCustomer.DataSource = buskhachhang.ListCustomer();
                    LoadGridView();
                    MessBox("Thêm khách hàng thành công");

                    txtID.Text = "";
                    txtAddress.Text = "";
                    txtName.Text = "";
                    txtPhone.Text = "";
                }

                else
                {
                    MessBox("Thêm khách hàng không thành công", true);
                }
            }
            else
            {
                MessBox("Vui lòng nhập nhập đầy đủ thông tin", true);
            }
        }

        private void btnRefresh_Click(object sender, EventArgs e)
        {
            // Chuyển đổi biến b sang dạng int và lưu vào một biến khác (ví dụ: intB)
            DAL_NuaMua nuamua = new DAL_NuaMua();
            txtID.Text = nuamua.CreateNewID("SELECT MAX(ma_kh) AS Largest_ma_kh FROM KHACH_HANG");
            txtName.Text = null;
            txtAddress.Text = null;
            txtPhone.Text = null;

        }

        private void dtgvCustomer_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (dtgvCustomer.Rows.Count > 0)
            {
                btnDelete.Enabled = true;
                btnUpdate.Enabled = true;
                txtID.ReadOnly = true;
                txtID.Text = dtgvCustomer.CurrentRow.Cells[0].Value.ToString();
                txtName.Text = dtgvCustomer.CurrentRow.Cells[1].Value.ToString();
                txtAddress.Text = dtgvCustomer.CurrentRow.Cells[2].Value.ToString();
                txtPhone.Text = dtgvCustomer.CurrentRow.Cells[3].Value.ToString();
            }
        }

        private void btnUpdate_Click(object sender, EventArgs e)
        {
            if (txtID.Text != "" && txtName.Text != "" && txtAddress.Text != "" && txtPhone.Text != "")
            {
                if (MessageBox.Show("Bạn có chắc muốn sửa?", "Xác nhận", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    dtokhachhang = new DTO_KhachHang
                    (
                        txtID.Text,
                        txtName.Text,
                        txtAddress.Text,
                        txtPhone.Text
                    );
                    if (buskhachhang.UpdateCustomer(dtokhachhang))
                    {
                        dtgvCustomer.DataSource = buskhachhang.ListCustomer();
                        LoadGridView();
                        MessBox("Sửa thông tin khách hàng thành công");
                    }
                    else
                        MessBox("Sửa thông tin khách hàng không thành công", true);
                }
                else
                {
                    MessBox("Vui lòng kiểm tra lại thông tin thông tin", true);
                }
            }
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            string ma = txtID.Text;
            if (MessageBox.Show("Bạn có chắc muốn xóa?", "Xác nhận", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                if (ma != null)
                {
                    //MessageBox.Show(ma);
                    buskhachhang.DeleteCustomer(ma);
                    dtgvCustomer.DataSource = buskhachhang.ListCustomer();
                    LoadGridView();

                    txtID.Text = "";
                    txtName.Text = null;
                    txtAddress.Text = null;
                    txtPhone.Text = null;
                    MessBox("Xóa khách hàng thành công");
                }
                else
                {
                    MessBox("Xóa không thành công", true);
                }
            }
        }

        private void btnSearch_Click(object sender, EventArgs e)
        {
            string kh = txtFind.Text;
            if (kh == "")
            {
                frmCustomers_Load(sender, e);
            }
            else
            {
                DataTable table = buskhachhang.SearchCustomer(kh);
                dtgvCustomer.DataSource = table;
            }
        }

        private void guna2GradientButton1_Click(object sender, EventArgs e)
        {
            ExportToExcel(dtgvCustomer);
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
    }
}
