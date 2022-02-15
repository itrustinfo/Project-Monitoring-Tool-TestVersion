    <%@ Page Title="" Language="C#" MasterPageFile="~/_master_pages/default.Master" AutoEventWireup="true" CodeBehind="default.aspx.cs" Inherits="ProjectManager._content_pages.dashboard" %>
    <asp:Content ID="content_head" ContentPlaceHolderID="default_master_head" runat="server">
        <style type="text/css">
               .div {
  padding-top: 0px;
  padding-right: 200px;
  padding-bottom: 0px;
  padding-left: 10px;
  width:100% !important;
  overflow-x:scroll;
  overflow-y: hidden;
}

              

               .chklist{
                   padding-left :10px;
               }

              .container {
         /*  position:fixed;*/
         
   
            width:100%; 
            height:100%;
            overflow:hidden;
        }
        .container img {
          position:absolute;
             
    top:0; 
    left:0; 
    right:0; 
    bottom:0; 
           /* margin:auto; */
           /* min-width:100%;
            min-height:100%;*/
            overflow: hidden;
        }



            #chart_div .google-visualization-tooltip {  position:relative !important; top:0 !important;right:0 !important; z-index:+1;} 
        </style>
        <script type="text/javascript" src="https://www.gstatic.com/charts/loader.js"></script>
        <script type="text/javascript">
          

            function printdiv(printpage) {
                var frame = document.getElementById(printpage);
                 document.getElementById("btnPrint").style.display = "none";
    var data = frame.innerHTML;
    var win = window.open('', '', 'height=500,width=900');
    win.document.write('<style>@page{size:landscape;}</style><html><head><title></title>');
    win.document.write('</head><body >');
    win.document.write(data);
    win.document.write('</body></html>');
    win.print();
                win.close();
                document.getElementById("btnPrint").style.display = "block";
    return true;
            }


             function printdivFin(printpage) {
                var frame = document.getElementById(printpage);
                 document.getElementById("btnPrintFin").style.display = "none";
    var data = frame.innerHTML;
    var win = window.open('', '', 'height=500,width=900');
    win.document.write('<style>@page{size:landscape;}</style><html><head><title></title>');
    win.document.write('</head><body >');
    win.document.write(data);
    win.document.write('</body></html>');
    win.print();
                win.close();
                document.getElementById("btnPrintFin").style.display = "block";
    return true;
             }

            function GetDetails() {
                  var value = document.getElementById("<%=DDlProject.ClientID%>");  
                var getvalue = value.options[value.selectedIndex].value;
                 // var valuew = document.getElementById("<%=DDLWorkPackage.ClientID%>");  
               // var getvaluew = value.options[valuew.selectedIndex].value;
                if (getvalue != '--Select--') {
                    PageMethods.GetDetails('test', OnSuccess);
                }
            }

            function OnSuccess(response) {
                //alert(response);
                  var value = document.getElementById("<%=DDlProject.ClientID%>");  
                var getvalue = value.options[value.selectedIndex].value;
                 
                var getvaluew = response.split("$")[1];
                var username = '<%= Session["UserUID"] %>';

                var divmain = document.getElementById("<%=divUsersdocs.ClientID%>");
                divmain.innerHTML = 'You have <a id="Hluserdocs">following pending documents</a> to act on';
                //alert(divmain.innerHTML);
                
                document.getElementById("Hluserdocs").innerHTML = response.split("$")[0] + ' pending documents';
                document.getElementById("Hluserdocs").href = "/_content_pages/documents-contractor/?&type=Ontb&PrjUID=" + getvalue + "&UserUID=" + username + "&WkpgUID=" + getvaluew;
            }
            window.onload = GetDetails;
       
