<%@ Page Title="" Language="C#" MasterPageFile="~/_master_pages/modal.Master" AutoEventWireup="true" CodeBehind="add-locationmaster.aspx.cs" Inherits="ProjectManagementTool._modal_pages.add_locationmaster" %>
<asp:Content ID="Content1" ContentPlaceHolderID="modal_master_head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="modal_master_body" runat="server">
    <form id="AddMaster" runat="server">
        <div class="container-fluid" style="min-height:82vh; overflow-y:auto; max-height:82vh;">
        <div class="row">
            <div class="col-sm-12">
                 <div class="form-group">
                         <label class="lblCss" for="DDlProject">Project</label> 
                         <asp:DropDownList CssClass="form-control" ID="DDlProject" runat="server"></asp:DropDownList>
                   </div>

                            <div class="form-group">
                                <label class="lblCss" for="txtlocation">Location</label> &nbsp;<span style="color:red; font-size:1.2rem;">*</span>
                                <asp:TextBox ID="txtlocation" runat="server" CssClass="form-control" autocomplete="off" required></asp:TextBox>
                            </div>
                            <div class="form-group">
                                <label class="lblCss" for="txtcode">Location Code</label> &nbsp;<span style="color:red; font-size:1.2rem;">*</span>
                                <asp:TextBox ID="txtcode" runat="server" CssClass="form-control" autocomplete="off" required></asp:TextBox>
                            </div>
               </div>
            </div>
        </div>

        <div class="modal-footer">
            <asp:Button ID="btnSubmit" runat="server" Text="Submit" CssClass="btn btn-primary" OnClick="btnSubmit_Click" />
                </div>
    </form>
</asp:Content>
