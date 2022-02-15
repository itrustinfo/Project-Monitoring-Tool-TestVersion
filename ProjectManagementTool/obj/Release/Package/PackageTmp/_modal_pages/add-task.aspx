<%@ Page Title="" Language="C#" MasterPageFile="~/_master_pages/modal.Master" AutoEventWireup="true" CodeBehind="add-task.aspx.cs" Inherits="ProjectManagementTool._modal_pages.add_task" %>
<asp:Content ID="Content1" ContentPlaceHolderID="modal_master_head" runat="server">
   <script type="text/javascript">
       function compareActualStartDates(endate) {
           var startDate = document.getElementById("dtStartdate").value;
           var EndDate = document.getElementById("dtActualEndDate").value;
           if (startDate != "" && EndDate != "") {
               startDate = startDate.split("/");
               EndDate = EndDate.split("/");
               startDate = startDate[1] + "/" + startDate[0] + "/" + startDate[2];
               EndDate = EndDate[1] + "/" + EndDate[0] + "/" + EndDate[2];
               var stDate = new Date(startDate);
               var eDate = new Date(EndDate);
               
               if (stDate > eDate) {
                   document.getElementById("dtActualEndDate").value = "";
                   alert("End Date should be greater than Start Date");
               }
           }
       }

       function compareProjectedDates(endate) {
           var startDate = document.getElementById("dtProjectedStartDate").value;
           var EndDate = document.getElementById("dtProjecteddate").value;
           if (startDate != "" && EndDate != "") {
               startDate = startDate.split("/");
               EndDate = EndDate.split("/");
               startDate = startDate[1] + "/" + startDate[0] + "/" + startDate[2];
               EndDate = EndDate[1] + "/" + EndDate[0] + "/" + EndDate[2];
               var stDate = new Date(startDate);
               var eDate = new Date(EndDate);
               
               if (stDate > eDate) {
                   document.getElementById("dtProjecteddate").value = "";
                   alert("Projected Enddate should be greater than Projected Start Date");
               }
           }
       }

       function comparePlannedDates(endate) {
           var startDate = document.getElementById("dtPlannedStartDate").value;
           var EndDate = document.getElementById("dtPlanneddate").value;
           if (startDate != "" && EndDate != "") {
               startDate = startDate.split("/");
               EndDate = EndDate.split("/");
               startDate = startDate[1] + "/" + startDate[0] + "/" + startDate[2];
               EndDate = EndDate[1] + "/" + EndDate[0] + "/" + EndDate[2];
               var stDate = new Date(startDate);
               var eDate = new Date(EndDate);
               
               if (stDate > eDate) {
                   document.getElementById("dtPlanneddate").value = "";
                   alert("Planned Enddate should be greater than Planned Start Date");
               }
           }
       }

<%--       function sum() {
           var txtFirstNumberValue = document.getElementById('<%= txtBasicBudget.ClientID %>').value.replace(/,/g, '').replace('₹ ', '').replace('$ ', '').replace('¥ ', '').replace('₹', '').replace('$', '').replace('¥', '');
           var txtSecondNumberValue = document.getElementById('<%= txtGST.ClientID %>').value.replace(/,/g, '').replace('₹ ', '').replace('$ ', '').replace('¥ ', '').replace('₹', '').replace('$', '').replace('¥', '');
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
                   document.getElementById('<%= txtBudget.ClientID %>').value = ddlcurrency + ' ' + result;
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
                   document.getElementById('<%= txtBudget.ClientID %>').value = ddlcurrency + ' ' +result;
               }
           }
       }--%>

