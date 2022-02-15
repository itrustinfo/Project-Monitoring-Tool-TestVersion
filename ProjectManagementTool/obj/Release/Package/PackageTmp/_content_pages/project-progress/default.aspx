<%@ Page Title="" Language="C#" MasterPageFile="~/_master_pages/default.Master" AutoEventWireup="true" CodeBehind="default.aspx.cs" Inherits="ProjectManagementTool._content_pages.project_progress._default" %>
<asp:Content ID="Content1" ContentPlaceHolderID="default_master_head" runat="server">
     <link rel="stylesheet" href="//code.jquery.com/ui/1.12.1/themes/base/jquery-ui.css">
  <script src="https://code.jquery.com/jquery-1.12.4.js"></script>
  <script src="https://code.jquery.com/ui/1.12.1/jquery-ui.js"></script>

    <script>
  $( function() {
    $("input[id$='dtReviewDate']").datepicker({
      changeMonth: true,
        changeYear: true,
        dateFormat:'dd/mm/yy'
      });

    });
  </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="default_master_body" runat="server">
    <div class="container-fluid">
            <div class="row">
                <div class="col-md-12 col-lg-3 form-group">Project Status of Work Progress</div>
               
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
                    <label class="sr-only" for="DDLProject">Review Meeting</label>
                    <div class="input-group">
                        <div class="input-group-prepend">
                            <span class="input-group-text">Review Meeting</span>
                        </div>
                        <asp:DropDownList ID="DDLMeetingMaster" runat="server" CssClass="form-control"></asp:DropDownList>
                    </div>
                </div>
                <div class="col-md-6 col-lg-1 form-group">
                    <asp:HiddenField ID="HiddenDate" runat="server" />
                <asp:Button ID="btnSubmit" runat="server" CssClass="btn btn-primary" Text="Submit" OnClick="btnSubmit_Click" />
                    </div>
               <%-- <div class="col-md-6 col-lg-4 form-group">
                    <label class="sr-only" for="DDLWorkPackage">Work Package</label>
                    <div class="input-group">
                        <div class="input-group-prepend">
                            <span class="input-group-text">Work Package</span>
                        </div>
                        <asp:DropDownList ID="DDLWorkPackage" runat="server" CssClass="form-control"></asp:DropDownList>
                    </div>
                </div>--%>
            </div>
        </div>
