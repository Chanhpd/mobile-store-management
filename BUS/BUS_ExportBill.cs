using DAL;
using DTO;
using System.Data;

namespace BUS
{
    public class BUS_ExportBill
    {
        DAL_ExportBills dalHDX = new DAL_ExportBills();
        public DataTable ListExportBill(string ma)
        {
            return dalHDX.ListExportBills(ma);
        }
        public bool InsertExportBills(DTO_HoaDonXuat hdx, DTO_CTHoaDonXuat ct_hdx)
        {
            return dalHDX.InsertExportBills(hdx, ct_hdx);
        }
        public DataTable ListAllExportBills()
        {
            return dalHDX.ListAllExportBills();
        }

        public bool deleteExportBill(string ma)
        {
            return dalHDX.deleteExportBill(ma);
        }
        //public bool UpdateProduct(DTO_HangHoa product)
        //{
        //    return dalproduct.UpdateProduct(product);
        //}
        //public bool DeleteProduct(string hh_ma)
        //{
        //    //MessageBox.Show("Tầng BUS:" + hh_ma);
        //    return dalproduct.DeleteProduct(hh_ma);
        //}
        //public DataTable SearchProduct(string tenhang)
        //{
        //    return dalproduct.SearchProduct(tenhang);
        //}
    }
}
