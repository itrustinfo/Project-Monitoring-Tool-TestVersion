<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="UserControl.ascx.cs" Inherits="ProjectManagementTool.UserControl" %>
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

<table>
    <tr>
        <td>
            Enter State
        </td>
        <td>
            State ID
        </td>
        <td>
        </td>
    </tr>
    <tr>
        <td>
            <asp:TextBox ID="txtState" runat="server" />
        </td>
        <td>
            <asp:TextBox ID="txtStateId" runat="server" />
        </td>
        <td>
            <asp:Button ID="btnRemove" runat="server" OnClick="btnRemove_Click" Text="Remove" />
        </td>
    </tr>
</table>
