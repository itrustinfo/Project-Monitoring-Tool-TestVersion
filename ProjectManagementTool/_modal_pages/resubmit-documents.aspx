<%@ Page Title="" Language="C#" MasterPageFile="~/_master_pages/modal.Master" AutoEventWireup="true" CodeBehind="resubmit-documents.aspx.cs" Inherits="ProjectManagementTool._modal_pages.resubmit_documents" %>
<asp:Content ID="Content1" ContentPlaceHolderID="modal_master_head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="modal_master_body" runat="server">
    <form id="frmAddDocumentModal" runat="server">
        <asp:ScriptManager ID="ScriptManager1" runat="server"></asp:ScriptManager>
        <asp:HiddenField ID="Hidden1" runat="server" />
        <div class="container-fluid" style="overflow-y:auto;">
            <div class="row">
                <div class="col-sm-12">
                    <div class="form-group">
                        <label class="lblCss" for="RBLUploadType">Upload Type</label> 
                         <asp:RadioButtonList ID="RBLUploadType" runat="server" Width="100%" CssClass="lblCss" CellPadding="5" RepeatDirection="Horizontal" AutoPostBack="true" OnSelectedIndexChanged="RBLUploadType_SelectedIndexChanged">
                                 <asp:ListItem Value="File" Selected="True">&nbsp;&nbsp;File</asp:ListItem>
                                 <%--<asp:ListItem Value="Folder">&nbsp;&nbsp;Folder</asp:ListItem>--%>
                             </asp:RadioButtonList>
                    </div>
                    <div class="form-group" id="CoverLetter" runat="server">
                        <label class="lblCss" for="FilCoverLetter">Choose Cover Letter</label> 
                        <div class="custom-file">
                            <asp:FileUpload ID="FileCoverLetterUpload" runat="server" CssClass="custom-file-input" />

                            <%--<asp:FileUpload ID="FilCoverLetter" CssClass="custom-file-input" runat="server" ClientIDMode="Static" /> --%>
                            <label class="custom-file-label" for="FilCoverLetter">Choose document</label>
                        </div>
                    </div>
                    <div class="form-group" id="UploadFile" runat="server">
                        <label class="lblCss" for="FilCoverLetter">Choose File</label> &nbsp;<span style="color:red; font-size:1.1rem;">*</span>
                        <div class="custom-file">
                            <asp:FileUpload ID="FileUploadDoc" runat="server" required CssClass="custom-file-input" />

                            <%--<asp:FileUpload ID="FilCoverLetter" CssClass="custom-file-input" runat="server" ClientIDMode="Static" /> --%>
                            <label class="custom-file-label" for="FilCoverLetter">Choose document</label>
                        </div>
                    </div>
                    <div class="form-group" id="UploadFolder" runat="server">
                        <label class="lblCss" for="FilDocument">Choose Folder</label>
                        <div class="custom-file">
                            <input type="file" runat="server" id="FolderUpload1" CssClass="custom-file-input" name="FilDocument" webkitdirectory mozdirectory msdirectory odirectory directory multiple />
                        </div>
                    </div>
                    <div class="form-group">
                        <label class="lblCss" for="txtcomments">Comments</label> 
                        <asp:TextBox ID="txtcomments" CssClass="form-control" TextMode="MultiLine" runat="server" ClientIDMode="Static"></asp:TextBox>
                    </div>
              
            </div> 
        </div>
            </div>
        <div class="modal-footer">
            <asp:Button ID="btnSubmit" runat="server" Text="Submit" CssClass="btn btn-primary" OnClick="btnSubmit_Click" />
                </div>
    </form>
</asp:Content>
