using MB.Extensions;
using NHST.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using WebUI.Business;

namespace NHST.Controllers
{
    public class VolumeFeeController
    {
        public static tbl_VolumeFee GetByID(int ID)
        {
            using (var dbe = new NHSTEntities())
            {
                var c = dbe.tbl_VolumeFee.Where(p => p.ID == ID).FirstOrDefault();
                if (c != null)
                {
                    return c;
                }
                else
                    return null;
            }
        }

        public static string Update(int ID, int WarehouseFromID, int WarehouseID, double Price,
            int ShippingTypeToWareHouseID, int ShippingType, bool IsHidden, bool IsHelpMoving, DateTime ModifiedDate,
            string ModifiedBy)
        {
            using (var dbe = new NHSTEntities())
            {
                var c = dbe.tbl_VolumeFee.Where(p => p.ID == ID).FirstOrDefault();
                if (c != null)
                {
                    c.WarehouseFromID = WarehouseFromID;
                    c.WarehouseToID = WarehouseID;                   
                    c.Price = Price;
                    c.ShippingTypeToWareHouseID = ShippingTypeToWareHouseID;
                    c.ShippingType = ShippingType;
                    c.IsHidden = IsHidden;
                    c.IsHelpMoving = IsHelpMoving;
                    c.ModifiedDate = ModifiedDate;
                    c.ModifiedBy = ModifiedBy;
                    string kq = dbe.SaveChanges().ToString();
                    return kq;
                }
                else
                    return null;
            }
        }
        public static List<tbl_VolumeFee> GetByAndWarehouseFromAndToWarehouseAndShippingTypeAndAndHelpMoving(int WarehouseFromID, int WarehouseID, int ShippingTypeToWareHouseID, bool IsHelpMoving)
        {
            using (var dbe = new NHSTEntities())
            {
                List<tbl_VolumeFee> cs = new List<tbl_VolumeFee>();
                cs = dbe.tbl_VolumeFee.Where(c => c.WarehouseFromID == WarehouseFromID && c.WarehouseToID == WarehouseID && c.ShippingTypeToWareHouseID == ShippingTypeToWareHouseID && c.IsHelpMoving == IsHelpMoving).ToList();
                return cs;

            }
        }
        public static int GetTotal(string s)
        {
            var sql = @"select Total=Count(*) ";
            sql += "from tbl_VolumeFee as a left join tbl_WarehouseFrom as b on a.WarehouseFromID = b.ID ";
            sql += "left join tbl_Warehouse as c on a.WarehouseToID = c.ID ";
            sql += "left join tbl_ShippingTypeToWareHouse as d on d.ID=a.ShippingType ";
            sql += "Where b.WareHouseName Like N'%" + s + "%' or c.WareHouseName like N'%" + s + "%' ";
            var reader = (IDataReader)SqlHelper.ExecuteDataReader(sql);
            int a = 0;
            while (reader.Read())
            {
                if (reader["Total"] != DBNull.Value)
                    a = reader["Total"].ToString().ToInt(0);
            }
            reader.Close();
            return a;
        }

        public static List<WareHouseFeeVolume> GetAllBySQL(string s, int pageIndex, int pageSize)
        {
            var sql = @"select wareFrom=b.WareHouseName,wareTo=c.WareHouseName,shipName=d.ShippingTypeName, a.* ";
            sql += "from tbl_VolumeFee as a left join tbl_WarehouseFrom as b on a.WarehouseFromID = b.ID ";
            sql += "left join tbl_Warehouse as c on a.WarehouseToID = c.ID ";
            sql += "left join tbl_ShippingTypeToWareHouse as d on d.ID=a.ShippingType ";
            sql += "Where b.WareHouseName Like N'%" + s + "%' or c.WareHouseName like N'%" + s + "%' ";
            sql += "order by IsHelpMoving DESC OFFSET " + pageIndex + "*" + pageSize + " ROWS FETCH NEXT " + pageSize + " ROWS ONLY";
            var reader = (IDataReader)SqlHelper.ExecuteDataReader(sql);
            List<WareHouseFeeVolume> a = new List<WareHouseFeeVolume>();
            while (reader.Read())
            {
                var entity = new WareHouseFeeVolume();
                if (reader["ID"] != DBNull.Value)
                    entity.warehouseFee.ID = reader["ID"].ToString().ToInt(0);

                if (reader["wareFrom"] != DBNull.Value)
                    entity.wareFrom = reader["wareFrom"].ToString();

                if (reader["wareTo"] != DBNull.Value)
                    entity.wareTo = reader["wareTo"].ToString();               

                if (reader["Price"] != DBNull.Value)
                    entity.warehouseFee.Price = Convert.ToDouble(reader["Price"].ToString());

                if (reader["ShippingTypeToWareHouseID"] != DBNull.Value)
                    entity.warehouseFee.ShippingTypeToWareHouseID = reader["ShippingTypeToWareHouseID"].ToString().ToInt(0);

                if (reader["shipName"] != DBNull.Value)
                    entity.ShippingName = reader["shipName"].ToString();

                if (reader["IsHelpMoving"] != DBNull.Value)
                    entity.warehouseFee.IsHelpMoving = Convert.ToBoolean(reader["IsHelpMoving"]);
                a.Add(entity);
            }
            reader.Close();
            return a;
        }

        public partial class WareHouseFeeVolume
        {
            public tbl_VolumeFee warehouseFee { get; set; }
            public string wareFrom { get; set; }
            public string wareTo { get; set; }
            public string ShippingName { get; set; }
            public WareHouseFeeVolume()
            {
                warehouseFee = new tbl_VolumeFee();
            }
        }
    }
}