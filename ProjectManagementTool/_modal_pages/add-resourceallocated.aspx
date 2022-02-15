<%@ Page Title="" Language="C#" MasterPageFile="~/_master_pages/modal.Master" AutoEventWireup="true" CodeBehind="add-resourceallocated.aspx.cs" Inherits="ProjectManagementTool._modal_pages.add_resourceallocated" %>
<asp:Content ID="Content1" ContentPlaceHolderID="modal_master_head" runat="server">
    <script type="text/javascript">
        function toUSD(objctrl) {
            //Get the Entered Value
    var number = objctrl.value.toString(),
        number = number.replace(',', '');
                //Split the number between dollars and cents
    dollars = number.split('.')[0], cents = (number.split('.')[1] || '') + '00';
    dollars=dollars.replace(',', '');
    dollars = dollars.split('').reverse().join('').replace(/(\d{3}(?!$))/g, '$1,').split('').reverse().join('');
    dollars=dollars.replace(',,', ',');
                //Concatenate the number with currecny symbol
     objctrl.value = dollars + '.' + cents.slice(0, 2);
        }

          function allnumericplusminus(inputtxt) {
              var numbers = /^[0-9]+$/;
              //alert(inputtxt.value);
              if (inputtxt.value != "") {
                  if (inputtxt.value.match(numbers)) {
                      //alert('Correct...Try another');
                      document.getElementById("txtTotalCost").value = parseInt(document.getElementById("txtCost").value) + parseInt(document.getElementById("txtGST").value);
                     
                      return true;
                  }
                  else {
                      alert('Incorrect Input Value');
                      inputtxt.value = "0";
                      inputtxt.focus()
                      return false;
                  }
              }
          }

    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="modal_master_body" runat="server">
    <form id="frmAddResourceAllocationModal" runat="server">
         <div class="container-fluid">
            <div class="row">
                <div class="col-sm-12">
                    
                    <%--<div class="form-group">
                        <label class="lblCss" for="txtMileStone">Resource Name</label>
                        <asp:DropDownList ID="ddlResource" runat="server" CssClass="form-control" AutoPostBack="True" OnSelectedIndexChanged="ddlResource_SelectedIndexChanged">
                     </asp:DropDownList>
                    </div>--%>
                     <div class="form-group">
                        <label class="lblCss" for="txtbudget">BOQ</label> &nbsp;<span style="color:red; font-size:1rem;">*</span>
                        <asp:DropDownList ID="DDLBOQDetails" runat="server" CssClass="form-control" AutoPostBack="true" OnSelectedIndexChanged="DDLBOQDetails_SelectedIndexChanged" required>
                            </asp:DropDownList>
                        </div>
                     <div class="form-group">
                        <%--<label class="lblCss" for="txtBasicBudget">Basic Budget</label> &nbsp;<span style="color:red; font-size:1rem;">*</span>--%>
                         <label class="lblCss" for="txtquantity">Quantity</label>
                         <input type="text" id="txtquantity" disabled="disabled" runat="server" class="form-control" autocomplete="off" required />
                    </div>
                   <div class="form-group">
                        <label class="lblCss" for="txtGST">GST (%)</label> 
                        <input type="text" id="txtGST" runat="server" disabled="disabled" class="form-control" autocomplete="off" required />
                    </div>
                    <div class="form-group">
                        <label class="lblCss" for="txtprice">Price</label> 
                        <input type="text" id="txtprice" runat="server" disabled="disabled" class="form-control" autocomplete="off" required />
                        
                        
                    </div>

                   <%-- <div class="form-group">
                        <label class="lblCss" for="ddlStatus">Cost Type</label>
                        <asp:TextBox ID="txtCostType" runat="server" CssClass="form-control" Enabled="False">0</asp:TextBox>
                    </div>
                     <div class="form-group">
                        <label class="lblCss" for="dtStartdate">Total Cost</label>
                        <asp:TextBox ID="txtTotalCost" runat="server" CssClass="form-control" Enabled="False">0</asp:TextBox>
                    </div>--%>
                     <div class="form-group">
                        <label class="lblCss" for="dtprojectDate">Allocated Quantity</label> &nbsp;<span style="color:red; font-size:1.1rem;">*</span>
                        <asp:TextBox ID="txtAllocatedUnits" runat="server" TextMode="Number" CssClass="form-control" required>0</asp:TextBox>
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
