<%@ Page Title="" Language="C#" MasterPageFile="~/_master_pages/modal.Master" AutoEventWireup="true" CodeBehind="view-financemilestonehistory.aspx.cs" Inherits="ProjectManagementTool._modal_pages.view_financemilestonehistory" %>
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
    <form id="frmAddDocumentModal" runat="server">
        <asp:ScriptManager ID="ScriptManager1" runat="server"></asp:ScriptManager>
        <asp:HiddenField ID="Hidden1" runat="server" />
        <div class="container-fluid" style="min-height:85vh; overflow-y:auto; max-height:85vh;">
            <div class="row"> 
                <div class="col-sm-12">

                    <asp:GridView ID="GrdPaymentHistory" runat="server" AllowPaging="true" PageSize="10" AutoGenerateColumns="False" CssClass="table table-bordered" Width="100%" OnPageIndexChanging="GrdPaymentHistory_PageIndexChanging" OnRowCommand="GrdPaymentHistory_RowCommand" OnRowDeleting="GrdPaymentHistory_RowDeleting">
                <Columns>
                    <asp:BoundField DataField="Actual_PaymentDate" HeaderText="Payment Date" DataFormatString="{0: dd MMM yyyy}" >
                    <HeaderStyle HorizontalAlign="Left" />
                    </asp:BoundField>
                    <asp:TemplateField HeaderText="Actual Payment">
                        <ItemTemplate>
                            <span style="color:#006699;"><%#Eval("Currency")%></span>&nbsp;<%#string.Format(new System.Globalization.CultureInfo(Eval("Currency_CultureInfo").ToString()),"{0:N}", Eval("Actual_Payment"))%>
                        </ItemTemplate>
                    </asp:TemplateField>
                      <asp:TemplateField>
                            <ItemTemplate>
                                      <asp:LinkButton ID="lnkdelete" runat="server" CausesValidation="false" OnClientClick="return DeleteItem()" CommandArgument='<%#Eval("FinanceMileStoneUpdate_UID")%>' CommandName="delete"><span title="Delete" class="fas fa-trash"></span></asp:LinkButton>
                            </ItemTemplate>
                     </asp:TemplateField>          
                </Columns>
                <EmptyDataTemplate>
                    <strong>No Payment Found ! </strong>
                </EmptyDataTemplate>
            </asp:GridView>
                    </div>
        </div>
            </div>
        <div class="modal-footer">
            
                </div>
    </form>

</asp:Content>
