using MB.Extensions;
using Microsoft.AspNet.SignalR;
using NHST.Bussiness;
using NHST.Controllers;
using NHST.Hubs;
using NHST.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using ZLADIPJ.Business;

namespace NHST
{
    public partial class dang_ky2 : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                //loadPrefix();
                //LoadSEO();
                LoadDDL();
            }
        }
        public void LoadSEO()
        {
            var confi = ConfigurationController.GetByTop1();            
            var home = PageSEOController.GetByID(5);
            string weblink = "https://vantaihoakieu.com";
            string url = HttpContext.Current.Request.Url.AbsoluteUri;
            if (home != null)
            {
                HtmlHead objHeader = (HtmlHead)Page.Header;

                //we add meta description
                HtmlMeta objMetaFacebook = new HtmlMeta();

                objMetaFacebook = new HtmlMeta();
                objMetaFacebook.Attributes.Add("property", "fb:app_id");
                objMetaFacebook.Content = "676758839172144";
                objHeader.Controls.Add(objMetaFacebook);

                objMetaFacebook = new HtmlMeta();
                objMetaFacebook.Attributes.Add("property", "og:url");
                objMetaFacebook.Content = url;
                objHeader.Controls.Add(objMetaFacebook);

                objMetaFacebook = new HtmlMeta();
                objMetaFacebook.Attributes.Add("property", "og:type");
                objMetaFacebook.Content = "website";
                objHeader.Controls.Add(objMetaFacebook);

                objMetaFacebook = new HtmlMeta();
                objMetaFacebook.Attributes.Add("property", "og:title");
                objMetaFacebook.Content = home.ogtitle;
                objHeader.Controls.Add(objMetaFacebook);

                objMetaFacebook = new HtmlMeta();
                objMetaFacebook.Attributes.Add("property", "og:description");
                objMetaFacebook.Content = home.ogdescription;
                objHeader.Controls.Add(objMetaFacebook);

                objMetaFacebook = new HtmlMeta();
                objMetaFacebook.Attributes.Add("property", "og:image");
                if (string.IsNullOrEmpty(home.ogimage))
                    objMetaFacebook.Content = weblink + "/App_Themes/vantaihoakieu/images/logo.png";
                else
                    objMetaFacebook.Content = weblink + home.ogimage;
                objHeader.Controls.Add(objMetaFacebook);

                objMetaFacebook = new HtmlMeta();
                objMetaFacebook.Attributes.Add("property", "og:image:width");
                objMetaFacebook.Content = "200";
                objHeader.Controls.Add(objMetaFacebook);

                objMetaFacebook = new HtmlMeta();
                objMetaFacebook.Attributes.Add("property", "og:image:height");
                objMetaFacebook.Content = "500";
                objHeader.Controls.Add(objMetaFacebook);

                this.Title = home.metatitle;
                HtmlMeta meta = new HtmlMeta();
                meta = new HtmlMeta();
                meta.Attributes.Add("name", "description");
                meta.Content = home.metadescription;
                objHeader.Controls.Add(meta);

                meta = new HtmlMeta();
                meta.Attributes.Add("name", "keyword");
                meta.Content = home.metakeyword;
                objHeader.Controls.Add(meta);

            }
        }
       
        public void LoadDDL()
        {       
            ddlWarehouseFrom.Items.Clear();
            ddlWarehouseFrom.Items.Insert(0, "Chọn kho TQ");

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
            ListItem sleTT = new ListItem("Chọn kho nhận hàng", "0");
            ddlReceivePlace.Items.Insert(0, sleTT);

            var shipping = ShippingTypeToWareHouseController.GetAllWithIsHidden(false);
            if (shipping.Count > 0)
            {
                ddlShippingType.DataSource = shipping;
                ddlShippingType.DataBind();
            }  
        }
        protected void btncreateuser_Click(object sender, EventArgs e)
        {
            string Username = txtUsername.Text.Trim().ToLower();
            string Email = txtEmail.Text.Trim();
            var checkuser = AccountController.GetByUsername(Username);
            var checkemail = AccountController.GetByEmail(Email);
            string Phone = txtPhone.Text.Trim().Replace(" ", "");
            var getaccountinfor = AccountInfoController.GetByPhone(Phone);
            bool checkusernamebool = false;
            bool checkemailbool = false;
            bool checkphonebool = false;
            string error = "";
            bool check = PJUtils.CheckUnicode(Username);
            DateTime currentDate = DateTime.Now;
            DateTime bir = DateTime.Now;
            int nvkdID = 0;
            string nvkd = txtSalerUsername.Text;
            var setNoti = SendNotiEmailController.GetByID(1);

            if (!string.IsNullOrEmpty(nvkd))
            {
                var acckinhdoanh = AccountController.GetByUsername(nvkd);
                if (acckinhdoanh != null)
                {
                    nvkdID = acckinhdoanh.ID;
                }
            }
            if (!string.IsNullOrEmpty(rBirthday.Text.ToString()))
            {
                bir = DateTime.ParseExact(rBirthday.Text, "dd/MM/yyyy", null);
            }
            if (Username.Contains(" "))
            {             
                PJUtils.ShowMessageBoxSwAlert("Tên đăng nhập không được có dấu cách.", "e", false, Page);
            }
            else if (check == true)
            {              
                PJUtils.ShowMessageBoxSwAlert("Tên đăng nhập không được có dấu tiếng Việt.", "e", false, Page);
            }
            else
            {
                if (checkuser != null)
                {
                    //lblcheckemail.Visible = true;
                    error += "Tên đăng nhập / Nickname đã được sử dụng vui lòng chọn Tên đăng nhập / Nickname khác.<br/>";
                    checkusernamebool = true;
                }
                if (checkemail != null)
                {
                    //lblcheckemail.Visible = true;
                    error += "Email đã được sử dụng vui lòng chọn Email khác.<br/>";
                    checkemailbool = true;
                }
                if (getaccountinfor != null)
                {
                    //lblcheckemail.Visible = true;
                    error += "Số điện thoại đã được sử dụng vui lòng chọn Số điện thoại khác.<br/>";
                    checkphonebool = true;
                }
                if (checkusernamebool == false && checkemailbool == false && checkphonebool == false)
                {
                    string Token = PJUtils.RandomStringWithText(16);
                    string id = AccountController.Insert(Username, Email, txtpass.Text.Trim(), 1, 1, 1, 2, nvkdID, 0, DateTime.Now, "", DateTime.Now, "", Token);
                    if (Convert.ToInt32(id) > 0)
                    {                       
                        AccountController.updatewarehouseFromwarehouseTo(id.ToInt(0), ddlWarehouseFrom.SelectedValue.ToInt(0),
                            ddlReceivePlace.SelectedValue.ToInt(0));
                        AccountController.updateshipping(id.ToInt(0), ddlShippingType.SelectedValue.ToInt(0));
                        AccountController.UpdateScanWareHouse(id.ToInt(0), 0, 0);
                        int UID = Convert.ToInt32(id);
                        string kh = "KH" + PJUtils.RandomString(3) + id;
                        AccountController.InsertMaKH(UID, kh, Username, DateTime.Now);
                        string idai = AccountInfoController.Insert(UID, txtFirstName.Text.Trim(), txtLastName.Text.Trim(), "",
                            Phone, Email, txtPhone.Text.Trim(), txtAddress.Text, "", "", bir, ddlGender.SelectedValue.ToString().ToInt(1),
                            DateTime.Now, "", DateTime.Now, "");
                        if (idai == "1")
                        {
                            tbl_Account ac = AccountController.Login(Username, txtpass.Text.Trim());
                            if (ac != null)
                            {
                                var ai = AccountInfoController.GetByUserID(ac.ID);
                                if (ac.Status == 1)
                                {                                  
                                }
                                else if (ac.Status == 2)
                                {
                                    //Đã kích hoạt
                                    Session["userLoginSystem"] = ac.Username;
                                    if (setNoti != null)
                                    {
                                        if (setNoti.IsSentNotiAdmin == true)
                                        {
                                            var admins = AccountController.GetAllByRoleID(0);
                                            if (admins.Count > 0)
                                            {
                                                foreach (var admin in admins)
                                                {
                                                    NotificationsController.Inser(admin.ID,
                                                                                       admin.Username, 0,
                                                                                       "Khách hàng mới có username là: " + ac.Username,
                                                                                       6, currentDate, ac.Username, false);
                                                }
                                            }

                                            var managers = AccountController.GetAllByRoleID(2);
                                            if (managers.Count > 0)
                                            {
                                                foreach (var manager in managers)
                                                {
                                                    NotificationsController.Inser(manager.ID,
                                                                                       manager.Username, 0,
                                                                                       "Khách hàng mới có username là: " + ac.Username,
                                                                                       6, currentDate, ac.Username, false);
                                                }
                                            }
                                        }                                  
                                        if (setNoti.IsSentEmailAdmin == true)
                                        {
                                            var admins = AccountController.GetAllByRoleID(0);
                                            if (admins.Count > 0)
                                            {
                                                foreach (var admin in admins)
                                                {
                                                    try
                                                    {
                                                        PJUtils.SendMailGmail("no-reply@mona-media.com",
                                                            "demonhunter", admin.Email,
                                                            "Thông báo tại Taobao.vn.",
                                                            "Khách hàng mới có username là: " + ac.Username, "");
                                                    }
                                                    catch { }
                                                }
                                            }
                                            var managers = AccountController.GetAllByRoleID(2);
                                            if (managers.Count > 0)
                                            {
                                                foreach (var manager in managers)
                                                {
                                                    try
                                                    {
                                                        PJUtils.SendMailGmail("no-reply@mona-media.com", "demonhunter", manager.Email,
                                                            "Thông báo tại Taobao.vn.",
                                                            "Khách hàng mới có username là: " + ac.Username, "");
                                                    }
                                                    catch { }
                                                }
                                            }
                                        }
                                        if (setNoti.IsSendEmailUser == true)
                                        {
                                            try
                                            {
                                                PJUtils.SendMailGmail("no-reply@mona-media.com", "demonhunter", ac.Email,
                                                    "Thông báo tại Taobao.vn.",
                                                    "Chào mừng bạn đã đến với Taobao.vn.", "");
                                            }
                                            catch { }
                                        }
                                    }                                  
                                    var hubContext = GlobalHost.ConnectionManager.GetHubContext<ChatHub>();
                                    hubContext.Clients.All.addNewMessageToPage("", "");
                                    Response.Redirect("/thong-tin-nguoi-dung");
                                }
                                else if (ac.Status == 3)
                                {
                                    //Đã Block
                                }
                            }
                            else
                            {
                                //lblError.Text = "Đăng nhập không thành công, vui lòng kiểm tra lại.";
                                //lblError.Visible = true;
                            }
                            //Response.Redirect("/Login.aspx");
                        }
                    }
                }
                else
                {
                    PJUtils.ShowMessageBoxSwAlert(error, "e", false, Page);
                }
            }
        }
    }
}