    <%@ Page Title="" Language="C#" MasterPageFile="~/_master_pages/modal.Master" AutoEventWireup="true" CodeBehind="add-documentstatus.aspx.cs" Inherits="ProjectManagementTool._modal_pages.add_documentstatus" %>
<asp:Content ID="Content1" ContentPlaceHolderID="modal_master_head" runat="server">
    <script type="text/javascript">
        function getValue(obj)
        {
            var Desc = '';
            var text = obj.options[obj.selectedIndex].innerHTML;
            
            if (text == "Code A") {
                Desc = "* Approved, manufacture/construction may commence";
            }
            else if (text == "Code B")
            {
                Desc = "* Acceptable subject to changes indicated.Resubmit for approval but manufacture / construction may commence.";
            }
            else if (text == "Code H")
            {
                Desc = "* Returned without review.";
            }
            else if (text == "Code C")
            {
                Desc = "* Amend as comments indicated and resubmit for approval.";
            }
            else if (text == "Code E")
            {
                Desc = "* Amend as comments indicated resubmit for record.";
            }
            else if (text == "Code G")
            {
                Desc = "* Drawing of this category is for information and hence not required to be approved.";
            }
            else if (text == "Code D")
            {
                Desc = "* Comments noted in Letter/memo attached to forwarding transmittal No.";
            }
            else if (text == "Code F")
            {
                Desc = "* Comments noted in Letter/memo attached to forwarding transmittal No....... dated......... Amend as comments indicated and resubmit for record.";
            }
            document.getElementById("tooltip").innerHTML = Desc;
            //alert(text);
            if (text == "Closed") {
                document.getElementById("divUpdateStatus").style.display = "none";
                document.getElementById("divReviewFile").style.display = "none";
            }
            else {
                  document.getElementById("divUpdateStatus").style.display = "block";
                document.getElementById("divReviewFile").style.display = "block";
            }
        }

        function setValue() {
          
            var value = document.getElementById("<%=DDlStatus.ClientID%>");  
   
   var text = value.options[value.selectedIndex].text;  
  // alert("Text:-" +" "+ text);  
          //  alert(text);
             if (text == "Closed") {
                document.getElementById("divUpdateStatus").style.display = "none";
                document.getElementById("divReviewFile").style.display = "none";
            }
            else {
                  document.getElementById("divUpdateStatus").style.display = "block";
                document.getElementById("divReviewFile").style.display = "block";
            }

        }

        function getValueFromCodeBehind(obj)
        {
            var Desc = '';
            var text = obj;
            
            if (text == "Code A") {
                Desc = "* Approved, manufacture/construction may commence";
            }
            else if (text == "Code B")
            {
                Desc = "* Acceptable subject to changes indicated.Resubmit for approval but manufacture / construction may commence.";
            }
            else if (text == "Code H")
            {
                Desc = "* Returned without review.";
            }
            else if (text == "Code C")
            {
                Desc = "* Amend as comments indicated and resubmit for approval.";
            }
            else if (text == "Code E")
            {
                Desc = "* Amend as comments indicated resubmit for record.";
            }
            else if (text == "Code G")
            {
                Desc = "* Drawing of this category is for information and hence not required to be approved.";
            }
            else if (text == "Code D")
            {
                Desc = "* Comments noted in Letter/memo attached to forwarding transmittal No.";
            }
            else if (text == "Code F")
            {
                Desc = "* Comments noted in Letter/memo attached to forwarding transmittal No....... dated......... Amend as comments indicated and resubmit for record.";
            }
            document.getElementById("tooltip").innerHTML = Desc;

            
        }
 $( function() {
    $("input[id$='dtDocumentDate']").datepicker({
      changeMonth: true,
        changeYear: true,
      dateFormat:'dd/mm/yy'
      });

      $("input[id$='dtStartdate']").datepicker({
      changeMonth: true,
        changeYear: true,
      dateFormat:'dd/mm/yy'
      });
        });


        window.onload  = setValue;
