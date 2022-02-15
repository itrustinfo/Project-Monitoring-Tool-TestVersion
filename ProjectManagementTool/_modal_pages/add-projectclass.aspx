<%@ Page Title="" Language="C#" MasterPageFile="~/_master_pages/modal.Master" AutoEventWireup="true" CodeBehind="add-projectclass.aspx.cs" Inherits="ProjectManagementTool._modal_pages.add_projectclass" %>
<asp:Content ID="Content1" ContentPlaceHolderID="modal_master_head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="modal_master_body" runat="server">
     <form id="AddProjectClass" runat="server">
        <div class="container-fluid" style="overflow-y:auto; max-height:85vh; min-height:76vh;">
        <div class="row">
            <div class="col-sm-12">
                 <div class="form-group">
                         <label class="lblCss" for="txtprojectclass">Project Category</label> &nbsp;<span style="color:red; font-size:1.2rem;">*</span>
                         <asp:TextBox ID="txtprojectclass" runat="server" CssClass="form-control" autocomplete="off" required></asp:TextBox>
                   </div>

                            <div class="form-group">
                                <label class="lblCss" for="txtprojectclassdesc">Description</label> &nbsp;<span style="color:red; font-size:1.2rem;">*</span>
                                <asp:TextBox ID="txtprojectclassdesc" runat="server" TextMode="MultiLine" CssClass="form-control" required></asp:TextBox>
                            </div>
               </div>
            </div>
        </div>

        <div class="modal-footer">
            <asp:Button ID="btnSubmit" runat="server" Text="Submit" CssClass="btn btn-primary" OnClick="btnSubmit_Click" />
                </div>
    </form>
</asp:Content>
