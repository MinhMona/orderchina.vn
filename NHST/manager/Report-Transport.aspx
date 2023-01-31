<%@ Page Language="C#" Title="Xuất file hàng ký gửi" MasterPageFile="~/manager/adminMasterNew.Master" AutoEventWireup="true" CodeBehind="Report-Transport.aspx.cs" Inherits="NHST.manager.Report_Transport" %>

<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>
<%@ Import Namespace="NHST.Controllers" %>
<%@ Import Namespace="NHST.Models" %>
<%@ Import Namespace="NHST.Bussiness" %>
<%@ Import Namespace="MB.Extensions" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div id="main" class="main-full">
        <div class="row">
            <div class="content-wrapper-before bg-dark-gradient"></div>
            <div class="page-title">
                <div class="card-panel">
                    <h4 class="title no-margin" style="display: inline-block;">Xuất file đơn hàng ký gửi</h4>
                    <div class="clearfix"></div>
                </div>
            </div>
            <div class="col s12">
                <div class="card-panel">
                    <div class="order-list-info">
                        <div class="total-info">
                            <div class="row section">
                                <div class="col s12">
                                    <div class="filter filter-fix">
                                        <div class="row">
                                            <div class="input-field col s6 m4 l5">
                                                <asp:TextBox runat="server" ID="txtdatefrom" type="text" class="datetimepicker from-date"></asp:TextBox>
                                                <label>Từ ngày</label>
                                            </div>
                                            <div class="input-field col s6 m4 l5">
                                                <asp:TextBox runat="server" ID="txtdateto" type="text" class="datetimepicker to-date"></asp:TextBox>
                                                <label>Đến ngày</label>
                                                <span class="helper-text"
                                                    data-error="Vui lòng chọn ngày bắt đầu trước"></span>
                                            </div>
                                            <div class="input-field col s12 m4 l2">
                                                <a href="javascript:;" class="btn" id="btnFilter">Xem thống kê</a>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                            </div>
                            <div class="row section ">
                                <div class="input-field col s12 m4 l2">
                                    <a href="javascript:;" class="btn" onclick="Export()" id="btnExport">Xuất thống kê</a>
                                </div>
                                <div class="col s12">
                                    <div class="responsive-tb">
                                        <table class="table responsive-table bordered highlight">
                                            <thead>
                                                <tr class="teal darken-4">
                                                    <th class="white-text"></th>
                                                    <th class="white-text">Waybill Number</th>
                                                    <th class="white-text">Shipment Reference No.</th>
                                                    <th class="white-text">Company Name</th>
                                                    <th class="white-text">Contact Name</th>
                                                    <th class="white-text">Tel</th>
                                                    <th class="white-text">Address</th>
                                                    <th class="white-text">Country</th>
                                                    <th class="white-text">Postal Code</th>
                                                    <th class="white-text">Company Name</th>
                                                    <th class="white-text">Contact Name</th>
                                                    <th class="white-text">Tel</th>
                                                    <th class="white-text">Address</th>
                                                    <th class="white-text">Country</th>
                                                    <th class="white-text">Postal Code</th>
                                                    <th class="white-text">Currency</th>
                                                    <th class="white-text">Content</th>
                                                    <th class="white-text">Quantity</th>
                                                    <th class="white-text">Unit Price</th>
                                                    <th class="white-text">Total Value</th>
                                                    <th class="white-text">More than 2 items</th>
                                                    <th class="white-text">Shipment Type</th>
                                                    <th class="white-text">Total Package</th>
                                                    <th class="white-text">Total Actual Weight</th>
                                                    <th class="white-text">Origin</th>
                                                    <th class="white-text">Destination</th>
                                                    <th class="white-text">Creation Date</th>
                                                    <th class="white-text">Processing Shipment Date</th>
                                                    <th class="white-text">HSCODE</th>
                                                    <th class="white-text">Category</th>
                                                    <th class="white-text">BoxID</th>
                                                    <th class="white-text">Package Number</th>
                                                    <th class="white-text">Payment Method</th>
                                                    <th class="white-text">COD Value</th>
                                                    <th class="white-text">Product Category ID</th>
                                                    <th class="white-text">Sort Code</th>
                                                    <th class="white-text">SKUID</th>
                                                    <th class="white-text">Total COD Value</th>
                                                    <th class="white-text">Category (ZH)</th>
                                                    <th class="white-text">Package Code</th>
                                                    <th class="white-text">Order ID</th>
                                                    <th class="white-text">Link</th>
                                                    <th class="white-text">Note</th>
                                                </tr>
                                            </thead>
                                            <tbody>
                                                <asp:Literal runat="server" ID="ltr"></asp:Literal>
                                            </tbody>
                                        </table>
                                    </div>
                                    <div class="pagi-table float-right mt-2">
                                        <%this.DisplayHtmlStringPaging1();%>
                                    </div>
                                    <div class="clearfix"></div>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </div>
    </div>
    <!-- END: Page Main-->
    <asp:HiddenField ID="hdfID" runat="server" />
    <asp:Button runat="server" ID="buttonFilter" OnClick="btnFilter_Click" Style="display: none" UseSubmitBehavior="false" />
    <asp:Button runat="server" ID="buttonExport" OnClick="btnExport_Click" Style="display: none" UseSubmitBehavior="false" />
    <asp:HiddenField runat="server" ID="hdfDataChart" />
    <script>
        $('#btnFilter').click(function () {
            $(<%=buttonFilter.ClientID%>).click();
        });
        function Export() {
            var c = confirm('Bạn muốn xuất file đã chọn?');
            if (c == true) {
                $("#<%=buttonExport.ClientID%>").click();
            }
        }
        function CheckBarcode(IDBarcode) {
            $.ajax({
                type: "POST",
                url: "/manager/Report-Transport.aspx/CheckBarcode",
                data: "{IDBarcode:'" + IDBarcode + "'}",
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: function (msg) {

                },
                error: function (xmlhttprequest, textstatus, errorthrow) {
                    alert(errorthrow);
                }
            });
        }
    </script>
</asp:Content>
