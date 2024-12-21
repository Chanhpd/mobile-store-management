namespace DTO
{
    public class DTO_KhachHang
    {
        private string ma_KH;
        private string ten_KH;
        private string diaChi;
        private string sDT;

        public DTO_KhachHang() { }

        public DTO_KhachHang(string ma_KH, string ten_KH, string diaChi, string sDT)
        {
            this.ma_KH = ma_KH;
            this.ten_KH = ten_KH;
            this.diaChi = diaChi;
            this.sDT = sDT;
        }

        public string Ma_KH { get => ma_KH; set => ma_KH = value; }
        public string Ten_KH { get => ten_KH; set => ten_KH = value; }
        public string DiaChi { get => diaChi; set => diaChi = value; }
        public string SDT { get => sDT; set => sDT = value; }

    }
}
