using BUS;
using DAL;
using DocumentFormat.OpenXml;
using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Spreadsheet;
using DTO;
using MySql.Data.MySqlClient;
using System;
using System.Data;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Windows.Forms;

namespace GUI
{
    public partial class frmStaffs : Form
    {
        DAL_TKHT daltkht = new DAL_TKHT();
        BUS_TKHT bustkht = new BUS_TKHT();
        DTO_TKHT dtotkht;

        public frmStaffs()
        {
            InitializeComponent();
        }
        private void MessBox(string message, bool isError = false)
        {
            if (isError)
                MessageBox.Show(message, "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Error);
            else
                MessageBox.Show(message, "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }
        private void LoadGridView()
        {
            dtgvStaff.Columns[0].HeaderText = "Email nhân viên";
            dtgvStaff.Columns[1].HeaderText = "Họ tên";
            dtgvStaff.Columns[2].HeaderText = "Tên phân quyền";
            dtgvStaff.Columns[3].HeaderText = "Địa chỉ";
            dtgvStaff.Columns[4].HeaderText = "Số điện thoại";
            dtgvStaff.Columns[5].HeaderText = "Giới tính";
            dtgvStaff.Columns[6].HeaderText = "Ngày sinh";

            foreach (DataGridViewColumn item in dtgvStaff.Columns)
                item.DividerWidth = 1;
            dtgvStaff.DefaultCellStyle.Font = new System.Drawing.Font("Arial", 10);
            dtgvStaff.ColumnHeadersDefaultCellStyle.Font = new System.Drawing.Font("Arial", 11, FontStyle.Bold);
            dtgvStaff.RowTemplate.Height = 30;
        }
        private void LoadComboBoxPhanQuyen()
        {
            DataTable dt = daltkht.PhanQuyen();
            cbPhanQuyen.DisplayMember = "PQ_Ten";
            cbPhanQuyen.ValueMember = "PQ_Ma";
            cbPhanQuyen.DataSource = dt;
        }
        private void frmStaffs_Load(object sender, EventArgs e)
        {
            dtgvStaff.DataSource = bustkht.ListStaff();
            LoadGridView();
            LoadComboBoxPhanQuyen();
            cbPhanQuyen.SelectedIndex = -1;
        }

        private void btnInsert_Click(object sender, EventArgs e)
        {
            // Kiểm tra dữ liệu nhập vào
            if (string.IsNullOrWhiteSpace(txtEmail.Text) ||
                string.IsNullOrWhiteSpace(txtAddress.Text) ||
                string.IsNullOrWhiteSpace(txtName.Text) ||
                string.IsNullOrWhiteSpace(txtPassword.Text) ||
                string.IsNullOrWhiteSpace(txtPhone.Text) ||
                string.IsNullOrWhiteSpace(txtNgaySinh.Text) ||
                cbPhanQuyen.SelectedIndex == -1 ||
                (!checkMale.Checked && !checkFemale.Checked))
            {
                MessBox("Vui lòng nhập đầy đủ thông tin và chọn giới tính!", true);
                return;
            }

            // Xử lý định dạng ngày sinh
            DateTime ngaySinh;
            string ngaySinhString = txtNgaySinh.Text;
            string ngaySinhFormat = "dd/MM/yyyy";
            try
            {
                ngaySinh = DateTime.ParseExact(ngaySinhString, ngaySinhFormat, CultureInfo.InvariantCulture);
            }
            catch
            {
                MessBox("Ngày sinh không đúng định dạng dd/MM/yyyy!", true);
                txtNgaySinh.Focus();
                return;
            }

            // Kiểm tra giới tính
            bool gioiTinh = checkMale.Checked ? true : false;

            dtotkht = new DTO_TKHT(txtEmail.Text, txtName.Text, cbPhanQuyen.SelectedValue.ToString(), txtPassword.Text, txtAddress.Text, txtPhone.Text, gioiTinh, ngaySinh);

            if (bustkht.InsertStaff(dtotkht))
            {
                LoadGridView();
                dtgvStaff.DataSource = bustkht.ListStaff();
            }

        }

        private void btnUpdate_Click(object sender, EventArgs e)
        {
            DateTime ngaySinh;

            string ngaySinhString = txtNgaySinh.Text;
            string ngaySinhFormat = "dd/MM/yyyy"; // Thay đổi định dạng theo ý muốn

            ngaySinh = DateTime.ParseExact(ngaySinhString, ngaySinhFormat, CultureInfo.InvariantCulture);

            string ngaySinhFormatted = ngaySinh.ToString("yyyy-MM-dd");

            bool gioitinh = true;
            MySqlConnection conn = new MySqlConnection(DbConnect.stringConnect);

            Console.WriteLine(ngaySinh);
            if (txtEmail.Text != "" && txtName.Text != "" && txtAddress.Text != "" && txtPhone.Text != "")
            {
                if (MessageBox.Show("Bạn có chắc muốn sửa?", "Xác nhận", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    if (gioitinh == true)
                    {
                        checkMale.Checked = true;
                    }
                    else
                    {
                        checkFemale.Checked = true;
                    }

                    dtotkht = new DTO_TKHT(txtEmail.Text, txtName.Text, cbPhanQuyen.SelectedValue.ToString(), txtPassword.Text, txtAddress.Text, txtPhone.Text, gioitinh, ngaySinh);

                    if (bustkht.UpdateStaff(dtotkht))
                    {
                        LoadGridView();
                        dtgvStaff.DataSource = bustkht.ListStaff();
                    }
                }
                else
                {
                    MessBox("Vui lòng kiểm tra lại thông tin thông tin", true);
                }
            }
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtEmail.Text))
            {
                MessBox("Vui lòng nhập email cần xóa!", true);
                return;
            }

            if (MessageBox.Show("Bạn có chắc muốn xóa?", "Xác nhận",
                MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                using (MySqlConnection conn = new MySqlConnection(DbConnect.stringConnect))
                {
                    string sql = "DELETE FROM TAI_KHOAN_HE_THONG WHERE TKHT_Email = @Email";
                    try
                    {
                        conn.Open();
                        using (MySqlCommand cmd = new MySqlCommand(sql, conn))
                        {
                            // Thêm tham số an toàn
                            cmd.Parameters.AddWithValue("@Email", txtEmail.Text.Trim());

                            int rowsAffected = cmd.ExecuteNonQuery();
                            if (rowsAffected > 0)
                            {
                                MessBox("Xóa thông tin nhân viên thành công!");
                                LoadGridView();
                                dtgvStaff.DataSource = bustkht.ListStaff();
                            }
                            else
                            {
                                MessBox("Không tìm thấy nhân viên với email này!", true);
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        MessBox($"Đã xảy ra lỗi: {ex.Message}", true);
                    }
                }
            }
        }


        private void dtgvStaff_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (dtgvStaff.Rows.Count > 0)
            {
                btnDelete.Enabled = true;
                btnUpdate.Enabled = true;
                txtEmail.ReadOnly = true;
                txtEmail.Text = dtgvStaff.CurrentRow.Cells[0].Value.ToString();
                txtName.Text = dtgvStaff.CurrentRow.Cells[1].Value.ToString();
                txtAddress.Text = dtgvStaff.CurrentRow.Cells[3].Value.ToString();
                txtPhone.Text = dtgvStaff.CurrentRow.Cells[4].Value.ToString();
                cbPhanQuyen.Text = dtgvStaff.CurrentRow.Cells[2].Value.ToString();
                DateTime ngaysinh = (DateTime)dtgvStaff.CurrentRow.Cells[6].Value;
                txtNgaySinh.Text = ngaysinh.ToString("dd/MM/yyyy");
                string gioitinh = dtgvStaff.CurrentRow.Cells[5].Value.ToString();
                txtPassword.Text = daltkht.GetFieldValues("Select TKHT_Password from TAI_KHOAN_HE_THONG where TKHT_Email = '" + txtEmail.Text + "'");
                if (gioitinh == "Nam")
                {
                    checkMale.Checked = true;
                }
                else
                {
                    checkFemale.Checked = true;
                }
            }
        }

        private void btnRefresh_Click(object sender, EventArgs e)
        {
            txtEmail.Text = "";
            txtName.Text = "";
            txtAddress.Text = "";
            txtNgaySinh.Text = "";
            txtPassword.Text = "";
            txtPhone.Text = "";
            checkMale.Checked = false;
            checkFemale.Checked = false;
            cbPhanQuyen.SelectedIndex = -1;
        }

        private void guna2GradientButton1_Click(object sender, EventArgs e)
        {
            ExportToExcel(dtgvStaff);
        }
        public void ExportToExcel(DataGridView dataGridView)
        {

            // Định dạng lại theo chuẩn của SQL Server
            string fileName = "dsnv.xlsx";
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

        private void btnSearch_Click(object sender, EventArgs e)
        {
            string name = txtSearchStaff.Text.ToString().Trim();


            dtgvStaff.DataSource = bustkht.SearchStaff(name);
        }

        private void dtgvStaff_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                DataGridViewRow row = dtgvStaff.Rows[e.RowIndex];

                string emailNhanVien = row.Cells[0].Value?.ToString();
                string hoTen = row.Cells[1].Value?.ToString();
                string tenPhanQuyen = row.Cells[2].Value?.ToString();
                string diaChi = row.Cells[3].Value?.ToString();
                string soDienThoai = row.Cells[4].Value?.ToString();
                string gioiTinh = row.Cells[5].Value?.ToString();
                string ngaySinh = row.Cells[6].Value?.ToString();

                Console.WriteLine("Email nhân viên: " + emailNhanVien);
                Console.WriteLine("Họ tên: " + hoTen);
                Console.WriteLine("Tên phân quyền: " + tenPhanQuyen);
                Console.WriteLine("Địa chỉ: " + diaChi);
                Console.WriteLine("Số điện thoại: " + soDienThoai);
                Console.WriteLine("Giới tính: " + gioiTinh);
                Console.WriteLine("Ngày sinh: " + ngaySinh);
            }
        }
    }
}
