using MySql.Data.MySqlClient;
using System;
using System.Data;

using System.Windows.Forms;

namespace DAL
{
    public class DAL_LoaiHang : DbConnect
    {
        MySqlConnection conn = new MySqlConnection(stringConnect);
        public DataTable GetLoaiHang()
        {

            DataTable table = new DataTable();
            try
            {
                conn.Open();
                string query = "SELECT * FROM LOAI_HANG";
                MySqlDataAdapter data = new MySqlDataAdapter(query, conn);
                data.Fill(table);
            }
            catch (Exception)
            {
                MessageBox.Show("Không thể kết nối cơ sở dữ liệu");
            }
            finally
            {
                conn.Close();
            }
            return table;
        }


        public void HienThiDG(DataGridView dg, String search = "")
        {
            string sql = "SELECT * FROM LOAI_HANG WHERE LH_ten LIKE '%" + search + "%' OR LH_ma LIKE '%" + search + "%' OR LH_mota LIKE '%" + search + "%'";
            MySqlDataAdapter dt = new MySqlDataAdapter(sql, conn);

            DataSet dase = new DataSet();
            dt.Fill(dase, "a");

            dg.DataSource = dase;
            dg.DataMember = "a";

            if (dg.Columns["LH_ma"] != null)
            {
                dg.Columns["LH_ma"].HeaderText = "Mã Loại";
            }
            if (dg.Columns["LH_ten"] != null)
            {
                dg.Columns["LH_ten"].HeaderText = "Tên Loại";
            }
            if (dg.Columns["LH_mota"] != null)
            {
                dg.Columns["LH_mota"].HeaderText = "Mô Tả";
            }
        }
        public bool addProducts(string mloai, string tloai, string mota)
        {
            string query = "insert into LOAI_HANG values('" + mloai + "','" + tloai + "','" + mota + "')";

            try
            {
                conn.Open();
                MySqlCommand comd = new MySqlCommand(query, conn);
                comd.ExecuteNonQuery();
                return true;

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                Console.WriteLine(ex.Message);
                return false;
            }
            finally
            {
                conn.Close();
            }
        }
        public bool deleteProducts(string maHang)
        {
            string query_sql = "DELETE FROM LOAI_HANG WHERE LH_ma='" + maHang + "'";
            DialogResult result = MessageBox.Show("Bạn có chắc chắn muốn xóa?", "Xác nhận xóa", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

            if (result == DialogResult.Yes)
            {
                try
                {
                    conn.Open();
                    MySqlCommand comd = new MySqlCommand(query_sql, conn);
                    comd.ExecuteNonQuery();
                    MessageBox.Show("Xóa danh mục thành công");
                    return true;
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                    return false;
                }
                finally
                {
                    conn.Close();
                }
            }
            else
            {
                MessageBox.Show("Hủy xóa danh mục");
                return false;
            }
        }

        public bool updateProducts(string maloai, string tenloai, string mota)
        {
            string query = "UPDATE LOAI_HANG SET LH_ten=N'" + tenloai + "',LH_mota=N'" + mota + "' Where LH_ma='" + maloai + "'";
            try
            {
                conn.Open();
                MySqlCommand comd = new MySqlCommand(query, conn);
                comd.ExecuteNonQuery();

                return true;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                return false;
            }
            finally
            {
                conn.Close();
            }
        }
        public void ShowData(TextBox maloai, TextBox tenloai, TextBox mota, DataGridView dg, DataGridViewCellEventArgs e)
        {

        }
    }
}
