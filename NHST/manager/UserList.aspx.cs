using MB.Extensions;
using NHST.Bussiness;
using NHST.Controllers;
using NHST.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace NHST.manager
{
    public partial class UserList1 : System.Web.UI.Page
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
                    if (ac.RoleID != 0 && ac.RoleID != 2)
                        Response.Redirect("/trang-chu");
                    LoadData();
                    LoadPrefix();
                    LoadSaler();
                }
            }
        }

        public void LoadSaler()
        {
            var salers = AccountController.GetAllByRoleID(6);
            ddlSale.Items.Clear();
            ddlSale.Items.Insert(0, "Chọn nhân viên kinh doanh");
            if (salers.Count > 0)
            {
                ddlSale.DataSource = salers;
                ddlSale.DataBind();
            }
            var dathangs = AccountController.GetAllByRoleID(3);
            ddlDathang.Items.Clear();
            ddlDathang.Items.Insert(0, "Chọn nhân viên đặt hàng");
            if (dathangs.Count > 0)
            {
                ddlDathang.DataSource = dathangs;
                ddlDathang.DataBind();
            }
        }
        protected void btnExcel_Click(object sender, EventArgs e)
        {
            //var la = AccountController.GetAllOrderDesc("");
            var la = AccountController.GetAll_ViewUserListExcel("").Where(u => u.RoleID == 1).ToList();
            StringBuilder StrExport = new StringBuilder();
            StrExport.Append(@"<html xmlns:o='urn:schemas-microsoft-com:office:office' xmlns:w='urn:schemas-microsoft-com:office:excel' xmlns='http://www.w3.org/TR/REC-html40'><head><title>Time</title>");
            StrExport.Append(@"<body lang=EN-US style='mso-element:header' id=h1><span style='mso--code:DATE'></span><div class=Section1>");
            StrExport.Append("<DIV  style='font-size:12px;'>");
            StrExport.Append("<table border=\"1\">");
            StrExport.Append("  <tr>");
            StrExport.Append("      <th><strong>ID</strong></th>");
            StrExport.Append("      <th style=\"mso-number-format:'\\@'\" ><strong>Username</strong></th>");
            StrExport.Append("      <th><strong>Họ</strong></th>");
            StrExport.Append("      <th><strong>Tên</strong></th>");
            StrExport.Append("      <th style=\"mso-number-format:'\\@'\" ><strong>Số ĐT</strong></th>");
            StrExport.Append("      <th><strong>Trạng thái</strong></th>");
            StrExport.Append("      <th><strong>Quyền hạn</strong></th>");
            StrExport.Append("      <th><strong>Nhân viên kinh doanh</strong></th>");
            StrExport.Append("      <th><strong>Địa chỉ</strong></th>");
            StrExport.Append("      <th><strong>Email</strong></th>");
            StrExport.Append("      <th><strong>Level</strong></th>");
            StrExport.Append("      <th><strong>Ngày tạo</strong></th>");
            StrExport.Append("  </tr>");
            foreach (var item in la)
            {
                string firstname = item.FirstName;
                string lastname = item.LastName;
                string phone = item.MobilePhonePrefix + item.MobilePhone;
                string address = item.Address;
                string email = item.Email;               
                StrExport.Append("  <tr>");
                StrExport.Append("      <td>" + item.ID + "</td>");
                StrExport.Append("      <td style=\"mso-number-format:'\\@'\" >" + item.Username + "</td>");
                StrExport.Append("      <td>" + firstname + "</td>");
                StrExport.Append("      <td>" + lastname + "</td>");
                StrExport.Append("      <td style=\"mso-number-format:'\\@'\" >" + phone + "</td>");
                StrExport.Append("      <td>" + PJUtils.StatusToRequest(item.Status) + "</td>");               
                StrExport.Append("      <td>" + item.RoleName + "</td>");
                StrExport.Append("      <td>" + item.saler + "</td>");
                StrExport.Append("      <td>" + address + "</td>");
                StrExport.Append("      <td>" + email + "</td>");                
                StrExport.Append("      <td>" + item.LevelName + "</td>");
                StrExport.Append("      <td>" + string.Format("{0:dd/MM/yyyy HH:mm}", item.CreatedDate) + "</td>");
                StrExport.Append("  </tr>");
            }
            StrExport.Append("</table>");
            StrExport.Append("</div></body></html>");
            string strFile = "UserList.xls";
            string strcontentType = "application/vnd.ms-excel";
            Response.ClearContent();
            Response.ClearHeaders();
            Response.BufferOutput = true;
            Response.ContentType = strcontentType;
            Response.AddHeader("Content-Disposition", "attachment; filename=" + strFile);
            Response.Write(StrExport.ToString());
            Response.Flush();
            //Response.Close();
            Response.End();
        }
        public class UserToExcel
        {
            public int ID { get; set; }
            public string UserName { get; set; }
            public string Ho { get; set; }
            public string Ten { get; set; }
            public string HoVaTen { get; set; }
            public string Sodt { get; set; }
            public string Status { get; set; }
            public string Role { get; set; }
            public int RoleID { get; set; }
            public string Saler { get; set; }
            public string dathang { get; set; }
            public string wallet { get; set; }
            public string CreatedDate { get; set; }
        }

        public void LoadData()
        {
            string search = "";
            string searchid = "";
            string searchsdt = "";
            if (!string.IsNullOrEmpty(Request.QueryString["s"]))
            {
                search = Request.QueryString["s"].ToString().Trim();
                search_name.Text = search;
            }
            if (!string.IsNullOrEmpty(Request.QueryString["id"]))
            {
                searchid = Request.QueryString["id"].ToString().Trim();
                search_id.Text = searchid;
            }
            if (!string.IsNullOrEmpty(Request.QueryString["sdt"]))
            {
                searchsdt = Request.QueryString["sdt"].ToString().Trim();
                search_sdt.Text = searchsdt;
            }
            int page = 0;
            Int32 Page = GetIntFromQueryString("Page");
            if (Page > 0)
            {
                page = Page - 1;
            }
            var la = AccountController.GetListUserBySQL_searchid_searchsdt(search, searchid, searchsdt, page, 10);
            int total = AccountController.GetTotalUser_searchid_searchsdt(search, searchid, searchsdt);
            pagingall(la, total);
        }

        #region Pagging
        public void pagingall(List<View_UserList> acs, int total)
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
                StringBuilder hcm = new StringBuilder();
                for (int i = 0; i < acs.Count; i++)
                {
                    var item = acs[i];
                    int status = Convert.ToInt32(item.Status);
                    hcm.Append("<tr>");

                    hcm.Append("<td>" + item.ID + "</td>");

                    hcm.Append("<td>" + item.MaKH + "</td>");

                    hcm.Append("<td>" + item.Username + "</td>");

                    hcm.Append("<td>" + item.FirstName + " " + item.LastName + "</td>");

                    hcm.Append("<td>" + item.MobilePhone + "</td>");

                    hcm.Append("<td>" + string.Format("{0:N0} VND", Convert.ToDouble(item.Wallet)) + "</td>");

                    hcm.Append("<td>" + string.Format("{0:N0} ¥", Convert.ToDouble(item.WalletCYN)) + "</td>");

                    hcm.Append("<td>" + PJUtils.StatusToRequest(item.Status) + "</td>");

                    hcm.Append("<td>" + string.Format("{0:dd/MM/yyyy}", item.CreatedDate) + "</td>");

                    hcm.Append("<td class=\"no-wrap\">");
                    hcm.Append("<div class=\"action-table\">");
                    hcm.Append("<a href=\"UserInfo.aspx?i=" + item.ID + "\" class=\"tooltipped\" data-position=\"top\"");
                    hcm.Append(" data-tooltip=\"Cập nhật\"><i class=\"material-icons\">edit</i></a>");

                    hcm.Append("<a href=\"javascript:; \" class=\"tooltipped dropdown-trigger\" data-target=\"drop-" + item.ID + "\"");
                    hcm.Append(" data-position=\"top\" data-tooltip=\"Nạp tiền\"><i");
                    hcm.Append(" class=\"material-icons\">monetization_on</i></a>");
                    hcm.Append("<ul id='drop-" + item.ID + "' class='dropdown-content'>");
                    hcm.Append("<li><a href=\"UserWallet.aspx?i=" + item.ID + "&pre=" + Page + "\">Nạp VNĐ</a></li>");
                    hcm.Append("<li><a href=\"AddRequestRechargeCYN.aspx?i=" + item.ID + "&pre=" + Page + "\">Nạp tệ</a></li>");
                    hcm.Append("</ul>");
                    hcm.Append("<a href=\"javascript:; \" class=\"tooltipped dropdown-trigger\" data-target=\"draw-" + item.ID + "\"");
                    hcm.Append(" data-position=\"top\" data-tooltip=\"Rút tiền\"><span");
                    hcm.Append(" class=\"bg-draw\"></span></a>");
                    hcm.Append("<ul id='draw-" + item.ID + "' class='dropdown-content'>");
                    hcm.Append("<li><a href=\"add-withdraw.aspx?u=" + item.Username + "&pre=" + Page + "\">Rút VNĐ</a></li>");
                    hcm.Append("<li><a href=\"addRefund.aspx?i=" + item.ID.ToString() + "&pre=" + Page + "\">Rút tệ</a></li>");
                    hcm.Append("</ul>");
                    hcm.Append("<a href=\"user-order?uid=" + item.ID + "\" class=\"tooltipped\" data-position=\"top\"");
                    hcm.Append(" data-tooltip=\"Danh sách đơn hàng\"><span class=\"list-order\"></span></a>");
                    hcm.Append("<a href=\"tao-don-hang-khac.aspx?i=" + item.ID + "\" class=\"tooltipped\" data-position=\"top\"");
                    hcm.Append(" data-tooltip=\"Tạo đơn hàng khác\"><span class=\"add-order\"></span></a>");

                    hcm.Append("<a href=\"User-Transaction.aspx?i=" + item.ID + "\" class=\"tooltipped\" data-position=\"top\"");
                    hcm.Append(" data-tooltip=\"Xem lịch sử giao dịch\"><i");
                    hcm.Append(" class=\"material-icons\">view_list</i></a>");

                    hcm.Append("<a href=\"javascript:;" + item.ID + "\" onclick =\"Delete(" + item.ID + ")\" class=\"tooltipped\" data-position=\"top\"");
                    hcm.Append(" data-tooltip=\"Xóa\"><i");
                    hcm.Append(" class=\"material-icons\">delete</i></a>");
                   
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

        protected void btnSearch_Click(object sender, EventArgs e)
        {
            string searchname = search_name.Text.Trim();
            string searchid = search_id.Text.Trim();
            string searchsdt = search_sdt.Text.Trim();
            if (string.IsNullOrEmpty(searchname) == true && searchid == "" && searchsdt == "")
            {
                Response.Redirect("userlist");
            }
            else
            {
                Response.Redirect("userlist?s=" + searchname + "&id=" + searchid + "&sdt=" + searchsdt + "");
            }
        }

        public void LoadPrefix()
        {
            var Levels = UserLevelController.GetAll("");
            if (Levels.Count > 0)
            {
                ddlLevelID.DataSource = Levels;
                ddlLevelID.DataBind();
            }
        }

        protected void btnDelete_Click(object sender, EventArgs e)
        {
            string username_current = Session["userLoginSystem"].ToString();
            int ID = hdfID.Value.ToInt(0);          
            var acc = AccountController.GetByUsername(username_current);
            if (acc.RoleID != 0)
            {
                PJUtils.ShowMessageBoxSwAlert("Bạn không có quyền duyệt yêu cầu này", "e", true, Page);
            }
            else
            {
                var kq = AccountController.DeleteUser(ID);
                if (Convert.ToInt32(kq) > 0)
                {
                    PJUtils.ShowMessageBoxSwAlert("Xóa khách hàng thành công.", "s", true, Page);
                }
                else
                {
                    PJUtils.ShowMessageBoxSwAlert("Có lỗi trong quá trình xử lý. Vui lòng thử lại.", "e", true, Page);
                }
            }           
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            if (!Page.IsValid) return;
            string Username = Session["userLoginSystem"].ToString();
            string Email = txtEmail.Text.Trim();
            string nickname = txtUsername.Text.Trim();
            //string ddlprefix = ddlPrefix.SelectedValue;
            int SaleID = ddlSale.SelectedValue.ToString().ToInt(0);
            int DathangID = ddlDathang.SelectedValue.ToString().ToInt(0);
            int LevelID = ddlLevelID.SelectedValue.ToString().ToInt();
            //int VIPLevel = ddlVipLevel.SelectedValue.ToString().ToInt();
            int VIPLevel = 0;
            var checkuser = AccountController.GetByUsername(nickname);
            var checkemail = AccountController.GetByEmail(Email);
            int RoleID = ddlRole.SelectedValue.ToString().ToInt();
            var getaccountinfor = AccountInfoController.GetByPhone(txtPhone.Text.Trim());
            if (checkuser != null)
            {
                PJUtils.ShowMessageBoxSwAlert("Tên đăng nhập / Nickname đã được sử dụng vui lòng chọn Tên đăng nhập / Nickname khác.", "e", false, Page);
            }
            else if (checkemail != null)
            {
                PJUtils.ShowMessageBoxSwAlert("Email đã được sử dụng vui lòng chọn Email khác.", "e", false, Page);
            }
            else if (getaccountinfor != null)
            {                
                PJUtils.ShowMessageBoxSwAlert("Số điện thoại đã được sử dụng vui lòng chọn Số điện thoại khác.", "e", false, Page);
            }
            else
            {
                string Token = PJUtils.RandomStringWithText(16);
                string id = AccountController.Insert(nickname, Email, txt_Password.Text.Trim(), RoleID, LevelID, VIPLevel, Convert.ToInt32(ddlStatus.SelectedValue),
                    SaleID, DathangID, DateTime.Now, Username, DateTime.Now, Username, Token);
                int UID = Convert.ToInt32(id);
                if (UID > 0)
                {
                    string kh = "KH" + PJUtils.RandomString(3) + UID;
                    AccountController.InsertMaKH(UID, kh, Username, DateTime.Now);
                    string idai = AccountInfoController.Insert(UID, txtFirstName.Text.Trim(), txtLastName.Text.Trim(), "", txtPhone.Text.Trim(), Email, txtPhone.Text.Trim(), "" , "", "",
                        DateTime.ParseExact(rBirthday.Text, "dd/MM/yyyy HH:mm", null), gender.Value.ToInt(1), DateTime.Now, "", DateTime.Now, "");
                    if (idai == "1")
                    {
                        PJUtils.ShowMsg("Tạo tài khoản thành công.", true, Page);
                    }
                }
            }
        }
    }
}