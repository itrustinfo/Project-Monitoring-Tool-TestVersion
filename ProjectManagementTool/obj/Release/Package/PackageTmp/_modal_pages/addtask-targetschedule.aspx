<%@ Page Title="" Language="C#" MasterPageFile="~/_master_pages/modal.Master" AutoEventWireup="true" CodeBehind="addtask-targetschedule.aspx.cs" Inherits="ProjectManagementTool._modal_pages.addtask_targetschedule" %>
<%@ Register Src="~/_modal_pages/TaskScheduleUserControl.ascx" TagName="UserControl" TagPrefix="uc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="modal_master_head" runat="server">
    <script type = "text/javascript">
    function SetSource(SourceID)
    {
        var hidSourceID = document.getElementById("<%=hidSourceID.ClientID%>");
        hidSourceID.value = SourceID;
    }
</script>
     <script type = "text/javascript">

        function VersionConfirm() {
            if (document.getElementById('<%= HiddenAction.ClientID%>').value == "Update") {
                var confirm_value = document.createElement("INPUT");
            confirm_value.type = "hidden";
            confirm_value.name = "confirm_value";
            if (confirm("Do you want to create a new schedule version?")) {
                confirm_value.value = "Yes";
            } else {
                confirm_value.value = "No";
            }
            document.forms[0].appendChild(confirm_value);
            }
        }
    </script>

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
      //$("input[id$='dtEndDate']").datepicker({
      //changeMonth: true,
      //  changeYear: true,
      //dateFormat:'dd/mm/yy'
      //});
</script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="modal_master_body" runat="server">
    <form id="frmAddTaskSchedule" runat="server">
    <div>
        <asp:HiddenField ID="hidSourceID" runat="server" />
        <asp:ScriptManager ID="sm1" runat="server" />
        <div class="container-fluid" style="max-height:84vh; overflow-y:auto; min-height:84vh;">
             <asp:UpdatePanel ID="up1" runat="server">
            <ContentTemplate>
                <asp:HiddenField ID="HiddenAction" runat="server" />
                 <div class="row">
                 <div class="col-sm-6">
                     <div class="form-group">
                        <label class="lblCss" for="RBScheduleTye">Schedule Type</label>
                          <asp:RadioButtonList ID="RBScheduleTye" runat="server" Width="100%" CssClass="lblCss" CellPadding="5" RepeatDirection="Horizontal" AutoPostBack="true" OnSelectedIndexChanged="RBScheduleTye_SelectedIndexChanged" RepeatColumns="4">
                                 <asp:ListItem Selected="True" Value="Month">&nbsp;&nbsp;By Month</asp:ListItem>
                                 <asp:ListItem Value="Date">&nbsp;&nbsp;By Date</asp:ListItem>
                             </asp:RadioButtonList>
                    </div>
                     </div>
                <div class="col-sm-6">
                    </div>
                </div>
                <div class="row" id="ByMonth" runat="server">
                    <asp:PlaceHolder ID="ph1" runat="server" />
                    <div class="col-sm-12">
                    <asp:Button ID="btnAdd" runat="server" Text="Add New" OnClientClick="SetSource(this.id)" CssClass="btn btn-primary" />
                        </div>
                    </div>
                <div class="row" id="ByDate" runat="server" visible="false">
                    <asp:PlaceHolder ID="ph2" runat="server" />
                    <div class="col-sm-12">
                    <asp:Button ID="btnAddbyDate" runat="server" Text="Add New" OnClientClick="SetSource(this.id)" CssClass="btn btn-primary" />
                        </div>
                    </div>
                 <div class="demo">
                    <asp:Literal ID="ltlValues" runat="server" />
                   <%--<asp:Button ID="btnDisplayValues" runat="server" Text="Display Values" OnClick="btnDisplayValues_Click" />--%>
                </div>
            </ContentTemplate>
        </asp:UpdatePanel>
        <asp:Literal ID="ltlCount" runat="server" Text="0" Visible="false" />
        <asp:Literal ID="ltlRemoved" runat="server" Visible="false" />
            </div>
       
    </div>
        <div class="modal-footer">
            <asp:Button ID="btnSave" runat="server" Text="  Save  " CssClass="btn btn-primary" OnClientClick="VersionConfirm()" OnClick="btnSave_Click" />
                </div>
        </form>
</asp:Content>
