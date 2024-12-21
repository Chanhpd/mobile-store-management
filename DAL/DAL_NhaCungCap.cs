using MySql.Data.MySqlClient;
using System;

using System.Data;
using System.Windows.Forms;

namespace DAL
{
    public class DAL_NhaCungCap : DbConnect
    {


        public DataTable GetNhaCungCap()
        {
            MySqlConnection conn = new MySqlConnection(stringConnect);
            DataTable table = new DataTable();
            try
            {
                conn.Open();
                string query = "SELECT * FROM nha_cung_cap";
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
        public DataTable findNCCByName(string name)
        {
            MySqlConnection conn = new MySqlConnection(stringConnect);
            DataTable table = new DataTable();
            try
            {
                conn.Open();
                string query = "SELECT * FROM nha_cung_cap WHERE NCC_Ten = @name";
                MySqlCommand cmd = new MySqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@name", name);
                MySqlDataAdapter data = new MySqlDataAdapter(cmd);
                data.Fill(table);
            }
            catch (Exception ex)
            {
                // Handle exception
                MessageBox.Show("An error occurred: " + ex.Message);
            }
            finally
            {
                conn.Close();
            }
            return table;
        }
    }
}
