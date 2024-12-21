
using DTO;
using MySql.Data.MySqlClient;
using System;
using System.Data;
using System.Windows.Forms;

namespace DAL
{
    public class DAL_ImportBill : DbConnect
    {

        public DataTable ListImportBills(string ma)
        {
            MySqlConnection conn = new MySqlConnection(stringConnect);
            try
            {
                conn.Open();
                string sql = @"SELECT
                        a.HH_Ma, 
                        b.HH_Ten, 
                        a.SoLuongNhap, 
                        b.HH_DonGia,
                        a.ThanhTien 
                    FROM 
                        CHITIET_HD_NHAP AS a, 
                        HANGHOA AS b 
                    WHERE 
                        a.HDn_Ma = @HDN_Ma AND a.HH_Ma=b.HH_Ma";
                MySqlCommand comd = new MySqlCommand(sql, conn);

                comd.CommandType = CommandType.Text;
                comd.Parameters.AddWithValue("@HDN_Ma", ma);
                DataTable data = new DataTable();
                data.Load(comd.ExecuteReader());
                return data;
            }
            finally
            {
                conn.Close();
            }
        }
        public DataTable ListAllImportBills()
        {
            MySqlConnection conn = new MySqlConnection(stringConnect);
            try
            {
                conn.Open();
                string sql = @" SELECT
                        hoa_don_nhap.HDN_Ma, 
                        nha_cung_cap.NCC_Ten,
                        hoa_don_nhap.TKHT_Email ,
                        hoa_don_nhap.HDN_NgayNhap,
                        hoa_don_nhap.HDN_TongTien    
                    FROM 
                        hoa_don_nhap
                    INNER JOIN 
                        nha_cung_cap ON hoa_don_nhap.NCC_Ma = nha_cung_cap.NCC_Ma";

                MySqlCommand comd = new MySqlCommand(sql, conn);
                DataTable data = new DataTable();
                data.Load(comd.ExecuteReader());
                return data;
            }
            finally
            {
                conn.Close();
            }
        }

        public bool insertImportBill(DTO_HoaDonNhap hdn, DTO_CTHoaDonNhap ct_hdn)
        {
            MySqlConnection conn = new MySqlConnection(stringConnect);
            try
            {
                conn.Open();
                string sql = @"INSERT INTO hoa_don_nhap (NCC_Ma, TKHT_Email, HDN_NgayNhap, HDN_TongTien)
                       VALUES (@NCC_Ma, @Email, @NgayNhap, @TongTien)";
                MySqlCommand cmd = new MySqlCommand(sql, conn);
                cmd.Parameters.AddWithValue("@NCC_Ma", hdn.Ncc_Ma);
                cmd.Parameters.AddWithValue("@Email", hdn.Tkht_Email);
                cmd.Parameters.AddWithValue("@NgayNhap", hdn.Hdn_NgayLap);
                cmd.Parameters.AddWithValue("@TongTien", hdn.TongTien);

                int result = cmd.ExecuteNonQuery();


                // Get the last inserted ID
                sql = "SELECT LAST_INSERT_ID()";
                cmd = new MySqlCommand(sql, conn);
                hdn.Hdn_Ma = Convert.ToInt32(cmd.ExecuteScalar());
                Console.WriteLine("Inserted ID: " + hdn.Hdn_Ma);

                insertImportBillDetails(hdn.Hdn_Ma, ct_hdn);


                return result > 0;
            }
            catch (Exception ex)
            {
                // Handle exception
                MessageBox.Show("An error occurred: " + ex.Message);
                return false;
            }
            finally
            {
                conn.Close();
            }
        }

        public void insertImportBillDetails(int hdn_Ma, DTO_CTHoaDonNhap cthdn)
        {

            MySqlConnection conn = new MySqlConnection(stringConnect);
            try
            {
                conn.Open();
                string sql = @"INSERT INTO CHITIET_HD_NHAP (HDN_Ma, HH_Ma, SoLuongNhap, DonGiaNhap, ThanhTien)
                       VALUES (@HDN_Ma, @HH_Ma, @SoLuongNhap, @DonGiaNhap, @ThanhTien)";
                MySqlCommand cmd = new MySqlCommand(sql, conn);
                cmd.Parameters.AddWithValue("@HDN_Ma", hdn_Ma);
                cmd.Parameters.AddWithValue("@HH_Ma", cthdn.Hh_Ma);
                cmd.Parameters.AddWithValue("@SoLuongNhap", cthdn.SoLuongNhap);
                cmd.Parameters.AddWithValue("@DonGiaNhap", cthdn.DonGiaNhap);
                cmd.Parameters.AddWithValue("@ThanhTien", cthdn.SoLuongNhap * cthdn.DonGiaNhap);

                cmd.ExecuteNonQuery();

                updateQuantityProduct(cthdn); // Cập nhật số lượng hàng hóa
            }
            catch (Exception ex)
            {

                MessageBox.Show("Có lỗi xảy ra: " + ex.Message);
            }
            finally
            {
                conn.Close();
            }
        }