</script>
       
    </asp:Content>
    <asp:Content ID="content_body" ContentPlaceHolderID="default_master_body" runat="server">
        <%--project selection dropdowns--%>
        <%--<div id="loader"></div>  --%>
     <asp:ScriptManager ID="smMain" runat="server" EnablePageMethods="true" />
        <div class="container-fluid">
            <div class="row">
                <div class="col-md-12 col-lg-12 form-group" id="divUsersdocs" runat="server" style="background-color:orange;color:white;font-size:medium;text-align:center;align-content:center">Please wait processing the documents.....</div>
                <div class="col-md-12 col-lg-4 form-group">Dashboard</div>
                <div class="col-md-6 col-lg-4 form-group">
                    <label class="sr-only" for="DDLProject">Project</label>
                    <div class="input-group">
                        <div class="input-group-prepend">
                            <span class="input-group-text">Project</span>
                        </div>
                        <asp:DropDownList ID="DDlProject" runat="server" CssClass="form-control" AutoPostBack="true" OnSelectedIndexChanged="DDlProject_SelectedIndexChanged"></asp:DropDownList>
                       <%-- <select class="form-control" id="DDlProject" runat="server">
                           
                        </select>--%>
                    </div>
                </div>
                <div class="col-md-6 col-lg-4 form-group">
                    <label class="sr-only" for="DDLWorkPackage">Work Package</label>
                    <div class="input-group">
                        <div class="input-group-prepend">
                            <span class="input-group-text">Work Package</span>
                        </div>
                        <asp:DropDownList ID="DDLWorkPackage" runat="server" CssClass="form-control" AutoPostBack="true" OnSelectedIndexChanged="DDLWorkPackage_SelectedIndexChanged"></asp:DropDownList>

                        <%--<select class="form-control" id="DDLWorkPackage" runat="server">
                        </select>--%>
                    </div>
                </div>
            </div>
        </div>
            <div id="divdashboardimage" runat="server" visible="false">
         <div class="container-fluid" style="opacity: 0.9 !important;background-color:none;font-weight:800">
        <div class="row">
          <div class="col-lg-6 col-xl-12 form-group">
                        <div class="card">
                            <div class="card-body" style="padding-bottom:0; margin-bottom:0;">
                                <div class="row" align="center">
                                    <%-- <div class="col-lg-1">
                            <h6 class="card-title text-muted text-uppercase font-weight-bold">Chart : </h6>

                        </div>--%>
                        <div class="col-lg-12" >
                            <asp:RadioButtonList ID="rdSelect" runat="server" class="card-title text-muted text-uppercase font-weight-bold text-center" AutoPostBack="true" OnSelectedIndexChanged="rdSelect_SelectedIndexChanged" RepeatDirection="Horizontal">
                       <asp:ListItem Selected="True" Value="0">&nbsp;Without Progress Chart&nbsp;</asp:ListItem>
                       <asp:ListItem Value="1">&nbsp;With Physical Progress Chart&nbsp;</asp:ListItem>
                       <asp:ListItem Value="2">&nbsp;With Financial Progress Chart&nbsp;</asp:ListItem>
                                 <asp:ListItem Value="3">&nbsp;NJSEI Project MIS&nbsp;</asp:ListItem>
                    </asp:RadioButtonList>

                        </div>                     
                                </div>
                            </div>
                        </div>
                       
                     </div>  
            </div>
    </div>

        <div class="container-fluid" id="DivSyncedData" runat="server">
        <div class="row">
          <div class="col-lg-6 col-xl-12 form-group">
                        <div class="card">
                            <div class="card-body mb-3" style="padding-bottom:0; margin-bottom:0;">
                                <div class="row">
                        <div class="col-lg-12" style="text-align:center;">
                          <b class="card-title text-muted text-uppercase">Last Documents Synced Date : </b>
                            <asp:Label ID="LblLastSyncedDate" runat="server" Font-Bold="true" ForeColor="Green" Text="2021-09-03 14:34:05.847"></asp:Label>&nbsp;&nbsp;&nbsp;&nbsp;
                            <b class="card-title text-muted text-uppercase"><asp:Label ID="LblSourceHeading" runat="server"></asp:Label>  </b>
                           <%-- <asp:Label ID="LblTotalSourceDocuments" runat="server" Font-Bold="true" ForeColor="Green" Text="500"></asp:Label>&nbsp;&nbsp;&nbsp;&nbsp;
                            <b class="card-title text-muted text-uppercase"><asp:Label ID="LblDestinationHeading" runat="server"></asp:Label> : </b>
                            <asp:Label ID="LblTotalDestinationDocuments" runat="server" Font-Bold="true" ForeColor="Green" Text="500"></asp:Label>--%>
                        </div>                     
                                </div>
                            </div>
                        </div>
                       
                     </div>  
            </div>
    </div>

        <%--dashboard cards--%>
         <div class="container-fluid" id="divProgresschart" runat="server" visible="false" style="opacity: 0.9 !important">
            <div class="row">
                <div class="col-lg-12 col-xl-12 form-group">
                <div class="card mb-4">
                    <div class="card-body">
                        <div class="table-responsive">
                            <div class="row">
                           <div class="col-md-6 col-lg-6 form-group" align="center"><h4 id="heading" runat="server" style="font-size:20px">Physical Progress Chart</h4></div>
                            <div class="col-md-6 col-lg-6 form-group" align="right"><asp:Button ID="btnPrint" runat="server" Text="Print Chart" Visible="true" OnClientClick="printdiv('default_master_body_divProgresschart');" ClientIDMode="Static"/></div></div>
                                 <asp:Literal ID="ltScript_PhysicalProgress" runat="server"></asp:Literal>
                                  <div id="chart_divProgress" style="width:100%; height:300px;">
                                     
                                  </div><br /><br /><br />
                            <div id="divtable" runat="server" class="div">
                            
                                </div>
                        </div>
                        </div>
                    </div>

                </div>
            </div>
        </div>
         <div class="container-fluid" id="divFinProgressChart" runat="server" visible="false" style="opacity: 0.9 !important">
            <div class="row">
                <div class="col-lg-12 col-xl-12 form-group">
                <div class="card mb-4">
                    <div class="card-body">
                        <div class="table-responsive">
                            <div class="row">
                           <div class="col-md-6 col-lg-6 form-group" align="center"><h4 id="headingF" runat="server" style="font-size:20px">Financial Progress Chart</h4></div>
                            <div class="col-md-6 col-lg-6 form-group" align="right"><asp:Button ID="btnPrintFin" runat="server" Text="Print Chart" Visible="true" OnClientClick="printdivFin('default_master_body_divFinProgressChart');" ClientIDMode="Static"/></div></div>
                                 <asp:Literal ID="ltScript_FinProgress" runat="server"></asp:Literal>
                                  <div id="chart_divProgressFin" style="width:100%; height:300px;">
                                     
                                  </div><br /><br /><br />
                            <div id="divtableFin" runat="server" class="div">
                            
                                </div>
                        </div>
                        </div>
                    </div>

                </div>
            </div>
        </div>
          
        <div id="divMainblocks" runat="server" class="container-fluid" style="opacity: 0.9 !important;background-color:none;font-weight:800">
            <div class="row">
                  <div class="col-md-6 col-xl-4 mb-4">
                    <div class="card h-100">
                        <div class="card-body">
                          
                           
                                <asp:RadioButtonList ID="RdList" runat="server" RepeatDirection="Horizontal"  class="card-title text-muted text-uppercase font-weight-bold" AutoPostBack="true" OnSelectedIndexChanged="RdList_SelectedIndexChanged">
                                    <asp:ListItem Value="Progress" Selected="True">&nbsp;Progress Chart&nbsp;&nbsp;</asp:ListItem>
                                    <asp:ListItem Value="Cost">&nbsp;Cost Chart</asp:ListItem>                      
                                </asp:RadioButtonList>
                                <%--<h6 class="card-title text-muted text-uppercase font-weight-bold">Progress</h6>--%>
                                  
                                 <asp:Literal ID="ltScript_Progress" runat="server" Visible="true"></asp:Literal>
                                  <div id="chart_div" style="width:100%; height:275px; overflow-y: auto; overflow-x:scroll;"></div>
                               
                        </div>
                    </div>
                </div>
                
                <div class="col-md-6 col-xl-4 mb-4">
                    <div class="card h-100">
                        <div class="card-body">
                            <h6 class="card-title text-muted text-uppercase font-weight-bold">Status of Design,Drawings & Documentation </h6>
                            <%--<p class="card-text"></p>--%>
                            <asp:Literal ID="ltScript_Document" runat="server"></asp:Literal>
                      <div id="DocChart_Div" style="width:100%; height:275px;"></div>
                            </div>
                    </div>
                </div>
              <div id="divsyncdetails" runat="server" visible="false" class="col-md-6 col-xl-4 mb-4">
                    <div class="card h-100">
                        <div class="card-body" style="font-weight:900">
                            <h6 class="card-title text-muted text-uppercase font-weight-bold" style="color:forestgreen !important">Synchronization Details</h6>
                            <table style="width:100%;line-height:30px">
                                 <tr><td><a href="#" id="hlContractor" runat="server">Documents</a></td><td><asp:Label ID="lblContractorTo" runat="server" Text="Contractor->ONTB"></asp:Label></td><td><asp:Label ID="lblContractorToNo" runat="server" Text="0"></asp:Label></td></tr>
                                <tr><td><a href="#" id="hlONTB" runat="server">Documents</a></td><td><asp:Label ID="lblONTBTo" runat="server" Text="ONTB->Contractor"></asp:Label></td><td><asp:Label ID="lblONTBTo_No" runat="server" Text="0"></asp:Label></td></tr>
                                 <tr><td><a href="#" id="hlMeasurement" runat="server">Measurments</a></td><td><asp:Label runat="server" Text="Contractor->ONTB"></asp:Label></td><td><asp:Label ID="lblMeasurements" runat="server" Text="0"></asp:Label></td></tr>
                                 <tr><td><a href="#" id="hlRABills" runat="server">RA Bills</a></td><td><asp:Label  runat="server" Text="Contractor->ONTB"></asp:Label></td><td><asp:Label ID="lblRABills" runat="server" Text="0"></asp:Label></td></tr>
                                 <tr><td><a href="#" id="hlInvoices" runat="server">Invoices</a></td><td><asp:Label runat="server" Text="Contractor->ONTB"></asp:Label></td><td><asp:Label ID="lblInvoices" runat="server" Text="0"></asp:Label></td></tr>
                                 <tr><td><a href="#" id="hlBankGuarantee" runat="server">Bank Guarantee</a></td><td><asp:Label runat="server" Text="Contractor->ONTB"></asp:Label></td><td><asp:Label ID="lblBankG" runat="server" Text="0"></asp:Label></td></tr>
                                 <tr><td><a href="#" id="hlInsurance" runat="server">Insurance</a></td><td><asp:Label runat="server" Text="Contractor->ONTB"></asp:Label></td><td><asp:Label ID="lblInsurance" runat="server" Text="0"></asp:Label></td></tr>
                            </table>
                        </div>
                    </div>  
                </div>
               <div class="col-md-6 col-xl-4 mb-4">
                    <div class="card h-100">
                        <div class="card-body">
                            <h6 class="card-title text-muted text-uppercase font-weight-bold">Issues</h6>
                            <asp:Literal ID="ltScripts_piechart" runat="server"></asp:Literal>
                        <div id="piechart_3d" style="width:100%; height:275px;"></div>
                        </div>
                    </div>
                </div>
          
                <div class="col-md-6 col-xl-4 mb-4" style="display:none">
                    <div class="card h-100">
                        <div class="card-body">
                            <%--<h6 class="card-title text-muted text-uppercase font-weight-bold">Resource</h6>--%>
                             <div class="input-group">
                                <div class="input-group-prepend">
                                    <span class="input-group-text">Resource</span>
                                </div>
                                <asp:DropDownList ID="DDlResource" runat="server" CssClass="form-control" AutoPostBack="true" OnSelectedIndexChanged="DDlResource_SelectedIndexChanged"></asp:DropDownList>
                              
                            </div>
                           <asp:Literal ID="ltScript_Resource" runat="server"></asp:Literal>
                            <div id="Resource_div" style="width:100%; height:275px; margin-top:10px;"></div>
                        </div>
                    </div>
                </div>
                    <div class="col-md-6 col-xl-4 mb-4" id="divCamera" runat="server">
                    <div class="card h-100">
                        <div class="card-body">
                            <%--<h6 class="card-title text-muted text-uppercase font-weight-bold">IP Camera</h6>--%>
                            <div class="input-group" style="margin-bottom:8px;">
                                <div class="input-group-prepend">
                                    <span class="input-group-text">IP Camera</span>
                                </div>
                                <asp:DropDownList ID="DDLCamera" runat="server" CssClass="form-control"></asp:DropDownList>
                              
                            </div>
                            
                            <h6 id="camera" style="display:none;font-weight:800">As you have denied access to the camera. Image will not be streamed to the dashboard</h6>
                            <video autoplay="true" id="videoElement" height="275">

                     </video>
                     <script>
                         var video = document.querySelector("#videoElement");

                         if (navigator.mediaDevices.getUserMedia) {
                             navigator.mediaDevices.getUserMedia({ video: true })
                                     .then(function (stream) {
                                         video.srcObject = stream;
                                         document.getElementById("camera").style.display = "none";
                                     })
                                     .catch(function (err0r) {
                                         //console.log("Something went wrong!");
                                         //alert(err0r);
                                         //alert('As you have denied access to the camera. Image will not be streamed to the dashboard. Press OK to continue');
                                          document.getElementById("camera").style.display = "block";
                                     });
                         }
        </script>
                        </div>
                    </div>
                </div>
                 <div class="col-md-6 col-xl-4 mb-4" style="display:none">
                    <div class="card h-100">
                        <div class="card-body">
                            <%--<h6 class="card-title text-muted text-uppercase font-weight-bold">Resource</h6>--%>
                             <div class="input-group">
                                <div class="input-group-prepend">
                                    <span class="input-group-text">Images</span>
                                </div>
                                
                              
                            </div><br />
                            <div class="container">
                                <img src="../../_assets/images/dashboard1.jpg" width="100%";height="100%" style="display:block"/>
                                <img src="../../_assets/images/dashboard2.jpg" width="100%";height="100%" style="display:none"/>
                            </div>
                          
                        </div>
                    </div>
                </div>
                    <div class="col-md-6 col-xl-4 mb-4">
                    <div class="card h-100">
                        <div class="card-body" style="font-weight:800">
                            <h6 class="card-title text-muted text-uppercase font-weight-bold">Alerts</h6>
                            <asp:Panel ID="Panel1" runat="server" BorderStyle="None" 
            BorderWidth="3" Width="100%">
 <marquee direction="up" behavior="slide" onmouseover="this.stop()" onmouseout="this.start()"
