<%@ Page Title="" Language="C#" MasterPageFile="~/_master_pages/modal.Master" AutoEventWireup="true" CodeBehind="add-Insurance.aspx.cs" Inherits="ProjectManagementTool._modal_pages.add_Insurance" %>
<asp:Content ID="Content1" ContentPlaceHolderID="modal_master_head" runat="server">
    <script type="text/javascript">
         function ShowCurrencyFormat() {
             var Budget = document.getElementById('<%= txtamount.ClientID %>').value.replace(/,/g, '').replace('₹ ', '').replace('$ ', '').replace('¥ ', '').replace('₹', '').replace('$', '').replace('¥', '');
             var Premium = document.getElementById('<%= txtpremiumamount.ClientID %>').value.replace(/,/g, '').replace('₹ ', '').replace('$ ', '').replace('¥ ', '').replace('₹', '').replace('$', '').replace('¥', '');
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

               if (Premium != "") {
                 var x = Premium;
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
                         document.getElementById('<%= txtpremiumamount.ClientID %>').value = ddlcurrency + ' ' + x;
                     }
                     else {
                         
                         var lastThree = x.substring(x.length - 3);
                         var otherNumbers = x.substring(0, x.length - 3);
                         if (otherNumbers != '')
                             lastThree = "," + lastThree;
                         var res = otherNumbers.replace(/\B(?=(\d{2})+(?!\d))/g, ",") + lastThree;
                         document.getElementById('<%= txtpremiumamount.ClientID %>').value = ddlcurrency + ' ' + res;
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
                     document.getElementById('<%= txtpremiumamount.ClientID %>').value = ddlcurrency + ' ' + (x1 + x2);
                 }
             }
         }

function toUSD(objctrl) {
           var x = objctrl.value.replace(/,/g, '');
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
                 objctrl.value = x;
                 }
                 else {
                      var lastThree = x.substring(x.length-3);
                     var otherNumbers = x.substring(0,x.length-3);
                     if(otherNumbers != '')
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
    $("input[id$='dtMaturityDate']").datepicker({
      changeMonth: true,
        changeYear: true,
      dateFormat:'dd/mm/yy'
      });

      $("input[id$='dtInsuredDate']").datepicker({
      changeMonth: true,
        changeYear: true,
      dateFormat:'dd/mm/yy'
      });
     $("input[id$='dtFirstPremiunDueDate']").datepicker({
      changeMonth: true,
        changeYear: true,
      dateFormat:'dd/mm/yy'
      });
    });
