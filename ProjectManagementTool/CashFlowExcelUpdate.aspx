<%@ Page Title="" UnobtrusiveValidationMode="None" Language="C#" MasterPageFile="~/_master_pages/default.Master" AutoEventWireup="true" CodeBehind="CashFlowExcelUpdate.aspx.cs" Inherits="ProjectManagementTool.CashFlowExcelUpdate" %>
<asp:Content ID="Content1" ContentPlaceHolderID="default_master_head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="default_master_body" runat="server">
     Select File <asp:FileUpload ID="FileUpload1" runat="server" />
            <br />
        <asp:RegularExpressionValidator ID="RegularExpressionValidator1" runat="server" 
            ControlToValidate="FileUpload1" ErrorMessage="Upload Excel File Only" 
            ValidationExpression="^.*\.xls[xm]?$" ValidationGroup="reg" Font-Size="Medium" 
                style="color: #FF0000"></asp:RegularExpressionValidator>
    <br />
    <br />
    <asp:Button runat="server" Text="Submit CP-10" ID="btnSubmit" OnClick="btnSubmit_Click"></asp:Button>
      <asp:Button ID="btnSubmit09" runat="server" Text="Submit CP-09" OnClick="btnSubmit09_Click" />
      <asp:Button ID="btnSubmit07" runat="server" Text="Submit CP-07" OnClick="btnSubmit07_Click"  />
    <br />
    <asp:GridView ID="grdExcelData" runat="server">
        </asp:GridView>
</asp:Content>
