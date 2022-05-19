<%@ Page Language="C#" MasterPageFile="~/_master_pages/modal.Master" AutoEventWireup="true" CodeBehind="Add-MobilisationAdvance.aspx.cs" Inherits="ProjectManagementTool._modal_pages.Add_MobilisationAdvance" %>

<asp:Content ID="Content1" ContentPlaceHolderID="modal_master_head" runat="server">
    <script>
        $(function () {
            $("input[id$='txtGivenDate']").datepicker({
                changeMonth: true,
                changeYear: true,
                dateFormat: 'dd/mm/yy'
            });
        });
  </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="modal_master_body" runat="server">
     <form id="FrmAddMobilisationAdvance" runat="server">
         
        <div class="container-fluid" style="min-height:70vh;">

            <div class="row">
                 <asp:HiddenField ID="hdnMobilizationAdvanceUID" runat="server" />

                <div class="col-sm-12">
                    <div class="form-group">
                        <label class="lblCss" for="txtGivenDate">Transaction Date</label> &nbsp;<span style="color:red; font-size:1.2rem;">*</span>
                        <%--<asp:TextBox   ID="txtGivenDate" runat="server" CssClass="form-control" autocomplete="off" DataFormatString="{0:dd/MM/yyyy}" ClientIDMode="Static" required  ></asp:TextBox>--%>
                    <asp:TextBox ID="txtGivenDate" CssClass="form-control" runat="server" placeholder="dd/mm/yyyy" autocomplete="off" required ClientIDMode="Static"></asp:TextBox>
                    </div>
                </div>
                <div class="col-sm-12">
                    <div class="form-group">
                        <label class="lblCss" for="txtInvoiceNumber">Invoice Number</label> &nbsp;<span style="color:red; font-size:1.2rem;">*</span>
                        <asp:TextBox ID="txtInvoiceNo" runat="server" CssClass="form-control" autocomplete="off" required=""></asp:TextBox>
                    </div>
                </div>
                <div class="col-sm-12">
                    <div class="form-group">
                        <label class="lblCss" for="txtAdvance">Advance Amount</label> &nbsp;<span style="color:red; font-size:1.2rem;">*</span>
                        <asp:TextBox ID="txtAdvance" runat="server" CssClass="form-control" autocomplete="off" required="0" TextMode ="Number"></asp:TextBox>
                    </div>
                </div>

                <%--<div class="col-sm-12">
                    <div class="form-group">
                        <label class="lblCss" for="ddlTransType">Transaction Type</label> &nbsp;<span style="color:red; font-size:1.2rem;">*</span>
                        <asp:DropDownList ID="txtTransType" runat="server" CssClass="form-control" autocomplete="on" >
                            <asp:listitem>Debit</asp:listitem>
                            <asp:listitem>Credit</asp:listitem>
                        </asp:DropDownList>
                    </div>
                </div>--%>
            </div>
        </div>

        <div class="modal-footer">
            <asp:Button ID="btnSubmit" runat="server" Text="SUBMIT" CssClass="btn btn-primary" OnClick="btnSubmit_Click" />
        </div>
    </form>
</asp:Content>