</script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="modal_master_body" runat="server">
    <form id="frmAddInsuranceModal" runat="server">
        <div class="container-fluid" style="max-height:80vh; overflow-y:auto;">
            <div class="row">
                <div class="col-sm-6">
                    <div class="form-group">
                        <label class="lblCss" for="DDlProject">Project</label>
                         <asp:DropDownList ID="DDlProject" runat="server" CssClass="form-control" AutoPostBack="true" OnSelectedIndexChanged="DDlProject_SelectedIndexChanged"></asp:DropDownList>
                    </div>
                    <div class="form-group">
                        <label class="lblCss" for="txtIssue_Description">WorkPackage</label>
                         <asp:DropDownList ID="DDLWorkPackage" CssClass="form-control" runat="server">
                       </asp:DropDownList>
                    </div>
                    <div class="form-group">
                        <label class="lblCss" for="txtvendorname">Vendor Name</label> &nbsp;<span style="color:red; font-size:1.1rem;">*</span>
                         <asp:TextBox ID="txtvendorname" runat="server" CssClass="form-control" autocomplete="off" required></asp:TextBox>
                    </div>
                     <div class="form-group">
                        <label class="lblCss" for="txtvendoraddress">Vendor Address</label> &nbsp;<span style="color:red; font-size:1.1rem;">*</span>
                         <asp:TextBox ID="txtvendoraddress" TextMode="MultiLine" runat="server" autocomplete="off" CssClass="form-control" required></asp:TextBox>
                    </div>
                    <div class="form-group">
                        <label class="lblCss" for="txtName_of_InsuranceCompany">Name of Insurance Company </label> &nbsp;<span style="color:red; font-size:1.1rem;">*</span>
                         <asp:TextBox ID="txtName_of_InsuranceCompany" runat="server" CssClass="form-control" autocomplete="off" required></asp:TextBox>
                    </div>
                   <div class="form-group">
                        <label class="lblCss" for="txtBranch">Insurance Company Branch</label> &nbsp;<span style="color:red; font-size:1.1rem;">*</span>
                         <asp:TextBox ID="txtBranch" runat="server" CssClass="form-control" autocomplete="off" required></asp:TextBox>
                    </div>
                    <div class="form-group">
                        <label class="lblCss" for="txtNominee">Nominee</label> &nbsp;<span style="color:red; font-size:1.1rem;">*</span>
                        <asp:TextBox ID="txtNominee" CssClass="form-control" runat="server" autocomplete="off" required ClientIDMode="Static"></asp:TextBox>
                    </div>
                </div>
                <div class="col-sm-6">
                     <div class="form-group">
                        <label class="lblCss" for="txtPolicy_Number">Policy Number</label> &nbsp;<span style="color:red; font-size:1.1rem;">*</span>
                       <input type="text" id="txtPolicy_Number" runat="server" class="form-control" autocomplete="off" required />
                        
                    </div>
                     <div class="form-group">
                        <label class="lblCss" for="txtamount">Policy Status</label> &nbsp;<span style="color:red; font-size:1.1rem;">*</span>
                       <asp:DropDownList ID="DDLPolicyStatus" CssClass="form-control" runat="server">
                         <asp:ListItem Value="Active">Active</asp:ListItem>
                         <asp:ListItem Value="Inforce">Inforce</asp:ListItem>
                         <asp:ListItem Value="Suspended">Suspended</asp:ListItem>
                         <asp:ListItem Value="Expired">Expired</asp:ListItem>
                     </asp:DropDownList>
                        
                    </div>
                     <div class="form-group">
                        <label class="lblCss" for="txtbudget">Currency</label>
                         <asp:DropDownList ID="DDLCurrency" runat="server" CssClass="form-control" onchange="ShowCurrencyFormat()">
                             <asp:ListItem Value="&#x20B9;">&#x20B9; (RUPEE)</asp:ListItem>
                             <asp:ListItem Value="&#36;">&#36; (USD)</asp:ListItem>
                             <asp:ListItem Value="&#165;">&#165; (YEN)</asp:ListItem>
                         </asp:DropDownList>
                    </div>  
                    <div class="form-group">
                        <label class="lblCss" for="txtamount">Insured Amount</label> &nbsp;<span style="color:red; font-size:1.1rem;">*</span>
                       <input type="text" id="txtamount" runat="server" class="form-control" autocomplete="off" onblur="toUSD(this)" required />
                        
                    </div>
                    <div class="form-group">
                        <label class="lblCss" for="txtpremiumamount">Premium/Installment Amount</label> &nbsp;<span style="color:red; font-size:1.1rem;">*</span>
                       <input type="text" id="txtpremiumamount" runat="server" class="form-control" autocomplete="off" onblur="toUSD(this)" required />
                    </div>
                    <div class="form-group">
                        <label class="lblCss" for="dtFirstPremiunDueDate">First Premium Due Date</label> &nbsp;<span style="color:red; font-size:1.1rem;">*</span>
                        <asp:TextBox ID="dtFirstPremiunDueDate" CssClass="form-control" runat="server" placeholder="dd/mm/yyyy" autocomplete="off" required ClientIDMode="Static"></asp:TextBox>
                        
                    </div>
                    <div class="form-group">
                        <label class="lblCss" for="txtbudget">Frequency</label>
                         <asp:DropDownList ID="DDLFreqency" runat="server" CssClass="form-control">
                             <asp:ListItem Value="1">1 Month</asp:ListItem>
                             <asp:ListItem Value="2">2 Months</asp:ListItem>
                             <asp:ListItem Value="3">3 Months</asp:ListItem>
                             <asp:ListItem Value="4">4 Months</asp:ListItem>
                             <asp:ListItem Value="5">5 Months</asp:ListItem>
                             <asp:ListItem Value="6">6 Months</asp:ListItem>
                             <asp:ListItem Value="7">7 Months</asp:ListItem>
                             <asp:ListItem Value="8">8 Months</asp:ListItem>
                             <asp:ListItem Value="9">9 Months</asp:ListItem>
                             <asp:ListItem Value="10">10 Months</asp:ListItem>
                             <asp:ListItem Value="11">11 Months</asp:ListItem>
                             <asp:ListItem Value="12">1 Year</asp:ListItem>
                             <asp:ListItem Value="24">2 Years</asp:ListItem>
                         </asp:DropDownList>
                    </div>  
                    <div class="form-group">
                        <label class="lblCss" for="dtInsuredDate">Insured Date</label> &nbsp;<span style="color:red; font-size:1.1rem;">*</span>
                        <asp:TextBox ID="dtInsuredDate" CssClass="form-control" runat="server" placeholder="dd/mm/yyyy" autocomplete="off" required ClientIDMode="Static"></asp:TextBox>
                    </div>
                    <div class="form-group">
                        <label class="lblCss" for="dtMaturityDate">Maturity Date</label> &nbsp;<span style="color:red; font-size:1.1rem;">*</span>
                        <asp:TextBox ID="dtMaturityDate" CssClass="form-control" runat="server" placeholder="dd/mm/yyyy" autocomplete="off" required ClientIDMode="Static"></asp:TextBox>
                        
                    </div>
                    
                    </div>
            </div> 
        </div>
        <div class="modal-footer">
            <asp:Button ID="btnSubmit" runat="server" Text="Submit" CssClass="btn btn-primary" OnClick="btnSubmit_Click" />
                </div>
    </form>
</asp:Content>
