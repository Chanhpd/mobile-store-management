using DTO;
using MySql.Data.MySqlClient;
using System;
using System.Data;
using System.Windows.Forms;

namespace DAL
{
    public class DAL_TKHT : DbConnect
    {
        public string CheckLogin(DTO_TKHT TKHT)
        {
            string info = CheckLoginDTO(TKHT);

            return info;
        }
        public string checkRole(DTO_TKHT tkht)
        {
            string role = checkRoleDTO(tkht);
            return role;
        }
        public DataTable GetTKHT(string role, string email)
        {
            MySqlConnection conn = new MySqlConnection(stringConnect);
            DataTable table = new DataTable();
            try
            {
                conn.Open();
                string query = "SELECT b.PQ_Ten, a.PQ_Ma FROM TAI_KHOAN_HE_THONG as a, PHAN_QUYEN AS b where a.PQ_Ma = b.PQ_Ma and b.PQ_Ma = @PQ_Ma and a.TKHT_Email = @TKHT";

                MySqlCommand cmd = new MySqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@PQ_Ma", role);
                cmd.Parameters.AddWithValue("@TKHT", email);
                MySqlDataAdapter data = new MySqlDataAdapter(cmd);
                data.Fill(table);
            }
            catch (Exception)
            {
                // Xử lý ngoại lệ
            }
            finally
            {
                conn.Close();
            }
            return table;
        }
        public string GetFieldValues(string sql)
        {
            string query = "";
            MySqlConnection conn = new MySqlConnection(stringConnect);
            conn.Open();
            MySqlCommand cmd = new MySqlCommand(sql, conn);
            MySqlDataReader reader;
            reader = cmd.ExecuteReader();
            while (reader.Read())
                query = reader.GetValue(0).ToString();
            reader.Close();
            return query;
        }
        public bool ChangePassword(string email, string oldPassword, string newPassword)
        {
            MySqlConnection conn = new MySqlConnection(stringConnect);
            try
            {
                conn.Open();
                MySqlCommand cmd = new MySqlCommand();
                cmd.Connection = conn;
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                cmd.CommandText = "ChangePassword";
                cmd.Parameters.AddWithValue("email", email);
                cmd.Parameters.AddWithValue("oldPassword", oldPassword);
                cmd.Parameters.AddWithValue("newPassword", newPassword);
                if (Convert.ToInt16(cmd.ExecuteScalar()) == 1)
                    return true;
                else
                    return false;
            }
            catch (Exception)
            {

            }
            finally
            {
                conn.Close();
            }
            return false;
        }
        public DataTable ListStaff(string nameSearch = "")
        {
            // Chuỗi kết nối MySQL (giả định rằng bạn đã khai báo stringConnect)
            MySqlConnection conn = new MySqlConnection(stringConnect);
            try
            {
                conn.Open();

                // Câu lệnh SQL với tham số @Search
                string query = @"SELECT 
                            a.TKHT_Email, 
                            a.TKHT_HoTen, 
                            b.PQ_Ten, 
                            a.TKHT_DiaChi, 
                            a.TKHT_SoDienThoai,
                            CASE 
                                WHEN a.TKHT_GioiTinh = 1 THEN N'Nam' 
                                ELSE N'Nữ' 
                            END AS TKHT_GioiTinh,
                            IFNULL(NULLIF(a.TKHT_NgaySinh, '0000-00-00'), NULL) AS TKHT_NgaySinh
                         FROM 
                            TAI_KHOAN_HE_THONG AS a
                         JOIN 
                            PHAN_QUYEN AS b 
                         ON 
                            a.PQ_Ma = b.PQ_Ma
                         WHERE 
                            a.TKHT_HoTen LIKE @Search";

                // Tạo đối tượng Command
                MySqlCommand comd = new MySqlCommand(query, conn);
                comd.CommandType = CommandType.Text;

                // Thêm tham số và giá trị
                comd.Parameters.AddWithValue("@Search", "%" + nameSearch + "%");

                // Thực thi và load dữ liệu vào DataTable
                DataTable data = new DataTable();
                using (MySqlDataReader reader = comd.ExecuteReader())
                {
                    data.Load(reader);
                }

                return data;
            }
            catch (Exception ex)
            {
                throw new Exception("Error: " + ex.Message);
            }
            finally
            {
                // Đảm bảo kết nối được đóng
                if (conn.State == ConnectionState.Open)
                {
                    conn.Close();
                }
            }
        }

        public DataTable PhanQuyen()
        {
            MySqlConnection conn = new MySqlConnection(stringConnect);
            DataTable table = new DataTable();
            try
            {
                conn.Open();
                string query = "SELECT * FROM PHAN_QUYEN";
                MySqlDataAdapter data = new MySqlDataAdapter(query, conn);
                data.Fill(table);
            }
            catch (Exception)
            {
            }
            finally
            {
                conn.Close();
            }
            return table;
        }

