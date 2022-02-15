<%@ Page Title="" Language="C#" MasterPageFile="~/_master_pages/modal.Master" AutoEventWireup="true" CodeBehind="add-general-document.aspx.cs" Inherits="ProjectManagementTool._modal_pages.add_general_document" %>
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
        function ShowProgressBar(status) {
            if (status == "true") {
                document.getElementById('dvProgressBar').style.visibility = 'visible';
            }
            else {
                document.getElementById('dvProgressBar').style.visibility = 'hidden';
            }
        
        }
         function folderMsg()
        {
             alert("Please Note if Same Files already exists, it will not upload the existing files.Please delete the files and try again !");
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
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="modal_master_body" runat="server">
    <form id="frmAddDocumentModal" runat="server">
        <asp:ScriptManager ID="Script1" runat="server"></asp:ScriptManager>
        <%--<div id="loader"></div>--%>
        <div class="container-fluid" style="max-height:75vh; overflow-y:auto;">
            <div class="row">
                <div class="col-sm-6">
                  
                    <asp:HiddenField ID="HiddenVal" runat="server" />
                    <asp:HiddenField ID="HiddenField1" runat="server" />
                    <asp:HiddenField ID="HiddenField2" runat="server" />
                    <div class="form-group">
                        <label class="lblCss" for="DDLSubmitter">Folder Name</label>
                         <asp:Label ID="lblFolder" runat="server" Text="" CssClass="form-control"></asp:Label>
                    </div>
                    <div class="form-group" id="DocNameDiv" runat="server" visible="false">
                        <label class="lblCss" for="txtdesc">Document Name</label> &nbsp;<span style="color:red; font-size:1.1rem;">*</span>
                        <asp:TextBox ID="txtDocumentName" CssClass="form-control" runat="server" autocomplete="off" required ClientIDMode="Static"></asp:TextBox>
                        
                    </div>
                    <div class="form-group">
                        <label class="lblCss" for="txtdesc">Subject / Description</label> &nbsp;<span style="color:red; font-size:1.1rem;">*</span>
                        <asp:TextBox ID="txtdesc" CssClass="form-control" runat="server" autocomplete="off" required ClientIDMode="Static"></asp:TextBox>
                        
                    </div>
                    <div class="form-group">
                        <label class="lblCss" for="txtprefnumber">Reference Number</label>
                        <asp:TextBox ID="txtprefnumber" CssClass="form-control" autocomplete="off" runat="server" ClientIDMode="Static"></asp:TextBox>
                    </div>
                    <div class="form-group">
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

                     <div class="form-group">
                        <label class="lblCss" for="txtFileRefNumber">File Reference Number</label>
                        <asp:TextBox ID="txtFileRefNumber" CssClass="form-control" autocomplete="off" runat="server" ClientIDMode="Static"></asp:TextBox>
                    </div>

                    
                     
                   
                </div>
                <div class="col-sm-6">
                   
                <%--    <div class="form-group" id="Originator" runat="server">
                        <label class="lblCss" for="DDLOriginator">Originator</label>
                          <asp:RadioButtonList ID="RBLOriginator" runat="server" Width="100%" CssClass="lblCss" CellPadding="5" RepeatDirection="Horizontal" AutoPostBack="true" OnSelectedIndexChanged="RBLOriginator_SelectedIndexChanged" RepeatColumns="4">
                                 <asp:ListItem Selected="True" Value="Contractor">&nbsp;&nbsp;Contractor</asp:ListItem>
                                 <asp:ListItem Value="JUIDCo">&nbsp;&nbsp;JUIDCo</asp:ListItem>
                                 <asp:ListItem Value="NJSEI">&nbsp;&nbsp;NJSEI</asp:ListItem>
                              <asp:ListItem Value="Others">&nbsp;&nbsp;Others</asp:ListItem>
                             </asp:RadioButtonList>
                    </div>--%>
                    <%--<div class="form-group" id="OriginatorNumber" runat="server">
                        <label class="lblCss" for="txtRefNumber">Originator Ref.Number</label> &nbsp;<span style="color:red; font-size:1.1rem;">*</span>
                        <asp:TextBox ID="txtRefNumber" CssClass="form-control" autocomplete="off" runat="server" required ClientIDMode="Static"></asp:TextBox>
                        
                    </div>--%>

                    <div class="form-group" id="UploadType" runat="server">
                        <label class="lblCss" for="RBLUploadType">Upload Type</label> 
                         <asp:RadioButtonList ID="RBLUploadType" runat="server" Width="100%" CssClass="lblCss" CellPadding="5" RepeatDirection="Horizontal" AutoPostBack="true" OnSelectedIndexChanged="RBLUploadType_SelectedIndexChanged">
                                 <asp:ListItem Value="File" Selected="True">&nbsp;&nbsp;File</asp:ListItem>
                                 <asp:ListItem Value="Folder">&nbsp;&nbsp;Folder</asp:ListItem>
                            
                             </asp:RadioButtonList>
                    </div>
                    <div class="form-group" id="UploadFile" runat="server">
                        <label class="lblCss" for="FilDocument">Choose File/s</label> &nbsp;<span style="color:red; font-size:1.1rem;">*</span>
                        <div class="custom-file">
                            <asp:FileUpload ID="FileUploadDoc" runat="server" AllowMultiple="true" CssClass="custom-file-input"  onchange="folderMsg()"/>
                            <label class="custom-file-label" for="FilDocument">Choose document</label>
                        </div>
                    </div>
                    <div class="form-group" id="UploadFolder" runat="server" visible="false">
                        <label class="lblCss" for="FilDocument">Choose Folder</label> &nbsp;<span style="color:red; font-size:1.1rem;">*</span>
                        <div class="custom-file">
                            <input type="file" runat="server" id="FolderUpload1" CssClass="custom-file-input" name="FilDocument" onchange="folderMsg()" webkitdirectory mozdirectory msdirectory odirectory directory multiple />

                       
                        </div>
                      
                    </div>
                    <div class="form-group">
                        <label class="lblCss" for="DatIncomingRevisionDate"><asp:Label ID="LblDateText" runat="server" Text="Incoming Recv. Date"></asp:Label></label>
                        <asp:TextBox ID="dtIncomingRevDate" CssClass="form-control" runat="server" placeholder="dd/mm/yyyy" autocomplete="off" onchange="CheckDate()" ClientIDMode="Static"></asp:TextBox>
                    </div>
                    <div class="form-group" id="DocumentDate" runat="server">
                        <%--Cover Letter Date--%>
                        <label class="lblCss" for="dtDocumentDate">Document Date</label> &nbsp;<span style="color:red; font-size:1.1rem;">*</span>
                        <asp:TextBox ID="dtDocumentDate" CssClass="form-control" required runat="server" placeholder="dd/mm/yyyy" autocomplete="off" onchange="CheckDate()" ClientIDMode="Static"></asp:TextBox>
                    </div>
                 
                    
                   
                   <div class="form-group">
                        <label class="lblCss" for="txtremarks">Remarks</label>
                        <asp:TextBox ID="txtremarks" CssClass="form-control" TextMode="MultiLine" runat="server" ClientIDMode="Static"></asp:TextBox>
                    </div>
                </div>
            </div> 
        </div>
        <asp:HiddenField ID="hidForModel" runat="server" />
       
        <div id="dvProgressBar" style=" text-align:center; position:relative; visibility:hidden;" >
                     <img src="../_assets/images/progress.gif" width="40" alt="loading"  /> <span style="color:#006699; font-weight:bold;">Uploading is in progress...</span>
                     </div> 
        
        <div class="modal-footer">
                   
            <asp:Button ID="btnSubmit" runat="server" Text="Submit" CssClass="btn btn-primary" OnClientClick="ShowProgressBar('true')" OnClick="btnSubmit_Click" />
                </div>
    </form>
</asp:Content>
