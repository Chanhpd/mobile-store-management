using BUS;
using DAL;
using DTO;
using MySql.Data.MySqlClient;
using System;
using System.Data;
using System.Windows.Forms;
using COMExcel = Microsoft.Office.Interop.Excel;

namespace GUI
{
    public partial class frmExportBills : Form
    {

        BUS_ExportBill busExportBill = new BUS_ExportBill();
        DAL_ExportBills dalHDX = new DAL_ExportBills();
        //DTO_HoaDonXuat dtoHDX;
        private string role;
        private string email;
        public frmExportBills(string role, string email)
        {
            InitializeComponent();
            this.role = role;
            LoadComboBoxTKHT(role, email);
            this.email = email;
        }
        private void MessBox(string message, bool isError = false)
        {
            if (isError)
                MessageBox.Show(message, "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Error);
            else
                MessageBox.Show(message, "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }
        //private string CovertDateTime(string date)
        //{
        //    string[] elements = date.Split('/');
        //    string dt = string.Format("{0}/{1}/{2}", elements[0], elements[1], elements[2]);
        //    return dt;
        //}
        private void LoadComboBoxKhachHang()
        {
            DAL_KhachHang kh = new DAL_KhachHang();
            DataTable data = kh.GetCustomer();
            cbCustomer.DisplayMember = "TEN_KH";
            cbCustomer.ValueMember = "MA_KH";
            cbCustomer.DataSource = data;
        }
        private void LoadComboBoxTKHT(string role, string email)
        {
            string str;
            DAL_TKHT tkht = new DAL_TKHT();
            DataTable data = tkht.GetTKHT(role, email);
            cbPQNV.DisplayMember = "PQ_Ten";
            cbPQNV.ValueMember = "PQ_Ma";
            cbPQNV.DataSource = data;
            str = "Select TKHT_Email from TAI_KHOAN_HE_THONG where PQ_Ma ='" + role + "' and TKHT_Email = '" + email + "'";
            Console.WriteLine(str);
            txtStaff.Text = tkht.GetFieldValues(str);

        }
        private void LoadComboBoxVatTu()
        {
            DAL_HangHoa hanghoa = new DAL_HangHoa();
            DataTable data = hanghoa.GetVatTu();
            cbIDProduct.DisplayMember = "HH_Ten";
            cbIDProduct.ValueMember = "HH_Ma";
            cbIDProduct.DataSource = data;
        }
        private void LoadDataGV()
        {
            dtgvExportBill.Columns[0].HeaderText = "Mã hóa đơn";
            dtgvExportBill.Columns[1].HeaderText = "Tên khách hàng";
            dtgvExportBill.Columns[2].HeaderText = "Ngày lập";
            dtgvExportBill.Columns[3].HeaderText = "Số lượng";
            dtgvExportBill.Columns[4].HeaderText = "Thành tiền";

            foreach (DataGridViewColumn item in dtgvExportBill.Columns)
                item.DividerWidth = 1;
        }
        private void frmExportBills_Load(object sender, System.EventArgs e)
        {
            txtIDExprotBill.ReadOnly = true;
            btnInsert.Enabled = true;
            btnUpdate.Enabled = false;
            btnDelete.Enabled = true;
            btnPrint.Enabled = false;
            cbPQNV.Enabled = false;
            txtStaff.ReadOnly = true;
            txtIDCustomer.ReadOnly = true;
            txtAddress.ReadOnly = true;
            txtPhone.ReadOnly = true;
            txtIDProduct.ReadOnly = true;
            txtPrice.ReadOnly = true;
            txtTotalProduct.ReadOnly = true;
            txtTotalBill.ReadOnly = true;
            LoadComboBoxKhachHang();
            cbCustomer.SelectedIndex = -1;
            datepicker.Value = DateTime.Now;
            LoadComboBoxTKHT(role, email);
            LoadComboBoxVatTu();
            cbIDProduct.SelectedIndex = -1;
            //dtgvExportBill.DataSource = busExportBill.ListExportBill(txtIDExprotBill.Text);
            dtgvExportBill.DataSource = busExportBill.ListAllExportBills();
            LoadDataGV();

        }

        private void cbCustomer_SelectedIndexChanged(object sender, System.EventArgs e)
        {
            MySqlConnection connection = new MySqlConnection(DbConnect.stringConnect);
            if (cbCustomer.SelectedItem != null)
            {
                string ma = cbCustomer.SelectedValue.ToString();
                string query = "Select * from KHACH_HANG where MA_KH = @Ten";
                MySqlCommand cmd = new MySqlCommand(query, connection);
                cmd.Parameters.AddWithValue("@Ten", ma);
                connection.Open();
                MySqlDataReader reader = cmd.ExecuteReader();
                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        txtIDCustomer.Text = reader["MA_KH"].ToString();
                        txtAddress.Text = reader["DIACHI"].ToString();
                        txtPhone.Text = reader["SDT"].ToString();
                    }
                }
                else
                {
                    txtIDCustomer.Text = "";
                    txtAddress.Text = "";
                    txtPhone.Text = "";
                }
                reader.Close();
                connection.Close();

            }
            else
            {
                txtIDCustomer.Text = "";
                txtAddress.Text = "";
                txtPhone.Text = "";
            }
        }
        private void cbIDProduct_SelectedIndexChanged(object sender, System.EventArgs e)
        {
            MySqlConnection connection = new MySqlConnection(DbConnect.stringConnect);
            if (cbIDProduct.SelectedItem != null)
            {
                string ma = cbIDProduct.SelectedValue.ToString();
                string query = "Select * from HANGHOA where HH_Ma = @Ma";
                MySqlCommand cmd = new MySqlCommand(query, connection);
                cmd.Parameters.AddWithValue("@Ma", ma);
                connection.Open();
                MySqlDataReader reader = cmd.ExecuteReader();
                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        txtIDProduct.Text = reader["HH_Ma"].ToString();
                        txtIntro.Text = reader["HH_MoTa"].ToString();
                        txtPrice.Text = reader["HH_DonGia"].ToString();
                    }
                }
                else
                {
                    txtIDProduct.Text = "";
                    txtIntro.Text = "";
                    txtPrice.Text = "";
                }
                reader.Close();
                connection.Close();

            }
            else
            {
                txtIDProduct.Text = "";
                txtIntro.Text = "";
                txtPrice.Text = "";
            }
        }
        private void ResetValuesHangHoa()
        {
            cbIDProduct.Text = "";
            txtNumberProduct.Text = "";
            txtTotalProduct.Text = "0";

        }
        private void btnInsert_Click(object sender, EventArgs e)
        {
            if (cbCustomer.Text == "")
            {
                MessBox("Bạn phải chọn một khách hàng", true);
                cbCustomer.Focus();
                return;
            }
            if (cbIDProduct.Text == "")
            {
                MessBox("Bạn phải chọn một sản phẩm", true);
                cbIDProduct.Focus();
                return;
            }

            int Hdx_Ma = Convert.ToInt32(txtIDExprotBill.Text);

            int Ma_KH = Convert.ToInt32(txtIDCustomer.Text);
            string Tkht_Email = txtStaff.Text;
            string Hdx_NgayLap = datepicker.Value.ToString("yyyy-MM-dd");

            float TongTien = Convert.ToSingle(txtTotalBill.Text);

            DTO_HoaDonXuat hdx = new DTO_HoaDonXuat(Hdx_Ma, Ma_KH, Tkht_Email, Hdx_NgayLap, TongTien);

            int HH_Ma = Convert.ToInt32(txtIDProduct.Text);
            int SoLuongXuat = Convert.ToInt32(txtNumberProduct.Text);
            float DonGia = Convert.ToSingle(txtPrice.Text);
            float ThanhTien = Convert.ToSingle(txtTotalProduct.Text);

            DTO_CTHoaDonXuat ct_hdx = new DTO_CTHoaDonXuat(Hdx_Ma, HH_Ma, SoLuongXuat, DonGia);

            if (busExportBill.InsertExportBills(hdx, ct_hdx))
            {
                MessBox("Thêm hóa đơn thành công", false);
                //dtgvExportBill.DataSource = busExportBill.ListExportBill(txtIDExprotBill.Text);
                dtgvExportBill.DataSource = busExportBill.ListAllExportBills();
                LoadDataGV();
            }
            else
            {
                MessBox("Thêm hóa đơn thất bại", true);
            }



        }
        private void txtNumberProduct_TextChanged(object sender, EventArgs e)
        {
            try
            {
                // Kiểm tra giá trị trong txtPrice
                if (int.TryParse(txtPrice.Text, out int price))
                {
                    // Nếu txtNumberProduct không trống và là số hợp lệ
                    if (int.TryParse(txtNumberProduct.Text, out int quantity))
                    {
                        int total = quantity * price;
                        txtTotalProduct.Text = total.ToString();
                    }
                    else
                    {
                        // Nếu txtNumberProduct trống hoặc không hợp lệ, mặc định quantity = 0
                        txtTotalProduct.Text = (0 * price).ToString();
                    }
                }
                else
                {
                    // Xử lý khi txtPrice không hợp lệ
                    txtTotalProduct.Text = "0";
                }
            }
            catch (Exception ex)
            {
                // Log lỗi hoặc hiển thị thông báo lỗi nếu cần
                MessageBox.Show($"Đã xảy ra lỗi: {ex.Message}");
                txtTotalProduct.Text = "0";
            }
        }

