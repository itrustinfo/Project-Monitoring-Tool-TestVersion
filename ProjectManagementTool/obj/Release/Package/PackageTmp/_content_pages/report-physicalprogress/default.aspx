<%@ Page Title="" Language="C#" MasterPageFile="~/_master_pages/default.Master" AutoEventWireup="true" CodeBehind="default.aspx.cs" Inherits="ProjectManagementTool._content_pages.report_physicalprogress._default" %>
<asp:Content ID="Content1" ContentPlaceHolderID="default_master_head" runat="server">
    <script type="text/javascript" src="https://www.gstatic.com/charts/loader.js"></script>

    <script type="text/javascript">
        function printdiv(printpage) {
                var frame = document.getElementById(printpage);
                 document.getElementById("btnActivityProgressPrint").style.display = "none";
    var data = frame.innerHTML;
    var win = window.open('', '', 'height=500,width=900');
    win.document.write('<style>@page{size:landscape;}</style><html><head><title></title>');
    win.document.write('</head><body >');
    win.document.write(data);
    win.document.write('</body></html>');
    win.print();
                win.close();
                document.getElementById("btnActivityProgressPrint").style.display = "block";
    return true;
            }
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="default_master_body" runat="server">
    <div class="container-fluid">
            <div class="row">
                <div class="col-md-12 col-lg-4 form-group">Project Physical Progress</div>
                <div class="col-md-6 col-lg-4 form-group">
                    <label class="sr-only" for="DDLProject">Project</label>
                    <div class="input-group">
                        <div class="input-group-prepend">
                            <span class="input-group-text">Project</span>
                        </div>
                        <asp:DropDownList ID="DDlProject" runat="server" CssClass="form-control" AutoPostBack="true" OnSelectedIndexChanged="DDlProject_SelectedIndexChanged"></asp:DropDownList>
                    </div>
                </div>
                <div class="col-md-6 col-lg-4 form-group">
                    <label class="sr-only" for="DDLWorkPackage">Work Package</label>
                    <div class="input-group">
                        <div class="input-group-prepend">
                            <span class="input-group-text">Work Package</span>
                        </div>
                        <asp:DropDownList ID="DDLWorkPackage" runat="server" CssClass="form-control" AutoPostBack="true" OnSelectedIndexChanged="DDLWorkPackage_SelectedIndexChanged"></asp:DropDownList>
                    </div>
                </div>
            </div>
        </div>
    <div class="container-fluid" >
            <div class="row">
                <div class="col-md-6 col-xl-4 mb-4" id="ReportFormat" runat="server">
                     <div class="card">
                        <div class="card-body">
                             <h6 class="card-title text-muted text-uppercase font-weight-bold">Report Format</h6>
                            <asp:RadioButtonList CssClass="form-control" ID="RBLReportFor" BorderStyle="None" runat="server" RepeatDirection="Horizontal" AutoPostBack="true" OnSelectedIndexChanged="RBLReportFor_SelectedIndexChanged">
                                <asp:ListItem Value="By Week">&nbsp;By Week</asp:ListItem>  
                                <asp:ListItem Value="By Month">&nbsp;By Month</asp:ListItem>             
                                <asp:ListItem Value="Across Months">&nbsp;Across Months</asp:ListItem>
                                <asp:ListItem Value="By Activity">&nbsp;By Activity</asp:ListItem>  
                        </asp:RadioButtonList>
                            </div>
                         </div>
                    </div>
                <div class="col-md-6 col-xl-8 mb-4" id="ByMonth" runat="server">
                     <div class="card">
                        <div class="card-body" >   
                            <div class="table-responsive">
                            <table style="width:80%;">
                              <tr>
                                  <td>
                                       <h6 class="card-title text-muted text-uppercase font-weight-bold">Select Month</h6>
                                  </td>
                              </tr>
                             <tr>
                                 <td>
                                        <asp:DropDownList ID="DDLYear" runat="server" CssClass="form-control">
                                        </asp:DropDownList>
                                 </td>
                                 <td>&nbsp;</td>
                                 <td>
                                        <asp:DropDownList ID="DDLMonth" runat="server" CssClass="form-control">
                                            <asp:ListItem Value="01">Jan</asp:ListItem>
                                            <asp:ListItem Value="02">Feb</asp:ListItem>
                                            <asp:ListItem Value="03">Mar</asp:ListItem>
                                            <asp:ListItem Value="04">Apr</asp:ListItem>
                                            <asp:ListItem Value="05">May</asp:ListItem>
                                            <asp:ListItem Value="06">Jun</asp:ListItem>
                                            <asp:ListItem Value="07">Jul</asp:ListItem>
                                            <asp:ListItem Value="08">Aug</asp:ListItem>
                                            <asp:ListItem Value="09">Sep</asp:ListItem>
                                            <asp:ListItem Value="10">Oct</asp:ListItem>
                                            <asp:ListItem Value="11">Nov</asp:ListItem>
                                            <asp:ListItem Value="12">Dec</asp:ListItem>
                                        </asp:DropDownList>
                                 </td>
                                 <td>&nbsp;</td>
                                 <td>
                                     <asp:Button ID="BntSubmit" runat="server" CssClass="btn btn-primary" Text="Submit" OnClick="BntSubmit_Click" />
                                 </td>
                             </tr>
                            </table>
                                </div>
                            </div>
                         </div>
                    </div>

                <div class="col-md-6 col-xl-8 mb-4" id="ByWeek" runat="server">
                     <div class="card">
                        <div class="card-body" >   
                            <div class="table-responsive">
                            <table style="width:70%;">
                              <tr>
                                  <td>
                                       <h6 class="card-title text-muted text-uppercase font-weight-bold">Select Week</h6>
                                  </td>
                              </tr>
                             <tr>
                                 <td>
                                        <asp:DropDownList ID="DDLWeek" runat="server" CssClass="form-control">
                                        </asp:DropDownList>
                                 </td>
                                 <td>&nbsp;</td>
                                 <td>
                                     <asp:Button ID="btnWeekSubmit" runat="server" CssClass="btn btn-primary" Text="Submit" OnClick="btnWeekSubmit_Click" />
                                 </td>
                             </tr>
                            </table>
                                </div>
                            </div>
                         </div>
                    </div>
                
                <div class="col-md-6 col-xl-8 mb-4" id="ByActivity" runat="server">
                     <div class="card">
                        <div class="card-body" >   
                            <div class="table-responsive">
                            <table style="width:100%;">
                              <tr>
                                  <td>
                                       <h6 class="card-title text-muted text-uppercase font-weight-bold">Select Activity</h6>
                                  </td>
                              </tr>
                             <tr>
                                 <td>
                                        <asp:DropDownList ID="DDLActivity" runat="server" CssClass="form-control" Width="60%" AutoPostBack="true" OnSelectedIndexChanged="DDLActivity_SelectedIndexChanged">
                                        </asp:DropDownList>
                                 </td>
                             </tr>
                            </table>
                                </div>
                            </div>
                         </div>
                    </div>
                </div>
        </div>

    <div class="container-fluid" id="MonthlyPhysicalProgress" runat="server">
        <div class="row">
            <div class="col-lg-12 col-xl-12 form-group">
            <div class="card mb-4">
                                    <div class="card-body">

                                        <div class="col-lg-12 col-xl-12 form-group" align="center">
                                            <h5 id="MonthlyProgressReportName" style="font-weight:bold;" runat="server">Monthly Progress Report</h5>
                                            <h5 id="MOnthlyProgressProjectName" runat="server">Project Name</h5>
                                            </div>

                                        <div class="card-title">
                                            <div class="d-flex justify-content-between">
                                                <h6 class="text-muted">
                                                    <%--<asp:Label ID="ActivityHeading" CssClass="text-uppercase font-weight-bold" runat="server" Text="Physical Progress Achievements in the month" />--%>
                                                </h6>

                                                <div>
                                                    <asp:Button ID="btnMonthlyProgressExporttoExcel" runat="server" Text="Export Excel" Visible="false" CssClass="btn btn-primary" OnClick="btnMonthlyProgressExporttoExcel_Click" />
                                                    <asp:Button ID="btnMonthlyProgressExportPDF" runat="server" Text="Export PDF" Visible="false" CssClass="btn btn-primary" OnClick="btnMonthlyProgressExportPDF_Click" />
                                                    <asp:Button ID="btnMonthlyProgressPrint" runat="server" Text="Print" Visible="false" CssClass="btn btn-primary" OnClick="btnMonthlyProgressPrint_Click" />
                                            </div>
                                            </div>
                                        </div>
                                        <div class="table-responsive">
                                                <asp:GridView ID="GrdMonthPhysicalProgress" runat="server" HeaderStyle-HorizontalAlign="Center" DataKeyNames="TaskUID" AutoGenerateColumns="False" EmptyDataText="No Data Found" Width="100%" AlternatingRowStyle-BackColor="LightGray" CssClass="table table-bordered" OnDataBound="GrdMonthPhysicalProgress_DataBound" OnRowDataBound="GrdMonthPhysicalProgress_RowDataBound">
                                                    <Columns>
                                                        <asp:TemplateField HeaderText="Sl.No" ItemStyle-HorizontalAlign="Center">
                                                            <ItemTemplate>
                                                                <%# Container.DataItemIndex + 1 %>
                                                            </ItemTemplate>
                                                            <ItemStyle HorizontalAlign="Center"></ItemStyle>
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="Description of Work" ItemStyle-HorizontalAlign="Left" HeaderStyle-CssClass="text-left">
                                                            <ItemTemplate>
                                                                <%#Eval("Name")%>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                       <%-- <asp:TemplateField HeaderText="UOM" ItemStyle-HorizontalAlign="Center">
                                                            <ItemTemplate>
                                                                <%#Eval("UnitforProgress")%>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>--%>
                                                        <asp:BoundField DataField="UnitforProgress" HeaderText="UOM" ItemStyle-HorizontalAlign="Center" />

                                                        <asp:BoundField DataField="UnitQuantity" HeaderText="Scope as per BOQ" ItemStyle-HorizontalAlign="Center" />
                                                        <%--<asp:TemplateField HeaderText="Scope as per BOQ" ItemStyle-HorizontalAlign="Center">
                                                            <ItemTemplate>
                                                                <%#Eval("UnitQuantity")%>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>--%>
                                                        <asp:BoundField DataField="TaskUID" HeaderText="Achieved" ItemStyle-HorizontalAlign="Center" />
                                                        <asp:BoundField DataField="TaskUID" HeaderText="Planned" ItemStyle-HorizontalAlign="Center" />
                                                        <asp:BoundField DataField="TaskUID" HeaderText="Achieved" ItemStyle-HorizontalAlign="Center" />
                                                        <asp:BoundField DataField="TaskUID" HeaderText="Achieved%" ItemStyle-HorizontalAlign="Center" />
                                                        <asp:BoundField DataField="TaskUID" HeaderText="Planned" ItemStyle-HorizontalAlign="Center" />
                                                        <asp:BoundField DataField="TaskUID" HeaderText="Achieved" ItemStyle-HorizontalAlign="Center" />
                                                        <asp:BoundField DataField="TaskUID" HeaderText="Balance" ItemStyle-HorizontalAlign="Center" />
                                                        <asp:BoundField DataField="TaskUID" HeaderText="Plan for the Next Month" ItemStyle-HorizontalAlign="Center" />
                                                        <asp:BoundField DataField="TaskUID" HeaderText="Overall % of Completion" ItemStyle-HorizontalAlign="Center" />
                                                        
                                                </Columns>
                                                </asp:GridView>
                                            </div>
                       </div>
                    </div>
                </div>
        </div>
        </div>

    <div class="container-fluid" id="WeeklyProgressReport" runat="server">
        <div class="row">
            <div class="col-lg-12 col-xl-12 form-group">
            <div class="card mb-4">
                                    <div class="card-body">
                                        <div class="col-lg-12 col-xl-12 form-group" align="center">
                                            <h5 id="WeeklyReportNameHeading" style="font-weight:bold;" runat="server">Contract Data</h5>
                                            <h5 id="WeeklyProjectName" runat="server">Contract Data</h5>
                                            </div>
                                        <div class="card-title">
                                            <div class="d-flex justify-content-between">
                                                <h6 class="text-muted">
                                                    <%--<asp:Label ID="LblWeeklyHeading" CssClass="text-uppercase font-weight-bold" runat="server" Text="Physical Progress Achievements in the month" />--%>
                                                </h6>
                                                <div>
                                                     <asp:Button ID="btnExportReportExcel" runat="server" Text="Export Excel" Visible="false" CssClass="btn btn-primary" OnClick="btnExportReportExcel_Click" />
                                                    <asp:Button ID="btnExportReportPDF" runat="server" Text="Export PDF" Visible="false" CssClass="btn btn-primary" OnClick="btnExportReportPDF_Click" />
                                                    <asp:Button ID="btnPrintPDF" runat="server" Text="Print" Visible="false" CssClass="btn btn-primary" OnClick="btnPrintPDF_Click" />
                                            </div>
                                            </div>
                                        </div>
                                        <div class="table-responsive">
                                                <asp:GridView ID="GrdWeeklyprogress" runat="server" HeaderStyle-HorizontalAlign="Center" DataKeyNames="TaskUID" AutoGenerateColumns="False" EmptyDataText="No Data Found" AlternatingRowStyle-BackColor="LightGray" Width="100%" CssClass="table table-bordered" OnDataBound="GrdWeeklyprogress_DataBound" OnRowDataBound="GrdWeeklyprogress_RowDataBound">
                                                    <Columns>
                                                        <asp:TemplateField HeaderText="Sl.No" ItemStyle-HorizontalAlign="Center">
                                                            <ItemTemplate>
                                                                <%# Container.DataItemIndex + 1 %>
                                                            </ItemTemplate>
                                                            <ItemStyle HorizontalAlign="Center"></ItemStyle>
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="Description of Work" ItemStyle-HorizontalAlign="Left" HeaderStyle-CssClass="text-left">
                                                            <ItemTemplate>
                                                                <%#Eval("Name")%>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="UOM" ItemStyle-HorizontalAlign="Center">
                                                            <ItemTemplate>
                                                                <%#Eval("UnitforProgress")%>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:BoundField DataField="TaskUID" HeaderText="Cumulative" ItemStyle-HorizontalAlign="Center" />
                                                        <asp:BoundField DataField="TaskUID" HeaderText="For the week" ItemStyle-HorizontalAlign="Center" />
                                                        <asp:BoundField DataField="TaskUID" HeaderText="Cumulative" ItemStyle-HorizontalAlign="Center" />
                                                        <asp:BoundField DataField="TaskUID" HeaderText="For the Week" ItemStyle-HorizontalAlign="Center" />
                                                        <asp:BoundField DataField="TaskUID" HeaderText="% of Progress Cumulative" ItemStyle-HorizontalAlign="Center" />
                                                </Columns>
                                                </asp:GridView>
                                            </div>
                       </div>
                    </div>
                </div>
        </div>
        </div>

    <div class="container-fluid" id="AcrossMonthsProgressReport" runat="server">
        <div class="row">
            <div class="col-lg-12 col-xl-12 form-group">
                <div class="card mb-4">
                                    <div class="card-body">

                                        <div class="col-lg-12 col-xl-12 form-group" align="center">
                                            <h5 id="AcrossMonthReportName" style="font-weight:bold;" runat="server">Physical Progress Monitoring Sheet</h5>
                                            <h5 id="AcrossMonthProjectName" runat="server">Projecct Name</h5>
                                            </div>
                                        <div class="card-title">
                                            <div class="d-flex justify-content-between">
                                                <h6 class="text-muted">
                                                    <%--<asp:Label ID="LblPhysicalProgress" CssClass="text-uppercase font-weight-bold" runat="server" Text="Physical Progress Monitoring Sheet" />--%>
                                                </h6>
                                                <div>
                                                    <asp:Button ID="btnAcrossMonthsExportExcel" runat="server" Visible="false" Text="Export to Excel" CssClass="btn btn-primary" OnClick="btnAcrossMonthsExportExcel_Click" />
                                                </div>
                                            </div>
                                        </div>
                                        <div class="table-responsive" id="DivAcrossMonths" runat="server">
                                                
                                          </div>
                       </div>
                    </div>
                </div>
            </div>
        </div>

    <div class="container-fluid" id="ActivityProgressReport" runat="server">
        <div class="row">
            <div class="col-lg-12 col-xl-12 form-group">
                <div class="card mb-4">
                                    <div class="card-body">
                                        <div class="col-lg-12 col-xl-12 form-group" align="center">
                                            <h4 id="ActivityProgressReportName" style="font-weight:bold; font-size:20px" runat="server">Physical Progress Monitoring Sheet</h4>
                                            <h5 id="ActivityProgressProjectName" runat="server">Project Name</h5>
                                            </div>
                                        <div class="card-title">
                                            <div class="d-flex justify-content-between">
                                                <h6 class="text-muted">
                                                    <%--<asp:Label ID="LblActivityPhysicalProgress" CssClass="text-uppercase font-weight-bold" runat="server" Text="Physical Progress" />--%>
                                                </h6>
                                                <div>
                                                    <asp:Button ID="btnActivityProgressPrint" runat="server" Text="Print Chart" Visible="true" OnClientClick="printdiv('default_master_body_ActivityProgressReport');" ClientIDMode="Static" CssClass="btn btn-primary"/>
                                            </div>
                                            </div>
                                        </div>
                                        <div class="table-responsive">
                                                <asp:Literal ID="ltScript_Progress" runat="server"></asp:Literal>
                                                <div id="chart_div" style="width:100%; height:400px;"></div>
                                            <br /><br /><br />
                                          </div>
                                        <div class="table-responsive">
                                             <div id="DivActivityProgressTabular" runat="server" style="width:70%; float:left;">
                            
                                            </div>
                                            </div>
                       </div>
                    </div>
                </div>
            </div>
        </div>
</asp:Content>
