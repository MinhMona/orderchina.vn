<%@ page title="Danh sách bài viết" language="C#" masterpagefile="~/manager/adminMasterNew.Master" autoeventwireup="true" codebehind="PageList.aspx.cs" inherits="NHST.manager.PageList" %>

<%@ register assembly="Telerik.Web.UI" namespace="Telerik.Web.UI" tagprefix="telerik" %>
<%@ import namespace="NHST.Controllers" %>
<%@ import namespace="NHST.Models" %>
<%@ import namespace="NHST.Bussiness" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">

    <div id="main" class="main-full">
        <div class="row">
            <div class="content-wrapper-before bg-dark-gradient"></div>
            <div class="page-title">
                <div class="card-panel">
                    <h4 class="title no-margin" style="display: inline-block;">Danh sách bài viết</h4>
                    <div class="right-action">
                        <a href="/manager/AddPage" class="btn waves-effect">Thêm bài viết</a>
                    </div>
                    <div class="clearfix"></div>
                </div>
            </div>
            <div class="list-staff col s12 section">
                <div class="list-table card-panel">
                    <div class="filter">
                        <div class="search-name input-field">
                            <asp:TextBox ID="search_name" placeholder="" name="txtSearchName" type="text" onkeypress="myFunction()" runat="server" />
                            <label for="search_name"><span>Tiêu đề</span></label>
                            <span class="material-icons search-action">search</span>
                            <asp:Button Style="display: none" UseSubmitBehavior="false" ID="btnSearch" runat="server" OnClick="btnSearch_Click" />
                        </div>
                    </div>
                    <table class="table responsive-table bordered highlight">
                        <thead>
                            <tr>
                                <th>ID</th>
                                <th>Tiêu đề bài viết</th>
                                <th>Trạng thái</th>
                                <th>Chuyên mục</th>
                                <th>Ngày tạo</th>
                                <th>Thao tác</th>
                            </tr>
                        </thead>
                        <tbody>
                            <asp:Literal ID="ltr" runat="server" EnableViewState="false"></asp:Literal>
                        </tbody>
                    </table>
                    <div class="pagi-table float-right mt-2">
                        <%this.DisplayHtmlStringPaging1();%>
                    </div>
                    <div class="clearfix"></div>
                </div>
            </div>
        </div>
    </div>
    <asp:HiddenField ID="hdfID" runat="server" Value="0" />
    <asp:Button runat="server" ID="btnDelete" OnClick="btnDelete_Click" Style="display: none" UseSubmitBehavior="false" />
    <script>
        function myFunction() {
            if (event.which == 13 || event.keyCode == 13) {
                console.log($('#<%=search_name.ClientID%>').val());
                $('#<%=btnSearch.ClientID%>').click();
            }
        }
        $('.search-action').click(function () {
            console.log('dkm ngon');
            console.log($('#<%=search_name.ClientID%>').val());
            $('#<%=btnSearch.ClientID%>').click();
        })
        function Delete(ID) {
            var c = confirm('bạn có muốn xóa không?');
            if (c) {
                $("#<%=hdfID.ClientID%>").val(ID);
                $("#<%=btnDelete.ClientID%>").click();
            }
        }
    </script>

</asp:Content>
