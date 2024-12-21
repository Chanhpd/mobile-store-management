using DTO;
using MySql.Data.MySqlClient;
using System;
using System.Data;

namespace DAL
{
    public class DAL_HangHoa : DbConnect
    {

        public DataTable ListProduct()
        {
            MySqlConnection conn = new MySqlConnection(stringConnect);
            try
            {
                conn.Open();
                MySqlCommand comd = new MySqlCommand(@"SELECT 
                    HH.HH_Ma,
                    LH.LH_Ten,
                    NSX.NSX_Ten,
                    NCC.NCC_Ten,
                    HH.HH_Ten,
                    HH.HH_SoLuong,
                    HH.HH_MoTa,
                    HH.HH_DonGia,
                    HH.HH_HinhAnh
                FROM HANGHOA AS HH
                INNER JOIN LOAI_HANG AS LH ON HH.LH_Ma = LH.LH_Ma
                INNER JOIN NUOC_SAN_XUAT AS NSX ON NSX.NSX_Ma = HH.NSX_Ma
                INNER JOIN NHA_CUNG_CAP AS NCC ON NCC.NCC_Ma = HH.NCC_Ma", conn);
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

        public bool InsertProduct(DTO_HangHoa product)
        {
            string query = @"
        INSERT INTO HANGHOA 
        (LH_Ma , NSX_ma , NCC_ma , HH_Ten, HH_SoLuong, HH_MoTa, HH_DonGia, HH_HinhAnh) 
        VALUES 
        (@maloai, @manuocsx, @manhacungcap, @tenhang, @soluonghang, @motahang, @dongiahang, @hinhanh)";

            using (MySqlConnection conn = new MySqlConnection(stringConnect))
            {
                try
                {
                    conn.Open();
                    using (MySqlCommand comd = new MySqlCommand(query, conn))
                    {
                        // Thêm tham số vào truy vấn
                        comd.Parameters.AddWithValue("@maloai", product.Lh_Ma);
                        comd.Parameters.AddWithValue("@manuocsx", product.Nsx_Ma);
                        comd.Parameters.AddWithValue("@manhacungcap", product.Ncc_ma);
                        comd.Parameters.AddWithValue("@tenhang", product.Hh_Ten);
                        comd.Parameters.AddWithValue("@soluonghang", product.Hh_SoLuong);
                        comd.Parameters.AddWithValue("@motahang", product.Hh_MoTa);
                        comd.Parameters.AddWithValue("@dongiahang", product.Hh_DonGia);
                        comd.Parameters.AddWithValue("@hinhanh", product.Hh_HinhAnh);

                        // Thực thi truy vấn
                        return comd.ExecuteNonQuery() > 0;
                    }
                }
                catch (Exception ex)
                {
                    // Ghi log lỗi để kiểm tra
                    Console.WriteLine($"Lỗi khi thêm sản phẩm: {ex.Message}");
                    return false;
                }
            }
        }


        public bool UpdateProduct(DTO_HangHoa product)
        {
            string query = @"
        UPDATE HANGHOA 
        SET 
            LH_Ma = @maloai,
            NSX_Ma = @manuocsx,
            NCC_Ma = @manhacungcap,
            HH_Ten = @tenhang,
            HH_SoLuong = @soluonghang,
            HH_MoTa = @motahang,
            HH_DonGia = @dongiahang,
            HH_HinhAnh = @hinhanh
        WHERE HH_Ma = @mahang";

            using (MySqlConnection conn = new MySqlConnection(stringConnect))
            {
                try
                {
                    conn.Open();
                    using (MySqlCommand comd = new MySqlCommand(query, conn))
                    {
                        // Thêm tham số vào truy vấn
                        comd.Parameters.AddWithValue("@maloai", product.Lh_Ma);
                        comd.Parameters.AddWithValue("@manuocsx", product.Nsx_Ma);
                        comd.Parameters.AddWithValue("@manhacungcap", product.Ncc_ma);
                        comd.Parameters.AddWithValue("@tenhang", product.Hh_Ten);
                        comd.Parameters.AddWithValue("@soluonghang", product.Hh_SoLuong);
                        comd.Parameters.AddWithValue("@motahang", product.Hh_MoTa);
                        comd.Parameters.AddWithValue("@dongiahang", product.Hh_DonGia);
                        comd.Parameters.AddWithValue("@hinhanh", product.Hh_HinhAnh);
                        comd.Parameters.AddWithValue("@mahang", product.Hh_Ma);

                        // Thực thi truy vấn
                        return comd.ExecuteNonQuery() > 0;
                    }
                }
                catch (Exception ex)
                {
                    // Log lỗi chi tiết
                    Console.WriteLine($"Lỗi khi cập nhật sản phẩm: {ex.Message}");
                    return false;
                }
            }
        }


        public bool DeleteProduct(string hh_ma)
        {
            MySqlConnection conn = new MySqlConnection(stringConnect);
            try
            {
                conn.Open();
                string query = "DELETE FROM HANGHOA WHERE HH_Ma = @hh_ma";
                MySqlCommand cmd = new MySqlCommand(query, conn);
                cmd.CommandType = CommandType.Text;
                cmd.Parameters.AddWithValue("hh_ma", hh_ma);
                if (cmd.ExecuteNonQuery() > 0)
                    return true;
                else
                    return false;
            }
            catch (Exception)
            {
                // Log or handle the exception as needed
                return false;
            }
            finally
            {
                conn.Close();
            }
        }

        public DataTable SearchProduct(string tenhang)
        {
            using (MySqlConnection conn = new MySqlConnection(stringConnect))
            {
                try
                {
                    conn.Open();

                    string query = @"
                SELECT 
                    HH.HH_Ma,
                    LH.LH_Ten,
                    NSX.NSX_Ten,
                    NCC.NCC_Ten,
                    HH.HH_Ten,
                    HH.HH_SoLuong,
                    HH.HH_MoTa,
                    HH.HH_DonGia,
                    HH.HH_HinhAnh
                FROM HANGHOA AS HH
                INNER JOIN LOAI_HANG AS LH ON HH.LH_Ma = LH.LH_Ma
                INNER JOIN NUOC_SAN_XUAT AS NSX ON NSX.NSX_Ma = HH.NSX_Ma
                INNER JOIN NHA_CUNG_CAP AS NCC ON NCC.NCC_Ma = HH.NCC_Ma
                WHERE HH.HH_Ten LIKE @tenhang";

                    using (MySqlCommand comd = new MySqlCommand(query, conn))
                    {
                        // Thêm dấu % vào tham số LIKE
                        comd.Parameters.AddWithValue("@tenhang", $"%{tenhang}%");

                        // Tạo DataTable và nạp dữ liệu
                        DataTable data = new DataTable();
                        using (MySqlDataReader reader = comd.ExecuteReader())
                        {
                            data.Load(reader);
                        }

                        return data;
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Lỗi khi tìm kiếm sản phẩm: {ex.Message}");
                    return null; // hoặc xử lý tùy theo yêu cầu
                }
                finally
                {
                    conn.Close();
                }
            }
        }


        public DataTable GetVatTu()
        {
            MySqlConnection conn = new MySqlConnection(stringConnect);
            try
            {
                conn.Open();
                MySqlCommand comd = new MySqlCommand("SELECT * FROM HANGHOA", conn);
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

        public string GetFieldValues(string sql)
        {
            MySqlConnection conn = new MySqlConnection(stringConnect);
            string ma = "";
            MySqlCommand cmd = new MySqlCommand(sql, conn);
            conn.Open();
            MySqlDataReader reader = cmd.ExecuteReader();
            while (reader.Read())
                ma = reader.GetValue(0).ToString();
            reader.Close();
            conn.Close();
            return ma;
        }

        public DataTable ListRevenue()
        {
            MySqlConnection conn = new MySqlConnection(stringConnect);
            try
            {
                conn.Open();
                MySqlCommand comd = new MySqlCommand(@"SELECT 
                    HH.HH_Ma,
                    LH.LH_Ten,
                    NSX.NSX_Ten,
                    NCC.NCC_Ten,
                    HH.HH_Ten,
                    HH.HH_SoLuong,
                    HH.HH_MoTa,
                    HH.HH_DonGia,
                    HH.HH_HinhAnh
                FROM HANGHOA AS HH
                INNER JOIN LOAI_HANG AS LH ON HH.LH_Ma = LH.LH_Ma
                INNER JOIN NUOC_SAN_XUAT AS NSX ON NSX.NSX_Ma = HH.NSX_Ma
                INNER JOIN NHA_CUNG_CAP AS NCC ON NCC.NCC_Ma = HH.NCC_Ma", conn);
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
    }
}
