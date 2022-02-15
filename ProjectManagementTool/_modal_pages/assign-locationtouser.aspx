<%@ Page Title="" Language="C#" MasterPageFile="~/_master_pages/modal.Master" AutoEventWireup="true" CodeBehind="assign-locationtouser.aspx.cs" Inherits="ProjectManagementTool._modal_pages.assign_locationtouser" %>
<asp:Content ID="Content1" ContentPlaceHolderID="modal_master_head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="modal_master_body" runat="server">
    <form id="FemAssignLocationtoUser" runat="server">
        <div class="container-fluid" style="min-height:82vh; overflow-y:auto; max-height:82vh;">
        <div class="row">
            <div class="col-sm-12">
                <div class="form-group">
                      <label class="lblCss" for="txtlocation">User</label> &nbsp;<span style="color:red; font-size:1.1rem;">*</span>
                    <asp:DropDownList CssClass="form-control" ID="DDLUser" runat="server"></asp:DropDownList>
                </div>
                 <div class="form-group">
                         <label class="lblCss" for="DDlProject">Location</label> &nbsp;<span style="color:red; font-size:1.1rem;">*</span>&nbsp;(Use <b>Ctrl</b> key to select multiple locations)
                         <asp:ListBox ID="LstLocation" runat="server" SelectionMode="Multiple" Height="200px" CssClass="form-control"></asp:ListBox>
                   </div>
                
               </div>
            </div>
        </div>

        <div class="modal-footer">
            <asp:Button ID="btnSubmit" runat="server" Text="Submit" CssClass="btn btn-primary" OnClick="btnSubmit_Click" />
                </div>
    </form>
</asp:Content>
