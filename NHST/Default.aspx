<%@ Page Language="C#" MasterPageFile="~/DatHangTrungQuoc.Master" AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="NHST.Default2" %>

<asp:Content runat="server" ContentPlaceHolderID="head"></asp:Content>
<asp:Content runat="server" ContentPlaceHolderID="ContentPlaceHolder1">
    <main class="main">
        <div class="banner-section sec" style="background-image: url(/App_Themes/VietTrung/images/banner.png);">
            <div class="container wow zoomIn">
                <h1 class="banner-title">CHÚNG TÔI PHỤ VỤ HƠN <span class="fw-500 hl-color">5000+</span> KHÁCH HÀNG</h1>
                <div class="search-tracking-wrapper tab-wrapper">
                    <ul class="search-tracking-nav">
                        <li class="tab-link current" data-tab="#search">
                            <span class="checkmark"></span>
                            Tìm kiếm sản phẩm
                        </li>
                        <li class="tab-link" data-tab="#tracking">
                            <span class="checkmark"></span>
                            Tracking
                        </li>
                    </ul>
                    <div class="tab-content-wrapper">
                        <div id="search" class="tab-content">
                            <div class="select-form">
                                <select class="f-control" id="brand-source">
                                    <option value="taobao" data-image="/App_Themes/VietTrung/images/place-img.png"></option>
                                    <option value="1688" data-image="/App_Themes/VietTrung/images/place-img-2.png"></option>
                                    <option value="tmall" data-image="/App_Themes/VietTrung/images/place-img-4.png"></option>
                                </select>
                                <span class="icon">
                                    <i class="fa fa-angle-down" aria-hidden="true"></i>
                                </span>
                            </div>
                            <asp:TextBox runat="server" ID="txtSearch" CssClass="input f-control txtsearchfield" placeholder="Tìm kiếm sản phẩm"></asp:TextBox>
                            <a href="javascript:;" onclick="searchProduct()" class="main-btn">Tìm kiếm</a>
                            <asp:Button ID="btnSearch" runat="server"
                                OnClick="btnSearch_Click" Style="display: none"
                                OnClientClick="document.forms[0].target = '_blank';" UseSubmitBehavior="false" />
                        </div>
                        <div id="tracking" class="search-form tab-content">
                            <input id="txtMVD" class="f-control" type="text" placeholder="Nhập mã vận đơn">
                            <a href="javascript:;" onclick="searchCode()" class="main-btn">Tìm kiếm</a>
                        </div>
                    </div>
                    <p class="search-tracking-note">
                        (Nếu bạn chưa tìm thấy sản phẩm mong muốn vui lòng Click vào đây gửi
                        yêu cầu từ khóa)
                    </p>
                </div>
                <div class="banner-place-img">
                    <img src="/App_Themes/VietTrung/images/place-img.png" alt="">
                    <img src="/App_Themes/VietTrung/images/place-img-2.png" alt="">
                    <img src="/App_Themes/VietTrung/images/place-img-3.png" alt="">
                    <img src="/App_Themes/VietTrung/images/place-img-4.png" alt="">
                </div>
            </div>
            <div class="why-choose-us wow fadeIn" data-wow-delay="1s">
                <div class="reason">
                    <div class="icon">
                        <img src="/App_Themes/VietTrung/images/icon-1.png" alt="">
                    </div>
                    <p class="text">
                        2 ngày<br>
                        hàng về
                    </p>
                </div>
                <div class="reason">
                    <div class="icon">
                        <img src="/App_Themes/VietTrung/images/icon-2.png" alt="">
                    </div>
                    <p class="text">
                        Bảo hiểm<br>
                        100%
                    </p>
                </div>
                <div class="reason">
                    <div class="icon">
                        <img src="/App_Themes/VietTrung/images/icon-3.png" alt="">
                    </div>
                    <p class="text">
                        Đặt hàng<br>
                        chỉ 2h
                    </p>
                </div>
                <div class="reason">
                    <div class="icon">
                        <img src="/App_Themes/VietTrung/images/icon-4.png" alt="">
                    </div>
                    <p class="text">
                        Tỉ giá<br>
                        thấp nhất
                    </p>
                </div>
                <div class="reason">
                    <div class="icon">
                        <img src="/App_Themes/VietTrung/images/icon-5.png" alt="">
                    </div>
                    <p class="text">
                        Tư vấn<br>
                        24/7
                    </p>
                </div>
                <div class="reason">
                    <div class="icon">
                        <img src="/App_Themes/VietTrung/images/icon-6.png" alt="">
                    </div>
                    <p class="text">
                        Kinh nghiệm<br>
                        12 năm
                    </p>
                </div>
                <div class="reason">
                    <div class="icon">
                        <img src="/App_Themes/VietTrung/images/icon-7.png" alt="">
                    </div>
                    <p class="text">
                        Vận chuyển<br>
                        siêu rẻ
                    </p>
                </div>
                <div class="reason">
                    <div class="icon">
                        <img src="/App_Themes/VietTrung/images/icon-8.png" alt="">
                    </div>
                    <p class="text">
                        Free phí<br>
                        dịch vụ
                    </p>
                </div>
            </div>
        </div>
        <div class="step-section sec">
            <div class="container wow fadeInUp">
                <div class="columns">
                    <asp:Literal runat="server" ID="ltrStep"></asp:Literal>                   
                </div>
            </div>
        </div>
        <section class="tools-section sec">
            <div class="container">
                <div class="main-title">
                    <h2 class="title">CÔNG CỤ TIỆN ÍCH</h2>
                    <div class="decor"></div>
                </div>
                <h3 class="sub-title">Công cụ phần mềm phục vụ việc đặt hằng, quản lý đơn hàng dễ dàng mọi lúc mọi nơi</h3>
                <div class="columns">
                    <div class="column wow fadeInLeft">
                        <div class="box-img">
                            <img src="/App_Themes/VietTrung/images/desc.png" alt="">
                        </div>
                    </div>
                    <div class="column wow fadeInRight">
                        <div class="setting-tools-wrapper">
                            <div class="setting-tools">
                                <p class="title">Công cụ đặt hàng trên máy tính</p>
                                <div class="extension-btn-wrapper">
                                    <a href="https://chrome.google.com/webstore/detail/gemdnehajmjdpgnphalongcchejcchko" class="extension-btn" target="_blank"><span><i class="fa fa-chrome"
                                        aria-hidden="true"></i>CHORME</span></a>
                                    <a href="https://chrome.google.com/webstore/detail/gemdnehajmjdpgnphalongcchejcchko" class="extension-btn" target="_blank"><span><i class="fa fa-chrome"
                                        aria-hidden="true"></i>CỐC CỐC</span></a>
                                </div>
                            </div>
                            <div class="setting-tools">
                                <p class="title">App quản lý mobile</p>
                                <div class="extension-btn-wrapper">
                                    <a href="https://apps.apple.com/us/app/id1495743521" class="extension-btn" target="_blank"><span><i class="fa fa-apple"
                                        aria-hidden="true"></i>IOS</span></a>
                                    <a href="https://play.google.com/store/apps/details?id=com.appteamvn.DatHangTrungQuoc&hl=en" class="extension-btn" target="_blank"><span><i class="fa fa-android"
                                        aria-hidden="true"></i>ANDROID</span></a>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </section>
        <section class="payment-info-section sec">
            <div class="container">
                <div class="main-title">
                    <h2 class="title">THÔNG TIN THANH TOÁN</h2>
                    <div class="decor"></div>
                </div>
                <div class="payment-info-list">
                    <div class="columns">
                        <asp:Literal runat="server" ID="ltrBank"></asp:Literal>
                    </div>
                </div>
            </div>
        </section>
        <section class="services-section sec">
            <div class="container">
                <div class="main-title">
                    <h2 class="title">DỊCH VỤ</h2>
                    <div class="decor"></div>
                </div>
                <div class="services-tab-wrapper tab-wrapper wow lightSpeedIn">
                    <ul class="services-tab-nav">
                        <asp:Literal runat="server" ID="ltrService1"></asp:Literal>
                    </ul>
                    <div class="services-tab-content-wrapper" style="background-image: url(/App_Themes/VietTrung/images/services-tab-bg.jpg);">
                        <asp:Literal runat="server" ID="ltrService2"></asp:Literal>
                    </div>
                </div>
            </div>
        </section>
        <section class="benefits-section sec">
            <div class="container">
                <div class="main-title">
                    <h2 class="title">QUYỀN LỢI</h2>
                    <div class="decor"></div>
                </div>
                <div class="benefit-content-wrapper">
                    <div class="benefit-list">
                        <asp:Literal runat="server" ID="ltrQuyenLoi"></asp:Literal>                       
                    </div>
                    <div class="benefit-bg wow zoomIn" data-wow-duration="1s" data-wow-delay="0s">
                        <img src="/App_Themes/VietTrung/images/benefits-bg.png" alt="">
                    </div>
                </div>
            </div>
        </section>
    </main>
    <asp:HiddenField ID="hdfWebsearch" runat="server" />
    <script type="text/javascript">
        $(document).ready(function () {
            $('.txtsearchfield').on("keypress", function (e) {
                if (e.keyCode == 13) {
                    searchProduct();
                }
            });
        });

        function searchProduct() {
            var web = $("#brand-source").val();
            $("#<%=hdfWebsearch.ClientID%>").val(web);
            $("#<%=btnSearch.ClientID%>").click();
        }
    </script>
    <script type="text/javascript">
        function keyclose_ms(e) {
            if (e.keyCode == 27) {
                close_popup_ms();
            }
        }
        function close_popup_ms() {
            $("#pupip_home").animate({ "opacity": 0 }, 400);
            $("#bg_popup_home").animate({ "opacity": 0 }, 400);
            setTimeout(function () {
                $("#pupip_home").remove();
                $(".zoomContainer").remove();
                $("#bg_popup_home").remove();
                $('body').css('overflow', 'auto').attr('onkeydown', '');
            }, 500);
        }
        function closeandnotshow() {
            $.ajax({
                type: "POST",
                url: "/Default.aspx/setNotshow",
                //data: "{ID:'" + id + "',UserName:'" + username + "'}",
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: function (msg) {
                    close_popup_ms();
                },
                error: function (xmlhttprequest, textstatus, errorthrow) {
                    alert('lỗi');
                }
            });

        }
        $(document).ready(function () {
            $.ajax({
                type: "POST",
                url: "/Default.aspx/getPopup",
                //data: "{ID:'" + id + "',UserName:'" + username + "'}",
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: function (msg) {
                    if (msg.d != "null") {
                        var data = JSON.parse(msg.d);
                        var title = data.NotiTitle;
                        var content = data.NotiContent;
                        var email = data.NotiEmail;
                        var obj = $('form');
                        $(obj).css('overflow', 'hidden');
                        $(obj).attr('onkeydown', 'keyclose_ms(event)');
                        var bg = "<div id='bg_popup_home'></div>";
                        var fr = "<div id='pupip_home' class=\"columns-container1\">" +
                            "  <div class=\"center_column col-xs-12 col-sm-5\" id=\"popup_content_home\">";
                        fr += "<div class=\"popup_header\">";
                        fr += title;
                        fr += "<a style='cursor:pointer;right:5px;' onclick='close_popup_ms()' class='close_message'></a>";
                        fr += "</div>";
                        fr += "     <div class=\"changeavatar\">";
                        fr += "         <div class=\"content1\">";
                        fr += content;
                        fr += "         </div>";
                        fr += "         <div class=\"content2\">";
                        fr += "<a href=\"javascript:;\" class=\"btn btn-close-full\" onclick='closeandnotshow()'>Đóng & không hiện thông báo</a>";
                        fr += "<a href=\"javascript:;\" class=\"btn btn-close\" onclick='close_popup_ms()'>Đóng</a>";
                        fr += "         </div>";
                        fr += "     </div>";
                        fr += "<div class=\"popup_footer\">";
                        fr += "<span class=\"float-right\">" + email + "</span>";
                        fr += "</div>";
                        fr += "   </div>";
                        fr += "</div>";
                        $(bg).appendTo($(obj)).show().animate({ "opacity": 0.7 }, 800);
                        $(fr).appendTo($(obj));
                        setTimeout(function () {
                            $('#pupip').show().animate({ "opacity": 1, "top": 20 + "%" }, 200);
                            $("#bg_popup").attr("onclick", "close_popup_ms()");
                        }, 1000);
                    }
                },
                error: function (xmlhttprequest, textstatus, errorthrow) {
                    alert('lỗi');
                }
            });
        });

        function searchCode() {
            var code = $("#txtMVD").val();
            if (isEmpty(code)) {
                alert('Vui lòng nhập mã vận đơn.');
            }
            else {
                $.ajax({
                    type: "POST",
                    url: "/Default.aspx/getInfo",
                    data: "{ordecode:'" + code + "'}",
                    contentType: "application/json; charset=utf-8",
                    dataType: "json",
                    success: function (msg) {
                        if (msg.d != "null") {
                            //var data = JSON.parse(msg.d);
                            var title = "Thông tin mã vận đơn";
                            var content = msg.d;
                            var email = "";
                            var obj = $('form');
                            $(obj).css('overflow', 'hidden');
                            $(obj).attr('onkeydown', 'keyclose_ms(event)');
                            var bg = "<div id='bg_popup_home'></div>";
                            var fr = "<div id='pupip_home' class=\"columns-container1\">" +
                                "  <div class=\"center_column col-xs-12 col-sm-5\" id=\"popup_content_home\">";
                            fr += "<div class=\"popup_header\">";
                            fr += title;
                            fr += "<a style='cursor:pointer;right:5px;' onclick='close_popup_ms()' class='close_message'></a>";
                            fr += "</div>";
                            fr += "     <div class=\"changeavatar\">";
                            fr += "         <div class=\"content1\" style=\"width:75%;margin-left:11%\">";
                            fr += content;
                            fr += "         </div>";
                            fr += "         <div class=\"content2\">";
                            fr += "             <a href=\"javascript:;\" class=\"btn btn-close\" onclick='close_popup_ms()'>Đóng</a>";
                            fr += "         </div>";
                            fr += "     </div>";
                            fr += "<div class=\"popup_footer\">";
                            fr += "<span class=\"float-right\">" + email + "</span>";
                            fr += "</div>";
                            fr += "   </div>";
                            fr += "</div>";
                            $(bg).appendTo($(obj)).show().animate({ "opacity": 0.7 }, 800);
                            $(fr).appendTo($(obj));
                            setTimeout(function () {
                                $('#pupip').show().animate({ "opacity": 1, "top": 20 + "%" }, 200);
                                $("#bg_popup").attr("onclick", "close_popup_ms()");
                            }, 1000);
                        }
                    },
                    error: function (xmlhttprequest, textstatus, errorthrow) {
                        alert('lỗi');
                    }
                });
            }

        }
    </script>
    <style>
        #bg_popup_home {
            position: fixed;
            width: 100%;
            height: 100%;
            background-color: #333;
            opacity: 0.7;
            filter: alpha(opacity=70);
            left: 0px;
            top: 0px;
            z-index: 999999999;
            opacity: 0;
            filter: alpha(opacity=0);
        }

        #popup_ms_home {
            background: #fff;
            border-radius: 0px;
            box-shadow: 0px 2px 10px #fff;
            float: left;
            position: fixed;
            width: 735px;
            z-index: 10000;
            left: 50%;
            margin-left: -370px;
            top: 200px;
            opacity: 0;
            filter: alpha(opacity=0);
            height: 360px;
        }

            #popup_ms_home .popup_body {
                border-radius: 0px;
                float: left;
                position: relative;
                width: 735px;
            }

            #popup_ms_home .content {
                /*background-color: #487175;     border-radius: 10px;*/
                margin: 12px;
                padding: 15px;
                float: left;
            }

            #popup_ms_home .title_popup {
                /*background: url("../images/img_giaoduc1.png") no-repeat scroll -200px 0 rgba(0, 0, 0, 0);*/
                color: #ffffff;
                font-family: Arial;
                font-size: 24px;
                font-weight: bold;
                height: 35px;
                margin-left: 0;
                margin-top: -5px;
                padding-left: 40px;
                padding-top: 0;
                text-align: center;
            }

            #popup_ms_home .text_popup {
                color: #fff;
                font-size: 14px;
                margin-top: 20px;
                margin-bottom: 20px;
                line-height: 20px;
            }

                #popup_ms_home .text_popup a.quen_mk, #popup_ms_home .text_popup a.dangky {
                    color: #FFFFFF;
                    display: block;
                    float: left;
                    font-style: italic;
                    list-style: -moz-hangul outside none;
                    margin-bottom: 5px;
                    margin-left: 110px;
                    -webkit-transition-duration: 0.3s;
                    -moz-transition-duration: 0.3s;
                    transition-duration: 0.3s;
                }

                    #popup_ms_home .text_popup a.quen_mk:hover, #popup_ms_home .text_popup a.dangky:hover {
                        color: #8cd8fd;
                    }

            #popup_ms_home .close_popup {
                background: url("/App_Themes/Camthach/images/close_button.png") no-repeat scroll 0 0 rgba(0, 0, 0, 0);
                display: block;
                height: 28px;
                position: absolute;
                right: 0px;
                top: 5px;
                width: 26px;
                cursor: pointer;
                z-index: 10;
            }

        #popup_content_home {
            height: auto;
            position: fixed;
            background-color: #fff;
            top: 15%;
            z-index: 999999999;
            left: 25%;
            border-radius: 10px;
            -moz-border-radius: 10px;
            -webkit-border-radius: 10px;
            width: 50%;
            padding: 20px;
        }

        #popup_content_home {
            padding: 0;
        }

        .popup_header, .popup_footer {
            float: left;
            width: 100%;
            background: #f2600e;
            padding: 10px;
            position: relative;
            color: #fff;
        }

        .popup_header {
            font-weight: bold;
            font-size: 16px;
            text-transform: uppercase;
        }

        .close_message {
            top: 10px;
        }

        .changeavatar {
            padding: 10px;
            margin: 5px 0;
            float: left;
            width: 100%;
        }

        .float-right {
            float: right;
        }

        .content1 {
            float: left;
            width: 100%;
        }

        .content2 {
            float: left;
            width: 100%;
            border-top: 1px solid #eee;
            clear: both;
            margin-top: 10px;
        }

        .btn.btn-close {
            float: right;
            background: #f2600e;
            color: #fff;
            margin: 10px 5px;
            text-transform: none;
            padding: 10px 20px;
        }

            .btn.btn-close:hover {
                background: #f58c31;
            }

        .btn.btn-close-full {
            float: right;
            background: #7bb1c7;
            color: #fff;
            margin: 10px 5px;
            text-transform: none;
            padding: 10px 20px;
        }

            .btn.btn-close-full:hover {
                background: #6692a5;
            }


        @media screen and (max-width: 768px) {
            #popup_content_home {
                left: 10%;
                width: 80%;
            }

            .content1 {
                overflow: auto;
                height: 300px;
            }
        }
    </style>
</asp:Content>