<%--       function ShowCurrencyFormat() {
             var Budget = document.getElementById('<%= txtBasicBudget.ClientID %>').value.replace(/,/g, '').replace('₹ ', '').replace('$ ', '').replace('¥ ', '').replace('₹', '').replace('$', '').replace('¥', '');
            
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
                         document.getElementById('<%= txtBasicBudget.ClientID %>').value = ddlcurrency + ' ' +x;
                     }
                     else {
                         
                         var lastThree = x.substring(x.length - 3);
                         var otherNumbers = x.substring(0, x.length - 3);
                         if (otherNumbers != '')
                             lastThree = "," + lastThree;
                         var res = otherNumbers.replace(/\B(?=(\d{2})+(?!\d))/g, ",") + lastThree;
                         document.getElementById('<%= txtBasicBudget.ClientID %>').value = ddlcurrency + ' ' +res;
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
                     document.getElementById('<%= txtBasicBudget.ClientID %>').value = ddlcurrency + ' ' + (x1 + x2);
                 }
           }
           var ActualExpenditure = document.getElementById('<%= txtActualExpenditure.ClientID %>').value.replace(/,/g, '').replace('₹ ', '').replace('$ ', '').replace('¥ ', '').replace('₹', '').replace('$', '').replace('¥', '');
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
                           lastThree1 = "," + lastThree1;
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
           var TotalBudget = document.getElementById('<%= txtBudget.ClientID %>').value.replace(/,/g, '').replace('₹ ', '').replace('$ ', '').replace('¥ ', '').replace('₹', '').replace('$', '').replace('¥', '');
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
                         document.getElementById('<%= txtBudget.ClientID %>').value = ddlcurrency + ' ' + x;
                     }
                     else {
                         
                         var lastThree = x.substring(x.length - 3);
                         var otherNumbers = x.substring(0, x.length - 3);
                         if (otherNumbers != '')
                             lastThree = "," + lastThree;
                         var res = otherNumbers.replace(/\B(?=(\d{2})+(?!\d))/g, ",") + lastThree;
                         document.getElementById('<%= txtBudget.ClientID %>').value = ddlcurrency + ' ' + res;
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
                     document.getElementById('<%= txtBudget.ClientID %>').value = ddlcurrency + ' ' + (x1 + x2);
               }
           }
       }--%>

