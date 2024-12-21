using DAL;
using DTO;
using System.Data;

namespace BUS
{
    public class BUS_KhachHang
    {
        DAL_KhachHang dalkhachhang = new DAL_KhachHang();

        public DataTable ListCustomer()
        {
            return dalkhachhang.ListCustomers();
        }
        public bool InsertCustomer(DTO_KhachHang dtokhachhang)
        {
            return dalkhachhang.InsertCustomer(dtokhachhang);
        }
        public bool UpdateCustomer(DTO_KhachHang dtokhachhang)
        {
            return dalkhachhang.UpdateCustomer(dtokhachhang);
        }
        public bool DeleteCustomer(string makh)
        {
            return dalkhachhang.DeleteCustomer(makh);
        }
        public DataTable SearchCustomer(string kh)
        {
            return dalkhachhang.SearchCustomer(kh);
        }
    }
}
