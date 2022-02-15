<%@ Page Title="" Language="C#" MasterPageFile="~/_master_pages/modal.Master" AutoEventWireup="true" CodeBehind="add-workpackagecategory.aspx.cs" Inherits="ProjectManagementTool._modal_pages.add_workpackagecategory" %>
<asp:Content ID="Content1" ContentPlaceHolderID="modal_master_head" runat="server">
        <style type="text/css">
        .lblCss {
            font-size:1rem;font-weight:400;
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="modal_master_body" runat="server">
    <form id="frmAddWorkpackageModal" runat="server">

     <div class="container-fluid" style="max-height:85vh; overflow-y:auto; min-height:85vh;">
         <div class="row">
                <div class="col-sm-12">
                        <div class="form-group">
                            <label class="lblCss" for="txtworkpackage">Select Option</label>
                            <asp:RadioButtonList ID="RBList" runat="server" Width="100%" AutoPostBack="true" RepeatDirection="Horizontal" OnSelectedIndexChanged="RBList_SelectedIndexChanged">
                                <asp:ListItem Value="Add New" Selected="True">&nbsp;Add New</asp:ListItem>
                                <asp:ListItem Value="Copy from another Workpackage">&nbsp;Copy from another Workpackage</asp:ListItem>
                            </asp:RadioButtonList>
                        </div>
                        <div id="AddNew" runat="server">
                        <div class="form-group">
                            <label class="lblCss" for="txtworkpackage">Work package</label> 
                            <asp:TextBox ID="txtworkpackage" Enabled="false" CssClass="form-control" runat="server" ClientIDMode="Static"></asp:TextBox>
                        </div>
                        <div class="form-group">
                            <label class="lblCss" for="lstCategory">Select from Master</label>
                            <asp:ListBox ID="lstCategory" runat="server" CssClass="form-control" SelectionMode="Multiple">
                            </asp:ListBox>  
                        </div>
                        <div class="form-group">
                            <label class="lblCss" style="font-weight:bold; text-align:center;" for="OR">OR</label>
                            </div>
                         <div class="form-group">
                            <label class="lblCss" for="txtcategory">Enter Category Name</label> &nbsp;<span style="color:red; font-size:1.2rem;">*</span>
                            <asp:TextBox ID="txtcategory" CssClass="form-control" runat="server" autocomplete="off" ClientIDMode="Static" ></asp:TextBox>
                        </div>
                            </div>
                    <div id="CopyFrom" runat="server">
                        <div class="form-group">
                            <label class="lblCss" for="txtworkpackage">Work package</label>
                            <asp:DropDownList ID="DDLWorkpackage" runat="server" CssClass="form-control"></asp:DropDownList>
                        </div>
                    </div>
           </div>
                   
         </div>
         </div>
        <div class="modal-footer">
                    <%--<button type="button" class="btn btn-secondary" data-dismiss="modal">Close</button>--%>
            <asp:Button ID="btnSubmit" runat="server" CssClass="btn btn-primary" Text="Submit" OnClick="btnSubmit_Click" />
                    <%--<button type="button" class="btn btn-primary">Save changes</button>--%>
                </div>
        </form>
</asp:Content>
