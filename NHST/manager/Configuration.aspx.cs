﻿using NHST.Bussiness;
using NHST.Controllers;
using NHST.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Telerik.Web.UI;

namespace NHST.manager
{
    public partial class Configuration : System.Web.UI.Page
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
                    if (ac.RoleID != 0)
                        Response.Redirect("/trang-chu");
                }
                loaddata();
            }
        }
        public void loaddata()
        {
            var c = ConfigurationController.GetByTop1();
            if (c != null)
            {
                txtWebName.Text = c.Websitename;
                //if (c.LogoIMG != null)
                //    imgLogo.ImageUrl = c.LogoIMG;
                //if (c.BannerIMG != null)
                //    imgBannerIMG.ImageUrl = c.BannerIMG;
                //txtChromeExtensionLink.Text = c.ChromeExtensionLink;
                //txtCocCocExtensionLink.Text = c.CocCocExtensionLink;
                hdfAbout.Value = c.AboutText;
                hdfAddress1.Value = c.Address;
                hdfAddress2.Value = c.Address2;
                txtEmailSupport.Text = c.EmailSupport;
                txtEmailContact.Text = c.EmailContact;
                txtHotline.Text = c.Hotline;
                txtHotlineSupport.Text = c.HotlineSupport;
                txtHotlineFeedback.Text = c.HotlineFeedback;
                txtFacebook.Text = c.Facebook;
                txtTwitter.Text = c.Twitter;
                txtGooglePlus.Text = c.GooglePlus;
                txtPinterest.Text = c.Pinterest;
                txtInstagram.Text = c.Instagram;
                txtSkype.Text = c.Skype;
                //txtTimeWork.Text = c.TimeWork;
                txtWorkingTime.Text = c.TimeWork;
                txtCNY_VN.Text = c.Currency; ;
                txtBuyCYN.Text = c.CurrencyBuyCYN; 
                hdfFooterConfig.Value = c.FooterLeft;
                //rFooterPhai.Content = c.FooterRight;
                //pContent.Content = c.InfoContent;
                //rPercent.Value = Convert.ToDouble(c.PercentOrder);
                //rWeightPrice.Value = Convert.ToDouble(c.WeightPrice);
                //rCurrencyIncome.Value = Convert.ToDouble(c.CurrencyIncome);
                txtPriceDefault.Text = c.PricePayHelpDefault;
                //pPriceSendDefaultHN.Value = Convert.ToDouble(c.PriceSendDefaultHN);
                //pPriceSendDefaultSG.Value = Convert.ToDouble(c.PriceSendDefaultSG);
                txtPercentAfter3M.Text = c.SalePercentAfter3Month;
                txtPercentIn3M.Text = c.SalePercent;
                txtPercentStaffOrder.Text = c.DathangPercent;
                hdfContentNotificationPopup.Value = c.NotiPopup;
                txtTitleNotificationPopup.Text = c.NotiPopupTitle;
                txtTitleNotificattion.Text = c.NotiPopupEmail;
                txtLinkYoutube.Text = c.YoutubeLink;
                txtLinkZalo.Text = c.ZaloLink;
                txtLinkWeChat.Text = c.WechatLink;
                txtLinkTelegram.Text = c.Telegram;
                txtLinkMessenger.Text = c.Messenger;
                txtLinkMap.Text = c.Address3;
                txtMetaDescription.Text = c.MetaDescription;
                txtMetaKeyWord.Text = c.MetaKeyword;
                txtGoogleAna.Text = c.GoogleAnalytics;
                txtWebMaster.Text = c.WebmasterTools;
                txtNumberLinkOfOrder.Text = Convert.ToInt32(c.NumberLinkOfOrder).ToString();

                txtTitle.Text = c.MetaTitle;
                txtOGTitle.Text = c.OGTitle;
                txtOGDescription.Text = c.OGDescription;
                OGImageBefore.ImageUrl = c.OGImage;

                txtOGFBTitle.Text = c.OGFBTitle;
                txtOGFBDescription.Text = c.OGFBDescription;
                OGFbImageBefore.ImageUrl = c.OGFBImage;

                txtTwitterTitle.Text = c.OGTwitterTitle;
                txtTwitterDescription.Text = c.OGTwitterDescription;
                OGTwitterImageBefore.ImageUrl = c.OGTwitterImage;
                txtInsurancePercent.Text = c.InsurancePercent;

                txtCompanyName.Text = c.CompanyName;
                txtCompanyLongName.Text = c.CompanyLongName;
                txtCompanyShortName.Text = c.CompanyShortName;
                txtTaxCode.Text = c.TaxCode;
                imgLogoBefore.ImageUrl = c.LogoIMG;

                rAgentCurrency.Text = c.AgentCurrency.ToString();
                rPriceCheckOutWareDefault.Text = c.PriceCheckOutWareDefault.ToString();
            }
        }        

        protected void btnUpdate_Click(object sender, EventArgs e)
        {
            if (!Page.IsValid) return;
            var c = ConfigurationController.GetByTop1();
            if (c != null)
            {
                string IMG = "";               
                if (imgLogo.PostedFiles.Count > 0)
                {
                    foreach (HttpPostedFile f in imgLogo.PostedFiles)
                    {
                        string fileContentType = f.ContentType; // getting ContentType

                        byte[] tempFileBytes = new byte[f.ContentLength];

                        var data = f.InputStream.Read(tempFileBytes, 0, Convert.ToInt32(f.ContentLength));

                        string fileName = f.FileName; // getting File Name
                        string fileExtension = Path.GetExtension(fileName).ToLower();

                        var result = FileUploadCheck.isValidFile(tempFileBytes, fileExtension, fileContentType); // Validate Header
                        if (result)
                        {
                            if (f.FileName.ToLower().Contains(".jpg") || f.FileName.ToLower().Contains(".png") || f.FileName.ToLower().Contains(".jpeg"))
                            {
                                if (f.ContentType == "image/png" || f.ContentType == "image/jpeg" || f.ContentType == "image/jpg")
                                {
                                    //var o = KhieuNaiIMG1 + Guid.NewGuid() + Path.GetExtension(f.FileName);
                                    try
                                    {
                                        //f.SaveAs(Server.MapPath(o));
                                        IMG = FileUploadCheck.ConvertToBase64(tempFileBytes);
                                    }
                                    catch { }
                                }
                            }
                        }
                        else
                            IMG = imgLogoBefore.ImageUrl;
                    }
                }
                else
                    IMG = imgLogoBefore.ImageUrl;

                string IMG1 = "";               
                if (OGImage.PostedFiles.Count > 0)
                {
                    foreach (HttpPostedFile f in OGImage.PostedFiles)
                    {
                        string fileContentType = f.ContentType; // getting ContentType

                        byte[] tempFileBytes = new byte[f.ContentLength];

                        var data = f.InputStream.Read(tempFileBytes, 0, Convert.ToInt32(f.ContentLength));

                        string fileName = f.FileName; // getting File Name
                        string fileExtension = Path.GetExtension(fileName).ToLower();

                        var result = FileUploadCheck.isValidFile(tempFileBytes, fileExtension, fileContentType); // Validate Header
                        if (result)
                        {
                            if (f.FileName.ToLower().Contains(".jpg") || f.FileName.ToLower().Contains(".png") || f.FileName.ToLower().Contains(".jpeg"))
                            {
                                if (f.ContentType == "image/png" || f.ContentType == "image/jpeg" || f.ContentType == "image/jpg")
                                {
                                    //var o = KhieuNaiIMG1 + Guid.NewGuid() + Path.GetExtension(f.FileName);
                                    try
                                    {
                                        //f.SaveAs(Server.MapPath(o));
                                        IMG1 = FileUploadCheck.ConvertToBase64(tempFileBytes);
                                    }
                                    catch { }
                                }
                            }
                        }
                        else
                            IMG1 = OGImageBefore.ImageUrl;
                    }
                }
                else
                    IMG1 = OGImageBefore.ImageUrl;

                string IMGFacebook = "";               
                if (OGFbImage.PostedFiles.Count > 0)
                {
                    foreach (HttpPostedFile f in OGFbImage.PostedFiles)
                    {
                        string fileContentType = f.ContentType; // getting ContentType

                        byte[] tempFileBytes = new byte[f.ContentLength];

                        var data = f.InputStream.Read(tempFileBytes, 0, Convert.ToInt32(f.ContentLength));

                        string fileName = f.FileName; // getting File Name
                        string fileExtension = Path.GetExtension(fileName).ToLower();

                        var result = FileUploadCheck.isValidFile(tempFileBytes, fileExtension, fileContentType); // Validate Header
                        if (result)
                        {
                            if (f.FileName.ToLower().Contains(".jpg") || f.FileName.ToLower().Contains(".png") || f.FileName.ToLower().Contains(".jpeg"))
                            {
                                if (f.ContentType == "image/png" || f.ContentType == "image/jpeg" || f.ContentType == "image/jpg")
                                {
                                    //var o = KhieuNaiIMGFacebook + Guid.NewGuid() + Path.GetExtension(f.FileName);
                                    try
                                    {
                                        //f.SaveAs(Server.MapPath(o));
                                        IMGFacebook = FileUploadCheck.ConvertToBase64(tempFileBytes);
                                    }
                                    catch { }
                                }
                            }
                        }
                        else
                            IMGFacebook = OGFbImageBefore.ImageUrl;
                    }
                }
                else
                    IMGFacebook = OGFbImageBefore.ImageUrl;

                string IMGTwitter = "";               
                if (OGTwitterImage.PostedFiles.Count > 0)
                {
                    foreach (HttpPostedFile f in OGTwitterImage.PostedFiles)
                    {
                        string fileContentType = f.ContentType; // getting ContentType

                        byte[] tempFileBytes = new byte[f.ContentLength];

                        var data = f.InputStream.Read(tempFileBytes, 0, Convert.ToInt32(f.ContentLength));

                        string fileName = f.FileName; // getting File Name
                        string fileExtension = Path.GetExtension(fileName).ToLower();

                        var result = FileUploadCheck.isValidFile(tempFileBytes, fileExtension, fileContentType); // Validate Header
                        if (result)
                        {
                            if (f.FileName.ToLower().Contains(".jpg") || f.FileName.ToLower().Contains(".png") || f.FileName.ToLower().Contains(".jpeg"))
                            {
                                if (f.ContentType == "image/png" || f.ContentType == "image/jpeg" || f.ContentType == "image/jpg")
                                {
                                    // var o = KhieuNaiIMGTwitter + Guid.NewGuid() + Path.GetExtension(f.FileName);
                                    try
                                    {
                                        //f.SaveAs(Server.MapPath(o));
                                        IMGTwitter = FileUploadCheck.ConvertToBase64(tempFileBytes);
                                    }
                                    catch { }
                                }
                            }
                        }
                        else
                            IMGTwitter = OGTwitterImageBefore.ImageUrl;
                    }
                }
                else
                    IMGTwitter = OGTwitterImageBefore.ImageUrl;


                var kq = ConfigurationController.UpdateVer2(c.ID, txtWebName.Text, txtEmailSupport.Text, txtEmailContact.Text, txtHotline.Text, hdfAddress1.Value.ToString(), txtFacebook.Text, txtTwitter.Text, txtGooglePlus.Text, txtInstagram.Text, txtSkype.Text,
                    txtWorkingTime.Text, txtCNY_VN.Text, txtBuyCYN.Text, txtPriceDefault.Text, hdfAbout.Value.ToString(), hdfAddress2.Value.ToString(), hdfFooterConfig.Value.ToString(), txtPercentAfter3M.Text, txtPercentIn3M.Text, txtPercentStaffOrder.Text,
                    txtHotlineSupport.Text, txtHotlineFeedback.Text, txtPinterest.Text, hdfContentNotificationPopup.Value.ToString(), txtTitleNotificationPopup.Text, txtTitleNotificattion.Text, txtLinkYoutube.Text, txtLinkZalo.Text, txtLinkWeChat.Text,txtLinkTelegram.Text, txtLinkMessenger.Text, txtLinkMap.Text, txtMetaKeyWord.Text, txtMetaDescription.Text, txtGoogleAna.Text, txtWebMaster.Text, Convert.ToInt32(txtNumberLinkOfOrder.Text), txtTitle.Text, txtOGTitle.Text,
                    txtOGDescription.Text, IMG1, txtOGFBTitle.Text, txtOGFBDescription.Text, IMGFacebook, txtTwitterTitle.Text, txtTwitterDescription.Text, IMGTwitter, txtInsurancePercent.Text, txtHeaderScript.Text, txtFooterScript.Text, txtCompanyShortName.Text, txtCompanyLongName.Text, txtCompanyName.Text, IMG, txtTaxCode.Text, rAgentCurrency.Text, rPriceCheckOutWareDefault.Text);
                if (kq == "ok")
                    PJUtils.ShowMsg("Cập nhật thiết lập thành công.", true, Page);
            }
        }

    }
}