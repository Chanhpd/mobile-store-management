using DAL;
using DTO;
using System.Data;

namespace BUS
{
    public class BUS_ImportBill
    {
        DAL_ImportBill dalImportBill = new DAL_ImportBill();
        public DataTable ListImportBill(string ma)
        {
            return dalImportBill.ListImportBills(ma);
        }
        public DataTable ListAllImportBill()
        {
            return dalImportBill.ListAllImportBills();
        }
        public DataTable findNccByName(string name)
        {
            DAL_NhaCungCap dalNhaCungCap = new DAL_NhaCungCap();
            return dalNhaCungCap.findNCCByName(name);
        }
        public bool insertImportBill(DTO_HoaDonNhap hdn, DTO_CTHoaDonNhap ct_hdn)
        {
            return dalImportBill.insertImportBill(hdn, ct_hdn);
        }

        public bool deleleImportBill(int ma)
        {
            return dalImportBill.deleteImportBill(ma);
        }

        public bool updateImportBill(DTO_HoaDonNhap hdn, DTO_CTHoaDonNhap ct_hdn)
        {
            return dalImportBill.updateImportBill(hdn, ct_hdn);
        }

    }
}
