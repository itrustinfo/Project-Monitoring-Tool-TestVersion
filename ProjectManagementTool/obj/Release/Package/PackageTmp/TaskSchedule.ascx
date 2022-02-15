<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="TaskSchedule.ascx.cs" Inherits="ProjectManagementTool.UserControls.TaskSchedule" %>

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