        private void btnPrint_Click(object sender, EventArgs e)
        {
            // Khởi động chương trình Excel
            COMExcel.Application exApp = new COMExcel.Application();
            COMExcel.Workbook exBook; //Trong 1 chương trình Excel có nhiều Workbook
            COMExcel.Worksheet exSheet; //Trong 1 Workbook có nhiều Worksheet
            COMExcel.Range exRange;
            string sql;
            int hang = 0, cot = 0;
            DataTable tblThongtinHD, tblThongtinHang;
            exBook = exApp.Workbooks.Add(COMExcel.XlWBATemplate.xlWBATWorksheet);
            exSheet = exBook.Worksheets[1];
            // Định dạng chung
            exRange = exSheet.Cells[1, 1];
            exRange.Range["A1:Z300"].Font.Name = "Times new roman"; //Font chữ
            exRange.Range["A1:B3"].Font.Size = 10;
            exRange.Range["A1:B3"].Font.Bold = true;
            exRange.Range["A1:B3"].Font.ColorIndex = 5; //Màu xanh da trời
            exRange.Range["A1:A1"].ColumnWidth = 7;
            exRange.Range["B1:B1"].ColumnWidth = 15;
            exRange.Range["A1:B1"].MergeCells = true;
            exRange.Range["A1:B1"].HorizontalAlignment = COMExcel.XlHAlign.xlHAlignCenter;
            exRange.Range["A1:B1"].Value = "Hệ thống quản lý vật tư";
            exRange.Range["A2:B2"].MergeCells = true;
            exRange.Range["A2:B2"].HorizontalAlignment = COMExcel.XlHAlign.xlHAlignCenter;
            exRange.Range["A2:B2"].Value = "Cần Thơ";
            exRange.Range["A3:B3"].MergeCells = true;
            exRange.Range["A3:B3"].HorizontalAlignment = COMExcel.XlHAlign.xlHAlignCenter;
            exRange.Range["A3:B3"].Value = "Điện thoại: (04)38526419";
            exRange.Range["C1:E3"].Font.Size = 16;
            exRange.Range["C1:E3"].Font.Bold = true;
            exRange.Range["C1:E3"].Font.ColorIndex = 3; //Màu đỏ
            exRange.Range["C1:E3"].MergeCells = true;
            exRange.Range["C1:E3"].HorizontalAlignment = COMExcel.XlHAlign.xlHAlignCenter;
            exRange.Range["C1:E3"].VerticalAlignment = COMExcel.XlVAlign.xlVAlignCenter;
            exRange.Range["C1:E3"].Value = "HÓA ĐƠN BÁN";
            // Biểu diễn thông tin chung của hóa đơn bán
            sql = "SELECT a.HDX_Ma, a.HDX_NgayLap, a.HDX_TongTien, b.Ten_KH, b.DIACHI, b.SDT, c.TKHT_HoTen FROM HOA_DON_XUAT AS a, KHACH_HANG AS b, TAI_KHOAN_HE_THONG AS c WHERE a.HDX_Ma = '" + txtIDExprotBill.Text + "' AND a.Ma_KH = b.Ma_KH AND a.TKHT_Email = c.TKHT_Email";
            tblThongtinHD = dalHDX.GetDataToTable(sql);
            exRange.Range["B6:C9"].Font.Size = 12;
            exRange.Range["B5:B5"].Value = "Mã hóa đơn:";
            exRange.Range["C5:E5"].MergeCells = true;
            //exRange.Range["C5:E5"].Value = tblThongtinHD.Rows[0][0].ToString();
            if (tblThongtinHD.Rows.Count > 0)
            {
                exRange.Range["C5:E5"].Value = tblThongtinHD.Rows[0][0].ToString();
                // Other code accessing tblThongtinHD.Rows
            }
            else
            {
                MessageBox.Show("No data found for the given export bill ID.");
            }
            exRange.Range["B6:B6"].Value = "Khách hàng:";
            exRange.Range["C6:E6"].MergeCells = true;
            exRange.Range["C6:E6"].Value = tblThongtinHD.Rows[0][3].ToString();
            exRange.Range["B7:B7"].Value = "Địa chỉ:";
            exRange.Range["C7:E7"].MergeCells = true;
            exRange.Range["C7:E7"].Value = tblThongtinHD.Rows[0][4].ToString();
            exRange.Range["B8:B8"].Value = "Điện thoại:";
            exRange.Range["C8:E8"].MergeCells = true;
            exRange.Range["C8:E8"].Value = tblThongtinHD.Rows[0][5].ToString();
            //Lấy thông tin các mặt hàng
            sql = "SELECT b.HH_Ten, a.SoLuongXuat, b.HH_DonGia, a.ThanhTien " +
                  "FROM CHI_TIET_HD_XUAT AS a , HANGHOA AS b WHERE a.HDX_Ma = N'" +
                  txtIDExprotBill.Text + "' AND a.HH_Ma = b.HH_Ma";
            tblThongtinHang = dalHDX.GetDataToTable(sql);
            //Tạo dòng tiêu đề bảng
            exRange.Range["A11:F11"].Font.Bold = true;
            exRange.Range["A11:F11"].HorizontalAlignment = COMExcel.XlHAlign.xlHAlignCenter;
            exRange.Range["C11:F11"].ColumnWidth = 12;
            exRange.Range["A11:A11"].Value = "STT";
            exRange.Range["B11:B11"].Value = "Tên hàng";
            exRange.Range["C11:C11"].Value = "Số lượng";
            exRange.Range["D11:D11"].Value = "Đơn giá";
            exRange.Range["E11:E11"].Value = "Thành tiền";
            for (hang = 0; hang < tblThongtinHang.Rows.Count; hang++)
            {
                //Điền số thứ tự vào cột 1 từ dòng 12
                exSheet.Cells[1][hang + 12] = hang + 1;
                for (cot = 0; cot < tblThongtinHang.Columns.Count; cot++)
                //Điền thông tin hàng từ cột thứ 2, dòng 12
                {
                    exSheet.Cells[cot + 2][hang + 12] = tblThongtinHang.Rows[hang][cot].ToString();
                    if (cot == 3) exSheet.Cells[cot + 2][hang + 12] = tblThongtinHang.Rows[hang][cot].ToString();
                }
            }
            exRange = exSheet.Cells[cot][hang + 14];
            exRange.Font.Bold = true;
            exRange.Value2 = "Tổng tiền:";
            exRange = exSheet.Cells[cot + 1][hang + 14];
            exRange.Font.Bold = true;
            exRange.Value2 = tblThongtinHD.Rows[0][2].ToString();
            exRange = exSheet.Cells[1][hang + 15]; //Ô A1 
            exRange.Range["A1:F1"].MergeCells = true;
            exRange.Range["A1:F1"].Font.Bold = true;
            exRange.Range["A1:F1"].Font.Italic = true;
            exRange.Range["A1:F1"].HorizontalAlignment = COMExcel.XlHAlign.xlHAlignRight;
            //exRange.Range["A1:F1"].Value = "Bằng chữ: " + dalHDX.ChuyenSoSangChu(tblThongtinHD.Rows[0][2].ToString());
            exRange = exSheet.Cells[4][hang + 17]; //Ô A1 
            exRange.Range["A1:C1"].MergeCells = true;
            exRange.Range["A1:C1"].Font.Italic = true;
            exRange.Range["A1:C1"].HorizontalAlignment = COMExcel.XlHAlign.xlHAlignCenter;
            DateTime d = Convert.ToDateTime(tblThongtinHD.Rows[0][1]);
            exRange.Range["A1:C1"].Value = "Hà Nội, ngày " + d.Day + " tháng " + d.Month + " năm " + d.Year;
            exRange.Range["A2:C2"].MergeCells = true;
            exRange.Range["A2:C2"].Font.Italic = true;
            exRange.Range["A2:C2"].HorizontalAlignment = COMExcel.XlHAlign.xlHAlignCenter;
            exRange.Range["A2:C2"].Value = "Nhân viên bán hàng";
            exRange.Range["A6:C6"].MergeCells = true;
            exRange.Range["A6:C6"].Font.Italic = true;
            exRange.Range["A6:C6"].HorizontalAlignment = COMExcel.XlHAlign.xlHAlignCenter;
            exRange.Range["A6:C6"].Value = tblThongtinHD.Rows[0][6];
            exSheet.Name = "Hóa đơn nhập";
            exApp.Visible = true;
        }

