<%@ Page Title="" Language="C#" MasterPageFile="~/_master_pages/modal.Master" AutoEventWireup="true" CodeBehind="add-project.aspx.cs" Inherits="ProjectManagementTool._modal_pages.add_project" %>
<asp:Content ID="Content1" ContentPlaceHolderID="modal_master_head" runat="server">
     <script type="text/javascript">
         function compareDates(endate) {
           var startDate = document.getElementById("dtStartDate").value;
           var EndDate = document.getElementById("dtPlannedEndDate").value;
           if (startDate != "" && EndDate != "") {
               startDate = startDate.split("/");
               EndDate = EndDate.split("/");
               startDate = startDate[1] + "/" + startDate[0] + "/" + startDate[2];
               EndDate = EndDate[1] + "/" + EndDate[0] + "/" + EndDate[2];
               var stDate = new Date(startDate);
               var eDate = new Date(EndDate);
               
               if (stDate > eDate) {
                   document.getElementById("dtPlannedEndDate").value = "";
                   alert("Planned end date should be greater than Start date");
               }
           }
         }

//function toUSD(objctrl) {
//            //Get the Entered Value
//    var number = objctrl.value.toString(),
//        number = number.replace(',', '');
//                //Split the number between dollars and cents
//    dollars = number.split('.')[0], cents = (number.split('.')[1] || '') + '00';
//    dollars=dollars.replace(',', '');
//    dollars = dollars.split('').reverse().join('').replace(/(\d{3}(?!$))/g, '$1,').split('').reverse().join('');
//    dollars=dollars.replace(',,', ',');
//                //Concatenate the number with currecny symbol
//     objctrl.value = dollars + '.' + cents.slice(0, 2);
         //        }
         function ShowCurrencyFormat() {
             var Budget = document.getElementById('<%= txtbudget.ClientID %>').value.replace(/,/g, '').replace('₹ ', '').replace('$ ', '').replace('¥ ', '').replace('₹', '').replace('$', '').replace('¥', '');
             var ActualExpenditure = document.getElementById('<%= txtActualExpenditure.ClientID %>').value.replace(/,/g, '').replace('₹ ', '').replace('$ ', '').replace('¥ ', '').replace('₹', '').replace('$', '').replace('¥', '');
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
                         document.getElementById('<%= txtbudget.ClientID %>').value = ddlcurrency + ' ' +x;
                     }
                     else {
                         
                         var lastThree = x.substring(x.length - 3);
                         var otherNumbers = x.substring(0, x.length - 3);
                         if (otherNumbers != '')
                             lastThree = "," + lastThree;
                         var res = otherNumbers.replace(/\B(?=(\d{2})+(?!\d))/g, ",") + lastThree;
                         document.getElementById('<%= txtbudget.ClientID %>').value = ddlcurrency + ' ' +res;
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
                     document.getElementById('<%= txtbudget.ClientID %>').value = ddlcurrency + ' ' + (x1 + x2);
                 }
             }
             if (ActualExpenditure != "") {
                  var y = ActualExpenditure;
                 y = y.toString();
                 //y = y.replace(",", "");
                 y += '';
                 var ddlcurrency = document.getElementById('<%= DDLCurrency.ClientID %>').value;
                 if (ddlcurrency == "₹") {
                     if (y.indexOf('.') > 0) {
                        
                         var n3, n4;
                         y = y + '' || '';
                         // works for integer and floating as well
                         n3 = y.split('.');
                         n4 = n3[1] || null;
                         n3 = n3[0].replace(/(\d)(?=(\d\d)+\d$)/g, "$1,");
                         y = n4 ? n3 + '.' + n4 : n3;
                         document.getElementById('<%= txtActualExpenditure.ClientID %>').value = ddlcurrency + ' ' + y;
                     }
                     else {
                        
                         var lastThree1 = y.substring(x.length - 3);
                         var otherNumbers1 = y.substring(0, x.length - 3);
                         if (otherNumbers1 != '')
                             lastThree1 =  ","+lastThree1;
                         var res1 = otherNumbers1.replace(/\B(?=(\d{2})+(?!\d))/g, ",") + lastThree1;
                         document.getElementById('<%= txtActualExpenditure.ClientID %>').value = ddlcurrency + ' ' + res1;
                     }
                 }
                 else {
                     y = y.split('.');
                     var x3 = y[0];
                     var x4 = y.length > 1 ? '.' + y[1] : '';
                     var rgx = /(\d+)(\d{3})/;
                     while (rgx.test(x3)) {
                         x3 = x3.replace(rgx, '$1' + ',' + '$2');
                     }
                     //var num2 = y.toString().replace(/(\d)(?=(\d{3})+(?!\d))/g, '$1,');
                     document.getElementById('<%= txtActualExpenditure.ClientID %>').value = ddlcurrency + ' ' + (x3 + x4);
                 }
             }
         }
         function toCurrencyFormat(objctrl) {
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
     <script>
  $( function() {
    $("input[id$='dtStartDate']").datepicker({
      changeMonth: true,
        changeYear: true,
      dateFormat:'dd/mm/yy'
      });

      $("input[id$='dtPlannedEndDate']").datepicker({
      changeMonth: true,
        changeYear: true,
      dateFormat:'dd/mm/yy'
      });

      $("input[id$='dtProjectedEndDate']").datepicker({
      changeMonth: true,
        changeYear: true,
      dateFormat:'dd/mm/yy'
    });
    });
  </script>

</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="modal_master_body" runat="server">
    <form id="frmAddWorkpackageModal" runat="server">

     <div class="container-fluid" style="max-height:80vh; overflow-y:auto; margin-top:15px;">
            <div class="row">
                <div class="col-sm-6">
                    <div class="form-group">
                        <label class="lblCss" for="txtprojectName">Project Category</label>
                        <asp:DropDownList ID="DDLProjectClass" runat="server" CssClass="form-control">
                         </asp:DropDownList>
                        
                    </div>
                    <div class="form-group">
                        <label class="lblCss" for="txtprojectName">Project Name</label> &nbsp;<span style="color:red; font-size:1.1rem;">*</span>
                        <asp:TextBox ID="txtprojectName" CssClass="form-control" required runat="server" autocomplete="off" ClientIDMode="Static"></asp:TextBox>
                        
                    </div>
                    <div class="form-group">
                        <label class="lblCss" for="txtprojectcode">Project Code</label> &nbsp;<span style="color:red; font-size:1.1rem;">*</span>
                        <%--<label class="lblCss" for="txtprojectcode">NJSEI Project Number</label>--%>
                        <asp:TextBox ID="txtprojectcode" CssClass="form-control" runat="server" autocomplete="off" required ClientIDMode="Static"></asp:TextBox>
                    </div>
                    <div class="form-group">
                        <label class="lblCss" for="txtClient">Funding Agency</label> &nbsp;<span style="color:red; font-size:1.1rem;">*</span>
                        <asp:TextBox ID="txtFundingAgency" CssClass="form-control" runat="server" autocomplete="off" required ClientIDMode="Static"></asp:TextBox>
                        
                    </div>
                    <div class="form-group">
                        <label class="lblCss" for="dtStartDate">Start Date</label> &nbsp;<span style="color:red; font-size:1.2rem;">*</span>
                        <%--<input type="text" id="datepicker" Class="form-control" />--%>
                        <asp:TextBox ID="dtStartDate" CssClass="form-control" runat="server" ClientIDMode="Static" placeholder="dd/mm/yyyy" autocomplete="off" required></asp:TextBox>
                    </div>
                    
                </div>
                <div class="col-sm-6">
                    <div class="form-group">
                        <label class="lblCss" for="txtbudget">Currency</label>
                         <asp:DropDownList ID="DDLCurrency" runat="server" CssClass="form-control" onchange="ShowCurrencyFormat()">
                             <asp:ListItem Value="&#x20B9;">&#x20B9; (RUPEE)</asp:ListItem>
                             <asp:ListItem Value="&#36;">&#36; (USD)</asp:ListItem>
                             <asp:ListItem Value="&#165;">&#165; (YEN)</asp:ListItem>
                         </asp:DropDownList>
                    </div>  
                     <div class="form-group">
                        <label class="lblCss" for="txtbudget">Budget</label> &nbsp;<span style="color:red; font-size:1.1rem;">*</span>
                         <input type="text" id="txtbudget" runat="server" autocomplete="off"  class="form-control" onblur="toCurrencyFormat(this)" required />
                    </div>  
                     <div class="form-group">
                        <label class="lblCss" for="txtActualExpenditure">Actual Expenditure</label> &nbsp;<span style="color:red; font-size:1.1rem;">*</span>
                         <input type="text" id="txtActualExpenditure" runat="server" autocomplete="off" class="form-control" onblur="toCurrencyFormat(this)"  required />
                    </div> 
                    <div class="form-group">
                        <label class="lblCss" for="dtPlannedEndDate">Planned EndDate</label> &nbsp;<span style="color:red; font-size:1.1rem;">*</span>
                        <asp:TextBox ID="dtPlannedEndDate" CssClass="form-control" runat="server" placeholder="dd/mm/yyyy" autocomplete="off" ClientIDMode="Static" required onchange="javascript:compareDates(this);"></asp:TextBox>
                    </div>
                    <div class="form-group">
                        <label class="lblCss" for="dtProjectedEndDate">Projected EndDate</label> &nbsp;<span style="color:red; font-size:1.1rem;">*</span>
                        <asp:TextBox ID="dtProjectedEndDate" CssClass="form-control" runat="server" placeholder="dd/mm/yyyy" autocomplete="off" ClientIDMode="Static" required></asp:TextBox>
                    </div>
                    </div>
            </div> 

        </div>
        <div class="modal-footer">
                    <%--<button type="button" class="btn btn-secondary" data-dismiss="modal">Close</button>--%>
            <asp:Button ID="btnAdd" runat="server" CssClass="btn btn-primary" Text="Submit" OnClick="btnAdd_Click" />
                    <%--<button type="button" class="btn btn-primary">Save changes</button>--%>
                </div>
        
        </form>
</asp:Content>
