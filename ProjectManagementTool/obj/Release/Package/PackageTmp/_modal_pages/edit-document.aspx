<%@ Page Title="" Language="C#" MasterPageFile="~/_master_pages/modal.Master" AutoEventWireup="true" CodeBehind="edit-document.aspx.cs" Inherits="ProjectManagementTool._modal_pages.edit_document" %>
<asp:Content ID="Content1" ContentPlaceHolderID="modal_master_head" runat="server">
    <script type="text/javascript">
        function CheckDate() {
            var IncomingReceDate = document.getElementById('<%=dtIncomingRevDate.ClientID%>').value;
            var DocumentDate = document.getElementById('<%=dtDocumentDate.ClientID%>').value;
            if (IncomingReceDate != "" && DocumentDate !="") {
                IncomingReceDate = IncomingReceDate.split('/');
                
                var iDate = new Date(IncomingReceDate[2] + '-' + IncomingReceDate[1] + '-' + IncomingReceDate[0]);
                DocumentDate = DocumentDate.split('/');
                
                var dDate = new Date(DocumentDate[2] + '-' + DocumentDate[1] + '-' + DocumentDate[0]);
                if (dDate > iDate) {
                    alert("Document Date should be less than Incoming Received Date.");
                    document.getElementById('<%=dtDocumentDate.ClientID%>').value = '';
                }
            }
        }
        </script>
      <script type="text/javascript">
 $( function() {
    $("input[id$='dtIncomingRevDate']").datepicker({
      changeMonth: true,
        changeYear: true,
      dateFormat:'dd/mm/yy'
      });

      $("input[id$='dtDocumentDate']").datepicker({
      changeMonth: true,
        changeYear: true,
      dateFormat:'dd/mm/yy'
      });
    });
