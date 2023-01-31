﻿using MB.Extensions;
using Microsoft.Owin.BuilderProperties;
using NHST.Bussiness;
using NHST.Controllers;
using NHST.Models;
using System;
using System.Collections.Generic;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Script.Serialization;
using System.Web.Services;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace NHST.manager
{
    public partial class VNWarehouse_DHH : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {

                if (Session["userLoginSystem"] == null)
                {
                    Response.Redirect("/trang-chu");
                }
                else
                {
                    string username_current = Session["userLoginSystem"].ToString();
                    tbl_Account ac = AccountController.GetByUsername(username_current);
                    if (ac.RoleID != 5 && ac.RoleID != 0 && ac.RoleID != 2)
                        Response.Redirect("/trang-chu");
                }
            }
        }

        [WebMethod]
        public static string GetListPackage(string barcode)
        {
            DateTime currentDate = DateTime.Now;
            if (HttpContext.Current.Session["userLoginSystem"] != null)
            {
                string username = HttpContext.Current.Session["userLoginSystem"].ToString();
                var user = AccountController.GetByUsername(username);
                if (user != null)
                {
                    int userRole = Convert.ToInt32(user.RoleID);

                    if (userRole == 0 || userRole == 2 || userRole == 5)
                    {
                        var bigpackage = BigPackageController.GetByPackageCode(barcode);
                        if (bigpackage != null)
                        {
                            int bID = bigpackage.ID;
                            BigPackageItem bi = new BigPackageItem();
                            bi.BigpackageID = bID;
                            bi.BigpackageCode = bigpackage.PackageCode;
                            bi.BigpackageType = 1;
                            List<smallpackageitem> sis = new List<smallpackageitem>();
                            var smallpackages = SmallPackageController.GetBuyBigPackageID(bID, "");
                            if (smallpackages.Count > 0)
                            {
                                foreach (var item in smallpackages)
                                {
                                    smallpackageitem si = new smallpackageitem();
                                    int mID = Convert.ToInt32(item.MainOrderID);
                                    int tID = Convert.ToInt32(item.TransportationOrderID);
                                    si.IMG = item.ListIMG;
                                    si.Note = item.Description;
                                    si.ID = item.ID;

                                    if (mID > 0)
                                    {
                                        si.OrderType = "Đơn hàng mua hộ";
                                        si.MainorderID = mID;
                                        si.TransportationID = 0;
                                        si.OrderTypeInt = 1;
                                        var mainorder = MainOrderController.GetAllByID(mID);
                                        if (mainorder != null)
                                        {
                                            int UID = Convert.ToInt32(mainorder.UID);
                                            si.UID = UID;
                                            var acc = AccountController.GetByID(UID);
                                            if (acc != null)
                                            {
                                                si.Username = acc.Username;
                                                si.Wallet = Convert.ToDouble(acc.Wallet);
                                                si.OrderShopCode = mainorder.MainOrderCode;
                                                if (mainorder.IsCheckProduct == true)
                                                    si.Kiemdem = "Có";
                                                else
                                                    si.Kiemdem = "Không";

                                                if (mainorder.IsPacked == true)
                                                    si.Donggo = "Có";
                                                else
                                                    si.Donggo = "Không";
                                                si.Baohiem = "Không";

                                                if (!string.IsNullOrEmpty(item.Description))
                                                    si.Description = item.Description;
                                                else
                                                    si.Description = string.Empty;

                                                if (!string.IsNullOrEmpty(item.UserNote))
                                                    si.Khachghichu = item.UserNote;
                                                else
                                                    si.Khachghichu = string.Empty;

                                                si.Loaisanpham = item.ProductType;

                                                if (!string.IsNullOrEmpty(item.StaffNoteCheck))
                                                    si.NVKiemdem = item.StaffNoteCheck;
                                                else
                                                    si.NVKiemdem = string.Empty;

                                                var orders = OrderController.GetByMainOrderID(mID);
                                                si.Soloaisanpham = orders.Count.ToString();
                                                double totalProductQuantity = 0;
                                                if (orders.Count > 0)
                                                {
                                                    foreach (var p in orders)
                                                    {
                                                        totalProductQuantity += Convert.ToDouble(p.quantity);
                                                    }
                                                }
                                                si.Soluongsanpham = totalProductQuantity.ToString();

                                                string Phone = "";

                                                var ai = AccountInfoController.GetByUserID(acc.ID);
                                                if (ai != null)
                                                {
                                                    si.Fullname = ai.FirstName + " " + ai.LastName;
                                                    si.Email = acc.Email;
                                                    Phone = ai.Phone;
                                                    si.Address = ai.Address;
                                                }
                                                si.Phone = Phone;
                                            }
                                        }
                                    }
                                    else if (tID > 0)
                                    {
                                        si.OrderType = "Đơn hàng VC hộ";
                                        si.MainorderID = 0;
                                        si.TransportationID = tID;
                                        si.OrderTypeInt = 2;
                                        int UID = 0;
                                        string Phone = "";
                                        string Username = "";
                                        var orderTransportation = TransportationOrderController.GetByID(Convert.ToInt32(item.TransportationOrderID));
                                        if (orderTransportation != null)
                                        {
                                            var userorder = AccountController.GetByID(orderTransportation.UID.Value);
                                            if (userorder != null)
                                            {
                                                UID = userorder.ID;
                                                Phone = AccountInfoController.GetByUserID(userorder.ID).Phone;
                                                username = userorder.Username;
                                            }
                                        }
                                        si.UID = UID;
                                        si.Phone = Phone;
                                        si.Username = username;
                                        si.Soluongsanpham = item.ProductQuantity;

                                        string kiemdem = "Không";
                                        string donggo = "Không";
                                        string baohiem = "Không";
                                        if (item.IsCheckProduct == true)
                                            kiemdem = "Có";
                                        if (item.IsPackaged == true)
                                            donggo = "Có";
                                        if (item.IsInsurrance == true)
                                            baohiem = "Có";
                                        si.Kiemdem = kiemdem;
                                        si.Donggo = donggo;
                                        si.Baohiem = baohiem;
                                    }
                                    else
                                    {
                                        si.OrderType = "Kiện chưa xác định";
                                        si.MainorderID = 0;
                                        si.TransportationID = 0;
                                        si.OrderTypeInt = 3;
                                        si.Username = "NA";
                                        si.Phone = "NA";
                                        si.Soluongsanpham = "0";

                                        string kiemdem = "Không";
                                        string donggo = "Không";
                                        string baohiem = "Không";
                                        if (item.IsCheckProduct == true)
                                            kiemdem = "Có";
                                        if (item.IsPackaged == true)
                                            donggo = "Có";
                                        if (item.IsInsurrance == true)
                                            baohiem = "Có";
                                        si.Kiemdem = kiemdem;
                                        si.Donggo = donggo;
                                        si.Baohiem = baohiem;
                                    }
                                    si.Weight = Convert.ToDouble(item.Weight);
                                    si.BarCode = item.OrderTransactionCode;
                                    si.Status = Convert.ToInt32(item.Status);

                                    if (!string.IsNullOrEmpty(item.Description))
                                        si.Description = item.Description;
                                    else
                                        si.Description = string.Empty;

                                    si.BigPackageID = bigpackage.ID;
                                    si.IsTemp = Convert.ToBoolean(item.IsTemp);
                                    if (item.IsLost != null)
                                        si.IsThatlac = Convert.ToBoolean(item.IsLost);
                                    else
                                        si.IsThatlac = false;
                                    if (item.IsHelpMoving != null)
                                        si.IsVCH = Convert.ToBoolean(item.IsHelpMoving);
                                    else
                                        si.IsVCH = false;
                                    double dai = 0;
                                    double rong = 0;
                                    double cao = 0;
                                    if (item.Length.ToString().ToFloat(0) > 0)
                                    {
                                        dai = Convert.ToDouble(item.Length);
                                    }
                                    if (item.Width.ToString().ToFloat(0) > 0)
                                    {
                                        rong = Convert.ToDouble(item.Width);
                                    }
                                    if (item.Height.ToString().ToFloat(0) > 0)
                                    {
                                        cao = Convert.ToDouble(item.Height);
                                    }
                                    si.dai = dai;
                                    si.rong = rong;
                                    si.cao = cao;



                                    if (!string.IsNullOrEmpty(item.UserNote))
                                        si.Khachghichu = item.UserNote;
                                    else
                                        si.Khachghichu = string.Empty;

                                    si.Loaisanpham = item.ProductType;
                                    if (!string.IsNullOrEmpty(item.StaffNoteCheck))
                                        si.NVKiemdem = item.StaffNoteCheck;
                                    else
                                        si.NVKiemdem = string.Empty;

                                    sis.Add(si);
                                }
                            }
                            bi.BigpackageSmallPackageCount = smallpackages.Count;
                            bi.Smallpackages = sis;
                            JavaScriptSerializer serializer = new JavaScriptSerializer();
                            return serializer.Serialize(bi);
                        }
                        else
                        {
                            BigPackageItem bi = new BigPackageItem();
                            bi.BigpackageID = 0;
                            bi.BigpackageCode = "";
                            bi.BigpackageType = 2;
                            List<smallpackageitem> sis = new List<smallpackageitem>();
                            var smallpackages = SmallPackageController.GetListByOrderTransactionCode(barcode);
                            if (smallpackages.Count > 0)
                            {
                                foreach (var item in smallpackages)
                                {
                                    SmallPackageController.UpdateStatusAndIsLostAndDateInKhoDich(item.ID, 3, false, currentDate, currentDate, username);
                                    SmallPackageController.UpdateDateInVNWareHouse(item.ID, username, currentDate);
                                    int bID = Convert.ToInt32(item.BigPackageID);
                                    if (bID > 0)
                                    {
                                        var big = BigPackageController.GetByID(bID);
                                        if (big != null)
                                        {
                                            bool checkIschua = false;
                                            var smalls = SmallPackageController.GetBuyBigPackageID(bID, "");
                                            if (smalls.Count > 0)
                                            {
                                                foreach (var s in smalls)
                                                {
                                                    if (s.Status < 3)
                                                        checkIschua = true;
                                                }
                                                if (checkIschua == false)
                                                {
                                                    BigPackageController.UpdateStatus(bID, 2, currentDate, username);
                                                }
                                            }
                                        }
                                    }

                                    smallpackageitem si = new smallpackageitem();
                                    int mID = Convert.ToInt32(item.MainOrderID);
                                    int tID = Convert.ToInt32(item.TransportationOrderID);
                                    si.ID = item.ID;
                                    si.IMG = item.ListIMG;
                                    si.Note = item.Description;
                                    if (mID > 0)
                                    {
                                        si.OrderType = "Đơn hàng mua hộ";
                                        si.MainorderID = mID;
                                        si.TransportationID = 0;
                                        si.OrderTypeInt = 1;
                                        var mainorder = MainOrderController.GetAllByID(mID);
                                        if (mainorder != null)
                                        {
                                            int UID = Convert.ToInt32(mainorder.UID);
                                            si.UID = UID;
                                            var acc = AccountController.GetByID(UID);
                                            if (acc != null)
                                            {
                                                si.Username = acc.Username;
                                                si.Wallet = Convert.ToDouble(acc.Wallet);
                                                si.OrderShopCode = mainorder.MainOrderCode;
                                                if (mainorder.IsCheckProduct == true)
                                                    si.Kiemdem = "Có";
                                                else
                                                    si.Kiemdem = "Không";

                                                if (mainorder.IsPacked == true)
                                                    si.Donggo = "Có";
                                                else
                                                    si.Donggo = "Không";
                                                si.Baohiem = "Không";

                                                if (!string.IsNullOrEmpty(item.UserNote))
                                                    si.Khachghichu = item.UserNote;
                                                else
                                                    si.Khachghichu = string.Empty;

                                                if (!string.IsNullOrEmpty(item.Description))
                                                    si.Description = item.Description;
                                                else
                                                    si.Description = string.Empty;

                                                si.Loaisanpham = item.ProductType;

                                                if (!string.IsNullOrEmpty(item.StaffNoteCheck))
                                                    si.NVKiemdem = item.StaffNoteCheck;
                                                else
                                                    si.NVKiemdem = string.Empty;

                                                var orders = OrderController.GetByMainOrderID(mID);
                                                si.Soloaisanpham = orders.Count.ToString();
                                                double totalProductQuantity = 0;
                                                if (orders.Count > 0)
                                                {
                                                    foreach (var p in orders)
                                                    {
                                                        totalProductQuantity += Convert.ToDouble(p.quantity);
                                                    }
                                                }
                                                si.Soluongsanpham = totalProductQuantity.ToString();
                                                string Phone = "";
                                                var ai = AccountInfoController.GetByUserID(acc.ID);
                                                if (ai != null)
                                                {
                                                    si.Fullname = ai.FirstName + " " + ai.LastName;
                                                    si.Email = acc.Email;
                                                    Phone = ai.Phone;
                                                    si.Address = ai.Address;
                                                }
                                                si.Phone = Phone;
                                            }
                                        }
                                    }
                                    else if (tID > 0)
                                    {
                                        si.OrderType = "Đơn hàng VC hộ";
                                        si.MainorderID = 0;
                                        si.TransportationID = tID;
                                        si.OrderTypeInt = 2;
                                        int UID = 0;
                                        string Phone = "";
                                        string Username = "";
                                        var orderTransportation = TransportationOrderController.GetByID(Convert.ToInt32(item.TransportationOrderID));
                                        if (orderTransportation != null)
                                        {
                                            var userorder = AccountController.GetByID(orderTransportation.UID.Value);
                                            if (userorder != null)
                                            {
                                                UID = userorder.ID;
                                                Phone = AccountInfoController.GetByUserID(userorder.ID).Phone;
                                                Username = userorder.Username;
                                            }
                                        }
                                        si.UID = UID;
                                        si.Phone = Phone;
                                        si.Username = Username;
                                        si.Soluongsanpham = item.ProductQuantity;
                                        string kiemdem = "Không";
                                        string donggo = "Không";
                                        string baohiem = "Không";
                                        if (item.IsCheckProduct == true)
                                            kiemdem = "Có";
                                        if (item.IsPackaged == true)
                                            donggo = "Có";
                                        if (item.IsInsurrance == true)
                                            baohiem = "Có";
                                        si.Kiemdem = kiemdem;
                                        si.Donggo = donggo;
                                        si.Baohiem = baohiem;
                                    }
                                    else
                                    {
                                        si.OrderType = "Kiện chưa xác định";
                                        si.MainorderID = 0;
                                        si.TransportationID = 0;
                                        si.OrderTypeInt = 3;
                                        si.Username = "NA";
                                        si.Phone = "NA";
                                        si.Soluongsanpham = "0";
                                        string kiemdem = "Không";
                                        string donggo = "Không";
                                        string baohiem = "Không";
                                        if (item.IsCheckProduct == true)
                                            kiemdem = "Có";
                                        if (item.IsPackaged == true)
                                            donggo = "Có";
                                        if (item.IsInsurrance == true)
                                            baohiem = "Có";
                                        si.Kiemdem = kiemdem;
                                        si.Donggo = donggo;
                                        si.Baohiem = baohiem;
                                    }
                                    si.Weight = Convert.ToDouble(item.Weight);
                                    si.BarCode = item.OrderTransactionCode;
                                    si.Status = 3;
                                    si.Description = item.Description;
                                    si.BigPackageID = 0;
                                    si.IsTemp = Convert.ToBoolean(item.IsTemp);
                                    if (item.IsLost != null)
                                        si.IsThatlac = Convert.ToBoolean(item.IsLost);
                                    else
                                        si.IsThatlac = false;
                                    if (item.IsHelpMoving != null)
                                        si.IsVCH = Convert.ToBoolean(item.IsHelpMoving);
                                    else
                                        si.IsVCH = false;
                                    double dai = 0;
                                    double rong = 0;
                                    double cao = 0;
                                    if (item.Length.ToString().ToFloat(0) > 0)
                                    {
                                        dai = Convert.ToDouble(item.Length);
                                    }
                                    if (item.Width.ToString().ToFloat(0) > 0)
                                    {
                                        rong = Convert.ToDouble(item.Width);
                                    }
                                    if (item.Height.ToString().ToFloat(0) > 0)
                                    {
                                        cao = Convert.ToDouble(item.Height);
                                    }
                                    si.dai = dai;
                                    si.rong = rong;
                                    si.cao = cao;



                                    if (!string.IsNullOrEmpty(item.Description))
                                        si.Description = item.Description;
                                    else
                                        si.Description = string.Empty;

                                    if (!string.IsNullOrEmpty(item.UserNote))
                                        si.Khachghichu = item.UserNote;
                                    else
                                        si.Khachghichu = string.Empty;

                                    si.Loaisanpham = item.ProductType;
                                    if (!string.IsNullOrEmpty(item.StaffNoteCheck))
                                        si.NVKiemdem = item.StaffNoteCheck;
                                    else
                                        si.NVKiemdem = string.Empty;
                                    sis.Add(si);
                                }

                                bi.BigpackageSmallPackageCount = smallpackages.Count;
                                bi.Smallpackages = sis;
                                JavaScriptSerializer serializer = new JavaScriptSerializer();
                                return serializer.Serialize(bi);
                            }

                        }
                    }
                }
            }
            return "none";
        }

        [WebMethod]
        public static string UpdateQuantityNew(string barcode, string quantity, int status, int BigPackageID,
           int packageID, double dai, double rong, double cao, string base64, string note,
           string nvkiemdem, string khachghichu, string loaisanpham)
        {
            string username_current = HttpContext.Current.Session["userLoginSystem"].ToString();
            DateTime currentDate = DateTime.Now;
            double quantityT = 0;
            if (quantity.ToFloat(0) > 0)
                quantityT = Math.Round(Convert.ToDouble(quantity), 1);
            double volume = 0;
            if (dai > 0 && rong > 0 && cao > 0)
                volume = Convert.ToDouble((dai * rong * cao) / 1000000);
            var package = SmallPackageController.GetByID(packageID);
            if (package != null)
            {
                if (status == 0)
                {
                    SmallPackageController.UpdateWeightStatus(package.ID, 0, status, BigPackageID, 0, 0, 0);
                    SmallPackageController.UpdateStaffNoteCustdescproducttype(package.ID, nvkiemdem,
                        khachghichu, loaisanpham);
                    SmallPackageController.UpdateNote(package.ID, note);
                    SmallPackageController.UpdateDateCancelWareHouse(package.ID, username_current, currentDate);
                    return "1";
                }
                else
                {
                    SmallPackageController.UpdateWeightStatusAndDateInLasteWareHouseIsLost(package.ID, quantityT, status,
                        currentDate, false, dai, rong, cao);
                    SmallPackageController.UpdateStaffNoteCustdescproducttype(package.ID, nvkiemdem,
                        khachghichu, loaisanpham);
                    int WareHouseID = 0;
                    if (status == 3)
                    {
                        SmallPackageController.UpdateDateInVNWareHouse(package.ID, username_current, currentDate);
                        string currentPlace = "";
                        var acc = AccountController.GetByUsername(username_current);
                        if (acc != null)
                        {
                            var wh = WarehouseController.GetByID(acc.WareHouseVN.Value);
                            if (wh != null)
                            {
                                WareHouseID = wh.ID;
                                currentPlace = wh.WareHouseName;
                            }
                            SmallPackageController.UpdateCurrentPlace(package.ID, currentPlace, WareHouseID);
                            HistoryScanPackageController.Insert(package.ID, username_current, currentDate, currentPlace);
                        }
                    }
                    SmallPackageController.UpdateNote(package.ID, note);

                    string dbIMG = package.ListIMG;
                    string[] listk = { };
                    if (!string.IsNullOrEmpty(package.ListIMG))
                    {
                        listk = dbIMG.Split('|');
                    }
                    string value = base64;
                    string link = "";
                    if (!string.IsNullOrEmpty(value))
                    {
                        string[] listIMG = value.Split('|');
                        for (int i = 0; i < listIMG.Length - 1; i++)
                        {
                            string imageData = listIMG[i];
                            bool ch = listk.Any(x => x == imageData);
                            if (ch == true)
                            {
                                link += imageData + "|";
                            }
                            else
                            {
                                string path = System.Web.HttpContext.Current.Server.MapPath("~/Uploads/smallpackageIMG/");
                                string date = DateTime.Now.ToString("dd-MM-yyyy");
                                string time = DateTime.Now.ToString("hh:mm tt");
                                Page page = (Page)HttpContext.Current.Handler;
                                //  TextBox txtCampaign = (TextBox)page.FindControl("txtCampaign");
                                string k = i.ToString();
                                string fileNameWitPath = path + k + "-" + DateTime.Now.ToString().Replace("/", "-").Replace(" ", "-").Replace(":", "") + ".png";
                                string linkIMG = "/Uploads/smallpackageIMG/" + k + "-" + DateTime.Now.ToString().Replace("/", "-").Replace(" ", "-").Replace(":", "") + ".png";
                                link += linkIMG + "|";
                                //   string fileNameWitPath = path + s + ".png";
                                byte[] data;
                                string convert;
                                string contenttype;

                                using (FileStream fs = new FileStream(fileNameWitPath, FileMode.Create))
                                {
                                    using (BinaryWriter bw = new BinaryWriter(fs))
                                    {
                                        if (imageData.Contains("data:image/png"))
                                        {
                                            convert = imageData.Replace("data:image/png;base64,", String.Empty);
                                            data = Convert.FromBase64String(convert);
                                            contenttype = ".png";
                                            var result = FileUploadCheck.isValidFile(data, contenttype, contenttype); // Validate Header
                                            if (result)
                                            {
                                                bw.Write(data);
                                                link += linkIMG + "|";
                                            }
                                        }
                                        else if (imageData.Contains("data:image/jpeg"))
                                        {
                                            convert = imageData.Replace("data:image/jpeg;base64,", String.Empty);
                                            data = Convert.FromBase64String(convert);
                                            contenttype = ".jpeg";
                                            var result = FileUploadCheck.isValidFile(data, contenttype, contenttype); // Validate Header
                                            if (result)
                                            {
                                                bw.Write(data);
                                                link += linkIMG + "|";
                                            }
                                        }
                                        else if (imageData.Contains("data:image/gif"))
                                        {
                                            convert = imageData.Replace("data:image/gif;base64,", String.Empty);
                                            data = Convert.FromBase64String(convert);
                                            contenttype = ".gif";
                                            var result = FileUploadCheck.isValidFile(data, contenttype, contenttype); // Validate Header
                                            if (result)
                                            {
                                                bw.Write(data);
                                                link += linkIMG + "|";
                                            }
                                        }
                                        else if (imageData.Contains("data:image/jpg"))
                                        {
                                            convert = imageData.Replace("data:image/jpg;base64,", String.Empty);
                                            data = Convert.FromBase64String(convert);
                                            contenttype = ".jpg";
                                            var result = FileUploadCheck.isValidFile(data, contenttype, contenttype); // Validate Header
                                            if (result)
                                            {
                                                bw.Write(data);
                                                link += linkIMG + "|";
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }

                    SmallPackageController.UpdateIMG(package.ID, link, DateTime.Now, username_current);
                    SmallPackageController.UpdateWeightVolumeStatus(package.ID, quantityT, volume, status, BigPackageID, dai, rong, cao);
                    int bID = Convert.ToInt32(package.BigPackageID);
                    if (bID > 0)
                    {
                        var big = BigPackageController.GetByID(bID);
                        if (big != null)
                        {
                            bool checkIschua = false;
                            var smalls = SmallPackageController.GetBuyBigPackageID(bID, "");
                            if (smalls.Count > 0)
                            {
                                foreach (var s in smalls)
                                {
                                    if (s.Status < 3)
                                        checkIschua = true;
                                }
                                if (checkIschua == false)
                                {
                                    BigPackageController.UpdateStatus(bID, 2, currentDate, username_current);
                                }
                            }
                        }
                    }
                    var mainorder = MainOrderController.GetAllByID(Convert.ToInt32(package.MainOrderID));
                    if (mainorder != null)
                    {
                        int orderID = mainorder.ID;
                        int warehouse = mainorder.ReceivePlace.ToInt(1);
                        int shipping = Convert.ToInt32(mainorder.ShippingType);
                        int warehouseFrom = Convert.ToInt32(mainorder.FromPlace);
                        bool checkIsChinaCome = true;
                        bool CheckIsKhoVN = true;
                        double totalweight = 0;
                        double totalvolume = 0;
                        totalweight = quantityT;
                        totalvolume = volume;
                        var packages = SmallPackageController.GetByMainOrderID(mainorder.ID);
                        if (packages.Count > 0)
                        {
                            foreach (var p in packages)
                            {
                                if (p.Status < 3)
                                    checkIsChinaCome = false;

                                if (p.CurrentPlaceID.ToString().ToInt(0) != mainorder.ReceivePlace.ToInt())
                                    CheckIsKhoVN = false;

                                if (p.OrderTransactionCode != barcode)
                                {
                                    totalweight += Math.Round(Convert.ToDouble(p.Weight), 1);
                                    totalvolume += Convert.ToDouble(p.Volume);
                                }
                            }
                        }

                        var usercreate = AccountController.GetByID(Convert.ToInt32(mainorder.UID));

                        double FeeWeight = 0;
                        double FeeWeightDiscount = 0;
                        double ckFeeWeight = 0;
                        ckFeeWeight = Convert.ToDouble(UserLevelController.GetByID(usercreate.LevelID.ToString().ToInt()).FeeWeight.ToString());

                        double FeeVolume = 0;
                        double FeeVolumetDiscount = 0;
                        double ckFeeVolume = 0;
                        ckFeeVolume = Convert.ToDouble(UserLevelController.GetByID(usercreate.LevelID.ToString().ToInt()).FeeVolume.ToString());

                        double returnpricecn = 0;
                        double returnpricett = 0;
                        double pricePerWeight = 0;
                        double pricePerVolume = 0;
                        double finalPriceOfPackageCN = 0;
                        double finalPriceOfPackageTT = 0;
                        var smallpackage = SmallPackageController.GetByMainOrderID(orderID);
                        if (smallpackage.Count > 0)
                        {
                            double totalWeight = 0;
                            double totalVolume = 0;
                            foreach (var item in smallpackage)
                            {
                                double totalWeightCN = Convert.ToDouble(item.Weight);
                                double totalWeightTT = Convert.ToDouble(item.Volume);

                                if (totalWeightCN > 0)
                                {
                                    totalWeight += totalWeightCN;
                                }
                                if (totalWeightTT > 0)
                                {
                                    totalVolume += totalWeightTT;
                                }
                            }
                            if (!string.IsNullOrEmpty(usercreate.FeeTQVNPerWeight))
                            {
                                double feetqvn = 0;
                                if (usercreate.FeeTQVNPerWeight.ToFloat(0) > 0)
                                {
                                    feetqvn = Convert.ToDouble(usercreate.FeeTQVNPerWeight);
                                }
                                returnpricecn = totalWeight * feetqvn;
                            }
                            if (!string.IsNullOrEmpty(usercreate.FeeTQVNPerVolume))
                            {
                                double feetqvn = 0;
                                if (usercreate.FeeTQVNPerVolume.ToFloat(0) > 0)
                                {
                                    feetqvn = Convert.ToDouble(usercreate.FeeTQVNPerVolume);
                                }
                                returnpricett = totalVolume * feetqvn;
                            }
                            else
                            {
                                var feecn = WarehouseFeeController.GetByAndWarehouseFromAndToWarehouseAndShippingTypeAndAndHelpMoving(warehouseFrom, warehouse, shipping, false);
                                if (feecn.Count > 0)
                                {
                                    foreach (var f in feecn)
                                    {
                                        if (totalWeight > f.WeightFrom && totalWeight <= f.WeightTo)
                                        {
                                            pricePerWeight = Convert.ToDouble(f.Price);
                                            returnpricecn = totalWeight * Convert.ToDouble(f.Price);
                                            break;
                                        }
                                    }
                                }
                                var feett = VolumeFeeController.GetByAndWarehouseFromAndToWarehouseAndShippingTypeAndAndHelpMoving(warehouseFrom, warehouse, shipping, false);
                                if (feett.Count > 0)
                                {
                                    foreach (var f in feett)
                                    {
                                        pricePerVolume = Convert.ToDouble(f.Price);
                                        returnpricett = totalVolume * Convert.ToDouble(f.Price);
                                        break;
                                    }
                                }
                            }

                            foreach (var item in smallpackage)
                            {
                                double compareweight = 0;
                                double compareSize = 0;

                                double weight = Math.Round(Convert.ToDouble(item.Weight), 1);
                                compareweight = weight * pricePerWeight;

                                double thetich = Convert.ToDouble(item.Volume);
                                compareSize = thetich * pricePerVolume;

                                if (compareweight > 0)
                                {
                                    finalPriceOfPackageCN += compareweight;
                                    SmallPackageController.UpdateTotalPrice(item.ID, compareweight);
                                }
                                if (compareSize > 0)
                                {
                                    finalPriceOfPackageTT += compareSize;
                                    SmallPackageController.UpdateTotalPrice(item.ID, compareSize);
                                }
                            }
                        }

                        double currency = Convert.ToDouble(mainorder.CurrentCNYVN);
                        FeeWeight = Math.Round(returnpricecn, 0);
                        FeeWeightDiscount = Math.Round(FeeWeight * ckFeeWeight / 100, 0);
                        FeeWeight = Math.Round(FeeWeight - FeeWeightDiscount, 0);

                        FeeVolume = Math.Round(returnpricett, 0);
                        FeeVolumetDiscount = Math.Round(FeeVolume * ckFeeVolume / 100, 0);
                        FeeVolume = Math.Round(FeeVolume - FeeVolumetDiscount, 0);

                        double FeeFinal = 0;
                        if (FeeWeight > FeeVolume)
                        {
                            FeeFinal = Math.Round(FeeWeight, 0);
                        }
                        else
                        {
                            FeeFinal = Math.Round(FeeVolume, 0);
                        }

                        double FeeShipCN = Math.Round(Convert.ToDouble(mainorder.FeeShipCN), 0);
                        double FeeBuyPro = Math.Round(Convert.ToDouble(mainorder.FeeBuyPro), 0);
                        double IsCheckProductPrice = Math.Round(Convert.ToDouble(mainorder.IsCheckProductPrice), 0);
                        double IsPackedPrice = Math.Round(Convert.ToDouble(mainorder.IsPackedPrice), 0);
                        double IsFastDeliveryPrice = Math.Round(Convert.ToDouble(mainorder.IsFastDeliveryPrice), 0);
                        double TotalFeeSupport = Math.Round(Convert.ToDouble(mainorder.TotalFeeSupport), 0);
                        double InsuranceMoney = Math.Round(Convert.ToDouble(mainorder.InsuranceMoney), 0);
                        double isfastprice = 0;
                        if (mainorder.IsFastPrice.ToFloat(0) > 0)
                            isfastprice = Math.Round(Convert.ToDouble(mainorder.IsFastPrice), 0);
                        double pricenvd = 0;
                        if (mainorder.PriceVND.ToFloat(0) > 0)
                            pricenvd = Math.Round(Convert.ToDouble(mainorder.PriceVND), 0);
                        double Deposit = Math.Round(Convert.ToDouble(mainorder.Deposit), 0);

                        double TotalPriceVND = FeeShipCN + FeeBuyPro + FeeFinal + IsCheckProductPrice + IsPackedPrice
                                                     + IsFastDeliveryPrice + isfastprice + pricenvd + TotalFeeSupport + InsuranceMoney;
                        TotalPriceVND = Math.Round(TotalPriceVND, 0);
                        MainOrderController.UpdateFeeNew(mainorder.ID, Deposit.ToString(), FeeShipCN.ToString(), FeeBuyPro.ToString(), FeeWeight.ToString(), FeeVolume.ToString(),
                            IsCheckProductPrice.ToString(),
                            IsPackedPrice.ToString(), IsFastDeliveryPrice.ToString(), TotalPriceVND.ToString());
                        MainOrderController.UpdateFeeWeightCK(mainorder.ID, FeeWeightDiscount.ToString(), FeeVolumetDiscount.ToString());
                        MainOrderController.UpdateTotalWeightVolume(mainorder.ID, totalweight.ToString(), totalweight.ToString(), totalvolume.ToString());
                        var accChangeData = AccountController.GetByUsername(username_current);
                        if (accChangeData != null)
                        {
                            if (status == 3)
                            {
                                HistoryOrderChangeController.Insert(mainorder.ID, accChangeData.ID, accChangeData.Username, accChangeData.Username +
               " đã đổi trạng thái của mã vận đơn: <strong>" + barcode
               + "</strong> của đơn hàng ID là: " + mainorder.ID + ", là: Đã về kho VN", 8, currentDate);
                            }

                            if (Convert.ToDouble(package.Weight) != Convert.ToDouble(quantity))
                            {
                                HistoryOrderChangeController.Insert(mainorder.ID, accChangeData.ID, accChangeData.Username, accChangeData.Username +
                                           " đã đổi cân nặng của mã vận đơn: <strong>" + barcode
                                           + "</strong> của đơn hàng ID là: " + mainorder.ID + ", từ: " + package.Weight + " , sang: " + quantity + "", 8, currentDate);
                            }

                            if (checkIsChinaCome == true)
                            {
                                int MainorderID = mainorder.ID;
                                var smallpackages = SmallPackageController.GetByMainOrderID(MainorderID);
                                if (smallpackages.Count > 0)
                                {
                                    bool isChuaVekhoTQ = true;
                                    var sp_main = smallpackages.Where(s => s.IsTemp != true).ToList();
                                    var sp_support_isvekhotq = smallpackages.Where(s => s.IsTemp == true && s.Status >= 3).ToList();
                                    var sp_main_isvekhotq = smallpackages.Where(s => s.IsTemp != true && s.Status >= 3).ToList();
                                    double che = sp_support_isvekhotq.Count + sp_main_isvekhotq.Count;
                                    if (che >= sp_main.Count)
                                    {
                                        isChuaVekhoTQ = false;
                                    }
                                    if (isChuaVekhoTQ == false)
                                    {
                                        if (CheckIsKhoVN)
                                        {
                                            if (mainorder.Status <= 7)
                                            {
                                                if (mainorder.DateVN == null)
                                                {
                                                    MainOrderController.UpdateDateVN(mainorder.ID, currentDate);
                                                }
                                                MainOrderController.UpdateStatus(MainorderID, Convert.ToInt32(mainorder.UID), 7);

                                                var setNoti = SendNotiEmailController.GetByID(9);
                                                if (setNoti != null)
                                                {
                                                    var acc = AccountController.GetByID(mainorder.UID.Value);
                                                    if (acc != null)
                                                    {
                                                        if (setNoti.IsSentNotiUser == true)
                                                        {
                                                            if (mainorder.OrderType == 1)
                                                            {
                                                                NotificationsController.Inser(acc.ID,
                                                                 acc.Username, MainorderID,
                                                                 "Hàng của đơn hàng " + MainorderID + " đã về kho VN.", 1,
                                                                 currentDate, username_current, true);
                                                            }
                                                            else
                                                            {
                                                                NotificationsController.Inser(acc.ID,
                                                                 acc.Username, MainorderID,
                                                                 "Hàng của đơn hàng TMĐT " + MainorderID + " đã về kho VN.", 11,
                                                                 currentDate, username_current, true);
                                                            }
                                                        }

                                                        if (setNoti.IsSendEmailUser == true)
                                                        {
                                                            try
                                                            {
                                                                PJUtils.SendMailGmail("no-reply@mona-media.com", "demonhunter", acc.Email,
                                                                    "Thông báo tại Taobao.vn.",
                                                                    "Hàng của đơn hàng " + MainorderID + " đã về kho VN.", "");
                                                            }
                                                            catch { }
                                                        }
                                                    }
                                                }
                                            }
                                        }
                                    }
                                }
                                HistoryOrderChangeController.Insert(mainorder.ID, accChangeData.ID, accChangeData.Username, accChangeData.Username +
                                                   " đã đổi trạng thái đơn hàng ID là: " + mainorder.ID + ", là: Đã về kho đích", 8, currentDate);
                            }
                        }

                        return "1";
                    }
                    else
                    {
                        var transportation = TransportationOrderController.GetByID(Convert.ToInt32(package.TransportationOrderID));
                        if (transportation != null)
                        {
                            int tID = transportation.ID;

                            int warehouseFrom = Convert.ToInt32(transportation.WarehouseFromID);
                            int warehouse = Convert.ToInt32(transportation.WarehouseID);
                            int shipping = Convert.ToInt32(transportation.ShippingTypeID);

                            bool checkIsChinaCome = true;
                            bool CheckIsKhoVN = true;
                            double totalweight = 0;

                            var packages = SmallPackageController.GetByTransportationOrderID(tID);
                            if (packages.Count > 0)
                            {
                                foreach (var p in packages)
                                {
                                    if (p.Status < 2)
                                        checkIsChinaCome = false;

                                    if (p.CurrentPlaceID.ToString().ToInt(0) != transportation.WarehouseID)
                                        CheckIsKhoVN = false;
                                }
                            }
                            var usercreate = AccountController.GetByID(Convert.ToInt32(transportation.UID));
                            double returnprice = 0;
                            double pricePerWeight = 0;
                            double finalPriceOfPackage = 0;

                            foreach (var item in packages)
                            {
                                double compareSize = 0;
                                double weight = Convert.ToDouble(item.Weight);

                                double pDai = Convert.ToDouble(item.Length);
                                double pRong = Convert.ToDouble(item.Width);
                                double pCao = Convert.ToDouble(item.Height);
                                if (pDai > 0 && pRong > 0 && pCao > 0)
                                {
                                    compareSize = (pDai * pRong * pCao) / 6000;
                                }

                                if (weight >= compareSize)
                                {
                                    totalweight += weight;
                                }
                                else
                                {
                                    totalweight += compareSize;
                                }
                            }

                            totalweight = Math.Round(totalweight, 1);
                            if (!string.IsNullOrEmpty(usercreate.FeeTQVNPerWeight))
                            {
                                double feetqvn = 0;
                                if (usercreate.FeeTQVNPerWeight.ToFloat(0) > 0)
                                {
                                    feetqvn = Convert.ToDouble(usercreate.FeeTQVNPerWeight);
                                }
                                returnprice = totalweight * feetqvn;
                            }
                            else
                            {
                                var fee = WarehouseFeeController.GetByAndWarehouseFromAndToWarehouseAndShippingTypeAndAndHelpMoving(
                                warehouseFrom, warehouse, shipping, true);
                                if (fee.Count > 0)
                                {
                                    foreach (var f in fee)
                                    {
                                        if (totalweight > f.WeightFrom && totalweight <= f.WeightTo)
                                        {
                                            pricePerWeight = Convert.ToDouble(f.Price);
                                            returnprice = totalweight * Convert.ToDouble(f.Price);
                                            break;
                                        }
                                    }
                                }
                            }


                            foreach (var item in packages)
                            {
                                double compareSize = 0;
                                double weight = Convert.ToDouble(item.Weight);
                                double pDai = Convert.ToDouble(item.Length);
                                double pRong = Convert.ToDouble(item.Width);
                                double pCao = Convert.ToDouble(item.Height);
                                if (pDai > 0 && pRong > 0 && pCao > 0)
                                {
                                    compareSize = (pDai * pRong * pCao) / 6000;
                                }
                                if (weight >= compareSize)
                                {
                                    double TotalPriceCN = Math.Round(weight * pricePerWeight, 0);
                                    SmallPackageController.UpdateTotalPrice(item.ID, TotalPriceCN);
                                }
                                else
                                {
                                    double TotalPriceTT = Math.Round(compareSize * pricePerWeight, 0);
                                    SmallPackageController.UpdateTotalPrice(item.ID, TotalPriceTT);
                                }
                            }
                            finalPriceOfPackage = Math.Round(returnprice, 0);

                            double currency = Convert.ToDouble(transportation.Currency);
                            double CheckProductFee = Convert.ToDouble(transportation.CheckProductFee);
                            double PackagedFee = Convert.ToDouble(transportation.PackagedFee);
                            double TotalCODTQVND = Convert.ToDouble(transportation.TotalCODTQVND);
                            double InsurranceFee = Convert.ToDouble(transportation.InsurranceFee);

                            double totalPriceVND = finalPriceOfPackage + CheckProductFee + PackagedFee
                                + TotalCODTQVND + InsurranceFee;

                            double totalPriceCYN = 0;
                            totalPriceCYN = Math.Round(returnprice / currency, 2);

                            var accChangeData = AccountController.GetByUsername(username_current);
                            if (accChangeData != null)
                            {

                                if (checkIsChinaCome == true)
                                {
                                    //MainOrderController.UpdateStatus(mainorder.ID, Convert.ToInt32(mainorder.UID), 6);
                                    var smallpackages = SmallPackageController.GetByTransportationOrderID(tID);
                                    if (smallpackages.Count > 0)
                                    {
                                        bool isChuaVekhoTQ = true;
                                        var sp_main = smallpackages.Where(s => s.IsTemp != true).ToList();
                                        var sp_support_isvekhotq = smallpackages.Where(s => s.IsTemp == true && s.Status >= 3).ToList();
                                        var sp_main_isvekhotq = smallpackages.Where(s => s.IsTemp != true && s.Status >= 3).ToList();
                                        double che = sp_support_isvekhotq.Count + sp_main_isvekhotq.Count;
                                        if (che >= sp_main.Count)
                                        {
                                            isChuaVekhoTQ = false;
                                        }
                                        if (isChuaVekhoTQ == false)
                                        {
                                            if (CheckIsKhoVN)
                                            {
                                                TransportationOrderController.UpdateStatus(tID, 5, currentDate, username_current);
                                                if (transportation.Status != 5)
                                                {
                                                    if (transportation.WarehouseID == WareHouseID)
                                                    {
                                                        var setNoti = SendNotiEmailController.GetByID(17);
                                                        if (setNoti != null)
                                                        {
                                                            var acc = AccountController.GetByID(transportation.UID.Value);
                                                            if (acc != null)
                                                            {
                                                                if (setNoti.IsSentNotiUser == true)
                                                                {
                                                                    NotificationsController.Inser(acc.ID,
                                                                          acc.Username, transportation.ID,
                                                                          "Đơn hàng vận chuyển hộ " + transportation.ID + " đã về kho VN.", 10,
                                                                          currentDate, username_current, true);
                                                                }

                                                                if (setNoti.IsSendEmailUser == true)
                                                                {
                                                                    try
                                                                    {
                                                                        PJUtils.SendMailGmail("no-reply@mona-media.com", "demonhunter",
                                                                            acc.Email,
                                                                            "Thông báo tại Taobao.vn.",
                                                                            "Đơn hàng vận chuyển hộ " + transportation.ID + " đã về kho VN.", "");
                                                                    }
                                                                    catch { }
                                                                }
                                                            }
                                                        }
                                                    }

                                                }
                                            }
                                        }
                                    }
                                }
                            }
                            TransportationOrderController.UpdateTotalWeightTotalPrice(tID, totalweight, totalPriceVND, currentDate, username_current);
                            return "1";
                        }
                        else
                        {
                            return "1";
                        }
                    }
                    return "none";
                }
            }
            return "none";
        }


        [WebMethod]
        public static string UpdateLost(int packageID)
        {
            string username_current = HttpContext.Current.Session["userLoginSystem"].ToString();
            DateTime currentDate = DateTime.Now;
            var package = SmallPackageController.GetByID(packageID);
            if (package != null)
            {
                int bID = Convert.ToInt32(package.BigPackageID);
                SmallPackageController.UpdateIsLost(packageID, true, 0);
                if (bID > 0)
                {
                    var big = BigPackageController.GetByID(bID);
                    if (big != null)
                    {
                        bool checkIschua = false;
                        var smalls = SmallPackageController.GetBuyBigPackageID(bID, "");
                        if (smalls.Count > 0)
                        {
                            foreach (var s in smalls)
                            {
                                if (s.Status < 3)
                                    checkIschua = true;
                            }
                            if (checkIschua == false)
                            {
                                BigPackageController.UpdateStatus(bID, 2, currentDate, username_current);
                            }
                        }
                    }
                }
                return "1";
            }
            return "none";
        }

        [WebMethod]
        public static string Delete(string PackageID)
        {
            if (HttpContext.Current.Session["userLoginSystem"] != null)
            {
                string username = HttpContext.Current.Session["userLoginSystem"].ToString();
                var user = AccountController.GetByUsername(username);
                if (user != null)
                {
                    int userRole = Convert.ToInt32(user.RoleID);

                    if (userRole == 0 || userRole == 2 || userRole == 5)
                    {
                        var check = SmallPackageController.GetByID(Convert.ToInt32(PackageID));
                        if (check != null)
                        {
                            SmallPackageController.Delete(check.ID);
                            return "ok";
                        }
                        else return "null";
                    }
                    else return "null";
                }
                else return "null";
            }
            else return "null";
        }

        [WebMethod]
        public static string CheckOrderShopCodeNew(string ordershopcode, string ordertransaction, string Description, string OrderID, string Username, string UserPhone)
        {
            if (HttpContext.Current.Session["userLoginSystem"] != null)
            {
                string username_check = HttpContext.Current.Session["userLoginSystem"].ToString();
                var user_check = AccountController.GetByUsername(username_check);
                if (user_check != null)
                {
                    int userRole_check = Convert.ToInt32(user_check.RoleID);

                    if (userRole_check == 0 || userRole_check == 2 || userRole_check == 4)
                    {
                        if (HttpContext.Current.Session["userLoginSystem"] != null)
                        {
                            string username = HttpContext.Current.Session["userLoginSystem"].ToString();
                            var user = AccountController.GetByUsername(username);
                            if (user != null)
                            {
                                int userRole = Convert.ToInt32(user.RoleID);

                                if (userRole == 0 || userRole == 2 || userRole == 4)
                                {
                                    //if (OrderType == 1)
                                    //{
                                    if (!string.IsNullOrEmpty(OrderID))
                                    {
                                        var order = MainOrderController.GetAllByID(Convert.ToInt32(OrderID));
                                        if (order != null)
                                        {
                                            int MainOrderID = order.ID;
                                            string temp = "";
                                            if (!string.IsNullOrEmpty(ordertransaction))
                                                temp = ordertransaction;
                                            else
                                                temp = ordershopcode + "-" + PJUtils.GetRandomStringByDateTime();
                                            var getsmallcheck = SmallPackageController.GetByOrderCode(temp);
                                            if (getsmallcheck.Count > 0)
                                            {
                                                return "existsmallpackage";
                                            }
                                            else
                                            {
                                                string packageID = SmallPackageController.InsertWithMainOrderIDUIDUsernameNew(order.ID, order.UID.Value, AccountController.GetByID(order.UID.Value).Username,
                                           0, temp, "", 0, 0, 0, 3, Description, DateTime.Now, username, ordershopcode.ToInt(0), 0);

                                                //    string packageID = SmallPackageController.InsertWithMainOrderIDAndIsTemp(MainOrderID,
                                                //0, temp, "", 0, 0, 0, 2, true, 0, DateTime.Now, username);
                                                SmallPackageController.UpdateUserPhoneAndUsername(Convert.ToInt32(packageID), Username, UserPhone);
                                                SmallPackageController.UpdateNote(Convert.ToInt32(packageID), Description);

                                                #region Lấy tất cả các cục hiện có trong đơn

                                                var smallpackages = SmallPackageController.GetByMainOrderID(MainOrderID);
                                                PackageAll pa = new PackageAll();
                                                pa.PackageAllType = 0;
                                                pa.PackageGetCount = smallpackages.Count;
                                                List<smallpackageitem> og = new List<smallpackageitem>();

                                                smallpackageitem o = new smallpackageitem();
                                                o.ID = packageID.ToInt(0);
                                                o.OrderType = "Đơn hàng mua hộ";
                                                o.BigPackageID = 0;
                                                o.BarCode = temp;

                                                o.Status = 1;
                                                int mainOrderID = Convert.ToInt32(MainOrderID);
                                                o.MainorderID = mainOrderID;
                                                o.OrderShopCode = order.MainOrderCode;
                                                var orders = OrderController.GetByMainOrderID(MainOrderID);
                                                o.Soloaisanpham = orders.Count.ToString();
                                                double totalProductQuantity = 0;
                                                if (orders.Count > 0)
                                                {
                                                    foreach (var p in orders)
                                                    {
                                                        totalProductQuantity += Convert.ToDouble(p.quantity);
                                                    }
                                                }
                                                o.Soluongsanpham = totalProductQuantity.ToString();
                                                if (order.IsCheckProduct == true)
                                                    o.Kiemdem = "Có";
                                                else
                                                    o.Kiemdem = "Không";
                                                if (order.IsPacked == true)
                                                    o.Donggo = "Có";
                                                else
                                                    o.Donggo = "Không";

                                                double dai = 0;
                                                double rong = 0;
                                                double cao = 0;

                                                o.dai = dai;
                                                o.rong = rong;
                                                o.cao = cao;
                                                og.Add(o);
                                                #endregion
                                                pa.listPackageGet = og;

                                                if (smallpackages.Count > 0)
                                                {
                                                    bool isChuaVekhoTQ = true;
                                                    var sp_main = smallpackages.Where(s => s.IsTemp != true).ToList();
                                                    var sp_support_isvekhotq = smallpackages.Where(s => s.IsTemp == true && s.Status > 1).ToList();
                                                    var sp_main_isvekhotq = smallpackages.Where(s => s.IsTemp != true && s.Status > 1).ToList();
                                                    double che = sp_support_isvekhotq.Count + sp_main_isvekhotq.Count;
                                                    if (che >= sp_main.Count)
                                                    {
                                                        isChuaVekhoTQ = false;
                                                    }
                                                    if (isChuaVekhoTQ == false)
                                                    {
                                                        MainOrderController.UpdateStatus(mainOrderID, Convert.ToInt32(order.UID), 6);
                                                    }
                                                }
                                                JavaScriptSerializer serializer = new JavaScriptSerializer();
                                                return serializer.Serialize(pa);
                                            }
                                        }
                                        else
                                            return "noteexistordercode";
                                    }
                                    else if (!string.IsNullOrEmpty(ordershopcode))
                                    {
                                        var moCode = MainOrderCodeController.GetByID(Convert.ToInt32(ordershopcode));
                                        if (moCode != null)
                                        {
                                            var order = MainOrderController.GetAllByID(moCode.MainOrderID.Value);
                                            if (order != null)
                                            {
                                                int MainOrderID = order.ID;
                                                string temp = "";
                                                if (!string.IsNullOrEmpty(ordertransaction))
                                                    temp = ordertransaction;
                                                else
                                                    temp = moCode.MainOrderCode + "-" + PJUtils.GetRandomStringByDateTime();
                                                var getsmallcheck = SmallPackageController.GetByOrderCode(temp);
                                                if (getsmallcheck.Count > 0)
                                                {
                                                    return "existsmallpackage";
                                                }
                                                else
                                                {
                                                    string packageID = SmallPackageController.InsertWithMainOrderIDUIDUsernameNew(order.ID, order.UID.Value, AccountController.GetByID(order.UID.Value).Username,
                                           0, temp, "", 0, 0, 0, 3, Description, DateTime.Now, username, ordershopcode.ToInt(0), 0);
                                                    SmallPackageController.UpdateNote(Convert.ToInt32(packageID), Description);
                                                    SmallPackageController.UpdateUserPhoneAndUsername(Convert.ToInt32(packageID), Username, UserPhone);
                                                    #region Lấy tất cả các cục hiện có trong đơn

                                                    var smallpackages = SmallPackageController.GetByMainOrderID(MainOrderID);
                                                    PackageAll pa = new PackageAll();
                                                    pa.PackageAllType = 0;
                                                    pa.PackageGetCount = smallpackages.Count;
                                                    List<smallpackageitem> og = new List<smallpackageitem>();

                                                    smallpackageitem o = new smallpackageitem();
                                                    o.ID = packageID.ToInt(0);
                                                    o.OrderType = "Đơn hàng mua hộ";
                                                    o.BigPackageID = 0;
                                                    o.BarCode = temp;

                                                    o.Status = 1;
                                                    int mainOrderID = Convert.ToInt32(MainOrderID);
                                                    o.MainorderID = mainOrderID;
                                                    o.OrderShopCode = order.MainOrderCode;
                                                    var orders = OrderController.GetByMainOrderID(MainOrderID);
                                                    o.Soloaisanpham = orders.Count.ToString();
                                                    double totalProductQuantity = 0;
                                                    if (orders.Count > 0)
                                                    {
                                                        foreach (var p in orders)
                                                        {
                                                            totalProductQuantity += Convert.ToDouble(p.quantity);
                                                        }
                                                    }
                                                    o.Soluongsanpham = totalProductQuantity.ToString();
                                                    if (order.IsCheckProduct == true)
                                                        o.Kiemdem = "Có";
                                                    else
                                                        o.Kiemdem = "Không";
                                                    if (order.IsPacked == true)
                                                        o.Donggo = "Có";
                                                    else
                                                        o.Donggo = "Không";

                                                    double dai = 0;
                                                    double rong = 0;
                                                    double cao = 0;

                                                    o.dai = dai;
                                                    o.rong = rong;
                                                    o.cao = cao;
                                                    og.Add(o);
                                                    #endregion
                                                    pa.listPackageGet = og;

                                                    if (smallpackages.Count > 0)
                                                    {
                                                        bool isChuaVekhoTQ = true;
                                                        var sp_main = smallpackages.Where(s => s.IsTemp != true).ToList();
                                                        var sp_support_isvekhotq = smallpackages.Where(s => s.IsTemp == true && s.Status > 1).ToList();
                                                        var sp_main_isvekhotq = smallpackages.Where(s => s.IsTemp != true && s.Status > 1).ToList();
                                                        double che = sp_support_isvekhotq.Count + sp_main_isvekhotq.Count;
                                                        if (che >= sp_main.Count)
                                                        {
                                                            isChuaVekhoTQ = false;
                                                        }
                                                        if (isChuaVekhoTQ == false)
                                                        {
                                                            MainOrderController.UpdateStatus(mainOrderID, Convert.ToInt32(order.UID), 6);
                                                        }
                                                    }
                                                    JavaScriptSerializer serializer = new JavaScriptSerializer();
                                                    return serializer.Serialize(pa);
                                                }
                                            }
                                            else
                                                return "noteexistordercode";
                                        }
                                        else
                                            return "noteexistordercode";
                                    }
                                    else
                                    {
                                        int MainOrderID = 0;
                                        string temp = "";
                                        if (!string.IsNullOrEmpty(ordertransaction))
                                            temp = ordertransaction;
                                        else
                                            temp = "00-" + PJUtils.GetRandomStringByDateTime();
                                        #region Lấy tất cả các cục hiện có trong đơn
                                        var getsmallcheck = SmallPackageController.GetByOrderCode(temp);
                                        if (getsmallcheck.Count > 0)
                                        {
                                            return "existsmallpackage";
                                        }
                                        else
                                        {
                                            string packageID = SmallPackageController.InsertWithMainOrderIDAndIsTemp(MainOrderID,
                                            0, temp, "", 0, 0, 0, 3, true, 0, DateTime.Now, username);
                                            SmallPackageController.UpdateNote(Convert.ToInt32(packageID), Description);
                                            SmallPackageController.UpdateUserPhoneAndUsername(Convert.ToInt32(packageID), Username, UserPhone);
                                            PackageAll pa = new PackageAll();
                                            pa.PackageAllType = 0;
                                            pa.PackageGetCount = 0;
                                            List<smallpackageitem> og = new List<smallpackageitem>();
                                            //string temp = "temp-" + PJUtils.GetRandomStringByDateTime();
                                            smallpackageitem o = new smallpackageitem();
                                            o.ID = packageID.ToInt(0);
                                            o.OrderType = "Chưa xác định";
                                            o.BigPackageID = 0;
                                            o.BarCode = temp;

                                            o.Status = 1;
                                            int mainOrderID = Convert.ToInt32(MainOrderID);
                                            o.MainorderID = mainOrderID;
                                            o.TransportationID = 0;
                                            o.OrderShopCode = "";

                                            o.Soloaisanpham = "0";
                                            o.Soluongsanpham = "0";
                                            o.Kiemdem = "Không";
                                            o.Donggo = "Không";

                                            double dai = 0;
                                            double rong = 0;
                                            double cao = 0;

                                            o.dai = dai;
                                            o.rong = rong;
                                            o.cao = cao;
                                            og.Add(o);

                                            pa.listPackageGet = og;
                                            JavaScriptSerializer serializer = new JavaScriptSerializer();
                                            return serializer.Serialize(pa);

                                        }
                                        #endregion
                                    }
                                    //}
                                    //else
                                    //{
                                    //    int MainOrderID = 0;
                                    //    string temp = "";
                                    //    if (!string.IsNullOrEmpty(ordertransaction))
                                    //        temp = ordertransaction;
                                    //    else
                                    //        temp = "00-" + PJUtils.GetRandomStringByDateTime();
                                    //    #region Lấy tất cả các cục hiện có trong đơn
                                    //    var getsmallcheck = SmallPackageController.GetByOrderCode(temp);
                                    //    if (getsmallcheck.Count > 0)
                                    //    {
                                    //        return "existsmallpackage";
                                    //    }
                                    //    else
                                    //    {
                                    //        string packageID = SmallPackageController.InsertWithMainOrderIDAndIsTemp(MainOrderID,
                                    //        0, temp, "", 0, 0, 0, 3, true, 0, DateTime.Now, username);
                                    //        SmallPackageController.UpdateNote(Convert.ToInt32(packageID), Description);

                                    //        PackageAll pa = new PackageAll();
                                    //        pa.PackageAllType = 0;
                                    //        pa.PackageGetCount = 0;
                                    //        List<smallpackageitem> og = new List<smallpackageitem>();
                                    //        //string temp = "temp-" + PJUtils.GetRandomStringByDateTime();
                                    //        smallpackageitem o = new smallpackageitem();
                                    //        o.ID = packageID.ToInt(0);
                                    //        o.OrderType = "Chưa xác định";
                                    //        o.BigPackageID = 0;
                                    //        o.BarCode = temp;

                                    //        o.Status = 1;
                                    //        int mainOrderID = Convert.ToInt32(MainOrderID);
                                    //        o.MainorderID = mainOrderID;
                                    //        o.TransportationID = 0;
                                    //        o.OrderShopCode = "";

                                    //        o.Soloaisanpham = "0";
                                    //        o.Soluongsanpham = "0";
                                    //        o.Kiemdem = "Không";
                                    //        o.Donggo = "Không";

                                    //        double dai = 0;
                                    //        double rong = 0;
                                    //        double cao = 0;

                                    //        o.dai = dai;
                                    //        o.rong = rong;
                                    //        o.cao = cao;
                                    //        og.Add(o);

                                    //        pa.listPackageGet = og;
                                    //        JavaScriptSerializer serializer = new JavaScriptSerializer();
                                    //        return serializer.Serialize(pa);
                                    //    }
                                    //    #endregion
                                    //}
                                }
                                else
                                    return "none";
                            }
                            else
                            {
                                return "none";
                            }
                        }
                        else
                        {
                            return "none";
                        }
                    }
                    else
                        return "none";
                }
                else
                {
                    return "none";
                }
            }
            else
            {
                return "none";
            }
        }

        [WebMethod]
        public static string AddPackageSame(string barcode)
        {
            if (HttpContext.Current.Session["userLoginSystem"] != null)
            {
                string username_check = HttpContext.Current.Session["userLoginSystem"].ToString();
                var user_check = AccountController.GetByUsername(username_check);
                if (user_check != null)
                {
                    int userRole_check = Convert.ToInt32(user_check.RoleID);

                    if (userRole_check == 0 || userRole_check == 2 || userRole_check == 4)
                    {
                        if (HttpContext.Current.Session["userLoginSystem"] != null)
                        {
                            string username = HttpContext.Current.Session["userLoginSystem"].ToString();
                            var user = AccountController.GetByUsername(username);
                            if (user != null)
                            {
                                int userRole = Convert.ToInt32(user.RoleID);

                                if (userRole == 0 || userRole == 2 || userRole == 4)
                                {
                                    var checksmall = SmallPackageController.GetByOrderTransactionCode(barcode);
                                    if (checksmall != null)
                                    {
                                        var order = MainOrderController.GetAllByID(Convert.ToInt32(checksmall.MainOrderID));
                                        if (order != null)
                                        {
                                            int MainOrderID = order.ID;

                                            string temp = barcode + "-" + DateTime.Now.Second.ToString();
                                            var getsmallcheck = SmallPackageController.GetByOrderCode(temp);
                                            if (getsmallcheck.Count > 0)
                                            {
                                                return "existsmallpackage";
                                            }
                                            else
                                            {
                                                string packageID = SmallPackageController.InsertWithMainOrderIDUIDUsernameNew(order.ID, order.UID.Value, AccountController.GetByID(order.UID.Value).Username,
                                               0, temp, "", 0, 0, 0, 3, "", DateTime.Now, username, Convert.ToInt32(checksmall.MainOrderCodeID), 0);

                                                //    string packageID = SmallPackageController.InsertWithMainOrderIDAndIsTemp(MainOrderID,
                                                //0, temp, "", 0, 0, 0, 2, true, 0, DateTime.Now, username);


                                                #region Lấy tất cả các cục hiện có trong đơn

                                                var smallpackages = SmallPackageController.GetByMainOrderID(MainOrderID);
                                                PackageAll pa = new PackageAll();
                                                pa.PackageAllType = 0;
                                                pa.PackageGetCount = smallpackages.Count;
                                                List<smallpackageitem> og = new List<smallpackageitem>();

                                                smallpackageitem o = new smallpackageitem();
                                                o.ID = packageID.ToInt(0);
                                                o.OrderType = "Đơn hàng mua hộ";
                                                o.BigPackageID = 0;
                                                o.BarCode = temp;

                                                o.Status = 1;
                                                int mainOrderID = Convert.ToInt32(MainOrderID);
                                                o.MainorderID = mainOrderID;
                                                o.OrderShopCode = order.MainOrderCode;
                                                var orders = OrderController.GetByMainOrderID(MainOrderID);
                                                o.Soloaisanpham = orders.Count.ToString();
                                                double totalProductQuantity = 0;
                                                if (orders.Count > 0)
                                                {
                                                    foreach (var p in orders)
                                                    {
                                                        totalProductQuantity += Convert.ToDouble(p.quantity);
                                                    }
                                                }
                                                o.Soluongsanpham = totalProductQuantity.ToString();
                                                if (order.IsCheckProduct == true)
                                                    o.Kiemdem = "Có";
                                                else
                                                    o.Kiemdem = "Không";
                                                if (order.IsPacked == true)
                                                    o.Donggo = "Có";
                                                else
                                                    o.Donggo = "Không";

                                                double dai = 0;
                                                double rong = 0;
                                                double cao = 0;

                                                o.dai = dai;
                                                o.rong = rong;
                                                o.cao = cao;
                                                og.Add(o);
                                                #endregion
                                                pa.listPackageGet = og;

                                                if (smallpackages.Count > 0)
                                                {
                                                    bool isChuaVekhoTQ = true;
                                                    var sp_main = smallpackages.Where(s => s.IsTemp != true).ToList();
                                                    var sp_support_isvekhotq = smallpackages.Where(s => s.IsTemp == true && s.Status > 1).ToList();
                                                    var sp_main_isvekhotq = smallpackages.Where(s => s.IsTemp != true && s.Status > 1).ToList();
                                                    double che = sp_support_isvekhotq.Count + sp_main_isvekhotq.Count;
                                                    if (che >= sp_main.Count)
                                                    {
                                                        isChuaVekhoTQ = false;
                                                    }
                                                    if (isChuaVekhoTQ == false)
                                                    {
                                                        MainOrderController.UpdateStatus(mainOrderID, Convert.ToInt32(order.UID), 6);
                                                    }
                                                }
                                                JavaScriptSerializer serializer = new JavaScriptSerializer();
                                                return serializer.Serialize(pa);

                                            }
                                        }
                                        else
                                            return "none";
                                    }
                                    else
                                        return "none";
                                }
                                else
                                    return "none";
                            }
                            else
                            {
                                return "none";
                            }
                        }
                        else
                        {
                            return "none";
                        }
                    }
                    else
                        return "none";
                }
                else
                {
                    return "none";
                }
            }
            else
            {
                return "none";
            }
        }

        [WebMethod]
        public static string GetOrder(string MainOrderCode, string Username)
        {
            if (HttpContext.Current.Session["userLoginSystem"] != null)
            {
                if (!string.IsNullOrEmpty(MainOrderCode) || !string.IsNullOrEmpty(Username))
                {
                    if (!string.IsNullOrEmpty(MainOrderCode) && string.IsNullOrEmpty(Username))
                    {
                        List<MainOrder> lmo = new List<MainOrder>();
                        var lm = MainOrderCodeController.GetContainMainOrderCode(MainOrderCode);
                        if (lm.Count > 0)
                        {
                            foreach (var item in lm)
                            {
                                MainOrder mo = new MainOrder();
                                var Mainorder = MainOrderController.GetByIDandStatus(item.MainOrderID.Value, 9);
                                if (Mainorder != null)
                                {
                                    mo.ID = Mainorder.ID;
                                    mo.MainOrderCode = item.MainOrderCode;
                                    mo.MainOrderCodeID = item.ID;
                                    var od = OrderController.GetByMainOrderID(Mainorder.ID);
                                    if (od.Count > 0)
                                    {
                                        List<Order> lo = new List<Order>();
                                        foreach (var temp in od)
                                        {
                                            Order o = new Order();
                                            o.Image = temp.image_origin;
                                            o.SoLuong = temp.quantity.ToInt();
                                            lo.Add(o);
                                        }
                                        mo.Order = lo;
                                    }
                                    lmo.Add(mo);
                                }
                            }
                            JavaScriptSerializer serializer = new JavaScriptSerializer();
                            return serializer.Serialize(lmo);
                        }
                        else
                        {
                            return null;
                        }
                    }
                    else if (string.IsNullOrEmpty(MainOrderCode) && !string.IsNullOrEmpty(Username))
                    {
                        List<MainOrder> lmo = new List<MainOrder>();
                        var uskh = AccountController.GetByUsername(Username);
                        if (uskh != null)
                        {
                            var MainOder = MainOrderController.GetByUID(uskh.ID);
                            if (MainOder.Count > 0)
                            {
                                foreach (var tod in MainOder)
                                {
                                    var lm = MainOrderCodeController.GetAllByMainOrderID(tod.ID);
                                    if (lm.Count > 0)
                                    {
                                        foreach (var item in lm)
                                        {
                                            MainOrder mo = new MainOrder();
                                            mo.ID = tod.ID;
                                            mo.MainOrderCode = item.MainOrderCode;
                                            mo.MainOrderCodeID = item.ID;
                                            var od = OrderController.GetByMainOrderID(tod.ID);
                                            if (od.Count > 0)
                                            {
                                                List<Order> lo = new List<Order>();
                                                foreach (var temp in od)
                                                {
                                                    Order o = new Order();
                                                    o.Image = temp.image_origin;
                                                    o.SoLuong = temp.quantity.ToInt();
                                                    lo.Add(o);
                                                }
                                                mo.Order = lo;
                                            }
                                            lmo.Add(mo);
                                        }
                                    }

                                }
                            }
                            JavaScriptSerializer serializer = new JavaScriptSerializer();
                            return serializer.Serialize(lmo);
                        }
                        else
                        {
                            return null;
                        }
                    }
                    else
                    {
                        List<MainOrder> lmo = new List<MainOrder>();
                        var lm = MainOrderCodeController.GetContainMainOrderCode(MainOrderCode);
                        if (lm.Count > 0)
                        {
                            var uskh = AccountController.GetByUsername(Username);
                            if (uskh != null)
                            {
                                foreach (var item in lm)
                                {
                                    MainOrder mo = new MainOrder();
                                    var Mainorder = MainOrderController.GetByIDAndUID(item.MainOrderID.Value, uskh.ID);
                                    if (Mainorder != null)
                                    {
                                        mo.ID = Mainorder.ID;
                                        mo.MainOrderCode = item.MainOrderCode;
                                        mo.MainOrderCodeID = item.ID;
                                        var od = OrderController.GetByMainOrderID(Mainorder.ID);
                                        if (od.Count > 0)
                                        {
                                            List<Order> lo = new List<Order>();
                                            foreach (var temp in od)
                                            {
                                                Order o = new Order();
                                                o.Image = temp.image_origin;
                                                o.SoLuong = temp.quantity.ToInt();
                                                lo.Add(o);
                                            }
                                            mo.Order = lo;
                                        }
                                        lmo.Add(mo);
                                    }
                                }
                            }
                            JavaScriptSerializer serializer = new JavaScriptSerializer();
                            return serializer.Serialize(lmo);
                        }
                        else
                        {
                            return null;
                        }
                    }
                }
                else
                {
                    return null;
                }
            }
            else
                return null;
        }

        //[WebMethod]
        //public static string GetOrder(string MainOrderCode)
        //{
        //    if (HttpContext.Current.Session["userLoginSystem"] != null)
        //    {
        //        if (!string.IsNullOrEmpty(MainOrderCode))
        //        {
        //            List<MainOrder> lmo = new List<MainOrder>();
        //            var lm = MainOrderCodeController.GetContainMainOrderCode(MainOrderCode);
        //            if (lm.Count > 0)
        //            {
        //                // string html = "<ul id=\"mainordercode-list\" class=\"input-field col s12\">";
        //                foreach (var item in lm)
        //                {
        //                    MainOrder mo = new MainOrder();
        //                    var Mainorder = MainOrderController.GetByIDandStatus(item.MainOrderID.Value, 9);
        //                    if (Mainorder != null)
        //                    {
        //                        mo.ID = Mainorder.ID;
        //                        mo.MainOrderCode = item.MainOrderCode;
        //                        mo.MainOrderCodeID = item.ID;
        //                        //html += "<li>" + item.MainOrderCode + " - ";
        //                        var od = OrderController.GetByMainOrderID(Mainorder.ID);
        //                        if (od.Count > 0)
        //                        {
        //                            List<Order> lo = new List<Order>();
        //                            foreach (var temp in od)
        //                            {
        //                                Order o = new Order();
        //                                o.Image = temp.image_origin;
        //                                o.SoLuong = temp.quantity.ToInt();
        //                                lo.Add(o);
        //                                //html += "<img alt=\"\" src=\"" + temp.image_origin + "\">";
        //                                //html += "<span style=\"color:red\">(" + temp.quantity + ")</span>; ";
        //                            }
        //                            mo.Order = lo;
        //                        }
        //                        //html += "</li>";
        //                        lmo.Add(mo);
        //                    }
        //                }
        //                //html += "</ul>";
        //                JavaScriptSerializer serializer = new JavaScriptSerializer();
        //                return serializer.Serialize(lmo);
        //                //return html;
        //            }
        //            else
        //            {
        //                return null;
        //            }
        //        }
        //        else
        //        {
        //            return null;
        //        }
        //    }
        //    else
        //        return null;
        //}

        [WebMethod]
        public static string PriceBarcode(string barcode)
        {
            if (HttpContext.Current.Session["userLoginSystem"] != null)
            {
                string username = HttpContext.Current.Session["userLoginSystem"].ToString();
                var user = AccountController.GetByUsername(username);
                if (user != null)
                {
                    int userRole = Convert.ToInt32(user.RoleID);

                    if (userRole == 0 || userRole == 2 || userRole == 5)
                    {
                        if (!string.IsNullOrEmpty(barcode))
                        {
                            BillInfo b = new BillInfo();
                            string Username = "";
                            string Phone = "";
                            string Fullname = "";
                            string Address = "";
                            string Email = "";
                            b.Weight = string.Empty;
                            var sm = SmallPackageController.GetByOrderTransactionCode(barcode);
                            if (sm != null)
                            {
                                if (Convert.ToDouble(sm.Weight) > 0)
                                    b.Weight = sm.Weight.ToString();
                                if (Convert.ToInt32(sm.MainOrderID) > 0)
                                {
                                    var main = MainOrderController.GetAllByID(Convert.ToInt32(sm.MainOrderID));
                                    if (main != null)
                                    {
                                        var ac = AccountController.GetByID(main.UID.Value);
                                        if (ac != null)
                                        {
                                            var accInfor = AccountInfoController.GetByUserID(ac.ID);
                                            Username = ac.Username;
                                            Phone = accInfor.Phone;
                                            Fullname = accInfor.FirstName + " " + accInfor.LastName;
                                            Address = accInfor.Address;
                                            Email = accInfor.Email;
                                        }
                                    }
                                }
                                else if (Convert.ToInt32(sm.TransportationOrderID) > 0)
                                {
                                    var trans = TransportationOrderController.GetByID(Convert.ToInt32(sm.TransportationOrderID));
                                    if (trans != null)
                                    {
                                        var ac = AccountController.GetByID(trans.UID.Value);
                                        if (ac != null)
                                        {
                                            var accInfor = AccountInfoController.GetByUserID(ac.ID);
                                            Username = ac.Username;
                                            Phone = accInfor.Phone;
                                            Fullname = accInfor.FirstName + accInfor.LastName;
                                            Address = accInfor.Address;
                                            Email = accInfor.Email;
                                        }
                                    }
                                }
                            }

                            b.Fullname = Fullname;
                            b.Address = Address;
                            b.Email = Email;
                            b.Username = Username;
                            b.Phone = Phone;


                            string barcodeIMG = "/Uploads/smallpackagebarcode/" + barcode + ".Png";
                            System.Drawing.Image barCode = PJUtils.MakeBarcodeImage(barcode, 2, true);
                            barCode.Save(HttpContext.Current.Server.MapPath("" + barcodeIMG + ""), ImageFormat.Png);
                            b.Barcode = barcode;
                            b.BarcodeURL = barcodeIMG;
                            JavaScriptSerializer serializer = new JavaScriptSerializer();
                            return serializer.Serialize(b);
                            //return barcodeIMG;
                        }
                        else
                        {
                            return "";
                        }
                    }
                    else
                        return "none";
                }
                else
                {
                    return "none";
                }
            }
            else
            {
                return "none";
            }

        }

        public class BillInfo
        {
            public string Barcode { get; set; }
            public string BarcodeURL { get; set; }
            public string Weight { get; set; }
            public string Phone { get; set; }
            public string Username { get; set; }
            public string Fullname { get; set; }
            public string Address { get; set; }
            public string Email { get; set; }
        }

        public class MainOrder
        {
            public int ID { get; set; }
            public int MainOrderCodeID { get; set; }
            public string MainOrderCode { get; set; }
            public List<Order> Order { get; set; }
        }

        public class Order
        {
            public string Image { get; set; }
            public int SoLuong { get; set; }
            public string Title { get; set; }
        }

        [WebMethod]
        public static string GetMainOrder(string MainOrderID)
        {
            if (HttpContext.Current.Session["userLoginSystem"] != null)
            {
                if (!string.IsNullOrEmpty(MainOrderID))
                {

                    MainOrder mo = new MainOrder();
                    var Mainorder = MainOrderController.GetAllByID(Convert.ToInt32(MainOrderID));
                    if (Mainorder != null)
                    {
                        mo.ID = Mainorder.ID;
                        var od = OrderController.GetByMainOrderID(Mainorder.ID);
                        if (od.Count > 0)
                        {
                            List<Order> lo = new List<Order>();
                            foreach (var temp in od)
                            {
                                Order o = new Order();
                                o.Title = temp.title_origin;
                                o.Image = temp.image_origin;
                                o.SoLuong = temp.quantity.ToInt();
                                lo.Add(o);
                            }
                            mo.Order = lo;
                        }
                    }
                    JavaScriptSerializer serializer = new JavaScriptSerializer();
                    return serializer.Serialize(mo);
                }
                else
                {
                    return null;
                }
            }
            else
                return null;
        }

        #region Class      
        //public class OrderGet
        //{
        //    public int ID { get; set; }
        //    public int MainorderID { get; set; }
        //    public int UID { get; set; }
        //    public string Username { get; set; }
        //    public double Wallet { get; set; }
        //    public string OrderShopCode { get; set; }
        //    public string BarCode { get; set; }
        //    public string TotalWeight { get; set; }
        //    public string TotalPriceVND { get; set; }
        //    public double TotalPriceVNDNum { get; set; }
        //    public int Status { get; set; }
        //    public string Fullname { get; set; }
        //    public string Email { get; set; }
        //    public string Phone { get; set; }
        //    public string Address { get; set; }
        //    public string Kiemdem { get; set; }
        //    public string Donggo { get; set; }
        //} 
        public class PackageAll
        {
            public int PackageAllType { get; set; }
            public string OrderCode { get; set; }
            public int PackageGetCount { get; set; }
            public List<smallpackageitem> listPackageGet { get; set; }
        }
        public class OrderGet
        {
            public int ID { get; set; }
            public int MainorderID { get; set; }
            public int UID { get; set; }
            public string Username { get; set; }
            public double Wallet { get; set; }
            public string OrderShopCode { get; set; }
            public string BarCode { get; set; }
            public string TotalWeight { get; set; }
            public string TotalPriceVND { get; set; }
            public double TotalPriceVNDNum { get; set; }
            public string Kiemdem { get; set; }
            public string Donggo { get; set; }
            public string Soloaisanpham { get; set; }
            public string Soluongsanpham { get; set; }
            public string Baohiem { get; set; }
            public string NVKiemdem { get; set; }
            public string Loaisanpham { get; set; }
            public string Khachghichu { get; set; }
            public int Status { get; set; }
            public int BigPackageID { get; set; }
            public List<tbl_BigPackage> ListBig { get; set; }
            public int IsTemp { get; set; }
            public int IsThatlac { get; set; }
            public string Fullname { get; set; }
            public string Email { get; set; }
            public string Phone { get; set; }
            public string Address { get; set; }
            public string Description { get; set; }
        }
        public class BigPackageItem
        {
            public int BigpackageID { get; set; }
            public string BigpackageCode { get; set; }
            public int BigpackageSmallPackageCount { get; set; }
            public int BigpackageType { get; set; }
            public List<smallpackageitem> Smallpackages { get; set; }
        }
        public class smallpackageitem
        {
            public int ID { get; set; }
            public string OrderType { get; set; }
            public int MainorderID { get; set; }
            public int TransportationID { get; set; }
            public int UID { get; set; }
            public string Username { get; set; }
            public double Wallet { get; set; }
            public string OrderShopCode { get; set; }
            public string BarCode { get; set; }
            public double Weight { get; set; }

            public string Kiemdem { get; set; }
            public string Donggo { get; set; }
            public string Soloaisanpham { get; set; }
            public string Soluongsanpham { get; set; }
            public string Baohiem { get; set; }
            public string NVKiemdem { get; set; }
            public string Loaisanpham { get; set; }
            public string Khachghichu { get; set; }
            public int Status { get; set; }
            public int BigPackageID { get; set; }
            public bool IsTemp { get; set; }
            public bool IsThatlac { get; set; }
            public bool IsVCH { get; set; }
            public string Fullname { get; set; }
            public string Email { get; set; }
            public string Phone { get; set; }
            public string Address { get; set; }
            public string Description { get; set; }
            public double dai { get; set; }
            public double rong { get; set; }
            public double cao { get; set; }
            public string IMG { get; set; }
            public string Note { get; set; }
            public int OrderTypeInt { get; set; }
        }
        #endregion
    }
}