<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="TaskScheduleUserControl.ascx.cs" Inherits="ProjectManagementTool._modal_pages.TaskScheduleUserControl" %>
<link rel="Stylesheet" type="text/css" href="../_assets/styles/app.min.css" />
<table class="table table-borderless">
    <tr>
        <td>
            <asp:DropDownList ID="DDLMonth" runat="server" CssClass="form-control">
                <asp:ListItem>--Select Month--</asp:ListItem>
                <asp:ListItem Value="01">Jan</asp:ListItem>
                <asp:ListItem Value="02">Feb</asp:ListItem>
                <asp:ListItem Value="03">Mar</asp:ListItem>
                <asp:ListItem Value="04">Apr</asp:ListItem>
                <asp:ListItem Value="05">May</asp:ListItem>
                <asp:ListItem Value="06">Jun</asp:ListItem>
                <asp:ListItem Value="07">Jul</asp:ListItem>
                <asp:ListItem Value="08">Aug</asp:ListItem>
                <asp:ListItem Value="09">Sep</asp:ListItem>
                <asp:ListItem Value="10">Oct</asp:ListItem>
                <asp:ListItem Value="11">Nov</asp:ListItem>
                <asp:ListItem Value="12">Dec</asp:ListItem>
            </asp:DropDownList>
        </td>
        <td>
            <asp:DropDownList ID="DDLYear" runat="server" CssClass="form-control">
               <%-- <asp:ListItem>--Select Year--</asp:ListItem>
                <asp:ListItem Value="2019">2019</asp:ListItem>
                <asp:ListItem Value="2020">2020</asp:ListItem>
                <asp:ListItem Value="2021">2021</asp:ListItem>
                <asp:ListItem Value="2021">2022</asp:ListItem>
                <asp:ListItem Value="2021">2023</asp:ListItem>--%>
            </asp:DropDownList>
        </td>
        
        <td>
            <asp:TextBox ID="txtTraget" runat="server" CssClass="form-control" placeholder="Traget Value"></asp:TextBox>
        </td>
        <td>
            <asp:Button ID="btnRemove" runat="server" CssClass="btn btn-primary" OnClick="btnRemove_Click" Text="Remove" />
        </td>
    </tr>
</table>

<hr />