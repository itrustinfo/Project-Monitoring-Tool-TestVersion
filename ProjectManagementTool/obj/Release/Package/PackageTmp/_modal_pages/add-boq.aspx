<%@ Page Title="" Language="C#" MasterPageFile="~/_master_pages/modal.Master" AutoEventWireup="true" CodeBehind="add-boq.aspx.cs" Inherits="ProjectManagementTool._modal_pages.add_boq" %>
<asp:Content ID="Content1" ContentPlaceHolderID="modal_master_head" runat="server">
    <script type="text/javascript">
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
     <form id="FrmAddResource" runat="server">
        <asp:ScriptManager ID="ScriptManager1" runat="server"></asp:ScriptManager>
        <div class="container-fluid" style="max-height:80vh; overflow-y:auto; margin-top:15px;">
            <div class="row">
                <div class="col-sm-6">
                    <div class="form-group">
                        <label class="lblCss" for="txtitemnumber">Item_Number</label> &nbsp;<span style="color:red; font-size:1.1rem;">*</span>
                         <asp:TextBox ID="txtitemnumber" runat="server" CssClass="form-control" autocomplete="off" required></asp:TextBox>
                    </div>
                    <div class="form-group">
                        <label class="lblCss" for="txtdesc">Description</label> &nbsp;<span style="color:red; font-size:1.1rem;">*</span>
                         <asp:TextBox ID="txtdesc" runat="server" CssClass="form-control" TextMode="MultiLine" autocomplete="off" required></asp:TextBox>
                    </div>
                    <div class="form-group">
                        <label class="lblCss" for="DDLUnit">Unit for Measurement</label> &nbsp;<span style="color:red; font-size:1.1rem;">*</span>
                         <asp:DropDownList ID="DDLUnit" CssClass="form-control" runat="server" required></asp:DropDownList>
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
                        <label class="lblCss" for="txtquantity">Quantity</label> &nbsp;<span style="color:red; font-size:1.1rem;">*</span>
                         <input type="text" id="txtquantity" runat="server" class="form-control" autocomplete="off" required />
                    </div>
                     <div class="form-group">
                        <label class="lblCss" for="txtprice">Price</label> &nbsp;<span style="color:red; font-size:1.1rem;">*</span>
                        <input type="text" id="txtprice" runat="server" class="form-control" autocomplete="off" required  onblur="toUSD(this)"/>
                    </div>
                   <div class="form-group">
                        <label class="lblCss" for="txtGST">GST(%)</label> &nbsp;<span style="color:red; font-size:1.1rem;">*</span>
                        <input type="text" id="txtGST" runat="server" class="form-control" autocomplete="off" required />
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
