<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="TaskScheduleUserControlDatewise.ascx.cs" Inherits="ProjectManagementTool._modal_pages.TaskScheduleUserControlDatewise" %>
<style type="text/css">
        .TheDateTimePicker{display:block;width:100%;height:calc(1.5em + .75rem + 2px);padding:.375rem .75rem;font-size:1rem;font-weight:400;line-height:1.5;color:#495057;background-color:#fff;background-clip:padding-box;border:1px solid #ced4da;border-radius:.25rem;transition:border-color .15s ease-in-out,box-shadow .15s ease-in-out;}
    </style>
<link rel="Stylesheet" type="text/css" href="../_assets/styles/app.min.css" />
<table class="table table-borderless">
    <tr>
        <td>
            <asp:TextBox ID="dtStartDate" runat="server" CssClass="TheDateTimePicker" placeholder="Start Date"></asp:TextBox>
        </td>
        <td>
           <asp:TextBox ID="dtEndDate" runat="server" CssClass="TheDateTimePicker" placeholder="End Date"></asp:TextBox>
        </td>
        
        <td>
            <asp:TextBox ID="txtTarget" runat="server" CssClass="form-control" placeholder="Traget Value"></asp:TextBox>
        </td>
        <td>
            <asp:Button ID="btnRemove" runat="server" CssClass="btn btn-primary" OnClick="btnRemove_Click" Text="Remove" />
        </td>
    </tr>
</table>

<hr />