<%@ Page Title="" Language="C#" MasterPageFile="~/_master_pages/modal.Master" AutoEventWireup="true" CodeBehind="add-paymentbreakupterms.aspx.cs" Inherits="ProjectManagementTool._modal_pages.add_paymentbreakupterms" %>
<asp:Content ID="Content1" ContentPlaceHolderID="modal_master_head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="modal_master_body" runat="server">
    <form id="frmAddSubmittalModal" runat="server">
        <asp:ScriptManager ID="ScriptManager1" runat="server"></asp:ScriptManager>
        <div class="container-fluid" style="overflow-y:auto;">
            <div class="row">
                <div class="col-sm-12">
                    <%--<div class="form-group">
                        <label class="lblCss" for="DDLBOQItem">BOQ Item</label>
                         <asp:DropDownList ID="DDLBOQItem" CssClass="form-control" Enabled="false" runat="server"></asp:DropDownList>
                    </div>--%>
                    
                     <div class="form-group">
                        <label class="lblCss" for="txtcameraname">Payment Breakup Type</label> &nbsp;<span style="color:red; font-size:1.1rem;">*</span>
                          <asp:DropDownList ID="DDLPaymentBreakupType" CssClass="form-control" required runat="server"></asp:DropDownList>
                    </div>

                    <div class="form-group">
                        <label class="lblCss" for="txtpercentage">Percentage</label> &nbsp;<span style="color:red; font-size:1.1rem;">*</span>
                        <asp:TextBox ID="txtpercentage" CssClass="form-control" runat="server" autocomplete="off" required ClientIDMode="Static"></asp:TextBox>
                        
                    </div>
                    <div class="form-group">
                        <label class="lblCss" for="txtdesc">Description</label>
                        <asp:TextBox ID="txtdesc" CssClass="form-control" runat="server" TextMode="MultiLine" autocomplete="off" ClientIDMode="Static"></asp:TextBox>
                        
                    </div>
                </div>
            </div> 
        </div>

        <div class="modal-footer">
            <asp:Button ID="btnSubmit" runat="server" Text="Submit" CssClass="btn btn-primary" OnClick="btnSubmit_Click"/>
                </div>
    </form>
</asp:Content>