        // for Manage Staff
        public bool InsertNhanVien(DTO_TKHT tkht)
        {
            using (MySqlConnection conn = new MySqlConnection(stringConnect))
            {
                try
                {
                    conn.Open();

                    // Kiểm tra email đã tồn tại chưa
                    string checkEmailQuery = "SELECT TKHT_Email FROM TAI_KHOAN_HE_THONG WHERE TKHT_Email = @Email";
                    using (MySqlCommand checkCmd = new MySqlCommand(checkEmailQuery, conn))
                    {
                        checkCmd.Parameters.AddWithValue("@Email", tkht.Tkht_Email);
                        var result = checkCmd.ExecuteScalar();
                        if (result != null)
                        {
                            MessageBox.Show("Email này đã tồn tại, bạn hãy nhập email khác!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);

                        }
                    }

                    // Câu lệnh INSERT dữ liệu
                    string insertQuery = @"
                    INSERT INTO TAI_KHOAN_HE_THONG 
                    (TKHT_Email, TKHT_HoTen, PQ_Ma, TKHT_Password, TKHT_DiaChi, TKHT_SoDienThoai, TKHT_GioiTinh, TKHT_NgaySinh) 
                    VALUES 
                    (@Email, @HoTen, @PhanQuyen, @Password, @DiaChi, @SoDienThoai, @GioiTinh, @NgaySinh)";

                    using (MySqlCommand cmd = new MySqlCommand(insertQuery, conn))
                    {
                        // Thêm tham số vào câu lệnh
                        cmd.Parameters.AddWithValue("@Email", tkht.Tkht_Email);
                        cmd.Parameters.AddWithValue("@HoTen", tkht.Tkht_HoTen);
                        cmd.Parameters.AddWithValue("@PhanQuyen", tkht.Pq_Ma);
                        cmd.Parameters.AddWithValue("@Password", tkht.Tkht_Password);
                        cmd.Parameters.AddWithValue("@DiaChi", tkht.Tkht_DiaChi);
                        cmd.Parameters.AddWithValue("@SoDienThoai", tkht.Tkht_SoDienThoai);
                        cmd.Parameters.AddWithValue("@GioiTinh", tkht.Tkht_GioiTinh);
                        cmd.Parameters.AddWithValue("@NgaySinh", tkht.Tkht_NgaySinh);

                        // Thực thi câu lệnh
                        cmd.ExecuteNonQuery();
                        MessageBox.Show("Thêm nhân viên thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }

                    return true;

                }
                catch (Exception ex)
                {
                    MessageBox.Show("Thêm nhân viên thất bại!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return false;
                }
                finally
                {
                    conn.Close();
                }
            }
        }

        public bool UpdateNhanVien(DTO_TKHT tkht)
        {
            using (MySqlConnection conn = new MySqlConnection(stringConnect))
            {
                try
                {
                    conn.Open();

                    // Câu lệnh UPDATE dữ liệu
                    string updateQuery = @"
                    UPDATE TAI_KHOAN_HE_THONG 
                    SET TKHT_HoTen = @HoTen, PQ_Ma = @PhanQuyen, TKHT_Password = @Password, TKHT_DiaChi = @DiaChi, TKHT_SoDienThoai = @SoDienThoai, TKHT_GioiTinh = @GioiTinh, TKHT_NgaySinh = @NgaySinh
                    WHERE TKHT_Email = @Email";

                    using (MySqlCommand cmd = new MySqlCommand(updateQuery, conn))
                    {
                        // Thêm tham số vào câu lệnh
                        cmd.Parameters.AddWithValue("@Email", tkht.Tkht_Email);
                        cmd.Parameters.AddWithValue("@HoTen", tkht.Tkht_HoTen);
                        cmd.Parameters.AddWithValue("@PhanQuyen", tkht.Pq_Ma);
                        cmd.Parameters.AddWithValue("@Password", tkht.Tkht_Password);
                        cmd.Parameters.AddWithValue("@DiaChi", tkht.Tkht_DiaChi);
                        cmd.Parameters.AddWithValue("@SoDienThoai", tkht.Tkht_SoDienThoai);
                        cmd.Parameters.AddWithValue("@GioiTinh", tkht.Tkht_GioiTinh);
                        cmd.Parameters.AddWithValue("@NgaySinh", tkht.Tkht_NgaySinh);

                        // Thực thi câu lệnh
                        cmd.ExecuteNonQuery();
                        MessageBox.Show("Cập nhật nhân viên thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }

                    return true;

                }
                catch (Exception ex)
                {
                    MessageBox.Show("Cập nhật nhân viên thất bại!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return false;
                }
                finally
                {
                    conn.Close();
                }
            }
        }

        public bool DeleteNhanVien(string email)
        {
            using (MySqlConnection conn = new MySqlConnection(stringConnect))
            {
                try
                {
                    conn.Open();

                    // Câu lệnh DELETE dữ liệu
                    string deleteQuery = @"
                    DELETE FROM TAI_KHOAN_HE_THONG 
                    WHERE TKHT_Email = @Email";

                    using (MySqlCommand cmd = new MySqlCommand(deleteQuery, conn))
                    {
                        // Thêm tham số vào câu lệnh
                        cmd.Parameters.AddWithValue("@Email", email);

                        // Thực thi câu lệnh
                        cmd.ExecuteNonQuery();
                        MessageBox.Show("Xóa nhân viên thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }

                    return true;

                }
                catch (Exception ex)
                {
                    MessageBox.Show("Xóa nhân viên thất bại!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return false;
                }
                finally
                {
                    conn.Close();
                }
            }
        }

        public void SearchNhanVien(string search)
        {

        }
    }

}
