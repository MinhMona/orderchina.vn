<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="quen-mat-khau.aspx.cs" Inherits="NHST.quen_mat_khau2" %>

<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>

<!DOCTYPE html>
<html class="loading" lang="en" data-textdirection="ltr">
<!-- BEGIN: Head-->
<head>
    <meta http-equiv="Content-Type" content="text/html; charset=UTF-8">
    <meta http-equiv="X-UA-Compatible" content="IE=edge">
    <meta name="viewport" content="width=device-width, initial-scale=1.0, user-scalable=0, minimal-ui">
    <meta name="description" content="Materialize is a Material Design Admin Template,It's modern, responsive and based on Material Design by Google.">
    <meta name="keywords" content="materialize, admin template, dashboard template, flat admin template, responsive admin template, eCommerce dashboard, analytic dashboard">
    <meta name="author" content="ThemeSelect">
    <title>Quên mật khẩu</title>
    <link rel="apple-touch-icon" href="/App_Themes/AdminNew45/assets/images/favicon/apple-touch-icon-152x152.png">
    <link rel="shortcut icon" type="image/x-icon" href="App_Themes/AdminNew45/assets/images/favicon/favicon-32x32.icon">
    <link href="https://fonts.googleapis.com/icon?family=Material+Icons" rel="stylesheet">
    <!-- BEGIN: VENDOR CSS-->
    <link rel="stylesheet" type="text/css" href="/App_Themes/AdminNew45/assets/vendors/vendors.min.css">
    <!-- END: VENDOR CSS-->
    <!-- BEGIN: Page Level CSS-->
    <link rel="stylesheet" type="text/css" href="/App_Themes/AdminNew45/assets/css/materialize.css">
    <link rel="stylesheet" type="text/css" href="/App_Themes/AdminNew45/assets/css/style.css">
    <link rel="stylesheet" type="text/css" href="/App_Themes/AdminNew45/assets/css/pages/login.css">
    <!-- END: Page Level CSS-->
    <!-- BEGIN: Custom CSS-->
    <link rel="stylesheet" type="text/css" href="/App_Themes/AdminNew45/assets/css/custom/custom.css">
    <link href="/App_Themes/NewUI/js/sweet/sweet-alert.css" rel="stylesheet" type="text/css" />
    <!-- END: Custom CSS-->
