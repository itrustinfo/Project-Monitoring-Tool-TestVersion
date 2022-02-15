<%@ Page Title="" Language="C#" MasterPageFile="~/_master_pages/modal.Master" AutoEventWireup="true" CodeBehind="add-workpackage.aspx.cs" Inherits="ProjectManagementTool._modal_pages.add_workpackage" %>
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
                   alert("Planned End Date should be greater than Start Date");
               }
           }
        }

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
                        document.getElementById('<%= txtbudget.ClientID %>').value = ddlcurrency + ' ' + x;
                    }
                    else {

                        var lastThree = x.substring(x.length - 3);
                        var otherNumbers = x.substring(0, x.length - 3);
                        if (otherNumbers != '')
                            lastThree = "," + lastThree;
                        var res = otherNumbers.replace(/\B(?=(\d{2})+(?!\d))/g, ",") + lastThree;
                        document.getElementById('<%= txtbudget.ClientID %>').value = ddlcurrency + ' ' + res;
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

     <div class="container-fluid" style="max-height:82vh; overflow-y:auto; margin-top:15px;">
            <div class="row">
                <div class="col-sm-6">
                    <div class="form-group">
                        <label class="lblCss" for="DDLProject">Project</label>
                        <asp:DropDownList ID="DDLProject" runat="server" class="form-control" AutoPostBack="true" OnSelectedIndexChanged="DDLProject_SelectedIndexChanged" ></asp:DropDownList>
                    </div>
                    <div class="form-group">
                        <label class="lblCss" for="DDLContractor">Contractor</label> &nbsp;<span style="color:red; font-size:1rem;">*</span>
                        <asp:DropDownList ID="DDLContractor" runat="server" class="form-control" required AutoPostBack="true" OnSelectedIndexChanged="DDLContractor_SelectedIndexChanged"></asp:DropDownList>
                    </div>
                    <div class="form-group" id="WorkpackageoptionDiv" runat="server">
                        <label class="lblCss" for="DDLWorkpackageOption">Workpackage Option</label> &nbsp;<span style="color:red; font-size:1rem;">*</span>
                        <asp:DropDownList ID="DDLWorkpackageOption" runat="server"  CssClass="form-control" required>
                        </asp:DropDownList>
                    </div>
                    <div class="form-group">
                        <label class="lblCss" id="LblProjectNumber" runat="server" for="dtProjectedEndDate">NJSEI Project Number</label>
                        <asp:TextBox ID="txtnjseinumber" CssClass="form-control" Enabled="false" runat="server" autocomplete="off" ClientIDMode="Static"></asp:TextBox>
                    </div>
                    <div class="form-group">
                        <label class="lblCss" for="dtProjectedEndDate">Project Specific Package Number</label>
                        <asp:TextBox ID="txtprojectspecificnumber" CssClass="form-control" Enabled="false" runat="server" autocomplete="off" ClientIDMode="Static"></asp:TextBox>
                    </div>
                    <div class="form-group">
                        <label class="lblCss" for="txtworkpackage">Work Package</label>
                        <%--<asp:TextBox ID="txtworkpackage" CssClass="form-control" runat="server" ClientIDMode="Static"></asp:TextBox>--%>
                        <asp:DropDownList ID="DDLWorkPackage" runat="server" class="form-control" ></asp:DropDownList>
                    </div>
                    <div class="form-group">
                        <label class="lblCss" for="txtLocation">Location</label>
                        <%--<asp:TextBox ID="txtLocation" CssClass="form-control" runat="server" ClientIDMode="Static"></asp:TextBox>--%>
                        <asp:DropDownList ID="DDLLocation" runat="server" class="form-control" ></asp:DropDownList>
                    </div>
                    <div class="form-group">    
                        <label class="lblCss" for="TxtRemarks">Assign to User</label> &nbsp;<span style="color:red; font-size:1rem;">*</span>

                      
                       <asp:ListBox ID="lstUsers" runat="server" CssClass="form-control" SelectionMode="Multiple">

                       </asp:ListBox>  
                    </div>
                </div>
                <div class="col-sm-6">
                    
                    <div class="form-group">
                        <label class="lblCss" for="txtClient">Client</label>
                        <%--<asp:TextBox ID="txtClient" CssClass="form-control" runat="server" ClientIDMode="Static"></asp:TextBox>--%>
                        <asp:DropDownList ID="DDLClient" runat="server" class="form-control" ></asp:DropDownList>
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
                        <label class="lblCss" for="txtbudget">Budget</label> &nbsp;<span style="color:red; font-size:1rem;">*</span>
                       
                         <input type="text" id="txtbudget" runat="server"  class="form-control" autocomplete="off" onblur="toUSD(this)" required />
                        <%--<asp:TextBox ID="txtbudget" CssClass="form-control" TextMode="Number" onkeypress="moneyFormat(this)" runat="server" ClientIDMode="Static"></asp:TextBox>--%>
                    </div>  
                     <div class="form-group">
                        <label class="lblCss" for="txtActualExpenditure">Actual Expenditure</label> &nbsp;<span style="color:red; font-size:1rem;">*</span>
                         <input type="text" id="txtActualExpenditure" runat="server"  class="form-control" autocomplete="off" onblur="toUSD(this)"  required />
                    </div> 
                     <div class="form-group">
                        <label class="lblCss" for="dtStartDate">Start Date</label> &nbsp;<span style="color:red; font-size:1rem;">*</span>
                        <%--<input type="text" id="datepicker" Class="form-control" />--%>
                        <asp:TextBox ID="dtStartDate" CssClass="form-control" runat="server" placeholder="dd/mm/yyyy" autocomplete="off" ClientIDMode="Static" required></asp:TextBox>
                    </div>
                    <div class="form-group">
                        <label class="lblCss" for="dtPlannedEndDate">Planned EndDate</label> &nbsp;<span style="color:red; font-size:1rem;">*</span>
                        <asp:TextBox ID="dtPlannedEndDate" CssClass="form-control" runat="server" placeholder="dd/mm/yyyy" autocomplete="off" ClientIDMode="Static" required onchange="javascript:compareDates(this);"></asp:TextBox>
                    </div>
                    <div class="form-group">
                        <label class="lblCss" for="dtProjectedEndDate">Projected EndDate</label> &nbsp;<span style="color:red; font-size:1rem;">*</span>
                        <asp:TextBox ID="dtProjectedEndDate" CssClass="form-control" runat="server" placeholder="dd/mm/yyyy" autocomplete="off" ClientIDMode="Static" required></asp:TextBox>
                    </div>
                    
                   <div class="form-group">
                        <label class="lblCss" for="ddlstatus">Status</label>
                        <asp:DropDownList ID="ddlstatus" runat="server" class="form-control" ></asp:DropDownList>
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
