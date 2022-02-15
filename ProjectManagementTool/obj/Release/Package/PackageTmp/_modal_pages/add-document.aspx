 <%@ Page Title="" Language="C#" MasterPageFile="~/_master_pages/modal.Master" AutoEventWireup="true" CodeBehind="add-document.aspx.cs" Inherits="ProjectManager._modal_pages.add_document" %>
<%--<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>--%>
<asp:Content ID="ModalHead" ContentPlaceHolderID="modal_master_head" runat="server">
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
        function ShowProgressBar(status) {
            if (status == "true") {
                if (document.getElementById("txtdesc").value != "") {
                    document.getElementById('dvProgressBar').style.visibility = 'visible';
                }
            }
            else {
                document.getElementById('dvProgressBar').style.visibility = 'hidden';
            }
        }
         function folderMsg(sender)
         {
             //alert(document.getElementById('<%=DDLDocumentType.ClientID%>').value);
             if (document.getElementById('<%=DDLDocumentType.ClientID%>').value == "Photographs") {
                 var validExts = new Array(".jpeg", ".jpg", ".png",".gif",".bmp",".tiff");
                var fileExt = sender.value;
                fileExt = fileExt.substring(fileExt.lastIndexOf('.'));
                 if (validExts.indexOf(fileExt) < 0) {
                     alert("Invalid file selected, valid files are of " +
                         validExts.toString() + " types.");

                     var fu = document.getElementById("<%= FileUploadDoc.ClientID %>");
                     if (fu != null) {
                         fu.setAttribute("type", "input");
                         fu.setAttribute("type", "file");
                     }
                     return false;
                 }
                 else {
                     alert("Please Note if Same Files already exists, it will not upload the existing files.Please delete the files and try again !");
                     return true;
                 }
             }
             var showAlert = "False";
                let files = event.target.files;
                for (let i = 0; i < files.length; i++) {
                    var sFilePath = files[i].webkitRelativePath;
                    if (sFilePath.length > 240) {
                        showAlert = "True";
                    }
                }
             if (showAlert == "True") {
                 document.getElementById('<%=FolderUpload1.ClientID%>').value = "";
                 alert('Folder you uploaded exceeds the max limit. Please shorten the folder length and try again.');
             }
             else {
                 alert("Please Note if Same Files already exists, it will not upload the existing files.Please delete the files and try again !");
             }
         }

        function DisplayMessage(message) {
            alert(message);
            document.getElementById('divMessage').value = message;
        }
         window.addEventListener("offline", function (e) {
                alert("You are Offline.Please check network and Log in again !");
            // document.getElementById('divMessage').innerHTML = "You are Offline.Please check your network.";
              //document.getElementById('divMessage').style.color = "Red";
              
                parent.location.href =  window.location.protocol + "//" + window.location.host + "/Login.aspx";
         });

          window.addEventListener("online", function (e) {
               // alert("You are Online.Please check network and Log in again !");
             // document.getElementById('divMessage').innerHTML = "You are back Online";
             // document.getElementById('divMessage').style.color = "Green";
                //parent.location.href = parent.location.href;
            });

    </script>
    <%--<script type="text/javascript">
        var size = 2;
        var id = 0;

        function ProgressBar() {
            if (document.getElementById('<%=FileUploadDoc.ClientID %>').value != "") {
                //alert("processing");
                document.getElementById('<%=divProgress.ClientID %>').style.display = "block";
                document.getElementById("divUpload").style.display = "block";
                id = setInterval("progress()", 20);
                return true;
            }
            else {
                //alert("Select a file to upload");
                return false;
            }

        }

        function progress() {
            size = size + 1;
            if (size > 299) {
                clearTimeout(id);
            }
            document.getElementById('<%=divProgress.ClientID %>').style.width = size + "pt";
            document.getElementById("<%=lblPercentage.ClientID %>").firstChild.data = parseInt(size / 3) + "%";
        }

    </script>--%>
    <%--<style type="text/css">
         #loader {  
    position: fixed;  
    left: 0px;  
    top: 0px;  
    width: 100%;  
    height: 100%;  
    z-index: 9999;  
    background: url('../../_assets/images/pageloading.gif') 50% 50% no-repeat;  
    background-size: 242px 200px;
}  
          #spinner {
