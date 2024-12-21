using MySql.Data.MySqlClient;
using System;
using System.Data;


namespace DAL
{
    public class DAL_NuocSanXuat : DbConnect
    {
        public DataTable GetProduct()
        {
            MySqlConnection conn = new MySqlConnection(stringConnect);
            DataTable table = new DataTable();
            try
            {
                string query = "SELECT * FROM NUOC_SAN_XUAT";
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
    }
}
