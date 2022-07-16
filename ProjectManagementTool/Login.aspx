<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Login.aspx.cs" Inherits="ProjectManagementTool.Login" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
      <title>Login</title>
     <meta charset="utf-8">
  <meta name="viewport" content="width=device-width, initial-scale=1, shrink-to-fit=no">
  <link href="_assets/images/favicon.png" rel="icon" />

    <link rel="stylesheet" href="_assets/styles/style.css" />

</head>
<body>
                <div class="container-scroller">
    <div class="container-fluid page-body-wrapper full-page-wrapper auth-page">
      <div class="content-wrapper d-flex align-items-center auth auth-bg-1 theme-one">
        <div class="row w-100">
            
          <div class="col-lg-5 mx-auto">
              
            <div class="auto-form-wrapper">
                <%--<div style="text-align:center;" id="NJSEI" runat="server">
                    <img src="_assets/images/NJSEI Logo.jpg" alt="NJSEI" />
                    <span class="font-weight-bold" style="vertical-align:middle; color:#0521AC; font-size:xx-large;">NJSEI</span>
                </div>--%>
                <div style="text-align:center;">
                    <asp:Image ID="sLogo" runat="server" />
                    <span class="font-weight-bold" style="vertical-align:middle; color:#0521AC; font-size:xx-large;"><asp:Label ID="LblTitle" runat="server"></asp:Label></span>
                </div>

                <h5 style="text-align:center; margin-top:10px;"><asp:Label ID="LblDescription" runat="server"></asp:Label></h5>
                <br />
                <h3 style="margin-top:0; color:#0e31ce;" >Login here</h3>
                <hr />
              <form id="form2" runat="server">
                <div class="form-group">
                  <label style="font-family:Segoe UI, Tahoma, Geneva, Verdana, 'sans-serif';font-size:larger">Username</label>
                  <div class="input-group">
                    <input type="text" class="form-control" id="txtusername" autocomplete="off" runat="server" required placeholder="Username" style="font-family:Segoe UI, Tahoma, Geneva, Verdana, 'sans-serif';font-size:larger"/>
                    <div class="input-group-append">
                      <span class="input-group-text">
                        <i class="mdi mdi-check-circle-outline"></i>
                      </span>
                    </div>
                  </div>
                </div>
                <div class="form-group">
                  <label style="font-family:Segoe UI, Tahoma, Geneva, Verdana, 'sans-serif';font-size:larger" >Password</label>
                  <div class="input-group">
                    <input type="password"  class="form-control" id="txtpassword" runat="server" required placeholder="password" style="font-family:Segoe UI, Tahoma, Geneva, Verdana, 'sans-serif';font-size:larger"/>
                    <div class="input-group-append">
                      <span class="input-group-text">
                        <i class="mdi mdi-check-circle-outline"></i>
                      </span>
                    </div>
                  </div>
                </div>
                <div class="form-group">
                    <asp:Button ID="btnLogin" runat="server" Text="Login" BackColor="#0521AC" CssClass="btn btn-primary submit-btn btn-block" OnClick="btnLogin_Click" />
                    <%--<button class="btn btn-primary submit-btn btn-block">Login</button>--%>
                </div>
                <div class="form-group d-flex justify-content-between">
                  <div class="form-check form-check-flat mt-0">
                    
                      <asp:Label ID="lblMsg" runat="server" ForeColor="Red" Visible="False"></asp:Label>
                    
                  </div>
                  <a href="ForgotPassword.aspx" style="color:#0e31ce;" class="text-black" >Forgot Password</a>
                </div>
              
              </form>
            </div>
        
            <%--<p class="footer-text text-center">copyright © 2018 Bootstrapdash. All rights reserved.</p>--%>
          </div>
        </div>
      </div>
      <!-- content-wrapper ends -->
    </div>
    <!-- page-body-wrapper ends -->
  </div>
</body>
</html>
