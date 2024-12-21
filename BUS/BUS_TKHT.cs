using DAL;
using DTO;
using System;
using System.Data;

namespace BUS
{
    public class BUS_TKHT
    {
        DAL_TKHT dalthkt = new DAL_TKHT();
        public string CheckLogin(DTO_TKHT TKHT)
        {
            // Kiểm tra nghiệp vụ
            if (TKHT.Tkht_Email == "")
            {
                return "requeid_taikhoan";
            }

            if (TKHT.Tkht_Password == "")
            {
                return "requeid_password";
            }
            string info = dalthkt.CheckLogin(TKHT);
            Console.WriteLine("Info " + info);
            return info;
        }
        public string checkRole(DTO_TKHT TKHT)
        {
            string role = dalthkt.checkRole(TKHT);
            return role;
        }
        public bool ChangePassword(string email, string oldPassword, string newPassword)
        {
            return dalthkt.ChangePassword(email, oldPassword, newPassword);
        }
        public DataTable ListStaff()
        {
            return dalthkt.ListStaff();
        }
        public DataTable GetStaff()
        {
            return dalthkt.PhanQuyen();
        }
        public bool InsertStaff(DTO_TKHT dtotkht)
        {
            return dalthkt.InsertNhanVien(dtotkht);
        }
        public bool UpdateStaff(DTO_TKHT dtotkht)
        {
            return dalthkt.UpdateNhanVien(dtotkht);
        }
        public bool DeleteStaff(string email)
        {
            return dalthkt.DeleteNhanVien(email);
        }
        public DataTable SearchStaff(string name)
        {
            return dalthkt.ListStaff(name);
        }

    }
}
