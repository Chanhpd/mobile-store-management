namespace DTO
{
    public class DTO_CTHoaDonXuat
    {
        private int hdx_Ma;
        private int hh_Ma;
        private int soLuongXuat;
        private float donGiaNhap;

        public DTO_CTHoaDonXuat() { }
        public DTO_CTHoaDonXuat(int hdx_Ma, int hh_Ma, int soLuongXuat, float donGiaNhap)
        {
            this.hdx_Ma = hdx_Ma;
            this.hh_Ma = hh_Ma;
            this.soLuongXuat = soLuongXuat;
            this.donGiaNhap = donGiaNhap;
        }

        public int Hdx_Ma { get => hdx_Ma; set => hdx_Ma = value; }
        public int Hh_Ma { get => hh_Ma; set => hh_Ma = value; }
        public int SoLuongXuat { get => soLuongXuat; set => soLuongXuat = value; }
        public float DonGiaNhap { get => donGiaNhap; set => donGiaNhap = value; }

    }
}