</script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="modal_master_body" runat="server">
    <form id="frmAddDocumentModal" runat="server">
        <asp:ScriptManager ID="ScriptManager1" runat="server"></asp:ScriptManager>
        <asp:HiddenField ID="Hidden1" runat="server" />
        <div class="container-fluid" style="max-height:76vh; overflow-y:auto;">
            <div class="row">
                <div class="col-sm-12">
                    <div class="form-group">
                        <label class="lblCss" for="DDLDocument">Document</label>
                         <asp:DropDownList ID="DDLDocument" CssClass="form-control" Enabled="false" runat="server"></asp:DropDownList>
                    </div>
                    <div class="form-group" >
                        <label class="lblCss" for="DDlStatus">Status</label> &nbsp;<span style="color:red; font-size:1.1rem;">*</span>
                        <%--<i class="nav-link--icon fas fa-question-circle"></i>--%>
                         <asp:DropDownList ID="DDlStatus" CssClass="form-control" onchange="getValue(this)" required runat="server"></asp:DropDownList>
                        <label id="tooltip" style="margin-top:5px;"></label>
                    </div>
                    <div class="form-group" id="Originator" runat="server">
                        <label class="lblCss" for="DDLOriginator">Originator</label>
                          <asp:RadioButtonList ID="RBLOriginator" runat="server" Width="100%" CssClass="lblCss" CellPadding="5" RepeatDirection="Horizontal" RepeatColumns="4">
                             </asp:RadioButtonList>
                    </div>
                    <div class="form-group" id="divUpdateStatus">
                        <label class="lblCss" for="RBL">Update Status to</label> &nbsp;<span style="color:red; font-size:1.1rem;">*</span>
                        <%--<i class="nav-link--icon fas fa-question-circle"></i>--%>
                         <asp:RadioButtonList ID="RBLDocumentStatusUpdate" runat="server" BorderStyle="None" RepeatDirection="Horizontal" CssClass="form-control" required>
                             <asp:ListItem Value="Current" Selected="True">&nbsp;Current Document&nbsp;&nbsp;</asp:ListItem>
                             <asp:ListItem Value="All">&nbsp;All Related Documents</asp:ListItem>
                         </asp:RadioButtonList>
                    </div>

                    <div class="form-group">
                        <label class="lblCss" for="txtrefNumber">Ref. Number</label> &nbsp;<span style="color:red; font-size:1.1rem;">*</span>
                        <asp:TextBox ID="txtrefNumber" runat="server" autocomplete="off" CssClass="form-control" ClientIDMode="Static"></asp:TextBox>
                       
                    </div>
                    <div class="form-group" >
                        <label class="lblCss" for="dtDocumentDate">Cover Letter Date</label> &nbsp;<span style="color:red; font-size:1.1rem;">*</span>
                        <asp:TextBox ID="dtDocumentDate" CssClass="form-control" runat="server" placeholder="dd/mm/yyyy" autocomplete="off" ClientIDMode="Static"></asp:TextBox>
                    </div>
                    <div class="form-group" >
                        <label class="lblCss" for="dtStartdate">Incoming Recv. Date/Approval Date</label> &nbsp;<span style="color:red; font-size:1.1rem;">*</span>
                        <asp:TextBox ID="dtStartdate" CssClass="form-control" runat="server" placeholder="dd/mm/yyyy" autocomplete="off" ClientIDMode="Static"></asp:TextBox>
                    </div>

                    <div class="form-group">
                        <label class="lblCss" for="FilCoverLetter">Choose Cover Letter</label> &nbsp;<span style="color:red; font-size:1.1rem;">*</span>
                        <div class="custom-file">
                            <asp:FileUpload ID="FileUploadCoverLetter" runat="server" CssClass="custom-file-input" />

                            <%--<asp:FileUpload ID="FilCoverLetter" CssClass="custom-file-input" runat="server" ClientIDMode="Static" /> --%>
                            <label class="custom-file-label" for="FilCoverLetter">Choose covering letter</label>
                        </div>
                    </div>

                    <div class="form-group" id="divReviewFile">
                        <label class="lblCss" for="FilCoverLetter">Choose Review File</label>
                        <div class="custom-file">
                            <asp:FileUpload ID="FileUploadDoc" runat="server" CssClass="custom-file-input" />

                            <%--<asp:FileUpload ID="FilCoverLetter" CssClass="custom-file-input" runat="server" ClientIDMode="Static" /> --%>
                            <label class="custom-file-label" for="FilCoverLetter">Choose review file</label>
                        </div>
                    </div>

                    <div class="form-group">
                        <label class="lblCss" for="txtcomments">Comments</label>
                        <asp:TextBox ID="txtcomments" CssClass="form-control" TextMode="MultiLine" runat="server" ClientIDMode="Static"></asp:TextBox>
                    </div>
                     <div class="form-group" id="divPassword" runat="server" visible="false">
                        <label class="lblCss" for="txtcomments">Enter Password</label>
                        <asp:TextBox ID="txtPassword" CssClass="form-control" TextMode="Password" runat="server" ClientIDMode="Static"></asp:TextBox>
                    </div>
                </div>
            </div> 
        </div>
        <div class="modal-footer">
            <asp:Button ID="btnSubmit" runat="server" Text="Submit" CssClass="btn btn-primary" OnClick="btnSubmit_Click" />
                </div>
    </form>
</asp:Content>
