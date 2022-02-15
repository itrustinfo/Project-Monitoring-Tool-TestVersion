<%@ Page Title="" Language="C#" MasterPageFile="~/_master_pages/modal.Master" AutoEventWireup="true" CodeBehind="add-user.aspx.cs" Inherits="ProjectManagementTool._modal_pages.add_user" %>
<asp:Content ID="Content1" ContentPlaceHolderID="modal_master_head" runat="server">
    <style type="text/css">
         .circular {
	width: 40px;
	height: 40px;
	border-radius: 50px;
	-webkit-border-radius: 50px;
	-moz-border-radius: 50px;
	
	}
    </style>
    <script type="text/javascript">
        $(document).ready(function () {
            $(".showStatusModal").click(function (e) {
                e.preventDefault();
                jQuery.noConflict();
                var url = $(this).attr("href");
                $("#ModAdd iframe").attr("src", url);
                $("#ModAdd").modal("show");
            });

         
        });
    </script>
    <script type="text/javascript">
function isNumber(evt) {
    evt = (evt) ? evt : window.event;
    var charCode = (evt.which) ? evt.which : evt.keyCode;
    if (charCode > 31 && (charCode < 48 || charCode > 57)) {
        alert("only numbers allowed !");
        return false;
    }
    return true;
        }

       function sync(textbox)
    {
  document.getElementById('txtloginusername').value = textbox.value;
        }

        function ShowImagePreview(input) {
            if (input.files && input.files[0]) {
                var reader = new FileReader();
                reader.onload = function (e) {
                    $('#<%=ImgPicure.ClientID%>').prop('src', e.target.result)
                        .width(100)
                        .height(100);
                };
                reader.readAsDataURL(input.files[0]);
            }
        }
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="modal_master_body" runat="server">
    <form id="frmAddIssuesModal" runat="server">
        <div class="container-fluid" style="max-height:80vh; overflow-y:auto;">
            <div class="row">
                <div class="col-sm-6">
                    <div class="form-group">
                        <label class="lblCss" for="txtfirstname">First Name</label>&nbsp;<asp:Label runat="server" Text="*" ForeColor="Red"></asp:Label>
                         <asp:TextBox ID="txtfirstname" runat="server" CssClass="form-control" autocomplete="off" required></asp:TextBox>
                    </div>
                    <div class="form-group">
                        <label class="lblCss" for="txtIssue_Description">Last Name</label>&nbsp;<asp:Label runat="server" Text="*" ForeColor="Red"></asp:Label>
                         <asp:TextBox ID="txtlastname" runat="server" CssClass="form-control" autocomplete="off" required></asp:TextBox>
                       
                    </div>

                    <div class="form-group">
                        <label class="lblCss" for="txtemailid">Email-ID</label>&nbsp;<asp:Label runat="server" Text="*" ForeColor="Red"></asp:Label>
                         <asp:TextBox ID="txtemailid" runat="server" TextMode="Email" CssClass="form-control" autocomplete="off" ClientIDMode="Static" required onkeyup="sync(this)"></asp:TextBox>
                    </div>
                    <div class="form-group">
                        <label class="lblCss" for="txtaddress1">Address 1</label>
                       
                        <asp:TextBox ID="txtaddress1" runat="server" TextMode="MultiLine" CssClass="form-control"></asp:TextBox>
                    </div>
                     <div class="form-group">
                        <label class="lblCss" for="txtaddress2">Address 2</label>
                       
                        <asp:TextBox ID="txtaddress2" runat="server" TextMode="MultiLine" CssClass="form-control"></asp:TextBox>
                    </div>
                    
                </div>
                <div class="col-sm-6"> 
                    <div class="form-group">
                        <label class="lblCss" for="txtmobile">Mobile Number</label>&nbsp;<asp:Label runat="server" Text="*" ForeColor="Red"></asp:Label>
                         <asp:TextBox ID="txtmobile" runat="server" CssClass="form-control" required MaxLength="10" autocomplete="off" onkeypress="return isNumber(event)"></asp:TextBox>
                    </div>
                    <div class="form-group" id="divUserType" runat="server">
                        <label class="lblCss" for="DDlUserType">User Type</label>
                        <asp:DropDownList ID="DDlUserType" runat="server" CssClass="form-control">
                            <%--<asp:ListItem Value="PA">Project Admin</asp:ListItem>
                            <asp:ListItem Value="S">Submitter</asp:ListItem>
                            <asp:ListItem Value="R">Reviewer A</asp:ListItem>
                            <asp:ListItem Value="RB">Reviewer B</asp:ListItem>
                            <asp:ListItem Value="C">Quality Engineer</asp:ListItem>
                            <asp:ListItem Value="A">Approver</asp:ListItem>
                            <asp:ListItem Value="E">Engineer</asp:ListItem>
                            <asp:ListItem Value="F">Finance</asp:ListItem>
                            <asp:ListItem Value="EF">Engineer and Finance</asp:ListItem>--%>
                        </asp:DropDownList>
                    </div>
                     <div class="form-group" id="divUsername" runat="server">
                        <label class="lblCss" for="txtloginusername">Login UserName</label>&nbsp;<asp:Label runat="server" Text="*" ForeColor="Red"></asp:Label>
                        <asp:TextBox ID="txtloginusername" runat="server"  CssClass="form-control" ClientIDMode="Static" Enabled="false" required></asp:TextBox>
                    </div>
                     <div class="form-group" id="divPassword" runat="server">
                        <label class="lblCss" for="txtloginpassword">Login Password</label>&nbsp;<asp:Label runat="server" Text="*" ForeColor="Red"></asp:Label><a id="hlresetpassowrd" runat="server" visible="false" class="showStatusModal">&nbsp;Reset Password</a>
                        <asp:TextBox ID="txtloginpassword" runat="server" TextMode="Password"  CssClass="form-control" required></asp:TextBox>
                    </div>
             
                    <div class="form-group" id="divMailSettings" runat="server">
                        <label class="lblCss" for="chkboxlstMailSettings">Mail Settings</label>&nbsp;<asp:CheckBoxList runat="server" ID="chkboxlstMailSettings" CssClass="form-control" RepeatDirection="Horizontal">
                            <asp:ListItem Value="0">&nbsp;document mails&nbsp;</asp:ListItem>
                            <asp:ListItem Value="1">&nbsp;project master mail reminder</asp:ListItem>
                        </asp:CheckBoxList>
                    </div>
                     <div class="form-group">
                        <label class="lblCss" for="ProfileUpload">Profile Image</label>
                        <div class="custom-file">
                            <asp:FileUpload ID="ProfileUpload" runat="server" accept=".png,.jpg,.jpeg,.gif" CssClass="custom-file-input" onchange="ShowImagePreview(this);"/>

                            <%--<asp:FileUpload ID="FilCoverLetter" CssClass="custom-file-input" runat="server" ClientIDMode="Static" /> --%>
                            <label class="custom-file-label" for="FilCoverLetter">Choose Image</label>
                        </div><div style="line-height:10px">&nbsp;</div>
                         <div style="text-align:left">
                             <asp:Image ID="ImgPicure" runat="server" Width="100px" Height="100px"
                    ImageUrl="~/_assets/images/Photo_mb.png" CssClass="circular"/>
                         </div>
                    </div>
                    </div>
            </div> 
        </div>
        <div class="modal-footer">
            <asp:Button ID="btnSubmit" runat="server" Text="Submit" CssClass="btn btn-primary" OnClick="btnSubmit_Click" />
                </div>
         <div id="ModAdd" class="modal it-modal fade">
	    <div class="modal-dialog modal-dialog-centered modal-dialog-scrollable modal-lg">
		    <div class="modal-content">
			    <div class="modal-header">
				    <h5 class="modal-title">Reset Password</h5>
                    <button aria-label="Close" class="close" data-dismiss="modal" type="button"><span aria-hidden="true">&times;</span></button>
			    </div>
			    <div class="modal-body">
                    <iframe class="border-0 w-100" style="height:280px;" loading="lazy"></iframe>
			    </div>
              
		    </div>
	    </div>
    </div>
    </form>
</asp:Content>
