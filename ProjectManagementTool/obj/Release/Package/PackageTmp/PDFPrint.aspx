<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="PDFPrint.aspx.cs" Inherits="ProjectManagementTool.PDFPrint" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>PDF Print</title>
    <script src="/_assets/scripts/libs/jquery/jquery-3.5.1.min.js"></script>
    <script type="text/javascript">

  


        $(document).ready(function () {
            //alert("hi");
    //         var frame =  $('iframe1');
    //var contents =  frame.contents();
    //var body = contents.find('body').attr("oncontextmenu", "return false");
    //var body = contents.find('body').append('<div>New Div</div>');    

            var PDF = document.getElementById("iframe1");
            PDF.focus();
            PDF.contentWindow.print();
            
            });

       
        

    </script>
</head>
<body oncontextmenu="return false;">
    <form id="form1" runat="server">
        <div>
             <div id="PDFPreview" runat="server" style="max-height:95vh; overflow-y:auto; min-height:95vh;">
        <div class="row">
            <div class="col-sm-12">
                 <iframe width="100%" height="850" id="iframe1" runat="server"  oncontextmenu="return false;" style="pointer-events:none;">

                    </iframe>
                </div>
            </div>
   
        </div>
        </div>
    </form>
</body>
</html>
