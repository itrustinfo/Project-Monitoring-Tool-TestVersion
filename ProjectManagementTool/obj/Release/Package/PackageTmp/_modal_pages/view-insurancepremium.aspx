<%@ Page Title="" Language="C#" MasterPageFile="~/_master_pages/modal.Master" AutoEventWireup="true" CodeBehind="view-insurancepremium.aspx.cs" Inherits="ProjectManagementTool._modal_pages.view_insurancepremium" %>
<asp:Content ID="Content1" ContentPlaceHolderID="modal_master_head" runat="server">
    <script type="text/javascript">
        function DeleteItem() {
            if (confirm("Are you sure you want to delete ...?")) {
                return true;
            }
            return false;
    }
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="modal_master_body" runat="server">
    <form id="FrmAddInsuranceDocuments" runat="server">
        <asp:ScriptManager ID="ScriptManager1" runat="server"></asp:ScriptManager>
        <asp:HiddenField ID="Hidden1" runat="server" />
        <div class="container-fluid" style="min-height:85vh; overflow-y:auto; max-height:85vh;">
            <div class="row"> 
                <div class="col-sm-12">
                    <h6 class="text-muted">
                                   <asp:Label ID="ActivityHeading" CssClass="text-uppercase font-weight-bold" runat="server" Text="List of Paid Insurance Premium's" />
                                       <%--<asp:Label Text="Foo bar" CssClass="pl-1" runat="server" />--%>
                               </h6>
                    <asp:GridView ID="GrdPremium" runat="server" AutoGenerateColumns="False" CssClass="table table-bordered" OnRowCommand="GrdPremium_RowCommand" OnRowDeleting="GrdPremium_RowDeleting" Width="100%">
                <Columns>
                                <asp:TemplateField HeaderText="Amount">
                                        <ItemTemplate>
                                            <span style="color:#006699;"><%#Eval("Currency")%></span>&nbsp;<%#string.Format(new System.Globalization.CultureInfo(Eval("Currency_CultureInfo").ToString()),"{0:N}", Eval("Premium_Paid"))%>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                            <asp:BoundField DataField="Premium_PaidDate" HeaderText="Paid Date"  DataFormatString="{0:dd/MM/yyyy}">
                                    <HeaderStyle HorizontalAlign="Left" />
                                    </asp:BoundField>
                            <asp:BoundField DataField="Premium_DueDate" HeaderText="Due Date"  DataFormatString="{0:dd/MM/yyyy}">
                                    <HeaderStyle HorizontalAlign="Left" />
                                    </asp:BoundField>
                            <asp:BoundField DataField="Remarks" HeaderText="Remarks" >
                                    <HeaderStyle HorizontalAlign="Left" />
                                    </asp:BoundField>
                    <asp:TemplateField>
                    <ItemTemplate>
                         <asp:LinkButton ID="lnkdownload" runat="server" CommandName="download" CommandArgument='<%#Eval("PremiumUID")%>'>Download</asp:LinkButton>
                    </ItemTemplate>
                </asp:TemplateField>
                    <asp:TemplateField>
                           <ItemTemplate>
                                <asp:LinkButton ID="lnkdelete" OnClientClick="return DeleteItem()" runat="server" CausesValidation="false" CommandArgument='<%#Eval("PremiumUID")%>' CommandName="delete"><span title="Delete" class="fas fa-trash"></span></asp:LinkButton>
                            </ItemTemplate>
                   </asp:TemplateField>
                </Columns>
                <EmptyDataTemplate>
                    <strong>No Records Found ! </strong>
                </EmptyDataTemplate>
            </asp:GridView>
                    </div>
        </div>
            </div>
       <%-- <div class="modal-footer">
            
                </div>--%>
    </form>
</asp:Content>
