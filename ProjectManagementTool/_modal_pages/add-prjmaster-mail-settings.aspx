<%@ Page Title="" Language="C#" MasterPageFile="~/_master_pages/modal.Master" AutoEventWireup="true" CodeBehind="add-prjmaster-mail-settings.aspx.cs" Inherits="ProjectManagementTool._modal_pages.add_prjmaster_mail_settings" %>
<asp:Content ID="Content1" ContentPlaceHolderID="modal_master_head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="modal_master_body" runat="server">
      <form id="AddMaster" runat="server">
        <div class="container-fluid" style="min-height:82vh; overflow-y:auto; max-height:82vh;">
        <div class="row">
            <div class="col-sm-12">
                 <div class="form-group">
                         <label class="lblCss" for="DDlProject">Project</label> 
                         <asp:DropDownList CssClass="form-control" ID="DDlProject" runat="server" AutoPostBack="True" OnSelectedIndexChanged="DDlProject_SelectedIndexChanged"></asp:DropDownList>
                   </div>
                 <div class="form-group">
                         <label class="lblCss" for="DDLWorkPackage">WorkPackage</label> 
                         <asp:DropDownList CssClass="form-control" ID="DDLWorkPackage" runat="server" AutoPostBack="True" OnSelectedIndexChanged="DDLWorkPackage_SelectedIndexChanged"></asp:DropDownList>
                   </div>
                    <div class="form-group">
                         <label class="lblCss" for="DDlProject">Frequency</label> 
                         <asp:RadioButtonList runat="server" ID="rdFreqList" RepeatDirection="Horizontal" CssClass="form-control">
                           <%--  <asp:ListItem>Monthly</asp:ListItem>
                             <asp:ListItem>Quarterly</asp:ListItem>--%>
                         </asp:RadioButtonList>
                   </div>
                   <div class="form-group">
                         <label class="lblCss" for="DDlProject">Select Users</label> 
                         <asp:CheckBoxList runat="server" CssClass="form-control" ID="chkUserList" Height="150px"></asp:CheckBoxList>
                   </div>      
               </div>
            </div>
        </div>

        <div class="modal-footer">
            <asp:Button ID="btnSubmit" runat="server" Text="Submit" CssClass="btn btn-primary" OnClick="btnSubmit_Click" />
                </div>
    </form>
</asp:Content>
