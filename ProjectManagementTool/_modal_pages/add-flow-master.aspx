<%@ Page Title="" Language="C#" MasterPageFile="~/_master_pages/modal.Master" AutoEventWireup="true" CodeBehind="add-flow-master.aspx.cs" Inherits="ProjectManagementTool._modal_pages.add_flow_master" %>

<asp:Content ID="Content1" ContentPlaceHolderID="modal_master_head" runat="server">
    <style type="text/css">
        
        .chkChoice td 
        { 
            padding-right: 20px; 
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="modal_master_body" runat="server">
    <form id="ChangeMaster" runat="server">
        <div class="container-fluid" style="min-height: 82vh; overflow-y: auto; max-height: 82vh;">
            <div class="row">
                <div class="col-sm-6">
                    <div class="form-group">
                        <label class="lblCss" for="DDlProject">Project</label>
                        <asp:DropDownList CssClass="form-control" ID="DDlProject" runat="server" AutoPostBack="True" OnSelectedIndexChanged="DDlProject_SelectedIndexChanged"></asp:DropDownList>
                    </div>
                    <div class="form-group">
                        <label class="lblCss" for="DDLWorkPackageCategory">Work Package</label>
                        <asp:DropDownList CssClass="form-control" ID="DDLWorkPackage" runat="server" AutoPostBack="True" OnSelectedIndexChanged="DDLWorkPackage_SelectedIndexChanged"></asp:DropDownList>
                    </div>
                    <div class="form-group">
                        <label class="lblCss" for="DDLWorkPackageCategory">WorkPackage Category</label>
                        <asp:DropDownList CssClass="form-control" ID="DDLWorkPackageCategory" runat="server" AutoPostBack="True" OnSelectedIndexChanged="DDLWorkPackageCategory_SelectedIndexChanged"></asp:DropDownList>
                    </div>
                    
                    
                </div>


                <div class="col-sm-6">
                    <div class="form-group">
                        <label class="lblCss" for="DDLFlow">Flow</label>
                        <asp:DropDownList CssClass="form-control" ID="DDLFlow" runat="server" AutoPostBack="True" OnSelectedIndexChanged="DDLFlow_SelectedIndexChanged"></asp:DropDownList>
                    </div>
                    <div class="form-group">
                        <label class="lblCss" for="DDLSteps">Steps</label>
                        <asp:DropDownList CssClass="form-control" ID="DDLSteps" runat="server" AutoPostBack="True" OnSelectedIndexChanged="DDLSteps_SelectedIndexChanged"></asp:DropDownList>
                    </div>
                </div>
            </div>
            <div class="row">
                <div class="col-sm-12">
                    <div class="form-group">
                        <label class="lblCss" for="DDlProject">Select Users</label>
                        <asp:CheckBoxList runat="server" CssClass="form-control chkChoice" ID="chkUserList" RepeatLayout="Table" RepeatColumns="6"  RepeatDirection="Horizontal" height="100%"  ></asp:CheckBoxList>
                    </div>
                </div>
            </div>
        </div>
        <div class="modal-footer">
            <asp:Button ID="btnSubmit" runat="server" Text="Submit" CssClass="btn btn-primary" OnClick="btnSubmit_Click" />
        </div>
    </form>
</asp:Content>