</head>
<!-- END: Head-->
<body class="horizontal-layout page-header-light horizontal-menu 1-column login-bg  blank-page blank-page" data-open="click" data-menu="horizontal-menu" data-col="1-column">
    <div class="row">
        <div class="col s12">
            <div class="container">
                <div id="login-page" class="row">
                    <div class="col s12 m6 l4 z-depth-4 card-panel border-radius-6 login-card bg-opacity-8">
                        <form runat="server" class="login-form">
                            <asp:ScriptManager runat="server" ID="scr">
                            </asp:ScriptManager>
                            <asp:Literal ID="ltrHotlineCall" runat="server"></asp:Literal>
                            <%--<div id="fb-root"></div>
                            <script>(function (d, s, id) {
                                    var js, fjs = d.getElementsByTagName(s)[0];
                                    if (d.getElementById(id)) return;
                                    js = d.createElement(s); js.id = id;
                                    js.src = 'https://connect.facebook.net/vi_VN/sdk.js#xfbml=1&version=v3.2&appId=1916689885307106&autoLogAppEvents=1';
                                    fjs.parentNode.insertBefore(js, fjs);
                                }(document, 'script', 'facebook-jssdk'));
                            </script>
                            <div id="cfacebook">
                                <a href="javascript:;" class="chat_fb" onclick="return:false;"><i class="fa fa-facebook-square"></i>Phản hồi của bạn</a>
                                <div class="fchat">
                                    <div class="fb-page" data-tabs="messages" data-href="https://m.facebook.com/V%E1%BA%ADn-t%E1%BA%A3i-Hoa-Ki%E1%BB%81u-398069500742691/" data-width="250" data-height="400" data-small-header="false" data-adapt-container-width="true" data-hide-cover="false" data-show-facepile="true" data-show-posts="false"></div>
                                </div>
                            </div>
                            <style>
                                #cfacebook {
                                    position: fixed;
                                    bottom: 0px;
                                    right: 20px;
                                    z-index: 999999999999999;
                                    width: 250px;
                                    height: auto;
                                    box-shadow: 6px 6px 6px 10px rgba(0,0,0,0.2);
                                    border-top-left-radius: 5px;
                                    border-top-right-radius: 5px;
                                    overflow: hidden;
                                }

                                    #cfacebook .fchat {
                                        float: left;
                                        width: 100%;
                                        height: 270px;
                                        overflow: hidden;
                                        display: none;
                                        background-color: #fff;
                                    }

                                        #cfacebook .fchat .fb-page {
                                            margin-top: -130px;
                                            float: left;
                                        }

                                    #cfacebook a.chat_fb {
                                        float: left;
                                        padding: 0 25px;
                                        width: 250px;
                                        color: #fff;
                                        text-decoration: none;
                                        height: 40px;
                                        line-height: 40px;
                                        text-shadow: 0 1px 0 rgba(0,0,0,0.1);
                                        background-image: url(data:image/png;base64,iVBORw0KGgoAAAANSUhEUgAAAAEAAAAqCAMAAABFoMFOAAAAWlBMV…8/UxBxQDQuFwlpqgBZBq6+P+unVY1GnDgwqbD2zGz5e1lBdwvGGPE6OgAAAABJRU5ErkJggg==);
                                        background-repeat: repeat-x;
                                        background-size: auto;
                                        background-position: 0 0;
                                        background-color: #3a5795;
                                        border: 0;
                                        border-bottom: 1px solid #133783;
                                        z-index: 9999999;
                                        margin-right: 12px;
                                        font-size: 18px;
                                    }

                                        #cfacebook a.chat_fb:hover {
                                            color: yellow;
                                            text-decoration: none;
                                        }

                                .zalo-icon {
                                    width: auto !important;
                                    height: auto !important;
                                }
                            </style>
                            <script>
                                jQuery(document).ready(function () {
                                    jQuery(".chat_fb").click(function () {
                                        jQuery('.fchat').toggle('slow');
                                    });
                                });
                            </script>--%>
                            <div class="row">
                                <div class="input-field col s12 align-center">
                                    <span class="img logo-login">
                                        <a href="/"><img src="/App_Themes/VietTrung/images/logo-icon.png" alt="logo"></a></span>

                                </div>
                            </div>
                            <div class="row">
                                <div class="input-field col s12 mb-0">
                                    <h5 class="">Khôi phục mật khẩu</h5>
                                    <p class="">Nhập email của tài khoản</p>
                                </div>
                            </div>
                            <div class="row">
                                <div class="input-field col s12 mt-0">
                                    <i class="material-icons prefix">email</i>
                                    <asp:TextBox runat="server" placeholder="" ID="txtEmail" type="email"></asp:TextBox>
                                    <span class="help-text">
                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator3" runat="server" Display="Dynamic" ControlToValidate="txtEmail" ForeColor="Red" ErrorMessage="Không được để trống."></asp:RequiredFieldValidator>
                                    </span>
                                    <div class="clearfix"></div>
                                    <span class="help-text">
                                        <asp:RegularExpressionValidator runat="server" ID="RegularExpressionValidator2" Display="Dynamic" ControlToValidate="txtEmail"
                                            ErrorMessage="Sai định dạng Email" ValidationExpression="^\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*$" SetFocusOnError="true"
                                            ForeColor="Red" />
                                    </span>
                                </div>
                            </div>
                            <div class="row">
                                <div class="input-field col s12">
                                    <a onclick="GetPass()" class="btn waves-effect waves-light border-round bg-dark-gradient col s12 mb-1">Khôi phục mật khẩu</a>
                                </div>
                            </div>
                            <div class="row">
                                <div class="input-field col s6 m6 l6">
                                    <p class="margin medium-small"><a href="dang-nhap">Đăng nhập</a></p>
                                </div>
                                <div class="input-field col s6 m6 l6">
                                    <p class="margin right-align medium-small"><a href="dang-ky">Đăng ký</a></p>
                                </div>
                            </div>
                            <asp:Button ID="btngetpass" runat="server" Text="Gửi yêu cầu" Style="display: none" UseSubmitBehavior="false"
                                OnClick="btngetpass_Click" />
                        </form>
                    </div>
                </div>
            </div>
        </div>
    </div>
    <script>
        function GetPass() {
            $('#<%=btngetpass.ClientID%>').click();
        }
    </script>
    <!-- BEGIN VENDOR JS-->
    <script src="/App_Themes/AdminNew45/assets/js/vendors.min.js" type="text/javascript"></script>
    <!-- BEGIN VENDOR JS-->
    <!-- BEGIN PAGE VENDOR JS-->
    <!-- END PAGE VENDOR JS-->
    <!-- BEGIN THEME  JS-->
    <script src="/App_Themes/AdminNew45/assets/js/plugins.js" type="text/javascript"></script>
    <script src="/App_Themes/NewUI/js/sweet/sweet-alert.js" type="text/javascript"></script>
    <!-- END THEME  JS-->
    <!-- BEGIN PAGE LEVEL JS-->
    <!-- END PAGE LEVEL JS-->
</body>
</html>
