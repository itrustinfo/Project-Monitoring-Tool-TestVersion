<%@ Page Title="" Language="C#" MasterPageFile="~/_master_pages/modal.Master" AutoEventWireup="true" CodeBehind="view-documentdetails.aspx.cs" Inherits="ProjectManagementTool._modal_pages.view_documentdetails" %>
<asp:Content ID="Content1" ContentPlaceHolderID="modal_master_head" runat="server">
    <style type="text/css">
        .label {
  min-width: 200px !important;
  display: inline-block !important
}
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="modal_master_body" runat="server">
     <form id="frmViewDocumentModal" runat="server">
        <div class="container-fluid" style="max-height:85vh; overflow-y:auto;">
            <div class="row">
                <div class="col-sm-6">
                    <div class="form-group">
                        <label class="lblCss" for="txtDocumentName">Document Name</label>
                         <%--<asp:Label ID="lblDocumentName" runat="server" Text="Label" CssClass="form-control"></asp:Label>--%>
                        <asp:TextBox ID="txtDocumentName" runat="server" CssClass="form-control" BackColor="White" Enabled="false" TextMode="MultiLine"></asp:TextBox>
                    </div>
                    <div class="form-group">
                        <label class="lblCss" for="txtdesc">Subject / Description</label> 
                        <%--<asp:Label ID="lblDescription" runat="server" Text="Label"  CssClass="form-control"></asp:Label>--%>
                        <asp:TextBox ID="txtDescription" runat="server" CssClass="form-control" BackColor="White" Enabled="false" TextMode="MultiLine"></asp:TextBox>
                    </div>
                    <div class="form-group">
                        <label class="lblCss" for="txtprefnumber">Reference Number</label>
                        <asp:Label ID="lblReferenceNo" runat="server" Text="Label" CssClass="form-control"></asp:Label>
                    </div>
                    <div class="form-group">
                        <label class="lblCss" for="ChkMediaTypeHardCopy">Document Media</label>
                        <div class="form-check">
                           <asp:CheckBoxList ID="DDLDocumentMedia" width="100%" CssClass="lblCss" runat="server" RepeatColumns="2" RepeatDirection="Horizontal" Enabled="false">
                               <asp:ListItem Value="Hard Copy">&nbsp;&nbsp;Hard Copy</asp:ListItem>
                               <asp:ListItem Value="Soft Copy(PDF)">&nbsp;&nbsp;Soft Copy(PDF)</asp:ListItem>
                               <asp:ListItem Value="Soft Copy Editable Format">&nbsp;&nbsp;Soft Copy Editable Format</asp:ListItem>
                               <asp:ListItem Value="Soft Copy Ref.">&nbsp;&nbsp;Soft Copy Ref.</asp:ListItem>
                               <asp:ListItem Value="Hard Copy Ref.">&nbsp;&nbsp;Hard Copy Ref.</asp:ListItem>
                               <asp:ListItem Value="No Media">&nbsp;&nbsp;No Media</asp:ListItem>
                           </asp:CheckBoxList>
                        </div>
                    </div>
                </div>
                <div class="col-sm-6">
                    <div class="form-group" id="Originator" runat="server">
                        <label class="lblCss" for="DDLOriginator">Originator</label>
                          <asp:Label ID="lblOriginator" runat="server" Text="Label" CssClass="form-control"></asp:Label>
                    </div>
                    <div class="form-group" id="OriginatorNumber" runat="server">
                        <label class="lblCss" for="txtRefNumber">Originator Ref.Number</label> 
                       <asp:Label ID="lblOrgRefNo" runat="server" Text="Label" CssClass="form-control"></asp:Label>
                    </div>

                    <div class="form-group">
                        <label class="lblCss" for="DatIncomingRevisionDate"><asp:Label ID="LblDateText" runat="server"></asp:Label></label> 
                         <asp:Label ID="lblInDate" runat="server" Text="Label" CssClass="form-control"></asp:Label>
                    </div>
                    <div class="form-group" id="DocumentDate" runat="server">
                        <label class="lblCss" for="dtDocumentDate">Cover Letter Date</label> 
                        <asp:Label ID="lblCoverDate" runat="server" Text="Label" CssClass="form-control"></asp:Label>
                    </div>
                    
                     <div class="form-group">
                        <label class="lblCss" for="txtFileRefNumber">File Reference Number</label>
                        <asp:Label ID="lblfileRefNo" runat="server" Text="Label" CssClass="form-control"></asp:Label>
                    </div>
                   

                     <div class="form-group">
                        <label class="lblCss" for="txtremarks">Remarks</label>
                         <asp:TextBox ID="txtRemarks" runat="server" CssClass="form-control" BackColor="White" Enabled="false" TextMode="MultiLine"></asp:TextBox>
                        <%--<asp:Label ID="lblRemarks" runat="server" Text="Label" CssClass="form-control"></asp:Label>--%>
                    </div>
                </div>
            </div> 
        </div>
    
    </form>
</asp:Content>
