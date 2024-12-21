namespace DTO
{
    public class DTO_HoaDonNhap
    {
        private int hdn_Ma;
        private int ncc_Ma;
        private string tkht_Email;
        private string hdn_NgayLap;
        private float tongTien;

        public DTO_HoaDonNhap() { }
        public DTO_HoaDonNhap(int hdn_Ma, int ncc_Ma, string tkht_Email, string hdn_NgayLap, float tongTien)
        {
            this.hdn_Ma = hdn_Ma;
            this.ncc_Ma = ncc_Ma;
            this.tkht_Email = tkht_Email;
            this.hdn_NgayLap = hdn_NgayLap;
            this.tongTien = tongTien;
        }

        public int Hdn_Ma { get => hdn_Ma; set => hdn_Ma = value; }
        public int Ncc_Ma { get => ncc_Ma; set => ncc_Ma = value; }
        public string Tkht_Email { get => tkht_Email; set => tkht_Email = value; }
        public string Hdn_NgayLap { get => hdn_NgayLap; set => hdn_NgayLap = value; }

        public float TongTien { get => tongTien; set => tongTien = value; }

    }
}
