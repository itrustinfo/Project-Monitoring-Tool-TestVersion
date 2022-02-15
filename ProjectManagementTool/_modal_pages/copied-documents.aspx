<%@ Page Title="" Language="C#" MasterPageFile="~/_master_pages/modal.Master" AutoEventWireup="true" CodeBehind="copied-documents.aspx.cs" Inherits="ProjectManagementTool._modal_pages.copied_documents" %>
<asp:Content ID="Content1" ContentPlaceHolderID="modal_master_head" runat="server">
    <%--<script type="text/javascript">
        function HideCopyFiles() {
            if (window.opener != null && !window.opener.closed) {
            alert('Hi');
            var anchorFiles = window.opener.document.getElementById("CopyDocument");
            
            anchorFiles.style.display = 'none';
        }
        window.close();
    }
</script>--%>

</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="modal_master_body" runat="server">
      <form id="frmAddIssuesModal" runat="server">
        <div class="container-fluid" style="max-height:80vh; overflow-y:auto;">
            <div class="row">
                <div class="col-sm-12">
                    <div class="table-responsive">
                         <asp:GridView ID="GrdCopiedDocumentList" EmptyDataText="No Documents Found." AutoGenerateColumns="false" runat="server" Width="100%" CellPadding="6" CellSpacing="16" 
                                            AllowPaging="true" PageSize="15" CssClass="table table-bordered" OnPageIndexChanging="GrdCopiedDocumentList_PageIndexChanging" OnRowCommand="GrdCopiedDocumentList_RowCommand" OnRowDeleting="GrdCopiedDocumentList_RowDeleting">
                                       <Columns>
                                           <asp:TemplateField HeaderText="Submittal Name">
                                               <ItemTemplate>
                                                   <%#GetSubmittalName(Eval("DocumentUID").ToString())%>
                                               </ItemTemplate>
                                           </asp:TemplateField>
                                           <asp:TemplateField HeaderText="Document Name">
                                               <ItemTemplate>
                                                   <%#GetDocumentName(Eval("DocumentUID").ToString())%>
                                               </ItemTemplate>
                                           </asp:TemplateField>
                                           <asp:TemplateField>
                                               <ItemTemplate>
                                                   <asp:LinkButton ID="lnkDelete" runat="server" CausesValidation="false" CommandArgument='<%#Eval("DocumentUID")%>' CommandName="delete"><span title="Delete" class="fas fa-trash"></span></asp:LinkButton>
                                               </ItemTemplate>
                                           </asp:TemplateField>
                                       </Columns>
                                       </asp:GridView>
                            </div>
                    </div>
                </div>
            </div>
        </form>
</asp:Content>
