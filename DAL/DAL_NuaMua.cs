

using MySql.Data.MySqlClient;

namespace DAL
{
    public class DAL_NuaMua : DbConnect
    {
        public string CreateNewID(string query)
        {

            MySqlConnection conn = new MySqlConnection(stringConnect);
            conn.Open();
            MySqlCommand command = new MySqlCommand(query, conn);
            string largestHHMa = command.ExecuteScalar().ToString();
            string[] parts = largestHHMa.Split('_');  // Tách chuỗi thành mảng các chuỗi con bằng dấu '_'
            string a = parts[0];  // Lấy phần tử đầu tiên của mảng là chuỗi a
            string b = parts.Length > 1 ? parts[1] : "";  // Lấy phần tử thứ hai của mảng nếu có, nếu không có thì lấy chuỗi rỗng ""
            int intValue;
            int.TryParse(b, out intValue);
            int intB = intValue;
            intB++;
            a = a + '_';
            string newID = a + intB.ToString();
            conn.Close();
            //kết thúc code nửa
            return newID;
        }

    }
}
