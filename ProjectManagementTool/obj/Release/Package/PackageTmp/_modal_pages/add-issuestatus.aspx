<%@ Page Title="" Language="C#" MasterPageFile="~/_master_pages/modal.Master" AutoEventWireup="true" CodeBehind="add-issuestatus.aspx.cs" Inherits="ProjectManagementTool._modal_pages.add_issuestatus" %>
<asp:Content ID="Content1" ContentPlaceHolderID="modal_master_head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="modal_master_body" runat="server">
    <form id="FrmIssueStatus" runat="server">
        <asp:ScriptManager ID="ScriptManager1" runat="server"></asp:ScriptManager>
        <div class="container-fluid" style="min-height:75vh; overflow-y:auto;">
            <asp:HiddenField ID="HiddenActivity" runat="server" />
            <div class="row">
                <div class="col-sm-12">
                    <div class="form-group">
                        <label class="lblCss" for="DDLWorkPackage">Status</label> &nbsp;<span style="color:red; font-size:1.1rem;">*</span>
                         <asp:DropDownList ID="DDLStatus" CssClass="form-control" runat="server" required>
                         <asp:ListItem Value="">-- Select --</asp:ListItem>
                         <asp:ListItem Value="In-Progress">In-Progress</asp:ListItem>
                         <asp:ListItem Value="Close">Close</asp:ListItem>
                         <asp:ListItem Value="Rejected">Rejected</asp:ListItem>
                         </asp:DropDownList>
                    </div>
                    
                   
                    <div class="form-group">
                        <label class="lblCss" for="txtremarks">Remarks</label> &nbsp;<span style="color:red; font-size:1.1rem;">*</span>
                       <asp:TextBox ID="txtremarks" CssClass="form-control" runat="server" autocomplete="off" required TextMode="MultiLine" ClientIDMode="Static"></asp:TextBox>
                    </div>

                    <div class="form-group">
                        <label class="lblCss" for="FileUploadDoc">Issue Staus Document</label>
                        <div class="custom-file">
                            <asp:FileUpload ID="FileUploadDoc" runat="server" CssClass="custom-file-input" AllowMultiple="true"/>
                            <label class="custom-file-label" for="FilDocument">Choose document</label>
                        </div>
                    </div>
                </div>
            </div> 
        </div>

        <div class="modal-footer">
            <asp:Button ID="btnSubmit" runat="server" Text="Submit" CssClass="btn btn-primary" OnClick="btnSubmit_Click"/>
                </div>
    </form>
</asp:Content>
