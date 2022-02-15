<%@ Page Title="" Language="C#" MasterPageFile="~/_master_pages/modal.Master" AutoEventWireup="true" CodeBehind="testupload.aspx.cs" Inherits="ProjectManagementTool.testupload" %>
<asp:Content ID="Content1" ContentPlaceHolderID="modal_master_head" runat="server">
    
    <%--<script type="text/javascript">
        function uploadfile() {
            var xhr = new XMLHttpRequest();
           
    var data = new FormData();
            var files = document.getElementById('<%=FolderUpload1.ClientID %>').files;
    for (var i = 0; i < files.length; i++) {
      data.append(files[i].name, files[i]);
            }
            
    xhr.upload.addEventListener("progress", function (evt) {
    if (evt.lengthComputable) {
      var progress = Math.round(evt.loaded * 100 / evt.total);
      $("#progressbar").progressbar("value", progress);
    }
            }, false); 
            $("#progressbar").progressbar({
      max: 100,
        change: function (evt, ui) {
        $("#progresslabel").text($("#progressbar").progressbar("value") + "%");
      },
      complete: function (evt, ui) {
        $("#progresslabel").text("File upload successful!");
      }
    });
    xhr.open("POST", "testupload.aspx");
    xhr.send(data);
    
            evt.preventDefault();
            
        }
        
    </script>--%>
    <%--<script type="text/javascript">
        function uploadurl() {
            if (document.getElementById('<%=FolderUpload1.ClientID %>').value != "") {

                document.getElementById('<%=divProgress.ClientID %>').style.display = "block";
                document.getElementById("divUpload").style.display = "block";

                var data = new FormData();
                
                var request = new window.XMLHttpRequest();
                
                //request.responseType = "json";
                var cnt = document.getElementById('<%=FolderUpload1.ClientID %>');
                
                for (var i = 0; i < cnt.files.length; i++) {
                    data.append("file",cnt.files[i]);
                }
                
                request.upload.addEventListener("progress", function (e) {
                    var loaded = e.loaded;
                    var total = e.total;
                    var percomplete = Math.round((loaded / total) * 100);
                    document.getElementById('<%=divProgress.ClientID %>').style.width = percomplete + "%";
                    document.getElementById("<%=lblPercentage.ClientID %>").firstChild.data = percomplete + "%";
                })
                request.addEventListener("load", function (e) {
                    if (request.status == 200) {

                    }
                });
                request.open("post", window.location.href);
                request.send(data);
                //return true;
            }
        }
    </script>--%>
    <%--<script type="text/javascript">
        var size = 2;
        var id = 0;
        
        function ProgressBar() {
            if (document.getElementById('<%=FolderUpload1.ClientID %>').value != "") {
                var cnt = document.getElementById('<%=FolderUpload1.ClientID %>');
                document.getElementById('<%=divProgress.ClientID %>').style.display = "block";
                document.getElementById("divUpload").style.display = "block";
                var data = new formdata();
                var files = document.getelementbyid('<%=folderupload1.clientid %>').files;
                for (var i = 0; i < files.length; i++) {
                  data.append(files[i].name, files[i]);
                 }
                id = setInterval("progress()", 20);
                var xhr = new window.XMLHttpRequest();
                    xhr.upload.addEventListener("progress", function (evt) {
                        if (evt.lengthComputable) {
                            var progress = Math.round((evt.loaded / evt.total) * 100);
                            
                            //document.getElementById("<%=lblPercentage.ClientID %>").firstChild.data = progress + "%";
                            //evt.preventDefault();
                            id = setInterval("progress()", 40);
                        }
                }, false);
                xhr.open("POST", "testupload.aspx");
                xhr.send(data);
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
    <%--<script type="text/javascript"> 
        function showpercentage() {
            var uploadedFiles = $('#FolderUpload1')[0].files;  
                if (uploadedFiles.length > 0) {  
                    var formData = new FormData();  
                    for (var i = 0; i < uploadedFiles.length; i++) {  
                        formData.append(uploadedFiles[i].name, uploadedFiles[i]);  
                    }
                    
                }  
                var progressbarLabel = $('#progressbar-label');  
                var progressbarDiv = $('#progress-bar');  
                $.ajax  
                    ({  
                        url: 'testupload.aspx',  
                        method: 'post',  
                        contentType: false,  
                        processData: false,  
                        data: formData,  
                        success: function () {  
                            progressbarLabel.text('Uploaded Successfully');  
                            progressbarDiv.fadeOut(2000);  
                        },  
                        error: function (err) {  
                            alert(err.statusText);  
                        }  
                    });  
                progressbarLabel.text('Please Wait...');  
                progressbarDiv.progressbar({  
                    value: false  
            }).fadeIn(1000);  
            return true;
        }
    </script>--%>  
<%--    <script type="text/javascript">  
     var before_loadtime = new Date().getTime();  
     window.onload = Pageloadtime;  
     function Pageloadtime() {  
         var aftr_loadtime = new Date().getTime();  
         // Time calculating in seconds  
         pgloadtime = (aftr_loadtime - before_loadtime) / 1000  
  
         document.getElementById("loadtime").innerHTML = "Pgae load time is <font color='red'><b>" + pgloadtime + "</b></font> Seconds";  
     }  
</script> --%> 
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="modal_master_body" runat="server">
    <form id="frmAddDocumentModal" runat="server">
        <asp:ScriptManager ID="ScriptManager1" runat="server">
</asp:ScriptManager>
        <div class="container-fluid" style="max-height:77vh; overflow-y:auto;">
            <div class="row">
                <div class="col-sm-12">
                    <div class="form-group">
                        <label class="lblCss" for="FilDocument">Choose File/s</label>
                      
                        <div class="custom-file">
                            <input type="file" runat="server" id="FolderUpload1" CssClass="custom-file-input" name="FilDocument" webkitdirectory mozdirectory msdirectory odirectory directory multiple />
                        </div>
                    </div>
                    </div>
                <div>  
 
</div>  

                <%--<div id="divUpload" style="display:none; text-align:center; width:300pt; margin:auto;">
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
                </div>
            </div>
    <%--<div id="progressbar" class="progressbar">
            <div id="progresslabel" class="progressbarlabel"></div>
        </div>--%>
         <div id="dvProgressBar" style=" text-align:center; position:relative;" >
                     <img src="../_assets/images/progress.gif" width="40" alt="loading"  /> <span style="color:#006699; font-weight:bold;">Uploading is in progress...</span>
                     </div> 
    <div class="modal-footer">
                    <%--<button type="button" class="btn btn-secondary" data-dismiss="modal">Close</button>--%>
                    <%--<button type="button" class="btn btn-primary">Save changes</button>--%>
            <asp:Button ID="btnSubmit" runat="server" Text="Submit" CssClass="btn btn-primary" OnClientClick="return showpercentage()" OnClick="btnSubmit_Click" />
                </div>
        </form>
</asp:Content>