</script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="modal_master_body" runat="server">
    <form id="frmAddDocumentModal" runat="server">
        <asp:HiddenField ID="HiddenField1" runat="server" />
        <asp:HiddenField ID="HiddenProjectUID" runat="server" />
        <div class="container-fluid" style="max-height:85vh; overflow-y:auto;">
            <div class="row">
                <div class="col-sm-6">
                    <div class="form-group" style="display:none;">
                        <label class="lblCss" for="DDlProject">Project</label>
                         <asp:DropDownList ID="DDlProject" Enabled="false" CssClass="form-control" runat="server"></asp:DropDownList>
                    </div>
                    <div class="form-group" style="display:none;">
                        <label class="lblCss" for="DDLWorkPackage">Work Package</label>
                         <asp:DropDownList ID="DDLWorkPackage" Enabled="false" CssClass="form-control" runat="server"></asp:DropDownList>
                    </div>

                    <div class="form-group">
                        <label class="lblCss" for="txtDocumentName">Document Name</label>
                         <asp:TextBox ID="txtDocumentName" runat="server" TextMode="MultiLine" CssClass="form-control" Enabled="false"></asp:TextBox>
                    </div>
                    <div class="form-group">
                        <label class="lblCss" for="txtdesc">Subject / Description</label> &nbsp;<span style="color:red; font-size:1.1rem;">*</span>
                        <asp:TextBox ID="txtdesc" CssClass="form-control" TextMode="MultiLine" runat="server" required ClientIDMode="Static"></asp:TextBox>
                        
                    </div>
                    <div class="form-group" id="PrjRefNumber" runat="server">
                        <label class="lblCss" for="txtprefnumber">Reference Number</label>
                        <asp:TextBox ID="txtprefnumber" CssClass="form-control" Enabled="false" runat="server" ClientIDMode="Static"></asp:TextBox>
                    </div>
                    <div class="form-group" id="DocumentMedia" runat="server">
                        <label class="lblCss" for="ChkMediaTypeHardCopy">Document Media</label>
                        <div class="form-check">
                           <asp:CheckBoxList ID="DDLDocumentMedia" width="100%" CssClass="lblCss" runat="server" RepeatColumns="2" RepeatDirection="Horizontal">
                               <asp:ListItem Value="Hard Copy">&nbsp;&nbsp;Hard Copy</asp:ListItem>
                               <asp:ListItem Value="Soft Copy(PDF)">&nbsp;&nbsp;Soft Copy(PDF)</asp:ListItem>
                               <asp:ListItem Value="Soft Copy Editable Format">&nbsp;&nbsp;Soft Copy Editable Format</asp:ListItem>
                               <asp:ListItem Value="Soft Copy Ref.">&nbsp;&nbsp;Soft Copy Ref.</asp:ListItem>
                               <asp:ListItem Value="Hard Copy Ref.">&nbsp;&nbsp;Hard Copy Ref.</asp:ListItem>
                               <asp:ListItem Value="No Media">&nbsp;&nbsp;No Media</asp:ListItem>
                           </asp:CheckBoxList>
                        </div>
                    </div>
                      <div class="form-group" id="FileRefNumber" runat="server">
                        <label class="lblCss" for="txtFileRefNumber">File Reference Number</label>
                        <asp:TextBox ID="txtFileRefNumber" CssClass="form-control" runat="server" autocomplete="off" ClientIDMode="Static"></asp:TextBox>
                    </div>
                </div>
                <div class="col-sm-6">
                    <div class="form-group" id="Originator" runat="server">
                        <label class="lblCss" for="DDLOriginator">Originator</label>
                          <asp:RadioButtonList ID="RBLOriginator" runat="server" Width="100%" CssClass="lblCss" CellPadding="4" RepeatDirection="Horizontal" AutoPostBack="true" OnSelectedIndexChanged="RBLOriginator_SelectedIndexChanged" RepeatColumns="3">
                                 <%--<asp:ListItem Selected="True" Value="Contractor">&nbsp;&nbsp;Contractor</asp:ListItem>
                                 <asp:ListItem Value="JUIDCo">&nbsp;&nbsp;JUIDCo</asp:ListItem>
                                 <asp:ListItem Value="NJSEI">&nbsp;&nbsp;NJSEI</asp:ListItem>
                              <asp:ListItem Value="Others">&nbsp;&nbsp;Others</asp:ListItem>--%>
                             </asp:RadioButtonList>
                    </div>
                    <div class="form-group" id="OriginatorNumber" runat="server">
                        <label class="lblCss" for="txtRefNumber">Originator Ref.Number</label> &nbsp;<span style="color:red; font-size:1.1rem;">*</span>
                        <asp:TextBox ID="txtRefNumber" CssClass="form-control" runat="server" autocomplete="off" required ClientIDMode="Static"></asp:TextBox>
                        
                    </div>

                    <div class="form-group" id="IncomingReceDate" runat="server">
                        <label class="lblCss" for="DatIncomingRevisionDate"><asp:Label ID="LblDateText" runat="server"></asp:Label></label> &nbsp;<span style="color:red; font-size:1.2rem;">*</span>
                        <asp:TextBox ID="dtIncomingRevDate" CssClass="form-control" required runat="server" placeholder="dd/mm/yyyy" autocomplete="off" onchange="CheckDate()" ClientIDMode="Static"></asp:TextBox>
                    </div>
                    <div class="form-group" id="DocumentDate" runat="server">
                        <label class="lblCss" for="dtDocumentDate">Document Date</label> &nbsp;<span style="color:red; font-size:1.1rem;">*</span>
                        <asp:TextBox ID="dtDocumentDate" CssClass="form-control" required runat="server" placeholder="dd/mm/yyyy" autocomplete="off" onchange="CheckDate()" ClientIDMode="Static"></asp:TextBox>
                    </div>
                    
                    <div class="form-group" id="CoverLetterUpload" runat="server">
                        <label class="lblCss" for="FilCoverLetter"><asp:Label ID="LblDocType" Text="Cover Letter" runat="server"></asp:Label></label>
                        <div class="custom-file">
                            <asp:FileUpload ID="FileUploadCoverPage" runat="server" CssClass="custom-file-input" />

                            <%--<asp:FileUpload ID="FilCoverLetter" CssClass="custom-file-input" runat="server" ClientIDMode="Static" /> --%>
                            <label class="custom-file-label" for="FilCoverLetter">Choose covering letter</label>
                        </div>
                    </div>

                   
                   
                     

                     <div class="form-group">
                        <label class="lblCss" for="txtremarks">Remarks</label>
                        <asp:TextBox ID="txtremarks" CssClass="form-control" TextMode="MultiLine" runat="server" ClientIDMode="Static"></asp:TextBox>
                    </div>
                </div>
            </div> 
        </div>
        <div class="modal-footer">
                    <%--<button type="button" class="btn btn-secondary" data-dismiss="modal">Close</button>--%>
                    <%--<button type="button" class="btn btn-primary">Save changes</button>--%>
            <asp:Button ID="btnSubmit" runat="server" Text="Submit" CssClass="btn btn-primary" OnClick="btnSubmit_Click" />
                </div>
    </form>
</asp:Content>
