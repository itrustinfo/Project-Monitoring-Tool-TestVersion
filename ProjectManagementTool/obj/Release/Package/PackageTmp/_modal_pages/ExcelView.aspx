<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ExcelView.aspx.cs" Inherits="ProjectManagementTool._content_pages.ExcelView" %>
 
  
<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <style type="text/css">
        .aclass th {
            background:Red; font-weight:bold; color: white
        }

        .aclass{border: 1px solid black;}
.aclass tr td {background:green; font-weight:bold; color: white}
.aclass
{
    overflow :auto;
}
   .tabpanlecss{
      overflow :scroll;
   }

  


    </style>
</head>
<body>
  
    <form id="form1" runat="server">
          <asp:scriptmanager runat="server"></asp:scriptmanager>
        <div id="divExcel" runat="server">
           
        
            <ajaxToolkit:TabContainer ID="TabContainer1" runat="server" ActiveTabIndex="0">
           
             </ajaxToolkit:TabContainer>
        
             <br />
        </div>
        <div id="divWorddoc" runat="server">
        <div id="dvWord" runat="server"></div>
          <%--  <iframe  runat="server" id="iframhtml" height="800" width="100%" ></iframe>--%>
        </div>
    </form>
</body>
</html>
