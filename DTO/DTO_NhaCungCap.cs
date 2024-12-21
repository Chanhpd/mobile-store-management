using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTO
{
    public class DTO_NhaCungCap
    {
        private string ncc_Ma;
        private string ncc_Ten;
        private string ncc_DiaChi;
        private string ncc_SDT;

        public DTO_NhaCungCap() { }
        public DTO_NhaCungCap(string ncc_Ma, string ncc_Ten, string ncc_DiaChi, string ncc_SDT)
        {
            this.ncc_Ma = ncc_Ma;
            this.ncc_Ten = ncc_Ten;
            this.ncc_DiaChi = ncc_DiaChi;
            this.ncc_SDT = ncc_SDT;
        }

        public string Ncc_Ma { get => ncc_Ma; set => ncc_Ma = value; }
        public string Ncc_Ten { get => ncc_Ten; set => ncc_Ten = value; }
        public string Ncc_DiaChi { get => ncc_DiaChi; set => ncc_DiaChi = value; }
        public string Ncc_SDT { get => ncc_SDT; set => ncc_SDT = value; }
        /*public string NCC_Ma { get; set; }
public string NCC_Ten { get; set; }
public string NCC_DiaChi { get; set; }
public string NCC_SDT { get; set; }*/
    }
}
