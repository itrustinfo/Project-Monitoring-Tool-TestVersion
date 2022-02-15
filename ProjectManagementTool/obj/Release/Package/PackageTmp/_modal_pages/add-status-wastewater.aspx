<%@ Page Title="" Language="C#" MasterPageFile="~/_master_pages/modal.Master" AutoEventWireup="true" CodeBehind="add-status-wastewater.aspx.cs" Inherits="ProjectManagementTool._modal_pages.add_status_wastewater" ValidateRequest="false" %>
<asp:Content ID="Content1" ContentPlaceHolderID="modal_master_head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="modal_master_body" runat="server">
    <form id="frmAddIssuesModal" runat="server">
        <div class="container-fluid" style="max-height:80vh; overflow-y:auto;">
            <div class="row">
                <div class="col-sm-6">
                    <div class="form-group">
                        <label class="lblCss" for="DDlProject">Project</label>
                         <asp:DropDownList ID="DDlProject" runat="server" CssClass="form-control" AutoPostBack="true"></asp:DropDownList>
                    </div>
                      <div class="form-group">
                        <label class="lblCss" for="txtContractorName">Package Description</label>
                        <asp:TextBox ID="txtPackageDescription" runat="server"  CssClass="form-control" required></asp:TextBox>
                    </div>
                       <div class="form-group">
                        <label class="lblCss" for="txtAwardedCost">Awarded Cost / Sanction Cost (Crores)</label>
                        <asp:TextBox ID="txtAwardedCost" runat="server"  CssClass="form-control" required></asp:TextBox>
                    </div>
                    
                    
                </div>
                <div class="col-sm-6">
                     <div class="form-group">
                        <label class="lblCss" for="ddlComponenttype">Component Type</label>
                         <asp:DropDownList ID="ddlComponenttype" runat="server" CssClass="form-control">
                             <asp:ListItem>Sewerage Component</asp:ListItem>
                               <asp:ListItem>MIS Component</asp:ListItem>
                         </asp:DropDownList>
                    </div>
                     <div class="form-group">
                        <label class="lblCss" for="txtProjectComponents">Project Components</label>
                        <asp:TextBox ID="txtProjectComponents" runat="server" TextMode="MultiLine"  CssClass="form-control" required></asp:TextBox>
                    </div>
                     <div class="form-group">
                        <label class="lblCss" for="txtPresentStatus">Present Status</label>
                        <asp:TextBox ID="txtPresentStatus" runat="server" TextMode="MultiLine"  CssClass="form-control" required></asp:TextBox>
                    </div>
                   
                    </div>
            </div> 
        </div>
        <div class="modal-footer">
            <asp:Button ID="btnSubmit" runat="server" Text="Submit" CssClass="btn btn-primary" OnClick="btnSubmit_Click" />
                </div>
    </form>
</asp:Content>