<%--     <div class="container-fluid">
            <div class="row">
                <div class="col-md-6 col-xl-12 mb-4">
                     <div class="card">
                        <div class="card-body">
                            
                             <h6 class="card-title text-muted text-uppercase font-weight-bold">Select Date</h6>
                            
                            
                           
                            </div>
                         </div>
                    </div>
                </div>
         </div>--%>
    <%--<table class="table-borderless">
                                <tr>
                                    <td>
                                        <asp:TextBox ID="dtReviewDate" runat="server" CssClass="form-control" autocomplete="off" placeholder="dd/mm/yyyy" required ClientIDMode="Static"></asp:TextBox>
                                    </td>
                                   
                                    <td>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;</td>
                                    <td>
                                        <asp:Button ID="btnSubmit" runat="server" CssClass="btn btn-primary" Text="Submit" OnClick="btnSubmit_Click" />
                                    </td>
                                </tr>
                            </table>--%>
                             
                            <%-- <td>
                                        <asp:DropDownList ID="DDLMonth" runat="server" CssClass="form-control" Width="250" required>
                                 <asp:ListItem Value="">--Month--</asp:ListItem>
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
                                    <td>&nbsp;&nbsp;</td>
                                    <td>
                                         <asp:DropDownList ID="DDLYear" runat="server" CssClass="form-control" Width="250" required>
                                </asp:DropDownList>
                                    </td>--%>
    <div class="container-fluid" id="ProjectProgress" runat="server">
        <div class="row" >
            <div class="col-lg-12 col-xl-12 form-group">
                <div class="card mb-4">
                    <div class="card-body" >
                         <div class="card-title">
                            <asp:Label ID="LblContractorName" CssClass="text-uppercase font-weight-bold" runat="server" Text="Contractor Details"  />
                        </div>
                        <div class="table-responsive">
                             <table class="table table-bordered">
                                 <tr>
                                     <td>
                                         <b>Name of the Contractor</b>
                                     </td>
                                     <td>
                                         <asp:Label ID="LblNameoftheContractor" runat="server"></asp:Label>
                                     </td>
                                 </tr>
                                 <tr>
                                     <td>
                                         <b><asp:Label ID="LblCostName" runat="server"></asp:Label></b>
                                     </td>
                                     <td>
                                         <asp:Label ID="LblTotalCost" runat="server"></asp:Label>
                                     </td>
                                 </tr>
                                 <tr>
                                     <td>
                                         <b>Date of issue of LOA</b>
                                     </td>
                                     <td>
                                         <asp:Label ID="LblDateofLOA" runat="server"></asp:Label>
                                     </td>
                                 </tr>
                                 <tr>
                                     <td>
                                         <b>Date of signing of Agreement</b>
                                     </td>
                                     <td>
                                         <asp:Label ID="LblDateofAgreement" runat="server"></asp:Label>
                                     </td>
                                 </tr>
                                 <tr>
                                     <td>
                                         <b>Date of Commencement</b>
                                     </td>
                                     <td>
                                         <asp:Label ID="LblDateofCommencement" runat="server"></asp:Label>
                                     </td>
                                 </tr>
                                 <tr>
                                     <td>
                                         <b>Date of Completion</b>
                                     </td>
                                     <td>
                                         <asp:Label ID="LblDateofCompletion" runat="server"></asp:Label>
                                     </td>
                                 </tr>
                                 <tr>
                                     <td>
                                         <b>Period of Completion</b>
                                     </td>
                                     <td>
                                         <asp:Label ID="LblPeriodofCompletion" runat="server"></asp:Label>
                                     </td>
                                 </tr>
                             </table>
                         </div>
                        <hr />
                        <div class="card-title">
                            <asp:Label ID="LblPprojectprogress" CssClass="text-uppercase font-weight-bold" runat="server" Text="Project Status of Work Progress"  />
                        </div>
                         
                        <div class="table-responsive" id="ConstructionProgrammeDiv" runat="server">
                            <asp:GridView ID="GrdProjectProgress" runat="server" EmptyDataText="No Records Found." Font-Size="10pt" Width="100%" CssClass="table table-bordered" AutoGenerateColumns="False" OnRowDataBound="GrdProjectProgress_RowDataBound">
                            <Columns>
                                <asp:TemplateField HeaderText="Sl. No." ItemStyle-HorizontalAlign="Left" HeaderStyle-Font-Bold="true">
                                    <ItemTemplate>
                                        <%# Container.DataItemIndex + 1 %>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Activities" HeaderStyle-Font-Bold="true">
                                    <ItemTemplate>
                                        <%#Eval("Activity")%>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:BoundField DataField="Target" HeaderText="Target as on Submitted Construction Programme" HeaderStyle-Font-Bold="true" />
                                <asp:BoundField DataField="Achieved" HeaderText="Achieved as on " HeaderStyle-Font-Bold="true" />
                                <asp:TemplateField HeaderText="% Percentage" HeaderStyle-Font-Bold="true">
                                    <ItemTemplate>
                                        <%#Eval("percentage")%>%
                                    </ItemTemplate>
                                </asp:TemplateField>
                            </Columns>
                            </asp:GridView>

                            <%--<asp:GridView ID="GrdProjectProgress" runat="server" EmptyDataText="No Records Found." Font-Size="10pt" Width="100%" CssClass="table table-bordered" DataKeyNames="TaskUID" AutoGenerateColumns="False" OnRowDataBound="GrdProjectProgress_RowDataBound">
                            <Columns>
                                <asp:TemplateField HeaderText="Sl. No." ItemStyle-HorizontalAlign="Left" HeaderStyle-Font-Bold="true">
                                    <ItemTemplate>
                                        <%# Container.DataItemIndex + 1 %>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Activities" HeaderStyle-Font-Bold="true">
                                    <ItemTemplate>
                                        <%#GetTaskName(Eval("TaskUID").ToString())%>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:BoundField DataField="TargetValue" HeaderText="Target as on Submitted Construction Programme" HeaderStyle-Font-Bold="true" />
                                <asp:BoundField DataField="AchievedValue" HeaderText="Achieved as on " HeaderStyle-Font-Bold="true" />
                                <asp:BoundField DataField="TaskUID" HeaderText="% Percentage" HeaderStyle-Font-Bold="true" />
                            </Columns>
                            </asp:GridView>--%>
                         </div>

                        <div class="table-responsive" id="ProjectWorkProgressDiv" runat="server">
                            <asp:GridView ID="GrdWorkProgress" runat="server" EmptyDataText="No Records Found." Font-Size="10pt" Width="100%" CssClass="table table-bordered" AutoGenerateColumns="False">
                            <Columns>
                                <asp:TemplateField HeaderText="Sl. No." ItemStyle-HorizontalAlign="Left" HeaderStyle-Font-Bold="true">
                                    <ItemTemplate>
                                        <%# Container.DataItemIndex + 1 %>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:BoundField DataField="Activity" HeaderText="List of Activities" HeaderStyle-Font-Bold="true" />
                                <asp:BoundField DataField="status" HtmlEncode="False" HeaderText="Status of Work Progress as on date" HeaderStyle-Font-Bold="true" />
                            </Columns>
                            </asp:GridView>
                         </div>

                        <asp:Label ID="LblEmptyWorkProgress" runat="server" Text="No Project Status of Work Progress Found.." Visible="false"></asp:Label>

                        <hr />
                        <div class="card-title">
                            <asp:Label ID="LblSitePhotograph" CssClass="text-uppercase font-weight-bold" runat="server" Text="Site PhotoGraphs"  />
                        </div>

                        <div class="table-responsive">
                            <asp:DataList ID="GrdSitePhotograph" runat="server" RepeatColumns="2" HorizontalAlign="Center" CellPadding="10" RepeatDirection="Horizontal">
                                <ItemTemplate>
                                    <div style="width:350px; float:left; border:1px solid Gray; text-align:center; background-color:#f2f2f2;">
                                        <div style="padding:10px;">
                                            <%--<img src='http://localhost:50162<%#Eval("Site_Image").ToString().Replace("~","")%>' alt="" width="250" />--%>
                                            <asp:Image ID="imgEmp" runat="server" Width="300px" ImageUrl='<%# Bind("Site_Image", "{0}")%>' /><br />
                                    <b><%#Eval("Description")%></b>
                                        </div>
                                    </div>
                                    </ItemTemplate>
                            </asp:DataList>
                            <asp:Label ID="lblEmptyData" runat="server" Text="No Site Photographs Found.." Visible="false"></asp:Label>
                            </div>
                          <div class="text-right">
                            <asp:Button ID="btnExportPDF" runat="server" Text="Export to PDF" CausesValidation="false" CssClass="btn btn-primary" OnClick="btnExportPDF_Click" />
                                    </div>
                        </div>
                    </div>
                </div>
            </div>
        </div>
</asp:Content>
