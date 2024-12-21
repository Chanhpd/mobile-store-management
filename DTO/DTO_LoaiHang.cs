using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTO
{
    internal class DTO_LoaiHang
    {
        private string lh_Ma;
        private string lh_Ten;
        private string lh_MoTa;
        private bool lh_TrangThai;

        public DTO_LoaiHang() { }
        public DTO_LoaiHang(string lh_Ma, string lh_Ten, string lh_MoTa, bool lh_TrangThai)
        {
            this.lh_Ma = lh_Ma;
            this.lh_Ten = lh_Ten;
            this.lh_MoTa = lh_MoTa;
            this.lh_TrangThai = lh_TrangThai;
        }

        public string Lh_Ma { get => lh_Ma; set => lh_Ma = value; }
        public string Lh_Ten { get => lh_Ten; set => lh_Ten = value; }
        public string Lh_MoTa { get => lh_MoTa; set => lh_MoTa = value; }
        public bool Lh_TrangThai { get => lh_TrangThai; set => lh_TrangThai = value; }
        /*public string LH_Ma { get; set; }
public string LH_Ten { get; set; }
public string LH_MoTa { get; set; }
public bool LH_TrangThai { get; set; }*/
    }
}
