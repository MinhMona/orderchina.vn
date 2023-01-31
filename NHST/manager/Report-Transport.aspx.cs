using MB.Extensions;
using NHST.Bussiness;
using NHST.Controllers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Script.Serialization;
using System.Web.Services;
using System.Web.UI;
using System.Web.UI.WebControls;
using static NHST.Controllers.TransportationOrderNewController;

namespace NHST.manager
{
    public partial class Report_Transport : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                if (Session["userLoginSystem"] == null)
                {
                    Response.Redirect("/manager/Login.aspx");
                }
                else
                {
                    string Username = Session["userLoginSystem"].ToString();
                    var obj_user = AccountController.GetByUsername(Username);
                    if (obj_user != null)
                    {
                        if (obj_user.RoleID != 0 && obj_user.RoleID != 2 && obj_user.RoleID != 4)
                        {
                            Response.Redirect("/trang-chu");
                        }
                    }
                }
                LoadData();
            }
        }

        public void LoadData()
        {
            string fd = Request.QueryString["fd"];
            string td = Request.QueryString["td"];
            if (!string.IsNullOrEmpty(fd))
                txtdatefrom.Text = fd;
            if (!string.IsNullOrEmpty(td))
                txtdateto.Text = td;
            int page = 0;
            Int32 Page = GetIntFromQueryString("Page");
            if (Page > 0)
            {
                page = Page - 1;
            }
            var la = TransportationOrderNewController.GetReportTransBySQL(fd, td, page, 10);
            var total = TransportationOrderNewController.GetTotalBuyProBySQL(fd, td);
            pagingall(la, total);
        }
        public void pagingall(List<TransportOrderNew> acs, int total)
        {
            int PageSize = 10;
            if (total > 0)
            {
                int TotalItems = total;
                if (TotalItems % PageSize == 0)
                    PageCount = TotalItems / PageSize;
                else
                    PageCount = TotalItems / PageSize + 1;
                Int32 Page = GetIntFromQueryString("Page");
                if (Page == -1) Page = 1;
                int FromRow = (Page - 1) * PageSize;
                int ToRow = Page * PageSize - 1;
                if (ToRow >= TotalItems)
                    ToRow = TotalItems - 1;
                var list = HttpContext.Current.Session["CheckBarcode"] as List<ListBarcode>;
                StringBuilder hcm = new StringBuilder();
                for (int i = 0; i < acs.Count; i++)
                {
                    var item = acs[i];

                    string auto = "";
                    string QGVN = "VIETNAM";
                    string QGTQ = "CHINA";
                    string FullName = "";
                    FullName = item.FirstName + " " + item.LastName;
                    hcm.Append("<tr>");
                    hcm.Append("<td>");
                    if (list != null)
                    {
                        var check = list.Where(x => x.IDBarcode == item.IDMVD).SingleOrDefault();
                        if (check != null)
                        {
                            hcm.Append(" <label><input type=\"checkbox\" checked onchange=\"CheckBarcode(" + item.IDMVD + ")\"  data-id=\"" + item.IDMVD + "\"><span></span></label>");
                        }
                        else
                        {
                            hcm.Append(" <label><input type=\"checkbox\" onchange=\"CheckBarcode(" + item.IDMVD + ")\"  data-id=\"" + item.IDMVD + "\"><span></span></label>");
                        }
                    }
                    else
                    {
                        hcm.Append(" <label><input type=\"checkbox\" onchange=\"CheckBarcode(" + item.IDMVD + ")\"  data-id=\"" + item.IDMVD + "\"><span></span></label>");
                    }
                    hcm.Append("</td>");
                    hcm.Append("<td>" + item.MaVanDon + "</td>");
                    hcm.Append("<td>" + auto + "</td>");
                    hcm.Append("<td>" + auto + "</td>");
                    hcm.Append("<td>" + auto + "</td>");
                    hcm.Append("<td>" + auto + "</td>");
                    hcm.Append("<td>" + auto + "</td>");
                    hcm.Append("<td>" + QGTQ + "</td>");
                    hcm.Append("<td>" + auto + "</td>");
                    hcm.Append("<td>" + FullName + "</td>");
                    hcm.Append("<td>" + FullName + "</td>");
                    hcm.Append("<td>" + item.Phone + "</td>");
                    hcm.Append("<td>" + item.Address + "</td>");
                    hcm.Append("<td>" + QGVN + "</td>");
                    hcm.Append("<td>" + auto + "</td>");
                    hcm.Append("<td>" + auto + "</td>");
                    hcm.Append("<td>" + item.NameProduct + "</td>");
                    hcm.Append("<td>" + item.Quantity + "</td>");
                    hcm.Append("<td>" + string.Format("{0:N0}", Convert.ToDouble(item.PriceVND)) + "</td>");
                    hcm.Append("<td>" + string.Format("{0:N0}", item.TotalPriceVND) + "</td>");
                    hcm.Append("<td>" + auto + "</td>");
                    hcm.Append("<td>" + auto + "</td>");
                    hcm.Append("<td>" + auto + "</td>");
                    hcm.Append("<td>" + item.Weight + "</td>");
                    hcm.Append("<td>" + auto + "</td>");
                    hcm.Append("<td>" + auto + "</td>");
                    hcm.Append("<td>" + auto + "</td>");
                    hcm.Append("<td>" + auto + "</td>");
                    hcm.Append("<td>" + auto + "</td>");
                    hcm.Append("<td>" + auto + "</td>");
                    hcm.Append("<td>" + auto + "</td>");
                    hcm.Append("<td>" + auto + "</td>");
                    hcm.Append("<td>" + auto + "</td>");
                    hcm.Append("<td>" + auto + "</td>");
                    hcm.Append("<td>" + auto + "</td>");
                    hcm.Append("<td>" + auto + "</td>");
                    hcm.Append("<td>" + auto + "</td>");
                    hcm.Append("<td>" + auto + "</td>");
                    hcm.Append("<td>" + auto + "</td>");
                    hcm.Append("<td>" + item.PackageCode + "</td>");
                    hcm.Append("<td>" + item.ID + "</td>");
                    hcm.Append("<td>" + auto + "</td>");                   
                    hcm.Append("<td>" + item.Note + "</td>");                    
                    hcm.Append("</tr>");
                }
                ltr.Text = hcm.ToString();
            }
        }

        public class ListBarcode
        {
            public int IDBarcode { get; set; }
        }

        [WebMethod]
        public static string CheckBarcode(int IDBarcode)
        {
            List<ListBarcode> ldep = new List<ListBarcode>();
            var list = HttpContext.Current.Session["CheckBarcode"] as List<ListBarcode>;
            if (list != null)
            {
                if (list.Count > 0)
                {
                    var check = list.Where(x => x.IDBarcode == IDBarcode).FirstOrDefault();
                    if (check != null)
                    {
                        list.Remove(check);
                    }
                    else
                    {
                        ListBarcode d = new ListBarcode();
                        d.IDBarcode = IDBarcode;
                        list.Add(d);
                    }
                }
                else
                {
                    ListBarcode d = new ListBarcode();
                    d.IDBarcode = IDBarcode;
                    list.Add(d);
                }
                JavaScriptSerializer serializer = new JavaScriptSerializer();
                return serializer.Serialize(list);
            }
            else
            {
                ListBarcode d = new ListBarcode();
                d.IDBarcode = IDBarcode;
                ldep.Add(d);
                HttpContext.Current.Session["CheckBarcode"] = ldep;
                JavaScriptSerializer serializer = new JavaScriptSerializer();
                return serializer.Serialize(ldep);
            }
        }

        protected void btnExport_Click(object sender, EventArgs e)
        {
            int ID = hdfID.Value.ToInt(0);
            var list = HttpContext.Current.Session["CheckBarcode"] as List<ListBarcode>;
            if (list != null)
            {
                if (list.Count > 0)
                {
                    foreach (var item in list)
                    {
                        var ListReport = TransportationOrderNewController.GetReportGeneraltBySQLNotPage();
                        List<ReportGeneral> ro_gr = new List<ReportGeneral>();
                        if (ListReport.Count > 0)
                        {
                            foreach (var o in ListReport)
                            {
                                if (item.IDBarcode == o.IDMVD)
                                {
                                    ReportGeneral or = new ReportGeneral();

                                    string auto = "";
                                    string QGVN = "VIETNAM";
                                    string QGTQ = "CHINA";
                                    string FullName = "";
                                    FullName = o.FirstName + " " + o.LastName;
                                    or.ID = o.ID;
                                    or.MVD = o.MaVanDon;
                                    or.Auto = auto;
                                    or.QGTQ = QGTQ;
                                    or.QGVN = QGVN;
                                    or.PackageCode = o.PackageCode;
                                    or.FullName = FullName;
                                    or.Phone = o.Phone;
                                    or.Address = o.Address;
                                    or.Quantity = o.Quantity;
                                    or.PriceCNY = string.Format("{0:N0}", Convert.ToDouble(o.PriceProduct));
                                    or.TotalPriceVND = string.Format("{0:N0}", Convert.ToDouble(o.TotalPriceVND));
                                    or.TQVNWeight = o.Weight;
                                    or.Note = o.Note;
                                    or.category = o.NameProduct;

                                    ro_gr.Add(or);
                                }
                            }
                        }
                        StringBuilder StrExport = new StringBuilder();
                        StrExport.Append(@"<html xmlns:o='urn:schemas-microsoft-com:office:office' xmlns:w='urn:schemas-microsoft-com:office:excel' xmlns='http://www.w3.org/TR/REC-html40'><head><title>Time</title>");
                        StrExport.Append(@"<body lang=EN-US style='mso-element:header' id=h1><span style='mso--code:DATE'></span><div class=Section1>");
                        StrExport.Append("<DIV  style='font-size:14px;'>");
                        StrExport.Append("<table border=\"1\">");
                        StrExport.Append("  <tr>");
                        StrExport.Append("      <th style=\"mso-number-format:'\\@'\"><strong>Waybill Number</strong></th>");
                        StrExport.Append("      <th style=\"mso-number-format:'\\@'\"><strong>Shipment Reference No.</strong></th>");
                        StrExport.Append("      <th style=\"mso-number-format:'\\@'\"><strong>Company Name</strong></th>");
                        StrExport.Append("      <th style=\"mso-number-format:'\\@'\"><strong>Contact Name</strong></th>");
                        StrExport.Append("      <th style=\"mso-number-format:'\\@'\"><strong>Tel</strong></th>");
                        StrExport.Append("      <th style=\"mso-number-format:'\\@'\"><strong>Address</strong></th>");
                        StrExport.Append("      <th style=\"mso-number-format:'\\@'\"><strong>Country</strong></th>");
                        StrExport.Append("      <th style=\"mso-number-format:'\\@'\"><strong>Postal Code</strong></th>");
                        StrExport.Append("      <th style=\"mso-number-format:'\\@'\"><strong>Company Name</strong></th>");
                        StrExport.Append("      <th style=\"mso-number-format:'\\@'\"><strong>Contact Name</strong></th>");
                        StrExport.Append("      <th style=\"mso-number-format:'\\@'\"><strong>Tel</strong></th>");
                        StrExport.Append("      <th style=\"mso-number-format:'\\@'\"><strong>Address</strong></th>");
                        StrExport.Append("      <th style=\"mso-number-format:'\\@'\"><strong>Country</strong></th>");
                        StrExport.Append("      <th style=\"mso-number-format:'\\@'\"><strong>Currency</strong></th>");
                        StrExport.Append("      <th style=\"mso-number-format:'\\@'\"><strong>Content</strong></th>");
                        StrExport.Append("      <th style=\"mso-number-format:'\\@'\"><strong>Quantity</strong></th>");
                        StrExport.Append("      <th style=\"mso-number-format:'\\@'\"><strong>Unit Price</strong></th>");
                        StrExport.Append("      <th style=\"mso-number-format:'\\@'\"><strong>Total Value</strong></th>");
                        StrExport.Append("      <th style=\"mso-number-format:'\\@'\"><strong>More than 2 items</strong></th>");
                        StrExport.Append("      <th style=\"mso-number-format:'\\@'\"><strong>Shipment Type</strong></th>");
                        StrExport.Append("      <th style=\"mso-number-format:'\\@'\"><strong>Total Package</strong></th>");
                        StrExport.Append("      <th style=\"mso-number-format:'\\@'\"><strong>Total Actual Weight</strong></th>");
                        StrExport.Append("      <th style=\"mso-number-format:'\\@'\"><strong>Origin</strong></th>");
                        StrExport.Append("      <th style=\"mso-number-format:'\\@'\"><strong>Destination</strong></th>");
                        StrExport.Append("      <th style=\"mso-number-format:'\\@'\"><strong>Creation Date</strong></th>");
                        StrExport.Append("      <th style=\"mso-number-format:'\\@'\"><strong>Processing Shipment Date</strong></th>");
                        StrExport.Append("      <th style=\"mso-number-format:'\\@'\"><strong>HSCODE</strong></th>");
                        StrExport.Append("      <th style=\"mso-number-format:'\\@'\"><strong>Category</strong></th>");
                        StrExport.Append("      <th style=\"mso-number-format:'\\@'\"><strong>BoxID</strong></th>");
                        StrExport.Append("      <th style=\"mso-number-format:'\\@'\"><strong>Package Number</strong></th>");
                        StrExport.Append("      <th style=\"mso-number-format:'\\@'\"><strong>Payment Method</strong></th>");
                        StrExport.Append("      <th style=\"mso-number-format:'\\@'\"><strong>COD Value</strong></th>");
                        StrExport.Append("      <th style=\"mso-number-format:'\\@'\"><strong>Product Category ID</strong></th>");
                        StrExport.Append("      <th style=\"mso-number-format:'\\@'\"><strong>Sort Code</strong></th>");
                        StrExport.Append("      <th style=\"mso-number-format:'\\@'\"><strong>SKUID</strong></th>");
                        StrExport.Append("      <th style=\"mso-number-format:'\\@'\"><strong>Total COD Value</strong></th>");
                        StrExport.Append("      <th style=\"mso-number-format:'\\@'\"><strong>Category (ZH)</strong></th>");
                        StrExport.Append("      <th style=\"mso-number-format:'\\@'\"><strong>Package Code</strong></th>");
                        StrExport.Append("      <th style=\"mso-number-format:'\\@'\"><strong>Order ID</strong></th>");
                        StrExport.Append("      <th style=\"mso-number-format:'\\@'\"><strong>Link</strong></th>");
                        StrExport.Append("      <th style=\"mso-number-format:'\\@'\"><strong>Note</strong></th>");
                        StrExport.Append("  </tr>");

                        foreach (var item2 in ro_gr)
                        {
                            StrExport.Append("  <tr>");
                            StrExport.Append("      <td style=\"mso-number-format:'\\@'\">" + item2.MVD + "</td>");
                            StrExport.Append("      <td style=\"mso-number-format:'\\@'\">" + item2.Auto + "</td>");
                            StrExport.Append("      <td style=\"mso-number-format:'\\@'\">" + item2.Auto + "</td>");
                            StrExport.Append("      <td style=\"mso-number-format:'\\@'\">" + item2.Auto + "</td>");
                            StrExport.Append("      <td style=\"mso-number-format:'\\@'\">" + item2.Auto + "</td>");
                            StrExport.Append("      <td style=\"mso-number-format:'\\@'\">" + item2.Auto + "</td>");
                            StrExport.Append("      <td style=\"mso-number-format:'\\@'\">" + item2.QGTQ + "</td>");
                            StrExport.Append("      <td style=\"mso-number-format:'\\@'\">" + item2.Auto + "</td>");
                            StrExport.Append("      <td style=\"mso-number-format:'\\@'\">" + item2.FullName + "</td>");
                            StrExport.Append("      <td style=\"mso-number-format:'\\@'\">" + item2.FullName + "</td>");
                            StrExport.Append("      <td style=\"mso-number-format:'\\@'\">" + item2.Phone + "</td>");
                            StrExport.Append("      <td style=\"mso-number-format:'\\@'\">" + item2.Address + "</td>");
                            StrExport.Append("      <td style=\"mso-number-format:'\\@'\">" + item2.QGVN + "</td>");
                            StrExport.Append("      <td style=\"mso-number-format:'\\@'\">" + item2.Auto + "</td>");
                            StrExport.Append("      <td style=\"mso-number-format:'\\@'\">" + item2.Auto + "</td>");
                            StrExport.Append("      <td style=\"mso-number-format:'\\@'\">" + item2.category + "</td>");
                            StrExport.Append("      <td style=\"mso-number-format:'\\@'\">" + item2.Quantity + "</td>");
                            StrExport.Append("      <td style=\"mso-number-format:'\\@'\">" + item2.PriceVND + "</td>");
                            StrExport.Append("      <td style=\"mso-number-format:'\\@'\">" + item2.TotalPriceVND + "</td>");
                            StrExport.Append("      <td style=\"mso-number-format:'\\@'\">" + item2.Auto + "</td>");
                            StrExport.Append("      <td style=\"mso-number-format:'\\@'\">" + item2.Auto + "</td>");
                            StrExport.Append("      <td style=\"mso-number-format:'\\@'\">" + item2.Auto + "</td>");
                            StrExport.Append("      <td style=\"mso-number-format:'\\@'\">" + item2.TQVNWeight + "</td>");
                            StrExport.Append("      <td style=\"mso-number-format:'\\@'\">" + item2.Auto + "</td>");
                            StrExport.Append("      <td style=\"mso-number-format:'\\@'\">" + item2.Auto + "</td>");
                            StrExport.Append("      <td style=\"mso-number-format:'\\@'\">" + item2.Auto + "</td>");
                            StrExport.Append("      <td style=\"mso-number-format:'\\@'\">" + item2.Auto + "</td>");
                            StrExport.Append("      <td style=\"mso-number-format:'\\@'\">" + item2.Auto + "</td>");
                            StrExport.Append("      <td style=\"mso-number-format:'\\@'\">" + item2.Auto + "</td>");
                            StrExport.Append("      <td style=\"mso-number-format:'\\@'\">" + item2.Auto + "</td>");
                            StrExport.Append("      <td style=\"mso-number-format:'\\@'\">" + item2.Auto + "</td>");
                            StrExport.Append("      <td style=\"mso-number-format:'\\@'\">" + item2.Auto + "</td>");
                            StrExport.Append("      <td style=\"mso-number-format:'\\@'\">" + item2.Auto + "</td>");
                            StrExport.Append("      <td style=\"mso-number-format:'\\@'\">" + item2.Auto + "</td>");
                            StrExport.Append("      <td style=\"mso-number-format:'\\@'\">" + item2.Auto + "</td>");
                            StrExport.Append("      <td style=\"mso-number-format:'\\@'\">" + item2.Auto + "</td>");
                            StrExport.Append("      <td style=\"mso-number-format:'\\@'\">" + item2.Auto + "</td>");
                            StrExport.Append("      <td style=\"mso-number-format:'\\@'\">" + item2.Auto + "</td>");
                            StrExport.Append("      <td style=\"mso-number-format:'\\@'\">" + item2.PackageCode + "</td>");
                            StrExport.Append("      <td style=\"mso-number-format:'\\@'\">" + item2.ID + "</td>");
                            StrExport.Append("      <td style=\"mso-number-format:'\\@'\">" + item2.Auto + "</td>");
                            StrExport.Append("      <td style=\"mso-number-format:'\\@'\">" + item2.Note + "</td>");
                            StrExport.Append("  </tr>");
                        }
                        StrExport.Append("</table>");
                        StrExport.Append("</div></body></html>");
                        string strFile = "Thong-ke-don-hang-ky-gui.xls";
                        string strcontentType = "application/vnd.ms-excel";
                        Response.Clear();
                        Response.Write("<meta http-equiv=\"Content-Type\" content=\"text/html; charset=utf-8\" />");
                        Response.ClearHeaders();
                        Response.BufferOutput = true;
                        Response.ContentType = strcontentType;
                        Response.ContentEncoding = System.Text.Encoding.UTF8;
                        Response.AddHeader("Content-Disposition", "attachment; filename=" + strFile);
                        Response.Write(StrExport.ToString());
                        Response.Flush();
                        Response.End();
                    }
                }
            }
            else
            {
                PJUtils.ShowMessageBoxSwAlert("Chưa có kiện hàng nào được chọn, vui lòng thử lại.", "e", false, Page);
            }
        }

        public class ReportGeneral
        {
            public int ID { get; set; }
            public string MVD { get; set; }
            public int UID { get; set; }
            public string ShopName { get; set; }
            public string Site { get; set; }
            public string PackageCode { get; set; }
            public string PriceVND { get; set; }
            public string PriceCNY { get; set; }
            public string FeeBuyPro { get; set; }
            public string FeeWeight { get; set; }
            public string FullName { get; set; }
            public string Note { get; set; }
            public string Address { get; set; }
            public string Email { get; set; }
            public string CreatedDate { get; set; }
            public string Phone { get; set; }
            public string Status { get; set; }
            public string Deposit { get; set; }
            public string TotalPriceVND { get; set; }
            public string AmountDeposit { get; set; }
            public string TQVNWeight { get; set; }
            public string SKU { get; set; }
            public string category { get; set; }
            public string link { get; set; }
            public string PTVC { get; set; }
            public string Quantity { get; set; }
            public string Auto { get; set; }
            public string MaDonHang { get; set; }
            public string QGTQ { get; set; }
            public string QGVN { get; set; }
        }

        private int PageCount;
        protected void DisplayHtmlStringPaging1()
        {
            Int32 CurrentPage = Convert.ToInt32(Request.QueryString["Page"]);
            if (CurrentPage == -1) CurrentPage = 1;
            string[] strText = new string[4] { "Trang đầu", "Trang cuối", "Trang sau", "Trang trước" };
            if (PageCount > 1)
                Response.Write(GetHtmlPagingAdvanced(6, CurrentPage, PageCount, Context.Request.RawUrl, strText));
        }
        private static string GetPageUrl(int currentPage, string pageUrl)
        {
            pageUrl = Regex.Replace(pageUrl, "(\\?|\\&)*" + "Page=" + currentPage, "");
            if (pageUrl.IndexOf("?") > 0)
            {
                if (pageUrl.IndexOf("Page=") > 0)
                {
                    int a = pageUrl.IndexOf("Page=");
                    int b = pageUrl.Length;
                    pageUrl.Remove(a, b - a);
                }
                else
                {
                    pageUrl += "&Page={0}";
                }

            }
            else
            {
                pageUrl += "?Page={0}";
            }
            return pageUrl;
        }
        public static string GetHtmlPagingAdvanced(int pagesToOutput, int currentPage, int pageCount, string currentPageUrl, string[] strText)
        {
            //Nếu Số trang hiển thị là số lẻ thì tăng thêm 1 thành chẵn
            if (pagesToOutput % 2 != 0)
            {
                pagesToOutput++;
            }

            //Một nửa số trang để đầu ra, đây là số lượng hai bên.
            int pagesToOutputHalfed = pagesToOutput / 2;

            //Url của trang
            string pageUrl = GetPageUrl(currentPage, currentPageUrl);


            //Trang đầu tiên
            int startPageNumbersFrom = currentPage - pagesToOutputHalfed; ;

            //Trang cuối cùng
            int stopPageNumbersAt = currentPage + pagesToOutputHalfed; ;

            StringBuilder output = new StringBuilder();

            //Nối chuỗi phân trang
            //output.Append("<div class=\"paging\">");
            //output.Append("<ul class=\"paging_hand\">");

            //Link First(Trang đầu) và Previous(Trang trước)
            if (currentPage > 1)
            {
                //output.Append("<li class=\"UnselectedPrev \" ><a title=\"" + strText[0] + "\" href=\"" + string.Format(pageUrl, 1) + "\">|<</a></li>");
                //output.Append("<li class=\"UnselectedPrev\" ><a title=\"" + strText[1] + "\" href=\"" + string.Format(pageUrl, currentPage - 1) + "\"><i class=\"fa fa-angle-left\"></i></a></li>");
                output.Append("<a class=\"prev-page pagi-button\" title=\"" + strText[1] + "\" href=\"" + string.Format(pageUrl, currentPage - 1) + "\">Prev</a>");
                //output.Append("<span class=\"Unselect_prev\"><a href=\"" + string.Format(pageUrl, currentPage - 1) + "\"></a></span>");
            }

            /******************Xác định startPageNumbersFrom & stopPageNumbersAt**********************/
            if (startPageNumbersFrom < 1)
            {
                startPageNumbersFrom = 1;

                //As page numbers are starting at one, output an even number of pages.  
                stopPageNumbersAt = pagesToOutput;
            }

            if (stopPageNumbersAt > pageCount)
            {
                stopPageNumbersAt = pageCount;
            }

            if ((stopPageNumbersAt - startPageNumbersFrom) < pagesToOutput)
            {
                startPageNumbersFrom = stopPageNumbersAt - pagesToOutput;
                if (startPageNumbersFrom < 1)
                {
                    startPageNumbersFrom = 1;
                }
            }
            /******************End: Xác định startPageNumbersFrom & stopPageNumbersAt**********************/

            //Các dấu ... chỉ những trang phía trước  
            if (startPageNumbersFrom > 1)
            {
                output.Append("<a href=\"" + string.Format(GetPageUrl(currentPage - 1, pageUrl), startPageNumbersFrom - 1) + "\">&hellip;</a>");
            }

            //Duyệt vòng for hiển thị các trang
            for (int i = startPageNumbersFrom; i <= stopPageNumbersAt; i++)
            {
                if (currentPage == i)
                {
                    output.Append("<a class=\"pagi-button current-active\">" + i.ToString() + "</a>");
                }
                else
                {
                    output.Append("<a class=\"pagi-button\" href=\"" + string.Format(pageUrl, i) + "\">" + i.ToString() + "</a>");
                }
            }

            //Các dấu ... chỉ những trang tiếp theo  
            if (stopPageNumbersAt < pageCount)
            {
                output.Append("<a href=\"" + string.Format(pageUrl, stopPageNumbersAt + 1) + "\">&hellip;</a>");
            }

            //Link Next(Trang tiếp) và Last(Trang cuối)
            if (currentPage != pageCount)
            {
                //output.Append("<span class=\"Unselect_next\"><a href=\"" + string.Format(pageUrl, currentPage + 1) + "\"></a></span>");
                //output.Append("<li class=\"UnselectedNext\" ><a title=\"" + strText[2] + "\" href=\"" + string.Format(pageUrl, currentPage + 1) + "\"><i class=\"fa fa-angle-right\"></i></a></li>");
                output.Append("<a class=\"next-page pagi-button\" title=\"" + strText[2] + "\" href=\"" + string.Format(pageUrl, currentPage + 1) + "\">Next</a>");
                //output.Append("<li class=\"UnselectedNext\" ><a title=\"" + strText[3] + "\" href=\"" + string.Format(pageUrl, pageCount) + "\">>|</a></li>");
            }
            //output.Append("</ul>");
            //output.Append("</div>");
            return output.ToString();
        }
        public static Int32 GetIntFromQueryString(String key)
        {
            Int32 returnValue = -1;
            String queryStringValue = HttpContext.Current.Request.QueryString[key];
            try
            {
                if (queryStringValue == null)
                    return returnValue;
                if (queryStringValue.IndexOf("#") > 0)
                    queryStringValue = queryStringValue.Substring(0, queryStringValue.IndexOf("#"));
                returnValue = Convert.ToInt32(queryStringValue);
            }
            catch
            { }
            return returnValue;
        }

        protected void btnFilter_Click(object sender, EventArgs e)
        {
            string fd = "";
            string td = "";
            if (!string.IsNullOrEmpty(txtdatefrom.Text))
            {
                fd = txtdatefrom.Text.ToString();
            }
            if (!string.IsNullOrEmpty(txtdateto.Text))
            {
                td = txtdateto.Text.ToString();
            }

            if (fd == "" && td == "")
            {
                Response.Redirect("Report_Transport.aspx");
            }
            else
            {
                Response.Redirect("Report_Transport.aspx?&fd=" + fd + "&td=" + td);
            }

        }
    }
}