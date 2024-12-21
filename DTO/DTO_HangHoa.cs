using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTO
{
    public class DTO_HangHoa
    {
        private string hh_Ma;
        private string lh_Ma;
        private string nsx_Ma;
        private string ncc_Ma;
        private string hh_Ten;
        private int hh_SoLuong;
        private string hh_MoTa;
        private float hh_DonGia;
        private string hh_HinhAnh;



        //hàm dựng
        public DTO_HangHoa() 
        {
        }
        public DTO_HangHoa(string hh_Ma, string lh_Ma, string nsx_Ma, string ncc_ma, string hh_Ten, int hh_SoLuong, string hh_MoTa, float hh_DonGia, string hh_HinhAnh)
        {
            this.hh_Ma = hh_Ma;
            this.lh_Ma = lh_Ma;
            this.nsx_Ma = nsx_Ma;
            this.ncc_Ma = ncc_ma;
            this.hh_Ten = hh_Ten;
            this.Hh_SoLuong = hh_SoLuong;
            this.hh_MoTa = hh_MoTa;
            this.hh_DonGia = hh_DonGia;
            this.hh_HinhAnh = hh_HinhAnh;


        }


        public string Hh_Ma { get => hh_Ma; set => hh_Ma = value; }
        public string Lh_Ma { get => lh_Ma; set => lh_Ma = value; }
        public string Nsx_Ma { get => nsx_Ma; set => nsx_Ma = value; }
        public string Ncc_ma { get => ncc_Ma; set => ncc_Ma = value; }
        public string Hh_Ten { get => hh_Ten; set => hh_Ten = value; }
        public float Hh_DonGia { get => hh_DonGia; set => hh_DonGia = value; }
        public string Hh_MoTa { get => hh_MoTa; set => hh_MoTa = value; }
        public string Hh_HinhAnh { get => hh_HinhAnh; set => hh_HinhAnh = value; }
        public int Hh_SoLuong { get => hh_SoLuong; set => hh_SoLuong = value; }



        /*public string HH_Ma { get; set; }
public string LH_Ma { get; set; }
public string NSX_ma { get; set; }
public string HH_Ten { get; set; }
public string HH_MoTa { get; set; }
public float HH_DonGia { get; set; }
public string HH_HinhAnh { get; set; }*/
    }
}
