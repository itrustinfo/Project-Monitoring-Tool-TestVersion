<%@ Page Title="" Language="C#" MasterPageFile="~/_master_pages/modal.Master" AutoEventWireup="true" CodeBehind="add-finance-task.aspx.cs" Inherits="ProjectManagementTool._modal_pages.add_finance_task" %>
<asp:Content ID="Content1" ContentPlaceHolderID="modal_master_head" runat="server">
    <script type="text/javascript">
       function sum() {
           var txtFirstNumberValue = document.getElementById('<%= txtBasicBudget.ClientID %>').value.replace(/,/g, '');
           var txtSecondNumberValue = document.getElementById('<%= txtGST.ClientID %>').value.replace(/,/g, '');
           var result = parseInt(txtFirstNumberValue) + ((parseInt(txtFirstNumberValue) * parseInt(txtSecondNumberValue)) / 100);
           if (!isNaN(result)) {
               var x = result.toString()
               x = x.toString();
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
                   document.getElementById('<%= txtBudget.ClientID %>').value = result;
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
                   document.getElementById('<%= txtBudget.ClientID %>').value = result;
               }
           }
       }

       function ShowCurrencyFormat() {
             var Budget = document.getElementById('<%= txtBasicBudget.ClientID %>').value.replace(/,/g, '');
            
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
                         document.getElementById('<%= txtBasicBudget.ClientID %>').value = x;
                     }
                     else {
                         
                         var lastThree = x.substring(x.length - 3);
                         var otherNumbers = x.substring(0, x.length - 3);
                         if (otherNumbers != '')
                             lastThree = "," + lastThree;
                         var res = otherNumbers.replace(/\B(?=(\d{2})+(?!\d))/g, ",") + lastThree;
                         document.getElementById('<%= txtBasicBudget.ClientID %>').value = res;
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
                     document.getElementById('<%= txtBasicBudget.ClientID %>').value = x1 + x2;
                 }
           }
           var ActualExpenditure = document.getElementById('<%= txtActualExpenditure.ClientID %>').value.replace(/,/g, '');
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
                       document.getElementById('<%= txtActualExpenditure.ClientID %>').value = y;
                   }
                   else {

                       var lastThree1 = y.substring(x.length - 3);
                       var otherNumbers1 = y.substring(0, x.length - 3);
                       if (otherNumbers1 != '')
                           lastThree1 = "," + lastThree1;
                       var res1 = otherNumbers1.replace(/\B(?=(\d{2})+(?!\d))/g, ",") + lastThree1;
                       document.getElementById('<%= txtActualExpenditure.ClientID %>').value = res1;
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
                     document.getElementById('<%= txtActualExpenditure.ClientID %>').value = x3 + x4;
               }
           }
           var TotalBudget = document.getElementById('<%= txtBudget.ClientID %>').value.replace(/,/g, '');
           if (TotalBudget != "") {
                var x = TotalBudget;
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
                         document.getElementById('<%= txtBudget.ClientID %>').value = x;
                     }
                     else {
                         
                         var lastThree = x.substring(x.length - 3);
                         var otherNumbers = x.substring(0, x.length - 3);
                         if (otherNumbers != '')
                             lastThree = "," + lastThree;
                         var res = otherNumbers.replace(/\B(?=(\d{2})+(?!\d))/g, ",") + lastThree;
                         document.getElementById('<%= txtBudget.ClientID %>').value = res;
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
                     document.getElementById('<%= txtBudget.ClientID %>').value = x1 + x2;
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
                   var lastThree = x.substring(x.length - 3);
                   var otherNumbers = x.substring(0, x.length - 3);
                   if (otherNumbers != '')
                       lastThree = ',' + lastThree;
                   var res = otherNumbers.replace(/\B(?=(\d{2})+(?!\d))/g, ",") + lastThree;
                   objctrl.value = res;
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
               objctrl.value = x1 + x2;
           }
       }