scrolldelay="100" style="height:275px; width:100%;">
<asp:Literal ID="lt1" runat="server"></asp:Literal></marquee>
        </asp:Panel>
                        </div>
                    </div>  
                </div>
               

            </div>
        </div>
       </div>

          <div id="divdummydashboard" runat="server" visible="false">
         

       

        <%--dashboard cards--%>
        
       
        <div class="container-fluid" style="opacity: 0.9 !important;background-color:none;font-weight:800">
            <div class="row">
                  <div class="col-md-6 col-xl-4 mb-4">
                    <div class="card h-100">
                        <div class="card-body">
                          
                           
                                <asp:RadioButtonList ID="RadioButtonList2" runat="server" RepeatDirection="Horizontal"  class="card-title text-muted text-uppercase font-weight-bold" AutoPostBack="true" OnSelectedIndexChanged="RadioButtonList2_SelectedIndexChanged">
                                    <asp:ListItem Value="Progress" Selected="True">&nbsp;Progress Chart&nbsp;&nbsp;</asp:ListItem>
                                    <asp:ListItem Value="Cost">&nbsp;Cost Chart</asp:ListItem>                      
                                </asp:RadioButtonList>
                                <%--<h6 class="card-title text-muted text-uppercase font-weight-bold">Progress</h6>--%>
                                   <div id="divdummyblock1" runat="server" class="container" visible="true">
                                <img id="imgd1" src="../../_assets/images/block1.jpg" width="100%";height="100%" style="display:block"/>
                                        
                               
                            </div>
                            <div id="divdummyblock1_1" runat="server" class="container" visible="false">
                                
                                        <img id="imgd2" src="../../_assets/images/block1_1.jpg" width="100%";height="100%" style="display:block"/>
                               
                            </div>
                                
                               
                        </div>
                    </div>
                </div>
                
                <div class="col-md-6 col-xl-4 mb-4">
                    <div class="card h-100">
                        <div class="card-body">
                            <h6 class="card-title text-muted text-uppercase font-weight-bold">Status of Design,Drawings & Documentation </h6>
                            <%--<p class="card-text"></p>--%>
                           <div class="container">
                                <img src="../../_assets/images/block2.jpg" width="100%";height="100%" style="display:block"/>
                               
                            </div>
                            </div>
                    </div>
                </div>
                <div class="col-md-6 col-xl-4 mb-4">
                    <div class="card h-100">
                        <div class="card-body" style="font-weight:800">
                            <h6 class="card-title text-muted text-uppercase font-weight-bold">Alerts</h6>
                            <div class="container">
                                <img src="../../_assets/images/block3.jpg" width="100%";height="100%" style="display:block"/>
                               
                            </div>
                        </div>
                    </div>  
                </div>
               <div class="col-md-6 col-xl-4 mb-4">
                    <div class="card h-100">
                        <div class="card-body">
                            <h6 class="card-title text-muted text-uppercase font-weight-bold">Issues</h6>
                           <div class="container">
                                <img src="../../_assets/images/block4.jpg" width="100%";height="100%" style="display:block"/>
                               
                            </div>
                        </div>
                    </div>
                </div>
          
              
                    <div class="col-md-6 col-xl-4 mb-4" id="div7" runat="server">
                    <div class="card h-100">
                        <div class="card-body">
                            <%--<h6 class="card-title text-muted text-uppercase font-weight-bold">IP Camera</h6>--%>
                            <div class="input-group" style="margin-bottom:8px;">
                                <div class="input-group-prepend">
                                    <span class="input-group-text">IP Camera</span>
                                </div>
                                <asp:DropDownList ID="DropDownList2" runat="server" CssClass="form-control"></asp:DropDownList>
                              
                            </div>
                            <div class="container">
                                <img src="../../_assets/images/cp25.jpg" width="100%";height="100%" style="display:block"/>
                               
                            </div>
                           

                    
                   
                        </div>
                    </div>
                </div>
               

            </div>
        </div>
       </div>
        <div id="dummyNJSEIdashboard" runat="server" visible="true">
             <div class="container-fluid">
            <div class="row">
                <div class="col-lg-12 col-xl-12 form-group">
                <div class="card mb-12">
                    <div class="card-body">
                        
                        <div class="table-responsive">
                            <br /><br />
                            <div class="container">
                                <img src="../../_assets/images/njsei-banner-image4.jpg" width="100%";height="100%" style="display:block"/>
                                            <img src="../../_assets/images/njsei-banner-image3.jpg" width="100%";height="100%" style="display:block"/>
                                 <img src="../../_assets/images/njsei-banner-image2.jpg" width="100%";height="100%" style="display:block"/>
                                <img src="../../_assets/images/njsei-banner-image.jpg" width="100%";height="100%" style="display:block"/>
                              
                    
                                
                            </div>
                            
                        </div>
                        </div>
                    </div>

                </div>
            </div>
        </div>
        </div>
            <div id="dummyONTBdashboard" runat="server" visible="false">
             <div class="container-fluid">
            <div class="row">
                <div class="col-lg-12 col-xl-12 form-group">
                <div class="card mb-12">
                    <div class="card-body">
                        <div class="table-responsive">
                         
                                <img src="../../_assets/images/ONTB_map.jpg" width="100%";height="100%" style="display:block"/>
                               
                            
                        </div>
                        </div>
                    </div>

                </div>
            </div>
        </div>
        </div>
        <div class="container-fluid" id="divNJSEIMIS" runat="server" visible="false" style="opacity: 0.9 !important">
            <div class="row">
                <div class="col-lg-12 col-xl-12 form-group">
                <div class="card mb-12">
                    <div class="card-body">
                        <div class="table-responsive">
                            <asp:RadioButtonList ID="RadioButtonList1" runat="server" RepeatDirection="Horizontal"  class="card-title text-muted text-uppercase font-weight-bold" AutoPostBack="true">
                                    <asp:ListItem Value="Progress" Selected="True">&nbsp;Progress Chart&nbsp;&nbsp;</asp:ListItem>
                                    <asp:ListItem Value="Cost">&nbsp;Cost Chart</asp:ListItem>                      
                                </asp:RadioButtonList>
                             <h4>  Under Implementation</h4>
                               
                            
                        </div>
                        </div>
                    </div>

                </div>
            </div>
        </div>
    </asp:Content>
