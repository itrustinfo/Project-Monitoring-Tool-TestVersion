<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="WebForm1.aspx.cs" Inherits="ProjectManagementTool.WebForm1" %>

<%--<%@ Register Assembly="System.Web.DataVisualization, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35" Namespace="System.Web.UI.DataVisualization.Charting" TagPrefix="asp" %>--%>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <style type="text/css">
        body {
            margin:0;
            padding:0;
            font-family:Arial;
            font-size:12px;

        }
        .lblcss {
            text-transform:uppercase;
            font-size:8px;
        }
       
    </style>
     <script type="text/javascript" src="https://www.gstatic.com/charts/loader.js"></script>
    <script type="text/javascript">
      google.charts.load('current', {'packages':['corechart']});
      google.charts.setOnLoadCallback(drawVisualization);

      function drawVisualization() {
        // Some raw data (not necessarily accurate)
        var data = google.visualization.arrayToDataTable([
          ['Month', 'Bolivia', 'Ecuador', 'Madagascar', 'Average'],
          ['2004/05',  165,      938,         522,       614.6],
          ['2005/06',  135,      1120,        599,       682],
          ['2006/07',  157,      1167,        587,       623],
          ['2007/08',  139,      1110,        615,       609.4],
          ['2008/09',  136,      691,         629,       569.6]
        ]);

        var options = {
          title : 'Monthly Coffee Production by Country',
          vAxis: {title: 'Cups'},
          hAxis: {title: 'Month'},
          seriesType: 'bars',
            series: { 2: { type: 'line' },3: { type: 'line' } }

        };

        var chart = new google.visualization.ComboChart(document.getElementById('chart_div'));
        chart.draw(data, options);
      }
    </script>
    <%--<script src="_assets/scripts/jquery.min.js"></script>--%>
    <%--<script>
        var counter;
        function UploadFile() {
            var files = $("#<%=file1.ClientID%>").get(0).files;
            counter = 0;
            // Loop through files
            for (var i = 0; i < files.length ; i++) {
                //var file = files[i];
                //var formdata = new FormData();
                //formdata.append("file1", file);
                var ajax = new XMLHttpRequest();

                ajax.upload.addEventListener("progress", progressHandler, false);
                ajax.addEventListener("load", completeHandler, false);
                ajax.addEventListener("error", errorHandler, false);
                ajax.addEventListener("abort", abortHandler, false);
                //ajax.open("POST", "FileUploadHandler.ashx");
                //ajax.send(formdata);
            }
        }
        function progressHandler(event) {
            $("#loaded_n_total").html("Uploaded " + event.loaded + " bytes of " + event.total);
            var percent = (event.loaded / event.total) * 100;
            $("#progressBar").val(Math.round(percent));
            $("#status").html(Math.round(percent) + "% uploaded... please wait");
        }
        function completeHandler(event) {
            counter++
            $("#status").html(counter + " " + event.target.responseText);
        }
        function errorHandler(event) {
            $("#status").html("Upload Failed");
        } function abortHandler(event) {
            $("#status").html("Upload Aborted");
        }
    </script>--%>

   <%-- <style>  
        table, th, td {  
    border: 1px solid black; 
    padding:6px;
}  
    </style>  --%>
   
</head>
<body>
    <form id="form1" runat="server">
         <div style="float:left;width:100%;  margin-bottom:50px;">
            <div style="float:left;width:50%;">
                <asp:Label ID="LblDateTime1" runat="server" Font-Bold="true"></asp:Label><br />
                <asp:Button ID="btnSubmit1" runat="server" Text="Get DateTime" OnClick="btnSubmit1_Click" />
                </div>
             <div style="float:left;width:50%;">
                 <asp:Label ID="LblTime" runat="server" Font-Bold="true"></asp:Label><br />
                <asp:Button ID="btnSubmit2" runat="server" Text="Get Time" OnClick="btnSubmit2_Click" />
                </div>
        </div>
        <br /><br /> <br /><br />
        <div id="chart_div" style="width: 900px; height: 500px; "></div>
        <div style="float:left; width:100%;">
            <table style="width:80%;">
                <tr>
                    <td></td>
                    <td></td>
                    <td>1. Supply of MS Bare Pipes</td>
                    <td>2. Supply of Coated MS Pipes</td>
                    <td>3. Valves delivery at Site</td>
                </tr>
                <tr>
                    <td>Units</td>
                    <td></td>
                    <td>M</td>
                    <td>M</td>
                    <td>M</td>
                </tr>
                <tr>
                    <td>Bill of Quantity</td>
                    <td></td>
                    <td>53427</td>
                    <td>53427</td>
                    <td>139</td>
                </tr>
                <tr>
                    <td>Rev. Scope</td>
                    <td></td>
                    <td>53227</td>
                    <td>53227</td>
                    <td>120</td>
                </tr>
            </table>
        </div>

        <asp:Label ID="LblProjectname" runat="server"  ></asp:Label>
        <%--<asp:Chart ID="Chart1" runat="server">
                                <Series>
                                    <asp:Series Name="Touches" ChartType="Column" >
                                        <Points>
                                            <asp:DataPoint YValues="20" XValue="5"  />
                                            <asp:DataPoint YValues="10" XValue="10" />
                                            <asp:DataPoint YValues="30" XValue="30"/>
                                        </Points>
                                    </asp:Series>
                                    <asp:Series Name="column1" ChartType="Column" >
                                        <Points>
                                            <asp:DataPoint YValues="5" XValue="5"  />
                                            <asp:DataPoint YValues="10" XValue="10" />
                                            <asp:DataPoint YValues="25" XValue="30"/>
                                        </Points>
                                    </asp:Series>
                                    <asp:Series Name="Goal" ChartType="Line">
                                        <Points>
                                            <asp:DataPoint XValue="5"    YValues="7"  />
                                            <asp:DataPoint XValue="10"    YValues="18" />
                                            <asp:DataPoint XValue="30" YValues="10" />
                                        </Points>
                                    </asp:Series>
                                    <asp:Series Name="Goal1" ChartType="Line">
                                        <Points>
                                            <asp:DataPoint XValue="5"    YValues="5" />
                                            <asp:DataPoint XValue="10"    YValues="20" />
                                            <asp:DataPoint XValue="25" YValues="12" />
                                        </Points>
                                    </asp:Series>
                                </Series>
                                <ChartAreas>
                                    <asp:ChartArea Name="ChartArea1"></asp:ChartArea>
                                </ChartAreas>
                            </asp:Chart>--%>
         <asp:Button ID="btnexporttoPDF" runat="server" Text="Export" OnClick="btnexporttoPDF_Click" />
       
      <%--  <div>
    <asp:Button ID="btnAdd" runat="server" Text="Add" OnClick="AddTextBox" />
    <asp:Button ID="btnRemove" runat="server" Text="Remove" OnClick="RemoveTextBox" />
    <br />
    <asp:Panel ID="txtPanel" runat="server">
    </asp:Panel>

            