        private void dtgvExportBill_CellContentDoubleClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            var id = txtIDExprotBill.Text;
            if (id == "")
            {
                MessBox("Bạn phải chọn một hóa đơn để xóa", true);
                return;
            }

            busExportBill.deleteExportBill(id);
            dtgvExportBill.DataSource = busExportBill.ListAllExportBills();
        }

        private void btnSearch_Click(object sender, EventArgs e)
        {
            if (cbHDX_Ma.Text == "")
            {
                MessBox("Bạn phải chọn một hóa đơn để tìm kiếm", true);
                cbHDX_Ma.Focus();
                return;
            }
            txtIDExprotBill.Text = cbHDX_Ma.Text;
            LoadComboBoxKhachHang();
            LoadComboBoxTKHT(role, email);
            LoadComboBoxVatTu();
            string query = "Select HDX_NgayLap from HOA_DON_XUAT where HDX_Ma = '" + txtIDExprotBill.Text + "'";
            datepicker.Text = dalHDX.GetFieldValues(query);
            dtgvExportBill.DataSource = busExportBill.ListExportBill(txtIDExprotBill.Text);
            LoadDataGV();
        }

        private void cbHDX_Ma_DropDown(object sender, EventArgs e)
        {
            dalHDX.FillCombo("Select HDX_Ma from HOA_DON_XUAT", cbHDX_Ma, "HDX_Ma", "HDX_Ma");
            cbHDX_Ma.SelectedIndex = -1;
        }

        private void btnRefresh_Click(object sender, EventArgs e)
        {
            dtgvExportBill.ClearSelection();
            cbCustomer.SelectedIndex = -1;
            cbHDX_Ma.SelectedIndex = -1;
            cbIDProduct.SelectedIndex = -1;
            cbCustomer.SelectedIndex = -1;
            cbHDX_Ma.SelectedIndex = -1;
            cbIDProduct.SelectedIndex = -1;
            txtNumberProduct.Text = "";
            txtTotalBill.Text = "";
            txtTotalProduct.Text = "";

            txtIDExprotBill.Text = "";
            dtgvExportBill.DataSource = busExportBill.ListExportBill(txtIDExprotBill.Text);
            dtgvExportBill.ClearSelection();
        }

        private void cbHDX_Ma_SelectedValueChanged(object sender, EventArgs e)
        {
            txtIDExprotBill.Text = cbHDX_Ma.Text;
            LoadComboBoxKhachHang();
            LoadComboBoxTKHT(role, email);
            LoadComboBoxVatTu();
            string query = "Select HDX_NgayLap from HOA_DON_XUAT where HDX_Ma = '" + txtIDExprotBill.Text + "'";
            datepicker.Text = dalHDX.GetFieldValues(query);
            dtgvExportBill.DataSource = busExportBill.ListExportBill(txtIDExprotBill.Text);
            btnPrint.Enabled = true;
            LoadDataGV();
        }
        private void btnUpdate_Click(object sender, EventArgs e)
        {

        }
        private void backgroundWorker1_DoWork(object sender, System.ComponentModel.DoWorkEventArgs e)
        {

        }

        private void dtgvExportBill_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            datepicker.Value = DateTime.Parse(dtgvExportBill.CurrentRow.Cells["HDX_NgayLap"].Value.ToString());
            cbCustomer.Text = dtgvExportBill.CurrentRow.Cells["Ten_KH"].Value.ToString();
            txtIDExprotBill.Text = dtgvExportBill.CurrentRow.Cells["HDX_Ma"].Value.ToString();
            txtTotalBill.Text = dtgvExportBill.CurrentRow.Cells["HDX_TongTien"].Value.ToString();
            string ct_hdx = dtgvExportBill.CurrentRow.Cells["HDX_Ma"].Value.ToString();
            DataTable dataHH = busExportBill.ListExportBill(ct_hdx);

            if (dataHH.Rows.Count > 0)
            {
                cbIDProduct.Text = dataHH.Rows[0]["HH_Ten"].ToString();
                txtNumberProduct.Text = dataHH.Rows[0]["SoLuongXuat"].ToString();
                txtTotalProduct.Text = dataHH.Rows[0]["ThanhTien"].ToString();
            }
            else
            {
                // Handle the case where no data is returned
                MessageBox.Show("No data found for the selected export bill.");
            }


        }
    }



}
