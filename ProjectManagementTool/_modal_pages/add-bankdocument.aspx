<%@ Page Title="" Language="C#" MasterPageFile="~/_master_pages/modal.Master" AutoEventWireup="true" CodeBehind="add-bankdocument.aspx.cs" Inherits="ProjectManagementTool._modal_pages.add_bankdocument" %>
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
                <div class="col-sm-6">
                    <div class="form-group">
                        <label class="lblCss" for="txtcomments">Document Name</label>
                        <asp:TextBox ID="txtdocumentName" CssClass="form-control" required runat="server" ClientIDMode="Static"></asp:TextBox>
                    </div>
                    <div class="form-group">
                        <label class="lblCss" for="FilCoverLetter">Choose Document</label>
                        <div class="custom-file">
                            <asp:FileUpload ID="FileUpload1" runat="server" required CssClass="custom-file-input" />

                            <%--<asp:FileUpload ID="FilCoverLetter" CssClass="custom-file-input" runat="server" ClientIDMode="Static" /> --%>
                            <label class="custom-file-label" for="FilCoverLetter">Choose document</label>
                        </div>
                    </div>
                </div> 
                <div class="col-sm-6">
                    <h6 class="text-muted">
                                   <asp:Label ID="ActivityHeading" CssClass="text-uppercase font-weight-bold" runat="server" Text="List of Bank Documents" />
                                       <%--<asp:Label Text="Foo bar" CssClass="pl-1" runat="server" />--%>
                               </h6>
                    <asp:GridView ID="grdBankDocuments" runat="server" AutoGenerateColumns="False" CssClass="table table-bordered" OnRowCommand="grdBankDocuments_RowCommand" OnRowDeleting="grdBankDocuments_RowDeleting" Width="100%">
                <Columns>
                    <asp:BoundField DataField="Document_Name" HeaderText="Document Name" >
                    <HeaderStyle HorizontalAlign="Left" />
                    </asp:BoundField>
                    <asp:BoundField DataField="Document_Type" HeaderText="Document Type" >
                    <HeaderStyle HorizontalAlign="Left" />
                    </asp:BoundField>
                    <asp:TemplateField>
                    <ItemTemplate>
                         <asp:LinkButton ID="lnkdownload" runat="server" CommandName="download" CommandArgument='<%#Eval("BankDoc_UID")%>'>Download</asp:LinkButton>
                    </ItemTemplate>
                </asp:TemplateField>
                    <asp:TemplateField>
                           <ItemTemplate>
                                <asp:LinkButton ID="lnkdelete" OnClientClick="return DeleteItem()" runat="server" CausesValidation="false" CommandArgument='<%#Eval("BankDoc_UID")%>' CommandName="delete"><span title="Delete" class="fas fa-trash"></span></asp:LinkButton>
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
        <div class="modal-footer">
            <asp:Button ID="btnSubmit" runat="server" Text="Submit" CssClass="btn btn-primary" OnClick="btnSubmit_Click" />
                </div>
    </form>
</asp:Content>
