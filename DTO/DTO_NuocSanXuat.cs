using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTO
{
    public class DTO_NuocSanXuat
    {
        private string nsx_Ma;
        private string nsx_Ten;

        public DTO_NuocSanXuat() { }
        public DTO_NuocSanXuat(string nsx_Ma, string nsx_Ten)
        {
            this.nsx_Ma = nsx_Ma;
            this.nsx_Ten = nsx_Ten;
        }

        public string Nsx_Ma { get => nsx_Ma; set => nsx_Ma = value; }
        public string Nsx_Ten { get => nsx_Ten; set => nsx_Ten = value; }
        /*public string NSX_Ma { get; set; }
public string NSX_Ten { get; set; }*/
    }
}
