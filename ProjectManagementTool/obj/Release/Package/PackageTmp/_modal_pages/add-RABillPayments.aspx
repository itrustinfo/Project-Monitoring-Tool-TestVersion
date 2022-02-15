<%@ Page Title="" Language="C#" MasterPageFile="~/_master_pages/modal.Master" AutoEventWireup="true" CodeBehind="add-RABillPayments.aspx.cs" Inherits="ProjectManagementTool._modal_pages.add_RABillPayments" %>
<asp:Content ID="Content1" ContentPlaceHolderID="modal_master_head" runat="server">

     <script type="text/javascript">
 $( function() {
    $("input[id$='dtPaymentDate']").datepicker({
      changeMonth: true,
        changeYear: true,
      dateFormat:'dd/mm/yy'
      });
    });
</script>


    <script type="text/javascript">
        function isNumberKey(evt, element) {
          //  alert(element);
  var charCode = (evt.which) ? evt.which : event.keyCode
  if (charCode > 31 && (charCode < 48 || charCode > 57) && !(charCode == 46 || charCode == 8))
    return false;
  else {
    var len = $(element).val().length;
    var index = $(element).val().indexOf('.');
    if (index > 0 && charCode == 46) {
      return false;
    }
    if (index > 0) {
      var CharAfterdot = (len + 1) - index;
      if (CharAfterdot > 3) {
        return false;
      }
      }
      //
      var totaldeductions = 0.0;
      if (document.getElementById("txtDeducPer1").value != "" && document.getElementById("txtAmnt").value != "") {
          document.getElementById("txtDeduction1").value = (parseFloat(document.getElementById("txtDeducPer1").value) * parseFloat(document.getElementById("txtAmnt").value)) / 100;
          totaldeductions = totaldeductions + parseFloat(document.getElementById("txtDeduction1").value);
           document.getElementById("txtNetAmnt").value = parseFloat(document.getElementById("txtAmnt").value) - parseFloat(totaldeductions);
      }
       if (document.getElementById("txtDeducPer2").value != "" && document.getElementById("txtAmnt").value != "") {
          
          document.getElementById("txtDeduction2").value = (parseFloat(document.getElementById("txtDeducPer2").value) * parseFloat(document.getElementById("txtAmnt").value)) / 100;
           totaldeductions = totaldeductions + parseFloat(document.getElementById("txtDeduction2").value);
            document.getElementById("txtNetAmnt").value = parseFloat(document.getElementById("txtAmnt").value) - parseFloat(totaldeductions);
      }
       if (document.getElementById("txtDeducPer3").value != "" && document.getElementById("txtAmnt").value != "") {
        
          document.getElementById("txtDeduction3").value = (parseFloat(document.getElementById("txtDeducPer3").value) * parseFloat(document.getElementById("txtAmnt").value)) / 100;
           totaldeductions = totaldeductions + parseFloat(document.getElementById("txtDeduction3").value);
            document.getElementById("txtNetAmnt").value = parseFloat(document.getElementById("txtAmnt").value) - parseFloat(totaldeductions);
      }
       if (document.getElementById("txtDeducPer4").value != "" && document.getElementById("txtAmnt").value != "") {
          document.getElementById("txtDeduction4").value = (parseFloat(document.getElementById("txtDeducPer4").value) * parseFloat(document.getElementById("txtAmnt").value)) / 100;
           totaldeductions = totaldeductions + parseFloat(document.getElementById("txtDeduction4").value);
            document.getElementById("txtNetAmnt").value = parseFloat(document.getElementById("txtAmnt").value) - parseFloat(totaldeductions);
       }
        if (document.getElementById("txtDeducPer5").value != "" && document.getElementById("txtAmnt").value != "") {
            document.getElementById("txtDeduction5").value = (parseFloat(document.getElementById("txtDeducPer5").value) * parseFloat(document.getElementById("txtAmnt").value)) / 100;
            totaldeductions = totaldeductions + parseFloat(document.getElementById("txtDeduction5").value);
             document.getElementById("txtNetAmnt").value = parseFloat(document.getElementById("txtAmnt").value) - parseFloat(totaldeductions);
      }
        if (document.getElementById("txtDeducPer6").value != "" && document.getElementById("txtAmnt").value != "") {
            document.getElementById("txtDeduction6").value = (parseFloat(document.getElementById("txtDeducPer6").value) * parseFloat(document.getElementById("txtAmnt").value)) / 100;
            totaldeductions = totaldeductions + parseFloat(document.getElementById("txtDeduction6").value);
             document.getElementById("txtNetAmnt").value = parseFloat(document.getElementById("txtAmnt").value) - parseFloat(totaldeductions);
      }
        if (document.getElementById("txtDeducPer7").value != "" && document.getElementById("txtAmnt").value != "") {
            document.getElementById("txtDeduction7").value = (parseFloat(document.getElementById("txtDeducPer7").value) * parseFloat(document.getElementById("txtAmnt").value)) / 100;
            totaldeductions = totaldeductions + parseFloat(document.getElementById("txtDeduction7").value);
             document.getElementById("txtNetAmnt").value = parseFloat(document.getElementById("txtAmnt").value) - parseFloat(totaldeductions);
      }
        if (document.getElementById("txtDeducPer8").value != "" && document.getElementById("txtAmnt").value != "") {
            document.getElementById("txtDeduction8").value = (parseFloat(document.getElementById("txtDeducPer8").value) * parseFloat(document.getElementById("txtAmnt").value)) / 100;
            totaldeductions = totaldeductions + parseFloat(document.getElementById("txtDeduction8").value);
             document.getElementById("txtNetAmnt").value = parseFloat(document.getElementById("txtAmnt").value) - parseFloat(totaldeductions);
      }
        if (document.getElementById("txtDeducPer9").value != "" && document.getElementById("txtAmnt").value != "") {
            document.getElementById("txtDeduction9").value = (parseFloat(document.getElementById("txtDeducPer9").value) * parseFloat(document.getElementById("txtAmnt").value)) / 100;
            totaldeductions = totaldeductions + parseFloat(document.getElementById("txtDeduction9").value);
             document.getElementById("txtNetAmnt").value = parseFloat(document.getElementById("txtAmnt").value) - parseFloat(totaldeductions);
      }
        if (document.getElementById("txtDeducPer10").value != "" && document.getElementById("txtAmnt").value != "") {
            document.getElementById("txtDeduction10").value = (parseFloat(document.getElementById("txtDeducPer10").value) * parseFloat(document.getElementById("txtAmnt").value)) / 100;
            totaldeductions = totaldeductions + parseFloat(document.getElementById("txtDeduction10").value);
             document.getElementById("txtNetAmnt").value = parseFloat(document.getElementById("txtAmnt").value) - parseFloat(totaldeductions);
      }
     
  }
  return true;
}
    </script>
    
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="modal_master_body" runat="server">
    <form id="frmAddSubmittalModal" runat="server">
        <asp:ScriptManager ID="ScriptManager1" runat="server"></asp:ScriptManager>
          <div class="container-fluid">
            <asp:HiddenField ID="HiddenParentTask" runat="server" />
            <div class="row">
                <div class="col-sm-12">
    
                    <div class="form-group">
                        <label class="lblCss" for="DDLInvoice">Bill Invoice</label>
                         <asp:DropDownList ID="DDLInvoice" CssClass="form-control" runat="server" AutoPostBack="True" OnSelectedIndexChanged="DDLInvoice_SelectedIndexChanged">
                             
                        </asp:DropDownList>
                       

                    </div>
                     <div class="form-group" style="display :none">
                        <label class="lblCss" for="txtRABill">RABill</label>
                         <asp:TextBox ID="txtRABill" CssClass="form-control" runat="server"  ClientIDMode="Static"></asp:TextBox>
                    </div>
                    <div class="form-group">
                        <label class="lblCss" for="txtAmnt">Amount</label>
                         <asp:TextBox ID="txtAmnt" CssClass="form-control" runat="server" required ClientIDMode="Static" onkeyup="return isNumberKey(event,this)" Enabled="False"></asp:TextBox>
                    </div>
                      <div class="form-group" id="divD1" runat="server" visible="false">
                        <label class="lblCss" for="txtDeduction1" id="lblDeduction1" runat="server">Amount</label>&nbsp;<asp:TextBox ID="txtDeducPer1" runat="server" Width="40px" ClientIDMode="Static" onkeyup="return isNumberKey(event,this)"></asp:TextBox>
                         &nbsp;%<asp:TextBox ID="txtDeduction1" CssClass="form-control" runat="server"  ClientIDMode="Static"></asp:TextBox>
                    </div>
                      <div class="form-group" id="divD2" runat="server" visible="false">
                        <label class="lblCss" for="txtDeduction2" id="lblDeduction2" runat="server">Amount</label>&nbsp;<asp:TextBox ID="txtDeducPer2" runat="server" Width="40px"  ClientIDMode="Static" onkeyup="return isNumberKey(event,this)"></asp:TextBox>
                         &nbsp;%<asp:TextBox ID="txtDeduction2" CssClass="form-control" runat="server"  ClientIDMode="Static"></asp:TextBox>
                    </div>
                     <div class="form-group" id="divD3" runat="server" visible="false">
                        <label class="lblCss" for="txtDeduction3" id="lblDeduction3" runat="server">Amount</label>&nbsp;<asp:TextBox ID="txtDeducPer3" runat="server" Width="40px"  ClientIDMode="Static" onkeyup="return isNumberKey(event,this)"></asp:TextBox>
                         &nbsp;%<asp:TextBox ID="txtDeduction3" CssClass="form-control" runat="server"  ClientIDMode="Static"></asp:TextBox>
                    </div>
                       <div class="form-group" id="divD4" runat="server" visible="false">
                        <label class="lblCss" for="txtDeduction4" id="lblDeduction4" runat="server">Amount</label>&nbsp;<asp:TextBox ID="txtDeducPer4" runat="server" Width="40px"  ClientIDMode="Static" onkeyup="return isNumberKey(event,this)"></asp:TextBox>
                         &nbsp;%<asp:TextBox ID="txtDeduction4" CssClass="form-control" runat="server"  ClientIDMode="Static"></asp:TextBox>
                    </div>
                      <div class="form-group" id="divD5" runat="server" visible="false">
                        <label class="lblCss" for="txtDeduction5" id="lblDeduction5" runat="server">Amount</label>&nbsp;<asp:TextBox ID="txtDeducPer5" runat="server" Width="40px"  ClientIDMode="Static" onkeyup="return isNumberKey(event,this)"></asp:TextBox>
                         &nbsp;%<asp:TextBox ID="txtDeduction5" CssClass="form-control" runat="server"  ClientIDMode="Static"></asp:TextBox>

                    </div>
                     <div class="form-group" id="divD6" runat="server" visible="false">
                        <label class="lblCss" for="txtDeduction6" id="lblDeduction6" runat="server">Amount</label>&nbsp;<asp:TextBox ID="txtDeducPer6" runat="server" Width="40px"  ClientIDMode="Static" onkeyup="return isNumberKey(event,this)"></asp:TextBox>
                         &nbsp;%<asp:TextBox ID="txtDeduction6" CssClass="form-control" runat="server"  ClientIDMode="Static"></asp:TextBox>
                    </div>
                      <div class="form-group" id="divD7" runat="server" visible="false">
                        <label class="lblCss" for="txtDeduction7" id="lblDeduction7" runat="server">Amount</label>&nbsp;<asp:TextBox ID="txtDeducPer7" runat="server" Width="40px"  ClientIDMode="Static" onkeyup="return isNumberKey(event,this)"></asp:TextBox>
                         &nbsp;%<asp:TextBox ID="txtDeduction7" CssClass="form-control" runat="server"  ClientIDMode="Static"></asp:TextBox>
                    </div>
                        <div class="form-group" id="divD8" runat="server" visible="false">
                        <label class="lblCss" for="txtDeduction8" id="lblDeduction8" runat="server">Amount</label>&nbsp;<asp:TextBox ID="txtDeducPer8" runat="server" Width="40px"  ClientIDMode="Static" onkeyup="return isNumberKey(event,this)"></asp:TextBox>
                         &nbsp;%<asp:TextBox ID="txtDeduction8" CssClass="form-control" runat="server"  ClientIDMode="Static"></asp:TextBox>
                    </div>
                    <div class="form-group" id="divD9" runat="server" visible="false">
                        <label class="lblCss" for="txtDeduction9" id="lblDeduction9" runat="server">Amount</label>&nbsp;<asp:TextBox ID="txtDeducPer9" runat="server" Width="40px"  ClientIDMode="Static" onkeyup="return isNumberKey(event,this)"></asp:TextBox>
                         &nbsp;%<asp:TextBox ID="txtDeduction9" CssClass="form-control" runat="server"  ClientIDMode="Static"></asp:TextBox>
                    </div>
                    <div class="form-group" id="divD10" runat="server" visible="false">
                        <label class="lblCss" for="txtDeduction10" id="lblDeduction10" runat="server">Amount</label>&nbsp;<asp:TextBox ID="txtDeducPer10" runat="server" Width="40px"  ClientIDMode="Static" onkeyup="return isNumberKey(event,this)"></asp:TextBox>
                         &nbsp;%<asp:TextBox ID="txtDeduction10" CssClass="form-control" runat="server"  ClientIDMode="Static"></asp:TextBox>
                    </div>
                     <div class="form-group" id="divNetAmount" runat="server" visible="true">
                        <label class="lblCss" for="txtNetAmnt" id="Label1" runat="server">Net Amount</label>
                         <asp:TextBox ID="txtNetAmnt" CssClass="form-control" runat="server"  ClientIDMode="Static" onkeyup="return isNumberKey(event,this)" Enabled="False"></asp:TextBox>
                    </div>
                     <div class="form-group" id="div1" runat="server" visible="true">
                        <label class="lblCss" for="dtPaymentDate" id="Label2" runat="server">Payment Date</label>
                        <asp:TextBox ID="dtPaymentDate" CssClass="form-control" runat="server" placeholder="dd/mm/yyyy" autocomplete="off" required ClientIDMode="Static"></asp:TextBox>
                    </div>
                    </div>

                </div>
              </div>
        
        <div class="modal-footer">
                  
            <asp:Button ID="btnSubmit" runat="server" Text="Submit" CssClass="btn btn-primary" OnClick="btnSubmit_Click" />
                </div>
        </form>
</asp:Content>
