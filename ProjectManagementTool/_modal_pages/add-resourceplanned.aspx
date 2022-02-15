<%@ Page Title="" Language="C#" MasterPageFile="~/_master_pages/modal.Master" AutoEventWireup="true" CodeBehind="add-resourceplanned.aspx.cs" Inherits="ProjectManagementTool._modal_pages.add_resourceplanned" %>
<asp:Content ID="Content1" ContentPlaceHolderID="modal_master_head" runat="server">
        <style type="text/css">
        .TheDateTimePicker{display:block;width:100%;height:calc(1.5em + .75rem + 2px);padding:.375rem .75rem;font-size:1rem;font-weight:400;line-height:1.5;color:#495057;background-color:#fff;background-clip:padding-box;border:1px solid #ced4da;border-radius:.25rem;transition:border-color .15s ease-in-out,box-shadow .15s ease-in-out;}
    </style>
      <script type="text/javascript">
        $(function () {
        bindDatePickers(); // bind date picker on first page load
        Sys.WebForms.PageRequestManager.getInstance().add_endRequest(bindDatePickers); // bind date picker on every UpdatePanel refresh
    });

          function bindDatePickers() {
              $('.TheDateTimePicker').datepicker({ dateFormat: 'dd/mm/yy',changeMonth: true,changeYear: true });
          }
</script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="modal_master_body" runat="server">
    <form id="frmAddTaskSchedule" runat="server">
       <asp:ScriptManager ID="scrpManager" runat="server" />
        <div class="container-fluid" style="max-height:84vh; overflow-y:auto; min-height:84vh;">
            <asp:UpdatePanel runat="server" ID="myUpdtPanel" UpdateMode="Conditional">   
            <ContentTemplate>
                
                <asp:HiddenField ID="HiddenAction" runat="server" />
            <div class="row">
                 <div class="col-sm-6">
                     <div class="form-group">
                        <label class="lblCss" for="RBScheduleTye">Planned Type</label>
                          <asp:RadioButtonList ID="RBScheduleTye" runat="server" Width="100%" CssClass="lblCss" CellPadding="5" RepeatDirection="Horizontal" AutoPostBack="true" OnSelectedIndexChanged="RBScheduleTye_SelectedIndexChanged" RepeatColumns="4">
                                 <asp:ListItem Selected="True" Value="Month">&nbsp;&nbsp;By Month</asp:ListItem>
                                 <%--<asp:ListItem Value="Date">&nbsp;&nbsp;By Date</asp:ListItem>--%>
                             </asp:RadioButtonList>
                    </div>
                     </div>
                <div class="col-sm-6">
                    </div>
                </div>
            <div class="row" id="ByMonth" runat="server">
                <div class="col-sm-4">
                    <%--<asp:PlaceHolder ID="PlaceHolder1" runat="server"></asp:PlaceHolder>--%>

                    <asp:PlaceHolder ID="PlaceHolder1" runat="server" EnableViewState="true"></asp:PlaceHolder>
                  </div>
                <div class="col-sm-4">
                   <asp:PlaceHolder ID="PlaceHolder2" runat="server" EnableViewState="true"></asp:PlaceHolder>
                    </div>
                <div class="col-sm-4">
                    <asp:PlaceHolder ID="PlaceHolder3" runat="server" EnableViewState="true"></asp:PlaceHolder>
                    </div>
               <%-- <div class="col-sm-3">
                    <asp:PlaceHolder ID="PnlDelete" runat="server" EnableViewState="true"></asp:PlaceHolder>
                    </div>--%>
                <div class="col-sm-12">
                    <br />
                    <asp:Button ID="btnAdd" runat="server" CausesValidation="false" CssClass="btn btn-primary" Text="Add New" OnClick="btnAdd_Click" />

                    <asp:Button ID="btnRemove" runat="server" CausesValidation="false" CssClass="btn btn-primary" Text="Remove" OnClick="btnRemove_Click" />
                    </div>
                
                </div>
            <div class="row" id="ByDate" runat="server" visible="false">
               
                <div class="col-sm-4">
               <asp:PlaceHolder ID="PlaceHolder4" runat="server" EnableViewState="true"></asp:PlaceHolder>
                    </div>
                <div class="col-sm-4">
               <asp:PlaceHolder ID="PlaceHolder5" runat="server" EnableViewState="true"></asp:PlaceHolder>
                    </div>
                <div class="col-sm-4">
               <asp:PlaceHolder ID="PlaceHolder6" runat="server" EnableViewState="true"></asp:PlaceHolder>
                    </div>
                <div class="col-sm-12">
                    <br />
                    <asp:Button ID="btnaddDatewise" runat="server" CausesValidation="false" CssClass="btn btn-primary" Text="Add New" OnClick="btnaddDatewise_Click" />
                    </div>
                </div>
                </ContentTemplate>
                <Triggers>
                    <asp:AsyncPostBackTrigger ControlID="RBScheduleTye" EventName="SelectedIndexChanged" />
                    <asp:AsyncPostBackTrigger ControlID="btnAdd" EventName="Click" />
                </Triggers>
                </asp:UpdatePanel>
            </div>

        <div class="modal-footer">
                    <%--<button type="button" class="btn btn-secondary" data-dismiss="modal">Close</button>--%>
                    <%--<button type="button" class="btn btn-primary">Save changes</button>--%>
            <asp:Button ID="btnSave" runat="server" Text="Save" CssClass="btn btn-primary" OnClick="btnSave_Click" />
                </div>
        </form>
</asp:Content>
