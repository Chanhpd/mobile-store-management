namespace DTO
{
    public class DTO_HoaDonXuat
    {
        private int hdx_Ma;
        private int ma_KH;
        private string tkht_Email;
        private string hdx_NgayLap;
        private float tongTien;

        public DTO_HoaDonXuat() { }

        public DTO_HoaDonXuat(int hdx_Ma, int ma_KH, string tkht_Email, string hdx_NgayLap, float tongTien)
        {
            this.hdx_Ma = hdx_Ma;
            this.ma_KH = ma_KH;
            this.tkht_Email = tkht_Email;
            this.hdx_NgayLap = hdx_NgayLap;
            this.tongTien = tongTien;
        }

        public int Hdx_Ma { get => hdx_Ma; set => hdx_Ma = value; }
        public int Ma_KH { get => ma_KH; set => ma_KH = value; }
        public string Tkht_Email { get => tkht_Email; set => tkht_Email = value; }
        public string Hdx_NgayLap { get => hdx_NgayLap; set => hdx_NgayLap = value; }
        public float TongTien { get => tongTien; set => tongTien = value; }

    }
}