        public bool deleteImportBill(int HDN_Ma)
        {
            MySqlConnection conn = new MySqlConnection(stringConnect);
            try
            {
                conn.Open();


                MySqlTransaction transaction = conn.BeginTransaction();


                string sqlDeleteDetails = @"DELETE FROM CHITIET_HD_NHAP WHERE HDN_Ma = @HDN_Ma";
                MySqlCommand cmdDeleteDetails = new MySqlCommand(sqlDeleteDetails, conn, transaction);
                cmdDeleteDetails.Parameters.AddWithValue("@HDN_Ma", HDN_Ma);
                cmdDeleteDetails.ExecuteNonQuery();


                string sqlDeleteBill = @"DELETE FROM hoa_don_nhap WHERE HDN_Ma = @HDN_Ma";
                MySqlCommand cmdDeleteBill = new MySqlCommand(sqlDeleteBill, conn, transaction);
                cmdDeleteBill.Parameters.AddWithValue("@HDN_Ma", HDN_Ma);
                int result = cmdDeleteBill.ExecuteNonQuery();

                transaction.Commit();

                return result > 0;
            }
            catch (Exception ex)
            {
                // Handle exception
                MessageBox.Show("An error occurred: " + ex.Message);
                return false;
            }
            finally
            {
                conn.Close();
            }
        }

        public bool updateImportBill(DTO_HoaDonNhap hdn, DTO_CTHoaDonNhap cthdn)
        {
            MySqlConnection conn = new MySqlConnection(stringConnect);
            try
            {
                conn.Open();

                // Start a transaction
                MySqlTransaction transaction = conn.BeginTransaction();

                // Update the import bill
                string sqlUpdateBill = @"UPDATE hoa_don_nhap 
                                 SET NCC_Ma = @NCC_Ma, 
                                     TKHT_Email = @Email, 
                                     HDN_NgayNhap = @NgayNhap, 
                                     HDN_TongTien = @TongTien 
                                 WHERE HDN_Ma = @HDN_Ma";
                MySqlCommand cmdUpdateBill = new MySqlCommand(sqlUpdateBill, conn, transaction);
                cmdUpdateBill.Parameters.AddWithValue("@HDN_Ma", hdn.Hdn_Ma);
                cmdUpdateBill.Parameters.AddWithValue("@NCC_Ma", hdn.Ncc_Ma);
                cmdUpdateBill.Parameters.AddWithValue("@Email", hdn.Tkht_Email);
                cmdUpdateBill.Parameters.AddWithValue("@NgayNhap", hdn.Hdn_NgayLap);
                cmdUpdateBill.Parameters.AddWithValue("@TongTien", hdn.TongTien);

                int resultBill = cmdUpdateBill.ExecuteNonQuery();

                // Update the import bill details
                string sqlUpdateDetails = @"UPDATE CHITIET_HD_NHAP 
                                    SET SoLuongNhap = @SoLuongNhap, 
                                        DonGiaNhap = @DonGiaNhap, 
                                        ThanhTien = @ThanhTien 
                                    WHERE HDN_Ma = @HDN_Ma AND HH_Ma = @HH_Ma";
                MySqlCommand cmdUpdateDetails = new MySqlCommand(sqlUpdateDetails, conn, transaction);
                cmdUpdateDetails.Parameters.AddWithValue("@HDN_Ma", hdn.Hdn_Ma);
                cmdUpdateDetails.Parameters.AddWithValue("@HH_Ma", cthdn.Hh_Ma);
                cmdUpdateDetails.Parameters.AddWithValue("@SoLuongNhap", cthdn.SoLuongNhap);
                cmdUpdateDetails.Parameters.AddWithValue("@DonGiaNhap", cthdn.DonGiaNhap);
                cmdUpdateDetails.Parameters.AddWithValue("@ThanhTien", cthdn.SoLuongNhap * cthdn.DonGiaNhap);

                int resultDetails = cmdUpdateDetails.ExecuteNonQuery();

                updateQuantityProduct(cthdn); // Cập nhật số lượng hàng hóa

                // Commit the transaction
                transaction.Commit();

                return resultBill > 0 && resultDetails > 0;
            }
            catch (Exception ex)
            {
                // Handle exception
                MessageBox.Show("An error occurred: " + ex.Message);
                return false;
            }
            finally
            {
                conn.Close();
            }
        }

        public DataTable findNccByName(string name)
        {
            MySqlConnection conn = new MySqlConnection(stringConnect);
            try
            {
                conn.Open();
                string sql = @"SELECT * FROM nha_cung_cap WHERE NCC_Ten LIKE @NCC_Ten";
                MySqlCommand comd = new MySqlCommand(sql, conn);
                comd.Parameters.AddWithValue("@NCC_Ten", "%" + name + "%");
                DataTable data = new DataTable();
                data.Load(comd.ExecuteReader());




                return data;
            }
            finally
            {
                conn.Close();
            }

        }

        public void updateQuantityProduct(DTO_CTHoaDonNhap cthdn)
        {
            MySqlConnection conn = new MySqlConnection(stringConnect);
            try
            {
                conn.Open();
                string sql = @"UPDATE hanghoa 
                            SET HH_SoLuong = HH_SoLuong + @SoLuongNhap 
                            WHERE HH_Ma = @HH_Ma";
                MySqlCommand cmd = new MySqlCommand(sql, conn);
                cmd.Parameters.AddWithValue("@HH_Ma", cthdn.Hh_Ma);
                cmd.Parameters.AddWithValue("@SoLuongNhap", cthdn.SoLuongNhap);
                cmd.ExecuteNonQuery();
            }
            finally
            {
                conn.Close();
            }
        }
    }


}
