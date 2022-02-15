<%@ Page Title="" Language="C#" MasterPageFile="~/_master_pages/modal.Master" AutoEventWireup="true" CodeBehind="add-Budget_disburse.aspx.cs" Inherits="ProjectManagementTool._modal_pages.add_Budget_disburse" %>
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
                         <asp:TextBox ID="txtProject" runat="server" Visible="false"  CssClass="form-control"></asp:TextBox>
                    </div>
                      <div class="form-group">
                        <label class="lblCss" for="txtContractorName">Contractor Name</label>
                        <asp:TextBox ID="txtContractorName" runat="server"  CssClass="form-control" required></asp:TextBox>
                    </div>
                       <div class="form-group">
                        <label class="lblCss" for="txtAwardedCost">Awarded Cost / Sanction Cost (Crores)</label>
                        <asp:TextBox ID="txtAwardedCost" runat="server"  CssClass="form-control" required></asp:TextBox>
                    </div>
                     <div class="form-group">
                        <label class="lblCss" for="txtDisburseFY">Disbursement for FY 2019-20&nbsp; (MJPY)</label><asp:TextBox ID="txtDisburseFY" runat="server"  CssClass="form-control" required></asp:TextBox>
                    </div>
                    <div class="form-group">
                        <label class="lblCss" for="txtDisburseFY2021">Disbursement for FY 2020-21&nbsp; (MJPY)</label><asp:TextBox ID="txtDisburseFY2021" runat="server"  CssClass="form-control" required></asp:TextBox>
                    </div>
                    <div class="form-group">
                        <label class="lblCss" for="txtActualQ1">Actual Disbursement Q1 &nbsp; (MJPY)</label>
                        <asp:TextBox ID="txtActualQ1" runat="server"  CssClass="form-control" required></asp:TextBox>
                    </div>
                     <div class="form-group">
                        <label class="lblCss" for="txtActualQ2">Actual Disbursement Q2&nbsp; (MJPY)</label>
                        <asp:TextBox ID="txtActualQ2" runat="server"  CssClass="form-control" required></asp:TextBox>
                    </div>
                     <div class="form-group">
                        <label class="lblCss" for="txtActualQ3">Actual Disbursement Q3&nbsp; (MJPY)</label>
                        <asp:TextBox ID="txtActualQ3" runat="server"  CssClass="form-control" required></asp:TextBox>
                    </div>
                     <div class="form-group">
                        <label class="lblCss" for="txtActualQ4">Actual Disbursement Q4&nbsp; (MJPY)</label>
                        <asp:TextBox ID="txtActualQ4" runat="server"  CssClass="form-control" required></asp:TextBox>
                    </div>
                </div>
                <div class="col-sm-6">
                     <div class="form-group">
                        <label class="lblCss" for="txtBudgetQ1">Budget Q1&nbsp; (MJPY)</label>
                        <asp:TextBox ID="txtBudgetQ1" runat="server"  CssClass="form-control" required></asp:TextBox>
                    </div>
                     <div class="form-group">
                        <label class="lblCss" for="txtBudgetQ2">Budget Q2&nbsp; (MJPY)</label>
                        <asp:TextBox ID="txtBudgetQ2" runat="server"  CssClass="form-control" required></asp:TextBox>
                    </div>
                    <div class="form-group">
                        <label class="lblCss" for="txtBudgetQ3">Budget Q3&nbsp; (MJPY)</label>
                        <asp:TextBox ID="txtBudgetQ3" runat="server"  CssClass="form-control" required></asp:TextBox>
                    </div>
                     <div class="form-group">
                        <label class="lblCss" for="txtBudgetQ4">Budget Q4&nbsp; (MJPY)</label>
                        <asp:TextBox ID="txtBudgetQ4" runat="server"  CssClass="form-control" required></asp:TextBox>
                    </div>
                     
                   
                    </div>
            </div> 
        </div>
        <div class="modal-footer">
            <asp:Button ID="btnSubmit" runat="server" Text="Submit" CssClass="btn btn-primary" OnClick="btnSubmit_Click" />
                </div>
    </form>
</asp:Content>
