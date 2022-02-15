<%@ Page Title="" Language="C#" MasterPageFile="~/_master_pages/modal.Master" AutoEventWireup="true" CodeBehind="add-rabill-priceadj.aspx.cs" Inherits="ProjectManagementTool._modal_pages.add_rabill_priceadj" %>
<asp:Content ID="Content1" ContentPlaceHolderID="modal_master_head" runat="server">
    <script type="text/javascript">
        function BindEvents() {
        $(".showBOQModal").click(function(e) {
        //document.getElementsByClassName("showTaskModal").click(function(e) {
            e.preventDefault();
            jQuery.noConflict();
        var url = $(this).attr("href");
        $("#ModBOQData iframe").attr("src", url);
        $("#ModBOQData").modal("show");
        });
        }
        $(document).ready(function () {
            BindEvents();
        });

         $(function () {
            
             $("input[id$='dtLatestDate']").datepicker({
                 changeMonth: true,
                 changeYear: true,
                 dateFormat: 'dd/mm/yy'
             });

             $("input[id$='dtInitialDate']").datepicker({
                 changeMonth: true,
                 changeYear: true,
                 dateFormat: 'dd/mm/yy'
             });
         });
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
                        <label class="lblCss" for="DDLInvoice">RA Bill</label>
                         <asp:DropDownList ID="DDLInvoice" CssClass="form-control" runat="server">
                             
                        </asp:DropDownList>
                       

                    </div>
                       <div class="form-group" style="display :block">
                        <label class="lblCss" for="txtRABill">Adjustment Description</label>
                         <asp:TextBox ID="txtDescription" CssClass="form-control" runat="server" required ClientIDMode="Static"></asp:TextBox>
                    </div>
                      <div class="form-group" id="div2" runat="server" visible="true">
                        <label class="lblCss" for="dtInitialDate" id="Label1" runat="server">Initial Indices Date</label>
                        <asp:TextBox ID="dtInitialDate" CssClass="form-control" runat="server" placeholder="dd/mm/yyyy" autocomplete="off" required ClientIDMode="Static"></asp:TextBox>
                    </div>
                     <div class="form-group" id="div1" runat="server" visible="true">
                        <label class="lblCss" for="dtLatestDate" id="Label2" runat="server">Latest Indices Date</label>
                        <asp:TextBox ID="dtLatestDate" CssClass="form-control" runat="server" placeholder="dd/mm/yyyy" autocomplete="off" required ClientIDMode="Static"></asp:TextBox>
                    </div>
                    </div>

                </div>
              </div>
        
        <div class="modal-footer">
                  
            <asp:Button ID="btnSubmit" runat="server" Text="Submit" CssClass="btn btn-primary" OnClick="btnSubmit_Click" />
                </div>
        </form>
</asp:Content>
