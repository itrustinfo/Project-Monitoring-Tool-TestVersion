<%@ Page Title="" Language="C#" MasterPageFile="~/_master_pages/modal.Master" AutoEventWireup="true" CodeBehind="add-financemilestone.aspx.cs" Inherits="ProjectManagementTool._modal_pages.add_financemilestone" %>
<asp:Content ID="Content1" ContentPlaceHolderID="modal_master_head" runat="server">
    <script type="text/javascript">
          function sum() {
           var txtFirstNumberValue = document.getElementById('<%= txtallowedpayment.ClientID %>').value.replace(/,/g, '').replace('₹ ', '').replace('$ ', '').replace('¥ ', '').replace('₹', '').replace('$', '').replace('¥', '');
           var txtSecondNumberValue = document.getElementById('<%= txtGST.ClientID %>').value.replace(/,/g, '').replace('₹ ', '').replace('$ ', '').replace('¥ ', '').replace('₹', '').replace('$', '').replace('¥', '');
           var result = parseInt(txtFirstNumberValue) + ((parseInt(txtFirstNumberValue) * parseInt(txtSecondNumberValue)) / 100);
           if (!isNaN(result)) {
               var x = result.toString()
               x = x.toString();
               var ddlcurrency = document.getElementById('<%= HiddenVal.ClientID %>').value;
               if (ddlcurrency == "₹") {
                   if (x.indexOf('.') > 0) {
                       var n1, n2;
                       x = x + '' || '';
                       // works for integer and floating as well
                       n1 = x.split('.');
                       n2 = n1[1] || null;
                       n1 = n1[0].replace(/(\d)(?=(\d\d)+\d$)/g, "$1,");
                       x = n2 ? n1 + '.' + n2 : n1;
                       result = x;
                   }
                   else {
                       var lastThree = x.substring(x.length - 3);
                       var otherNumbers = x.substring(0, x.length - 3);
                       if (otherNumbers != '')
                           lastThree = ',' + lastThree;
                       var res = otherNumbers.replace(/\B(?=(\d{2})+(?!\d))/g, ",") + lastThree;
                       result = res;
                   }
                   document.getElementById('<%= txttotalpayment.ClientID %>').value = ddlcurrency + ' ' + result;
               }
               else {
                     x = x.split('.');
                     var x1 = x[0];
                     var x2 = x.length > 1 ? '.' + x[1] : '';
                     var rgx = /(\d+)(\d{3})/;
                     while (rgx.test(x1)) {
                         x1 = x1.replace(rgx, '$1' + ',' + '$2');
                   }
                   result = x1 + x2;
                   document.getElementById('<%= txttotalpayment.ClientID %>').value = ddlcurrency + ' ' +result;
               }
           }
        }
        function toUSD(objctrl) {
           var x = objctrl.value.replace(/,/g, '').replace('₹ ', '').replace('$ ', '').replace('¥ ', '').replace('₹', '').replace('$', '').replace('¥', '');
           x = x.toString();
           x += '';
           var ddlcurrency = document.getElementById('<%= HiddenVal.ClientID %>').value;
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
     <script type="text/javascript">
     $( function() {
        $("input[id$='dtPlannedDate']").datepicker({
          changeMonth: true,
            changeYear: true,
          dateFormat:'dd/mm/yy'
          });
        });
</script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="modal_master_body" runat="server">
    <form id="frmAddFinanceMileStoneModal" runat="server">
        <asp:HiddenField ID="HiddenVal" runat="server" />
        <asp:HiddenField ID="HiddenCurrency_CultureInfo" runat="server" />
        <asp:HiddenField ID="TaskBudget" runat="server" />
        <div class="container-fluid">
            <div class="row">
                <div class="col-sm-12">
                    
                    <div class="form-group">
                        <label class="lblCss" for="txtMileStone">MileStone Name</label> &nbsp;<span style="color:red; font-size:1.2rem;">*</span>
                        <asp:TextBox ID="txtMileStone" CssClass="form-control" runat="server" required  autocomplete="off" ClientIDMode="Static"></asp:TextBox>
                    </div>
                    <div class="form-group">
                        <label class="lblCss" for="dtTargetDate">Allowed Payment</label> &nbsp;<span style="color:red; font-size:1.2rem;">*</span>
                        <asp:TextBox ID="txtallowedpayment" CssClass="form-control" runat="server" required autocomplete="off" ClientIDMode="Static" onkeyup="javascript:sum();" onblur="toUSD(this)"></asp:TextBox>
                    </div>
                     <div class="form-group">
                        <label class="lblCss" for="dtTargetDate">GST(%)</label> &nbsp;<span style="color:red; font-size:1.2rem;">*</span>
                        <asp:TextBox ID="txtGST" CssClass="form-control" runat="server" TextMode="Number" required autocomplete="off" ClientIDMode="Static" onkeyup="javascript:sum();"></asp:TextBox>
                    </div>
                    <div class="form-group">
                        <label class="lblCss" for="dtTargetDate">Total Payment</label> &nbsp;<span style="color:red; font-size:1.2rem;">*</span>
                        <asp:TextBox ID="txttotalpayment" CssClass="form-control" Enabled="false" runat="server" autocomplete="off" onblur="toUSD(this)" ClientIDMode="Static"></asp:TextBox>
                    </div>
                     <div class="form-group">
                        <label class="lblCss" for="dtPlannedDate">Planned Date</label> &nbsp;<span style="color:red; font-size:1.2rem;">*</span>
                        <asp:TextBox ID="dtPlannedDate" CssClass="form-control" runat="server" required placeholder="dd/mm/yyyy" autocomplete="off" ClientIDMode="Static"></asp:TextBox>
                    </div>
                </div>
        </div>
            </div>
        <div class="container-lg">
            <div class="row">
                <div class="col-sm-12">
        <div class="modal-footer">
                    <%--<button type="button" class="btn btn-secondary" data-dismiss="modal">Close</button>--%>
                    <%--<button type="button" class="btn btn-primary">Save changes</button>--%>
            <asp:Button ID="btnSubmit" runat="server" Text="Submit" CssClass="btn btn-primary" OnClick="btnSubmit_Click" />
            </div>
                </div>
                </div>
            </div>
    </form>
</asp:Content>
