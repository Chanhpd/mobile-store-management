using DTO;
using MySql.Data.MySqlClient;
using System;
using System.Data;

namespace DAL
{
    public class DAL_KhachHang : DbConnect
    {

        public DataTable ListCustomers()
        {
            MySqlConnection conn = new MySqlConnection(stringConnect);
            try
            {
                conn.Open();
                MySqlCommand comd = new MySqlCommand("SELECT * FROM khach_hang", conn);
                comd.CommandType = CommandType.Text;
                DataTable data = new DataTable();
                data.Load(comd.ExecuteReader());
                return data;
            }
            finally
            {
                conn.Close();
            }
        }
        public bool InsertCustomer(DTO_KhachHang dtoKhachHang)
        {
            string query = "INSERT INTO khach_hang (TEN_KH, DIACHI, SDT) " +
                "VALUES (@tenkh, @diachi, @sdt)";
            using (MySqlConnection conn = new MySqlConnection(stringConnect))
            {
                try
                {
                    conn.Open();
                    using (MySqlCommand comd = new MySqlCommand(query, conn))
                    {
                        // Thêm tham số vào câu lệnh truy vấn                       
                        comd.Parameters.AddWithValue("@tenkh", dtoKhachHang.Ten_KH);
                        comd.Parameters.AddWithValue("@diachi", dtoKhachHang.DiaChi);
                        comd.Parameters.AddWithValue("@sdt", dtoKhachHang.SDT);

                        // Thực thi truy vấn
                        return comd.ExecuteNonQuery() > 0;
                    }
                }
                catch (Exception ex)
                {
                    // Ghi log lỗi (tùy chỉnh theo nhu cầu)
                    Console.WriteLine($"Lỗi khi thêm khách hàng: {ex.Message}");
                    return false;
                }
            }
        }

        public bool UpdateCustomer(DTO_KhachHang khachhang)
        {
            string query = "UPDATE khach_hang SET TEN_KH = @tenkh, DIACHI = @diachi, SDT = @sdt WHERE MA_KH = @makh";
            using (MySqlConnection conn = new MySqlConnection(stringConnect))
            {
                try
                {
                    conn.Open();
                    using (MySqlCommand comd = new MySqlCommand(query, conn))
                    {
                        // Thêm tham số vào truy vấn
                        comd.Parameters.AddWithValue("@makh", khachhang.Ma_KH);
                        comd.Parameters.AddWithValue("@tenkh", khachhang.Ten_KH);
                        comd.Parameters.AddWithValue("@diachi", khachhang.DiaChi);
                        comd.Parameters.AddWithValue("@sdt", khachhang.SDT);

                        // Thực thi truy vấn
                        return comd.ExecuteNonQuery() > 0;
                    }
                }
                catch (Exception ex)
                {
                    // Ghi log lỗi
                    Console.WriteLine($"Lỗi khi cập nhật khách hàng: {ex.Message}");
                    return false;
                }
            }
        }

        public bool DeleteCustomer(string kh_ma)
        {
            MySqlConnection conn = new MySqlConnection(stringConnect);
            try
            {
                conn.Open();
                string query = "Delete from KHACH_HANG where MA_KH = '" + kh_ma + "'";
                MySqlCommand cmd = new MySqlCommand(query, conn);
                cmd.CommandType = CommandType.Text;
                cmd.Parameters.AddWithValue("makh", kh_ma);
                if (cmd.ExecuteNonQuery() > 0)
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
        public DataTable SearchCustomer(string khachhang)
        {
            MySqlConnection conn = new MySqlConnection(stringConnect);
            try
            {
                conn.Open();
                MySqlCommand comd = new MySqlCommand("SearchCustomers", conn);
                comd.CommandType = CommandType.StoredProcedure;
                comd.Parameters.AddWithValue("ten", khachhang);
                DataTable data = new DataTable();
                data.Load(comd.ExecuteReader());
                return data;
            }
            finally
            {
                conn.Close();
            }
        }
        public DataTable GetCustomer()
        {
            MySqlConnection conn = new MySqlConnection(stringConnect);
            DataTable table = new DataTable();
            try
            {
                conn.Open();
                string query = "SELECT * FROM KHACH_HANG";
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
        public string GetFieldValues(string sql)
        {
            string ma = "";
            MySqlConnection conn = new MySqlConnection(stringConnect);
            conn.Open();
            MySqlCommand cmd = new MySqlCommand(sql, conn);
            MySqlDataReader reader;
            reader = cmd.ExecuteReader();
            while (reader.Read())
                ma = reader.GetValue(0).ToString();
            reader.Close();
            conn.Close();
            return ma;
        }
    }
}
