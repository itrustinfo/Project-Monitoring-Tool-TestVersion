<%@ Page Title="" Language="C#" MasterPageFile="~/_master_pages/modal.Master" AutoEventWireup="true" CodeBehind="add-taskschedule.aspx.cs" Inherits="ProjectManagementTool._modal_pages.add_taskschedule" %>
<%@ Register Src="~/_modal_pages/UserControl.ascx" TagName="UserControl" TagPrefix="uc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="modal_master_head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="modal_master_body" runat="server">
    <form id="form1" runat="server">
    <div>
        <asp:ScriptManager ID="sm1" runat="server" />
        <asp:UpdatePanel ID="up1" runat="server">
            <ContentTemplate>
                <div class="demo">
                    <table>
                        <tr>
                            <td>
                                <asp:PlaceHolder ID="ph1" runat="server" />
                                <br />
                                <asp:Button ID="btnAdd" runat="server" Text="Add" />
                            </td>
                        </tr>
                    </table>
                </div>
            </ContentTemplate>
        </asp:UpdatePanel>
        <br />        
        <asp:Literal ID="ltlCount" runat="server" Text="0" Visible="false" />
        <asp:Literal ID="ltlRemoved" runat="server" Visible="false" />
        <br />
                
    </div>
        </form>
</asp:Content>
