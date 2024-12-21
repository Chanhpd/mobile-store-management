using DTO;
using MySql.Data.MySqlClient;
using System;

using System.Data;
using System.Windows.Forms;

namespace DAL
{
    public class DAL_ExportBills : DbConnect
    {
        public DataTable ListExportBills(string ma)
        {
            MySqlConnection conn = new MySqlConnection(stringConnect);
            try
            {
                conn.Open();
                MySqlCommand comd = new MySqlCommand("SELECT a.HH_Ma, b.HH_Ten, a.SoLuongXuat, b.HH_DonGia,a.ThanhTien FROM CHI_TIET_HD_XUAT AS a, HANGHOA AS b WHERE a.HDX_Ma = @HDX_Ma AND a.HH_Ma=b.HH_Ma", conn);
                comd.CommandType = CommandType.Text;
                comd.Parameters.AddWithValue("@HDX_Ma", ma);
                DataTable data = new DataTable();
                data.Load(comd.ExecuteReader());
                return data;
            }
            finally
            {
                conn.Close();
            }
        }

        public DataTable ListAllExportBills()
        {
            MySqlConnection conn = new MySqlConnection(stringConnect);
            try
            {
                conn.Open();
                string sql = "SELECT hdx.HDX_ma, kh.TEN_KH, hdx.TKHT_Email, hdx.HDX_NgayLap, hdx.HDX_TongTien " +
                             "FROM HOA_DON_XUAT AS hdx " +
                             "JOIN khach_hang AS kh ON hdx.Ma_KH = kh.Ma_KH";
                MySqlCommand comd = new MySqlCommand(sql, conn);
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

        public void FillCombo(string sql, ComboBox cbo, string ma, string ten)
        {
            MySqlConnection conn = new MySqlConnection(stringConnect);
            MySqlDataAdapter dap = new MySqlDataAdapter(sql, conn);
            DataTable table = new DataTable();
            dap.Fill(table);
            cbo.DataSource = table;
            cbo.ValueMember = ma; //Trường giá trị
            cbo.DisplayMember = ten; //Trường hiển thị
        }


        public bool InsertExportBills(DTO_HoaDonXuat HDX, DTO_CTHoaDonXuat ct_hdx)
        {
            MySqlConnection conn = new MySqlConnection(stringConnect);
            try
            {
                conn.Open();
                string query = "INSERT INTO HOA_DON_XUAT( MA_KH, TKHT_Email, HDX_NgayLap, HDX_TongTien) VALUES(@makh, @emailtkht, @ngaylap, @tongtien)";


                MySqlCommand comd = new MySqlCommand(query, conn);

                comd.CommandType = CommandType.Text;

                comd.Parameters.AddWithValue("@makh", HDX.Ma_KH);
                comd.Parameters.AddWithValue("@emailtkht", HDX.Tkht_Email);
                comd.Parameters.AddWithValue("@ngaylap", HDX.Hdx_NgayLap);
                comd.Parameters.AddWithValue("@tongtien", HDX.TongTien);

                if (comd.ExecuteNonQuery() > 0)
                {
                    MySqlCommand getLastIdCommand = new MySqlCommand("SELECT LAST_INSERT_ID()", conn);
                    int hdxma = Convert.ToInt32(getLastIdCommand.ExecuteScalar());
                    Console.WriteLine(hdxma);
                    insertCTHDX(hdxma, ct_hdx, conn);

                    return true;
                }
                else
                    return false;
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
            finally
            {
                conn.Close();
            }

            return false;
        }

        public void insertCTHDX(int HDX_ma, DTO_CTHoaDonXuat ct_hdx, MySqlConnection conn)
        {
            try
            {
                string query = "INSERT INTO CHI_TIET_HD_XUAT(HDX_Ma, HH_Ma, SoLuongXuat, DonGiaXuat, ThanhTien) VALUES(@hdxma, @hhma, @soluongxuat, @dongiaxuat, @thanhtien)";
                MySqlCommand comd = new MySqlCommand(query, conn);
                comd.CommandType = CommandType.Text; comd.Parameters.AddWithValue("@hdxma", HDX_ma);
                comd.Parameters.AddWithValue("@hhma", ct_hdx.Hh_Ma);
                comd.Parameters.AddWithValue("@soluongxuat", ct_hdx.SoLuongXuat);
                comd.Parameters.AddWithValue("@dongiaxuat", ct_hdx.DonGiaNhap);
                comd.Parameters.AddWithValue("@thanhtien", ct_hdx.SoLuongXuat * ct_hdx.DonGiaNhap);


                updateQuantityProduct(ct_hdx.Hh_Ma, ct_hdx.SoLuongXuat);

                comd.ExecuteNonQuery();
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
        }
        public bool deleteExportBill(string ma)
        {
            using (MySqlConnection conn = new MySqlConnection(stringConnect))
            {
                conn.Open();
                MySqlTransaction transaction = conn.BeginTransaction();

                try
                {
                    // Xóa chi tiết hóa đơn xuất
                    string deleteDetailsQuery = "DELETE FROM CHI_TIET_HD_XUAT WHERE HDX_Ma = @hdxma";
                    using (MySqlCommand deleteDetailsCommand = new MySqlCommand(deleteDetailsQuery, conn, transaction))
                    {
                        deleteDetailsCommand.Parameters.AddWithValue("@hdxma", ma);
                        deleteDetailsCommand.ExecuteNonQuery();
                    }

                    // Xóa hóa đơn xuất
                    string deleteBillQuery = "DELETE FROM HOA_DON_XUAT WHERE HDX_Ma = @hdxma";
                    using (MySqlCommand deleteBillCommand = new MySqlCommand(deleteBillQuery, conn, transaction))
                    {
                        deleteBillCommand.Parameters.AddWithValue("@hdxma", ma);
                        deleteBillCommand.ExecuteNonQuery();
                    }

                    // Commit giao dịch
                    transaction.Commit();
                    return true;
                }
                catch (Exception e)
                {
                    // Rollback nếu có lỗi
                    transaction.Rollback();
                    Console.WriteLine("Error: " + e.Message);
                    return false;
                }
            }
        }

        public void updateQuantityProduct(int productId, int quantitySold)
        {
            MySqlConnection conn = new MySqlConnection(stringConnect);
            try
            {
                conn.Open();
                // Tạo câu lệnh SQL để cập nhật số lượng hàng hóa trong kho
                string query = "UPDATE HANGHOA SET HH_SoLuong = HH_SoLuong - @quantitySold WHERE HH_Ma = @productId";
                MySqlCommand comd = new MySqlCommand(query, conn);
                comd.CommandType = CommandType.Text;

                // Gán giá trị cho các tham số
                comd.Parameters.AddWithValue("@quantitySold", quantitySold);
                comd.Parameters.AddWithValue("@productId", productId);

                // Thực hiện câu lệnh SQL
                comd.ExecuteNonQuery();
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
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
            MySqlDataReader reader;
            reader = cmd.ExecuteReader();
            while (reader.Read())
                ma = reader.GetValue(0).ToString();
            reader.Close();
            conn.Close();
            return ma;
        }
        public DataTable GetDataToTable(string sql)
        {
            MySqlConnection conn = new MySqlConnection(stringConnect);
            MySqlDataAdapter dap = new MySqlDataAdapter(); //Định nghĩa đối tượng thuộc lớp MySqlDataAdapter
            //Tạo đối tượng thuộc lớp MySqlCommand
            dap.SelectCommand = new MySqlCommand();
            dap.SelectCommand.Connection = conn; //Kết nối cơ sở dữ liệu
            dap.SelectCommand.CommandText = sql; //Lệnh SQL
            //Khai báo đối tượng table thuộc lớp DataTable
            DataTable table = new DataTable();
            dap.Fill(table);
            return table;
        }

    }
}
