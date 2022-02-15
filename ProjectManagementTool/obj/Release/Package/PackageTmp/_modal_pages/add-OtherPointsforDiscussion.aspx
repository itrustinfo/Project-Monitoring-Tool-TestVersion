<%@ Page Title="" Language="C#" MasterPageFile="~/_master_pages/modal.Master" AutoEventWireup="true" CodeBehind="add-OtherPointsforDiscussion.aspx.cs" Inherits="ProjectManagementTool._modal_pages.add_OtherPointsforDiscussion" %>
<asp:Content ID="Content1" ContentPlaceHolderID="modal_master_head" runat="server">
     <style type="text/css">
        textarea
        {
            resize: none;
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="modal_master_body" runat="server">
    <form id="FrmAddResource" runat="server">
        <asp:ScriptManager ID="ScriptManager1" runat="server"></asp:ScriptManager>
        <div class="container-fluid" style="max-height:80vh; overflow-y:auto; margin-top:15px;">
            <div class="row">
                <div class="col-sm-12">
                      <div class="form-group">
                        <label class="lblCss" for="DDLMeeting">Select Meeting</label>
                        <asp:DropDownList ID="ddlMeeting" runat="server" CssClass="form-control" ></asp:DropDownList>
                    </div>
                    <div class="form-group">
                        <label class="lblCss" for="txtdesc">Other Points</label> &nbsp;<span style="color:red; font-size:1.1rem;">*</span>
                         <asp:TextBox ID="txtdesc" runat="server" CssClass="form-control" TextMode="MultiLine" autocomplete="off" required></asp:TextBox>
                        <asp:HiddenField ID="hiduid" runat="server" />
                    </div>
                 </div>
            </div> 
        </div>

        <div class="modal-footer">
                  
            <asp:Button ID="btnAdd" runat="server" Text="Add" CssClass="btn btn-primary" OnClick="btnAdd_Click"  />
            <asp:Button ID="btnUpdate" runat="server" Text="Update" CssClass="btn btn-primary" OnClick="btnUpdate_Click"  />
                </div>
    </form>
</asp:Content>
