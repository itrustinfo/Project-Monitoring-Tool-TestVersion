<%@ Page Title="" Language="C#" MasterPageFile="~/_master_pages/default.Master" AutoEventWireup="true" CodeBehind="default.aspx.cs" Inherits="ProjectManagementTool._content_pages.workpackage_delete_contractor._default" %>
<asp:Content ID="Content1" ContentPlaceHolderID="default_master_head" runat="server">
</asp:Content>

    <asp:Content ID="Content3" ContentPlaceHolderID="default_master_body" runat="server">
      <div class="container-fluid">
            <div class="row">
                <div class="col-md-6 col-lg-3 form-group">Monthly Financial Progress Report</div>
                <div class="col-md-6 col-lg-3 form-group">
                    <label class="sr-only" for="DDLProject">Project</label>
                    <div class="input-group">
                        <div class="input-group-prepend">
                            <span class="input-group-text">Project</span>
                        </div>
                        <asp:DropDownList ID="DDlProject" runat="server" CssClass="form-control" AutoPostBack="true" OnSelectedIndexChanged="DDlProject_SelectedIndexChanged"></asp:DropDownList>
                    </div>
                </div>
                <div class="col-md-6 col-lg-3 form-group">
                    <label class="sr-only" for="DDLWorkPackage">Work Package</label>
                    <div class="input-group">
                        <div class="input-group-prepend">
                            <span class="input-group-text">Work Package</span>
                        </div>
                        <asp:DropDownList ID="DDLWorkPackage" runat="server" CssClass="form-control"></asp:DropDownList>
                    </div>
                </div>
                <div class="col-md-6 col-lg-3 form-group"><asp:Button ID="btnDelete" runat="server" CssClass="btn btn-primary" Text="Delete All Data" OnClick="btnDelete_Click"/></div>
            </div>
         
            
        </div>
</asp:Content>
