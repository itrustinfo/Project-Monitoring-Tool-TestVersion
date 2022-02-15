<%@ Page Title="" Language="C#" MasterPageFile="~/_master_pages/modal.Master" AutoEventWireup="true" CodeBehind="add-location.aspx.cs" Inherits="ProjectManagementTool._modal_pages.add_location" %>
<asp:Content ID="Content1" ContentPlaceHolderID="modal_master_head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="modal_master_body" runat="server">
     <form id="FrmAddLocation" runat="server">
        <div class="container-fluid" style="min-height:70vh;">
        <div class="row">
            <div class="col-sm-12">
                <div class="form-group">
                    <label class="lblCss" for="txtlocation">Location Name</label> &nbsp;<span style="color:red; font-size:1.2rem;">*</span>
                     <asp:TextBox ID="txtlocation" runat="server" CssClass="form-control" autocomplete="off" required></asp:TextBox>
                </div>
               </div>
            </div>
        </div>

        <div class="modal-footer">
            <asp:Button ID="btnSubmit" runat="server" Text="Submit" CssClass="btn btn-primary" OnClick="btnSubmit_Click" />
                </div>
    </form>
</asp:Content>
