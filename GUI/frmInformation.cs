using BUS;
using DAL;
using MySql.Data.MySqlClient;
using System;
using System.Windows.Forms;

namespace GUI
{
    public partial class frmInformation : Form
    {

        BUS_TKHT tkht;
        private string email;
        public frmInformation(string email)
        {
            this.email = email;
            InitializeComponent();
        }



        private void frmInformation_Load(object sender, EventArgs e)
        {
            // truy vấn SQL
            string query = "SELECT TKHT_HoTen,PQ_Ma,TKHT_Password,TKHT_DiaChi,TKHT_SoDienThoai,TKHT_GioiTinh,TKHT_NgaySinh " +
                "FROM TAI_KHOAN_HE_THONG Where TKHT_Email = '" + email + "'";

            // khởi tạo đối tượng SqlConnection
            using (MySqlConnection connection = new MySqlConnection(DbConnect.stringConnect))
            {
                // mở kết nối
                connection.Open();

                // khởi tạo đối tượng SqlCommand với truy vấn và kết nối
                using (MySqlCommand command = new MySqlCommand(query, connection))
                {
                    // khởi tạo đối tượng SqlDataReader để đọc dữ liệu từ truy vấn
                    using (MySqlDataReader reader = command.ExecuteReader())
                    {
                        // kiểm tra nếu có dữ liệu
                        if (reader.HasRows)
                        {
                            // đọc dữ liệu và gán vào biến tương ứng
                            while (reader.Read())
                            {
                                string TKHT_Email = reader.GetString(0);
                                string TKHT_HoTen = reader.GetString(0);
                                string PQ_Ma = reader.GetInt32(1).ToString();
                                string TKHT_Password = reader.GetString(2);
                                string TKHT_DiaChi = reader.GetString(3);
                                string TKHT_SoDienThoai = reader.GetString(4);
                                bool TKHT_GioiTinh = reader.GetBoolean(5);
                                DateTime TKHT_NgaySinh = reader.GetDateTime(6);
                                string NgaySinh = TKHT_NgaySinh.ToString("yyyy-MM-dd");
                                txtAddress.Text = TKHT_DiaChi;
                                txtPhone.Text = TKHT_SoDienThoai;
                                txtEmail.Text = email;
                                txtName.Text = TKHT_HoTen;
                                guna2TextBox1.Text = NgaySinh;
                                if (TKHT_GioiTinh == true)
                                {
                                    checkMale.Checked = true;
                                }
                                if (TKHT_GioiTinh == false)
                                {
                                    checkFemale.Checked = true;
                                }
                            }
                        }
                        else
                        {
                            // không có dữ liệu
                            // xử lý tương ứng ở đây
                            // ...
                        }
                    }
                }
            }
        }
        private void btnUpdatePass_Click(object sender, EventArgs e)
        {
            if (txtOldPassword.Text != "")
            {
                if (txtNewPassword.Text == txtRepeatPassword.Text)
                {
                    tkht = new BUS_TKHT();
                    if (tkht.ChangePassword(txtEmail.Text, txtOldPassword.Text, txtNewPassword.Text))
                    {
                        MessageBox.Show("Đổi mật khẩu thành công, vui lòng đăng nhập lại.", "Thông báo",
                            MessageBoxButtons.OK, MessageBoxIcon.Information);
                        this.Close();
                        Application.Restart();

                    }
                    else MessageBox.Show("Mật khẩu cũ không đúng", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                else MessageBox.Show("Mật khẩu mới không trùng nhau!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            else MessageBox.Show("Vui lòng nhập mật khẩu cũ", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }

        private void btnRefresh_Click(object sender, EventArgs e)
        {
            txtAddress.Text = "";
            txtPhone.Text = "";
            txtEmail.Text = "";
            txtName.Text = "";
            guna2TextBox1.Text = "";

        }

        private void btnUpdate_Click(object sender, EventArgs e)
        {

            if (txtAddress.Text == "" || txtPhone.Text == "" || txtEmail.Text == "" || guna2TextBox1.Text == "")
            {
                MessageBox.Show("Vui lòng nhập đầy đủ thông tin");

            }
            else
            {
                int gender;
                string role;

                if (checkMale.Checked == true)
                {
                    gender = 1;
                }
                else
                {
                    gender = 0;
                }
                string query = "UPDATE TAI_KHOAN_HE_THONG" +
                    "SET TKHT_Email = '" + txtEmail.Text + "'," +
                    "TKHT_HoTen = '" + txtName.Text + "',TKHT_DiaChi = '" + txtAddress.Text +
                    "',TKHT_SoDienThoai = '" + txtPhone.Text +
                    "',TKHT_GioiTinh = " + gender + ",TKHT_NgaySinh = '" + guna2TextBox1.Text +
                    "' WHERE TKHT_Email='" + email + "'";
                try
                {
                    MySqlConnection conn = new MySqlConnection(DbConnect.stringConnect);
                    conn.Open();
                    MySqlCommand comd = new MySqlCommand(query, conn);
                    comd.ExecuteNonQuery();
                    MessageBox.Show("Cập nhật thông tin thành công");
                    frmInformation_Load(sender, e);
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }

        }
    }
}
