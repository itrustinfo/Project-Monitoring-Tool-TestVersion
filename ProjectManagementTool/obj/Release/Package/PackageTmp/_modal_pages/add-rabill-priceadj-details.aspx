<%@ Page Title="" Language="C#" MasterPageFile="~/_master_pages/modal.Master" AutoEventWireup="true" CodeBehind="add-rabill-priceadj-details.aspx.cs" Inherits="ProjectManagementTool._modal_pages.add_rabill_priceadj_details" %>
<asp:Content ID="Content1" ContentPlaceHolderID="modal_master_head" runat="server">
    <script type="text/javascript">
function isNumberKey(txt, evt) {
      var charCode = (evt.which) ? evt.which : evt.keyCode;
      if (charCode == 46) {
        //Check if the text already contains the . character
        if (txt.value.indexOf('.') === -1) {
          return true;
        } else {
          return false;
        }
      } else {
        if (charCode > 31 &&
          (charCode < 48 || charCode > 57))
          return false;
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
                        <label class="lblCss" for="ddlItem">Select Item (Index)</label>
                         <asp:DropDownList ID="ddlItem" CssClass="form-control" runat="server">
                             
                        </asp:DropDownList>
                    </div>
                       <div class="form-group">
                        <label class="lblCss" for="txtRABill">Source of Index</label>
                         <asp:TextBox ID="txtIndex" CssClass="form-control" runat="server" required ClientIDMode="Static"></asp:TextBox>
                    </div>
                     <div class="form-group">
                        <label class="lblCss" for="txtRABill">Proposed Weighting %</label>
                         <asp:TextBox ID="txtweighting" CssClass="form-control" runat="server" onkeypress="return isNumberKey(this, event);" required ClientIDMode="Static"></asp:TextBox>
                    </div>
                    <div class="form-group">
                        <label class="lblCss" for="txtRABill">Coefficient</label>
                         <asp:TextBox ID="txtCoefficient" CssClass="form-control" runat="server" onkeypress="return isNumberKey(this, event);" required ClientIDMode="Static"></asp:TextBox>
                    </div>
                    <div class="form-group">
                        <label class="lblCss" for="txtInitialIndices">Initial indices</label>
                         <asp:TextBox ID="txtInitialIndices" CssClass="form-control" runat="server" onkeypress="return isNumberKey(this, event);" required ClientIDMode="Static"></asp:TextBox>
                    </div>
                    <div class="form-group">
                        <label class="lblCss" for="txtLatestIndices">Latest indices</label>
                         <asp:TextBox ID="txtLatestIndices" CssClass="form-control" runat="server" onkeypress="return isNumberKey(this, event);" required ClientIDMode="Static"></asp:TextBox>
                    </div>
                    </div>

                </div>
              </div>
        
        <div class="modal-footer">
                  
            <asp:Button ID="btnSubmit" runat="server" Text="Submit" CssClass="btn btn-primary" OnClick="btnSubmit_Click" />
                </div>
        </form>
</asp:Content>
