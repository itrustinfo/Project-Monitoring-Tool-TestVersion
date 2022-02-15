<%@ Page Title="" Language="C#" MasterPageFile="~/_master_pages/modal.Master" AutoEventWireup="true" CodeBehind="add-activitydatafromexcel.aspx.cs" Inherits="ProjectManagementTool._modal_pages.add_activitydatafromexcel" %>
<asp:Content ID="Content1" ContentPlaceHolderID="modal_master_head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="modal_master_body" runat="server">
     <form id="FrmAddActivityData" runat="server">
        <div class="container-fluid">
            <div class="row">
                <div class="col-sm-12">
                    <div class="form-group">
                        <label class="lblCss" for="txtoptionname">Workpackage Option Name</label>
                        <asp:TextBox ID="txtoptionname" CssClass="form-control" required Enabled="false" runat="server" ClientIDMode="Static"></asp:TextBox>
                    </div>
                    <div class="form-group">
                        <label class="lblCss" for="FilCoverLetter">Choose Excel file</label>
                        <div class="custom-file">
                            <asp:FileUpload ID="FileUpload1" runat="server" required CssClass="custom-file-input" />

                            <%--<asp:FileUpload ID="FilCoverLetter" CssClass="custom-file-input" runat="server" ClientIDMode="Static" /> --%>
                            <label class="custom-file-label" for="FilCoverLetter">Choose Excel file</label>
                        </div>
                    </div>
                </div> 
                
        </div>
            </div>
        <div class="modal-footer">
            <asp:Button ID="btnSubmit" runat="server" Text="Submit" CssClass="btn btn-primary" OnClick="btnSubmit_Click" />
                </div>
    </form>
</asp:Content>
