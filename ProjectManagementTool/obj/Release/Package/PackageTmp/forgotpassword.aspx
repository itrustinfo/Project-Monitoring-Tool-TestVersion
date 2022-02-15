<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="forgotpassword.aspx.cs" Inherits="ProjectManagementTool.forgotpassword" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Forgot Password</title>
    <meta charset="utf-8">
  <meta name="viewport" content="width=device-width, initial-scale=1, shrink-to-fit=no">
  <link href="_assets/images/favicon.png" rel="icon" />

    <link rel="stylesheet" href="_assets/styles/style.css" />

</head>
<body>
    <form id="form1" runat="server">
       <div class="container-scroller">
    <div class="container-fluid page-body-wrapper full-page-wrapper auth-page">
      <div class="content-wrapper d-flex align-items-center auth auth-bg-1 theme-one">
        <div class="row w-100">
            
          <div class="col-lg-4 mx-auto" id="Forgot" runat="server">
              
            <div class="auto-form-wrapper">
                <h1 style="margin-top:0; color:#1D74C0;">Forgot Password?</h1>
                <hr />
             
                <div class="form-group">
                  <label class="label">Enter your Email-ID</label>
                  <div class="input-group"> 
                    <input type="text" class="form-control" autocomplete="off" id="txtEmialID" runat="server" placeholder="Email-ID">
                    <div class="input-group-append">
                      <span class="input-group-text">
                        <i class="mdi mdi-check-circle-outline"></i>
                      </span>
                    </div>
                  </div>
                </div>
                <div class="form-group">
                    <table style="width:100%;">
                        <tr>
                            <td>
                                <asp:Button ID="BtnSubmit" runat="server" Text="Submit" CssClass="btn btn-primary submit-btn btn-block" OnClick="BtnSubmit_Click" />
                            </td>
                            <td>
                                <asp:Button ID="Btnback" runat="server" Text="Back" CausesValidation="false" CssClass="btn btn-primary submit-btn btn-block" PostBackUrl="~/Login.aspx" />
                            </td>
                        </tr>
                    </table>
                    

                    
                    
                    
                    <%--<button class="btn btn-primary submit-btn btn-block">Login</button>--%>
                </div>
                
              
            
            </div>
         </div>

            <div class="col-lg-4 mx-auto" id="resetPass" runat="server" visible="false">
                <div class="auto-form-wrapper">
                     <h1 style="margin-top:0; color:#1D74C0;">Create New Password</h1>
                <hr />
                    <div class="form-group">
                  <label class="label">Enter new Password</label>
                  <div class="input-group">
                    <asp:TextBox ID="txtnewpasswd" runat="server" TextMode="Password" CssClass="form-control"></asp:TextBox><asp:RequiredFieldValidator ID="RequiredFieldValidator2" ControlToValidate="txtnewpasswd" ValidationGroup="createnew" runat="server" ForeColor="Red" ErrorMessage="*"></asp:RequiredFieldValidator>
                    <div class="input-group-append">
                      <span class="input-group-text">
                        <i class="mdi mdi-check-circle-outline"></i>
                      </span>
                    </div>
                  </div>
                </div>
                    <div class="form-group">
                  <label class="label">Re-enter new Password</label>
                  <div class="input-group">
                    <asp:TextBox ID="txtconfirmpasswd" runat="server" TextMode="Password" CssClass="form-control"></asp:TextBox><asp:RequiredFieldValidator ID="RequiredFieldValidator3" ControlToValidate="txtconfirmpasswd" ValidationGroup="createnew" runat="server" ForeColor="Red" ErrorMessage="*"></asp:RequiredFieldValidator>
                    <div class="input-group-append">
                      <span class="input-group-text">
                        <i class="mdi mdi-check-circle-outline"></i>
                      </span>
                    </div>
                  </div>
                </div>
                    <div class="form-group">
                    <asp:Button ID="btnchange" runat="server" Text="Change Password" ValidationGroup="createnew" CssClass="btn btn-primary submit-btn btn-block" OnClick="btnchange_Click" />
                    
                </div>
                </div>
            </div>
         
            <div class="col-lg-4 mx-auto" id="NotAvailabel" runat="server" visible="false">
                <div class="auto-form-wrapper">
                  This link can no longer be used to complete<br />your request.<br /><br />
                  <asp:Button ID="btnlog" runat="server" PostBackUrl="~/Login.aspx" CausesValidation="false" CssClass="button" Text="Log in" />
                    </div>
              </div>

        </div>

       
      </div>
      <!-- content-wrapper ends -->
    </div>
    <!-- page-body-wrapper ends -->
  </div>
    </form>
</body>
</html>
