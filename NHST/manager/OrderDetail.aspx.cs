﻿using NHST.Models;
using NHST.Controllers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.Services;
using System.Net;
using Supremes;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;
using NHST.Bussiness;
using MB.Extensions;
using Telerik.Web.UI;
using Microsoft.AspNet.SignalR;
using NHST.Hubs;
using System.Web.Script.Serialization;

namespace NHST.manager
{
    public partial class OrderDetail : System.Web.UI.Page
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
                    int RoleID = Convert.ToInt32(ac.RoleID);
                    if (ac.RoleID == 1)
                        Response.Redirect("/trang-chu");
                    else
                    {
                        if (RoleID == 4 || RoleID == 5 || RoleID == 8)
                        {
                            Response.Redirect("/manager/home.aspx");
                        }
                    }
                }              
                checkOrderStaff();
                LoadDDL();
                loaddata();
            }
        }
       
        public void checkOrderStaff()
        {
            string username_current = Session["userLoginSystem"].ToString();
            var obj_user = AccountController.GetByUsername(username_current);
            if (obj_user != null)
            {
                int RoleID = obj_user.RoleID.ToString().ToInt();
                int UID = obj_user.ID;
                var id = Convert.ToInt32(Request.QueryString["id"]);
                if (id > 0)
                {
                    var o = MainOrderController.GetAllByID(id);
                    if (o != null)
                    {
                        int status_order = Convert.ToInt32(o.Status);
                        if (RoleID == 0 || RoleID == 2)
                        {

                        }
                        else if (RoleID == 3)
                        {
                            if (status_order >= 2)
                            {
                                //Role đặt hàng
                                if (o.DathangID == UID)
                                {
                                }
                                else
                                {
                                    Response.Redirect("/manager/OrderList.aspx");
                                }
                            }
                            else
                            {
                                Response.Redirect("/manager/OrderList.aspx");
                            }

                        }
                        else if (RoleID == 4)
                        {
                            if (status_order >= 5 && status_order < 7)
                            {
                                //Role kho TQ
                                if (o.KhoTQID == UID || o.KhoTQID == 0)
                                {

                                }
                                else
                                {
                                    Response.Redirect("/manager/OrderList.aspx");
                                }
                            }
                            else
                            {
                                Response.Redirect("/manager/OrderList.aspx");
                            }

                        }
                        else if (RoleID == 5)
                        {
                            if (status_order >= 5 && status_order <= 7)
                            {
                                //Role Kho VN
                                if (o.KhoVNID == UID || o.KhoVNID == 0)
                                {

                                }
                                else
                                {
                                    Response.Redirect("/manager/OrderList.aspx");
                                }
                            }
                            else
                            {
                                Response.Redirect("/manager/OrderList.aspx");
                            }

                        }
                        else if (RoleID == 6)
                        {
                            if (status_order != 1)
                            {

                                //Role sale
                                if (o.SalerID == UID)
                                {

                                }
                                else
                                {
                                    Response.Redirect("/manager/OrderList.aspx");
                                }
                            }
                            else
                            {
                                Response.Redirect("/manager/OrderList.aspx");
                            }
                        }
                        else if (RoleID == 7)
                        {
                            if (status_order >= 2)
                            {

                            }
                            else
                            {
                                Response.Redirect("/manager/OrderList.aspx");
                            }
                        }
                        else if (RoleID == 8)
                        {
                            if (status_order >= 9 && status_order < 10)
                            {

                            }
                            else
                            {
                                Response.Redirect("/manager/OrderList.aspx");
                            }
                        }
                    }
                }
                else
                {
                    Response.Redirect("/manager/OrderList.aspx");
                }
            }
        }
        public void LoadDDL()
        {
            ddlSaler.Items.Clear();
            ddlSaler.Items.Insert(0, "Chọn Saler");

            ddlDatHang.Items.Clear();
            ddlDatHang.Items.Insert(0, "Chọn nhân viên đặt hàng");

            ddlKhoTQ.Items.Clear();
            ddlKhoTQ.Items.Insert(0, "Chọn nhân viên kho Trung");

            ddlKhoVN.Items.Clear();
            ddlKhoVN.Items.Insert(0, "Chọn nhân viên kho Việt");

            var salers = AccountController.GetAllByRoleID(6);
            if (salers.Count > 0)
            {
                ddlSaler.DataSource = salers;
                ddlSaler.DataBind();
            }

            var dathangs = AccountController.GetAllByRoleID(3);
            if (dathangs.Count > 0)
            {
                ddlDatHang.DataSource = dathangs;
                ddlDatHang.DataBind();
            }

            var khotqs = AccountController.GetAllByRoleID(4);
            if (khotqs.Count > 0)
            {
                ddlKhoTQ.DataSource = khotqs;
                ddlKhoTQ.DataBind();
            }

            var khovns = AccountController.GetAllByRoleID(5);
            if (khovns.Count > 0)
            {
                ddlKhoVN.DataSource = khovns;
                ddlKhoVN.DataBind();
            }
            var warehousefrom = WarehouseFromController.GetAllWithIsHidden(false);
            if (warehousefrom.Count > 0)
            {
                ddlWarehouseFrom.DataSource = warehousefrom;
                ddlWarehouseFrom.DataBind();
            }


            var warehouse = WarehouseController.GetAllWithIsHidden(false);
            if (warehouse.Count > 0)
            {
                ddlReceivePlace.DataSource = warehouse;
                ddlReceivePlace.DataBind();
            }

            var shippingtype = ShippingTypeToWareHouseController.GetAllWithIsHidden(false);
            if (shippingtype.Count > 0)
            {
                ddlShippingType.DataSource = shippingtype;
                ddlShippingType.DataBind();
            }
        }
        public void loaddata()
        {
            var config = ConfigurationController.GetByTop1();
            double currency = 0;
            double currency1 = 0;
            if (config != null)
            {
                double currencyconfig = 0;
                if (!string.IsNullOrEmpty(config.Currency))
                    currencyconfig = Convert.ToDouble(config.Currency);

                currency = Math.Round(currencyconfig, 0);
                currency1 = Math.Round(currencyconfig, 0);
            }
            
            string username_current = Session["userLoginSystem"].ToString();
            var obj_user = AccountController.GetByUsername(username_current);

            int uid = obj_user.ID;

            var id = Convert.ToInt32(Request.QueryString["id"]);
            if (id > 0)
            {
                var o = MainOrderController.GetAllByID(id);
                if (o != null)
                {
                    hdfOrderID.Value = o.ID.ToString();
                    if (o.OrderType == 3)
                    {
                        pnbaogia.Visible = true;
                        hdfBaoGiaVisible.Value = "1";
                    }
                    else
                    {
                        pnbaogia.Visible = false;
                        hdfBaoGiaVisible.Value = "0";
                    }
                    chkIsCheckPrice.Value = Convert.ToBoolean(o.IsCheckNotiPrice).ToString();
                    chkIsDoneSmallPackage.Value = Convert.ToBoolean(o.IsDoneSmallPackage).ToString();

                    ViewState["ID"] = id;
                    //ltrPrint.Text += "<a class=\"btn btn border-btn\" target=\"_blank\" href='/manager/PrintStamp.aspx?id=" + id + "'>In Tem</a>";
                    double currentcyynn = 0;
                    if (!string.IsNullOrEmpty(o.CurrentCNYVN))
                        currentcyynn = Math.Round(Convert.ToDouble(o.CurrentCNYVN), 0);
                    currency = currentcyynn;
                    currency1 = currency;
                    hdfcurrent.Value = Math.Round(currency, 0).ToString();
                    ViewState["MOID"] = id;
                    #region Lịch sử thanh toán
                    StringBuilder htmlPaid = new StringBuilder();
                    var PayorderHistory = PayOrderHistoryController.GetAllByMainOrderID(o.ID);
                    if (PayorderHistory.Count > 0)
                    {

                        foreach (var item in PayorderHistory)
                        {
                            htmlPaid.Append("<tr>");
                            htmlPaid.Append("    <td>" + item.CreatedDate + "</td>");
                            htmlPaid.Append("    <td>" + PJUtils.ShowStatusPayHistoryNew(item.Status.ToString().ToInt(0)) + "</td>");
                            if (item.Type.ToString() == "1")
                            {
                                htmlPaid.Append("    <td>Trực tiếp</td>");
                            }
                            else
                            {
                                htmlPaid.Append("    <td>Ví điện tử</td>");
                            }
                            htmlPaid.Append("    <td>" + string.Format("{0:N0}", item.Amount.Value) + " VNÐ</td>");
                            htmlPaid.Append("</tr>");
                        }
                        //rptPayment.DataSource = PayorderHistory;
                        //rptPayment.DataBind();
                    }
                    else
                    {

                        htmlPaid.Append("<tr class=\"noti\"><td class=\"red-text\" colspan=\"4\">Không có lịch sử thanh toán nào</td></tr>");
                        //ltrpa.Text = "<tr>Chưa có lịch sử thanh toán nào.</tr>";
                    }
                    #endregion
                    ltrpa.Text = htmlPaid.ToString();

                    if (obj_user != null)
                    {
                        hdfID.Value = obj_user.ID.ToString();
                        #region CheckRole
                        int RoleID = Convert.ToInt32(obj_user.RoleID);
                        ////if (RoleID == 0)
                        ////    ltr_currentUserImg.Text = "<img src=\"/App_Themes/NHST/images/icon.png\" width=\"100%\" />";
                        ////else if (RoleID == 1)
                        ////    ltr_currentUserImg.Text = "<img src=\"/App_Themes/NHST/images/user-icon.png\" width=\"100%\" />";
                        ////else
                        ////    ltr_currentUserImg.Text = "<img src=\"/App_Themes/NHST/images/icon.png\" width=\"100%\" />";

                        if (RoleID == 7)
                        {
                            ddlStatus.Items.Add(new ListItem("Chờ đặt cọc", "0"));
                            ddlStatus.Items.Add(new ListItem("Đã đặt cọc", "2"));
                            ddlStatus.Items.Add(new ListItem("Đã mua hàng", "5"));
                            ddlStatus.Items.Add(new ListItem("Đã về kho Trung Quốc", "6"));
                            ddlStatus.Items.Add(new ListItem("Đã về kho Việt Nam", "7"));
                            ////ddlStatus.Items.Add(new ListItem("Chờ thanh toán", "8"));
                            ddlStatus.Items.Add(new ListItem("Khách đã thanh toán", "9"));
                            ddlStatus.Items.Add(new ListItem("Đã hoàn thành", "10"));
                            ddlStatus.Enabled = false;
                            pCNShipFeeNDT.Visible = false;
                            ////pBuyNDT.Visible = false;
                            pWeightNDT.Visible = false;
                            pCheckNDT.Visible = false;
                            pPackedNDT.Visible = false;
                            pDeposit.Enabled = false;
                            pCNShipFee.Enabled = false;
                            pBuyNDT.Enabled = false;
                            pBuy.Enabled = false;
                            pCheck.Enabled = false;
                            pWeight.Enabled = false;
                            pPacked.Enabled = false;
                            pShipHome.Enabled = true;
                            ltr_OrderFee_UserInfo.Visible = false;
                            ltr_AddressReceive.Visible = false;
                            //btnUpdate.Visible = true;
                            btnThanhtoan.Visible = true;
                            ////pShipHomeNDT.Visible = true;
                            ////pnadminmanager.Visible = true;
                            ddlWarehouseFrom.Enabled = true;
                            ddlReceivePlace.Enabled = true;
                            ddlShippingType.Enabled = true;
                            pAmountDeposit.Enabled = false;
                        }
                        else if (RoleID == 3)
                        {
                            ddlStatus.Items.Add(new ListItem("Đã đặt cọc", "2"));
                            ddlStatus.Items.Add(new ListItem("Đã mua hàng", "5"));
                            pCNShipFeeNDT.Visible = true;
                            pCNShipFee.Enabled = true;
                            if (o.Status > 2)
                            {
                                ddlStatus.Enabled = false;
                                ddlStatus.Items.Add(new ListItem("Đã về kho Trung Quốc", "6"));
                                ddlStatus.Items.Add(new ListItem("Đã về kho Việt Nam", "7"));
                                ////ddlStatus.Items.Add(new ListItem("Chờ thanh toán", "8"));
                                ddlStatus.Items.Add(new ListItem("Khách đã thanh toán", "9"));
                                ddlStatus.Items.Add(new ListItem("Đã hoàn thành", "10"));
                                pCNShipFeeNDT.Enabled = true;
                                pCNShipFee.Enabled = true;
                            }
                            ////ltraddordercode.Text = "<div class=\"ordercode addordercode\"><a href=\"javascript:;\" onclick=\"addordercode()\">Thêm mã vận đơn</a></div>";
                            ////pDepositNDT.Visible = false;

                            ////pBuyNDT.Visible = false;
                            pWeightNDT.Visible = false;
                            pCheckNDT.Visible = false;
                            pPackedNDT.Visible = false;
                            pAmountDeposit.Enabled = false;
                            pDeposit.Enabled = false;

                            pBuyNDT.Enabled = false;
                            pBuy.Enabled = false;
                            pCheck.Enabled = false;
                            pWeight.Enabled = false;
                            pPacked.Enabled = false;
                            pShipHome.Enabled = false;
                            ltr_OrderFee_UserInfo.Visible = true;
                            ltr_AddressReceive.Visible = true;
                            ltrBtnUpdate.Text = "<a href=\"javascript:;\" class=\"btn mt-2\" onclick=\"UpdateOrder()\">CẬP NHẬT</a>";
                        }
                        else if (RoleID == 4)
                        {
                            ////ddlStatus.Items.Add(new ListItem("Đã đặt cọc", "2"));
                            ////ddlStatus.Items.Add(new ListItem("Đã mua hàng", "5"));
                            ////ddlStatus.Items.Add(new ListItem("Đang về Việt Nam", "6"));
                            if (o.Status < 5)
                            {
                                ddlStatus.Enabled = false;
                                ddlStatus.Items.Add(new ListItem("Đã đặt cọc", "2"));
                                ddlStatus.Items.Add(new ListItem("Đã mua hàng", "5"));
                                ddlStatus.Items.Add(new ListItem("Đã về kho Trung Quốc", "6"));
                                ddlStatus.Items.Add(new ListItem("Đã về kho Việt Nam", "7"));
                                ////ddlStatus.Items.Add(new ListItem("Chờ thanh toán", "8"));
                                ddlStatus.Items.Add(new ListItem("Khách đã thanh toán", "9"));
                                ddlStatus.Items.Add(new ListItem("Đã hoàn thành", "10"));
                                //btnUpdate.Visible = false;
                                pPackedNDT.Enabled = false;
                                pPacked.Enabled = false;
                                pWeightNDT.Enabled = false;
                                pWeight.Enabled = false;
                            }
                            else if (o.Status >= 5 && o.Status < 6)
                            {
                                ddlStatus.Enabled = true;
                                pPackedNDT.Enabled = true;
                                pPacked.Enabled = true;

                                pWeightNDT.Enabled = true;
                                pWeight.Enabled = true;


                                ////ltraddordercode.Text = "<div class=\"ordercode addordercode\"><a href=\"javascript:;\" onclick=\"addordercode()\">Thêm mã vận đơn</a></div>";

                                ddlStatus.Items.Add(new ListItem("Đã mua hàng", "5"));
                                ddlStatus.Items.Add(new ListItem("Đang về Việt Nam", "6"));
                            }
                            else if (o.Status >= 6)
                            {
                                ddlStatus.Enabled = false;
                                ddlStatus.Items.Add(new ListItem("Đã đặt cọc", "2"));
                                ddlStatus.Items.Add(new ListItem("Đã mua hàng", "5"));
                                ddlStatus.Items.Add(new ListItem("Đã về kho Trung Quốc", "6"));
                                ddlStatus.Items.Add(new ListItem("Đã về kho Việt Nam", "7"));
                                ////ddlStatus.Items.Add(new ListItem("Chờ thanh toán", "8"));
                                ddlStatus.Items.Add(new ListItem("Khách đã thanh toán", "9"));
                                ddlStatus.Items.Add(new ListItem("Đã hoàn thành", "10"));
                                //btnUpdate.Visible = false;
                                pPackedNDT.Enabled = false;
                                pPacked.Enabled = false;
                                pWeightNDT.Enabled = false;
                                pWeight.Enabled = false;
                            }
                            ////pDepositNDT.Visible = false;                            

                            pCNShipFeeNDT.Enabled = false;
                            pCNShipFee.Enabled = false;

                            pCheck.Enabled = false;
                            pCheckNDT.Enabled = false;



                            pDeposit.Enabled = false;
                            pBuyNDT.Enabled = false;

                            pBuyNDT.Enabled = false;
                            pBuy.Enabled = false;



                            pShipHome.Enabled = false;
                            ltr_OrderFee_UserInfo.Visible = false;
                            ltr_AddressReceive.Visible = false;

                            txtOrderWeight.Enabled = true;

                            ////pShipHomeNDT.Visible = false;
                        }
                        else if (RoleID == 5)
                        {
                            //ddlStatus.Items.Add(new ListItem("Đã đặt cọc", "2"));
                            //ddlStatus.Items.Add(new ListItem("Đã mua hàng", "5"));

                            if (o.Status < 5)
                            {
                                ddlStatus.Enabled = false;
                                ddlStatus.Items.Add(new ListItem("Đã đặt cọc", "2"));
                                ddlStatus.Items.Add(new ListItem("Đã mua hàng", "5"));
                                ddlStatus.Items.Add(new ListItem("Đã về kho Trung Quốc", "6"));
                                ddlStatus.Items.Add(new ListItem("Đã về kho Việt Nam", "7"));
                                ////ddlStatus.Items.Add(new ListItem("Chờ thanh toán", "8"));
                                ddlStatus.Items.Add(new ListItem("Khách đã thanh toán", "9"));
                                ddlStatus.Items.Add(new ListItem("Đã hoàn thành", "10"));
                                //btnUpdate.Visible = false;
                                pPackedNDT.Enabled = false;
                                pPacked.Enabled = false;
                                pWeightNDT.Enabled = false;
                                pWeight.Enabled = false;
                            }
                            else if (o.Status >= 5)
                            {
                                ddlStatus.Enabled = true;
                                ddlStatus.Items.Add(new ListItem("Đã mua hàng", "5"));
                                ddlStatus.Items.Add(new ListItem("Đã về kho Trung Quốc", "6"));
                                ddlStatus.Items.Add(new ListItem("Đã về kho Việt Nam", "7"));
                                ////ddlStatus.Items.Add(new ListItem("Chờ thanh toán", "8"));
                                ////ddlStatus.Items.Add(new ListItem("Khách đã thanh toán", "9"));
                                ////ddlStatus.Items.Add(new ListItem("Đã hoàn thành", "10"));
                                pPackedNDT.Enabled = false;
                                pPacked.Enabled = false;

                                pWeightNDT.Enabled = false;
                                pWeight.Enabled = false;
                            }
                            ////if (o.Status >= 7)
                            ////{
                            ////    ddlStatus.Enabled = false;
                            ////    ddlStatus.Items.Add(new ListItem("Đã đặt cọc", "2"));
                            ////    ddlStatus.Items.Add(new ListItem("Đã mua hàng", "5"));
                            ////    ddlStatus.Items.Add(new ListItem("Đang về Việt Nam", "6"));
                            ////    ddlStatus.Items.Add(new ListItem("Đã nhận hàng tại VN", "7"));
                            ////    ddlStatus.Items.Add(new ListItem("Chờ thanh toán", "8"));
                            ////    ddlStatus.Items.Add(new ListItem("Khách đã thanh toán", "9"));
                            ////    btnUpdate.Visible = false;
                            ////    pPackedNDT.Enabled = false;
                            ////    pPacked.Enabled = false;
                            ////    pWeightNDT.Enabled = false;
                            ////    pWeight.Enabled = false;
                            ////}

                            ////pDepositNDT.Visible = false;

                            pCNShipFeeNDT.Enabled = false;



                            pCheckNDT.Enabled = false;
                            pCheck.Enabled = false;

                            pDeposit.Enabled = false;

                            pCNShipFeeNDT.Enabled = false;
                            pCNShipFee.Enabled = false;

                            pBuyNDT.Enabled = false;
                            pBuy.Enabled = false;

                            pShipHome.Enabled = false;

                            ltr_OrderFee_UserInfo.Visible = false;
                            ltr_AddressReceive.Visible = false;
                            txtOrderWeight.Enabled = true;

                            ////ltraddordercode.Text = "<div class=\"ordercode addordercode\"><a href=\"javascript:;\" onclick=\"addordercode()\" >Thêm mã vận đơn</a></div>";
                            ////pShipHomeNDT.Visible = false;
                        }
                        else if (RoleID == 0)
                        {
                            pnadminmanager.Visible = true;
                            ddlStatus.Items.Add(new ListItem("Chờ đặt cọc", "0"));
                            ddlStatus.Items.Add(new ListItem("Hủy đơn hàng", "1"));
                            ddlStatus.Items.Add(new ListItem("Đã đặt cọc", "2"));
                            ddlStatus.Items.Add(new ListItem("Đã mua hàng", "5"));
                            ddlStatus.Items.Add(new ListItem("Đã về kho Trung Quốc", "6"));
                            ddlStatus.Items.Add(new ListItem("Đã về kho Việt Nam", "7"));
                            //ddlStatus.Items.Add(new ListItem("Chờ thanh toán", "8"));
                            ddlStatus.Items.Add(new ListItem("Khách đã thanh toán", "9"));
                            ddlStatus.Items.Add(new ListItem("Đã hoàn thành", "10"));

                            //ltraddordercode.Text = "<div class=\"ordercode addordercode\"><a href=\"javascript:;\" onclick=\"addordercode()\" >Thêm mã vận đơn</a></div>";
                            txtOrderWeight.Enabled = true;
                            btnThanhtoan.Visible = true;
                            pAmountDeposit.Enabled = true;
                            pDeposit.Enabled = true;
                            chkCheck.Value += "true";
                            chkPackage.Value += "true";
                            chkShiphome.Value += "true";
                            ddlWarehouseFrom.Enabled = true;
                            ddlReceivePlace.Enabled = true;
                            ddlShippingType.Enabled = true;
                            ltrBtnUpdate.Text = "<a href=\"javascript:;\" class=\"btn mt-2\" onclick=\"UpdateOrder()\">CẬP NHẬT</a>";
                        }
                        else if (RoleID == 2)
                        {
                            pnadminmanager.Visible = true;
                            ddlStatus.Items.Add(new ListItem("Chờ đặt cọc", "0"));
                            ddlStatus.Items.Add(new ListItem("Hủy đơn hàng", "1"));
                            ddlStatus.Items.Add(new ListItem("Đã đặt cọc", "2"));
                            ddlStatus.Items.Add(new ListItem("Đã mua hàng", "5"));
                            ddlStatus.Items.Add(new ListItem("Đã về kho Trung Quốc", "6"));
                            ddlStatus.Items.Add(new ListItem("Đã về kho Việt Nam", "7"));
                            //ddlStatus.Items.Add(new ListItem("Chờ thanh toán", "8"));
                            ddlStatus.Items.Add(new ListItem("Khách đã thanh toán", "9"));
                            ddlStatus.Items.Add(new ListItem("Đã hoàn thành", "10"));
                            ddlStatus.Enabled = true;
                            pDeposit.Enabled = false;
                            pCNShipFeeNDT.Enabled = true;
                            pCNShipFee.Enabled = true;
                            pBuy.Enabled = true;
                            pWeightNDT.Enabled = false;
                            pWeight.Enabled = false;
                            pCheckNDT.Enabled = false;
                            pCheck.Enabled = false;
                            pPackedNDT.Enabled = true;
                            pPacked.Enabled = true;
                            pShipHome.Enabled = true;
                            //btnUpdate.Visible = true;
                            //txtComment.Visible = true;
                            ////ddlTypeComment.Visible = true;
                            //btnSend.Visible = true;
                            ltrBtnUpdate.Text = "<a href=\"javascript:;\" class=\"btn mt-2\" onclick=\"UpdateOrder()\">CẬP NHẬT</a>";
                        }
                        else if (RoleID == 6)
                        {
                            ddlStatus.Items.Add(new ListItem("Chờ đặt cọc", "0"));
                            ////ddlStatus.Items.Add(new ListItem("Hủy đơn hàng", "1"));
                            ddlStatus.Items.Add(new ListItem("Đã đặt cọc", "2"));
                            ddlStatus.Items.Add(new ListItem("Đã mua hàng", "5"));
                            ddlStatus.Items.Add(new ListItem("Đã về kho Trung Quốc", "6"));
                            ddlStatus.Items.Add(new ListItem("Đã về kho Việt Nam", "7"));
                            ////ddlStatus.Items.Add(new ListItem("Chờ thanh toán", "8"));
                            ddlStatus.Items.Add(new ListItem("Khách đã thanh toán", "9"));
                            ddlStatus.Items.Add(new ListItem("Đã hoàn thành", "10"));
                            ddlStatus.Enabled = false;
                            pDeposit.Enabled = false;
                            pCNShipFeeNDT.Enabled = false;
                            pCNShipFee.Enabled = false;
                            pBuyNDT.Enabled = false;
                            pBuy.Enabled = false;
                            pWeightNDT.Enabled = false;
                            pWeight.Enabled = false;
                            pCheckNDT.Enabled = false;
                            pCheck.Enabled = false;
                            pPackedNDT.Enabled = false;
                            pPacked.Enabled = false;
                            pShipHome.Enabled = false;

                            //txtComment.Visible = true;
                            ////ddlTypeComment.Visible = true;
                            //btnSend.Visible = true;
                            //btnUpdate.Visible = false;
                           
                        }
                        else if (RoleID == 8)
                        {
                            //ddlStatus.Items.Add(new ListItem("Chờ đặt cọc", "0"));
                            //ddlStatus.Items.Add(new ListItem("Hủy đơn hàng", "1"));
                            //ddlStatus.Items.Add(new ListItem("Đã đặt cọc", "2"));
                            //ddlStatus.Items.Add(new ListItem("Đã mua hàng", "5"));
                            //ddlStatus.Items.Add(new ListItem("Đang về Việt Nam", "6"));
                            //ddlStatus.Items.Add(new ListItem("Đã nhận hàng tại VN", "7"));
                            //ddlStatus.Items.Add(new ListItem("Chờ thanh toán", "8"));
                            ddlStatus.Items.Add(new ListItem("Khách đã thanh toán", "9"));
                            ddlStatus.Items.Add(new ListItem("Đã hoàn thành", "10"));
                            ddlStatus.Enabled = true;
                            pDeposit.Enabled = false;
                            pCNShipFeeNDT.Enabled = false;
                            pCNShipFee.Enabled = false;
                            pBuyNDT.Enabled = false;
                            pBuy.Enabled = false;
                            pWeightNDT.Enabled = false;
                            pWeight.Enabled = false;
                            pCheckNDT.Enabled = false;
                            pCheck.Enabled = false;
                            pPackedNDT.Enabled = false;
                            pPacked.Enabled = false;
                            pShipHome.Enabled = false;
                            //btnUpdate.Visible = true;

                            //txtComment.Visible = true;
                            ////ddlTypeComment.Visible = true;
                            //btnSend.Visible = true;
                            txtOrderWeight.Enabled = false;
                        }
                        int countOc = 1;
                        if (!string.IsNullOrEmpty(o.OrderTransactionCode2) || !string.IsNullOrEmpty(o.OrderTransactionCodeWeight2))
                        {
                            hdfoc2.Value = "1";
                            countOc++;
                        }
                        else
                        {
                            hdfoc2.Value = "0";
                        }
                        if (!string.IsNullOrEmpty(o.OrderTransactionCode3) || !string.IsNullOrEmpty(o.OrderTransactionCodeWeight3))
                        {
                            hdfoc3.Value = "1";
                            countOc++;
                        }
                        else
                        {
                            hdfoc3.Value = "0";
                        }
                        if (!string.IsNullOrEmpty(o.OrderTransactionCode4) || !string.IsNullOrEmpty(o.OrderTransactionCodeWeight4))
                        {
                            hdfoc4.Value = "1";
                            countOc++;
                        }
                        else
                        {
                            hdfoc4.Value = "0";
                        }
                        if (!string.IsNullOrEmpty(o.OrderTransactionCode5) || !string.IsNullOrEmpty(o.OrderTransactionCodeWeight5))
                        {
                            hdfoc5.Value = "1";
                            countOc++;
                        }
                        else
                        {
                            hdfoc5.Value = "0";
                        }
                        hdforderamount.Value = countOc.ToString();
                        #endregion
                        #region Lấy thông tin nhân viên
                        ddlSaler.SelectedValue = o.SalerID.ToString();
                        ddlDatHang.SelectedValue = o.DathangID.ToString();
                        ddlKhoTQ.SelectedValue = o.KhoTQID.ToString();
                        ddlKhoVN.SelectedValue = o.KhoVNID.ToString();
                        #endregion
                        #region Lấy thông tin người đặt
                        var usercreate = AccountController.GetByID(Convert.ToInt32(o.UID));
                        double ckFeeBuyPro = 0;
                        double ckFeeWeight = 0;
                        double ckVolume = 0;
                        if (usercreate != null)
                        {
                            ckFeeBuyPro = Convert.ToDouble(UserLevelController.GetByID(usercreate.LevelID.ToString().ToInt()).FeeBuyPro.ToString());
                            ckFeeWeight = Convert.ToDouble(UserLevelController.GetByID(usercreate.LevelID.ToString().ToInt()).FeeWeight.ToString());
                            ckVolume = Convert.ToDouble(UserLevelController.GetByID(usercreate.LevelID.ToString().ToInt()).FeeVolume.ToString());

                            lblCKFeebuypro.Text = ckFeeBuyPro.ToString();
                            lblCKFeeWeight.Text = ckFeeWeight.ToString();
                            lblCKFeeVolume.Text = ckVolume.ToString();

                            hdfFeeBuyProDiscount.Value = ckFeeBuyPro.ToString();
                            hdfFeeWeightDiscount.Value = ckFeeWeight.ToString();
                            hdfFeeVolumeDiscount.Value = ckVolume.ToString();
                        }
                        else
                        {
                            lblCKFeebuypro.Text = "0";
                            lblCKFeeWeight.Text = "0";
                            lblCKFeeVolume.Text = "0";
                            hdfFeeBuyProDiscount.Value = "0";
                            hdfFeeWeightDiscount.Value = "0";
                            hdfFeeVolumeDiscount.Value = "0";
                        }

                        if (RoleID != 8)
                        {
                            StringBuilder customerInfo = new StringBuilder();
                            if (RoleID == 3)
                            {
                                customerInfo.Append("<span>Tài khoản không đủ quyền xem thông tin này</span>");
                                //ltr_OrderFee_UserInfo.Text += "Tài khoản không đủ quyền xem thông tin này";
                            }
                            else
                            {
                                var ui = AccountInfoController.GetByUserID(Convert.ToInt32(o.UID));
                                if (ui != null)
                                {
                                    string phone = ui.MobilePhonePrefix + ui.MobilePhone;                                    

                                    customerInfo.Append("<table class=\"table\">");
                                    customerInfo.Append("    <tbody>");
                                    customerInfo.Append("        <tr>");
                                    customerInfo.Append("            <td>Username</td>");
                                    customerInfo.Append("            <td>" + AccountController.GetByID(Convert.ToInt32(o.UID)).Username + "</td>");
                                    customerInfo.Append("        </tr>");
                                    customerInfo.Append("        <tr>");
                                    customerInfo.Append("            <td>Mã khách hàng</td>");
                                    customerInfo.Append("            <td>" + AccountController.GetByID(Convert.ToInt32(o.UID)).MaKH + "</td>");
                                    customerInfo.Append("        </tr>");
                                    customerInfo.Append("        <tr>");
                                    customerInfo.Append("            <td>Địa chỉ</td>");
                                    customerInfo.Append("            <td>" + ui.Address + "</td>");
                                    customerInfo.Append("        </tr>");
                                    customerInfo.Append("        <tr>");
                                    customerInfo.Append("            <td>Email</td>");
                                    customerInfo.Append("            <td><a href=\"" + ui.Email + "\">" + ui.Email + "</a></td>");
                                    customerInfo.Append("        </tr>");
                                    customerInfo.Append("        <tr>");
                                    customerInfo.Append("            <td>Số ĐT</td>");
                                    customerInfo.Append("            <td><a href=\"tel:+" + phone + "\">" + phone + "</a></td>");
                                    customerInfo.Append("        </tr>");
                                    customerInfo.Append("        <tr>");
                                    customerInfo.Append("            <td>Ghi chú</td>");
                                    customerInfo.Append("            <td>" + o.Note + "</td>");
                                    customerInfo.Append("        </tr>");
                                    customerInfo.Append("    </tbody>");
                                    customerInfo.Append("</table>");
                                }
                            }
                            ltr_OrderFee_UserInfo.Text = customerInfo.ToString();
                        }

                        ltr_OrderCode.Text += "<div class=\"order-panel\">";
                        ltr_OrderCode.Text += " <div class=\"title\">Mã đơn hàng</div>";
                        ltr_OrderCode.Text += "     <div class=\"cont\">";
                        ltr_OrderCode.Text += "         <p><strong>" + o.ID + "</strong></p>";
                        ltr_OrderCode.Text += "     </div>";
                        ltr_OrderCode.Text += "</div>";
                        ltr_OrderID.Text += "<strong>" + o.ID + "</strong>";

                        var kd = AccountController.GetByID(Convert.ToInt32(o.SalerID));
                        var dathang = AccountController.GetByID(Convert.ToInt32(o.DathangID));
                        var khotq = AccountController.GetByID(Convert.ToInt32(o.KhoTQID));
                        var khovn = AccountController.GetByID(Convert.ToInt32(o.KhoVNID));
                        if (kd != null)
                        {
                            ltr_OrderFee_UserInfo2.Text += "    <dt style=\"width: 200px;\">Nhân viên kinh doanh:</dt>";
                            ltr_OrderFee_UserInfo2.Text += "    <dd><strong>" + kd.Username + "</strong></dd>";
                        }
                        if (dathang != null)
                        {
                            ltr_OrderFee_UserInfo2.Text += "    <dt style=\"width: 200px;\">Nhân viên đặt hàng:</dt>";
                            ltr_OrderFee_UserInfo2.Text += "    <dd><strong>" + dathang.Username + "</strong></dd>";
                        }
                        if (khotq != null)
                        {
                            ltr_OrderFee_UserInfo2.Text += "    <dt style=\"width: 200px;\">Nhân viên kho Trung Quốc:</dt>";
                            ltr_OrderFee_UserInfo2.Text += "    <dd><strong>" + khotq.Username + "</strong></dd>";
                        }
                        if (khovn != null)
                        {
                            ltr_OrderFee_UserInfo2.Text += "    <dt style=\"width: 200px;\">Nhân viên kho Việt Nam:</dt>";
                            ltr_OrderFee_UserInfo2.Text += "    <dd><strong>" + khovn.Username + "</strong></dd>";
                        }
                        #endregion
                        #region Lấy thông tin đơn hàng
                        txtMainOrderCode.Text = o.MainOrderCode;

                        //NEW HDK
                        var listMainOrderCode = MainOrderCodeController.GetAllByMainOrderID(o.ID);
                        ListItem ddlitem = new ListItem("Chọn mã đơn hàng", "0");
                        ddlMainOrderCode.Items.Add(ddlitem);
                        if (listMainOrderCode != null)
                        {

                            if (listMainOrderCode.Count > 0)
                            {
                                StringBuilder html = new StringBuilder();
                                foreach (var item in listMainOrderCode)
                                {
                                    ListItem listitem = new ListItem(item.MainOrderCode, item.ID.ToString());
                                    ddlMainOrderCode.Items.Add(listitem);                                  

                                    html.Append("<div class=\"row order-wrap\">");
                                    html.Append("    <div class=\"input-field col s10 m11 MainOrderInPut\">");
                                    html.Append("        <input type=\"text\" class=\"MainOrderCode\"  data-orderCodeID=\"" + item.ID + "\"  onkeypress=\"myFunction($(this))\" value=\"" + item.MainOrderCode + "\">");
                                    html.Append("       <span class=\"helper-text hide\" style=\"position:absolute;\">");
                                    html.Append("       <label style=\"color:green\">Đã cập nhật</label>");
                                    html.Append("       </span>");
                                    html.Append("    </div>");
                                    html.Append("    <a href=\"javascript:;\" onclick=\"deleteMVD($(this))\" style=\"line-height:80px;position:absolute\" class=\"remove-order tooltipped\" data-position=\"top\" data-tooltip=\"Xóa\"><i class=\"material-icons\">delete</i></a>");
                                    html.Append("</div>");
                                }
                                lrtMainOrderCode.Text = html.ToString();
                            }
                        }

                        chkCheck.Value = o.IsCheckProduct.ToString().ToBool().ToString();
                        chkPackage.Value = o.IsPacked.ToString().ToBool().ToString();
                        chkShiphome.Value = o.IsFastDelivery.ToString().ToBool().ToString();
                        hdfIsInsurrance.Value = Convert.ToBoolean(o.IsInsurrance).ToString();
                        //chkIsFast.Checked = o.IsFast.ToString().ToBool();
                        double feeeinwarehouse = 0;
                        if (o.FeeInWareHouse != null)
                            feeeinwarehouse = Convert.ToDouble(o.FeeInWareHouse);
                        rFeeWarehouse.Text = string.Format("{0:N0}", Convert.ToDouble(Math.Round(feeeinwarehouse)));

                        if (o.IsGiaohang != null)
                        {
                            chkIsGiaohang.Value = o.IsGiaohang.ToString();
                        }
                        else
                        {
                            chkIsGiaohang.Value = "false";
                        }

                        if (!string.IsNullOrEmpty(o.AmountDeposit))
                        {
                            double amountdeposit = Math.Round(Convert.ToDouble(o.AmountDeposit.ToString()), 0);
                            pAmountDeposit.Text = string.Format("{0:N0}", amountdeposit);                            
                        }
                        else
                        {
                            pAmountDeposit.Text = "0";
                            //lblAmountDeposit.Text = "0 ";
                        }
                        if (!string.IsNullOrEmpty(o.TotalPriceReal))
                            rTotalPriceReal.Text = string.Format("{0:N0}", Math.Round(Convert.ToDouble(o.TotalPriceReal)));                      
                        else
                            rTotalPriceReal.Text = "0";

                        if (!string.IsNullOrEmpty(o.TotalPriceRealCYN))
                            rTotalPriceRealCYN.Text = Math.Round(Convert.ToDouble(o.TotalPriceRealCYN), 2).ToString();
                        else
                            rTotalPriceRealCYN.Text = "0";

                        ddlStatus.SelectedValue = o.Status.ToString();
                        if (!string.IsNullOrEmpty(o.Deposit))
                            pDeposit.Text = string.Format("{0:N0}", Math.Round(Convert.ToDouble(o.Deposit)));

                        double fscn = 0;
                        if (!string.IsNullOrEmpty(o.FeeShipCN))
                        {
                            fscn = Math.Floor(Convert.ToDouble(o.FeeShipCN));
                            pCNShipFeeNDT.Text = (fscn / currency1).ToString();
                            pCNShipFee.Text = string.Format("{0:N0}", Convert.ToDouble(fscn));
                            lblShipTQ.Text = string.Format("{0:N0}", Convert.ToDouble(o.FeeShipCN));
                        }
                        double realprice = 0;
                        if (!string.IsNullOrEmpty(o.TotalPriceReal))
                            realprice = Convert.ToDouble(o.TotalPriceReal);

                        double tot = Convert.ToDouble(o.PriceVND) + fscn - realprice;
                        double totCYN = tot / currency1;
                        pHHCYN.Text = totCYN.ToString();
                        pHHVND.Text = string.Format("{0:N0}", Convert.ToDouble(tot));

                        if (!string.IsNullOrEmpty(o.FeeBuyPro))
                        {
                            pBuy.Text = string.Format("{0:N0}", Convert.ToDouble(o.FeeBuyPro));
                            lblFeeBuyProduct.Text = string.Format("{0:N0}", Convert.ToDouble(o.FeeBuyPro));
                        }
                        if (!string.IsNullOrEmpty(o.FeeWeight))
                        {
                            pWeight.Text = string.Format("{0:N0}", Convert.ToDouble(o.FeeWeight));
                        }
                        else
                        {
                            pWeight.Text = "0";
                        }
                        if (!string.IsNullOrEmpty(o.TQVNWeight))
                        {
                            pWeightNDT.Text = Convert.ToDouble(o.TQVNWeight).ToString();
                        }
                        else
                        {
                            pWeightNDT.Text = "0";
                        }
                        if (!string.IsNullOrEmpty(o.FeeVolume))
                        {
                            pVolume.Text = string.Format("{0:N0}", Convert.ToDouble(o.FeeVolume));
                        }
                        else
                        {
                            pVolume.Text = "0";
                        }
                        if (!string.IsNullOrEmpty(o.OrderVolume))
                        {
                            pVolumeNDT.Text = Convert.ToDouble(o.OrderVolume).ToString();
                        }
                        else
                        {
                            pVolumeNDT.Text = "0";
                        }

                        double checkproductprice = Convert.ToDouble(o.IsCheckProductPrice);
                        pCheck.Text = string.Format("{0:N0}", Convert.ToDouble(checkproductprice));
                        pCheckNDT.Text = (checkproductprice / currency).ToString();

                        double packagedprice = Convert.ToDouble(o.IsPackedPrice);
                        pPacked.Text = string.Format("{0:N0}", Convert.ToDouble(packagedprice));
                        pPackedNDT.Text = (packagedprice / currency).ToString();

                        double InsuranceMoney = Convert.ToDouble(o.InsuranceMoney);
                        txtInsuranceMoney.Text = string.Format("{0:N0}", Convert.ToDouble(InsuranceMoney));

                        pShipHome.Text = string.Format("{0:N0}", Convert.ToDouble(o.IsFastDeliveryPrice));

                        double TotalFeeSupport = Convert.ToDouble(o.TotalFeeSupport);

                        lblMoneyNotFee.Text = string.Format("{0:N0}", Convert.ToDouble(o.PriceVND));
                        lblTotalMoneyVND1.Text = string.Format("{0:N0}", Convert.ToDouble(o.PriceVND));
                        lblTotalMoneyCNY1.Text = string.Format("{0:#.##}", Convert.ToDouble(o.PriceVND) / currency);
                        double totalFee = Convert.ToDouble(o.IsCheckProductPrice) + Convert.ToDouble(o.IsPackedPrice) +
                           Convert.ToDouble(o.IsFastDeliveryPrice) + Convert.ToDouble(o.IsFastPrice) + InsuranceMoney;
                        lblAllFee.Text = string.Format("{0:N0}", totalFee);
                        lblFeeTQVN.Text = string.Format("{0:N0}", Convert.ToDouble(o.FeeWeight));
                        double odweight = 0;
                        if (!string.IsNullOrEmpty(o.OrderWeight))
                            odweight = Convert.ToDouble(o.OrderWeight);
                        txtOrderWeight.Text = odweight.ToString();
                        string orderweightfeedc = o.FeeWeightCK;

                        double odvolume = 0;
                        if (!string.IsNullOrEmpty(o.OrderVolume))
                            odvolume = Convert.ToDouble(o.OrderVolume);
                        txtOrderVolume.Text = odvolume.ToString();
                        string ordervolumefeedc = o.FeeVolumeCK;

                        ddlWarehouseFrom.SelectedValue = o.FromPlace.ToString();
                        hdfFromPlace.Value = o.FromPlace.ToString();

                        ddlReceivePlace.SelectedValue = o.ReceivePlace;
                        hdfReceivePlace.Value = o.ReceivePlace;

                        ddlShippingType.SelectedValue = o.ShippingType.ToString();
                        hdfShippingType.Value = o.ShippingType.ToString();

                        if (string.IsNullOrEmpty(orderweightfeedc))
                        {
                            lblCKFeeweightPrice.Text = "0";
                            hdfFeeweightPriceDiscount.Value = "0";
                        }
                        else
                        {
                            lblCKFeeweightPrice.Text = orderweightfeedc;
                            hdfFeeweightPriceDiscount.Value = orderweightfeedc;
                        }

                        if (string.IsNullOrEmpty(ordervolumefeedc))
                        {
                            lblCKFeeVolumePrice.Text = "0";
                            hdfFeeVolumePriceDiscount.Value = "0";
                        }
                        else
                        {
                            lblCKFeeVolumePrice.Text = ordervolumefeedc;
                            hdfFeeVolumePriceDiscount.Value = ordervolumefeedc;
                        }

                        double FeeFinal = 0;
                        if (Convert.ToDouble(o.FeeWeight) > Convert.ToDouble(o.FeeVolume))
                        {
                            FeeFinal = Convert.ToDouble(o.FeeWeight);
                        }
                        else
                        {
                            FeeFinal = Convert.ToDouble(o.FeeVolume);
                        }
                        double alltotal = totalFee + Convert.ToDouble(o.PriceVND) + Convert.ToDouble(o.FeeShipCN) + Convert.ToDouble(o.FeeBuyPro) + Convert.ToDouble(o.FeeShipCNToVN)
                            + Convert.ToDouble(FeeFinal) + feeeinwarehouse + TotalFeeSupport;

                        lblAllTotal.Text = string.Format("{0:N0}", alltotal);
                        lblDeposit.Text = string.Format("{0:N0}", Convert.ToDouble(o.Deposit));
                        lblLeftPay.Text = string.Format("{0:N0}", alltotal - Convert.ToDouble(o.Deposit));

                        ltrlblAllTotal1.Text = string.Format("{0:N0}", alltotal);
                        lblDeposit1.Text = string.Format("{0:N0}", Convert.ToDouble(o.Deposit));
                        lblLeftPay1.Text = string.Format("{0:N0}", alltotal - Convert.ToDouble(o.Deposit));

                        string statreturn = PJUtils.IntToRequestAdminReturnBG(Convert.ToInt32(o.Status));
                        
                        #endregion
                        #region Lấy thông tin nhận hàng
                        StringBuilder customerInfo2 = new StringBuilder();
                        if (RoleID == 3)
                        {
                            //ltr_AddressReceive.Text = "Tài khoản không đủ quyền xem thông tin này";
                            customerInfo2.Append("<span>Tài khoản không đủ quyền xem thông tin này</span>");
                        }
                        else
                        {
                            //ltr_AddressReceive.Text += "<dt>Tên:</dt>";
                            //ltr_AddressReceive.Text += "<dd>" + o.FullName + "</dd>";
                            //ltr_AddressReceive.Text += "<dt>Địa chỉ:</dt>";
                            //ltr_AddressReceive.Text += "<dd>" + o.Address + "</dd>";
                            //ltr_AddressReceive.Text += "<dt>Email:</dt>";
                            //ltr_AddressReceive.Text += "<dd><a href=\"" + o.Email + "\">" + o.Email + "</a></dd>";
                            //ltr_AddressReceive.Text += "<dt>Số dt:</dt>";
                            //ltr_AddressReceive.Text += "<dd><a href=\"tel:+" + o.Phone + "\">" + o.Phone + "</a></dd>";
                            ////ltr_AddressReceive.Text += "<dt>Ghi chú:</dt>";
                            ////ltr_AddressReceive.Text += "<dd>" + o.Note + "</dd>";

                            customerInfo2.Append("<table class=\"table\">");
                            customerInfo2.Append("    <tbody>");
                            customerInfo2.Append("        <tr>");
                            customerInfo2.Append("            <td>Tên</td>");
                            customerInfo2.Append("            <td>" + o.FullName + "</td>");
                            customerInfo2.Append("        </tr>");
                            customerInfo2.Append("        <tr>");
                            customerInfo2.Append("            <td>Địa chỉ</td>");
                            customerInfo2.Append("            <td>" + o.Address + "</td>");
                            customerInfo2.Append("        </tr>");
                            customerInfo2.Append("        <tr>");
                            customerInfo2.Append("            <td>Email</td>");
                            customerInfo2.Append("            <td><a href=\"" + o.Email + "\">" + o.Email + "</a></td>");
                            customerInfo2.Append("        </tr>");
                            customerInfo2.Append("        <tr>");
                            customerInfo2.Append("            <td>Sô´ ÐT</td>");
                            customerInfo2.Append("            <td><a href=\"tel:+" + o.Phone + "\">" + o.Phone + "</a></td>");
                            customerInfo2.Append("        </tr>");
                            customerInfo2.Append("        <tr>");
                            customerInfo2.Append("            <td>Ghi chú</td>");
                            customerInfo2.Append("            <td>" + o.Note + "</td>");
                            customerInfo2.Append("        </tr>");
                            customerInfo2.Append("    </tbody>");
                            customerInfo2.Append("</table>");
                        }
                        ltr_AddressReceive.Text = customerInfo2.ToString();
                        #endregion
                        #region Lấy sản phẩm
                        int totalproduct = 0;
                        List<tbl_Order> lo = new List<tbl_Order>();
                        lo = OrderController.GetByMainOrderID(o.ID);
                        if (lo.Count > 0)
                        {
                            //rpt.DataSource = lo;
                            //rpt.DataBind();
                            int stt = 1;
                            StringBuilder html = new StringBuilder();
                            foreach (var item in lo)
                            {
                                double currentcyt = Convert.ToDouble(item.CurrentCNYVN);
                                double price = 0;
                                double pricepromotion = Convert.ToDouble(item.price_promotion);
                                double priceorigin = Convert.ToDouble(item.price_origin);
                                if (pricepromotion > 0)
                                {
                                    if (priceorigin > pricepromotion)
                                    {
                                        price = pricepromotion;
                                    }
                                    else
                                    {
                                        price = priceorigin;
                                    }
                                }
                                else
                                {
                                    price = priceorigin;
                                }
                                double vndprice = price * currentcyt;
                                //ltrProducts.Text += "<tr>";
                                ////ltrProducts.Text += "<td rowspan=\"2\">" + item.ID + "</td>";
                                //ltrProducts.Text += "<td rowspan=\"2\">" + stt + "</td>";
                                //ltrProducts.Text += "<td>";
                                //ltrProducts.Text += "<div class=\"product-infobox\">";
                                //ltrProducts.Text += "<a href=\"" + item.link_origin + "\" target=\"_blank\" class=\"img\">";
                                //ltrProducts.Text += "<img src=\"" + item.image_origin + "\" alt=\"\"></a>";
                                //ltrProducts.Text += "<div class=\"info\">";
                                //ltrProducts.Text += "<a href=\"" + item.link_origin + "\" target=\"_blank\">" + item.title_origin + "</a>";
                                //ltrProducts.Text += "</div>";
                                //ltrProducts.Text += "</div>";
                                //ltrProducts.Text += "</td>";
                                //ltrProducts.Text += "<td>" + item.quantity + "</td>";
                                //ltrProducts.Text += "<td>";
                                //ltrProducts.Text += "<p>" + string.Format("{0:N0}", vndprice) + " vnđ</p>";
                                //ltrProducts.Text += "<p>¥ " + string.Format("{0:0.##}", price) + "</p>";
                                //ltrProducts.Text += "</td>";
                                //if (string.IsNullOrEmpty(item.ProductStatus.ToString()))
                                //{
                                //    ltrProducts.Text += "<td>Còn hàng</td>";
                                //}
                                //else
                                //{
                                //    if (item.ProductStatus == 2)
                                //        ltrProducts.Text += "<td>Hết hàng</td>";
                                //    else
                                //        ltrProducts.Text += "<td>Còn hàng</td>";
                                //}
                                //ltrProducts.Text += "<td rowspan=\"2\" class=\"hl-txt\">";
                                //ltrProducts.Text += "<a href=\"/manager/ProductEdit.aspx?id=" + item.ID + "\" class=\"left edit-btn\">Sửa</a>";
                                //ltrProducts.Text += "<a href=\"javascript:;\" class=\"toggle-detail-row right\"><i class=\"fa fa-caret-down\"></i></a>";
                                //ltrProducts.Text += "</td>";
                                //ltrProducts.Text += "</tr>";
                                //ltrProducts.Text += "<tr class=\"detail-row\" style=\"display:block\">";
                                //ltrProducts.Text += "<td colspan=\"4\">";
                                //ltrProducts.Text += "<dl class=\"dl\">";
                                //ltrProducts.Text += "<dt>Thuộc tính</dt>";
                                //ltrProducts.Text += "<dd>" + item.property + "</dd>";
                                //ltrProducts.Text += "<dt>Ghi chú:</dt>";
                                //ltrProducts.Text += "<dd>" + item.brand + "</dd>";
                                //ltrProducts.Text += "</dl>";
                                //ltrProducts.Text += "</td>";
                                //ltrProducts.Text += "</tr>";


                                html.Append("<div class=\"item-wrap\">");
                                html.Append("    <div class=\"item-name\">");
                                html.Append("        <div class=\"number\">");
                                html.Append("            <span class=\"count\">" + stt + "</span>");
                                html.Append("        </div>");
                                html.Append("        <div class=\"name\">");
                                html.Append("            <span class=\"item-img\">");
                                html.Append("                <a href=\"" + item.link_origin + "\" target=\"_blank\"><img src=\"" + item.image_origin + "\" alt=\"image\"></a>");
                                html.Append("            </span>");
                                html.Append("            <div class=\"caption\">");
                                html.Append("                <a href=\"" + item.link_origin + "\" target=\"_blank\" class=\"title black-text\">" + item.title_origin + "</a>");
                                html.Append("                <div class=\"item-price mt-1\">");
                                html.Append("                    <span class=\"pr-2 black-text font-weight-600\">Thuộc tính: </span><span class=\"pl-2 black-text font-weight-600\">" + item.property + "</span>");
                                html.Append("                </div>");
                                html.Append("                <div class=\"note\">");
                                html.Append("                    <span class=\"black-text font-weight-500\">Ghi chú: </span>");
                                html.Append("                    <div class=\"input-field inline\">");
                                html.Append("                        <input type=\"text\" value=\"" + item.brand + "\" class=\"validate\">");
                                html.Append("                    </div>");
                                html.Append("                </div>");
                                html.Append("            </div>");
                                html.Append("        </div>");
                                html.Append("    </div>");
                                html.Append("    <div class=\"item-info\">");
                                html.Append("        <div class=\"item-num column\">");
                                html.Append("            <span class=\"black-text\"><strong>Số lượng</strong></span>");
                                html.Append("            <p>" + item.quantity + "</p>");
                                html.Append("            <p></p>");
                                html.Append("        </div>");
                                html.Append("        <div class=\"item-price column\">");
                                html.Append("            <span class=\"black-text\"><strong>Đơn giá</strong></span>");
                                html.Append("            <p class=\"grey-text font-weight-500\">¥" + string.Format("{0:0.##}", price) + "</p>");
                                html.Append("            <p class=\"grey-text font-weight-500\">" + string.Format("{0:N0}", vndprice) + " VNÐ</p>");
                                html.Append("        </div>");
                                html.Append("        <div class=\"item-status column\">");
                                html.Append("            <span class=\"black-text\"><strong>Trạng thái</strong></span>");
                                if (string.IsNullOrEmpty(item.ProductStatus.ToString()))
                                {
                                    html.Append("            <p class=\"green-text\">Còn hàng</p>");
                                }
                                else
                                {
                                    if (item.ProductStatus == 2)
                                        html.Append("            <p class=\"red-text\">Hết hàng</p>");
                                    else
                                        html.Append("            <p class=\"green-text\">Còn hàng</p>");
                                }
                                html.Append("        </div>");
                                //html.Append("        <div class=\"item-status column\">");
                                //html.Append("            <span class=\"black-text\"><strong>Mua hàng</strong></span>");
                                //if (Convert.ToBoolean(item.IsBuy))
                                //{
                                //    html.Append("            <p class=\"green-text\">Còn hàng</p>");
                                //}
                                //else
                                //{
                                //    html.Append("            <p class=\"green-text\">Còn hàng</p>");
                                //}
                                //html.Append("        </div>");
                                html.Append("        <div class=\"delete\">");
                                html.Append("            <a href=\"/manager/ProductEdit.aspx?id=" + item.ID + "\" class=\"btn-update tooltipped\" data-position=\"top\" data-tooltip=\"Sửa\"><i class=\"material-icons\">edit</i></a>");
                                html.Append("        </div>");
                                html.Append("    </div>");
                                html.Append("</div>");

                                totalproduct += Convert.ToInt32(item.quantity);

                                //Print
                                //ltrProductPrint.Text += "<tr>";
                                //ltrProductPrint.Text += "<td class=\"pro\">" + item.ID + "</td>";
                                //ltrProductPrint.Text += "<td class=\"pro\">";
                                //ltrProductPrint.Text += "   <div class=\"thumb-product\">";
                                //ltrProductPrint.Text += "       <div class=\"pd-img\"><img src=\"" + item.image_origin + "\" alt=\"\"></div>";
                                //ltrProductPrint.Text += "       <div class=\"info\">" + item.title_origin + "</div>";
                                //ltrProductPrint.Text += "   </div>";
                                //ltrProductPrint.Text += "</td>";
                                //ltrProductPrint.Text += "<td class=\"pro\">" + item.property + "</td>";
                                //ltrProductPrint.Text += "<td class=\"qty\">" + item.quantity + "</td>";

                                //ltrProductPrint.Text += "<td class=\"price\"><p class=\"\">" + string.Format("{0:N0}", vndprice) + " vnđ</p></td>";
                                //ltrProductPrint.Text += "<td class=\"price\"><p class=\"\">¥" + string.Format("{0:0.##}", price) + "</p></td>";

                                //ltrProductPrint.Text += "<td class=\"price\"><p class=\"\">" + item.brand + "</p></td>";
                                //if (string.IsNullOrEmpty(item.ProductStatus.ToString()))
                                //{
                                //    ltrProductPrint.Text += "<td class=\"price\"><p class=\"\">Còn hàng</p></td>";
                                //}
                                //else
                                //{
                                //    if (item.ProductStatus == 2)
                                //        ltrProductPrint.Text += "<td class=\"price\"><p class=\"bg-red\">Hết hàng</p></td>";
                                //    else
                                //        ltrProductPrint.Text += "<td class=\"price\"><p class=\"\">Còn hàng</p></td>";
                                //}
                                //ltrProducts.Text += "</tr>";
                                stt++;
                            }
                            ltrProducts.Text = html.ToString();
                        }
                        ltrTotalProduct.Text = totalproduct.ToString();
                        #endregion
                        #region Lấy bình luận nội bộ
                        StringBuilder chathtml = new StringBuilder();
                        var cs = OrderCommentController.GetByOrderIDAndType(o.ID, 2);
                        if (cs != null)
                        {
                            if (cs.Count > 0)
                            {
                                foreach (var item in cs)
                                {
                                    string fullname = "";
                                    int role = 0;
                                    int user_postID = 0;
                                    var user = AccountController.GetByID(Convert.ToInt32(item.CreatedBy));
                                    if (user != null)
                                    {
                                        user_postID = user.ID;
                                        role = Convert.ToInt32(user.RoleID);
                                        var userinfo = AccountController.GetByID(user.ID);
                                        if (userinfo != null)
                                        {
                                            fullname = userinfo.Username;

                                        }
                                    }

                                    if (uid == user_postID)
                                    {
                                        //ltrInComment.Text += "<div class=\"mess-item mymess\">";
                                        chathtml.Append("<div class=\"chat chat-right\">");
                                    }
                                    else
                                    {
                                        //ltrInComment.Text += "<div class=\"mess-item \">";
                                        chathtml.Append("<div class=\"chat\">");
                                    }
                                    chathtml.Append("<div class=\"chat-avatar\">");
                                    chathtml.Append("    <p class=\"name\">" + fullname + "</p>");
                                    //chathtml.Append("    <p class=\"role\">"+RoleController.GetByID(user.RoleID.Value).RoleName+"</p>");
                                    chathtml.Append("</div>");
                                    chathtml.Append("<div class=\"chat-body\">");
                                    chathtml.Append("        <div class=\"chat-text\">");
                                    chathtml.Append("                <div class=\"date-time center-align\">" + string.Format("{0:dd/MM/yyyy HH:mm}", item.CreatedDate) + "</div>");
                                    chathtml.Append("                <div class=\"text-content\">");
                                    chathtml.Append("                    <div class=\"content\">");
                                    if (!string.IsNullOrEmpty(item.Link))
                                    {
                                        chathtml.Append("<div class=\"content-img\">");
                                        //if (uid == user_postID)
                                        //{
                                        //    chathtml.Append("<div class=\"content-img\" style=\"border-radius: 5px;background-color: #2196f3;\">");
                                        //    //ltrInComment.Text += "<div class=\"mess-item mymess\">";

                                        //}
                                        //else
                                        //{
                                        //    //ltrInComment.Text += "<div class=\"mess-item \">";
                                        //    chathtml.Append("<div class=\"content-img\">");
                                        //}
                                        chathtml.Append("   <div class=\"img-block\">");
                                        if (item.Link.Contains(".doc"))
                                        {
                                            chathtml.Append("<a href=\"" + item.Link + "\" target=\"_blank\"><img src=\"/App_Themes/AdminNew45/assets/images/icon/file.png\" title=\"" + item.Comment + "\"  class=\"\" height=\"50\"/></a>");

                                        }
                                        else if (item.Link.Contains(".xls"))
                                        {
                                            chathtml.Append("<a href=\"" + item.Link + "\" target=\"_blank\"><img src=\"/App_Themes/AdminNew45/assets/images/icon/file.png\" title =\"" + item.Comment + "\"  class=\"\" height=\"50\"/></a>");
                                        }
                                        else
                                        {
                                            chathtml.Append("<a href=\"" + item.Link + "\" target=\"_blank\"><img src=\"" + item.Link + "\" title=\"" + item.Comment + "\"  class=\"\" height=\"50\"/></a>");
                                        }

                                        //chathtml.Append("       <img src=\"" + item.Link + "\" title=\"" + item.Comment + "\"  class=\"materialboxed\" height=\"50\"/>");
                                        chathtml.Append("   </div>");
                                        chathtml.Append("</div>");
                                    }
                                    else
                                    {
                                        chathtml.Append("                    <p>" + item.Comment + "</p>");
                                    }
                                    chathtml.Append("                    </div>");
                                    chathtml.Append("                </div>");
                                    chathtml.Append("        </div>");
                                    chathtml.Append("</div>");
                                    chathtml.Append("</div>");
                                }
                            }
                            else
                            {
                                //chathtml.Append("<span class=\"no-comment-staff\">Hiện chưa có đánh giá nào.</span>");
                            }
                        }
                        else
                        {

                            //chathtml.Append("<span class=\"no-comment-staff\">Hiện chưa có đánh giá nào.</span>");
                        }
                        ltrInComment.Text = chathtml.ToString();
                        #endregion
                        #region Lấy bình luận ngoài
                        StringBuilder chathtml2 = new StringBuilder();
                        var cs1 = OrderCommentController.GetByOrderIDAndType(o.ID, 1);
                        if (cs1 != null)
                        {
                            if (cs1.Count > 0)
                            {
                                foreach (var item in cs1)
                                {
                                    string fullname = "";

                                    int role = 0;
                                    int user_postID = 0;
                                    var user = AccountController.GetByID(Convert.ToInt32(item.CreatedBy));
                                    if (user != null)
                                    {
                                        user_postID = user.ID;
                                        role = Convert.ToInt32(user.RoleID);
                                        var userinfo = AccountController.GetByID(user.ID);
                                        if (userinfo != null)
                                        {
                                            fullname = userinfo.Username;
                                        }
                                    }
                                    if (uid == user_postID)
                                    {
                                        //ltrOutComment.Text += "<div class=\"mess-item mymess\">";
                                        chathtml2.Append("<div class=\"chat chat-right\">");
                                    }
                                    else
                                    {
                                        //ltrOutComment.Text += "<div class=\"mess-item \">";
                                        chathtml2.Append("<div class=\"chat\">");
                                    }
                                    chathtml2.Append("<div class=\"chat-avatar\">");
                                    chathtml2.Append("    <p class=\"name\">" + fullname + "</p>");
                                    //chathtml2.Append("    <p class=\"role\">" + RoleController.GetByID(user.RoleID.Value).RoleName + "</p>");
                                    chathtml2.Append("</div>");
                                    chathtml2.Append("<div class=\"chat-body\">");
                                    chathtml2.Append("        <div class=\"chat-text\">");
                                    chathtml2.Append("                <div class=\"date-time center-align\">" + string.Format("{0:dd/MM/yyyy HH:mm}", item.CreatedDate) + "</div>");
                                    chathtml2.Append("                <div class=\"text-content\">");
                                    chathtml2.Append("                    <div class=\"content\">");
                                    if (!string.IsNullOrEmpty(item.Link))
                                    {
                                        chathtml2.Append("<div class=\"content-img\">");
                                        //if (uid == user_postID)
                                        //{
                                        //    chathtml2.Append("<div class=\"content-img\" style=\"border-radius: 5px;background-color: #2196f3;\">");
                                        //    //ltrInComment.Text += "<div class=\"mess-item mymess\">";

                                        //}
                                        //else
                                        //{
                                        //    //ltrInComment.Text += "<div class=\"mess-item \">";
                                        //    chathtml2.Append("<div class=\"content-img\">");
                                        //}
                                        chathtml2.Append("<div class=\"img-block\">");
                                        if (item.Link.Contains(".doc"))
                                        {
                                            chathtml2.Append("<a href=\"" + item.Link + "\" target=\"_blank\"><img src=\"/App_Themes/AdminNew45/assets/images/icon/file.png\" title=\"" + item.Comment + "\"  class=\"\" height=\"50\"/></a>");

                                        }
                                        else if (item.Link.Contains(".xls"))
                                        {
                                            chathtml2.Append("<a href=\"" + item.Link + "\" target=\"_blank\"><img src=\"/App_Themes/AdminNew45/assets/images/icon/file.png\" title=\"" + item.Comment + "\"  class=\"\" height=\"50\"/></a>");
                                        }
                                        else
                                        {
                                            chathtml2.Append("<a href=\"" + item.Link + "\" target=\"_blank\"><img src=\"" + item.Link + "\" title=\"" + item.Comment + "\"  class=\"\" height=\"50\"/></a>");
                                        }                                        //chathtml2.Append("<img src=\"" + item.Link + "\" title=\"" + item.Comment + "\"  class=\"materialboxed\" height=\"50\"/>");
                                        chathtml2.Append("</div>");
                                        chathtml2.Append("</div>");
                                    }
                                    else
                                    {
                                        chathtml2.Append("                    <p>" + item.Comment + "</p>");
                                    }
                                    chathtml2.Append("                    </div>");
                                    chathtml2.Append("                </div>");
                                    chathtml2.Append("        </div>");
                                    chathtml2.Append("</div>");
                                    chathtml2.Append("</div>");

                                }
                            }
                            else
                            {
                                //ltrOutComment.Text += "<span class=\"no-comment-cust\">Hiện chưa có đánh giá nào.</span>";
                                //chathtml2.Append("<span class=\"no-comment-staff\">Hiện chưa có đánh giá nào.</span>");
                            }
                        }
                        else
                        {
                            //ltrOutComment.Text += "<span class=\"no-comment-cust\">Hiện chưa có đánh giá nào.</span>";
                            //chathtml2.Append("<span class=\"no-comment-staff\">Hiện chưa có đánh giá nào.</span>");
                        }
                        ltrOutComment.Text = chathtml2.ToString();
                        #endregion
                        #region Lấy danh sách bao nhỏ
                        StringBuilder spsList = new StringBuilder();
                        var smallpackages = SmallPackageController.GetByMainOrderID(id);
                        if (smallpackages.Count > 0)
                        {
                            foreach (var s in smallpackages)
                            {
                                int status = Convert.ToInt32(s.Status);
                                ltrMavandon.Text += "<tr>";
                                ltrMavandon.Text += "   <td>" + s.OrderTransactionCode + "</td>";
                                ltrMavandon.Text += "   <td>" + s.Weight + "</td>";
                                if (status == 1)
                                    ltrMavandon.Text += "<td>Chưa về kho Trung Quốc</td>";
                                else if (status == 2)
                                    ltrMavandon.Text += "<td>Đã về kho Trung Quốc</td>";
                                else if (status == 3)
                                    ltrMavandon.Text += "<td>Đã về kho Việt Nam</td>";
                                else if (status == 4)
                                    ltrMavandon.Text += "<td>Đã giao khách hàng</td>";
                                else if (status == 0)
                                    ltrMavandon.Text += "<td>Đã hủy</td>";
                                ltrMavandon.Text += "</tr>";


                                //spsList.Append("<div class=\"ordercode order-versionnew\" data-packageID=\"" + s.ID + "\">");
                                //spsList.Append("    <div class=\"item-element\">");
                                //spsList.Append("        <span>Mã Vận đơn:</span>");
                                //spsList.Append("        <input class=\"transactionCode form-control\" value=\"" + s.OrderTransactionCode + "\" type=\"text\" placeholder=\"Mã vận đơn\" />");
                                //spsList.Append("    </div>");
                                //spsList.Append("    <div class=\"item-element\">");
                                //spsList.Append("        <span>Cân nặng:</span>");
                                //spsList.Append("        <input class=\"transactionWeight form-control\" value=\"" + s.Weight + "\" type=\"text\" placeholder=\"Cân nặng\" onkeyup=\"returnWeightFee()\" />");
                                //spsList.Append("    </div>");
                                //spsList.Append("    <div class=\"item-element\">");
                                //spsList.Append("        <span>Trạng thái:</span>");
                                //spsList.Append("        <select class=\"transactionCodeStatus form-control\">");

                                //if (status == 1)
                                //    spsList.Append("            <option value=\"1\" selected>Mới đặt - chưa về kho TQ</option>");
                                //else
                                //    spsList.Append("            <option value=\"1\">Mới đặt - chưa về kho TQ</option>");
                                //if (status == 2)
                                //    spsList.Append("            <option value=\"2\" selected>Đã về kho TQ</option>");
                                //else
                                //    spsList.Append("            <option value=\"2\">Đã về kho TQ</option>");
                                //if (status == 3)
                                //    spsList.Append("            <option value=\"3\" selected>Đã về kho đích</option>");
                                //else
                                //    spsList.Append("            <option value=\"3\">Đã về kho đích</option>");
                                //if (status == 4)
                                //    spsList.Append("            <option value=\"4\" selected>Đã giao khách hàng</option>");
                                //else
                                //    spsList.Append("            <option value=\"4\">Đã giao khách hàng</option>");
                                //if (status == 0)
                                //    spsList.Append("            <option value=\"0\" selected>Đã hủy</option>");
                                //else
                                //    spsList.Append("            <option value=\"0\">Đã hủy</option>");
                                //spsList.Append("        </select>");
                                //spsList.Append("    </div>");
                                //spsList.Append("    <div class=\"item-element\">");
                                //spsList.Append("        <span>Ghi chú:</span>");
                                //spsList.Append("        <input class=\"transactionDescription form-control\" value=\"" + s.Description + "\" type=\"text\" placeholder=\"Ghi chú\"/>");
                                //spsList.Append("    </div>");
                                //spsList.Append("    <div class=\"item-element\"><a href=\"javascript:;\" class=\"btn primary-btn\" style=\"margin-top:19px;\" onclick=\"deleteOrderCode($(this))\">Xóa</a></div>");
                                //spsList.Append("</div>");

                                spsList.Append("            <tr class=\"ordercode order-versionnew\" data-packageID=\"" + s.ID + "\">");
                                spsList.Append("                <td>");
                                spsList.Append("                    <input class=\"transactionCode\" type=\"text\" value=\"" + s.OrderTransactionCode + "\"></td>");
                                spsList.Append("                <td>");
                                spsList.Append("                    <input class=\"transactionWeight\" onkeyup=\"returnWeightFee()\" data-type=\"number\" type=\"text\" value=\"" + s.Weight + "\"></td>");
                                spsList.Append("                <td>");
                                spsList.Append("                    <div class=\"input-field\">");
                                spsList.Append("                        <select class=\"transactionCodeMainOrderCode\">");

                                var ListMainOrderCode = MainOrderCodeController.GetAllByMainOrderID(o.ID);
                                if (ListMainOrderCode != null)
                                {

                                    var mainOrderCode = MainOrderCodeController.GetByID(Convert.ToInt32(s.MainOrderCodeID));
                                    if (mainOrderCode != null)
                                    {
                                        spsList.Append("            <option value=\"0\">Chọn mã đơn hàng</option>");
                                        foreach (var item in ListMainOrderCode)
                                        {
                                            if (mainOrderCode.MainOrderCode == item.MainOrderCode)
                                            {
                                                spsList.Append("            <option value=\"" + item.ID + "\" selected>" + item.MainOrderCode + "</option>");
                                            }
                                            else
                                            {
                                                spsList.Append("            <option value=\"" + item.ID + "\">" + item.MainOrderCode + "</option>");
                                            }
                                        }
                                    }
                                    else
                                    {
                                        spsList.Append("            <option value=\"0\">Chọn mã đơn hàng</option>");
                                        foreach (var item in ListMainOrderCode)
                                        {
                                            spsList.Append("            <option value=\"" + item.ID + "\">" + item.MainOrderCode + "</option>");
                                        }
                                    }

                                }
                                else
                                {
                                    spsList.Append("            <option value=\"0\">Chọn mã đơn hàng</option>");
                                }

                                spsList.Append("                        </select>");
                                spsList.Append("                    </div>");
                                spsList.Append("                </td>");

                                spsList.Append("                <td>");
                                spsList.Append("                    <div class=\"input-field\">");
                                spsList.Append("                        <select class=\"transactionCodeStatus\">");
                                if (status == 1)
                                    spsList.Append("            <option value=\"1\" selected>Chưa về kho Trung Quốc</option>");
                                else
                                    spsList.Append("            <option value=\"1\">Chưa về kho Trung Quốc</option>");
                                if (status == 2)
                                    spsList.Append("            <option value=\"2\" selected>Đã về kho Trung Quốc</option>");
                                else
                                    spsList.Append("            <option value=\"2\">Đã về kho Trung Quốc</option>");
                                if (status == 3)
                                    spsList.Append("            <option value=\"3\" selected>Đã về kho Việt Nam</option>");
                                else
                                    spsList.Append("            <option value=\"3\">Đã về kho Việt Nam</option>");
                                if (status == 4)
                                    spsList.Append("            <option value=\"4\" selected>Đã giao khách hàng</option>");
                                else
                                    spsList.Append("            <option value=\"4\">Đã giao khách hàng</option>");
                                if (status == 0)
                                    spsList.Append("            <option value=\"0\" selected>Đã hủy</option>");
                                else
                                    spsList.Append("            <option value=\"0\">Đã hủy</option>");

                                spsList.Append("                        </select>");
                                spsList.Append("                    </div>");
                                spsList.Append("                </td>");


                                spsList.Append("                <td>");
                                spsList.Append("                    <input class=\"transactionDescription\" type=\"text\" value=\"" + s.Description + "\"></td>");
                                spsList.Append("                </td>");
                                spsList.Append("            <td class=\"\">");
                                spsList.Append("                <a href='javascript:;' onclick=\"deleteOrderCode($(this))\" class=\"tooltipped\" data-position=\"top\" data-tooltip=\"Xóa\"><i class=\"material-icons valign-center\">delete</i></a>");
                                spsList.Append("            </td>");
                                spsList.Append("            </tr>");
                            }
                            ltrCodeList.Text = spsList.ToString();
                        }
                        #endregion
                        #region Lấy danh sách phụ phí
                        var listsp = FeeSupportController.GetAllByMainOrderID(o.ID);
                        if (listsp.Count > 0)
                        {
                            foreach (var item in listsp)
                            {
                                ltrFeeSupport.Text += "<tr class=\"feesupport fee-versionnew\" data-feesupportid=\"" + item.ID + "\">";
                                ltrFeeSupport.Text += "<td><input class=\"feesupportname\" type=\"text\" value=\"" + item.SupportName + "\"></td>";
                                ltrFeeSupport.Text += "<td><input class=\"feesupportvnd\" type=\"text\" value=\"" + string.Format("{0:N0}", Convert.ToDouble(item.SupportInfoVND )) + "\"></td>";
                                ltrFeeSupport.Text += "<td class=\"\"><a href=\"javascript:;\" onclick=\"deleteSupportFee($(this))\" class=\"tooltipped\" data-position=\"top\" data-tooltip=\"Xóa\"><i class=\"material-icons valign-center\">delete</i></a></td>";
                                ltrFeeSupport.Text += "</tr>";
                            }
                        }
                        #endregion
                    }
                    else
                    {
                        Response.Redirect("/trang-chu");
                    }
                    StringBuilder hisChange = new StringBuilder();
                    var historyorder = HistoryOrderChangeController.GetByMainOrderID(o.ID);
                    if (historyorder.Count > 0)
                    {
                        foreach (var item in historyorder)
                        {
                            string username = item.Username;
                            string rolename = "admin";
                            var acc = AccountController.GetByUsername(username);
                            if (acc != null)
                            {
                                int role = Convert.ToInt32(acc.RoleID);

                                var r = RoleController.GetByID(role);
                                if (r != null)
                                {
                                    rolename = r.RoleDescription;
                                }
                            }
                            hisChange.Append("<tr>");
                            hisChange.Append("    <td class=\"no-wrap\">" + string.Format("{0:dd/MM/yyyy HH:mm}", item.CreatedDate) + "</td>");
                            hisChange.Append("    <td class=\"no-wrap\">" + username + "</td>");
                            hisChange.Append("    <td class=\"no-wrap\">" + rolename + "</td>");
                            hisChange.Append("    <td>" + item.HistoryContent + "</td>");
                            hisChange.Append("</tr>");
                        }

                    }
                    else
                    {
                        hisChange.Append("<tr class=\"noti\">");
                        hisChange.Append("    <td class=\"red-text\" colspan=\"4\">Không có lịch sử thay đổi nào.</td>");
                        hisChange.Append("</tr>");
                    }
                    //lrtHistoryChange.Text = hisChange.ToString();
                }

            }


        }

        #region Button
        protected void btnSend1_Click(object sender, EventArgs e)
        {
            var orderID = hdfOrderID.Value.ToString();
            //var comment = txtComment1.Text;
            //sendcustomercomment(comment, orderID.ToInt(0));
            //if (!Page.IsValid) return;
            //string username = Session["userLoginSystem"].ToString();
            //var obj_user = AccountController.GetByUsername(username);
            //DateTime currentDate = DateTime.Now;
            //if (obj_user != null)
            //{
            //    int uid = obj_user.ID;
            //    //var id = Convert.ToInt32(Request.QueryString["id"]);
            //    var id = Convert.ToInt32(ViewState["ID"]);
            //    if (id > 0)
            //    {
            //        var o = MainOrderController.GetAllByID(id);
            //        if (o != null)
            //        {
            //            int type = 1;
            //            if (type > 0)
            //            {
            //                //txtComment1.Text
            //                string kq = OrderCommentController.Insert(id, txtComment1.Text, true, type, DateTime.Now, uid);
            //                if (type == 1)
            //                {
            //                    NotificationController.Inser(obj_user.ID, obj_user.Username, Convert.ToInt32(o.UID),
            //                        AccountController.GetByID(Convert.ToInt32(o.UID)).Username, id, "Đã có đánh giá mới cho đơn hàng #" + id + " của bạn. CLick vào để xem", 0,
            //                        1, currentDate, obj_user.Username, true);
            //                    try
            //                    {
            //                        PJUtils.SendMailGmail("no-reply@mona-media.com",
            //                            "demonhunter",
            //                            AccountInfoController.GetByUserID(Convert.ToInt32(o.UID)).Email,
            //                            "Thông báo tại Taobao.vn.",
            //                            "Đã có đánh giá mới cho đơn hàng #" + id
            //                            + " của bạn. CLick vào để xem", "");
            //                    }
            //                    catch { }
            //                }
            //                if (Convert.ToInt32(kq) > 0)
            //                {
            //                    var hubContext = GlobalHost.ConnectionManager.GetHubContext<ChatHub>();
            //                    hubContext.Clients.All.addNewMessageToPage("", "");
            //                    PJUtils.ShowMsg("Gửi đánh giá thành công.", true, Page);
            //                }
            //            }
            //            else
            //            {
            //                PJUtils.ShowMessageBoxSwAlert("Vui lòng chọn khu vực", "e", false, Page);
            //            }
            //        }
            //    }
            //}
        }
        protected void btnSend_Click(object sender, EventArgs e)
        {
            var orderID = hdfOrderID.Value.ToString();
            //var comment = txtComment.Text;
            //sendstaffcomment(comment, orderID.ToInt(0));
            //if (!Page.IsValid) return;
            //string username = Session["userLoginSystem"].ToString();
            //var obj_user = AccountController.GetByUsername(username);
            //DateTime currentDate = DateTime.Now;
            //if (obj_user != null)
            //{
            //    int uid = obj_user.ID;
            //    //var id = Convert.ToInt32(Request.QueryString["id"]);
            //    var id = Convert.ToInt32(ViewState["ID"]);
            //    if (id > 0)
            //    {
            //        var o = MainOrderController.GetAllByID(id);
            //        if (o != null)
            //        {
            //            int type = 2;
            //            if (type > 0)
            //            {
            //                //txtComment.Text
            //                string kq = OrderCommentController.Insert(id, txtComment.Text, true, type, DateTime.Now, uid);
            //                if (type == 1)
            //                {
            //                    NotificationController.Inser(obj_user.ID, obj_user.Username, Convert.ToInt32(o.UID),
            //                        AccountController.GetByID(Convert.ToInt32(o.UID)).Username, id, "Đã có đánh giá mới cho đơn hàng #" + id + " của bạn. CLick vào để xem", 0,
            //                        1, currentDate, obj_user.Username, false);
            //                    try
            //                    {
            //                        PJUtils.SendMailGmail("no-reply@mona-media.com", "demonhunter",
            //                            AccountInfoController.GetByUserID(Convert.ToInt32(o.UID)).Email,
            //                            "Thông báo tại Taobao.vn.",
            //                            "Đã có đánh giá mới cho đơn hàng #" + id
            //                            + " của bạn. CLick vào để xem", "");
            //                    }
            //                    catch { }
            //                }
            //                if (Convert.ToInt32(kq) > 0)
            //                {
            //                    var hubContext = GlobalHost.ConnectionManager.GetHubContext<ChatHub>();
            //                    hubContext.Clients.All.addNewMessageToPage("", "");
            //                    PJUtils.ShowMsg("Gửi đánh giá thành công.", true, Page);
            //                }
            //            }
            //            else
            //            {
            //                PJUtils.ShowMessageBoxSwAlert("Vui lòng chọn khu vực", "e", false, Page);
            //            }
            //        }
            //    }
            //}
        }
        protected void btnUpdate_Click(object sender, EventArgs e)
        {
            if (!Page.IsValid) return;
            string username = Session["userLoginSystem"].ToString();
            var obj_user = AccountController.GetByUsername(username);
            DateTime currentDate = DateTime.Now;
            if (obj_user != null)
            {
                int uid = obj_user.ID;

                int RoleID = obj_user.RoleID.ToString().ToInt();
                //var id = Convert.ToInt32(Request.QueryString["id"]);
                var id = Convert.ToInt32(ViewState["ID"]);
                if (id > 0)
                {
                    var o = MainOrderController.GetAllByID(id);
                    if (o != null)
                    {
                        int uidmuahang = Convert.ToInt32(o.UID);
                        string usermuahang = "";

                        var accmuahan = AccountController.GetByID(uidmuahang);
                        if (accmuahan != null)
                        {
                            usermuahang = accmuahan.Username;
                        }
                        string CurrentOrderTransactionCode = o.OrderTransactionCode;
                        string CurrentOrderTransactionCode2 = o.OrderTransactionCode2;
                        string CurrentOrderTransactionCode3 = o.OrderTransactionCode3;
                        string CurrentOrderTransactionCode4 = o.OrderTransactionCode4;
                        string CurrentOrderTransactionCode5 = o.OrderTransactionCode5;

                        string CurrentOrderTransactionCodeWeight = o.OrderTransactionCodeWeight;
                        string CurrentOrderTransactionCodeWeight2 = o.OrderTransactionCodeWeight2;
                        string CurrentOrderTransactionCodeWeight3 = o.OrderTransactionCodeWeight3;
                        string CurrentOrderTransactionCodeWeight4 = o.OrderTransactionCodeWeight4;
                        string CurrentOrderTransactionCodeWeight5 = o.OrderTransactionCodeWeight5;

                        string CurrentOrderWeight = o.OrderWeight;

                        #region cập nhật và tạo mới smallpackage
                        string tcl = hdfCodeTransactionList.Value;
                        if (!string.IsNullOrEmpty(tcl))
                        {
                            string[] list = tcl.Split('|');
                            for (int i = 0; i < list.Length - 1; i++)
                            {
                                string[] item = list[i].Split(',');
                                int ID = item[0].ToInt(0);
                                string code = item[1].Trim();
                                string weight = item[2];
                                double weightin = 0;
                                if (!string.IsNullOrEmpty(weight))
                                    weightin = Math.Round(Convert.ToDouble(weight), 1);
                                int smallpackage_status = item[3].ToInt(1);
                                string description = item[4];
                                string mainOrderCodeID = item[5];
                                var MainOrderCode = MainOrderCodeController.GetByID(mainOrderCodeID.ToInt(0));
                                if (MainOrderCode == null)
                                    PJUtils.ShowMessageBoxSwAlert("Lỗi, không có mã đơn hàng", "e", false, Page);
                                if (ID > 0)
                                {
                                    var smp = SmallPackageController.GetByID(ID);
                                    if (smp != null)
                                    {
                                        int bigpackageID = Convert.ToInt32(smp.BigPackageID);
                                        bool check = false;
                                        var getsmallcheck = SmallPackageController.GetByOrderCode(code);
                                        if (getsmallcheck.Count > 0)
                                        {
                                            foreach (var sp in getsmallcheck)
                                            {
                                                if (sp.ID == ID)
                                                {
                                                    check = true;
                                                }
                                            }
                                        }
                                        else
                                        {
                                            check = true;
                                        }
                                        if (check)
                                        {
                                            SmallPackageController.UpdateNew(ID, accmuahan.ID, usermuahang, bigpackageID, code,
                                                smp.ProductType, Math.Round(Convert.ToDouble(smp.FeeShip), 0),
                                            weightin, Math.Round(Convert.ToDouble(smp.Volume), 1), smallpackage_status,
                                            description, currentDate, username, mainOrderCodeID.ToInt(0));

                                            if (smallpackage_status == 2)
                                            {
                                                SmallPackageController.UpdateDateInTQWareHouse(ID, username, currentDate);
                                            }
                                            else if (smallpackage_status == 3)
                                            {
                                                SmallPackageController.UpdateDateInVNWareHouse(ID, username, currentDate);
                                            }
                                            var bigpack = BigPackageController.GetByID(bigpackageID);
                                            if (bigpack != null)
                                            {
                                                int TotalPackageWaiting = SmallPackageController.GetCountByBigPackageIDStatus(bigpackageID, 1, 2);
                                                if (TotalPackageWaiting == 0)
                                                {
                                                    BigPackageController.UpdateStatus(bigpackageID, 2, currentDate, username);
                                                }
                                            }
                                        }
                                    }
                                    else
                                    {
                                        //var getsmallcheck = SmallPackageController.GetByOrderCode(code);
                                        //if (getsmallcheck.Count > 0)
                                        //{
                                        //    PJUtils.ShowMessageBoxSwAlert("Mã kiện đã tồn tại, vui lòng tạo mã khác", "e", true, Page);
                                        //}
                                        //else
                                        //{
                                        SmallPackageController.InsertWithMainOrderIDUIDUsernameNew(id, accmuahan.ID, usermuahang,
                                            0, code, "", 0, weightin, 0,
                                        smallpackage_status, description, currentDate, username, mainOrderCodeID.ToInt(0), 0);

                                        HistoryOrderChangeController.Insert(o.ID, obj_user.ID, obj_user.Username, obj_user.Username +
                                 " đã thêm mã vận đơn của đơn hàng ID là: " + o.ID + ", Mã vận đơn: " + code + ", cân nặng: " + weightin + "", 8, currentDate);

                                        if (smallpackage_status == 2)
                                        {
                                            SmallPackageController.UpdateDateInTQWareHouse(ID, username, currentDate);
                                        }
                                        else if (smallpackage_status == 3)
                                        {
                                            SmallPackageController.UpdateDateInVNWareHouse(ID, username, currentDate);
                                        }
                                        //}

                                    }
                                }
                                else
                                {
                                    //bool check = false;
                                    //var getsmallcheck = SmallPackageController.GetByOrderCode(code);
                                    //if (getsmallcheck.Count > 0)
                                    //{
                                    //    PJUtils.ShowMessageBoxSwAlert("Mã kiện đã tồn tại, vui lòng tạo mã khác", "e", true, Page);
                                    //}
                                    //else
                                    //{
                                    SmallPackageController.InsertWithMainOrderIDUIDUsernameNew(id, accmuahan.ID, usermuahang, 0,
                                        code, "", 0, weightin, 0,
                                    smallpackage_status, description, currentDate, username, mainOrderCodeID.ToInt(0), 0);
                                    if (smallpackage_status == 2)
                                    {
                                        SmallPackageController.UpdateDateInTQWareHouse(ID, username, currentDate);
                                    }
                                    else if (smallpackage_status == 3)
                                    {
                                        SmallPackageController.UpdateDateInVNWareHouse(ID, username, currentDate);
                                    }
                                    //}
                                }
                            }
                        }
                        #endregion

                        double TotalFeeSupport = 0;
                        #region Cập nhật và tạo mới phụ phí
                        string lsp = hdfListFeeSupport.Value;
                        if (!string.IsNullOrEmpty(lsp))
                        {
                            string[] list = lsp.Split('|');
                            for (int i = 0; i < list.Length - 1; i++)
                            {
                                string[] item = list[i].Split(',');
                                int ID = item[0].ToInt(0);
                                string fname = item[1];
                                double FeeSupport = Convert.ToDouble(item[2]);
                                TotalFeeSupport += FeeSupport;
                                if (ID > 0)
                                {
                                    var check = FeeSupportController.GetByID(ID);
                                    if (check != null)
                                    {
                                        FeeSupportController.Update(check.ID, fname, FeeSupport.ToString(), obj_user.Username, currentDate);
                                        if (check.SupportName != fname)
                                        {
                                            HistoryOrderChangeController.Insert(o.ID, obj_user.ID, obj_user.Username, obj_user.Username +
                                      " đã thay đổi tên phụ phí của đơn hàng ID là: " + o.ID + ", Từ: " + check.SupportName + ", Sang: "
                                      + fname + "", 10, currentDate);
                                        }

                                        if (check.SupportInfoVND != FeeSupport.ToString())
                                        {
                                            HistoryOrderChangeController.Insert(o.ID, obj_user.ID, obj_user.Username, obj_user.Username +
                                      " đã thay đổi tiền phụ phí của đơn hàng ID là: " + o.ID + ", Tên phụ phí: " + fname + ", Số tiền từ: "
                                      + string.Format("{0:N0}", Convert.ToDouble(check.SupportInfoVND)) + ", Sang: "
                                      + string.Format("{0:N0}", Convert.ToDouble(FeeSupport)) + "", 10, currentDate);
                                        }
                                    }
                                }
                                else
                                {
                                    FeeSupportController.Insert(o.ID, fname, FeeSupport.ToString(), obj_user.Username, currentDate);
                                    HistoryOrderChangeController.Insert(o.ID, obj_user.ID, obj_user.Username, obj_user.Username +
                                       " đã thêm phụ phí của đơn hàng ID là: " + o.ID + ", Tên phụ phí: " + fname + ", Số tiền: "
                                       + string.Format("{0:N0}", Convert.ToDouble(FeeSupport)) + "", 10, currentDate);

                                }
                            }
                        }
                        #endregion
                        #region Lấy ra text của trạng thái đơn hàng
                        string orderstatus = "";
                        int currentOrderStatus = Convert.ToInt32(o.Status);
                        switch (currentOrderStatus)
                        {
                            case 0:
                                orderstatus = "Chờ đặt cọc";
                                break;
                            case 1:
                                orderstatus = "Hủy đơn hàng";
                                break;
                            case 2:
                                orderstatus = "Đã đặt cọc";
                                break;
                            case 5:
                                orderstatus = "Đã mua hàng";
                                break;
                            case 6:
                                orderstatus = "Đã về kho Trung Quốc";
                                break;
                            case 7:
                                orderstatus = "Đã về kho Việt Nam";
                                break;
                            case 8:
                                orderstatus = "Chờ thanh toán";
                                break;
                            case 9:
                                orderstatus = "Khách đã thanh toán";
                                break;
                            case 10:
                                orderstatus = "Đã hoàn thành";
                                break;
                            default:
                                break;
                        }
                        #endregion
                        #region Cập nhật nhân viên KhoTQ và nhân viên KhoVN
                        if (RoleID == 4)
                        {
                            if (o.KhoTQID == uid || o.KhoTQID == 0)
                            {
                                MainOrderController.UpdateStaff(o.ID, o.SalerID.ToString().ToInt(0), o.DathangID.ToString().ToInt(0), uid, o.KhoVNID.ToString().ToInt(0));
                            }
                            else
                            {
                                PJUtils.ShowMessageBoxSwAlert("Có lỗi trong quá trình xử lý", "e", true, Page);
                            }
                        }
                        else if (RoleID == 5)
                        {
                            if (o.KhoVNID == uid || o.KhoTQID == 0)
                            {
                                MainOrderController.UpdateStaff(o.ID, o.SalerID.ToString().ToInt(0), o.DathangID.ToString().ToInt(0), o.KhoTQID.ToString().ToInt(0), uid);
                            }
                            else
                            {
                                PJUtils.ShowMessageBoxSwAlert("Có lỗi trong quá trình xử lý", "e", true, Page);
                            }
                        }
                        #endregion
                        #region cập nhật thông tin của đơn hàng
                        double feeeinwarehouse = 0;
                        int status = ddlStatus.SelectedValue.ToString().ToInt();
                        if (status == 1)
                        {
                            double Deposit = 0;
                            if (o.Deposit.ToFloat(0) > 0)
                                Deposit = Math.Round(Convert.ToDouble(o.Deposit), 0);
                            if (Deposit > 0)
                            {
                                var user_order = AccountController.GetByID(o.UID.ToString().ToInt());
                                if (user_order != null)
                                {
                                    double wallet = 0;
                                    if (user_order.Wallet.ToString().ToFloat(0) > 0)
                                        wallet = Math.Round(Convert.ToDouble(user_order.Wallet), 0);
                                    wallet = wallet + Deposit;
                                    HistoryPayWalletController.Insert(user_order.ID, user_order.Username, o.ID, Deposit,
                                        "Đơn hàng: " + o.ID + " bị hủy và hoàn tiền cọc cho khách.", wallet, 2, 2, currentDate, obj_user.Username);
                                    //HistoryOrderChangeController.Insert(o.ID, obj_user.ID, obj_user.Username, obj_user.Username +
                                    //    " đã đổi trạng thái của đơn hàng: " + o.ID + " từ " + orderstatus + " sang " + ddlStatus.SelectedItem + "", 0, currentDate);
                                    AccountController.updateWallet(user_order.ID, wallet, currentDate, obj_user.Username);
                                    MainOrderController.UpdateDeposit(o.ID, "0");
                                }
                            }
                        }
                        else
                        {
                            double OCurrent_deposit = 0;
                            if (o.Deposit.ToFloat(0) > 0)
                                OCurrent_deposit = Math.Round(Convert.ToDouble(o.Deposit), 0);

                            double OCurrent_FeeShipCN = 0;
                            if (o.FeeShipCN.ToFloat(0) > 0)
                                OCurrent_FeeShipCN = Math.Round(Convert.ToDouble(o.FeeShipCN), 2);

                            double OCurrent_FeeBuyPro = 0;
                            if (o.FeeBuyPro.ToFloat(0) > 0)
                                OCurrent_FeeBuyPro = Math.Round(Convert.ToDouble(o.FeeBuyPro), 0);

                            double OCurrent_FeeWeight = 0;
                            if (o.FeeWeight.ToFloat(0) > 0)
                                OCurrent_FeeWeight = Math.Round(Convert.ToDouble(o.FeeWeight), 0);

                            double OCurrent_IsCheckProductPrice = 0;
                            if (o.IsCheckProductPrice.ToFloat(0) > 0)
                                OCurrent_IsCheckProductPrice = Math.Round(Convert.ToDouble(o.IsCheckProductPrice), 0);

                            double OCurrent_IsPackedPrice = 0;
                            if (o.IsPackedPrice.ToFloat(0) > 0)
                                OCurrent_IsPackedPrice = Math.Round(Convert.ToDouble(o.IsPackedPrice), 0);

                            double OCurrent_IsFastDeliveryPrice = 0;
                            if (o.IsFastDeliveryPrice.ToFloat(0) > 0)
                                OCurrent_IsFastDeliveryPrice = Math.Round(Convert.ToDouble(o.IsFastDeliveryPrice), 0);

                            double OCurrent_TotalPriceReal = 0;
                            if (o.TotalPriceReal.ToFloat(0) > 0)
                                OCurrent_TotalPriceReal = Math.Round(Convert.ToDouble(o.TotalPriceReal), 0);

                            double OCurrent_TotalPriceRealCYN = 0;
                            if (o.TotalPriceRealCYN.ToFloat(0) > 0)
                                OCurrent_TotalPriceRealCYN = Math.Round(Convert.ToDouble(o.TotalPriceRealCYN), 2);

                            //double OCurrent_FeeShipCNToVN = Convert.ToDouble(o.FeeShipCNToVN);

                            double Deposit = Math.Round(Convert.ToDouble(pDeposit.Text), 0);
                            double FeeShipCN = Math.Round(Convert.ToDouble(pCNShipFee.Text), 0);
                            double FeeBuyPro = Math.Round(Convert.ToDouble(pBuy.Text), 0);
                            double FeeWeight = Math.Round(Convert.ToDouble(pWeight.Text), 0);
                            double FeeVolume = Math.Round(Convert.ToDouble(pVolume.Text), 0);
                            double TotalPriceReal = Math.Round(Convert.ToDouble(rTotalPriceReal.Text), 0);
                            double TotalPriceRealCYN = Math.Round(Convert.ToDouble(rTotalPriceRealCYN.Text), 2);

                            if (o.FeeInWareHouse != null)
                                feeeinwarehouse = Math.Round(Convert.ToDouble(o.FeeInWareHouse), 0);
                            //double FeeShipCNToVN = Convert.ToDouble(pWeight.Value);

                            double IsCheckProductPrice = 0;
                            if (pCheck.Text.ToString().ToFloat(0) > 0)
                                IsCheckProductPrice = Math.Round(Convert.ToDouble(pCheck.Text), 0);
                            //if (o.IsCheckProduct == true)
                            //    IsCheckProductPrice = Convert.ToDouble(pCheck.Value);
                            //else
                            //    IsCheckProductPrice = Convert.ToDouble(o.IsCheckProductPrice);

                            double IsPackedPrice = 0;
                            if (pPacked.Text.ToString().ToFloat(0) > 0)
                                IsPackedPrice = Math.Round(Convert.ToDouble(pPacked.Text), 0);
                            //if (o.IsPacked == true)
                            //    IsPackedPrice = Convert.ToDouble(pPacked.Value);
                            //else
                            //    IsPackedPrice = Convert.ToDouble(o.IsPackedPrice);

                            double IsFastDeliveryPrice = 0;
                            if (pShipHome.Text.ToString().ToFloat(0) > 0)
                                IsFastDeliveryPrice = Math.Round(Convert.ToDouble(pShipHome.Text), 0);

                            //if (o.IsFastDelivery == true)
                            //    IsFastDeliveryPrice = Convert.ToDouble(pShipHome.Value);
                            //else
                            //    IsFastDeliveryPrice = Convert.ToDouble(o.IsFastDeliveryPrice);


                            #region Ghi lịch sử chỉnh sửa các loại giá
                            if (OCurrent_deposit != Deposit)
                            {
                                //NotificationController.Inser(obj_user.ID, obj_user.Username, Convert.ToInt32(o.UID),
                                //    AccountController.GetByID(Convert.ToInt32(o.UID)).Username, id, "Đã có cập nhật mới về tiền đặt cọc cho đơn hàng #" + id + " của bạn. CLick vào để xem", 0,
                                //    currentDate, obj_user.Username);

                                //try
                                //{
                                //    PJUtils.SendMailGmail("no-reply@mona-media.com", "demonhunter", AccountInfoController.GetByUserID(Convert.ToInt32(o.UID)).Email,
                                //        "Thông báo tại 1688 Express.", "Đã có cập nhật mới về tiền đặt cọc cho đơn hàng #" + id + " của bạn. CLick vào để xem", "");
                                //}
                                //catch
                                //{

                                //}
                                HistoryOrderChangeController.Insert(o.ID, obj_user.ID, obj_user.Username, obj_user.Username +
                                        " đã đổi tiền đặt cọc của đơn hàng ID là: " + o.ID + ", từ: " + string.Format("{0:N0}", OCurrent_deposit) + ", sang: "
                                        + string.Format("{0:N0}", Deposit) + "", 1, currentDate);
                            }
                            if (OCurrent_FeeShipCN != FeeShipCN)
                            {
                                //NotificationController.Inser(obj_user.ID, obj_user.Username, Convert.ToInt32(o.UID),
                                //    AccountController.GetByID(Convert.ToInt32(o.UID)).Username, id, "Đã có cập nhật mới về tiền phí ship Trung Quốc cho đơn hàng #" + id + " của bạn. CLick vào để xem", 0,
                                //    currentDate, obj_user.Username);


                                //try
                                //{
                                //    PJUtils.SendMailGmail("no-reply@mona-media.com", "demonhunter", AccountInfoController.GetByUserID(Convert.ToInt32(o.UID)).Email,
                                //        "Thông báo tại 1688 Express.", "Đã có cập nhật mới về tiền phí ship Trung Quốc cho đơn hàng #" + id + " của bạn. CLick vào để xem", "");
                                //}
                                //catch { }
                                HistoryOrderChangeController.Insert(o.ID, obj_user.ID, obj_user.Username, obj_user.Username +
                                        " đã đổi tiền phí ship Trung Quốc của đơn hàng ID là: " + o.ID + ", từ " + string.Format("{0:N0}", OCurrent_FeeShipCN) + " sang "
                                        + string.Format("{0:N0}", FeeShipCN) + "", 2, currentDate);
                            }
                            if (OCurrent_FeeBuyPro < FeeBuyPro || OCurrent_FeeBuyPro > FeeBuyPro)
                            {
                                //NotificationController.Inser(obj_user.ID, obj_user.Username, Convert.ToInt32(o.UID),
                                //    AccountController.GetByID(Convert.ToInt32(o.UID)).Username, id, "Đã có cập nhật mới về tiền phí mua sản phẩm cho đơn hàng #" + id + " của bạn. CLick vào để xem", 0,
                                //    currentDate, obj_user.Username);


                                //try
                                //{
                                //    PJUtils.SendMailGmail("no-reply@mona-media.com", "demonhunter", AccountInfoController.GetByUserID(Convert.ToInt32(o.UID)).Email,
                                //        "Thông báo tại 1688 Express.", "Đã có cập nhật mới về tiền phí mua sản phẩm cho đơn hàng #" + id + " của bạn. CLick vào để xem", "");
                                //}
                                //catch { }
                                HistoryOrderChangeController.Insert(o.ID, obj_user.ID, obj_user.Username, obj_user.Username +
                                        " đã đổi tiền phí mua sản phẩm của đơn hàng ID là: " + o.ID + ", từ: " + string.Format("{0:N0}", OCurrent_FeeBuyPro) + ", sang: "
                                        + string.Format("{0:N0}", FeeBuyPro) + "", 3, currentDate);
                            }
                            if (OCurrent_TotalPriceReal < TotalPriceReal || OCurrent_TotalPriceReal > TotalPriceReal)
                            {
                                //NotificationController.Inser(obj_user.ID, obj_user.Username, Convert.ToInt32(o.UID),
                                //    AccountController.GetByID(Convert.ToInt32(o.UID)).Username, id, "Đã có cập nhật mới về Tổng tiền mua thật cho đơn hàng #" + id + " của bạn. CLick vào để xem", 0,
                                //    currentDate, obj_user.Username);
                                //try
                                //{
                                //    PJUtils.SendMailGmail("no-reply@mona-media.com", "demonhunter", AccountInfoController.GetByUserID(Convert.ToInt32(o.UID)).Email,
                                //        "Thông báo tại 1688 Express.", "Đã có cập nhật mới về tiền phí mua sản phẩm cho đơn hàng #" + id + " của bạn. CLick vào để xem", "");
                                //}
                                //catch { }
                                HistoryOrderChangeController.Insert(o.ID, obj_user.ID, obj_user.Username, obj_user.Username +
                                        " đã đổi tiền phí mua thật của đơn hàng ID là: " + o.ID + ", từ: " + string.Format("{0:N0}", OCurrent_TotalPriceReal) + ", sang: "
                                        + string.Format("{0:N0}", TotalPriceReal) + "", 3, currentDate);
                            }
                            if (OCurrent_FeeWeight != FeeWeight)
                            {
                                //NotificationController.Inser(obj_user.ID, obj_user.Username, Convert.ToInt32(o.UID),
                                //    AccountController.GetByID(Convert.ToInt32(o.UID)).Username, id, "Đã có cập nhật mới về tiền phí TQ-VN cho đơn hàng #" + id + " của bạn. CLick vào để xem", 0,
                                //    currentDate, obj_user.Username);

                                //try
                                //{
                                //    PJUtils.SendMailGmail("no-reply@mona-media.com", "demonhunter", AccountInfoController.GetByUserID(Convert.ToInt32(o.UID)).Email,
                                //        "Thông báo tại 1688 Express.", "Đã có cập nhật mới về tiền phí TQ-VN cho đơn hàng #" + id + " của bạn. CLick vào để xem", "");
                                //}
                                //catch { }
                                HistoryOrderChangeController.Insert(o.ID, obj_user.ID, obj_user.Username, obj_user.Username +
                                        " đã đổi tiền phí TQ-VN của đơn hàng ID là: " + o.ID + ", từ: " + string.Format("{0:N0}", OCurrent_FeeWeight) + ", sang: "
                                        + string.Format("{0:N0}", FeeWeight) + "", 4, currentDate);
                            }
                            if (OCurrent_IsCheckProductPrice != IsCheckProductPrice)
                            {
                                //NotificationController.Inser(obj_user.ID, obj_user.Username, Convert.ToInt32(o.UID),
                                //    AccountController.GetByID(Convert.ToInt32(o.UID)).Username, id, "Đã có cập nhật mới về tiền phí kiểm tra sản phẩm cho đơn hàng #" + id + " của bạn. CLick vào để xem", 0,
                                //    currentDate, obj_user.Username);

                                //try
                                //{
                                //    PJUtils.SendMailGmail("no-reply@mona-media.com", "demonhunter", AccountInfoController.GetByUserID(Convert.ToInt32(o.UID)).Email,
                                //        "Thông báo tại 1688 Express.", "Đã có cập nhật mới về tiền phí kiểm tra sản phẩm cho đơn hàng #" + id + " của bạn. CLick vào để xem", "");
                                //}
                                //catch { }
                                HistoryOrderChangeController.Insert(o.ID, obj_user.ID, obj_user.Username, obj_user.Username +
                                        " đã đổi tiền phí kiểm tra sản phẩm của đơn hàng ID là: " + o.ID + ", từ: " + string.Format("{0:N0}", OCurrent_IsCheckProductPrice) + ", sang: "
                                        + string.Format("{0:N0}", IsCheckProductPrice) + "", 5, currentDate);
                            }
                            if (OCurrent_IsPackedPrice != IsPackedPrice)
                            {
                                //NotificationController.Inser(obj_user.ID, obj_user.Username, Convert.ToInt32(o.UID),
                                //    AccountController.GetByID(Convert.ToInt32(o.UID)).Username, id, "Đã có cập nhật mới về tiền phí đóng gỗ cho đơn hàng #" + id + " của bạn. CLick vào để xem", 0,
                                //    currentDate, obj_user.Username);


                                //try
                                //{
                                //    PJUtils.SendMailGmail("no-reply@mona-media.com", "demonhunter", AccountInfoController.GetByUserID(Convert.ToInt32(o.UID)).Email,
                                //        "Thông báo tại 1688 Express.", "Đã có cập nhật mới về tiền phí đóng gỗ cho đơn hàng #" + id + " của bạn. CLick vào để xem", "");
                                //}
                                //catch { }
                                HistoryOrderChangeController.Insert(o.ID, obj_user.ID, obj_user.Username, obj_user.Username +
                                        " đã đổi tiền phí đóng gỗ của đơn hàng ID là: " + o.ID + ", từ: " + string.Format("{0:N0}", OCurrent_IsPackedPrice) + ", sang: "
                                        + string.Format("{0:N0}", IsPackedPrice) + "", 6, currentDate);
                            }
                            if (OCurrent_IsFastDeliveryPrice != IsFastDeliveryPrice)
                            {
                                //NotificationController.Inser(obj_user.ID, obj_user.Username, Convert.ToInt32(o.UID),
                                //    AccountController.GetByID(Convert.ToInt32(o.UID)).Username, id, "Đã có cập nhật mới về tiền phí ship giao hàng tận nhà cho đơn hàng #" + id + " của bạn. CLick vào để xem", 0,
                                //    currentDate, obj_user.Username);


                                //try
                                //{
                                //    PJUtils.SendMailGmail("no-reply@mona-media.com", "demonhunter", AccountInfoController.GetByUserID(Convert.ToInt32(o.UID)).Email,
                                //        "Thông báo tại 1688 Express.", "Đã có cập nhật mới về tiền phí ship giao hàng tận nhà cho đơn hàng #" + id + " của bạn. CLick vào để xem", "");
                                //}
                                //catch { }
                                HistoryOrderChangeController.Insert(o.ID, obj_user.ID, obj_user.Username, obj_user.Username +
                                        " đã đổi tiền phí ship giao hàng tận nhà của đơn hàng ID là: " + o.ID + ", từ: " + string.Format("{0:N0}", OCurrent_IsFastDeliveryPrice) + ", sang: "
                                        + string.Format("{0:N0}", IsFastDeliveryPrice) + "", 7, currentDate);
                            }
                            //if (OCurrent_FeeShipCNToVN != FeeShipCNToVN)
                            //{
                            //    HistoryOrderChangeController.Insert(o.ID, obj_user.ID, obj_user.Username, obj_user.Username +
                            //            " đã đổi tiền phí vận chuyển TQ-VN của đơn hàng ID là: " + o.ID + ", từ: " + string.Format("{0:N0}", OCurrent_FeeShipCNToVN) + ", sang: "
                            //            + string.Format("{0:N0}", FeeShipCNToVN) + "", 7, currentDate);
                            //}
                            #endregion
                            double isfastprice = 0;
                            if (o.IsFastPrice.ToFloat(0) > 0)
                                isfastprice = Math.Round(Convert.ToDouble(o.IsFastPrice), 0);

                            double pricenvd = 0;
                            if (o.PriceVND.ToFloat(0) > 0)
                                pricenvd = Math.Round(Convert.ToDouble(o.PriceVND), 0);

                            double TotalPriceVND = FeeShipCN + FeeBuyPro + FeeWeight + IsCheckProductPrice + IsPackedPrice
                                                 + IsFastDeliveryPrice + isfastprice + pricenvd + TotalFeeSupport;
                            TotalPriceVND = Math.Round(TotalPriceVND, 0);
                            //if (ddlStatus.SelectedValue != o.Status.ToString())
                            //{
                            //int stt = Convert.ToInt32(ddlStatus.SelectedValue);
                            //MainOrderController.UpdateFeeShipTQVN(o.ID, FeeShipCNToVN.ToString());
                            MainOrderController.UpdateFeeNew(o.ID, Deposit.ToString(), FeeShipCN.ToString(), FeeBuyPro.ToString(), FeeWeight.ToString(), FeeVolume.ToString(), IsCheckProductPrice.ToString(),
                                IsPackedPrice.ToString(), IsFastDeliveryPrice.ToString(), TotalPriceVND.ToString());

                            if (status == 2)
                            {
                                MainOrderController.UpdateDepositDate(o.ID, currentDate);
                            }

                        }
                        int currentstt = Convert.ToInt32(o.Status);

                        if (currentstt < 3 || currentstt > 7)
                        {
                            if (status != currentstt)
                            {
                                //string retnote = NotificationController.Inser(obj_user.ID, obj_user.Username, Convert.ToInt32(o.UID),
                                //    AccountController.GetByID(Convert.ToInt32(o.UID)).Username, id, "Đã có cập nhật mới cho đơn hàng #" + id + " của bạn. CLick vào để xem", 0,
                                //    currentDate, obj_user.Username);

                                OrderCommentController.Insert(id, "Đã có cập nhật mới cho đơn hàng #" + id + " của bạn.", true, 1, DateTime.Now, uid);
                                try
                                {
                                    //PJUtils.SendMailGmail("cskh@1688pgs.vn", "1688pegasus", 
                                    //AccountInfoController.GetByUserID(Convert.ToInt32(o.UID)).Email, 
                                    //"Thông báo tại 1688PGS.", "Đã có cập nhật trạng thái cho đơn hàng #" + id + " của bạn. CLick vào để xem", "");
                                    PJUtils.SendMailGmail("no-reply@mona-media.com", "demonhunter",
                                        AccountInfoController.GetByUserID(Convert.ToInt32(o.UID)).Email,
                                                "Thông báo tại Taobao.vn",
                                                "Đã có cập nhật trạng thái cho đơn hàng #" + id
                                                + " của bạn. CLick vào để xem", "");
                                }
                                catch { }
                            }
                        }
                        else if (currentstt > 2 && currentstt < 8)
                        {
                            if (status < 3 || status > 7)
                            {
                                //string retnote = NotificationController.Inser(obj_user.ID, obj_user.Username, Convert.ToInt32(o.UID),
                                //    AccountController.GetByID(Convert.ToInt32(o.UID)).Username, id, "Đã có cập nhật mới cho đơn hàng #" + id + " của bạn. CLick vào để xem", 0,
                                //    currentDate, obj_user.Username);
                                OrderCommentController.Insert(id, "Đã có cập nhật mới cho đơn hàng #" + id + " của bạn.", true, 1, DateTime.Now, uid);

                                try
                                {
                                    //PJUtils.SendMailGmail("cskh@1688pgs.vn", "1688pegasus", AccountInfoController.GetByUserID(Convert.ToInt32(o.UID)).Email, 
                                    //    "Thông báo tại 1688PGS.", 
                                    //    "Đã có cập nhật trạng thái cho đơn hàng #" + id + " của bạn. CLick vào để xem", "");
                                    PJUtils.SendMailGmail("no-reply@mona-media.com", "demonhunter",
                                        AccountInfoController.GetByUserID(Convert.ToInt32(o.UID)).Email,
                                                "Thông báo tại Taobao.vn",
                                                "Đã có cập nhật trạng thái cho đơn hàng #" + id
                                                + " của bạn. CLick vào để xem", "");
                                }
                                catch { }
                            }
                        }
                        #region Ghi lịch sử update status của đơn hàng
                        if (status != currentstt)
                        {
                            HistoryOrderChangeController.Insert(o.ID, obj_user.ID, obj_user.Username, obj_user.Username +
                                       " đã đổi trạng thái của đơn hàng ID là: " + o.ID + ", từ: " + orderstatus + ", sang: " + ddlStatus.SelectedItem + "", 0, currentDate);
                        }
                        #endregion
                        //}
                        string OrderWeight = txtOrderWeight.Text.ToString();
                        OrderWeight = Math.Round(Convert.ToDouble(OrderWeight), 1).ToString();
                        if (string.IsNullOrEmpty(CurrentOrderWeight))
                        {
                            if (CurrentOrderWeight != OrderWeight)
                            {
                                //NotificationController.Inser(obj_user.ID, obj_user.Username, Convert.ToInt32(o.UID),
                                //    AccountController.GetByID(Convert.ToInt32(o.UID)).Username, id, "Đã có cập nhật mới về cân nặng cho đơn hàng #" + id + " của bạn. CLick vào để xem", 0,
                                //    currentDate, obj_user.Username);


                                //try
                                //{
                                //    PJUtils.SendMailGmail("no-reply@mona-media.com", "demonhunter", AccountInfoController.GetByUserID(Convert.ToInt32(o.UID)).Email,
                                //        "Thông báo tại 1688 Express.", "Đã có cập nhật mới về cân nặng cho đơn hàng #" + id + " của bạn. CLick vào để xem", "");
                                //}
                                //catch { }
                                HistoryOrderChangeController.Insert(o.ID, obj_user.ID, obj_user.Username, obj_user.Username +
                                           " đã đổi cân nặng của đơn hàng ID là: " + o.ID + ", là: " + OrderWeight + "", 8, currentDate);
                            }
                        }
                        else
                        {
                            if (CurrentOrderWeight != OrderWeight)
                            {
                                //NotificationController.Inser(obj_user.ID, obj_user.Username, Convert.ToInt32(o.UID),
                                //    AccountController.GetByID(Convert.ToInt32(o.UID)).Username, id, "Đã có cập nhật mới về cân nặng cho đơn hàng #" + id + " của bạn. CLick vào để xem", 0,
                                //    currentDate, obj_user.Username);


                                //try
                                //{
                                //    PJUtils.SendMailGmail("no-reply@mona-media.com", "demonhunter", AccountInfoController.GetByUserID(Convert.ToInt32(o.UID)).Email,
                                //        "Thông báo tại 1688 Express.", "Đã có cập nhật mới về cân nặng cho đơn hàng #" + id + " của bạn. CLick vào để xem", "");
                                //}
                                //catch { }
                                HistoryOrderChangeController.Insert(o.ID, obj_user.ID, obj_user.Username, obj_user.Username +
                                           " đã đổi cân nặng của đơn hàng ID là: " + o.ID + ", từ: " + CurrentOrderWeight + ", sang: " + OrderWeight + "",
                                           9, currentDate);
                            }
                        }
                        if (status == 5 && status != currentstt)
                        {
                            var setNoti = SendNotiEmailController.GetByID(7);
                            if (setNoti != null)
                            {
                                if (setNoti.IsSentNotiUser == true)
                                {
                                    if (o.OrderType == 1)
                                    {
                                        NotificationsController.Inser(accmuahan.ID,
                                          accmuahan.Username, o.ID,
                                          "Đơn hàng " + o.ID + " đã được mua hàng.", 1,
                                          currentDate, obj_user.Username, true);
                                    }
                                    else
                                    {
                                        NotificationsController.Inser(accmuahan.ID,
                                          accmuahan.Username, o.ID,
                                          "Đơn hàng " + o.ID + " đã được mua hàng.", 11,
                                          currentDate, obj_user.Username, true);
                                    }

                                }

                                if (setNoti.IsSendEmailUser == true)
                                {
                                    try
                                    {
                                        PJUtils.SendMailGmail("no-reply@mona-media.com", "demonhunter",
                                            accmuahan.Email,
                                            "Thông báo tại Taobao.vn.", "Đơn hàng " + o.ID
                                            + " đã được mua hàng.", "");
                                    }
                                    catch { }
                                }
                            }
                        }
                        //string OrderTransactionCode = txtOrdertransactionCode.Text;
                        //if (string.IsNullOrEmpty(CurrentOrderTransactionCode))
                        //{
                        //    if (CurrentOrderTransactionCode != OrderTransactionCode)
                        //    {
                        //        HistoryOrderChangeController.Insert(o.ID, obj_user.ID, obj_user.Username, obj_user.Username +
                        //                   " đã đổi mã vận đơn của đơn hàng ID là: " + o.ID + ", là: " + OrderTransactionCode + "", 8, currentDate);
                        //    }
                        //}
                        //else
                        //{
                        //    if (CurrentOrderTransactionCode != OrderTransactionCode)
                        //    {
                        //        HistoryOrderChangeController.Insert(o.ID, obj_user.ID, obj_user.Username, obj_user.Username +
                        //                   " đã đổi mã vận đơn của đơn hàng ID là: " + o.ID + ", từ: " + CurrentOrderTransactionCode + ", sang: " + OrderTransactionCode + "",
                        //                   8, currentDate);
                        //    }
                        //}
                        //string OrderTransactionCode2 = txtOrdertransactionCode2.Text;
                        //if (string.IsNullOrEmpty(OrderTransactionCode2))
                        //{
                        //    if (CurrentOrderTransactionCode2 != OrderTransactionCode2)
                        //    {
                        //        HistoryOrderChangeController.Insert(o.ID, obj_user.ID, obj_user.Username, obj_user.Username +
                        //                   " đã đổi mã vận đơn 2 của đơn hàng ID là: " + o.ID + ", là: " + OrderTransactionCode2 + "", 8, currentDate);
                        //    }
                        //}
                        //else
                        //{
                        //    if (CurrentOrderTransactionCode2 != OrderTransactionCode2)
                        //    {
                        //        HistoryOrderChangeController.Insert(o.ID, obj_user.ID, obj_user.Username, obj_user.Username +
                        //                   " đã đổi mã vận đơn 2 của đơn hàng ID là: " + o.ID + ", từ: " + CurrentOrderTransactionCode2 + ", sang: " + OrderTransactionCode2 + "",
                        //                   8, currentDate);
                        //    }
                        //}
                        //if (!string.IsNullOrEmpty(OrderTransactionCode2))
                        //{
                        //    if (CurrentOrderTransactionCode2 != OrderTransactionCode2)
                        //    {
                        //        HistoryOrderChangeController.Insert(o.ID, obj_user.ID, obj_user.Username, obj_user.Username +
                        //                   " đã đổi mã vận đơn 2 của đơn hàng ID là: " + o.ID + ", từ: " + CurrentOrderTransactionCode2 + ", sang: " + OrderTransactionCode2 + "",
                        //                   8, currentDate);
                        //    }
                        //}
                        //string OrderTransactionCode3 = txtOrdertransactionCode3.Text;
                        //if (string.IsNullOrEmpty(OrderTransactionCode3))
                        //{
                        //    if (CurrentOrderTransactionCode3 != OrderTransactionCode3)
                        //    {
                        //        HistoryOrderChangeController.Insert(o.ID, obj_user.ID, obj_user.Username, obj_user.Username +
                        //                   " đã đổi mã vận đơn 3 của đơn hàng ID là: " + o.ID + ", là: " + OrderTransactionCode3 + "", 8, currentDate);
                        //    }
                        //}
                        //else
                        //{
                        //    if (CurrentOrderTransactionCode3 != OrderTransactionCode3)
                        //    {
                        //        HistoryOrderChangeController.Insert(o.ID, obj_user.ID, obj_user.Username, obj_user.Username +
                        //                   " đã đổi mã vận đơn 3 của đơn hàng ID là: " + o.ID + ", từ: " + CurrentOrderTransactionCode3 + ", sang: " + OrderTransactionCode3 + "",
                        //                   8, currentDate);
                        //    }
                        //}
                        //if (!string.IsNullOrEmpty(OrderTransactionCode3))
                        //{
                        //    if (CurrentOrderTransactionCode3 != OrderTransactionCode3)
                        //    {
                        //        HistoryOrderChangeController.Insert(o.ID, obj_user.ID, obj_user.Username, obj_user.Username +
                        //                   " đã đổi mã vận đơn 3 của đơn hàng ID là: " + o.ID + ", từ: " + CurrentOrderTransactionCode3 + ", sang: " + OrderTransactionCode3 + "",
                        //                   8, currentDate);
                        //    }
                        //}
                        //string OrderTransactionCode4 = txtOrdertransactionCode4.Text;
                        //if (string.IsNullOrEmpty(OrderTransactionCode4))
                        //{
                        //    if (CurrentOrderTransactionCode4 != OrderTransactionCode4)
                        //    {
                        //        HistoryOrderChangeController.Insert(o.ID, obj_user.ID, obj_user.Username, obj_user.Username +
                        //                   " đã đổi mã vận đơn 4 của đơn hàng ID là: " + o.ID + ", là: " + OrderTransactionCode4 + "", 8, currentDate);
                        //    }
                        //}
                        //else
                        //{
                        //    if (CurrentOrderTransactionCode4 != OrderTransactionCode4)
                        //    {                                
                        //        HistoryOrderChangeController.Insert(o.ID, obj_user.ID, obj_user.Username, obj_user.Username +
                        //                   " đã đổi mã vận đơn 4 của đơn hàng ID là: " + o.ID + ", từ: " + CurrentOrderTransactionCode4 + ", sang: " + OrderTransactionCode4 + "",
                        //                   8, currentDate);                                
                        //    }
                        //}
                        //if (!string.IsNullOrEmpty(OrderTransactionCode4))
                        //{
                        //    if (CurrentOrderTransactionCode4 != OrderTransactionCode4)
                        //    {
                        //        HistoryOrderChangeController.Insert(o.ID, obj_user.ID, obj_user.Username, obj_user.Username +
                        //                   " đã đổi mã vận đơn 4 của đơn hàng ID là: " + o.ID + ", từ: " + CurrentOrderTransactionCode4 + ", sang: " + OrderTransactionCode4 + "",
                        //                   8, currentDate);
                        //    }
                        //}
                        //string OrderTransactionCode5 = txtOrdertransactionCode5.Text;
                        //if (!string.IsNullOrEmpty(OrderTransactionCode5))
                        //{
                        //    if (CurrentOrderTransactionCode5 != OrderTransactionCode5)
                        //    {
                        //        HistoryOrderChangeController.Insert(o.ID, obj_user.ID, obj_user.Username, obj_user.Username +
                        //                   " đã đổi mã vận đơn 5 của đơn hàng ID là: " + o.ID + ", từ: " + CurrentOrderTransactionCode5 + ", sang: " + OrderTransactionCode5 + "",
                        //                   8, currentDate);
                        //    }
                        //}

                        string CurrentReceivePlace = o.ReceivePlace;
                        string ReceivePlace = ddlReceivePlace.SelectedValue;

                        if (CurrentReceivePlace != ReceivePlace)
                        {
                            HistoryOrderChangeController.Insert(o.ID, obj_user.ID, obj_user.Username, obj_user.Username +
                                       " đã đổi nơi nhận hàng của đơn hàng ID là: " + o.ID + ", từ: " + CurrentReceivePlace + ", sang: " + ReceivePlace + "",
                                       8, currentDate);
                        }

                        string CurrentAmountDeposit = o.AmountDeposit.Trim();
                        CurrentAmountDeposit = Math.Round(Convert.ToDouble(CurrentAmountDeposit), 0).ToString();
                        string AmountDeposit = pAmountDeposit.Text.ToString().Trim();
                        AmountDeposit = Math.Round(Convert.ToDouble(AmountDeposit), 0).ToString();

                        if (CurrentAmountDeposit != AmountDeposit)
                        {
                            HistoryOrderChangeController.Insert(o.ID, obj_user.ID, obj_user.Username, obj_user.Username +
                                       " đã đổi số tiền phải cọc của đơn hàng ID là: " + o.ID + ", từ: "
                                       + CurrentAmountDeposit + ", sang: " + AmountDeposit + "",
                                       8, currentDate);
                        }

                        //bool Currentcheckpro = o.IsCheckProduct.ToString().ToBool();
                        //bool checkpro = Convert.ToBoolean(chkCheck.Value.ToString());
                        //bool Package = Convert.ToBoolean(chkPackage.Value.ToString());
                        //bool MoveIsFastDelivery = Convert.ToBoolean(chkShiphome.Value.ToString());
                        //bool baogia = Convert.ToBoolean(chkIsCheckPrice.Value.ToString());
                        //bool smallPackage = Convert.ToBoolean(chkIsDoneSmallPackage.Value.ToString());
                        bool Currentcheckpro = new bool();
                        bool checkpro = new bool();
                        bool Package = new bool();
                        bool MoveIsFastDelivery = new bool();
                        bool baogia = new bool();
                        bool smallPackage = new bool();
                        bool ycg = new bool();
                        bool baohiem = new bool();

                        var listCheck = hdfListCheckBox.Value.Split('|').ToList();
                        foreach (var item in listCheck)
                        {
                            if (!string.IsNullOrEmpty(item))
                            {
                                var ck = item.Split(',').ToList();

                                if (ck != null)
                                {
                                    if (ck[0] == "1")
                                    {
                                        smallPackage = Convert.ToBoolean(ck[1].ToInt(0));
                                    }
                                    if (ck[0] == "2")
                                    {
                                        baogia = Convert.ToBoolean(ck[1].ToInt(0));
                                    }
                                    if (ck[0] == "3")
                                    {
                                        checkpro = Convert.ToBoolean(ck[1].ToInt(0));
                                    }
                                    if (ck[0] == "4")
                                    {
                                        Package = Convert.ToBoolean(ck[1].ToInt(0));
                                    }
                                    if (ck[0] == "5")
                                    {
                                        MoveIsFastDelivery = Convert.ToBoolean(ck[1].ToInt(0));
                                    }
                                    if (ck[0] == "6")
                                    {
                                        ycg = Convert.ToBoolean(ck[1].ToInt(0));
                                    }
                                    if (ck[0] == "7")
                                    {
                                        baohiem = Convert.ToBoolean(ck[1].ToInt(0));
                                    }
                                }
                            }
                        }

                        if (Currentcheckpro != checkpro)
                        {
                            HistoryOrderChangeController.Insert(o.ID, obj_user.ID, obj_user.Username, obj_user.Username +
                                       " đã đổi dịch vụ kiểm tra đơn hàng của đơn hàng ID là: " + o.ID + ", từ: " + Currentcheckpro + ", sang: " + checkpro + "",
                                       8, currentDate);
                        }
                        bool CurrentPackage = o.IsPacked.ToString().ToBool();

                        if (CurrentPackage != Package)
                        {
                            HistoryOrderChangeController.Insert(o.ID, obj_user.ID, obj_user.Username, obj_user.Username +
                                       " đã đổi dịch vụ đóng gỗ của đơn hàng ID là: " + o.ID + ", từ: " + CurrentPackage + ", sang: " + Package + "",
                                       8, currentDate);
                        }
                        bool CurrentIsFastDelivery = o.IsFastDelivery.ToString().ToBool();

                        string TotalPriceReal1 = rTotalPriceReal.Text.ToString();
                        TotalPriceReal1 = Math.Round(Convert.ToDouble(TotalPriceReal1), 0).ToString();
                        string TotalPriceRealCYN1 = rTotalPriceRealCYN.Text.ToString();
                        TotalPriceRealCYN1 = Math.Round(Convert.ToDouble(TotalPriceRealCYN1), 2).ToString();
                        if (CurrentIsFastDelivery != MoveIsFastDelivery)
                        {
                            HistoryOrderChangeController.Insert(o.ID, obj_user.ID, obj_user.Username, obj_user.Username +
                                       " đã đổi dịch vụ giao hàng tận nhà của đơn hàng ID là: " + o.ID + ", từ: " + CurrentIsFastDelivery + ", sang: " + MoveIsFastDelivery + "",
                                       8, currentDate);
                        }

                        if (status == 2)
                        {
                            if (o.DepostiDate == null)
                            {
                                MainOrderController.UpdateDepositDate(o.ID, currentDate);
                            }
                        }
                        else if (status == 5)
                        {
                            if (o.DateBuy == null)
                            {
                                MainOrderController.UpdateDateBuy(o.ID, currentDate);
                            }
                        }
                        else if (status == 6)
                        {
                            if (o.DateTQ == null)
                            {
                                MainOrderController.UpdateDateTQ(o.ID, currentDate);
                            }
                        }
                        else if (status == 7)
                        {
                            if (o.DateVN == null)
                            {
                                MainOrderController.UpdateDateVN(o.ID, currentDate);
                            }
                        }
                        else if (status == 9)
                        {
                            if (o.PayDate == null)
                            {
                                MainOrderController.UpdatePayDate(o.ID, currentDate);
                            }
                        }
                        else if (status == 10)
                        {
                            if (o.CompleteDate == null)
                            {
                                MainOrderController.UpdateCompleteDate(o.ID, currentDate);
                            }
                        }

                        MainOrderController.UpdateTotalFeeSupport(o.ID, TotalFeeSupport.ToString());
                        MainOrderController.UpdateOrderWeight(o.ID, OrderWeight);
                        MainOrderController.UpdateCheckPro(o.ID, checkpro);
                        MainOrderController.UpdateBaogia(o.ID, baogia);
                        MainOrderController.UpdateIsGiaohang(o.ID, ycg);
                        MainOrderController.UpdateIsPacked(o.ID, Package);
                        MainOrderController.UpdateIsFastDelivery(o.ID, MoveIsFastDelivery);
                        MainOrderController.UpdateAmountDeposit(o.ID, AmountDeposit);
                        MainOrderController.UpdateFeeWarehouse(o.ID, feeeinwarehouse);
                        //MainOrderController.UpdateReceivePlace(o.ID, Convert.ToInt32(o.UID), ddlReceivePlace.SelectedValue.ToString());
                        double FeeweightPriceDiscount = 0;
                        if (!string.IsNullOrEmpty(hdfFeeweightPriceDiscount.Value))
                        {
                            FeeweightPriceDiscount = Math.Round(Convert.ToDouble(hdfFeeweightPriceDiscount.Value));
                        }
                        MainOrderController.UpdateFeeWeightDC(o.ID, FeeweightPriceDiscount.ToString());
                        MainOrderController.UpdateStatusByID(o.ID, Convert.ToInt32(ddlStatus.SelectedValue));
                        MainOrderController.UpdateOrderWeightCK(o.ID, FeeweightPriceDiscount.ToString());
                        MainOrderController.UpdateTQVNWeight(o.ID, o.UID.ToString().ToInt(), Math.Round(Convert.ToDouble(pWeightNDT.Text.ToString()), 2).ToString());
                        MainOrderController.UpdateTotalPriceReal(o.ID, TotalPriceReal1.ToString(), TotalPriceRealCYN1.ToString());
                        MainOrderController.UpdateMainOrderCode(o.ID, o.UID.ToString().ToInt(), txtMainOrderCode.Text);
                        MainOrderController.UpdateFTS(o.ID, o.UID.ToString().ToInt(), ddlWarehouseFrom.SelectedValue.ToInt(),
                            ddlReceivePlace.SelectedValue, ddlShippingType.SelectedValue.ToInt());

                        MainOrderController.UpdateDoneSmallPackage(o.ID, smallPackage);

                        MainOrderController.UpdateIsInsurrance(o.ID, baohiem);

                        if (baohiem == false)
                        {
                            MainOrderController.UpdateInsurranceMoney(o.ID, "0", o.InsurancePercent);
                        }
                        else
                        {
                            double InsurranceMoney = Convert.ToDouble(o.PriceVND) * (Convert.ToDouble(o.InsurancePercent) / 100);
                            MainOrderController.UpdateInsurranceMoney(o.ID, InsurranceMoney.ToString(), o.InsurancePercent);
                        }

                        #endregion
                        #region Update User Level
                        if (status >= 9)
                        {
                            //int cusID = o.UID.ToString().ToInt(0);
                            //var cust = AccountController.GetByID(cusID);
                            //if (cust != null)
                            //{
                            //    var cus_orders = MainOrderController.GetSuccessByCustomer(cust.ID);
                            //    double totalpay = 0;
                            //    if (cus_orders.Count > 0)
                            //    {
                            //        foreach (var item in cus_orders)
                            //        {
                            //            double ttpricenvd = 0;
                            //            if (item.TotalPriceVND.ToFloat(0) > 0)
                            //                ttpricenvd = Convert.ToDouble(item.TotalPriceVND);
                            //            totalpay += ttpricenvd;
                            //        }

                            //        if (totalpay >= 100000000 && totalpay < 300000000)
                            //        {
                            //            if (cust.LevelID == 1)
                            //            {
                            //                AccountController.updateLevelID(cusID, 2, currentDate, cust.Username);
                            //            }
                            //        }
                            //        else if (totalpay >= 300000000 && totalpay < 800000000)
                            //        {
                            //            if (cust.LevelID == 2)
                            //            {
                            //                AccountController.updateLevelID(cusID, 3, currentDate, cust.Username);
                            //            }
                            //        }
                            //        else if (totalpay >= 800000000 && totalpay < 1500000000)
                            //        {
                            //            if (cust.LevelID == 3)
                            //            {
                            //                AccountController.updateLevelID(cusID, 4, currentDate, cust.Username);
                            //            }
                            //        }
                            //        else if (totalpay >= 1500000000 && totalpay < 2500000000)
                            //        {
                            //            if (cust.LevelID == 4)
                            //            {
                            //                AccountController.updateLevelID(cusID, 5, currentDate, cust.Username);
                            //            }
                            //        }
                            //        else if (totalpay >= 2500000000 && totalpay < 5000000000)
                            //        {
                            //            if (cust.LevelID == 5)
                            //            {
                            //                AccountController.updateLevelID(cusID, 11, currentDate, cust.Username);
                            //            }
                            //        }
                            //        else if (totalpay >= 5000000000 && totalpay < 10000000000)
                            //        {
                            //            if (cust.LevelID == 11)
                            //            {
                            //                AccountController.updateLevelID(cusID, 12, currentDate, cust.Username);
                            //            }
                            //        }
                            //        else if (totalpay >= 10000000000 && totalpay < 20000000000)
                            //        {
                            //            if (cust.LevelID == 12)
                            //            {
                            //                AccountController.updateLevelID(cusID, 13, currentDate, cust.Username);
                            //            }
                            //        }
                            //        else if (totalpay >= 20000000000)
                            //        {
                            //            if (cust.LevelID == 13)
                            //            {
                            //                AccountController.updateLevelID(cusID, 14, currentDate, cust.Username);
                            //            }
                            //        }
                            //    }
                            //}
                        }
                        #endregion
                        #region Cập nhật thông tin nhân viên sale và đặt hàng
                        int SalerID = ddlSaler.SelectedValue.ToString().ToInt(0);
                        int DathangID = ddlDatHang.SelectedValue.ToString().ToInt(0);
                        int KhoTQID = ddlKhoTQ.SelectedValue.ToString().ToInt(0);
                        int khoVNID = ddlKhoVN.SelectedValue.ToString().ToInt(0);
                        var mo = MainOrderController.GetAllByID(id);
                        if (mo != null)
                        {
                            double feebp = Math.Round(Convert.ToDouble(mo.FeeBuyPro), 0);
                            DateTime CreatedDate = Convert.ToDateTime(mo.CreatedDate);
                            double salepercent = 0;
                            double salepercentaf3m = 0;
                            double dathangpercent = 0;
                            var config = ConfigurationController.GetByTop1();
                            if (config != null)
                            {
                                salepercent = Convert.ToDouble(config.SalePercent);
                                salepercentaf3m = Convert.ToDouble(config.SalePercentAfter3Month);
                                dathangpercent = Convert.ToDouble(config.DathangPercent);
                            }
                            string salerName = "";
                            string dathangName = "";

                            int salerID_old = Convert.ToInt32(mo.SalerID);
                            int dathangID_old = Convert.ToInt32(mo.DathangID);

                            #region Saler
                            if (SalerID > 0)
                            {
                                if (SalerID == salerID_old)
                                {
                                    var staff = StaffIncomeController.GetByMainOrderIDUID(id, salerID_old);
                                    if (staff != null)
                                    {
                                        int rStaffID = staff.ID;
                                        int staffstatus = Convert.ToInt32(staff.Status);
                                        if (staffstatus == 1)
                                        {
                                            var sale = AccountController.GetByID(salerID_old);
                                            if (sale != null)
                                            {
                                                salerName = sale.Username;
                                                var createdDate = Convert.ToDateTime(sale.CreatedDate);
                                                int d = CreatedDate.Subtract(createdDate).Days;
                                                if (d > 90)
                                                {
                                                    salepercentaf3m = Convert.ToDouble(staff.PercentReceive);
                                                    double per = Math.Round(feebp * salepercentaf3m / 100, 0);
                                                    StaffIncomeController.Update(rStaffID, mo.TotalPriceVND, salepercentaf3m.ToString(), 1,
                                                        per.ToString(), false, currentDate, username);
                                                }
                                                else
                                                {
                                                    salepercent = Convert.ToDouble(staff.PercentReceive);
                                                    double per = Math.Round(feebp * salepercent / 100, 0);
                                                    StaffIncomeController.Update(rStaffID, mo.TotalPriceVND, salepercent.ToString(), 1,
                                                        per.ToString(), false, currentDate, username);
                                                }
                                            }
                                        }
                                    }
                                }
                                else
                                {
                                    var staff = StaffIncomeController.GetByMainOrderIDUID(id, salerID_old);
                                    if (staff != null)
                                    {
                                        StaffIncomeController.Delete(staff.ID);
                                    }
                                    var sale = AccountController.GetByID(SalerID);
                                    if (sale != null)
                                    {
                                        salerName = sale.Username;
                                        var createdDate = Convert.ToDateTime(sale.CreatedDate);
                                        int d = CreatedDate.Subtract(createdDate).Days;
                                        if (d > 90)
                                        {
                                            double per = Math.Round(feebp * salepercentaf3m / 100, 0);
                                            StaffIncomeController.Insert(id, per.ToString(), salepercent.ToString(), SalerID, salerName, 6, 1, per.ToString(), false,
                                            CreatedDate, currentDate, username);
                                        }
                                        else
                                        {
                                            double per = Math.Round(feebp * salepercent / 100, 0);
                                            StaffIncomeController.Insert(id, per.ToString(), salepercent.ToString(), SalerID, salerName, 6, 1, per.ToString(), false,
                                            CreatedDate, currentDate, username);
                                        }
                                    }
                                }
                            }
                            #endregion
                            #region Đặt hàng
                            if (DathangID > 0)
                            {
                                if (DathangID == dathangID_old)
                                {
                                    var staff = StaffIncomeController.GetByMainOrderIDUID(id, dathangID_old);
                                    if (staff != null)
                                    {
                                        if (staff.Status == 1)
                                        {
                                            //double totalPrice = Convert.ToDouble(mo.TotalPriceVND);
                                            double totalPrice = Convert.ToDouble(mo.PriceVND) + Convert.ToDouble(mo.FeeShipCN);
                                            totalPrice = Math.Round(totalPrice, 0);
                                            double totalRealPrice = 0;
                                            if (mo.TotalPriceReal.ToFloat(0) > 0)
                                                totalRealPrice = Math.Round(Convert.ToDouble(mo.TotalPriceReal), 0);
                                            if (totalRealPrice > 0)
                                            {
                                                double totalpriceloi = totalPrice - totalRealPrice;
                                                dathangpercent = Convert.ToDouble(staff.PercentReceive);
                                                double income = Math.Round(totalpriceloi * dathangpercent / 100, 0);
                                                //double income = totalpriceloi;
                                                StaffIncomeController.Update(staff.ID, totalRealPrice.ToString(), dathangpercent.ToString(), 1,
                                                            income.ToString(), false, currentDate, username);
                                            }

                                        }
                                    }
                                }
                                else
                                {
                                    var staff = StaffIncomeController.GetByMainOrderIDUID(id, dathangID_old);
                                    if (staff != null)
                                    {
                                        StaffIncomeController.Delete(staff.ID);
                                    }
                                    var dathang = AccountController.GetByID(DathangID);
                                    if (dathang != null)
                                    {
                                        dathangName = dathang.Username;
                                        //double totalPrice = Convert.ToDouble(mo.TotalPriceVND);
                                        double totalPrice = Convert.ToDouble(mo.PriceVND) + Convert.ToDouble(mo.FeeShipCN);
                                        totalPrice = Math.Round(totalPrice, 0);
                                        double totalRealPrice = 0;
                                        if (mo.TotalPriceReal.ToFloat(0) > 0)
                                            totalRealPrice = Math.Round(Convert.ToDouble(mo.TotalPriceReal), 0);
                                        if (totalRealPrice > 0)
                                        {
                                            double totalpriceloi = totalPrice - totalRealPrice;
                                            double income = Math.Round(totalpriceloi * dathangpercent / 100, 0);
                                            //double income = totalpriceloi;

                                            StaffIncomeController.Insert(id, totalpriceloi.ToString(), dathangpercent.ToString(), DathangID, dathangName, 3, 1,
                                                income.ToString(), false, CreatedDate, currentDate, username);
                                        }
                                        else
                                        {
                                            StaffIncomeController.Insert(id, "0", dathangpercent.ToString(), DathangID, dathangName, 3, 1, "0", false,
                                            CreatedDate, currentDate, username);
                                        }
                                    }
                                }
                            }
                            #endregion
                        }

                        MainOrderController.UpdateStaff(id, SalerID, DathangID, KhoTQID, khoVNID);
                        #endregion
                        #region Cập nhật ngày đặt hàng
                        //int statusOOld = Convert.ToInt32(o.Status);
                        //int statusONew = Convert.ToInt32(ddlStatus.SelectedValue);
                        //if (statusONew != statusOOld)
                        //{
                        //    StatusChangeHistoryController.Insert(o.ID, statusOOld, statusONew, currentDate, username);
                        //}

                        #endregion
                        PJUtils.ShowMsg("Cập nhật thông tin thành công.", true, Page);
                    }
                }
            }
        }
        protected void btnStaffUpdate_Click(object sender, EventArgs e)
        {
            if (!Page.IsValid) return;
            int SalerID = ddlSaler.SelectedValue.ToString().ToInt(0);
            int DathangID = ddlDatHang.SelectedValue.ToString().ToInt(0);
            int KhoTQID = ddlKhoTQ.SelectedValue.ToString().ToInt(0);
            int khoVNID = ddlKhoVN.SelectedValue.ToString().ToInt(0);
            int ID = ViewState["MOID"].ToString().ToInt();
            string username = Session["userLoginSystem"].ToString();
            var obj_user = AccountController.GetByUsername(username);
            DateTime currentDate = DateTime.Now;
            var mo = MainOrderController.GetAllByID(ID);
            if (mo != null)
            {
                double feebp = Convert.ToDouble(mo.FeeBuyPro);
                DateTime CreatedDate = Convert.ToDateTime(mo.CreatedDate);
                double salepercent = 0;
                double salepercentaf3m = 0;
                double dathangpercent = 0;
                var config = ConfigurationController.GetByTop1();
                if (config != null)
                {
                    salepercent = Convert.ToDouble(config.SalePercent);
                    salepercentaf3m = Convert.ToDouble(config.SalePercentAfter3Month);
                    dathangpercent = Convert.ToDouble(config.DathangPercent);
                }
                string salerName = "";
                string dathangName = "";

                int salerID_old = Convert.ToInt32(mo.SalerID);
                int dathangID_old = Convert.ToInt32(mo.DathangID);

                #region Saler
                if (SalerID > 0)
                {
                    if (SalerID == salerID_old)
                    {
                        var staff = StaffIncomeController.GetByMainOrderIDUID(ID, salerID_old);
                        if (staff != null)
                        {
                            int rStaffID = staff.ID;
                            int status = Convert.ToInt32(staff.Status);
                            if (status == 1)
                            {
                                var sale = AccountController.GetByID(salerID_old);
                                if (sale != null)
                                {
                                    salerName = sale.Username;
                                    var createdDate = Convert.ToDateTime(sale.CreatedDate);
                                    int d = CreatedDate.Subtract(createdDate).Days;
                                    if (d > 90)
                                    {
                                        salepercentaf3m = Convert.ToDouble(staff.PercentReceive);
                                        double per = Math.Round(feebp * salepercentaf3m / 100, 0);
                                        StaffIncomeController.Update(rStaffID, mo.TotalPriceVND, salepercentaf3m.ToString(), 1,
                                            per.ToString(), false, currentDate, username);
                                    }
                                    else
                                    {
                                        salepercent = Convert.ToDouble(staff.PercentReceive);
                                        double per = Math.Round(feebp * salepercent / 100, 0);
                                        StaffIncomeController.Update(rStaffID, mo.TotalPriceVND, salepercent.ToString(), 1,
                                            per.ToString(), false, currentDate, username);
                                    }
                                }
                            }
                        }
                        else
                        {
                            var sale = AccountController.GetByID(SalerID);
                            if (sale != null)
                            {
                                salerName = sale.Username;
                                var createdDate = Convert.ToDateTime(sale.CreatedDate);
                                int d = CreatedDate.Subtract(createdDate).Days;
                                if (d > 90)
                                {
                                    double per = Math.Round(feebp * salepercentaf3m / 100, 0);
                                    StaffIncomeController.Insert(ID, per.ToString(), salepercent.ToString(), SalerID, salerName, 6, 1, per.ToString(), false,
                                    CreatedDate, currentDate, username);
                                }
                                else
                                {
                                    double per = Math.Round(feebp * salepercent / 100, 0);
                                    StaffIncomeController.Insert(ID, per.ToString(), salepercent.ToString(), SalerID, salerName, 6, 1, per.ToString(), false,
                                    CreatedDate, currentDate, username);
                                }
                            }
                        }
                    }
                    else
                    {
                        var staff = StaffIncomeController.GetByMainOrderIDUID(ID, salerID_old);
                        if (staff != null)
                        {
                            StaffIncomeController.Delete(staff.ID);
                        }
                        var sale = AccountController.GetByID(SalerID);
                        if (sale != null)
                        {
                            salerName = sale.Username;
                            var createdDate = Convert.ToDateTime(sale.CreatedDate);
                            int d = CreatedDate.Subtract(createdDate).Days;
                            if (d > 90)
                            {
                                double per = Math.Round(feebp * salepercentaf3m / 100, 0);
                                StaffIncomeController.Insert(ID, per.ToString(), salepercent.ToString(), SalerID, salerName, 6, 1, per.ToString(), false,
                                CreatedDate, currentDate, username);
                            }
                            else
                            {
                                double per = Math.Round(feebp * salepercent / 100, 0);
                                StaffIncomeController.Insert(ID, per.ToString(), salepercent.ToString(), SalerID, salerName, 6, 1, per.ToString(), false,
                                CreatedDate, currentDate, username);
                            }
                        }
                    }
                }
                #endregion
                #region Đặt hàng
                if (DathangID > 0)
                {
                    if (DathangID == dathangID_old)
                    {
                        var staff = StaffIncomeController.GetByMainOrderIDUID(ID, dathangID_old);
                        if (staff != null)
                        {
                            if (staff.Status == 1)
                            {
                                //double totalPrice = Convert.ToDouble(mo.TotalPriceVND);
                                double totalPrice = Convert.ToDouble(mo.PriceVND) + Convert.ToDouble(mo.FeeShipCN);
                                totalPrice = Math.Round(totalPrice, 0);
                                double totalRealPrice = 0;
                                if (!string.IsNullOrEmpty(mo.TotalPriceReal))
                                    totalRealPrice = Math.Round(Convert.ToDouble(mo.TotalPriceReal), 0);
                                if (totalRealPrice > 0)
                                {
                                    double totalpriceloi = totalPrice - totalRealPrice;
                                    totalpriceloi = Math.Round(totalpriceloi, 0);
                                    dathangpercent = Convert.ToDouble(staff.PercentReceive);
                                    double income = Math.Round(totalpriceloi * dathangpercent / 100, 0);
                                    //double income = totalpriceloi;
                                    StaffIncomeController.Update(staff.ID, totalRealPrice.ToString(), dathangpercent.ToString(), 1,
                                                income.ToString(), false, currentDate, username);
                                }

                            }
                        }
                        else
                        {
                            var dathang = AccountController.GetByID(DathangID);
                            if (dathang != null)
                            {
                                dathangName = dathang.Username;
                                //double totalPrice = Convert.ToDouble(mo.TotalPriceVND);
                                double totalPrice = Convert.ToDouble(mo.PriceVND) + Convert.ToDouble(mo.FeeShipCN);
                                totalPrice = Math.Round(totalPrice, 0);
                                double totalRealPrice = 0;
                                if (!string.IsNullOrEmpty(mo.TotalPriceReal))
                                    totalRealPrice = Math.Round(Convert.ToDouble(mo.TotalPriceReal), 0);
                                if (totalRealPrice > 0)
                                {
                                    double totalpriceloi = totalPrice - totalRealPrice;
                                    totalpriceloi = Math.Round(totalpriceloi, 0);
                                    double income = Math.Round(totalpriceloi * dathangpercent / 100, 0);
                                    //double income = totalpriceloi;
                                    StaffIncomeController.Insert(ID, totalpriceloi.ToString(), dathangpercent.ToString(), DathangID, dathangName, 3, 1,
                                        income.ToString(), false, CreatedDate, currentDate, username);
                                }
                                else
                                {
                                    StaffIncomeController.Insert(ID, "0", dathangpercent.ToString(), DathangID, dathangName, 3, 1, "0", false,
                                    CreatedDate, currentDate, username);
                                }
                            }
                        }
                    }
                    else
                    {
                        var staff = StaffIncomeController.GetByMainOrderIDUID(ID, dathangID_old);
                        if (staff != null)
                        {
                            StaffIncomeController.Delete(staff.ID);
                        }
                        var dathang = AccountController.GetByID(DathangID);
                        if (dathang != null)
                        {
                            dathangName = dathang.Username;
                            //double totalPrice = Convert.ToDouble(mo.TotalPriceVND);
                            double totalPrice = Convert.ToDouble(mo.PriceVND) + Convert.ToDouble(mo.FeeShipCN);
                            totalPrice = Math.Round(totalPrice, 0);
                            double totalRealPrice = 0;
                            if (!string.IsNullOrEmpty(mo.TotalPriceReal))
                                totalRealPrice = Math.Round(Convert.ToDouble(mo.TotalPriceReal), 0);
                            if (totalRealPrice > 0)
                            {
                                double totalpriceloi = totalPrice - totalRealPrice;
                                double income = Math.Round(totalpriceloi * dathangpercent / 100, 0);
                                //double income = totalpriceloi;

                                StaffIncomeController.Insert(ID, totalpriceloi.ToString(), dathangpercent.ToString(), DathangID, dathangName, 3, 1,
                                    income.ToString(), false, CreatedDate, currentDate, username);
                            }
                            else
                            {
                                StaffIncomeController.Insert(ID, "0", dathangpercent.ToString(), DathangID, dathangName, 3, 1, "0", false,
                                CreatedDate, currentDate, username);
                            }
                        }
                    }
                }
                #endregion
            }

            MainOrderController.UpdateStaff(ID, SalerID, DathangID, KhoTQID, khoVNID);
            PJUtils.ShowMsg("Cập nhật nhân viên thành công.", true, Page);
        }
        protected void btnThanhtoan_Click(object sender, EventArgs e)
        {
            int id = ViewState["MOID"].ToString().ToInt(0);
            //var id = Convert.ToInt32(Request.QueryString["id"]);
            if (id > 0)
            {
                var o = MainOrderController.GetAllByID(id);
                if (o != null)
                {
                    Response.Redirect("/manager/Pay-Order.aspx?id=" + id);
                }
            }
        }
        #endregion
        #region Ajax
        [WebMethod]
        public static string DeleteSmallPackage(string IDPackage)
        {
            if (HttpContext.Current.Session["userLoginSystem"] == null)
            {
                return null;
            }
            else
            {
                int ID = IDPackage.ToInt(0);
                var smallpackage = SmallPackageController.GetByID(ID);
                if (smallpackage != null)
                {
                    string kq = SmallPackageController.Delete(ID);
                    return "ok";
                }
                else
                {
                    return "null";
                }
            }

        }

        [WebMethod]
        public static string DeleteSupportFee(string IDPackage)
        {
            if (HttpContext.Current.Session["userLoginSystem"] == null)
            {
                return null;
            }
            else
            {
                DateTime currentDate = DateTime.Now;
                string username = HttpContext.Current.Session["userLoginSystem"].ToString();
                var obj_user = AccountController.GetByUsername(username);
                if (obj_user != null)
                {
                    int ID = IDPackage.ToInt(0);
                    var supportfee = FeeSupportController.GetByID(ID);
                    if (supportfee != null)
                    {
                        string kq = FeeSupportController.Delete(ID);
                        HistoryOrderChangeController.Insert(supportfee.MainOrderID.Value, obj_user.ID, obj_user.Username, obj_user.Username +
                                         " đã xóa tiền phụ phí của đơn hàng ID là: " + supportfee.MainOrderID + ", Tên phụ phí: " + supportfee.SupportName + ", Số tiền: "
                                         + string.Format("{0:N0}", Convert.ToDouble(supportfee.SupportInfoVND)) + "", 10, currentDate);
                        return "ok";
                    }
                    else
                    {
                        return "null";
                    }
                }
                else
                {
                    return "null";
                }
            }

        }

        [WebMethod]
        public static string DeleteMainOrderCode(int IDCode)
        {

            if (HttpContext.Current.Session["userLoginSystem"] == null)
            {
                return null;
            }
            else
            {
                int ID = IDCode;
                var MainOrderCode = MainOrderCodeController.GetByID(ID);
                if (MainOrderCode != null)
                {
                    string kq = MainOrderCodeController.Delete(ID);
                    return kq;
                }
                else
                {
                    return null;
                }
            }

        }

        [WebMethod]
        public static string UpdateMainOrderCode(int ID, string MainOrderCode, int MainOrderID)
        {
            string username_current = HttpContext.Current.Session["userLoginSystem"].ToString();
            tbl_Account ac = AccountController.GetByUsername(username_current);
            if (ac != null)
            {
                var o = MainOrderController.GetAllByID(MainOrderID);
                if (o != null)
                {
                    var lo = MainOrderCodeController.GetByID(ID);
                    if (!string.IsNullOrEmpty(MainOrderCode))
                    {
                        if (lo == null)
                        {
                            var so = MainOrderCodeController.GetByMainOrderIDANDMainOrderCode(MainOrderID, MainOrderCode);
                            if (so == null)
                            {
                                var kq = MainOrderCodeController.Insert(MainOrderID, MainOrderCode, DateTime.Now, ac.Username);
                                return kq;
                            }
                            return null;
                        }
                        else
                        {
                            var so = MainOrderCodeController.GetByMainOrderIDANDMainOrderCode(MainOrderID, MainOrderCode);
                            if (so == null)
                            {
                                var kq = MainOrderCodeController.UpdateCode(ID, MainOrderCode, DateTime.Now, ac.Username);
                                return kq;
                            }
                            return null;
                        }

                    }
                    return null;
                }
                else
                {
                    return null;
                }
            }
            else
            {
                return null;
            }
        }

        [WebMethod]
        public static string CountFeeWeight(int orderID, int receivePlace, int shippingTypeValue, double weight, int WarehouseFrom)
        {
            var order = MainOrderController.GetAllByID(orderID);
            if (order != null)
            {
                double pricePerKg = 0;
                int fromPlace = WarehouseFrom;
                var warehousefee = WarehouseFeeController.GetByAndWarehouseFromAndToWarehouseAndShippingTypeAndAndHelpMoving(
                    fromPlace, receivePlace, shippingTypeValue, false);
                if (warehousefee.Count > 0)
                {
                    foreach (var w in warehousefee)
                    {
                        if (w.WeightFrom < weight && weight <= w.WeightTo)
                        {
                            pricePerKg = Convert.ToDouble(w.Price);
                        }
                    }
                }
                int UID = Convert.ToInt32(order.UID);
                var usercreate = AccountController.GetByID(UID);
                if (!string.IsNullOrEmpty(usercreate.FeeTQVNPerWeight))
                {
                    double feeweightuser = 0;
                    if (usercreate.FeeTQVNPerWeight.ToFloat(0) > 0)
                    {
                        feeweightuser = Convert.ToDouble(usercreate.FeeTQVNPerWeight);
                    }
                    pricePerKg = feeweightuser;
                }

                double ckFeeWeight = 0;
                if (usercreate != null)
                {
                    ckFeeWeight = Convert.ToDouble(UserLevelController.GetByID(usercreate.LevelID.ToString().ToInt()).FeeWeight.ToString());
                }
                double currency = Convert.ToDouble(order.CurrentCNYVN);
                double totalPriceFeeweightVN = pricePerKg * weight;

                double discountVN = totalPriceFeeweightVN * ckFeeWeight / 100;
                double discountCYN = discountVN / currency;

                double feeoutVN = totalPriceFeeweightVN - discountVN;
                double feeoutCYN = feeoutVN / currency;

                FeeWeightObj f = new FeeWeightObj();
                f.FeeWeightCYN = Math.Floor(feeoutCYN);
                f.FeeWeightVND = Math.Floor(feeoutVN);
                f.DiscountFeeWeightCYN = Math.Floor(discountCYN);
                f.DiscountFeeWeightVN = Math.Floor(discountVN);
                JavaScriptSerializer serializer = new JavaScriptSerializer();
                return serializer.Serialize(f);
            }
            return "none";
        }

        [WebMethod]
        public static string sendcustomercomment(string comment, int id, string urlIMG, string real)
        {
            var listLink = urlIMG.Split('|').ToList();
            if (listLink.Count > 0)
            {
                listLink.RemoveAt(listLink.Count - 1);
            }
            var listComment = real.Split('|').ToList();
            if (listComment.Count > 0)
            {
                listComment.RemoveAt(listComment.Count - 1);
            }
            string username = HttpContext.Current.Session["userLoginSystem"].ToString();
            var obj_user = AccountController.GetByUsername(username);
            DateTime currentDate = DateTime.Now;
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            if (obj_user != null)
            {
                string ret = "";
                var ai = AccountInfoController.GetByUserID(obj_user.ID);
                if (ai != null)
                {
                    ret += ai.FirstName + " " + ai.LastName + "," + ai.IMGUser + "," + string.Format("{0:dd/MM/yyyy HH:mm}", currentDate);
                }
                int uid = obj_user.ID;
                //var id = Convert.ToInt32(Request.QueryString["id"]);
                if (id > 0)
                {
                    var o = MainOrderController.GetAllByID(id);
                    if (o != null)
                    {
                        var setNoti = SendNotiEmailController.GetByID(12);
                        int type = 1;
                        if (type > 0)
                        {
                            for (int i = 0; i < listLink.Count; i++)
                            {
                                string kqq = OrderCommentController.InsertNew(id, listLink[i], listComment[i], true, type, DateTime.Now, uid);
                            }
                            if (!string.IsNullOrEmpty(comment))
                            {
                                string kq = OrderCommentController.Insert(id, comment, true, type, DateTime.Now, uid);
                                if (type == 1)
                                {
                                    if (setNoti != null)
                                    {
                                        if (setNoti.IsSentNotiUser == true)
                                        {
                                            if (o.OrderType == 1)
                                            {
                                                NotificationsController.Inser(Convert.ToInt32(o.UID),
                                       AccountController.GetByID(Convert.ToInt32(o.UID)).Username, id, "Đã có đánh giá mới cho đơn hàng #" + id + " của bạn. CLick vào để xem",
                                       12, currentDate, obj_user.Username, true);
                                            }
                                            else
                                            {
                                                NotificationsController.Inser(Convert.ToInt32(o.UID),
                                       AccountController.GetByID(Convert.ToInt32(o.UID)).Username, id, "Đã có đánh giá mới cho đơn hàng #" + id + " của bạn. CLick vào để xem",
                                       13, currentDate, obj_user.Username, true);
                                            }

                                        }

                                        if (setNoti.IsSendEmailUser == true)
                                        {
                                            try
                                            {
                                                PJUtils.SendMailGmail("no-reply@mona-media.com", "demonhunter",
                                                    AccountInfoController.GetByUserID(Convert.ToInt32(o.UID)).Email,
                                                    "Thông báo tại Taobao.vn.",
                                                    "Đã có đánh giá mới cho đơn hàng #" + id
                                                    + " của bạn. CLick vào để xem", "");
                                            }
                                            catch { }
                                        }
                                    }
                                }
                                ChatHub ch = new ChatHub();
                                ch.SendMessenger(uid, id, comment, listLink, listComment);

                                CustomerComment dataout = new CustomerComment();
                                dataout.UID = uid;
                                dataout.OrderID = id;
                                StringBuilder showIMG = new StringBuilder();
                                for (int i = 0; i < listLink.Count; i++)
                                {
                                    showIMG.Append("<div class=\"chat chat-right\">");
                                    showIMG.Append("    <div class=\"chat-avatar\">");
                                    showIMG.Append("    <p class=\"name\">" + AccountController.GetByID(uid).Username + "</p>");
                                    showIMG.Append("    </div>");
                                    showIMG.Append("    <div class=\"chat-body\">");
                                    showIMG.Append("        <div class=\"chat-text\">");
                                    showIMG.Append("            <div class=\"date-time center-align\">" + string.Format("{0:dd/MM/yyyy HH:mm}", DateTime.Now) + "</div>");
                                    showIMG.Append("            <div class=\"text-content\">");
                                    showIMG.Append("                <div class=\"content\">");
                                    showIMG.Append("                    <div class=\"content-img\" style=\"border-radius: 5px;background-color: #2196f3;\">");
                                    showIMG.Append("	                    <div class=\"img-block\">");
                                    if (listLink[i].Contains(".doc"))
                                    {
                                        showIMG.Append("<a href=\"" + listLink[i] + "\" target=\"_blank\"><img src=\"/App_Themes/AdminNew45/assets/images/icon/file.png\" title=\"" + listComment[i] + "\"  class=\"\" height=\"50\"/></a>");

                                    }
                                    else if (listLink[i].Contains(".xls"))
                                    {
                                        showIMG.Append("<a href=\"" + listLink[i] + "\" target=\"_blank\"><img src=\"/App_Themes/AdminNew45/assets/images/icon/file.png\" title=\"" + listComment[i] + "\"  class=\"\" height=\"50\"/></a>");
                                    }
                                    else
                                    {
                                        showIMG.Append("<a href=\"" + listLink[i] + "\" target=\"_blank\"><img src=\"" + listLink[i] + "\" title=\"" + listComment[i] + "\"  class=\"\" height=\"50\"/></a>");
                                    }
                                    showIMG.Append("	                    </div>");
                                    showIMG.Append("                    </div>");
                                    showIMG.Append("                </div>");
                                    showIMG.Append("            </div>");
                                    showIMG.Append("        </div>");
                                    showIMG.Append("    </div>");
                                    showIMG.Append("</div>");
                                }
                                showIMG.Append("<div class=\"chat chat-right\">");
                                showIMG.Append("    <div class=\"chat-avatar\">");
                                showIMG.Append("    <p class=\"name\">" + AccountController.GetByID(uid).Username + "</p>");
                                showIMG.Append("    </div>");
                                showIMG.Append("    <div class=\"chat-body\">");
                                showIMG.Append("        <div class=\"chat-text\">");
                                showIMG.Append("            <div class=\"date-time center-align\">" + string.Format("{0:dd/MM/yyyy HH:mm}", DateTime.Now) + "</div>");
                                showIMG.Append("            <div class=\"text-content\">");
                                showIMG.Append("                <div class=\"content\">");
                                showIMG.Append("                    <p>" + comment + "</p>");
                                showIMG.Append("                </div>");
                                showIMG.Append("            </div>");
                                showIMG.Append("        </div>");
                                showIMG.Append("    </div>");
                                showIMG.Append("</div>");

                                dataout.Comment = showIMG.ToString();
                                return serializer.Serialize(dataout);

                            }
                            else
                            {

                                if (listComment.Count > 0)
                                {
                                    ChatHub ch = new ChatHub();
                                    ch.SendMessenger(uid, id, comment, listLink, listComment);
                                    CustomerComment dataout = new CustomerComment();
                                    StringBuilder showIMG = new StringBuilder();
                                    for (int i = 0; i < listLink.Count; i++)
                                    {

                                        showIMG.Append("<div class=\"chat chat-right\">");
                                        showIMG.Append("<div class=\"chat-avatar\">");
                                        showIMG.Append("    <p class=\"name\">" + AccountController.GetByID(uid).Username + "</p>");
                                        showIMG.Append("</div>");
                                        showIMG.Append("<div class=\"chat-body\">");
                                        showIMG.Append("<div class=\"chat-text\">");
                                        showIMG.Append("<div class=\"date-time center-align\">" + string.Format("{0:dd/MM/yyyy HH:mm}", DateTime.Now) + "</div>");
                                        showIMG.Append("<div class=\"text-content\">");
                                        showIMG.Append("<div class=\"content\">");
                                        showIMG.Append("<div class=\"content-img\" style=\"border-radius: 5px;background-color: #2196f3;\">");
                                        showIMG.Append("<div class=\"img-block\">");
                                        if (listLink[i].Contains(".doc"))
                                        {
                                            showIMG.Append("<a href=\"" + listLink[i] + "\" target=\"_blank\"><img src=\"/App_Themes/AdminNew45/assets/images/icon/file.png\" title=\"" + listComment[i] + "\"  class=\"\" height=\"50\"/></a>");

                                        }
                                        else if (listLink[i].Contains(".xls"))
                                        {
                                            showIMG.Append("<a href=\"" + listLink[i] + "\" target=\"_blank\"><img src=\"/App_Themes/AdminNew45/assets/images/icon/file.png\" title=\"" + listComment[i] + "\"  class=\"\" height=\"50\"/></a>");
                                        }
                                        else
                                        {
                                            showIMG.Append("<a href=\"" + listLink[i] + "\" target=\"_blank\"><img src=\"" + listLink[i] + "\" title=\"" + listComment[i] + "\"  class=\"\" height=\"50\"/></a>");
                                        }
                                        showIMG.Append("</div>");
                                        showIMG.Append("</div>");
                                        showIMG.Append("</div>");
                                        showIMG.Append("</div>");
                                        showIMG.Append("</div>");
                                        showIMG.Append("</div>");
                                        showIMG.Append("</div>");
                                    }
                                    dataout.UID = uid;
                                    dataout.OrderID = id;
                                    dataout.Comment = showIMG.ToString();
                                    return serializer.Serialize(dataout);
                                }

                            }

                        }
                    }
                }
            }
            return serializer.Serialize(null);
        }
        public partial class CustomerComment
        {
            public int UID { get; set; }
            public int OrderID { get; set; }
            public string Comment { get; set; }
            public List<string> Link { get; set; }
            public List<string> CommentName { get; set; }
        }
        [WebMethod]
        public static string sendstaffcomment(string comment, int id, string urlIMG, string real)
        {
            var listLink = urlIMG.Split('|').ToList();
            if (listLink.Count > 0)
            {
                listLink.RemoveAt(listLink.Count - 1);
            }
            var listComment = real.Split('|').ToList();
            if (listComment.Count > 0)
            {
                listComment.RemoveAt(listComment.Count - 1);
            }
            string username = HttpContext.Current.Session["userLoginSystem"].ToString();
            var obj_user = AccountController.GetByUsername(username);
            DateTime currentDate = DateTime.Now;
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            if (obj_user != null)
            {
                string ret = "";
                var ai = AccountInfoController.GetByUserID(obj_user.ID);
                if (ai != null)
                {
                    ret += ai.FirstName + " " + ai.LastName + "," + ai.IMGUser + "," + string.Format("{0:dd/MM/yyyy HH:mm}", currentDate);
                }
                int uid = obj_user.ID;
                //var id = Convert.ToInt32(Request.QueryString["id"]);
                if (id > 0)
                {
                    var o = MainOrderController.GetAllByID(id);
                    if (o != null)
                    {

                        int type = 2;
                        if (type > 0)
                        {
                            for (int i = 0; i < listLink.Count; i++)
                            {
                                string kqq = OrderCommentController.InsertNew(id, listLink[i], listComment[i], true, type, DateTime.Now, uid);
                            }
                            if (!string.IsNullOrEmpty(comment))
                            {
                                string kq = OrderCommentController.Insert(id, comment, true, type, DateTime.Now, uid);
                                var sale = AccountController.GetByID(o.SalerID.Value);
                                if (sale != null)
                                {
                                    if (obj_user.ID != sale.ID)
                                    {
                                        NotificationsController.Inser(sale.ID,
                                                                         sale.Username, id,
                                                                         "Đã có đánh giá mới cho đơn hàng #" + id + " của bạn. CLick vào để xem", 1,
                                                                          currentDate, username, false);
                                    }
                                }

                                var dathang = AccountController.GetByID(o.DathangID.Value);
                                if (dathang != null)
                                {
                                    if (obj_user.ID != dathang.ID)
                                    {
                                        NotificationsController.Inser(dathang.ID,
                                                                           dathang.Username, id,
                                                                           "Đã có đánh giá mới cho đơn hàng #" + id + " của bạn. CLick vào để xem", 1,
                                                                            currentDate, username, false);
                                    }
                                }

                                var admins = AccountController.GetAllByRoleID(0);
                                if (admins.Count > 0)
                                {
                                    foreach (var admin in admins)
                                    {
                                        if (obj_user.ID != admin.ID)
                                        {
                                            NotificationsController.Inser(admin.ID,
                                                                          admin.Username, id,
                                                                          "Đã có đánh giá mới cho đơn hàng #" + id + " của bạn. CLick vào để xem", 1,
                                                                           currentDate, username, false);
                                        }
                                    }
                                }
                                var managers = AccountController.GetAllByRoleID(2);
                                if (managers.Count > 0)
                                {
                                    foreach (var manager in managers)
                                    {
                                        if (obj_user.ID != manager.ID)
                                        {
                                            NotificationsController.Inser(manager.ID,
                                                                           manager.Username, id,
                                                                           "Đã có đánh giá mới cho đơn hàng #" + id + " của bạn. CLick vào để xem", 1,
                                                                          currentDate, username, false);
                                        }
                                    }
                                }
                                ChatHub ch = new ChatHub();
                                ch.SendMessengerToStaff(uid, id, comment, listLink, listComment);

                                CustomerComment dataout = new CustomerComment();
                                dataout.UID = uid;
                                dataout.OrderID = id;
                                StringBuilder showIMG = new StringBuilder();
                                for (int i = 0; i < listLink.Count; i++)
                                {
                                    showIMG.Append("<div class=\"chat chat-right\">");
                                    showIMG.Append("    <div class=\"chat-avatar\">");
                                    showIMG.Append("    <p class=\"name\">" + AccountController.GetByID(uid).Username + "</p>");
                                    showIMG.Append("    </div>");
                                    showIMG.Append("    <div class=\"chat-body\">");
                                    showIMG.Append("        <div class=\"chat-text\">");
                                    showIMG.Append("            <div class=\"date-time center-align\">" + string.Format("{0:dd/MM/yyyy HH:mm}", DateTime.Now) + "</div>");
                                    showIMG.Append("            <div class=\"text-content\">");
                                    showIMG.Append("                <div class=\"content\">");
                                    showIMG.Append("                    <div class=\"content-img\" style=\"border-radius: 5px;background-color: #2196f3;\">");
                                    showIMG.Append("	                    <div class=\"img-block\">");
                                    if (listLink[i].Contains(".doc"))
                                    {
                                        showIMG.Append("<a href=\"" + listLink[i] + "\" target=\"_blank\"><img src=\"/App_Themes/AdminNew45/assets/images/icon/file.png\" title=\"" + listComment[i] + "\"  class=\"\" height=\"50\"/></a>");

                                    }
                                    else if (listLink[i].Contains(".xls"))
                                    {
                                        showIMG.Append("<a href=\"" + listLink[i] + "\" target=\"_blank\"><img src=\"/App_Themes/AdminNew45/assets/images/icon/file.png\" title=\"" + listComment[i] + "\"  class=\"\" height=\"50\"/></a>");
                                    }
                                    else
                                    {
                                        showIMG.Append("<a href=\"" + listLink[i] + "\" target=\"_blank\"><img src=\"" + listLink[i] + "\" title=\"" + listComment[i] + "\"  class=\"\" height=\"50\"/></a>");
                                    }
                                    showIMG.Append("	                    </div>");
                                    showIMG.Append("                    </div>");
                                    showIMG.Append("                </div>");
                                    showIMG.Append("            </div>");
                                    showIMG.Append("        </div>");
                                    showIMG.Append("    </div>");
                                    showIMG.Append("</div>");
                                }
                                showIMG.Append("<div class=\"chat chat-right\">");
                                showIMG.Append("    <div class=\"chat-avatar\">");
                                showIMG.Append("    <p class=\"name\">" + AccountController.GetByID(uid).Username + "</p>");
                                showIMG.Append("    </div>");
                                showIMG.Append("    <div class=\"chat-body\">");
                                showIMG.Append("        <div class=\"chat-text\">");
                                showIMG.Append("            <div class=\"date-time center-align\">" + string.Format("{0:dd/MM/yyyy HH:mm}", DateTime.Now) + "</div>");
                                showIMG.Append("            <div class=\"text-content\">");
                                showIMG.Append("                <div class=\"content\">");
                                showIMG.Append("                    <p>" + comment + "</p>");
                                showIMG.Append("                </div>");
                                showIMG.Append("            </div>");
                                showIMG.Append("        </div>");
                                showIMG.Append("    </div>");
                                showIMG.Append("</div>");


                                dataout.Comment = showIMG.ToString();
                                return serializer.Serialize(dataout);


                            }
                            else
                            {
                                if (listComment.Count > 0)
                                {
                                    ChatHub ch = new ChatHub();
                                    ch.SendMessengerToStaff(uid, id, comment, listLink, listComment);
                                    CustomerComment dataout = new CustomerComment();
                                    StringBuilder showIMG = new StringBuilder();
                                    for (int i = 0; i < listLink.Count; i++)
                                    {

                                        showIMG.Append("<div class=\"chat chat-right\">");
                                        showIMG.Append("<div class=\"chat-avatar\">");
                                        showIMG.Append("    <p class=\"name\">" + AccountController.GetByID(uid).Username + "</p>");
                                        showIMG.Append("</div>");
                                        showIMG.Append("<div class=\"chat-body\">");
                                        showIMG.Append("<div class=\"chat-text\">");
                                        showIMG.Append("<div class=\"date-time center-align\">" + string.Format("{0:dd/MM/yyyy HH:mm}", DateTime.Now) + "</div>");
                                        showIMG.Append("<div class=\"text-content\">");
                                        showIMG.Append("<div class=\"content\">");
                                        showIMG.Append("<div class=\"content-img\" style=\"border-radius: 5px;background-color: #2196f3;\">");
                                        showIMG.Append("<div class=\"img-block\">");
                                        if (listLink[i].Contains(".doc"))
                                        {
                                            showIMG.Append("<a href=\"" + listLink[i] + "\" target=\"_blank\"><img src=\"/App_Themes/AdminNew45/assets/images/icon/file.png\" title=\"" + listComment[i] + "\"  class=\"\" height=\"50\"/></a>");

                                        }
                                        else if (listLink[i].Contains(".xls"))
                                        {
                                            showIMG.Append("<a href=\"" + listLink[i] + "\" target=\"_blank\"><img src=\"/App_Themes/AdminNew45/assets/images/icon/file.png\" title=\"" + listComment[i] + "\"  class=\"\" height=\"50\"/></a>");
                                        }
                                        else
                                        {
                                            showIMG.Append("<a href=\"" + listLink[i] + "\" target=\"_blank\"><img src=\"" + listLink[i] + "\" title=\"" + listComment[i] + "\"  class=\"\" height=\"50\"/></a>");
                                        }
                                        showIMG.Append("</div>");
                                        showIMG.Append("</div>");
                                        showIMG.Append("</div>");
                                        showIMG.Append("</div>");
                                        showIMG.Append("</div>");
                                        showIMG.Append("</div>");
                                        showIMG.Append("</div>");
                                    }
                                    dataout.UID = uid;
                                    dataout.OrderID = id;
                                    dataout.Comment = showIMG.ToString();
                                    return serializer.Serialize(dataout);
                                }
                            }


                        }
                    }
                }
            }
            return serializer.Serialize(null);
        }

        #endregion
        #region Class
        public class historyCustom
        {
            public int ID { get; set; }
            public string Username { get; set; }
            public string RoleName { get; set; }
            public string Date { get; set; }
            public string Content { get; set; }
        }
        public class FeeWeightObj
        {
            public double FeeWeightVND { get; set; }
            public double FeeWeightCYN { get; set; }
            public double DiscountFeeWeightCYN { get; set; }
            public double DiscountFeeWeightVN { get; set; }
        }
        #endregion
        protected void r_ItemCommand(object sender, GridCommandEventArgs e)
        {
            var g = e.Item as GridDataItem;
            if (g == null) return;

        }

        protected void gr_PageIndexChanged(object sender, GridPageChangedEventArgs e)
        {

        }
        protected void gr_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
        {
            var id = Convert.ToInt32(Request.QueryString["id"]);
            if (id > 0)
            {
                var o = MainOrderController.GetAllByID(id);
                if (o != null)
                {
                    var historyorder = HistoryOrderChangeController.GetByMainOrderID(o.ID);
                    if (historyorder.Count > 0)
                    {
                        List<historyCustom> hc = new List<historyCustom>();
                        foreach (var item in historyorder)
                        {
                            string username = item.Username;
                            string rolename = "admin";
                            var acc = AccountController.GetByUsername(username);
                            if (acc != null)
                            {
                                int role = Convert.ToInt32(acc.RoleID);

                                var r = RoleController.GetByID(role);
                                if (r != null)
                                {
                                    rolename = r.RoleDescription;
                                }
                            }
                            historyCustom h = new historyCustom();
                            h.ID = item.ID;
                            h.Username = username;
                            h.RoleName = rolename;
                            h.Date = string.Format("{0:dd/MM/yyyy HH:mm}", item.CreatedDate);
                            h.Content = item.HistoryContent;
                            hc.Add(h);
                        }
                        gr.DataSource = hc;
                    }
                }
            }
        }
    }
}