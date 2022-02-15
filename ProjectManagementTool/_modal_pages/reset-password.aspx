<%@ Page Title="" Language="C#" MasterPageFile="~/_master_pages/modal.Master" AutoEventWireup="true" CodeBehind="reset-password.aspx.cs" Inherits="ProjectManagementTool._modal_pages.reset_password" %>
<asp:Content ID="Content1" ContentPlaceHolderID="modal_master_head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="modal_master_body" runat="server">
    <form id="FrmIssueStatus" runat="server">
        <asp:ScriptManager ID="ScriptManager1" runat="server"></asp:ScriptManager>
        <div class="container-fluid" style="min-height:75vh; overflow-y:auto;">
            
            <div class="row">
                <div class="col-sm-12">
                  
                    
                   
                    <div class="form-group">
                        <label class="lblCss" for="txtnewpassword">New Password</label> &nbsp;<span style="color:red; font-size:1.1rem;">*</span>
                       <asp:TextBox ID="txtnewpassword" CssClass="form-control" runat="server" autocomplete="off" required ClientIDMode="Static" TextMode="Password"></asp:TextBox>
                    </div>
                     <div class="form-group">
                        <label class="lblCss" for="txtconfirmpassword">Confirm Password</label> &nbsp;<span style="color:red; font-size:1.1rem;">*</span>
                       <asp:TextBox ID="txtconfirmpassword" CssClass="form-control" runat="server" autocomplete="off" required ClientIDMode="Static" TextMode="Password"></asp:TextBox>
                    </div>

                   
                </div>
            </div> 
        </div>

        <div class="modal-footer">
            <asp:Button ID="btnSubmit" runat="server" Text="Submit" CssClass="btn btn-primary" OnClick="btnSubmit_Click"/>
                </div>
    </form>
</asp:Content>
