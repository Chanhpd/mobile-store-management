using System;

namespace DTO
{
    public class DTO_TKHT
    {
        private string tkht_Email;
        private string tkht_HoTen;
        private string pq_Ma;
        private string tkht_Password;
        private string tkht_DiaChi;
        private string tkht_SoDienThoai;
        private bool tkht_GioiTinh;
        private DateTime tkht_NgaySinh;
        public DTO_TKHT()
        {

        }
        //đây là hàm dựng để khi khởi tạo được nhanh hơn
        public DTO_TKHT(string tkht_Email, string tkht_HoTen, string pq_Ma, string tkht_Password, string tkht_DiaChi, string tkht_SoDienThoai, bool tkht_GioiTinh, DateTime tkht_NgaySinh)
        {
            this.tkht_Email = tkht_Email;
            this.tkht_HoTen = tkht_HoTen;
            this.pq_Ma = pq_Ma;
            this.tkht_Password = tkht_Password;
            this.tkht_DiaChi = tkht_DiaChi;
            this.tkht_SoDienThoai = tkht_SoDienThoai;
            this.tkht_GioiTinh = tkht_GioiTinh;
            this.tkht_NgaySinh = tkht_NgaySinh;
        }

        public string Tkht_Email { get => tkht_Email; set => tkht_Email = value; }

        public string Tkht_HoTen { get => tkht_HoTen; set => tkht_HoTen = value; }
        public string Pq_Ma { get => pq_Ma; set => pq_Ma = value; }
        public string Tkht_Password { get => tkht_Password; set => tkht_Password = value; }
        public string Tkht_DiaChi { get => tkht_DiaChi; set => tkht_DiaChi = value; }
        public string Tkht_SoDienThoai { get => tkht_SoDienThoai; set => tkht_SoDienThoai = value; }
        public bool Tkht_GioiTinh { get => tkht_GioiTinh; set => tkht_GioiTinh = value; }
        public DateTime Tkht_NgaySinh { get => tkht_NgaySinh; set => tkht_NgaySinh = value; }

    }
}
