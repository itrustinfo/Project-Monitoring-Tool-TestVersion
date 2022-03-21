<%@ Page Language="C#" MasterPageFile="~/_master_pages/modal.Master" AutoEventWireup="true" CodeBehind="upload-rabill-document.aspx.cs" Inherits="ProjectManagementTool._modal_pages.upload_rabill_document" %>

<asp:Content ID="Content1" ContentPlaceHolderID="modal_master_head" runat="server">
    <style type="text/css">
        .HideItem {
            display: none;
        }
    </style>
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
    <form id="frmUploadSitePhotograph" runat="server">
        <div class="container-fluid" style="max-height: 85vh; overflow-y: auto; min-height: 80vh;">
            <div class="row">
                <div class="col-sm-4">
                    <div class="form-group">
                        <label class="lblCss" for="UploadPhotographs">Choose File/s</label>
                        &nbsp;<span style="color: red; font-size: 1.2rem;">*</span>
                        <div class="custom-file">
                            <asp:FileUpload ID="ImageUpload" runat="server" AllowMultiple="true" CssClass="custom-file-input" />
                            <label class="custom-file-label" for="UploadPhotographs">Choose Files</label>
                        </div>
                    </div>
                </div>
                <div class="col-sm-4">
                    <label class="lblCss" for="UploadPhotographs">&nbsp;</label><br />
                    <asp:Button ID="btnUpload" runat="server" Text="Upload" CssClass="btn btn-primary" OnClick="btnUpload_Click" />
                </div>
                <div class="col-sm-4">
                    <div class="form-group">
                        <%--<label class="lblCss" for="txtoptionname">Description</label>--%>
                        <asp:TextBox ID="txtInvoiceNumber" CssClass="form-control" placeholder="Description" runat="server" ClientIDMode="Static" style="display:none"></asp:TextBox>
                    </div>
                </div>
                
            </div>
            <div class="row">
                <div class="col-sm-12">
                    <div class="table-responsive">
                        <asp:GridView ID="GrdTreeView" runat="server" AutoGenerateColumns="False" PageSize="20" OnRowDeleting="GrdTreeView_RowDeleting"
                            AllowPaging="True" CssClass="table table-bordered" EmptyDataText="No Data Found." DataKeyNames="RABillUid" 
                            Width="100%">
                            <Columns>
                                <asp:TemplateField HeaderText="File Name">
                                    <ItemTemplate>
                                        <%#Eval("FileName")%>
                                        <asp:HiddenField ID="documentDeleteuid" runat="server" Value='<%#Eval("DocumentUID")%>' />
                                    </ItemTemplate>
                                </asp:TemplateField>

                                <asp:TemplateField>
                                    <HeaderTemplate>
                                        Description
                                    </HeaderTemplate>
                                    <ItemTemplate>
                                        <%#Eval("Description")%>
                                    </ItemTemplate>
                                </asp:TemplateField>

                                <asp:TemplateField>
                                    <HeaderTemplate>
                                        Uploaded Date
                                    </HeaderTemplate>
                                    <ItemTemplate>
                                        <%#Eval("created_date")%>
                                    </ItemTemplate>

                                </asp:TemplateField>


                                <asp:TemplateField>
                                    <ItemTemplate>
                                        <a id="FileDownload" CommandArgument="MyVal1" href=<%#Eval("FilePath")%> onserverclick="Download_Click" runat="server">
                                            <span title="Download" class="fas fa-download "></a>&nbsp;&nbsp;&nbsp;
                                        <asp:LinkButton ID="lnkdelete" runat="server" OnClientClick="return DeleteItem()" CausesValidation="false" CommandName="delete"><span title="Delete" class="fas fa-trash text-danger"></span></asp:LinkButton>
                                                        

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
    </form>
</asp:Content>

