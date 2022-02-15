<%@ Page Title="" Language="C#" MasterPageFile="~/_master_pages/modal.Master" AutoEventWireup="true" CodeBehind="assign-projects.aspx.cs" Inherits="ProjectManagementTool._modal_pages.assign_projects" %>
<asp:Content ID="Content1" ContentPlaceHolderID="modal_master_head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="modal_master_body" runat="server">
    <form id="AssignProjects" runat="server">
        <div class="container-fluid" style="min-height:82vh; overflow-y:auto; max-height:82vh;">
        <div class="row">
            <div class="col-sm-12">
                 <div class="form-group">
                         <label class="lblCss" for="DDlProject">Project</label> 
                         <asp:DropDownList CssClass="form-control" ID="DDLProject" runat="server" required></asp:DropDownList>
                   </div>

                            <div class="form-group">
                                <label class="lblCss" for="txtlocation">User</label> &nbsp;<span style="color:red; font-size:1rem;">*</span>
                                <asp:DropDownList ID="DDLUser" runat="server" CssClass="form-control" AutoPostBack="true" OnSelectedIndexChanged="DDLUser_SelectedIndexChanged" required></asp:DropDownList>
                            </div>
                            <div class="form-group">
                                <label class="lblCss" for="txtcode">User Type</label> 
                                <asp:DropDownList ID="DDlUserType" runat="server" Enabled="false" CssClass="form-control" required></asp:DropDownList>
                            </div>
               </div>
            </div>
        </div>

        <div class="modal-footer">
            <asp:Button ID="btnSubmit" runat="server" Text="Submit" CssClass="btn btn-primary" OnClick="btnSubmit_Click" />
                </div>
    </form>
</asp:Content>
