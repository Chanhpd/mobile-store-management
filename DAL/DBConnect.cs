using DTO;
using MySql.Data.MySqlClient;
using System;
using System.Data;

namespace DAL
{

    public class SqlConnectionData
    {
        public static MySqlConnection Connect()
        {
            string stringConnect = @"server=localhost;Database=ql_cuahang_dienthoai;Uid=root;password=;SslMode=none";
            MySqlConnection conn = new MySqlConnection(stringConnect);
            //conn.ConnectionString = stringConnect;
            //conn.Open();
            return conn;
        }


    }
    public class DbConnect
    {
        public static string stringConnect = @"server=localhost;Database=ql_cuahang_dienthoai;Uid=root;password=;SslMode=none";

        public static string CheckLoginDTO(DTO_TKHT tkht)
        {
            string user = null;
            string query = @"
        SELECT *
        FROM tai_khoan_he_thong 
        WHERE TKHT_Email = @email AND TKHT_Password = @pass";

            using (MySqlConnection conn = SqlConnectionData.Connect())
            {
                try
                {
                    conn.Open();
                    using (MySqlCommand command = new MySqlCommand(query, conn))
                    {
                        // Thêm tham số
                        command.Parameters.AddWithValue("@email", tkht.Tkht_Email);
                        command.Parameters.AddWithValue("@pass", tkht.Tkht_Password);

                        using (MySqlDataReader reader = command.ExecuteReader())
                        {
                            if (reader.HasRows)
                            {
                                while (reader.Read())
                                {
                                    //user = reader.GetInt32(1).ToString(); // Lấy giá trị TKHT_Ma
                                    user = reader.GetString(1); // Lấy giá trị TKHT_Email


                                }
                            }
                            else
                            {
                                user = "wrong_info";
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    // Log lỗi nếu cần thiết
                    Console.WriteLine($"Lỗi khi đăng nhập: {ex.Message}");
                    user = "Đã xảy ra lỗi hệ thống.";
                }
            }

            return user;
        }

        public string checkRoleDTO(DTO_TKHT tkht)
        {
            MySqlConnection conn = SqlConnectionData.Connect();
            conn.Open();
            MySqlCommand command = new MySqlCommand("select PQ_Ma from TAI_KHOAN_HE_THONG WHERE TKHT_Email = @username", conn);
            command.CommandType = CommandType.Text;
            command.Connection = conn;
            command.Parameters.AddWithValue("@username", tkht.Tkht_Email);
            string role = (string)command.ExecuteScalar().ToString();
            return role;

        }
    }
}
