using BUS;
using DAL;
using DTO;
using System;
using System.Data;
using System.Windows.Forms;

namespace GUI
{
    public partial class frmImportBill : Form
    {
        DAL_ImportBill dalImportBill = new DAL_ImportBill();
        BUS_ImportBill busImportBill = new BUS_ImportBill();

        private string role;
        private string email;
        public frmImportBill(string role, string email)
        {
            InitializeComponent();
            this.role = role;
            this.email = email;
            LoadComboBoxTKHT(role, email);
            cbNCC.SelectedIndexChanged += CbNCC_SelectedIndexChanged;
            cbIDProduct.SelectedIndexChanged += CbIDProduct_SelectedIndexChanged;
        }
        private void LoadComboBoxTKHT(string role, string email)
        {
            string str;
            DAL_TKHT tkht = new DAL_TKHT();
            DataTable data = tkht.GetTKHT(role, email);
            cbPQNV.DisplayMember = "PQ_Ma";
            cbPQNV.ValueMember = "TKHT_Email";
            cbPQNV.DataSource = data;
            str = "Select TKHT_Email from TAI_KHOAN_HE_THONG where PQ_Ma ='" + role + "' and TKHT_Email ='" + email + "'";
            txtStaff.Text = tkht.GetFieldValues(str);

        }
        private void LoadComboBoxVatTu()
        {
            DAL_HangHoa hanghoa = new DAL_HangHoa();
            DataTable data = hanghoa.GetVatTu();
            cbIDProduct.DisplayMember = "HH_Ten";
            cbIDProduct.ValueMember = "HH_Ma";
            cbIDProduct.DataSource = data;
            txtIdProduct.Text = "";
            txtPrice.Text = "";
            txtTotalBill.Text = "";

        }
        private void CbIDProduct_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cbIDProduct.SelectedIndex != -1)
            {
                DataRowView selectedRow = cbIDProduct.SelectedItem as DataRowView;

                if (selectedRow != null)
                {
                    txtIdProduct.Text = selectedRow["HH_Ma"].ToString();
                    txtPrice.Text = selectedRow["HH_DonGia"].ToString();

                    int total = 0, num = 0;
                    int price = txtPrice.Text == "" ? 0 : int.Parse(txtPrice.Text);
                    num = txtNumberProduct.Text == "" ? 0 : int.Parse(txtNumberProduct.Text);
                    total = num * price;

                    txtTotalProduct.Text = total.ToString();
                }
            }
        }
        private void LoadComboBoxNCC()
        {
            DAL_NhaCungCap NhaCungCap = new DAL_NhaCungCap();
            DataTable data = NhaCungCap.GetNhaCungCap();
            cbNCC.DisplayMember = "NCC_Ten";
            cbNCC.ValueMember = "NCC_Ma";
            cbNCC.DataSource = data;

            txtMaNCC.Text = "";
            txtAddress.Text = "";
            txtPhone.Text = "";
        }
        private void CbNCC_SelectedIndexChanged(object sender, EventArgs e)
        {

            if (cbNCC.SelectedIndex != -1)
            {
                DataRowView selectedRow = cbNCC.SelectedItem as DataRowView;

                if (selectedRow != null)
                {
                    txtMaNCC.Text = selectedRow["NCC_Ma"].ToString();
                    txtAddress.Text = selectedRow["NCC_DiaChi"].ToString();
                    txtPhone.Text = selectedRow["NCC_SDT"].ToString();
                }
            }
        }

        private void LoadDataGV()
        {

            dtgvImportBill.Columns[0].HeaderText = "Mã hoá đơn";
            dtgvImportBill.Columns[1].HeaderText = "Tên nhà cung cấp";
            dtgvImportBill.Columns[2].HeaderText = "Email nhân viên";
            dtgvImportBill.Columns[3].HeaderText = "Ngày nhập";
            dtgvImportBill.Columns[4].HeaderText = "Tổng tiền";


            foreach (DataGridViewColumn item in dtgvImportBill.Columns)
                item.DividerWidth = 1;


        }
        private void frmImportBill_Load(object sender, EventArgs e)
        {
            btnInsert.Enabled = true;
            btnUpdate.Enabled = true;
            btnDelete.Enabled = true;
            btnPrint.Enabled = false;
            cbPQNV.Enabled = false;
            txtStaff.ReadOnly = true;
            txtMaNCC.ReadOnly = true;
            txtAddress.ReadOnly = true;
            txtPhone.ReadOnly = true;
            txtIdProduct.ReadOnly = true;
            txtPrice.ReadOnly = true;
            txtTotalProduct.ReadOnly = true;
            txtTotalBill.ReadOnly = true;
            LoadComboBoxNCC();
            cbNCC.SelectedIndex = -1;
            datePicker.Value = DateTime.Now;
            LoadComboBoxTKHT(role, email);
            LoadComboBoxVatTu();
            cbIDProduct.SelectedIndex = -1;

            dtgvImportBill.DataSource = busImportBill.ListAllImportBill();
            CalculateTotalBill();
            LoadDataGV();
            dtgvImportBill.DefaultCellStyle.ForeColor = System.Drawing.Color.Black;
            // Optionally, set the background color
            dtgvImportBill.DefaultCellStyle.BackColor = System.Drawing.Color.White;
        }

        private void cbPQNV_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void guna2Panel1_Paint(object sender, PaintEventArgs e)
        {

        }

        private void btnInsert_Click(object sender, EventArgs e)
        {
            if (txtMaNCC.Text == "")
            {
                MessageBox.Show("Vui lòng chọn nhà cung cấp");
                return;
            }
            if (txtTotalProduct.Text == "")
            {
                MessageBox.Show("Vui lòng nhập đủ thông tin!");
                return;
            }

            int NCC_ma = int.Parse(txtMaNCC.Text);
            string email = txtStaff.Text;
            string ngayNhap = datePicker.Value.ToString("yyyy-MM-dd");
            float tongTien = float.Parse(txtTotalProduct.Text);

            DTO_HoaDonNhap hdn = new DTO_HoaDonNhap(0, NCC_ma, email, ngayNhap, tongTien);

            int HH_ma = int.Parse(txtIdProduct.Text);
            int soLuongNhap = int.Parse(txtNumberProduct.Text);
            float donGiaNhap = float.Parse(txtPrice.Text);
            DTO_CTHoaDonNhap ct_hdn = new DTO_CTHoaDonNhap(0, HH_ma, soLuongNhap, donGiaNhap);
            busImportBill.insertImportBill(hdn, ct_hdn);

            dtgvImportBill.DataSource = busImportBill.ListAllImportBill();



        }

        private void CalculateTotalBill()
        {
            decimal total = 0;
            foreach (DataGridViewRow row in dtgvImportBill.Rows)
            {
                if (row.Cells["HDN_TongTien"].Value != null)
                {
                    total += Convert.ToDecimal(row.Cells["HDN_TongTien"].Value);
                }
            }
            txtTotalBill.Text = total.ToString("N2"); // Format as a number with two decimal places
        }
        private void txtNumberProduct_TextChanged(object sender, EventArgs e)
        {
            if (int.TryParse(txtNumberProduct.Text, out int num))
            {
                int price = txtPrice.Text == "" ? 0 : int.Parse(txtPrice.Text);
                int total = num * price;
                txtTotalProduct.Text = total.ToString();
            }
            else
            {
                txtTotalProduct.Text = "0"; // Reset to 0 if the input is not a valid integer
            }
        }

        private void dtgvImportBill_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            txtIDExprotBill.Text = dtgvImportBill.CurrentRow.Cells["HDN_Ma"].Value.ToString();

            string tenNCC = dtgvImportBill.CurrentRow.Cells["NCC_Ten"].Value.ToString();
            DataTable dtNcc = busImportBill.findNccByName(tenNCC);
            //cbNCC.DataSource = dtNcc;
            txtMaNCC.Text = dtNcc.Rows[0]["NCC_Ma"].ToString();
            txtAddress.Text = dtNcc.Rows[0]["NCC_DiaChi"].ToString();
            txtPhone.Text = dtNcc.Rows[0]["NCC_SDT"].ToString();
            cbNCC.Text = tenNCC;

            datePicker.Value = DateTime.Parse(dtgvImportBill.CurrentRow.Cells["HDN_NgayNhap"].Value.ToString());

            DataTable dataHH = busImportBill.ListImportBill(txtIDExprotBill.Text);
            //cbIDProduct.DataSource = dataHH;
            if (dataHH.Rows.Count > 0)
            {
                txtNumberProduct.Text = dataHH.Rows[0]["SoLuongNhap"].ToString();
                txtPrice.Text = dataHH.Rows[0]["HH_DonGia"].ToString();
                txtTotalProduct.Text = dataHH.Rows[0]["ThanhTien"].ToString();
                txtIdProduct.Text = dataHH.Rows[0]["HH_Ma"].ToString();
                cbIDProduct.Text = dataHH.Rows[0]["HH_Ten"].ToString();
            }
            else
            {

                MessageBox.Show("No data found for the selected import bill.");
            }

        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            int ma = int.Parse(txtIDExprotBill.Text);
            busImportBill.deleleImportBill(ma);
            // Reload the data grid view
            dtgvImportBill.DataSource = busImportBill.ListAllImportBill();
        }

        private void btnUpdate_Click(object sender, EventArgs e)
        {
            if (txtIDExprotBill.Text == "")
            {
                MessageBox.Show("Vui lòng chọn hoá đơn nhập để cập nhật");
                return;
            }
            if (txtMaNCC.Text == "" || txtTotalProduct.Text == "")
            {
                MessageBox.Show("Vui lòng nhập đủ thông tin!");
                return;
            }

            int hdn_Ma = int.Parse(txtIDExprotBill.Text);
            int NCC_ma = int.Parse(txtMaNCC.Text);
            string email = txtStaff.Text;
            string ngayNhap = datePicker.Value.ToString("yyyy-MM-dd");
            float tongTien = float.Parse(txtTotalProduct.Text);

            DTO_HoaDonNhap hdn = new DTO_HoaDonNhap(hdn_Ma, NCC_ma, email, ngayNhap, tongTien);

            int HH_ma = int.Parse(txtIdProduct.Text);
            int soLuongNhap = int.Parse(txtNumberProduct.Text);
            float donGiaNhap = float.Parse(txtPrice.Text);
            DTO_CTHoaDonNhap ct_hdn = new DTO_CTHoaDonNhap(hdn_Ma, HH_ma, soLuongNhap, donGiaNhap);

            bool result = busImportBill.updateImportBill(hdn, ct_hdn);

            if (result)
            {
                MessageBox.Show("Cập nhật hoá đơn nhập thành công!");
                dtgvImportBill.DataSource = busImportBill.ListAllImportBill();
            }
            else
            {
                MessageBox.Show("Cập nhật hoá đơn nhập thất bại!");
            }
        }

        private void btnSearch_Click(object sender, EventArgs e)
        {

        }
    }
}
