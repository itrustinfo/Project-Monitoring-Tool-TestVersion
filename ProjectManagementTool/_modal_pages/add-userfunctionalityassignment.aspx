<%@ Page Title="" Language="C#" MasterPageFile="~/_master_pages/modal.Master" AutoEventWireup="true" CodeBehind="add-userfunctionalityassignment.aspx.cs" Inherits="ProjectManagementTool._modal_pages.add_userfunctionalityassignment" %>
<asp:Content ID="Content1" ContentPlaceHolderID="modal_master_head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="modal_master_body" runat="server">
    <form id="frmAddUserFunactinalityModal" runat="server">
        <asp:ScriptManager ID="ScriptManager1" runat="server"></asp:ScriptManager>
        <div class="container-fluid" style="max-height:82vh; overflow-y:auto;">
            <div class="row">
                <div class="col-sm-12">
                    <div class="form-group">
                        <label class="lblCss" for="txtpagename">Select Page Name</label> &nbsp;<span style="color:red; font-size:1.1rem;">*</span>
                         <asp:DropDownList ID="DDLPageName" runat="server" CssClass="form-control"></asp:DropDownList>
                    </div>

                     <div class="form-group">
                        <label class="lblCss" for="txtpagename">Select User</label> &nbsp;<span style="color:red; font-size:1.1rem;">*</span>
                         <asp:DropDownList ID="DDLUser" runat="server" CssClass="form-control"></asp:DropDownList>
                    </div>

                    <div class="form-group">
                        <label class="lblCss" for="txtpageurl">Select Functionality</label> &nbsp;<span style="color:red; font-size:1.1rem;">*</span>
                        
                        <asp:CheckBoxList ID="ChkFunctionality" runat="server" RepeatDirection="Horizontal">
                            <asp:ListItem Value="Add">&nbsp;Add</asp:ListItem>
                            <asp:ListItem Value="Edit">&nbsp;Edit</asp:ListItem>
                            <asp:ListItem Value="Delete">&nbsp;Delete</asp:ListItem>
                        </asp:CheckBoxList>
                        
                    </div>
                </div>
            </div> 
        </div>

        <div class="modal-footer">
            <asp:Button ID="btnSubmit" runat="server" Text="Submit" CssClass="btn btn-primary" OnClick="btnSubmit_Click"/>
                </div>
    </form>
</asp:Content>
