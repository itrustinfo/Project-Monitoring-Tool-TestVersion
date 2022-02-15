<%@ Page Title="" Language="C#" MasterPageFile="~/_master_pages/modal.Master" AutoEventWireup="true" CodeBehind="upload-sitephotograph.aspx.cs" Inherits="ProjectManagementTool._modal_pages.upload_sitephotograph" %>
<asp:Content ID="Content1" ContentPlaceHolderID="modal_master_head" runat="server">
    <style type="text/css">
        .HideItem
        {
            display:none;
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="modal_master_body" runat="server">
    <form id="frmUploadSitePhotograph" runat="server">
        <div class="container-fluid" style="max-height:85vh; overflow-y:auto; min-height:80vh;">
            <div class="row">
                <div class="col-sm-4">
                    <div class="form-group">
                        <label class="lblCss" for="UploadPhotographs">Choose File/s</label> &nbsp;<span style="color:red; font-size:1.2rem;">*</span>
                        <div class="custom-file">
                            <asp:FileUpload ID="ImageUpload" runat="server" AllowMultiple="true" CssClass="custom-file-input" />

                            <%--<asp:FileUpload ID="FilDocument" CssClass="custom-file-input" runat="server" ClientIDMode="Static" /> --%>
                            <label class="custom-file-label" for="UploadPhotographs">Choose Photographs</label>
                        </div>
                    </div>
                    </div>
                <div class="col-sm-4">
                    <label class="lblCss" for="UploadPhotographs">&nbsp;</label><br />
                    <asp:Button ID="btnUpload" runat="server" Text="Upload" CssClass="btn btn-primary" OnClick="btnSubmit_Click" />
                    </div>
                <div class="col-sm-4">
                    </div>
                </div>
             <div class="row">
                <div class="col-sm-12">
                    <div class="table-responsive">
                         <asp:DataList ID="GrdSitePhotograph" runat="server" RepeatColumns="3" HorizontalAlign="Center" CellPadding="10" RepeatDirection="Horizontal">
                                <ItemTemplate>
                                    <div style="width:200px; float:left; border:1px solid Gray; text-align:center; background-color:#f2f2f2;">
                                        <div style="padding:10px;">
                                            <asp:Image ID="imgEmp" runat="server" Width="150px" ImageUrl='<%# Bind("Site_Image", "{0}") %>' /><br />
                                                <asp:TextBox ID="txtdesc" runat="server" CssClass="form-control" placeholder="Description" autocomplete="off"></asp:TextBox>
                                            <asp:Label ID="LblSitePhotoGraph_UID" runat="server" CssClass="HideItem" Text='<%#Eval("SitePhotoGraph_UID")%>'></asp:Label>
                                        </div>
                                    </div>
                                    </ItemTemplate>
                            </asp:DataList>
                            </div>
                    </div>
                </div>
            </div>

         <div class="modal-footer">
                    <%--<button type="button" class="btn btn-secondary" data-dismiss="modal">Close</button>--%>
            <asp:Button ID="btnAdd" runat="server" CssClass="btn btn-primary" Text="Submit" OnClick="btnAdd_Click" />
                    <%--<button type="button" class="btn btn-primary">Save changes</button>--%>
                </div>
        </form>
</asp:Content>
