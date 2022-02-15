<%@ Page Title="" Language="C#" MasterPageFile="~/_master_pages/modal.Master" AutoEventWireup="true" CodeBehind="add-bankguarantee.aspx.cs" Inherits="ProjectManagementTool._modal_pages.add_bankguarantee" %>
<asp:Content ID="Content1" ContentPlaceHolderID="modal_master_head" runat="server">
     <script type="text/javascript">
         function ShowCurrencyFormat() {
             var Budget = document.getElementById('<%= txtamount.ClientID %>').value.replace(/,/g, '');
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
    $("input[id$='dtExpiry']").datepicker({
      changeMonth: true,
        changeYear: true,
      dateFormat:'dd/mm/yy'
      });

      $("input[id$='dtdate']").datepicker({
      changeMonth: true,
        changeYear: true,
      dateFormat:'dd/mm/yy'
      });
     $("input[id$='dtClaimDate']").datepicker({
      changeMonth: true,
        changeYear: true,
      dateFormat:'dd/mm/yy'
      });
    });
</script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="modal_master_body" runat="server">
    <form id="frmAddIssuesModal" runat="server">
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
                        <label class="lblCss" for="txtBgNumber">BG Number</label> &nbsp;<span style="color:red; font-size:1.1rem;">*</span>
                         <asp:TextBox ID="txtBgNumber" runat="server" CssClass="form-control" autocomplete="off" required></asp:TextBox>
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
                        <label class="lblCss" for="txtamount">Amount</label> &nbsp;<span style="color:red; font-size:1.1rem;">*</span>
                       <input type="text" id="txtamount" runat="server" class="form-control" autocomplete="off" onblur="toUSD(this)" required />
                        
                    </div>
                    <div class="form-group">
                        <label class="lblCss" for="dtdate">Date of Guarantee</label> &nbsp;<span style="color:red; font-size:1.1rem;">*</span>
                        <asp:TextBox ID="dtdate" CssClass="form-control" runat="server" placeholder="dd/mm/yyyy" autocomplete="off" required ClientIDMode="Static"></asp:TextBox>
                    </div>
                    
                </div>
                <div class="col-sm-6">
                    <div class="form-group">
                        <label class="lblCss" for="dtExpiry">Date of Expiry</label> &nbsp;<span style="color:red; font-size:1.1rem;">*</span>
                        <asp:TextBox ID="dtExpiry" CssClass="form-control" runat="server" placeholder="dd/mm/yyyy" autocomplete="off" required ClientIDMode="Static"></asp:TextBox>
                    </div>
                    <div class="form-group">
                        <label class="lblCss" for="dtdate">Last Date Of Claim</label> &nbsp;<span style="color:red; font-size:1.1rem;">*</span>
                        <asp:TextBox ID="dtClaimDate" CssClass="form-control" runat="server" placeholder="dd/mm/yyyy" autocomplete="off" required ClientIDMode="Static"></asp:TextBox>
                    </div>
                    <div class="form-group">
                        <label class="lblCss" for="txtNo_of_Collaterals">No. of Collaterals</label> &nbsp;<span style="color:red; font-size:1.1rem;">*</span>
                         <asp:TextBox ID="txtNo_of_Collaterals" runat="server" CssClass="form-control" TextMode="Number" required></asp:TextBox>
                    </div>
                    <div class="form-group">
                        <label class="lblCss" for="txtBank_Name">Bank Name</label>
                        <asp:TextBox ID="txtBank_Name" runat="server"  CssClass="form-control" autocomplete="off"></asp:TextBox>
                    </div>
                     <div class="form-group">
                        <label class="lblCss" for="txtBank_Branch">Bank Branch</label>
                        <asp:TextBox ID="txtBank_Branch" runat="server"  CssClass="form-control" autocomplete="off"></asp:TextBox>
                    </div>
                     <div class="form-group">
                        <label class="lblCss" for="txtBank_Name">IFSC Code</label>
                        <asp:TextBox ID="txtIFSC_Code" runat="server"  CssClass="form-control" autocomplete="off"></asp:TextBox>
                    </div>
                     <div class="form-group">
                        <label class="lblCss" for="txtBank_Name">Bank Address</label>
                        <asp:TextBox ID="txtaddress" runat="server" TextMode="MultiLine" autocomplete="off"  CssClass="form-control"></asp:TextBox>
                    </div>
                     
                    </div>
            </div> 
        </div>
        <div class="modal-footer">
            <asp:Button ID="btnSubmit" runat="server" Text="Submit" CssClass="btn btn-primary" OnClick="btnSubmit_Click" />
                </div>
    </form>
</asp:Content>
