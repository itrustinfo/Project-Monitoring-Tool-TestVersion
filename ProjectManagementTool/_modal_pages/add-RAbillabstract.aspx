<%@ Page Title="" Language="C#" MasterPageFile="~/_master_pages/modal.Master" AutoEventWireup="true" CodeBehind="add-RAbillabstract.aspx.cs" Inherits="ProjectManagementTool._modal_pages.add_RAbillabstract" %>
<asp:Content ID="Content1" ContentPlaceHolderID="modal_master_head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="modal_master_body" runat="server">
    <form id="AddRABillNumber" runat="server">
        <div class="container-fluid" style="overflow-y:auto; max-height:85vh; min-height:76vh;">
        <div class="row">
            <div class="col-sm-12">
                 <div class="form-group">
                         <label class="lblCss" for="txtprojectclass">RA Bill Number</label> &nbsp;<span style="color:red; font-size:1.2rem;">*</span>
                         <asp:TextBox ID="txtrabillnumber" runat="server" CssClass="form-control" autocomplete="off" required></asp:TextBox>
                   </div>
               </div>
            </div>
        </div>

        <div class="modal-footer">
            <asp:Button ID="btnSubmit" runat="server" Text="Submit" CssClass="btn btn-primary" OnClick="btnSubmit_Click" />
                </div>
    </form>
</asp:Content>
