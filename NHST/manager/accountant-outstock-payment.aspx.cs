using NHST.Bussiness;
using NHST.Controllers;
using NHST.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using ZLADIPJ.Business;
using Telerik.Web.UI;
using System.Text;
using System.Text.RegularExpressions;

namespace NHST.manager
{
    public partial class accountant_outstock_payment : System.Web.UI.Page
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
                    if (ac.RoleID != 0 && ac.RoleID != 7 && ac.RoleID != 2)
                        Response.Redirect("/trang-chu");
                    LoadData();
                }
            }
        }
        public void LoadData()
        {
            string searchname = "";//search_name.Text.Trim();
            string fd = Request.QueryString["fd"];
            string td = Request.QueryString["td"];
            int status = -1;
            if (Request.QueryString["st"] != null)
                status = Convert.ToInt32(Request.QueryString["st"]);
            ddlStatus.SelectedValue = status.ToString();
            int page = 0;
            Int32 Page = GetIntFromQueryString("Page");
            if (Page > 0)
            {
                page = Page - 1;
            }
            if (!string.IsNullOrEmpty(Request.QueryString["s"]))
            {
                searchname = Request.QueryString["s"].ToString().Trim();
            }

            var la = OutStockSessionController.GetBySQL_DK(searchname, page, 20, fd, td, status);
            int Total = OutStockSessionController.GetTotalBySQL(searchname, fd, td, status);
            pagingall(la, Total, 20);

        }
        protected void btnSearch_Click(object sender, EventArgs e)
        {
            string searchname = search_name.Text.Trim();
            string fd = "";
            string td = "";
            string st = ddlStatus.SelectedValue;
            if (!string.IsNullOrEmpty(rFD.Text))
            {
                fd = rFD.Text.ToString();
            }
            if (!string.IsNullOrEmpty(rTD.Text))
            {
                td = rTD.Text.ToString();
            }
            if (!string.IsNullOrEmpty(searchname))
            {
                if (!string.IsNullOrEmpty(fd) && !string.IsNullOrEmpty(td))
                {
                    if (DateTime.ParseExact(fd, "dd/MM/yyyy HH:mm", null) > DateTime.ParseExact(td, "dd/MM/yyyy HH:mm", null))
                    {
                        PJUtils.ShowMessageBoxSwAlert("Từ ngày phải trước đến ngày", "e", false, Page);
                    }
                    else
                    {

                        Response.Redirect("accountant-outstock-payment?s=" + searchname + "&fd=" + fd + "&td=" + td + "&st=" + st + "");
                    }
                }
                else
                {
                    Response.Redirect("accountant-outstock-payment?st=" + st + "&s=" + searchname);
                }

            }
            else
            {
                if (!string.IsNullOrEmpty(fd) && !string.IsNullOrEmpty(td))
                {
                    if (DateTime.ParseExact(fd, "dd/MM/yyyy HH:mm", null) > DateTime.ParseExact(td, "dd/MM/yyyy HH:mm", null))
                    {
                        PJUtils.ShowMessageBoxSwAlert("Từ ngày phải trước đến ngày", "e", false, Page);
                    }
                    else
                    {

                        Response.Redirect("accountant-outstock-payment?fd=" + fd + "&td=" + td + "&st=" + st + "");
                    }
                }
                else
                {
                    Response.Redirect("accountant-outstock-payment?st=" + st + "");
                }
            }
        }
        #region grid event
        //protected void r_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
        //{
        //    string search = "";
        //    var la = OutStockSessionController.GetBySQL_DK(search);
        //    if (la != null)
        //    {
        //        if (la.Count > 0)
        //        {
        //            gr.DataSource = la;
        //        }
        //    }
        //}
        #endregion

        #region Pagging
        public void pagingall(List<OutStockSessionController.OutStockNew> acs, int total, int PageSize)
        {

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
                StringBuilder hcm = new StringBuilder();
                for (int i = 0; i < acs.Count; i++)
                {
                    var item = acs[i];

                    string MaKH = "";
                    var acc = AccountController.GetByID(item.UID);
                    if (acc != null)
                    {
                        MaKH = acc.MaKH;
                    }

                    hcm.Append("<tr>");
                    hcm.Append("<td>" + item.ID + "</td>");
                    hcm.Append("<td>" + MaKH + "</td>");
                    hcm.Append("<td>" + item.Username + "</td>");
                    hcm.Append("<td>" + item.TotalPayString + "</td>");
                    hcm.Append("<td>" + item.CreatedDateString + "</td>");
                    hcm.Append("<td>" + item.StatusName + "</td>");
                    hcm.Append("<td>");
                    hcm.Append("<div class=\"action-table\">");
                    hcm.Append("<a href=\"view-outstock-session.aspx?id=" + item.ID + "\" class=\"tooltipped\" data-position=\"top\" data-tooltip=\"Cập nhật\"><i class=\"material-icons\">edit</i><span>Cập nhật</span></a>");
                    //hcm.Append("<a href=\"#1\" class=\"tooltipped\" data-position=\"top\" data-tooltip=\"Thanh toán ngay\"><i class=\"material-icons\">payment</i></a>");
                    hcm.Append("</div>");
                    hcm.Append("</td>");
                    hcm.Append("</tr>");
                }
                ltr.Text = hcm.ToString();
            }
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
        #endregion
    }
}