</div>--%>
       <%-- <div style="float:left; width:100%; height:450px;">
             <iframe width="100%" height="450" src="https://www.edifyschoolchikkabanavara.com/">
                    </iframe>
        </div>
        <br /><br /><br /><br />
        <div style="float:left; width:100%; height:450px; margin-top:25px;">
             <iframe width="100%" height="450" src="https://njspm.itrustinfo.com/_PreviewLoad/NJSEI_H152_1_download.pdf">
                    </iframe>
        </div>--%>
       <%-- <div style='width:60%; float:left; padding:1%; border:2px solid #011496; border-radius:5px;'>
            <div style='float:left; width:100%; border-bottom:2px solid #011496;'>
                <div style='float:left; width:7%;'>
                    <img src='_assets/images/NJSEI Logo.jpg' width='50' />
            </div>
                <div style='float:left; width:60%;'>
                    <h2 style='margin-top:10px; '>Project Monitoring Tool</h2>
            </div>
            </div>
            
            
            
            <div style='width:100%; float:left;'><br/>Dear, All<br/><br/><span style='font-weight:bold;'>Ravi Shankar N added a new Submittal. Below are the details,</span> <br/><br/></div>
            <div style='width:100%; float:left;'>
                <table style="width:100%;">
                    <tr>
                        <td>
                            <b>Workpackage </b>
                        </td>
                        <td style='text-align:center;'><b>:</b></td>
                        <td>
                            30 mld teritiary treatment plant
                        </td>
                    </tr>
                    <tr>
                        <td>
                           <b>Activity Name </b>
                        </td>
                        <td style='text-align:center;'><b>:</b></td>
                        <td>
                            Civil works
                        </td>
                    </tr>
                    <tr>
                        <td>
                           <b> Submittal Name </b>
                        </td>
                        <td style='text-align:center;'><b>:</b></td>
                        <td>
                            Civil Works
                        </td>
                    </tr>
                    <tr>
                        <td>
                          <b>  Submittal Category </b>
                        </td>
                        <td style='text-align:center;'><b>:</b></td>
                        <td>
                            A. General Drawing - Planning Drawings
                        </td>
                    </tr>
                    <tr>
                        <td>
                           <b> Submitted By </b>
                        </td>
                        <td style='text-align:center;'><b>:</b></td>
                        <td>
                            Ravi Shankar N
                        </td>
                    </tr>
                    <tr>
                        <td>
                          <b>  Date</b>
                        </td>
                        <td style='text-align:center;'><b>:</b></td>
                        <td>
                            10/4/2020
                        </td>
                    </tr>
                </table>
            </div>
                                    <div style='width:100%; float:left;'><br/><br/>Regards, <br/> Project Monitoring Tool.</div>
            </div>
        <div class="wrapper">
            <asp:FileUpload ID="file1" runat="server" AllowMultiple="true" /><br />
            <input type="button" value="Upload File" onclick="UploadFile()" />
            <progress id="progressBar" value="0" max="100" style="width: 300px;"></progress>
            <h3 id="status"></h3>
            <p id="loaded_n_total"></p>
        </div>--%>

        <%--<div style="width:100%; margin:auto;" id="ExportDiv" runat="server">
            
            <div style="width:100%; float:left;" align='center'>
                <asp:Label ID="LblNJSEI" runat="server" CssClass="lblcss" Font-Bold="true">NJSEI</asp:Label><br /><br />
                <asp:Label ID="Label1" runat="server" CssClass="lblcss">Report Name: Document History</asp:Label><br /><br />
                <asp:Label ID="Label2" runat="server" CssClass="lblcss">Start Date : 01 Jul 2020   End Date : 30 Jul 2020</asp:Label><br /><br />
                <asp:Label ID="Label3" runat="server" CssClass="lblcss">Project Name : HUDA 30 MLD</asp:Label>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                <asp:Label ID="Label4" runat="server" CssClass="lblcss">Workpackage : 30 MLD teritory treatment plant</asp:Label><br /><br />
                <asp:Label ID="Label5" runat="server" CssClass="lblcss">Report Format : By Date</asp:Label>
                </div>
        </div>
        <div style="float:left; width:100%;">
         <asp:Button id="btnexport" runat="server" Text="Export" OnClick="btnexport_Click" />
        </div>--%>
    </form>
</body>
</html>
