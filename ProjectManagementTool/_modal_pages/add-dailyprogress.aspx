<%@ Page Title="" Language="C#" MasterPageFile="~/_master_pages/modal.Master" AutoEventWireup="true" CodeBehind="add-dailyprogress.aspx.cs" Inherits="ProjectManagementTool._modal_pages.add_dailyprogress" %>

<asp:Content ID="Content1" ContentPlaceHolderID="modal_master_head" runat="server">
    <script type="text/javascript">
</script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="modal_master_body" runat="server">
    <form id="frmAddIssuesModal" runat="server">
        <asp:HiddenField ID="ProjectHiddenValue" runat="server" />
        <asp:HiddenField ID="WorkPackageHiddenValue" runat="server" />
        <asp:HiddenField ID="TaskHiddenValue" runat="server" />
        <div class="container-fluid" style="max-height: 80vh; overflow-y: auto;">
            <div class="row">
                <div class="col-sm-6">
                    <div class="form-group" id="Div1" runat="server">
                        <label class="lblCss" for="txtactivityname">Daily Progress Master</label>
                        <asp:DropDownList ID="DDLDailyReportMaster" CssClass="form-control"  runat="server" ></asp:DropDownList>
                    </div>
                    <div class="form-group" id="Div2" runat="server">
                        <label class="lblCss" for="txtactivityname">Zone Name</label>
                        <asp:TextBox ID="txtZoneName" runat="server" CssClass="form-control" required ></asp:TextBox>
                    </div>
                    <div class="form-group" id="AcvitityName" runat="server">
                        <label class="lblCss" for="txtactivityname">Vilage Name</label>
                        <asp:TextBox ID="txtVillageName" runat="server" CssClass="form-control" required ></asp:TextBox>
                    </div>
                    <div class="form-group">
                        <label class="lblCss" for="txtIssue_Description">Pipe dia in mm </label>
                        &nbsp;<span style="color: red; font-size: 1.1rem;">*</span>
                        <asp:TextBox ID="txtPipeDia" runat="server"  CssClass="form-control" required></asp:TextBox>
                    </div>
                    <div class="form-group">
                        <label class="lblCss" for="txtIssue_Description"> Quantity as per BOQ, RMT </label>
                        &nbsp;<span style="color: red; font-size: 1.1rem;">*</span>
                        <asp:TextBox ID="txtQuantity" runat="server"  CssClass="form-control" required></asp:TextBox>
                    </div>
                    <div class="form-group">
                        <label class="lblCss" for="txtIssue_Description">Revised Quantity as per Construction drawing, RMT</label>
                        &nbsp;<span style="color: red; font-size: 1.1rem;">*</span>
                        <asp:TextBox ID="txtRevisedQty" runat="server"  CssClass="form-control" required></asp:TextBox>
                    </div>
                    <div class="form-group">
                        <label class="lblCss" for="txtIssue_Description">Pipes received, RMT </label>&nbsp;<span style="color: red; font-size: 1.1rem;">*</span>
                        <asp:TextBox ID="txtPipesReceived" runat="server"  CssClass="form-control" required></asp:TextBox>
                    </div>
                </div>
                <div class="col-sm-6">
                    <div class="form-group">
                        <label class="lblCss" for="txtIssue_Description">Previous Qty</label>&nbsp;<span style="color: red; font-size: 1.1rem;">*</span>
                        <asp:TextBox ID="txtPreviousQty" runat="server"  CssClass="form-control" OnTextChanged="TxtId_TextChanged"  AutoPostBack="true" required></asp:TextBox>
                    </div>
                    <div class="form-group">
                        <label class="lblCss" for="txtIssue_Description">Today's Quantity</label>&nbsp;<span style="color: red; font-size: 1.1rem;">*</span>
                        <asp:TextBox ID="txtTodaysQty" runat="server"  CssClass="form-control" OnTextChanged="TxtId_TextChanged"  AutoPostBack="true" required></asp:TextBox>
                    </div>
                    <div class="form-group">
                        <label class="lblCss" for="txtIssue_Description">Total up to date Quantity</label>&nbsp;<span style="color: red; font-size: 1.1rem;">*</span>
                        <asp:TextBox ID="txtTotalQty" runat="server"  CssClass="form-control" required></asp:TextBox>
                    </div>
                    <div class="form-group">
                        <label class="lblCss" for="txtIssue_Description">Balance</label>&nbsp;<span style="color: red; font-size: 1.1rem;">*</span>
                        <asp:TextBox ID="txtBalance" runat="server"  CssClass="form-control" required></asp:TextBox>
                    </div>
                    <div class="form-group">
                        <label class="lblCss" for="txtRefNumber">Remarks</label>
                        <asp:TextBox ID="txtRemarks" CssClass="form-control" TextMode="MultiLine" runat="server" ClientIDMode="Static"></asp:TextBox>
                    </div>

                </div>
            </div>
        </div>
        <div class="modal-footer">
            <asp:Button ID="btnSubmit" runat="server" Text="Submit" CssClass="btn btn-primary" OnClick="btnSubmit_Click" />
        </div>
    </form>
</asp:Content>


