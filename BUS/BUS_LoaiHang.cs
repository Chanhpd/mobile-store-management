using DAL;
using System.Windows.Forms;
namespace BUS
{

    public class BUS_LoaiHang
    {
        DAL_LoaiHang dal_LoaiHang = new DAL_LoaiHang();

        public void loadTableLoaiHang(DataGridView dg, string search = "")
        {
            dal_LoaiHang.HienThiDG(dg, search);
        }
        public bool insertLoaiHang(string mloai, string tloai, string mota)
        {
            if (dal_LoaiHang.addProducts(mloai, tloai, mota))
            {
                return true;
            }
            return false;

        }

        public bool updateLoaiHang(string mloai, string tloai, string mota)
        {
            if (dal_LoaiHang.updateProducts(mloai, tloai, mota))
            {
                return true;
            }
            return false;
        }

        public bool deleteLoaiHang(string mloai)
        {
            if (dal_LoaiHang.deleteProducts(mloai))
            {
                return true;
            }
            return false;
        }

    }
}
