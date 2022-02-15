<%@ Page Title="" Language="C#" MasterPageFile="~/_master_pages/modal.Master" AutoEventWireup="true" CodeBehind="add-Fin-Month-data.aspx.cs" Inherits="ProjectManagementTool._modal_pages.add_Fin_Month_data" %>
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
                        <label class="lblCss" for="ddlMonth">Select Moth</label>
                         <asp:DropDownList ID="ddlItem" CssClass="form-control" runat="server">
                             <asp:ListItem Value="Jan">January</asp:ListItem>
                             <asp:ListItem Value="Feb">Febuary</asp:ListItem>
                             <asp:ListItem Value="Mar">March</asp:ListItem>
                             <asp:ListItem Value="Apr">Aprill</asp:ListItem>
                             <asp:ListItem>May</asp:ListItem>
                             <asp:ListItem Value="Jun">June</asp:ListItem>
                             <asp:ListItem Value="Jul">July</asp:ListItem>
                             <asp:ListItem Value="Aug">August</asp:ListItem>
                             <asp:ListItem Value="Sep">September</asp:ListItem>
                             <asp:ListItem Value="Oct">October</asp:ListItem>
                             <asp:ListItem Value="Nov">November</asp:ListItem>
                             <asp:ListItem Value="Dec">December</asp:ListItem>
                             
                        </asp:DropDownList>
                    </div>
                       <div class="form-group">
                        <label class="lblCss" for="ddlYear">Select Year</label>
                          <asp:DropDownList ID="ddlYear" CssClass="form-control" runat="server">
                              <asp:ListItem Value="17">2017</asp:ListItem>
                              <asp:ListItem Value="18">2018</asp:ListItem>
                              <asp:ListItem Value="19">2019</asp:ListItem>
                              <asp:ListItem Value="20">2020</asp:ListItem>
                              <asp:ListItem Value="21">2021</asp:ListItem>
                              <asp:ListItem Value="22">2022</asp:ListItem>
                              <asp:ListItem Value="23">2023</asp:ListItem>
                              <asp:ListItem Value="24">2024</asp:ListItem>
                              <asp:ListItem Value="25">2025</asp:ListItem>
                              <asp:ListItem Value="26">2026</asp:ListItem>
                              <asp:ListItem Value="27">2027</asp:ListItem>
                              <asp:ListItem Value="28">2028</asp:ListItem>
                              <asp:ListItem Value="29">2029</asp:ListItem>
                              <asp:ListItem Value="30">2030</asp:ListItem>
                             
                        </asp:DropDownList>
                    </div>
                    <div class="form-group">
                        <label class="lblCss" for="txtPayment">Approved Payment (Crores.)</label>
                         <asp:TextBox ID="txtPayment" CssClass="form-control" runat="server" onkeypress="return isNumberKey(this, event);" required ClientIDMode="Static"></asp:TextBox>
                    </div>
                    </div>

                </div>
              </div>
        
        <div class="modal-footer">
                  
            <asp:Button ID="btnSubmit" runat="server" Text="Submit" CssClass="btn btn-primary" OnClick="btnSubmit_Click" />
                </div>
        </form>
</asp:Content>
