<%@ Page Title="" Language="C#" MasterPageFile="~/_master_pages/modal.Master" AutoEventWireup="true" CodeBehind="mail-documentlink.aspx.cs" Inherits="ProjectManagementTool._modal_pages.mail_documentlink" %>
<asp:Content ID="Content1" ContentPlaceHolderID="modal_master_head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="modal_master_body" runat="server">
      <form id="frmAddSubmittalModal" runat="server">
        <asp:ScriptManager ID="ScriptManager1" runat="server"></asp:ScriptManager>
        <div class="container-fluid" style="max-height:82vh; overflow-y:auto;">
            <div class="row">
                <div class="col-sm-12">
                  
                     <div class="form-group">
                        <label class="lblCss" for="txtemail">Enter E-Mail</label> &nbsp;<span style="color:red; font-size:1.1rem;">*</span>(Use , for multiple emails. Ex: abc@gmail.com,xyz@gmail.com)
                         <asp:TextBox ID="txtemail" CssClass="form-control" runat="server" autocomplete="off" required ClientIDMode="Static"></asp:TextBox>
                    </div>
                </div>
            </div> 
        </div>

        <div class="modal-footer">
            <asp:Button ID="btnSubmit" runat="server" Text="Submit" CssClass="btn btn-primary" OnClick="btnSubmit_Click"/>
                </div>
    </form>
</asp:Content>
