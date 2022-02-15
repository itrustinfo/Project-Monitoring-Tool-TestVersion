<%@ Page Title="" Language="C#" MasterPageFile="~/_master_pages/modal.Master" AutoEventWireup="true" CodeBehind="add-resourcemaster.aspx.cs" Inherits="ProjectManagementTool._modal_pages.add_resourcemaster" %>
<asp:Content ID="Content1" ContentPlaceHolderID="modal_master_head" runat="server">
    <script type="text/javascript">
       function sum() {
           var txtFirstNumberValue = document.getElementById('<%= txtCost.ClientID %>').value.replace(/,/g, '').replace('₹ ', '').replace('$ ', '').replace('¥ ', '').replace('₹', '').replace('$', '').replace('¥', '');
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
                   document.getElementById('<%= txtTotalCost.ClientID %>').value = ddlcurrency + ' ' + result;
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
                   document.getElementById('<%= txtTotalCost.ClientID %>').value = ddlcurrency + ' ' + result;
               }
           }
       }
</script>
    <script type="text/javascript">
        function ShowCurrencyFormat() {
            var Budget = document.getElementById('<%= txtCost.ClientID %>').value.replace(/,/g, '').replace('₹ ', '').replace('$ ', '').replace('¥ ', '').replace('₹', '').replace('$', '').replace('¥', '');
            var TotalCost = document.getElementById('<%= txtTotalCost.ClientID %>').value.replace(/,/g, '').replace('₹ ', '').replace('$ ', '').replace('¥ ', '').replace('₹', '').replace('$', '').replace('¥', '');
            if (Budget != "" && TotalCost != "") {
                var x = Budget;
                x = x.toString();
                //x = x.replace(",", "");
                x += '';
                var y = TotalCost;
                y = y.toString();
                //y = y.replace(",", "");
                y += '';
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
                        document.getElementById('<%= txtCost.ClientID %>').value = ddlcurrency + ' ' + x;
                    }
                    else {

                        var lastThree = x.substring(x.length - 3);
                        var otherNumbers = x.substring(0, x.length - 3);
                        if (otherNumbers != '')
                            lastThree = "," + lastThree;
                        var res = otherNumbers.replace(/\B(?=(\d{2})+(?!\d))/g, ",") + lastThree;
                        document.getElementById('<%= txtCost.ClientID %>').value = ddlcurrency + ' ' + res;
                    }

                    if (y.indexOf('.') > 0) {

                        var n3, n4;
                        y = y + '' || '';
                        // works for integer and floating as well
                        n3 = y.split('.');
                        n4 = n3[1] || null;
                        n3 = n3[0].replace(/(\d)(?=(\d\d)+\d$)/g, "$1,");
                        y = n4 ? n3 + '.' + n4 : n3;
                        document.getElementById('<%= txtTotalCost.ClientID %>').value = ddlcurrency + ' ' + y;
                    }
                    else {

                        var lastThree1 = y.substring(x.length - 3);
                        var otherNumbers1 = y.substring(0, x.length - 3);
                        if (otherNumbers1 != '')
                            lastThree1 = "," + lastThree1;
                        var res1 = otherNumbers1.replace(/\B(?=(\d{2})+(?!\d))/g, ",") + lastThree1;
                        document.getElementById('<%= txtTotalCost.ClientID %>').value = ddlcurrency + ' ' + res1;
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
                    document.getElementById('<%= txtCost.ClientID %>').value = ddlcurrency + ' ' + (x1 + x2);


                    y = y.split('.');
                    var x3 = y[0];
                    var x4 = y.length > 1 ? '.' + y[1] : '';
                    var rgx = /(\d+)(\d{3})/;
                    while (rgx.test(x3)) {
                        x3 = x3.replace(rgx, '$1' + ',' + '$2');
                    }
                    //var num2 = y.toString().replace(/(\d)(?=(\d{3})+(?!\d))/g, '$1,');
                    document.getElementById('<%= txtTotalCost.ClientID %>').value = ddlcurrency + ' ' + (x3 + x4);
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

</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="modal_master_body" runat="server">
    <form id="frmAddResourceMaster" runat="server">
        <asp:ScriptManager ID="ScriptManager1" runat="server"></asp:ScriptManager>
        <div class="container-fluid" style="max-height:80vh; overflow-y:auto; margin-top:15px;">
            <div class="row">
                <div class="col-sm-6">
                    <div class="form-group">
                        <label class="lblCss" for="txtResourceName">Resource Name</label> &nbsp;<span style="color:red; font-size:1.1rem;">*</span>
                         <asp:TextBox ID="txtResourceName" runat="server" CssClass="form-control" autocomplete="off" required></asp:TextBox>
                    </div>
                    
                    <div class="form-group">
                        <label class="lblCss" for="DDLResourceType">Resource Type</label>
                         <asp:DropDownList ID="DDLResourceType" runat="server" CssClass="form-control" >
                     </asp:DropDownList>
                    </div>

                  <%--  <div class="form-group">
                        <label class="lblCss" for="DDLResourceOwner">Resource Owner</label>
                         <asp:DropDownList ID="DDLResourceOwner" runat="server" CssClass="form-control">
                             <asp:ListItem Value="Contractor">Contractor</asp:ListItem>
                             <asp:ListItem Value="Sub-Contractor">Sub-Contractor</asp:ListItem>
                     </asp:DropDownList>
                    </div>--%>

                    <div class="form-group">
                        <label class="lblCss" for="DDLunit">Unit for Measurement</label>
                         <asp:DropDownList ID="DDLunit" CssClass="form-control" runat="server"></asp:DropDownList>
                    </div>
                    
                    
                    <div class="form-group">
                        <label class="lblCss" for="txtDescription">Description</label>
                        <asp:TextBox ID="txtdescription" CssClass="form-control" TextMode="MultiLine" runat="server" ClientIDMode="Static"></asp:TextBox>
                    </div>
                    
                </div>
                <div class="col-sm-6">
                    <div class="form-group">
                        <label class="lblCss" for="ddlCostType">Cost Type</label>
                        <asp:DropDownList ID="ddlCostType" CssClass="form-control" runat="server"></asp:DropDownList>
                        
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
                        <label class="lblCss" for="txtCost">Basic Budget</label> 
                         <input type="text" id="txtCost" runat="server" class="form-control" autocomplete="off" onkeyup="javascript:sum();" onblur="toUSD(this)"  />
                    </div>
                   <div class="form-group">
                        <label class="lblCss" for="txtGST">GST(%)</label> 
                        <input type="text" id="txtGST" runat="server" class="form-control" autocomplete="off" onkeyup="javascript:sum();"  />
                    </div>
                    <div class="form-group">
                        <label class="lblCss" for="txtTotalCost">Total Budget</label> 
                        <input type="text" id="txtTotalCost" runat="server" class="form-control" autocomplete="off" onblur="toUSD(this)"  />
                    </div>
                     
                    </div>
            </div> 
        </div>

        <div class="modal-footer">
                    <%--<button type="button" class="btn btn-secondary" data-dismiss="modal">Close</button>--%>
                    <%--<button type="button" class="btn btn-primary">Save changes</button>--%>
            <asp:Button ID="btnSubmit" runat="server" Text="Submit" CssClass="btn btn-primary" OnClick="btnSubmit_Click" />
                </div>
    </form>
</asp:Content>
