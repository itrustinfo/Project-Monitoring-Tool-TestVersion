<%@ Page Title="" Language="C#" MasterPageFile="~/_master_pages/modal.Master" AutoEventWireup="true" CodeBehind="change-password.aspx.cs" Inherits="ProjectManagementTool._modal_pages.change_password" %>
<asp:Content ID="Content1" ContentPlaceHolderID="modal_master_head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="modal_master_body" runat="server">
    <form id="ChangeMaster" runat="server">
        <div class="container-fluid" style="min-height:82vh; overflow-y:auto; max-height:82vh;">
        <div class="row">
            <div class="col-sm-12">
                  <div class="form-group">
                                <label class="lblCss" for="txtclientname">Old Password</label> &nbsp;<span style="color:red; font-size:1.2rem;">*</span>
                                <asp:TextBox ID="txtOldPassword"  TextMode="Password" runat="server" CssClass="form-control" autocomplete="off" required></asp:TextBox>
                            </div>

                            <div class="form-group">
                                <label class="lblCss" for="txtclientname">New Password</label> &nbsp;<span style="color:red; font-size:1.2rem;">*</span>
                                <asp:TextBox ID="txtNewPassword" TextMode="Password" runat="server" CssClass="form-control" autocomplete="off" required></asp:TextBox>
                            </div>
                            <div class="form-group">
                                <label class="lblCss" for="txtcode">Confirm Password</label> &nbsp;<span style="color:red; font-size:1.2rem;">*</span> 
                                <asp:TextBox ID="txtConfirmPassword" TextMode="Password" runat="server" CssClass="form-control" autocomplete="off" required></asp:TextBox>
                            </div>
               </div>
            </div>
        </div>

        <div class="modal-footer">
            <asp:Button ID="btnSubmit" runat="server" Text="Submit" CssClass="btn btn-primary" OnClick="btnSubmit_Click" />
                </div>
    </form>
</asp:Content>