position: fixed;
left: 0px;
top: 0px;
width: 100%;
height: 100%;
z-index: 9999;
background: url('../../_assets/images/pageloading.gif') 50% 50% no-repeat;  
    background-size: 242px 200px;
}
    </style>--%>
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
    <style type="text/css">
        .modalBackground
        {
            background-color: Gray;
            filter: alpha(opacity=70);
            opacity: 0.7;
        }
        .modalPopup
        {
            background-color: SkyBlue;
            border-width: 3px;
            border-style: solid;
            border-color: Black;
            padding: 3px;
            width: 100px;
            height: 100px;
        }
    </style>
   <%-- <script type="text/javascript">
        $(document).ready(function () {
            $('#loader').fadeOut();
        });
</script>--%>
</asp:Content>
<asp:Content ID="ModalBody" ContentPlaceHolderID="modal_master_body" runat="server">
    
    <form id="frmAddDocumentModal" runat="server">
        <asp:ScriptManager ID="Script1" runat="server"></asp:ScriptManager>
        <%--<div id="loader"></div>--%>
        <div class="container-fluid" style="max-height:75vh; overflow-y:auto; min-height:75vh;">
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
                        <label class="lblCss" for="DDLSubmitter">Submittal</label>
                         <asp:DropDownList ID="DDLDocuments" Enabled="false" CssClass="form-control"  runat="server"></asp:DropDownList>
                    </div>
                    <div class="form-group">
                        <label class="lblCss" for="txtdesc">Subject / Description</label> &nbsp;<span style="color:red; font-size:1.1rem;">*</span>
                        <asp:TextBox ID="txtdesc" CssClass="form-control" runat="server" autocomplete="off" required ClientIDMode="Static"></asp:TextBox>
                        
                    </div>
                    <div class="form-group" id="PrjRefNumber" runat="server">
                        <label class="lblCss" for="txtprefnumber">Reference Number</label>
                        <asp:TextBox ID="txtprefnumber" CssClass="form-control" autocomplete="off" runat="server" ClientIDMode="Static"></asp:TextBox>
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
                        <asp:TextBox ID="txtFileRefNumber" CssClass="form-control" autocomplete="off" runat="server" ClientIDMode="Static"></asp:TextBox>
                    </div>

                    
                     <div class="form-group">
                        <label class="lblCss" for="txtremarks">Remarks</label>
                        <asp:TextBox ID="txtremarks" CssClass="form-control" TextMode="MultiLine" runat="server" ClientIDMode="Static"></asp:TextBox>
                    </div>
                   
                </div>
                <div class="col-sm-6">
                    <div class="form-group">
                        <label class="lblCss" for="DDLDocumentType">Document Type</label>
                        <asp:DropDownList ID="DDLDocumentType" runat="server" CssClass="form-control"  AutoPostBack="true" OnSelectedIndexChanged="DDLDocumentType_SelectedIndexChanged">
                       <asp:ListItem Value="Cover Letter">Cover Letter</asp:ListItem>
                       <asp:ListItem Value="General Document">General Document</asp:ListItem>
                            <asp:ListItem Value="Photographs">Photographs</asp:ListItem>
                   </asp:DropDownList> 
                    </div>
                    <div class="form-group" id="Originator" runat="server">
                        <label class="lblCss" for="DDLOriginator">Originator</label>
                          <asp:RadioButtonList ID="RBLOriginator" runat="server" Width="100%" CssClass="lblCss" CellPadding="5" RepeatDirection="Horizontal" AutoPostBack="true" OnSelectedIndexChanged="RBLOriginator_SelectedIndexChanged" RepeatColumns="4">
                               <%--  <asp:ListItem Selected="True" Value="Contractor">&nbsp;&nbsp;Contractor</asp:ListItem>
                                 <asp:ListItem Value="JUIDCo">&nbsp;&nbsp;JUIDCo</asp:ListItem>
                                 <asp:ListItem Value="NJSEI">&nbsp;&nbsp;NJSEI</asp:ListItem>
                              <asp:ListItem Value="Others">&nbsp;&nbsp;Others</asp:ListItem>--%>
                             </asp:RadioButtonList>
                    </div>
                    <div class="form-group" id="OriginatorNumber" runat="server">
                        <label class="lblCss" for="txtRefNumber">Originator Ref.Number</label> &nbsp;<span style="color:red; font-size:1.1rem;">*</span>
                        <asp:TextBox ID="txtRefNumber" CssClass="form-control" autocomplete="off" runat="server" required ClientIDMode="Static"></asp:TextBox>
                        
                    </div>

                    <div class="form-group">
                        <label class="lblCss" for="RBLUploadType">Upload Type</label> 
                         <asp:RadioButtonList ID="RBLUploadType" runat="server" Width="100%" CssClass="lblCss" CellPadding="5" RepeatDirection="Horizontal" AutoPostBack="true" OnSelectedIndexChanged="RBLUploadType_SelectedIndexChanged">
                                 <asp:ListItem Value="File" Selected="True">&nbsp;&nbsp;File</asp:ListItem>
                                 <asp:ListItem Value="Folder">&nbsp;&nbsp;Folder</asp:ListItem>
                             <asp:ListItem Value="Paste">&nbsp;&nbsp;Paste</asp:ListItem>
                             </asp:RadioButtonList>
                    </div>

                    <div class="form-group" id="IncomingReceDate" runat="server">
                        <label class="lblCss" for="DatIncomingRevisionDate"><asp:Label ID="LblDateText" runat="server"></asp:Label></label>
                        <asp:TextBox ID="dtIncomingRevDate" CssClass="form-control" runat="server" placeholder="dd/mm/yyyy" autocomplete="off" onchange="CheckDate()" ClientIDMode="Static"></asp:TextBox>
                    </div>
                    <div class="form-group" id="DocumentDate" runat="server">
                        <%--Cover Letter Date--%>
                        <label class="lblCss" for="dtDocumentDate">Document Date</label> &nbsp;<span style="color:red; font-size:1.1rem;">*</span>
                        <asp:TextBox ID="dtDocumentDate" CssClass="form-control" required runat="server" placeholder="dd/mm/yyyy" autocomplete="off" onchange="CheckDate()" ClientIDMode="Static"></asp:TextBox>
                    </div>
                    <div class="form-group" id="CoverLetterUpload" runat="server">
                        <label class="lblCss" for="FilCoverLetter"><asp:Label ID="LblDocType" runat="server"></asp:Label></label> &nbsp;<span style="color:red; font-size:1.1rem;">*</span>
                        <div class="custom-file">
                            <asp:FileUpload ID="FileUploadCoverPage" runat="server" CssClass="custom-file-input" />

                            <%--<asp:FileUpload ID="FilCoverLetter" CssClass="custom-file-input" runat="server" ClientIDMode="Static" /> --%>
                            <label class="custom-file-label" for="FilCoverLetter">Choose covering letter</label>
                        </div>
                    </div>
                    <div class="form-group" id="UploadFile" runat="server">
                        <label class="lblCss" for="FilDocument">Choose File/s</label> &nbsp;<span id="spfile" runat="server" style="color:red; font-size:1.1rem;">*</span>
                        <div class="custom-file">
                            <asp:FileUpload ID="FileUploadDoc" runat="server" AllowMultiple="true" CssClass="custom-file-input"  onchange="folderMsg(this)"/>

                            <%--<asp:FileUpload ID="FilDocument" CssClass="custom-file-input" runat="server" ClientIDMode="Static" /> --%>
                            <label class="custom-file-label" id="SelectedFiles" for="FilDocument">Choose document</label>
                        </div>
                    </div>
                    <div class="form-group" id="UploadFolder" runat="server">
                        <label class="lblCss" for="FilDocument">Choose Folder</label> &nbsp;<span style="color:red; font-size:1.1rem;">*</span>
                        <div class="custom-file">
                            <input type="file" runat="server" id="FolderUpload1" CssClass="custom-file-input" name="FilDocument" onchange="folderMsg(this)" webkitdirectory mozdirectory msdirectory odirectory directory multiple />

                            <%--<asp:FileUpload ID="FilDocument" CssClass="custom-file-input" runat="server" ClientIDMode="Static" /> --%>
                            <%--<label class="custom-file-label" for="FilDocument">Choose folder</label>--%>
                        </div>
                      
                    </div>
                    <div class="form-group" id="SubmissionType" runat="server">
                        <label class="lblCss" for="RBLSubmissionType">Submission Type</label>
                          <asp:RadioButtonList ID="RBLSubmissionType" runat="server" Width="100%" CssClass="lblCss" CellPadding="5" RepeatDirection="Horizontal" RepeatColumns="4">
                              <asp:ListItem Value="Submission" Selected="True">&nbsp;Submission</asp:ListItem>
                              <asp:ListItem Value="Revision">&nbsp;Revision</asp:ListItem>
                              <asp:ListItem Value="Replacement">&nbsp;Replacement</asp:ListItem>
                             </asp:RadioButtonList>
                    </div>
                    <div class="form-group" id="CopiedDocuments" runat="server" visible="false">
                        <label class="lblCss" for="lstDocuments">Copied Documents</label>
                        <asp:ListBox ID="lstDocuments" runat="server" CssClass="form-control">
                       </asp:ListBox> 
                    </div>
                      <div id="divMessage" style="color:red"></div>
                   
                </div>
            </div> 
        </div>
        <asp:HiddenField ID="hidForModel" runat="server" />
       <%--  <asp:ModalPopupExtender ID="ModalPopupExtender1" runat="server" TargetControlID="hidForModel" PopupControlID="PnlModal" BackgroundCssClass="modalBackground">
    </asp:ModalPopupExtender>
        <asp:Panel ID="PnlModal" runat="server" Width="500px" CssClass="modalPopup">
        Document Exists. Do you want to override or Ignore ?<br />
        <asp:Button ID="Button3" runat="server" Text="Override"  />
        <asp:Button ID="Button4" runat="server" Text="Ignore" />
    </asp:Panel>--%>
      <%-- <div id="divUpload" style="display:none; text-align:center; width:300pt; margin:auto;">
               <div style="width:300pt; text-align: center;">
                                Uploading...
                            </div>
               <div style="width:300pt; height:20px; border: solid 1pt gray">
                                <div id="divProgress" runat="server" style="width: 1pt; height: 20px; background-color:#007bff;
                                    display:none">
                                </div>
                            </div>
               <div style="width:300pt; text-align: center;">
                                <asp:Label ID="lblPercentage" runat="server" Text="Label"></asp:Label>

                            </div>                            
         </div>--%>
        <div id="dvProgressBar" style=" text-align:center; position:relative; visibility:hidden;" >
                     <img src="../_assets/images/progress.gif" width="40" alt="loading"  /> <span style="color:#006699; font-weight:bold;">Uploading is in progress...</span>
                     </div> 
         <%--<div id="dvProgressBar" style="visibility:hidden; text-align:center; position:absolute;" >
                     <img src="../_assets/images/progress.gif" width="40" alt="loading"  /> <span style="color:#006699; font-weight:bold;">Uploading is in progress...</span>
                     </div> --%>
        <div class="modal-footer">
                    <%--<button type="button" class="btn btn-secondary" data-dismiss="modal">Close</button>--%>
                    <%--<button type="button" class="btn btn-primary">Save changes</button>--%>
            <asp:Button ID="btnSubmit" runat="server" Text="Submit" CssClass="btn btn-primary" OnClientClick="ShowProgressBar('true')" OnClick="btnSubmit_Click" />
                </div>
    </form>
</asp:Content>
