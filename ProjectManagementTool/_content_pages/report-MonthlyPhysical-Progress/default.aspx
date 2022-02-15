<%@ Page Title="" Language="C#" MasterPageFile="~/_master_pages/default.Master" AutoEventWireup="true" CodeBehind="default.aspx.cs" Inherits="ProjectManagementTool._content_pages.report_MonthlyPhysical_Progress._default" %>
<asp:Content ID="Content1" ContentPlaceHolderID="default_master_head" runat="server">
     <style>
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


             
       
</script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="default_master_body" runat="server">
     <div class="container-fluid">
            <div class="row">
                <div class="col-md-6 col-lg-3 form-group">Monthly Physical Progress Report</div>
                <div class="col-md-6 col-lg-3 form-group">
                    <label class="sr-only" for="DDLProject">Project</label>
                    <div class="input-group">
                        <div class="input-group-prepend">
                            <span class="input-group-text">Project</span>
                        </div>
                        <asp:DropDownList ID="DDlProject" runat="server" CssClass="form-control" AutoPostBack="true" OnSelectedIndexChanged="DDlProject_SelectedIndexChanged"></asp:DropDownList>
                    </div>
                </div>
                <div class="col-md-6 col-lg-3 form-group">
                    <label class="sr-only" for="DDLWorkPackage">Work Package</label>
                    <div class="input-group">
                        <div class="input-group-prepend">
                            <span class="input-group-text">Work Package</span>
                        </div>
                        <asp:DropDownList ID="DDLWorkPackage" runat="server" CssClass="form-control"></asp:DropDownList>
                    </div>
                </div>
                <div class="col-md-6 col-lg-3 form-group"><asp:Button ID="btnSubmit" runat="server" CssClass="btn btn-primary" Text="Submit" OnClick="btnSubmit_Click" /></div>
            </div>
           <div class="row">
                <div class="col-md-12 col-lg-12 form-group">
                   <asp:RadioButtonList ID="rdSelect" runat="server" AutoPostBack="true" RepeatDirection="Horizontal" Visible="true" OnSelectedIndexChanged="rdSelect_SelectedIndexChanged">
                       <asp:ListItem Selected="True"  Value="0">&nbsp;Tabular Form&nbsp;</asp:ListItem>
                      <asp:ListItem Value="1">&nbsp;Graphical Form (%)&nbsp;</asp:ListItem>
                    </asp:RadioButtonList>
                  
                </div>
                  </div>
            
        </div>
     <div class="container-fluid" id="divProgresschart" runat="server" visible="false">
            <div class="row">
                <div class="col-lg-12 col-xl-12 form-group">
                <div class="card mb-4">
                    <div class="card-body">
                        <div class="table-responsive">
                            <div class="row">
                           <div class="col-md-6 col-lg-10 form-group" align="center"><h4 id="heading" runat="server" style="font-size:20px">Physical Progress Chart</h4></div>
                            <div class="col-md-6 col-lg-2 form-group" align="right"><asp:Button ID="btnPrint" runat="server" Text="Print Chart" Visible="true" OnClientClick="printdiv('default_master_body_divProgresschart');" ClientIDMode="Static" CssClass="btn btn-primary"/></div></div>
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
         <div class="container-fluid" id="divTabular" runat="server" visible="false">
            <div class="row">
                <div class="col-lg-12 col-xl-12 form-group">
                <div class="card mb-4">
                    <div class="card-body">
                        <h5 id="headingTb" runat="server" style="text-align:center">Physical Progress Chart</h5>
                        <div class="table-responsive">
                             <div class="text-right">
                                 <asp:Button ID="btnexcelexport" runat="server" Text="Export to Excel" CssClass="btn btn-primary" OnClick="btnexcelexport_Click" />
                                <asp:Button ID="btnexport" runat="server" Text="Export to PDF" CssClass="btn btn-primary" OnClick="btnexport_Click" />
                                 <asp:Button ID="btnPrintPDF" runat="server" Text="Print" CssClass="btn btn-primary" OnClick="btnPrintPDF_Click" />
                            </div><br />
                           <asp:GridView ID="GrdPhysicalData" runat="server" HeaderStyle-HorizontalAlign="Center" CssClass="table table-bordered" EmptyDataText="No Data" AutoGenerateColumns="False" Width="100%" AlternatingRowStyle-BackColor="lightGray" OnDataBound="GrdPhysicalData_DataBound">
                                <Columns>
                                      <asp:BoundField DataField="Months" ItemStyle-HorizontalAlign="Center" HeaderText="Months" >
                           
                            </asp:BoundField>
                                     <asp:BoundField DataField="ProjectedPer" ItemStyle-HorizontalAlign="Right" HeaderText="As per Target" HtmlEncode="False" DataFormatString="{0:C2}">
                            
                            </asp:BoundField>
                                     <asp:BoundField DataField="AchievedPer" HeaderText="Achieved" DataFormatString="{0:C2}" ItemStyle-HorizontalAlign="Right">
                            
                            </asp:BoundField>
                                    <asp:BoundField DataField="CumulativeProjected" ItemStyle-HorizontalAlign="Right" HeaderText="As per Target" HtmlEncode="False" DataFormatString="{0:C2}">
                            
                            </asp:BoundField>
                                     <asp:BoundField DataField="CumulativeAchieved" HeaderText="Achieved" DataFormatString="{0:C2}" ItemStyle-HorizontalAlign="Right">
                            
                            </asp:BoundField>
                                </Columns>
                                </asp:GridView>
                        </div>
                        </div>
                    </div>

                </div>
                </div>
            </div>
</asp:Content>
