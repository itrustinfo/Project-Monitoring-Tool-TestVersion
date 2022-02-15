<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="download-document.aspx.cs" Inherits="ProjectManagementTool.download_document" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="content-type" content="text/html; charset=UTF-8"/>
    <meta http-equiv="X-UA-Compatible" content="IE=edge"/>
    <meta name="viewport" content="width=device-width, initial-scale=1"/>
    <meta name="description" content="i-Trust - Monitoring Tool"/>
    <title>Project Monitoring Tool</title>
     <link href="~/_assets/images/favicon.png" rel="icon" />
    <link href="~/_assets/styles/app.min.css" rel="stylesheet" />
</head>
<body>
    <form id="form1" runat="server">
        <div class="app_wrap">
            <div class="app_body">
                <nav class="navbar it-navbar navbar-expand-xl navbar-dark bg-dark">
	                <a class="navbar-brand" href="/_content_pages/dashboard" title="Project Manager">
                        <span class="navbar-brand--logo">
                            <i id="ONTBLogo" runat="server" class="far fa-dot-circle"></i>
                            <img id="NJSEILogo" runat="server" src="/_assets/images/NJSEI Logo.jpg" alt="NJSEI" width="40" />
                        </span>
                        <span class="navbar-brand--name">
                            <%--<span class="brand">i-Trust</span>--%>
                            <span id="NJSEI" runat="server" style="font-size:12px;" class="brand">NJSEI</span>
                            <span id="ONTB" runat="server" style="font-size:12px;" class="brand">ONTB-BWSSB Stage 5</span>
                            <span class="product">Project Monitoring Tool</span>
                        </span>
	                </a>
                    <button aria-controls="headerNavbar" aria-expanded="false" aria-label="Toggle navigation" class="navbar-toggler" data-target="#headerNavbar" data-toggle="collapse" type="button"><span class="navbar-toggler-icon"></span></button>
                     <div class="collapse navbar-collapse" id="headerNavbar">
		                
	                </div>
                </nav>
                <div class="app_main">
                    <div class="app_main__content" style="text-align:center;">
                        <h3 id="LblMessage" runat="server">Please wait.. Download is in Progress..</h3>
                    </div>
                    <footer class="app_footer container-fluid">
                        <div class="row">
                            <div class="app_footer__copyright col-md-6 py-2">&copy; 2020 i-Trust Informatics</div>
                            <ul class="app_footer__menu col-md-6 py-2 text-md-right">
                                <li class="app_footer__menu__item">
                                    <a class="app_footer__menu__item__text" href="#" title="About">About</a>
                                </li>
                                <li class="app_footer__menu__item">
                                    <a class="app_footer__menu__item__text" href="#" title="">Legals</a>
                                </li>
                                <li class="app_footer__menu__item">
                                    <a class="app_footer__menu__item__text" href="#" title="Contact">Contact</a>
                                </li>
                            </ul>
                        </div>
                    </footer>
                </div>
            </div>

        </div>
    </form>
</body>
</html>
