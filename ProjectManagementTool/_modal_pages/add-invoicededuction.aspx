<%@ Page Title="" Language="C#" MasterPageFile="~/_master_pages/modal.Master" AutoEventWireup="true" CodeBehind="add-invoicededuction.aspx.cs" Inherits="ProjectManagementTool._modal_pages.add_invoicededuction" %>
<asp:Content ID="Content1" ContentPlaceHolderID="modal_master_head" runat="server">
    <style type="text/css">
         .hideItem {
         display:none;
         
     }
    </style>
    <script type="text/javascript">
        function CalCulateAmount() {
            var InvoiceAmount = document.getElementById('<%= txtInvoiceamount.ClientID %>').value.replace(/,/g, '').replace('₹ ', '').replace('$ ', '').replace('¥ ', '').replace('₹', '').replace('$', '').replace('¥', '');
            var Percentage = document.getElementById('<%= txtperentage.ClientID %>').value.replace(/,/g, '');
            if (InvoiceAmount != "" && Percentage != "") {
                var result;
                //alert("InvoiceAmount=" + InvoiceAmount + " Percentage=" + Percentage);
                var GSTCalculation = document.getElementById('<%= Hidden1.ClientID %>').value;
                if (GSTCalculation != "") {
                    result = (InvoiceAmount / GSTCalculation);
                    //alert("Before=" + result);
                    result = ((result * Percentage) / 100);
                    //alert("Final=" + result);
                }
                else {
                    result = ((InvoiceAmount * Percentage) / 100);
                }
                
                var ddlcurrency = document.getElementById('<%= DDLCurrency.ClientID %>').value;
                var x = result.toFixed(2);
                x = x.toString();
                 if (ddlcurrency == "₹") {
                     if (x.indexOf('.') > 0) {
                         
                         var n1, n2;
                         x = x + '' || '';
                         // works for integer and floating as well
                         n1 = x.split('.');
                         n2 = n1[1] || null;
                         n1 = n1[0].replace(/(\d)(?=(\d\d)+\d$)/g, "$1,");
                         x = n2 ? n1 + '.' + n2 : n1;
                         document.getElementById('<%= txtamount.ClientID %>').value = ddlcurrency + ' ' + x;
                     }
                     else {
                         
                         var lastThree = x.substring(x.length - 3);
                         var otherNumbers = x.substring(0, x.length - 3);
                         if (otherNumbers != '')
                             lastThree = "," + lastThree;
                         var res = otherNumbers.replace(/\B(?=(\d{2})+(?!\d))/g, ",") + lastThree;
                         document.getElementById('<%= txtamount.ClientID %>').value = ddlcurrency + ' ' + res;
                     }
                 }
                 else {
                     
                     x = x.split('.');
                     var x1 = x[0];
                     var x2 = x.length > 1 ? '.' + x[1] : '';
                     var rgx = /(\d+)(\d{3})/;
                     while (rgx.test(x1)) {
                         x1 = x1.replace(rgx, '$1' + ',' + '$2');
                     }
                     document.getElementById('<%= txtamount.ClientID %>').value = ddlcurrency + ' ' + (x1 + x2);
                     
                 }
                //document.getElementById('<%= txtamount.ClientID %>').disabled = true;
            }
        }
             function ShowCurrencyFormat() {
             var Budget = document.getElementById('<%= txtamount.ClientID %>').value.replace(/,/g, '').replace('₹ ', '').replace('$ ', '').replace('¥ ', '').replace('₹', '').replace('$', '').replace('¥', '');
             if (Budget != "") {
                 var x = Budget;
                 x = x.toString();
                 //x = x.replace(",", "");
                 x += '';                 
                 var ddlcurrency = document.getElementById('<%= DDLCurrency.ClientID %>').value;
                 if (ddlcurrency == "₹") {
                     if (x.indexOf('.') > 0) {
                         
                         var n1, n2;
                         x = x + '' || '';
                         // works for integer and floating as well
                         n1 = x.split('.');
                         n2 = n1[1] || null;
                         n1 = n1[0].replace(/(\d)(?=(\d\d)+\d$)/g, "$1,");
                         x = n2 ? n1 + '.' + n2 : n1;
                         document.getElementById('<%= txtamount.ClientID %>').value = ddlcurrency + ' ' + x;
                     }
                     else {
                         
                         var lastThree = x.substring(x.length - 3);
                         var otherNumbers = x.substring(0, x.length - 3);
                         if (otherNumbers != '')
                             lastThree = "," + lastThree;
                         var res = otherNumbers.replace(/\B(?=(\d{2})+(?!\d))/g, ",") + lastThree;
                         document.getElementById('<%= txtamount.ClientID %>').value = ddlcurrency + ' ' + res;
                     }
                 }
                 else {
                     
                     x = x.split('.');
                     var x1 = x[0];
                     var x2 = x.length > 1 ? '.' + x[1] : '';
                     var rgx = /(\d+)(\d{3})/;
                     while (rgx.test(x1)) {
                         x1 = x1.replace(rgx, '$1' + ',' + '$2');
                     }
                     document.getElementById('<%= txtamount.ClientID %>').value = ddlcurrency + ' ' + (x1 + x2);
                     
                 }
             }
        }


        function toUSD(objctrl) {
            var x = objctrl.value.replace(/,/g, '').replace('₹ ', '').replace('$ ', '').replace('¥ ', '').replace('₹', '').replace('$', '').replace('¥', '');
            x = x.toString();
            x += '';
            var ddlcurrency = document.getElementById('<%= DDLCurrency.ClientID %>').value;
            if (ddlcurrency == "₹") {
                if (x.indexOf('.') > 0) {
                    var n1, n2;

                    x = x + '' || '';
                    // works for integer and floating as well
                    n1 = x.split('.');
                    n2 = n1[1] || null;
                    n1 = n1[0].replace(/(\d)(?=(\d\d)+\d$)/g, "$1,");
                    x = n2 ? n1 + '.' + n2 : n1;
                    objctrl.value = ddlcurrency + ' ' + x;
                }
                else {
                    var lastThree = x.substring(x.length - 3);
                    var otherNumbers = x.substring(0, x.length - 3);
                    if (otherNumbers != '')
                        lastThree = ',' + lastThree;
                    var res = otherNumbers.replace(/\B(?=(\d{2})+(?!\d))/g, ",") + lastThree;
                    objctrl.value = ddlcurrency + ' ' + res;
                }
            }
            else {
                x = x.split('.');
                var x1 = x[0];
                var x2 = x.length > 1 ? '.' + x[1] : '';
                var rgx = /(\d+)(\d{3})/;
                while (rgx.test(x1)) {
                    x1 = x1.replace(rgx, '$1' + ',' + '$2');
                }
                objctrl.value = ddlcurrency + ' ' + (x1 + x2);
            }
        }

    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="modal_master_body" runat="server">
    <form id="frmAddInvoiceDeductionModal" runat="server">
        <asp:HiddenField ID="Hidden1" runat="server" />
        <div class="container-fluid" style="max-height:80vh; overflow-y:auto;">
            <div class="row">
                <div class="col-sm-12">
                    <div class="form-group">
                        <label class="lblCss" for="DDLInvoiceMaster">Invoice Number</label> &nbsp;<span style="color:red; font-size:1.1rem;">*</span>
                         <asp:DropDownList ID="DDLInvoiceMaster" CssClass="form-control" Enabled="false"  runat="server" required></asp:DropDownList>
                    </div>
                    <div class="form-group">
                        <label class="lblCss" for="DDLDeduction">Deduction</label> &nbsp;<span style="color:red; font-size:1.1rem;">*</span>
                         <asp:DropDownList ID="DDLDeduction" CssClass="form-control"  runat="server" AutoPostBack="true" OnSelectedIndexChanged="DDLDeduction_SelectedIndexChanged" required></asp:DropDownList>
                    </div>
                    <div class="form-group">
                        <label class="lblCss" for="txtbudget">Currency</label> &nbsp;<span style="color:red; font-size:1.1rem;">*</span>
                         <asp:DropDownList ID="DDLCurrency" runat="server" CssClass="form-control" Enabled="false" onchange="ShowCurrencyFormat()">
                             <asp:ListItem Value="&#x20B9;">&#x20B9; (RUPEE)</asp:ListItem>
                             <asp:ListItem Value="&#36;">&#36; (USD)</asp:ListItem>
                             <asp:ListItem Value="&#165;">&#165; (YEN)</asp:ListItem>
                         </asp:DropDownList>
                    </div> 
                    <div class="form-group">
                        <label class="lblCss" for="txtbudget">Dedcution Mode</label> &nbsp;<span style="color:red; font-size:1.1rem;">*</span>
                        <asp:DropDownList ID="DDLMode" runat="server" CssClass="form-control" AutoPostBack="true" OnSelectedIndexChanged="DDLMode_SelectedIndexChanged">
                            <asp:ListItem Value="Percentage" Selected="True">Percentage</asp:ListItem>
                            <asp:ListItem Value="Amount">Amount</asp:ListItem>
                        </asp:DropDownList>
                    </div>
                    <div class="form-group" id="PercentageDiv" runat="server">
                          <label class="lblCss" for="txtperentage">Percentage</label> &nbsp;<span style="color:red; font-size:1.1rem;">*</span>
                          <%--<asp:TextBox ID="txtperentage" runat="server" CssClass="form-control" autocomplete="off" required></asp:TextBox>--%>
                        <input type="text" id="txtperentage" runat="server" class="form-control" autocomplete="off" onchange="CalCulateAmount()" required />        
                            <asp:TextBox ID="txtInvoiceamount" runat="server" CssClass="hideItem" ></asp:TextBox>
                    </div>
                    <div class="form-group">
                          <label class="lblCss" for="txtamount">Amount</label> &nbsp;<span style="color:red; font-size:1.1rem;">*</span>
                        <%--<asp:TextBox ID="txtamount" runat="server" Enabled="false" CssClass="form-control" EnableViewState="true" ClientIDMode="Static"></asp:TextBox>--%>
                          <input type="text" id="txtamount" runat="server" class="form-control" autocomplete="off" onblur="toUSD(this)" required />        
                    </div>
                </div>
            </div> 
        </div>
        <div class="modal-footer">
            <asp:Button ID="btnSubmit" runat="server" Text="Submit" CssClass="btn btn-primary" OnClick="btnSubmit_Click" />
                </div>
    </form>
</asp:Content>
