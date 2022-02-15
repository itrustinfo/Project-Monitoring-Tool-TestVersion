<%@ Page Title="" Language="C#" MasterPageFile="~/_master_pages/modal.Master" AutoEventWireup="true" CodeBehind="add-pagenamemaster.aspx.cs" Inherits="ProjectManagementTool._modal_pages.add_pagenamemaster" %>
<asp:Content ID="Content1" ContentPlaceHolderID="modal_master_head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="modal_master_body" runat="server">
    <form id="frmAddPageNameMasterModal" runat="server">
        <asp:ScriptManager ID="ScriptManager1" runat="server"></asp:ScriptManager>
        <div class="container-fluid" style="max-height:82vh; overflow-y:auto;">
            <div class="row">
                <div class="col-sm-12">
                    
                     <div class="form-group">
                        <label class="lblCss" for="txtpagename">Page Name</label> &nbsp;<span style="color:red; font-size:1.1rem;">*</span>
                         <asp:TextBox ID="txtpagename" CssClass="form-control" runat="server" autocomplete="off" required ClientIDMode="Static"></asp:TextBox>
                    </div>

                    <div class="form-group">
                        <label class="lblCss" for="txtpageurl">Page URL</label> &nbsp;<span style="color:red; font-size:1.1rem;">*</span>
                        <asp:TextBox ID="txtpageurl" CssClass="form-control" runat="server" autocomplete="off" required ClientIDMode="Static"></asp:TextBox>
                        
                    </div>
                </div>
            </div> 
        </div>

        <div class="modal-footer">
            <asp:Button ID="btnSubmit" runat="server" Text="Submit" CssClass="btn btn-primary" OnClick="btnSubmit_Click"/>
                </div>
    </form>
</asp:Content>