<%--       function toUSD(objctrl) {
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
       }--%>
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

        <script type="text/javascript">
        $(document).ready(function () {
            $(".showBOQModal").click(function(e) {
                e.preventDefault();
                jQuery.noConflict();
            var url = $(this).attr("href");
            $("#ModBOQData iframe").attr("src", url);
            $("#ModBOQData").modal("show");
            });
        });
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="modal_master_body" runat="server">
    <form id="frmAddTaskModal" runat="server">
        <asp:ScriptManager ID="ScriptManager1" runat="server"></asp:ScriptManager>
        <div class="container-fluid" style="max-height:82vh; overflow-y:auto; margin-top:15px;">
            <div class="row">
                <div class="col-sm-6">
                    <div class="form-group" style="display:none;">
                        <label class="lblCss" for="DDlProject">Project</label>
                         <asp:DropDownList ID="DDlProject" CssClass="form-control" runat="server" Enabled="false" AutoPostBack="true" OnSelectedIndexChanged="DDlProject_SelectedIndexChanged"></asp:DropDownList>
                    </div>
                    
                    <div class="form-group" style="display:none;">
                        <label class="lblCss" for="DDLWorkPackage">Work Package</label>
                         <asp:DropDownList ID="DDLWorkPackage" CssClass="form-control" Enabled="false" runat="server"></asp:DropDownList>
                    </div>
                    <div class="form-group">
                        <label class="lblCss" for="txtbudget">Choose BOQ Item</label> &nbsp;<asp:LinkButton ID="LnkChangeItem" runat="server" Text="Change Item" OnClick="LnkChangeItem_Click"></asp:LinkButton> <br />
                        <asp:Label ID="lblActivityName" runat="server" CssClass="form-control"></asp:Label>                        
                         <a id="LinkBOQData" runat="server" href="/_modal_pages/boq-treeview.aspx" class="showBOQModal">
                                <asp:Button ID="btnchoose" runat="server" CausesValidation="false" Text="Choose Item Number" CssClass="form-control btn-link" />
                         </a>
                    </div>
                    <div class="form-group">
                         <label class="lblCss" for="txtquantity">Select Option</label>
                         <asp:RadioButtonList ID="RBLOption" runat="server" CssClass="form-control" RepeatDirection="Horizontal">
                             <asp:ListItem Value="1">&nbsp;Create BOQ Sub Items</asp:ListItem>
                             <asp:ListItem Value="0" Selected="True">&nbsp;N/A</asp:ListItem>
                         </asp:RadioButtonList>
                    </div>
                    <div class="form-group">
                        <label class="lblCss" for="txtTaskName">Task Name</label> &nbsp;<span style="color:red; font-size:1rem;">*</span>
                        <asp:TextBox ID="txtTaskName" CssClass="form-control" runat="server" autocomplete="off" required ClientIDMode="Static"></asp:TextBox>
                        
                    </div>
                     <div class="form-group">
                        <label class="lblCss" for="txtDescription">Description</label> &nbsp;<span style="color:red; font-size:1rem;">*</span>
                        <asp:TextBox ID="txtDescription" CssClass="form-control" TextMode="MultiLine" required runat="server" ClientIDMode="Static"></asp:TextBox>
                    </div>
                    <div class="form-group">
                        <label class="lblCss" for="DDlDiscipline">Select Discipline</label>
                         <asp:DropDownList ID="DDlDiscipline" CssClass="form-control"  runat="server">
                             <asp:ListItem Value="Civil">Civil</asp:ListItem>
                             <asp:ListItem Value="Electrical">Electrical</asp:ListItem>
                             <asp:ListItem Value="Engineering">Engineering</asp:ListItem>
                             <asp:ListItem Value="General">General</asp:ListItem>
                             <asp:ListItem Value="Instrumentation">Instrumentation</asp:ListItem>
                             <asp:ListItem Value="Mechanical">Mechanical</asp:ListItem>
                         </asp:DropDownList>
                    </div>
                     <div class="form-group">
                        <label class="lblCss" for="dtStartdate">Actual Start Date</label> 
                        <asp:TextBox ID="dtStartdate" CssClass="form-control" runat="server" placeholder="dd/mm/yyyy" autocomplete="off" ClientIDMode="Static"></asp:TextBox>
                    </div>
                     <div class="form-group">
                        <label class="lblCss" for="dtPlannedStartDate">Planned Start Date</label> 
                        <asp:TextBox ID="dtPlannedStartDate" CssClass="form-control" runat="server" placeholder="dd/mm/yyyy" autocomplete="off" ClientIDMode="Static"></asp:TextBox>
                    </div>
                     <div class="form-group">
                        <label class="lblCss" for="dtProjectedStartDate">Projected Start Date</label> 
                        <asp:TextBox ID="dtProjectedStartDate" CssClass="form-control" runat="server" placeholder="dd/mm/yyyy" autocomplete="off" ClientIDMode="Static"></asp:TextBox>
                    </div>
                      <div class="form-group">
                        <label class="lblCss" for="txtprefnumber">Owner</label> &nbsp;<span style="color:red; font-size:1rem;">*</span>
                        <asp:DropDownList ID="DDLUsers" runat="server" CssClass="form-control"></asp:DropDownList>
                    </div>
                    <div class="form-group">
                        <label class="lblCss" for="txtDescription">Task Type</label> 
                        <asp:DropDownList ID="DDLTaskType" runat="server" CssClass="form-control">
                            <asp:ListItem Value="">--Select--</asp:ListItem>
                            <asp:ListItem Value="Procurement">Procurement</asp:ListItem>
                        </asp:DropDownList>
                    </div>
                  
                </div>
                <div class="col-sm-6">
                    
                    
                   <%-- <div class="form-group">
                        <label class="lblCss" for="txtbudget">Currency</label>
                         <asp:DropDownList ID="DDLCurrency" runat="server" CssClass="form-control" onchange="ShowCurrencyFormat()">
                             <asp:ListItem Value="&#x20B9;">&#x20B9; (RUPEE)</asp:ListItem>
                             <asp:ListItem Value="&#36;">&#36; (USD)</asp:ListItem>
                             <asp:ListItem Value="&#165;">&#165; (YEN)</asp:ListItem>
                         </asp:DropDownList>
                    </div>  --%>
                    <div class="form-group">
                        <label class="lblCss" for="DDLMeasurementUnit">Measurement Unit</label> 
                        <asp:DropDownList ID="DDLMeasurementUnit" runat="server" CssClass="form-control" AutoPostBack="true" OnSelectedIndexChanged="DDLMeasurementUnit_SelectedIndexChanged">
                        </asp:DropDownList>
                    </div>
                     <div class="form-group" id="MeasurementQuan" runat="server">
                        <label class="lblCss" for="txtMeasurementQuantity">Total Quantity</label> 
                        <asp:TextBox ID="txtMeasurementQuantity" CssClass="form-control" TextMode="Number" Enabled="false" Text="100" runat="server" ClientIDMode="Static"></asp:TextBox>
                    </div>
                     <div class="form-group">
                        <%--<label class="lblCss" for="txtBasicBudget">Basic Budget</label> &nbsp;<span style="color:red; font-size:1rem;">*</span>--%>
                         <label class="lblCss" for="txtquantity">Quantity</label>
                         <input type="text" id="txtquantity" runat="server" class="form-control" autocomplete="off" />
                    </div>
                   <div class="form-group">
                        <label class="lblCss" for="txtGST">GST (%)</label> 
                        <input type="text" id="txtGST" runat="server" class="form-control" autocomplete="off" />
                    </div>
                    <div class="form-group">
                        <label class="lblCss" for="txtprice">Price</label> 
                        <input type="text" id="txtprice" runat="server" class="form-control" autocomplete="off" />
                        
                        
                    </div>
                     <%--<div class="form-group">
                        <label class="lblCss" for="txtActualExpenditure">Actual Expenditure</label> &nbsp;<span style="color:red; font-size:1rem;">*</span>
                         <input type="text" id="txtActualExpenditure" runat="server" class="form-control" autocomplete="off" onblur="toUSD(this)" required />
                        
                    </div>--%>
                       <div class="form-group">
                        <label class="lblCss" for="dtActualEndDate">Actual End Date</label> 
                        <asp:TextBox ID="dtActualEndDate" CssClass="form-control" runat="server" placeholder="dd/mm/yyyy" autocomplete="off" ClientIDMode="Static" onchange="javascript:compareActualStartDates(this);"></asp:TextBox>
                    </div>
                     <div class="form-group">
                        <label class="lblCss" for="dtPlanneddate">Planned End Date</label> 
                        <asp:TextBox ID="dtPlanneddate" CssClass="form-control" runat="server" placeholder="dd/mm/yyyy" autocomplete="off" ClientIDMode="Static" onchange="javascript:comparePlannedDates(this);"></asp:TextBox>
                    </div>
                     <div class="form-group">
                        <label class="lblCss" for="dtProjecteddate">Projected End Date</label> 
                        <asp:TextBox ID="dtProjecteddate" CssClass="form-control" runat="server" placeholder="dd/mm/yyyy" autocomplete="off" ClientIDMode="Static" onchange="javascript:compareProjectedDates(this);"></asp:TextBox>
                    </div>
                    <div class="form-group">
                        <label class="lblCss" for="txtDescription">Weightage(%)</label> 
                        <asp:TextBox ID="txtweightage" CssClass="form-control" runat="server" ClientIDMode="Static"></asp:TextBox>
                    </div>
                    
                       <div class="form-group">
                        <label class="lblCss" for="ddlStatus">Status</label>
                        <asp:DropDownList ID="ddlStatus" runat="server" CssClass="form-control"></asp:DropDownList>
                    </div>
                </div>
            </div> 
        </div>

        <div class="modal-footer">
                    <%--<button type="button" class="btn btn-secondary" data-dismiss="modal">Close</button>--%>
                    <%--<button type="button" class="btn btn-primary">Save changes</button>--%>
            <asp:Button ID="btnSubmit" runat="server" Text="Submit" CssClass="btn btn-primary" OnClick="btnSubmit_Click" />
                </div>

             <%--Link BOQ Data modal--%>
    <div id="ModBOQData" class="modal it-modal fade">
	    <div class="modal-dialog modal-dialog-centered modal-dialog-scrollable modal-lg">
		    <div class="modal-content">
			    <div class="modal-header">
				    <h5 class="modal-title">Link BOQ Activity</h5>
                    <button aria-label="Close" class="close" data-dismiss="modal" type="button"><span aria-hidden="true">&times;</span></button>
			    </div>
			    <div class="modal-body">
                    <iframe class="border-0 w-100" style="height:340px;" loading="lazy"></iframe>
			    </div>
              
		    </div>
	    </div>
    </div>

    </form>
</asp:Content>