</script>
    <script>
  $( function() {
    $("input[id$='dtStartdate']").datepicker({
      changeMonth: true,
        changeYear: true,
      dateFormat:'dd/mm/yy'
      });

      $("input[id$='dtPlannedStartDate']").datepicker({
      changeMonth: true,
        changeYear: true,
      dateFormat:'dd/mm/yy'
      });

      $("input[id$='dtProjectedStartDate']").datepicker({
      changeMonth: true,
        changeYear: true,
      dateFormat:'dd/mm/yy'
      });

          $("input[id$='dtActualEndDate']").datepicker({
      changeMonth: true,
        changeYear: true,
      dateFormat:'dd/mm/yy'
      });

      $("input[id$='dtPlanneddate']").datepicker({
      changeMonth: true,
        changeYear: true,
      dateFormat:'dd/mm/yy'
      });

      $("input[id$='dtProjecteddate']").datepicker({
      changeMonth: true,
        changeYear: true,
      dateFormat:'dd/mm/yy'
      });

    });
  </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="modal_master_body" runat="server">
    <form id="frmAddFinanceTaskModal" runat="server">
        <asp:ScriptManager ID="ScriptManager1" runat="server"></asp:ScriptManager>
        <div class="container-fluid" style="max-height:82vh; overflow-y:auto; margin-top:15px;">
            <div class="row">
                <div class="col-sm-6">
                    <div class="form-group">
                        <label class="lblCss" for="DDlProject">Project</label>
                         <asp:DropDownList ID="DDlProject" CssClass="form-control" runat="server" AutoPostBack="true" ></asp:DropDownList>
                    </div>
                    
                    <div class="form-group">
                        <label class="lblCss" for="DDLWorkPackage">Work Package</label>
                         <asp:DropDownList ID="DDLWorkPackage" CssClass="form-control" runat="server"></asp:DropDownList>
                    </div>
                    
                    <div class="form-group">
                        <label class="lblCss" for="txtTaskName">Task Name</label>
                        <asp:TextBox ID="txtTaskName" CssClass="form-control" runat="server" required ClientIDMode="Static"></asp:TextBox>
                        
                    </div>

                   <%-- <div class="form-group">
                        <label class="lblCss" for="DDlDiscipline">Select Discipline</label>
                         <asp:DropDownList ID="DDlDiscipline" CssClass="form-control"  runat="server">
                             <asp:ListItem Value="Electrical">Electrical</asp:ListItem>
                             <asp:ListItem Value="Engineering">Engineering</asp:ListItem>
                             <asp:ListItem Value="Civil">Civil</asp:ListItem>
                         </asp:DropDownList>
                    </div>--%>
                    
                     <div class="form-group">
                        <label class="lblCss" for="dtStartdate">Actual Start Date</label>
                        <asp:TextBox ID="dtStartdate" CssClass="form-control" runat="server" required ClientIDMode="Static"></asp:TextBox>
                    </div>
                     <div class="form-group">
                        <label class="lblCss" for="dtPlannedStartDate">Planned Start Date</label>
                        <asp:TextBox ID="dtPlannedStartDate" CssClass="form-control" runat="server" required ClientIDMode="Static"></asp:TextBox>
                    </div>
                     <div class="form-group">
                        <label class="lblCss" for="dtProjectedStartDate">Projected Start Date</label>
                        <asp:TextBox ID="dtProjectedStartDate" CssClass="form-control" runat="server" required ClientIDMode="Static"></asp:TextBox>
                    </div>
                     <%-- <div class="form-group">
                        <label class="lblCss" for="txtprefnumber">Owner</label>
                        <asp:DropDownList ID="DDLUsers" runat="server" CssClass="form-control"></asp:DropDownList>
                    </div>--%>
                   <div class="form-group">
                        <label class="lblCss" for="txtDescription">Description</label>
                        <asp:TextBox ID="txtDescription" CssClass="form-control" TextMode="MultiLine" required runat="server" ClientIDMode="Static"></asp:TextBox>
                    </div>
                </div>
                <div class="col-sm-6">
                    
                    <div class="form-group">
                        <label class="lblCss" for="txtbudget">Currency</label>
                         <asp:DropDownList ID="DDLCurrency" runat="server" CssClass="form-control" onchange="ShowCurrencyFormat()">
                             <asp:ListItem Value="&#x20B9;">&#x20B9; (rupee)</asp:ListItem>
                             <asp:ListItem Value="&#36;">&#36; (dollar)</asp:ListItem>
                             <asp:ListItem Value="&#165;">&#165; (yen)</asp:ListItem>
                         </asp:DropDownList>
                    </div>  
                     <div class="form-group">
                        <label class="lblCss" for="txtBasicBudget">Basic Budget</label>
                         <input type="text" id="txtBasicBudget" runat="server" class="form-control" autocomplete="off" onkeyup="javascript:sum();" onblur="toUSD(this)" required />

                    </div>
                   <div class="form-group">
                        <label class="lblCss" for="txtGST">GST (%)</label>
                        <input type="text" id="txtGST" runat="server" class="form-control" autocomplete="off" onkeyup="javascript:sum();" required />
                    </div>
                    <div class="form-group">
                        <label class="lblCss" for="txtBudget">Total Budget</label>
                        <input type="text" id="txtBudget" runat="server" class="form-control" autocomplete="off" onblur="toUSD(this)" required />
                        
                        
                    </div>
                     <div class="form-group">
                        <label class="lblCss" for="txtActualExpenditure">Actual Expenditure</label>
                         <input type="text" id="txtActualExpenditure" runat="server" class="form-control" autocomplete="off" onblur="toUSD(this)" required />
                        
                    </div>
                       <div class="form-group">
                        <label class="lblCss" for="dtActualEndDate">Actual End Date</label>
                        <asp:TextBox ID="dtActualEndDate" CssClass="form-control" runat="server" required ClientIDMode="Static"></asp:TextBox>
                    </div>
                     <div class="form-group">
                        <label class="lblCss" for="dtPlanneddate">Planned End Date</label>
                        <asp:TextBox ID="dtPlanneddate" CssClass="form-control" runat="server" required ClientIDMode="Static"></asp:TextBox>
                    </div>
                     <div class="form-group">
                        <label class="lblCss" for="dtProjecteddate">Projected End Date</label>
                        <asp:TextBox ID="dtProjecteddate" CssClass="form-control" runat="server" required ClientIDMode="Static"></asp:TextBox>
                    </div>
                    
                     <%--  <div class="form-group">
                        <label class="lblCss" for="ddlStatus">Status</label>
                        <asp:DropDownList ID="ddlStatus" runat="server" CssClass="form-control"></asp:DropDownList>
                    </div>--%>
                </div>
            </div> 
        </div>

        <div class="modal-footer">
                    <%--<button type="button" class="btn btn-secondary" data-dismiss="modal">Close</button>--%>
                    <%--<button type="button" class="btn btn-primary">Save changes</button>--%>
            <asp:Button ID="btnSubmit" runat="server" Text="Submit" CssClass="btn btn-primary" />
                </div>
    </form>
</asp:Content>
