<%@ Page Title="" Language="C#" MasterPageFile="~/_master_pages/modal.Master" AutoEventWireup="true" CodeBehind="view-sitephotographs.aspx.cs" Inherits="ProjectManagementTool._modal_pages.view_sitephotographs" %>

<asp:Content ID="Content1" ContentPlaceHolderID="modal_master_head" runat="server">

    <style type="text/css">
        
     .modal
     {
        display: none;
        position: absolute;
        top: 0px;
        left: 0px;
        background-color:black;
        z-index:100;
        opacity: 0.8;
        filter: alpha(opacity=60);
        -moz-opacity:0.8;
        min-height: 100%;
     }

     #divImage
     {
        display: none;
        z-index: 1000;
        position: fixed;
        top: 0;
        left: 0;
        background-color:White;
        height: 450px;
        width: 500px;
        padding: 3px;
        border: solid 1px black;
     }

        .auto-style1 {
            text-align: center;
        }

   </style>


    <script type="text/javascript">
        function DeleteItem() {
            if (confirm("Are you sure you want to delete ...?")) {
                return true;
            }
            return false;
        }

        function LoadDiv(url) {
            debugger;
            var img = new Image();
            var bcgDiv = document.getElementById("divBackground");
            var imgDiv = document.getElementById("divImage");
            var imgFull = document.getElementById("imgFull");
            
            img.onload = function () {
                imgFull.src = img.src;
                imgFull.style.display = "block";
            };

            img.src = url;
            var width = document.body.clientWidth;
            if (document.body.clientHeight > document.body.scrollHeight) {
                bcgDiv.style.height = document.body.clientHeight + "px";
            }
            else {
                bcgDiv.style.height = document.body.scrollHeight + "px";
            }

            imgDiv.style.left = (width - 650) / 2 + "px";
            imgDiv.style.top = "20px";
            bcgDiv.style.width = "100%";

            bcgDiv.style.display = "block";
            imgDiv.style.display = "block";

            var mainDiv = document.getElementById("mainImage");
            mainDiv.style.display = "none";
            return false;
        }

        function HideDiv() {
            var bcgDiv = document.getElementById("divBackground");
            var mainDiv = document.getElementById("mainImage");
            
            var imgDiv = document.getElementById("divImage");
            var imgFull = document.getElementById("imgFull");
            if (bcgDiv != null) {
                bcgDiv.style.display = "none";
                imgDiv.style.display = "none";
                imgFull.style.display = "none";
                mainDiv.style.display = "block";
            }
        }

    </script>
    <script>



        $(function () {
            $("input[id$='txtFromDate']").datepicker({
                changeMonth: true,
                changeYear: true,
                dateFormat: 'dd/mm/yy'
            });

            $("input[id$='txtToDate']").datepicker({
                changeMonth: true,
                changeYear: true,
                dateFormat: 'dd/mm/yy'
            });

        });
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="modal_master_body" runat="server">
    <form id="frmUploadSitePhotograph" runat="server">
        <div id="divBackground">
        </div>

        <div id="divImage">
            <table style="height: 100%; width: 100%">
                <tr>
                    <td align="center" valign="bottom">
                        <input id="btnClose" type="button" value="Close" onclick="HideDiv()" />
                    </td>
                </tr>
                <tr>
                    <td>Description: <asp:Label runat="server" ID="txtDesc"></asp:Label></td>
                </tr>
                <tr>
                    <td>Uploaded Date: <asp:Label runat="server" ID="txtUploadDate"></asp:Label></td>
                </tr>
                <tr>
                    <td valign="middle" align="center">
                        <img id="imgFull" alt="" src="" style="display: none; height: 400px; width: 490px" />
                    </td>
                </tr>
            </table>
        </div>




        <div id="mainImage" class="container-fluid" style="max-height: 85vh; overflow-y: auto; min-height: 80vh;">
            <div class="row">
                <div class="col-sm-3">
                    <div class="form-group">
                        <label class="lblCss" for="txtRabillNumber">From Uploaded Date</label>
                        
                        <asp:TextBox ID="txtFromDate" CssClass="form-control" autocomplete="off" runat="server" ClientIDMode="Static"></asp:TextBox>

                    </div>
                </div>
                <div class="col-sm-3">
                    <div class="form-group">
                        <label class="lblCss" for="txtRabillNumber">To Uploaded Date</label>
                        <asp:TextBox ID="txtToDate" CssClass="form-control" autocomplete="off" runat="server" ClientIDMode="Static"></asp:TextBox>

                    </div>
                </div>
                <div class="col-sm-3">
                    <div class="form-group">
                        <label class="lblCss" for="txtDescription">Description</label>
                        <asp:TextBox ID="txtDescription" CssClass="form-control" autocomplete="off" runat="server" ClientIDMode="Static"></asp:TextBox>

                    </div>
                </div>
                <div class="col-sm-3">
                    <label class="lblCss" for="UploadPhotographs">&nbsp;</label><br />
                    <asp:Button ID="btnSearch" runat="server" Text="Search" CssClass="btn btn-primary" OnClick="btnSearch_Click" />
                </div>
            </div>
            <div class="row">
                <div class="col-sm-12">
                    <div class="table-responsive">

                        <asp:DataList ID="GrdSitePhotograph" runat="server" DataKeyField="SitePhotoGraph_UID" RepeatColumns="3" HorizontalAlign="Center" CellPadding="10" RepeatDirection="Horizontal" OnItemCommand="GrdSitePhotograph_UpdateCommand" OnDeleteCommand="GrdSitePhotograph_DeleteCommand">
                            <ItemTemplate>
                                <div style="width: 275px; float: left; border: 1px solid Gray; text-align: center; background-color: #f2f2f2;">

                                    <div style="padding: 10px;">
                                        <%--<asp:ImageButton ID="imgEmp" runat="server" Width="225px" ImageUrl='<%# Bind("Site_Image", "{0}")%>' Style="cursor: pointer" OnClientClick="return LoadDiv(this.src);"/></br>--%>
                                        <asp:ImageButton ID="imgEmp" runat="server" Width="225px" ImageUrl='<%# Bind("Site_Image", "{0}")%>' Style="cursor: pointer" CommandName ="ModifyRow" /></br>
                                        
                                        <%--<asp:Image ID="imgEmp" runat="server" Width="225px" ImageUrl='<%# Bind("Site_Image", "{0}")%>' Style="cursor: pointer" OnClientClick="return LoadDiv(this.src);"/><br />--%>
                                        <b>
                                            <asp:Label ID="LblDescription" runat="server" Text='<%#Eval("Description")%>'></asp:Label></b>
                                    </div>
                                    <asp:LinkButton ID="lnkdelete" runat="server" OnClientClick="return DeleteItem()" CausesValidation="false" CommandName="delete"><span title="Delete" class="fas fa-trash"></span></asp:LinkButton>
                                </div>
                            </ItemTemplate>
                        </asp:DataList>
                        <asp:Label ID="LblMessage" runat="server" class="lblCss" Text="No Site Photograph/s Found.."></asp:Label>
                    </div>
                </div>
            </div>
        </div>
    </form>

</asp:Content>
