using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTO
{
    public class DTO_PhanQuyen
    {
        private string pq_Ma;
        private string pq_Ten;
        private string pq_MoTa;
        private bool pq_TinhTrang;

        public DTO_PhanQuyen() { }

        public DTO_PhanQuyen(string pq_Ma, string pq_Ten, string pq_MoTa, bool pq_TinhTrang)
        {
            this.pq_Ma = pq_Ma;
            this.pq_Ten = pq_Ten;
            this.pq_MoTa = pq_MoTa;
            this.pq_TinhTrang = pq_TinhTrang;
        }

        public string Pq_Ma { get => pq_Ma; set => pq_Ma = value; }
        public string Pq_Ten { get => pq_Ten; set => pq_Ten = value; }
        public string Pq_MoTa { get => pq_MoTa; set => pq_MoTa = value; }
        public bool Pq_TinhTrang { get => pq_TinhTrang; set => pq_TinhTrang = value; }
        /*public string PQ_Ma { get; set; }
public string PQ_Ten { get; set; }
public string PQ_MoTa { get; set; }
public Boolean PQ_TinhTrang { get; set; }*/
    }
}
