<%@ Page Title="" Language="C#" MasterPageFile="~/_master_pages/modal.Master" AutoEventWireup="true" CodeBehind="add-insurancepremium.aspx.cs" Inherits="ProjectManagementTool._modal_pages.add_insurancepremium" %>
<asp:Content ID="Content1" ContentPlaceHolderID="modal_master_head" runat="server">
    <script type="text/javascript">
        function toUSD(objctrl) {
           var x = objctrl.value.replace(/,/g, '').replace('₹ ', '').replace('$ ', '').replace('¥ ', '').replace('₹', '').replace('$', '').replace('¥', '');
             x = x.toString();
             x += '';
             var ddlcurrency = document.getElementById('<%= HiddenCurrency.ClientID %>').value;
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
    $("input[id$='dtPremiumPaidDate']").datepicker({
      changeMonth: true,
        changeYear: true,
      dateFormat:'dd/mm/yy'
      });

      $("input[id$='dtDueDate']").datepicker({
      changeMonth: true,
        changeYear: true,
      dateFormat:'dd/mm/yy'
      });
    });
</script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="modal_master_body" runat="server">
    <form id="frmAddInsurancePremiumModal" runat="server">
        <div class="container-fluid" style="max-height:80vh; overflow-y:auto;">
            <div class="row">
                <div class="col-sm-6">
                    <div class="form-group">
                        <label class="lblCss" for="txtpremiumamount">Premium Amount</label> &nbsp;<span style="color:red; font-size:1.1rem;">*</span>
                       <input type="text" id="txtpremiumamount" runat="server" class="form-control" autocomplete="off" onblur="toUSD(this)" required />
                    </div>

                    <div class="form-group">
                        <label class="lblCss" for="txtinterest">Interest</label> &nbsp;<span style="color:red; font-size:1.1rem;">*</span>
                         <input type="text" id="txtinterest" runat="server" class="form-control" autocomplete="off" onblur="toUSD(this)" required />
                    </div>
                    <div class="form-group">
                        <label class="lblCss" for="txtpenalty">Penalty</label> &nbsp;<span style="color:red; font-size:1.1rem;">*</span>
                         <input type="text" id="txtpenalty" runat="server" class="form-control" autocomplete="off" onblur="toUSD(this)" required />
                    </div>
                    <div class="form-group">
                        <label class="lblCss" for="dtPremiumPaidDate">Premium Paid Date</label> &nbsp;<span style="color:red; font-size:1.1rem;">*</span>
                        <asp:TextBox ID="dtPremiumPaidDate" CssClass="form-control" runat="server" placeholder="dd/mm/yyyy" autocomplete="off" required ClientIDMode="Static"></asp:TextBox>
                    </div>
                </div>
                <div class="col-sm-6">
                    <asp:HiddenField ID="HiddenNextDueDate" runat="server" />
                    <asp:HiddenField ID="HiddenCurrency" runat="server" />
                    <asp:HiddenField ID="HiddenCulturelInfo" runat="server" />
                    <div class="form-group">
                        <label class="lblCss" for="dtPremiumPaidDate">Premium Due Date</label> 
                        <asp:TextBox ID="dtDueDate" CssClass="form-control" runat="server" Enabled="false" placeholder="dd/mm/yyyy" autocomplete="off" required ClientIDMode="Static"></asp:TextBox>
                    </div>
                    <div class="form-group">
                        <label class="lblCss" for="FilCoverLetter">Premium Receipt</label>
                        <div class="custom-file">
                            <asp:FileUpload ID="FileUpload1" runat="server" required CssClass="custom-file-input" />

                            <%--<asp:FileUpload ID="FilCoverLetter" CssClass="custom-file-input" runat="server" ClientIDMode="Static" /> --%>
                            <label class="custom-file-label" for="FilCoverLetter">Choose document</label>
                        </div>
                    </div>
                    <div class="form-group">
                        <label class="lblCss" for="txtinterest">Remarks</label> 
                         <asp:TextBox ID="txtrenarks" runat="server" CssClass="form-control" TextMode="MultiLine" autocomplete="off" ></asp:TextBox>
                    </div>
                    </div>
            </div> 
        </div>
        <div class="modal-footer">
            <asp:Button ID="btnSubmit" runat="server" Text="Submit" CssClass="btn btn-primary" OnClick="btnSubmit_Click" />
                </div>
    </form>
</asp:Content>
