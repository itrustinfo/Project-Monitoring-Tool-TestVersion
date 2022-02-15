<%@ Page Title="" Language="C#" MasterPageFile="~/_master_pages/modal.Master" AutoEventWireup="true" CodeBehind="add-contractor.aspx.cs" Inherits="ProjectManagementTool._modal_pages.add_contractor" %>
<asp:Content ID="Content1" ContentPlaceHolderID="modal_master_head" runat="server">
    <script type="text/javascript">
    
             function ShowCurrencyFormat() {
             var Budget = document.getElementById('<%= txtcontractvalue.ClientID %>').value.replace(/,/g, '').replace('₹ ', '').replace('$ ', '').replace('¥ ', '').replace('₹', '').replace('$', '').replace('¥', '');
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
                         document.getElementById('<%= txtcontractvalue.ClientID %>').value = ddlcurrency + ' ' + x;
                     }
                     else {
                         
                         var lastThree = x.substring(x.length - 3);
                         var otherNumbers = x.substring(0, x.length - 3);
                         if (otherNumbers != '')
                             lastThree = "," + lastThree;
                         var res = otherNumbers.replace(/\B(?=(\d{2})+(?!\d))/g, ",") + lastThree;
                         document.getElementById('<%= txtcontractvalue.ClientID %>').value = ddlcurrency + ' ' + res;
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
                     document.getElementById('<%= txtcontractvalue.ClientID %>').value = ddlcurrency + ' ' + (x1 + x2);
                     
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
        function FillCompletionDate() {
            var stDate = document.getElementById('<%= dtStartDate.ClientID %>').value;
            var days = document.getElementById('<%= txtduration.ClientID %>').value;
            if (stDate != "" && days != "") {
                //stDate = stDate.split('/');
                //var cEndDate = new Date(stDate[2], stDate[1], stDate[0]);
                
                //cEndDate.setMonth(cEndDate.getMonth() + days);
                //cEndDate = cEndDate.addMonths(days);
                //alert(cEndDate.getMonth());
                //document.getElementById('<%= dtCompletionDate.ClientID %>').value = cEndDate.toDateString();
                
            }
        }

    </script>

    <script>
  $( function() {
    $("input[id$='dtAgreementDate']").datepicker({
      changeMonth: true,
        changeYear: true,
      dateFormat:'dd/mm/yy'
      });

      $("input[id$='dtStartDate']").datepicker({
      changeMonth: true,
        changeYear: true,
      dateFormat:'dd/mm/yy'
      });

      $("input[id$='dtAcceptanceDate']").datepicker({
      changeMonth: true,
        changeYear: true,
      dateFormat:'dd/mm/yy'
      });
      $("input[id$='dtCompletionDate']").datepicker({
      changeMonth: true,
        changeYear: true,
      dateFormat:'dd/mm/yy'
    });
    });
  </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="modal_master_body" runat="server">
    <form id="AddContractor" runat="server">
        <div class="container-fluid" style="min-height:85vh; overflow-y:auto; max-height:85vh;">
        <div class="row">
            <div class="col-sm-6">
               <div class="form-group">
                                <label class="lblCss" for="txtcontractorname">Contractor Name</label> &nbsp;<span style="color:red; font-size:1.1rem;">*</span>
                                <asp:TextBox ID="txtcontractorname" runat="server" CssClass="form-control" autocomplete="off" required></asp:TextBox>
                   </div>
                <div class="form-group">
                                <label class="lblCss" for="txtcontractorcode">Contractor Code</label> &nbsp;<span style="color:red; font-size:1.1rem;">*</span>
                                <asp:TextBox ID="txtcontractorcode" runat="server" CssClass="form-control" autocomplete="off" required></asp:TextBox>
                   </div>
               <div class="form-group">
                                <label class="lblCss" for="txttype">Type of Contract</label> &nbsp;<span style="color:red; font-size:1.1rem;">*</span>
                                <asp:TextBox ID="txttype" runat="server" CssClass="form-control" autocomplete="off" required></asp:TextBox>
                            </div>
                <div class="form-group">
                                <label class="lblCss" for="txtrepresentatives">Contractor Representatives</label> &nbsp;<span style="color:red; font-size:1.1rem;">*</span>
                                <asp:TextBox ID="txtrepresentatives" runat="server" CssClass="form-control" autocomplete="off" required></asp:TextBox>
                                         
                            </div>
               <%-- <div class="form-group">
                                <label class="lblCss" for="txtrepresentativescontactnumber">Contractor Representatives Contact Number</label>
                                <asp:TextBox ID="txtrepresentativescontactnumber" runat="server" CssClass="form-control" autocomplete="off" required></asp:TextBox>
                                         
                            </div>--%>
                 <div class="form-group">
                                <label class="lblCss" for="txtrepresentatives">Contract Duration(in Months)</label> &nbsp;<span style="color:red; font-size:1.1rem;">*</span>
                                <asp:TextBox ID="txtduration" runat="server" CssClass="form-control" TextMode="Number" required></asp:TextBox>
                                         
                            </div>
                <div class="form-group">
                                <label class="lblCss" id="LblProjectNumber" runat="server" for="txtrepresentatives">NJSEI Project Number</label> &nbsp;<span style="color:red; font-size:1.1rem;">*</span>
                                <asp:TextBox ID="txtnjseinumber" runat="server" CssClass="form-control" autocomplete="off" required></asp:TextBox>
                                         
                            </div>
            
                <div class="form-group">
                                <label class="lblCss" for="txtcode">Contractor Representatives Details</label> &nbsp;<span style="color:red; font-size:1.2rem;"></span> 
                                <asp:TextBox ID="txtrepresentativesdetails" TextMode="MultiLine" runat="server" autocomplete="off" CssClass="form-control" style="height: 150px;"></asp:TextBox>
                            </div>
                     <div class="form-group">
                                <label class="lblCss" for="txtrepresentatives">Project Specific Package Number</label> &nbsp;<span style="color:red; font-size:1.1rem;">*</span>
                                <asp:TextBox ID="txtProjectSpecificNumber" runat="server" CssClass="form-control" autocomplete="off" required></asp:TextBox>
                                         
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
                                <label class="lblCss" for="txtcontractvalue">Contract Value</label> &nbsp;<span style="color:red; font-size:1.1rem;">*</span>
                                <input type="text" id="txtcontractvalue" runat="server" class="form-control" autocomplete="off" onblur="toUSD(this)" required />
                                         
                            </div>
               
                <div class="form-group">
                                <label class="lblCss" for="dtAgreementDate">Agreement Date</label> &nbsp;<span style="color:red; font-size:1.1rem;">*</span>
                                <asp:TextBox ID="dtAgreementDate" CssClass="form-control" runat="server" autocomplete="off" placeholder="dd/mm/yyyy" required ClientIDMode="Static"></asp:TextBox>
                                         
                            </div>

                <div class="form-group">
                                <label class="lblCss" for="dtStartDate">Start Date</label> &nbsp;<span style="color:red; font-size:1.1rem;">*</span>
                                <asp:TextBox ID="dtStartDate" CssClass="form-control" runat="server" autocomplete="off" placeholder="dd/mm/yyyy" required ClientIDMode="Static"></asp:TextBox>
                                         
                            </div>
                <div class="form-group">
                                <label class="lblCss" for="dtAcceptanceDate">Letter of Acceptance</label> &nbsp;<span style="color:red; font-size:1.1rem;">*</span>
                                <asp:TextBox ID="dtAcceptanceDate" CssClass="form-control" runat="server" autocomplete="off" placeholder="dd/mm/yyyy" required ClientIDMode="Static"></asp:TextBox>
                                         
                            </div>
                
                <div class="form-group"> 
                                <label class="lblCss" for="dtCompletionDate">Completion Date</label> &nbsp;<span style="color:red; font-size:1.1rem;">*</span>
                                <asp:TextBox ID="dtCompletionDate" CssClass="form-control" runat="server" autocomplete="off" placeholder="dd/mm/yyyy" required ClientIDMode="Static"></asp:TextBox>
                                         
                            </div>
                 <div class="form-group">
                                <label class="lblCss" for="txtcode">Company Details</label> &nbsp;<span style="color:red; font-size:1.2rem;"></span> 
                                <asp:TextBox ID="txtCompanyDetails" TextMode="MultiLine" runat="server" autocomplete="off" CssClass="form-control" style="height: 150px;"></asp:TextBox>
                            </div>
               <%--  <div class="form-group">
                                <label class="lblCss" for="txtcontractoraddress">Contractor Address</label>
                                <asp:TextBox ID="txtcontractoraddress" runat="server" CssClass="form-control" TextMode="MultiLine" required></asp:TextBox>
                                         
                            </div>--%>
                </div>
            </div>
        </div>

        <div class="modal-footer">
            <asp:Button ID="btnSubmit" runat="server" Text="Submit" CssClass="btn btn-primary" OnClick="btnSubmit_Click" />
                </div>
    </form>
</asp:Content>
