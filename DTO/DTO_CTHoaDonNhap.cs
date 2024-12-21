namespace DTO
{
    public class DTO_CTHoaDonNhap
    {
        private int hdn_Ma;
        private int hh_Ma;
        private int soLuongNhap;
        private float donGiaNhap;

        public DTO_CTHoaDonNhap()
        {
        }
        public DTO_CTHoaDonNhap(int hdn_Ma, int hh_Ma, int soLuongNhap, float donGiaNhap)
        {
            this.hdn_Ma = hdn_Ma;
            this.hh_Ma = hh_Ma;
            this.soLuongNhap = soLuongNhap;
            this.donGiaNhap = donGiaNhap;
        }

        public int Hdn_Ma { get => hdn_Ma; set => hdn_Ma = value; }
        public int Hh_Ma { get => hh_Ma; set => hh_Ma = value; }
        public int SoLuongNhap { get => soLuongNhap; set => soLuongNhap = value; }
        public float DonGiaNhap { get => donGiaNhap; set => donGiaNhap = value; }
        /*public string HDN_Ma { get; set; }
public string HH_Ma { get; set; }
public int SoLuongNhap { get; set; }
public float DonGiaNhap { get; set; }*/
    }
}
