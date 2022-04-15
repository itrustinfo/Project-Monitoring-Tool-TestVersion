<%@ Page Title="" Language="C#" MasterPageFile="~/_master_pages/default.Master" AutoEventWireup="true" EnableEventValidation="false" CodeBehind="Default.aspx.cs" Inherits="ProjectManager._content_pages.reports.Default" %>
<asp:Content ID="Content1" ContentPlaceHolderID="default_master_head" runat="server">
    <link rel="stylesheet" href="//code.jquery.com/ui/1.12.1/themes/base/jquery-ui.css">
  <script src="https://code.jquery.com/jquery-1.12.4.js"></script>
  <script src="https://code.jquery.com/ui/1.12.1/jquery-ui.js"></script>

     <script>
  $( function() {
    $("input[id$='dtStartDate']").datepicker({
      changeMonth: true,
        changeYear: true,
      dateFormat:'dd/mm/yy'
      });

      $("input[id$='dtEndDate']").datepicker({
      changeMonth: true,
        changeYear: true,
      dateFormat:'dd/mm/yy'
      });

    });
  </script>
    <style type="text/css">
        .lblalign {
            text-align:left;
        }
    </style>
   <%-- <script type="text/javascript">
        $(document).ready(function () {
            $('#loader').fadeOut();
        });
    </script>--%>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="default_master_body" runat="server">
    <%--<div id="loader"></div>--%>
           <%--project selection dropdowns--%>
        <div class="container-fluid">
            <div class="row">
                <div class="col-md-12 col-lg-4 form-group">Document Status Report</div>
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
                        <asp:DropDownList ID="DDLWorkPackage" runat="server" CssClass="form-control"></asp:DropDownList>
                    </div>
                </div>
            </div>
        </div>
        <div class="container-fluid" id="ReportFormat" runat="server">
            <div class="row">
                <div class="col-md-6 col-xl-4 mb-4">
                     <div class="card">
                        <div class="card-body">
                             <h6 class="card-title text-muted text-uppercase font-weight-bold">Report Format</h6>
                            <asp:RadioButtonList CssClass="form-control" ID="RBLReportFor" BorderStyle="None" runat="server" RepeatDirection="Horizontal" AutoPostBack="true" OnSelectedIndexChanged="RBLReportFor_SelectedIndexChanged">
                                <asp:ListItem Value="By Summary">&nbsp;By Summary</asp:ListItem>
                                <asp:ListItem Value="By Originator">&nbsp;By Originator</asp:ListItem>
                                <asp:ListItem Value="By Date">&nbsp;By Date</asp:ListItem>
                                <%--<asp:ListItem Value="By Status">&nbsp;By Status</asp:ListItem>--%>
                        </asp:RadioButtonList>
                            </div>
                         </div>
                    </div>
                <div class="col-md-6 col-xl-8 mb-4" id="ByDate" runat="server">
                     <div class="card">
                        <div class="card-body" >   
                            <div class="table-responsive">
                            <table style="width:85%;">
                                <tr>
                                    <td>
                                         <h6 class="card-title text-muted text-uppercase font-weight-bold">Date Range</h6>
                                    </td>
                                </tr>
                             <tr><td>

                                 <asp:Label ID="lblstartdate" runat="server" CssClass="lblCss">Start Date</asp:Label>
                                 </td><td>
                                 <asp:TextBox ID="dtStartDate" runat="server" CssClass="form-control" autocomplete="off" placeholder="dd/mm/yyyy" ClientIDMode="Static"></asp:TextBox>
                     </td><td>&nbsp;</td><td><asp:Label ID="lblenddate" runat="server" CssClass="lblCss">End Date</asp:Label></td><td>
                         <asp:TextBox ID="dtEndDate" runat="server" CssClass="form-control" autocomplete="off" placeholder="dd/mm/yyyy" ClientIDMode="Static"></asp:TextBox>
                     </td>
                                 <td>&nbsp;</td><td><asp:Button ID="btnSubmit" runat="server" CssClass="btn btn-primary" Text="Submit" OnClick="btnSubmit_Click" /></td>
                             </tr>
                        </table>
                                </div>
                            </div>
                         </div>
                    </div>
                <div class="col-md-6 col-xl-8 mb-4" id="ByOriginator" runat="server">
                     <div class="card">
                        <div class="card-body" >   
                            <div class="table-responsive">
                            <table style="width:90%;">
                                <tr>
                                    <td>
                                         <h6 class="card-title text-muted text-uppercase font-weight-bold">Originator</h6>
                                    </td>
                                    <td>&nbsp;</td>
                                    <td><h6 class="card-title text-muted text-uppercase font-weight-bold">Discipline</h6></td>
                                    <td>&nbsp;</td>
                                </tr>
                             <tr><td style="width:45%;">
                                 <asp:DropDownList ID="DDLOriginator" runat="server" CssClass="form-control" required></asp:DropDownList>
                            </td>
                                 <td>&nbsp;</td>
                                 <td style="width:45%;">
                                     <asp:DropDownList ID="DDLDocumentCategory" runat="server" CssClass="form-control" required></asp:DropDownList>
                                 </td>
                                 <td>&nbsp;</td>
                                 <td style="width:10%;">
                                     <asp:Button ID="BtnOriginatorSubmit" runat="server" Text="Submit" CssClass="btn btn-primary" OnClick="BtnOriginatorSubmit_Click" />
                                 </td>
                             </tr>
                        </table>
                                </div>
                            </div>
                         </div>
                    </div>
                </div>
        </div>
        <div class="container-fluid" id="DocumentSummary" runat="server">
            <div class="row" style="text-align:center;">
                <div class="col-lg-12 col-xl-12" style="text-align:left;">
                    <br />
                    <h6 id="HeadingSummary" runat="server" class="text-muted text-uppercase font-weight-bold">
                                   Document Summary
                               </h6>
                    </div>
                <div class="col-md-6 col-xl-2 mb-2">
                      <div class="card">
                          <div class="card-body">
                               <b>Total Documents</b><br /><b style="color:#444444;"><asp:Label ID="LblTotalDocuments" runat="server">0</asp:Label></b>
                         </div>
                     </div>
                </div>
                <div class="col-md-6 col-xl-2 mb-2">
                      <div class="card">
                          <div class="card-body">
                               <b>Submitted Documents</b><br /><b style="color:#444444;"><asp:Label ID="LblSubmitted" runat="server">0</asp:Label></b>
                         </div>
                     </div>
                </div>
                <div class="col-md-6 col-xl-2 mb-2">
                      <div class="card">
                          <div class="card-body">
                               <b>Code A</b><br /><b style="color:#444444;"><asp:Label ID="LblCodeA" runat="server">0</asp:Label></b>
                         </div>
                     </div>
                </div>
                <div class="col-md-6 col-xl-2 mb-2">
                      <div class="card">
                          <div class="card-body">
                               <b>Code B</b><br /><b style="color:#444444;"><asp:Label ID="LblCodeB" runat="server">0</asp:Label></b>
                         </div>
                     </div>
                </div>
                 <div class="col-md-6 col-xl-2 mb-2">
                      <div class="card">
                          <div class="card-body">
                               <b>Code C</b><br /><b style="color:#444444;"><asp:Label ID="LblCodeC" runat="server">0</asp:Label></b>
                         </div>
                     </div>
                </div>
                <div class="col-md-6 col-xl-2 mb-2">
                      <div class="card">
                          <div class="card-body">
                               <b>Code D</b><br /><b style="color:#444444;"><asp:Label ID="LblCodeD" runat="server">0</asp:Label></b>
                         </div>
                     </div>
                </div>
                <div class="col-md-6 col-xl-2 mb-2">
                      <div class="card">
                          <div class="card-body">
                               <b>Code E</b><br /><b style="color:#444444;"><asp:Label ID="LblCodeE" runat="server">0</asp:Label></b>
                         </div>
                     </div>
                </div>
                <div class="col-md-6 col-xl-2 mb-2">
                      <div class="card">
                          <div class="card-body">
                               <b>Code F</b><br /><b style="color:#444444;"><asp:Label ID="LblCodeF" runat="server">0</asp:Label></b>
                         </div>
                     </div>
                </div>
                <div class="col-md-6 col-xl-2 mb-2">
                      <div class="card">
                          <div class="card-body">
                               <b>Code G</b><br /><b style="color:#444444;"><asp:Label ID="LblCodeG" runat="server">0</asp:Label></b>
                         </div>
                     </div>
                </div>
                <div class="col-md-6 col-xl-2 mb-2">
                      <div class="card">
                          <div class="card-body">
                               <b>Code H</b><br /><b style="color:#444444;"><asp:Label ID="LblCodeH" runat="server">0</asp:Label></b>
                         </div>
                     </div>
                </div>
                <div class="col-md-6 col-xl-2 mb-4">
                      <div class="card">
                          <div class="card-body">
                               <b>Client Approved</b><br /><b style="color:#444444;"><asp:Label ID="LblClientApproved" runat="server">0</asp:Label></b>
                         </div>
                     </div>
                </div>
               
            </div>
        </div>
        <div class="container-fluid" id="ByDateGrid" runat="server">
        <div class="row">
            <div class="col-lg-12 col-xl-12 form-group">
                <%--<h6 id="HeadingbyDatewise" runat="server" class="text-muted text-uppercase font-weight-bold">
                                   Document List by Date
                               </h6>--%>
                <div class="card mb-4" >
                    <div class="card-body" >
                        <div class="col-lg-12 col-xl-12 form-group" align="center">
                                    <h5 id="DocumentByDateReportName" style="font-weight:bold;" runat="server">Report Name</h5>
                                    <h5 id="DocumentByDateProjectName" runat="server">Project Name</h5>
                                    </div>
                        <div class="card-title">
                            <div class="d-flex justify-content-between">
                            <h6 class="text-muted">
                                   <%--<asp:Label ID="ActivityHeading" CssClass="text-uppercase font-weight-bold" runat="server" Text="Contract Details" />--%>
                               </h6>
                                   <div class="text-right">
                                    <asp:Button ID="btnExportExcel" runat="server" Text="Export to Excel" CssClass="btn btn-primary" OnClick="btnExportExcel_Click" />&nbsp;
                                    <asp:Button ID="btnExportPDF" runat="server" Text="Export to PDF" CssClass="btn btn-primary" OnClick="btnExportPDF_Click" />
                                    </div>
                            </div>
                            </div>
                        <div class="table-responsive" id="GridDiv" runat="server">
                            <asp:GridView ID="GrdDocumentMaster" runat="server" EmptyDataText="No Documents Found." Width="100%" CssClass="table table-bordered" DataKeyNames="DocumentUID" ShowHeader="False" AutoGenerateColumns="False" OnRowDataBound="GrdDocumentMaster_RowDataBound">
                    <Columns>
                        <asp:TemplateField>
                            <ItemTemplate>
                              
                                <h5>Submittal Name : <%#Eval("DocName")%></h5>
                                <asp:GridView ID="GrdActualDocuments" runat="server" Width="100%" AutoGenerateColumns="false" AllowPaging="false" DataKeyNames="ActualDocumentUID" CssClass="table table-bordered" OnRowDataBound="GrdActualDocuments_RowDataBound"    >
                                       <Columns>
                                           <asp:BoundField DataField="ActualDocument_Name" HeaderText="Document Name" ItemStyle-HorizontalAlign="Left">
                                                            </asp:BoundField>
                                           <asp:BoundField DataField="ActualDocument_Version" HeaderText="Version" ItemStyle-HorizontalAlign="Left">
                                                            </asp:BoundField>
                                          <%-- <asp:BoundField DataField="Doc_Type" HeaderText="Document For">
                                            <HeaderStyle HorizontalAlign="Center" />
                                            </asp:BoundField>--%>
                                           <asp:BoundField DataField="ActualDocument_Type" HeaderText="Document Type" ItemStyle-HorizontalAlign="Left">
                                                            </asp:BoundField>
                                           
                                           <asp:BoundField DataField="ActualDocumentUID" HeaderText="Status" ItemStyle-HorizontalAlign="Left">
                                            <HeaderStyle HorizontalAlign="Left" />
                                            </asp:BoundField>
                                           <asp:BoundField DataField="ActualDocument_CreatedDate" HeaderText="Submit Date" ItemStyle-HorizontalAlign="Left" DataFormatString="{0:dd MMM yyyy}">
                                                            </asp:BoundField>

                                           <asp:BoundField DataField="ProjectRef_Number" HeaderText="Ontb Reference #" ItemStyle-HorizontalAlign="Left">
                                                            </asp:BoundField>
                                           <asp:BoundField DataField="Ref_Number" HeaderText="Originator Reference #" ItemStyle-HorizontalAlign="Left">
                                                            </asp:BoundField>

                                           <%--<asp:TemplateField HeaderText="Submit Date" ItemStyle-HorizontalAlign="Left">
                                               <HeaderStyle HorizontalAlign="Left" />
                                               <ItemTemplate>
                                                   <%#Eval("ActualDocument_CreatedDate","{0:dd MMM yyyy}")%>
                                               </ItemTemplate>
                                           </asp:TemplateField>--%>
                                       </Columns>
                                       </asp:GridView>
                            </ItemTemplate>
                        </asp:TemplateField>
                    </Columns>
                </asp:GridView>
                            </div>

                       
                           
                                
                                
                        </div>
                    </div>
                </div>
            </div>
        </div>

        <div class="container-fluid" id="ByStatusGrid" runat="server">
        <div class="row">
            <div class="col-lg-12 col-xl-12 form-group">
                 <h6  class="text-muted text-uppercase font-weight-bold">
                                   Document List by Status
                               </h6>
              
                <div class="card mb-4">
                    <div class="card-body">
                        <div class="card-title">
                        <div class="d-flex justify-content-between">
                              <h6 class="text-muted">
                                  
                                   <asp:Label ID="Label1" CssClass="text-uppercase font-weight-bold" runat="server" Text="Submitted Documents"  />
                                       <%--<asp:Label Text="Foo bar" CssClass="pl-1" runat="server" />--%>
                               </h6>
                                <div>
                                   
                                </div>
                          </div>
                            </div>
                        <div class="table-responsive">
                         <asp:GridView ID="GrdActualSubmittedDocuments" EmptyDataText="No Documents Found." runat="server" Width="100%" AutoGenerateColumns="false" CellPadding="6" CellSpacing="16" 
                                            AllowPaging="true" PageSize="15" DataKeyNames="ActualDocumentUID" CssClass="table table-bordered" OnRowDataBound="GrdActualSubmittedDocuments_RowDataBound" OnPageIndexChanging="GrdActualSubmittedDocuments_PageIndexChanging">
                                       <Columns>
                                           <asp:TemplateField HeaderText="Submittal Name">
                                            <ItemTemplate>
                                                <%#GetSubmittalName(Eval("DocumentUID").ToString())%>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                           <asp:TemplateField HeaderText="Document Name" ItemStyle-HorizontalAlign="Left">
                                            <ItemTemplate>
                                             <%-- <img width="24" src='Images/<%#GetDocumentTypeIcon(Eval("ActualDocument_Type").ToString())%>' alt='<%#Eval("ActualDocument_Type")%>' />  &nbsp;&nbsp;--%>
                                                <asp:Label ID="lblName" runat="server" Text='<%#Eval("ActualDocument_Name")%>'></asp:Label>
                                                <%--<a class='<%#Convert.ToString(Eval("Doc_Type")) == "Cover Letter" ? "test" : Convert.ToString(Eval("Doc_Type")) =="General Document" ? "test" : "view" %>'  href='ViewDocument.aspx?DocID=<%#Eval("ActualDocumentUID")%>&ProjectUID=<%#Eval("ProjectUID")%>'><%#Eval("ActualDocument_Name")%></a>
                                                <asp:Label ID="lblName" runat="server" Text='<%#Eval("ActualDocument_Name")%>' Visible='<%#Convert.ToString(Eval("Doc_Type")) == "Cover Letter" ? true : Convert.ToString(Eval("Doc_Type")) == "General Document" ? true : false %>'></asp:Label>--%>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                           <asp:TemplateField HeaderText="Document Type">
                                            <ItemTemplate>
                                                <%#GetDocumentName(Eval("ActualDocument_Type").ToString())%>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                           <asp:BoundField DataField="ActualDocument_CurrentStatus" HeaderText="Current Status">
                                            <HeaderStyle HorizontalAlign="Left" />
                                            </asp:BoundField>
                                            <asp:BoundField DataField="ActualDocumentUID" HeaderText="Submitted Date">
                                            <HeaderStyle HorizontalAlign="Left" />
                                            </asp:BoundField>
                                       </Columns>
                                       </asp:GridView>
                            </div>
                        </div>
                    </div>
                </div>
            </div>

        <div class="row">
            <div class="col-lg-12 col-xl-12 form-group">
                <div class="card mb-4">
                    <div class="card-body">
                        <div class="card-title">
                        <div class="d-flex justify-content-between">
                              <h6 class="text-muted">
                                   <asp:Label ID="Label2" CssClass="text-uppercase font-weight-bold" runat="server" Text="Reviewed Documents"  />
                                       <%--<asp:Label Text="Foo bar" CssClass="pl-1" runat="server" />--%>
                               </h6>
                                <div>
                                   
                                </div>
                          </div>
                            </div>
                        <div class="table-responsive">
                         <asp:GridView ID="GrdReviewedDocuments" EmptyDataText="No Documents Found." runat="server" Width="100%" AutoGenerateColumns="false" 
                                            AllowPaging="true" PageSize="15" DataKeyNames="ActualDocumentUID" CssClass="table table-bordered" OnRowDataBound="GrdReviewedDocuments_RowDataBound" OnPageIndexChanging="GrdReviewedDocuments_PageIndexChanging">
                                       <Columns>
                                           <asp:TemplateField HeaderText="Submittal Name">
                                            <ItemTemplate>
                                                <%#GetSubmittalName(Eval("DocumentUID").ToString())%>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                           <asp:TemplateField HeaderText="Document Name" ItemStyle-HorizontalAlign="Left">
                                            <ItemTemplate>
                                             <%-- <img width="24" src='Images/<%#GetDocumentTypeIcon(Eval("ActualDocument_Type").ToString())%>' alt='<%#Eval("ActualDocument_Type")%>' />  &nbsp;&nbsp;--%>
                                                <asp:Label ID="lblName" runat="server" Text='<%#Eval("ActualDocument_Name")%>'></asp:Label>
                                                <%--<a class='<%#Convert.ToString(Eval("Doc_Type")) == "Cover Letter" ? "test" : Convert.ToString(Eval("Doc_Type")) =="General Document" ? "test" : "view" %>'  href='ViewDocument.aspx?DocID=<%#Eval("ActualDocumentUID")%>&ProjectUID=<%#Eval("ProjectUID")%>'><%#Eval("ActualDocument_Name")%></a>
                                                <asp:Label ID="lblName" runat="server" Text='<%#Eval("ActualDocument_Name")%>' Visible='<%#Convert.ToString(Eval("Doc_Type")) == "Cover Letter" ? true : Convert.ToString(Eval("Doc_Type")) == "General Document" ? true : false %>'></asp:Label>--%>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                           <asp:TemplateField HeaderText="Document Type">
                                            <ItemTemplate>
                                                <%#GetDocumentName(Eval("ActualDocument_Type").ToString())%>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                           <asp:BoundField DataField="ActualDocument_CurrentStatus" HeaderText="Current Status">
                                            <HeaderStyle HorizontalAlign="Left" />
                                            </asp:BoundField>
                                           <asp:BoundField DataField="ActualDocumentUID" HeaderText="Reviewed Date">
                                            <HeaderStyle HorizontalAlign="Left" />
                                            </asp:BoundField>
                                           <asp:BoundField DataField="ActualDocumentUID" HeaderText="Reviewed Days">
                                            <HeaderStyle HorizontalAlign="Left" />
                                            </asp:BoundField>

                                           <%--<asp:TemplateField HeaderText="Reviewed Date">
                                               <ItemTemplate>
                                                   <%#Eval("ActualDocument_CreatedDate","{0:dd MMM yyyy}")%>
                                               </ItemTemplate>
                                           </asp:TemplateField>--%>
                                       </Columns>
                                       </asp:GridView>
                            </div>
                        </div>
                    </div>
                </div>
            </div>

        <div class="row">
            <div class="col-lg-12 col-xl-12 form-group">
                <div class="card mb-4">
                    <div class="card-body">
                        <div class="card-title">
                        <div class="d-flex justify-content-between">
                              <h6 class="text-muted">
                                   <asp:Label ID="Label3" CssClass="text-uppercase font-weight-bold" runat="server" Text="Approved(Code A) Documents"  />
                                       <%--<asp:Label Text="Foo bar" CssClass="pl-1" runat="server" />--%>
                               </h6>
                                <div>
                                   
                                </div>
                          </div>
                            </div>
                        <div class="table-responsive">
                         <asp:GridView ID="GrdApprovedDocuments" EmptyDataText="No Documents Found." runat="server" Width="100%" AutoGenerateColumns="false" 
                                            AllowPaging="true" PageSize="15" DataKeyNames="ActualDocumentUID" CssClass="table table-bordered" OnRowDataBound="GrdApprovedDocuments_RowDataBound" OnPageIndexChanging="GrdApprovedDocuments_PageIndexChanging">
                                       <Columns>
                                           <asp:TemplateField HeaderText="Submittal Name">
                                            <ItemTemplate>
                                                <%#GetSubmittalName(Eval("DocumentUID").ToString())%>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                           <asp:TemplateField HeaderText="Document Name" ItemStyle-HorizontalAlign="Left">
                                            <ItemTemplate>
                                             <%-- <img width="24" src='Images/<%#GetDocumentTypeIcon(Eval("ActualDocument_Type").ToString())%>' alt='<%#Eval("ActualDocument_Type")%>' />  &nbsp;&nbsp;--%>
                                                <asp:Label ID="lblName" runat="server" Text='<%#Eval("ActualDocument_Name")%>'></asp:Label>
                                                <%--<a class='<%#Convert.ToString(Eval("Doc_Type")) == "Cover Letter" ? "test" : Convert.ToString(Eval("Doc_Type")) =="General Document" ? "test" : "view" %>'  href='ViewDocument.aspx?DocID=<%#Eval("ActualDocumentUID")%>&ProjectUID=<%#Eval("ProjectUID")%>'><%#Eval("ActualDocument_Name")%></a>
                                                <asp:Label ID="lblName" runat="server" Text='<%#Eval("ActualDocument_Name")%>' Visible='<%#Convert.ToString(Eval("Doc_Type")) == "Cover Letter" ? true : Convert.ToString(Eval("Doc_Type")) == "General Document" ? true : false %>'></asp:Label>--%>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                           <asp:TemplateField HeaderText="Document Type">
                                            <ItemTemplate>
                                                <%#GetDocumentName(Eval("ActualDocument_Type").ToString())%>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                           <asp:BoundField DataField="ActualDocument_CurrentStatus" HeaderText="Current Status">
                                            <HeaderStyle HorizontalAlign="Left" />
                                            </asp:BoundField>
                                             <asp:BoundField DataField="ActualDocumentUID" HeaderText="Approved Date">
                                            <HeaderStyle HorizontalAlign="Left" />
                                            </asp:BoundField>
                                           <asp:BoundField DataField="ActualDocumentUID" HeaderText="Approved Days">
                                            <HeaderStyle HorizontalAlign="Left" />
                                            </asp:BoundField>
                                       </Columns>
                                       </asp:GridView>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </div>
        
        <div class="container-fluid" id="DivByOriginator" runat="server">
            <div class="row">
                <div class="col-lg-12 col-xl-12 form-group">
                <div class="card mb-4">
                    <div class="card-body">
                        <div class="col-lg-12 col-xl-12 form-group" align="center">
                               <h5 id="OriginatorReportName" style="font-weight:bold;" runat="server">Originator ReportName</h5>
                               <h5 id="OriginatorProjectName" runat="server">Project Name</h5>
                          </div>
                        <div class="card-title">
                        <div class="d-flex justify-content-between">
                              <h6 class="text-muted">
                                   <%--<asp:Label ID="LblOriginatorDocument" CssClass="text-uppercase font-weight-bold" runat="server" Text="Status of Documents submitted by Originator"  />--%>
                               </h6>
                               <div>
                                        <asp:Button ID="btnbyOriginatorExportExcel" runat="server" Text="Export Excel" Visible="false" CssClass="btn btn-primary" OnClick="btnbyOriginatorExportExcel_Click" />
                                        <asp:Button ID="btnbyOriginatorExportPDF" runat="server" Text="Export PDF" Visible="false" CssClass="btn btn-primary" OnClick="btnbyOriginatorExportPDF_Click" />
                                        <asp:Button ID="btnbyOriginatorPrint" runat="server" Text="Print" Visible="false" CssClass="btn btn-primary" OnClick="btnbyOriginatorPrint_Click" />
                                 </div>
                          </div>
                            </div>
                        <div class="table-responsive">
                            <asp:GridView ID="GrdOriginatorDocuments" EmptyDataText="No Documents Found." AlternatingRowStyle-BackColor="LightGray" runat="server" Width="100%" AutoGenerateColumns="false" 
                                            AllowPaging="false" DataKeyNames="ActualDocumentUID" CssClass="table table-bordered" OnRowDataBound="GrdOriginatorDocuments_RowDataBound">
                                       <Columns>
                                           <asp:TemplateField HeaderText="Sl.No" ItemStyle-HorizontalAlign="Center">
                                                            <ItemTemplate>
                                                                <%# Container.DataItemIndex + 1 %>
                                                            </ItemTemplate>
                                                            <ItemStyle HorizontalAlign="Center"></ItemStyle>
                                                        </asp:TemplateField>
                                           <asp:TemplateField HeaderText="Discipline">
                                            <ItemTemplate>
                                                <%#GetDocumentCategoryName(Eval("DocumentUID").ToString())%>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                           <asp:TemplateField HeaderText="Submittal Name">
                                            <ItemTemplate>
                                                <%#GetSubmittalName(Eval("DocumentUID").ToString())%>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                           <asp:TemplateField HeaderText="Document Name" ItemStyle-HorizontalAlign="Left" ItemStyle-Width="35%">
                                            <ItemTemplate>
                                             <%-- <img width="24" src='Images/<%#GetDocumentTypeIcon(Eval("ActualDocument_Type").ToString())%>' alt='<%#Eval("ActualDocument_Type")%>' />  &nbsp;&nbsp;--%>
                                                <%#Eval("ActualDocument_Name")%>
                                                <%--<a class='<%#Convert.ToString(Eval("Doc_Type")) == "Cover Letter" ? "test" : Convert.ToString(Eval("Doc_Type")) =="General Document" ? "test" : "view" %>'  href='ViewDocument.aspx?DocID=<%#Eval("ActualDocumentUID")%>&ProjectUID=<%#Eval("ProjectUID")%>'><%#Eval("ActualDocument_Name")%></a>
                                                <asp:Label ID="lblName" runat="server" Text='<%#Eval("ActualDocument_Name")%>' Visible='<%#Convert.ToString(Eval("Doc_Type")) == "Cover Letter" ? true : Convert.ToString(Eval("Doc_Type")) == "General Document" ? true : false %>'></asp:Label>--%>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                           <asp:TemplateField HeaderText="Document Type" ItemStyle-HorizontalAlign="Center">
                                            <ItemTemplate>
                                                <%#GetDocumentName(Eval("ActualDocument_Type").ToString())%>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                           <asp:BoundField DataField="ProjectRef_Number" HeaderText="Ontb Reference #" ItemStyle-HorizontalAlign="Center">
                                            <HeaderStyle HorizontalAlign="Left" />
                                            </asp:BoundField>
                                           <asp:BoundField DataField="Ref_Number" HeaderText="Originator Reference #" ItemStyle-HorizontalAlign="Center">
                                            <HeaderStyle HorizontalAlign="Left" />
                                            </asp:BoundField>
                                           <asp:BoundField DataField="ActualDocument_CurrentStatus" HeaderText="Current Status" ItemStyle-HorizontalAlign="Center">
                                            <HeaderStyle HorizontalAlign="Left" />
                                            </asp:BoundField>
                                            <asp:BoundField DataField="ActualDocument_CreatedDate" HeaderText="Created Date" ItemStyle-HorizontalAlign="Center" DataFormatString="{0:dd MMM yyyy}">
                                            <HeaderStyle HorizontalAlign="Left" />
                                            </asp:BoundField>
                                       </Columns>
                                       </asp:GridView>
                            </div>
                        </div>
                    </div>
                </div>
                </div>
        </div>

    <div class="container-fluid" id="DivDocumentSummary" runat="server">
            <div class="row">
                <div class="col-lg-12 col-xl-12 form-group">
                <div class="card mb-4">
                    <div class="card-body">
                        <div class="col-lg-12 col-xl-12 form-group" align="center">
                                            <h5 id="DocumentSummaryReportName" style="font-weight:bold;" runat="server">Document Summary</h5>
                                            <h5 id="DocumentSummaryProjectName" runat="server">Project Name</h5>
                                            </div>
                        <div class="card-title">
                        <div class="d-flex justify-content-between">
                              <h6 class="text-muted">
                                   <%--<asp:Label ID="LblDocumentSummary" CssClass="text-uppercase font-weight-bold" runat="server" Text="Status of Documents submitted by the Contractor"  />--%>
                                      
                               </h6>
                               <div>
                                        <asp:Button ID="btnDocumentSummaryExportExcel" runat="server" Text="Export Excel" Visible="false" CssClass="btn btn-primary" OnClick="btnDocumentSummaryExportExcel_Click" />
                                        <asp:Button ID="btnDocumentSummaryExportPDF" runat="server" Text="Export PDF" Visible="false" CssClass="btn btn-primary" OnClick="btnDocumentSummaryExportPDF_Click" />
                                        <asp:Button ID="btnDocumentSummaryPrint" runat="server" Text="Print" Visible="false" CssClass="btn btn-primary" OnClick="btnDocumentSummaryPrint_Click" />
                                 </div>
                          </div>
                            </div>
                        <div class="table-responsive">
                            <asp:GridView ID="GrdDocumentSummary" EmptyDataText="No Documents Found." HeaderStyle-HorizontalAlign="Center" runat="server" Width="100%" AutoGenerateColumns="false" AlternatingRowStyle-BackColor="LightGray" OnDataBound="GrdDocumentSummary_DataBound"
                                         OnRowDataBound="GrdDocumentSummary_RowDataBound"   AllowPaging="false" CssClass="table table-bordered">
                                       <Columns>
                                           <asp:TemplateField HeaderText="Sl.No" ItemStyle-HorizontalAlign="Center">
                                              <ItemTemplate>
                                                  <%--<%# Container.DataItemIndex + 1 %>--%>
                                                  <%#Eval("Sl_No")%>
                                               </ItemTemplate>
                                              <ItemStyle HorizontalAlign="Center"></ItemStyle>
                                           </asp:TemplateField>
                                           <asp:TemplateField HeaderText="Documents" ItemStyle-HorizontalAlign="Left" HeaderStyle-CssClass="text-left">
                                               <ItemTemplate>
                                                   <%#Eval("Documents")%>
                                               </ItemTemplate>
                                           </asp:TemplateField>
                                           <asp:TemplateField HeaderText="Submitted by the Contractor" ItemStyle-HorizontalAlign="Center">
                                               <ItemTemplate>
                                                   <%#Eval("Submitted_by_the_Contractor")%>
                                               </ItemTemplate>
                                           </asp:TemplateField>
                                           <asp:TemplateField HeaderText="Recommended / Returned by PMC" ItemStyle-HorizontalAlign="Center">
                                               <ItemTemplate>
                                                   <%#Eval("Recommended_Returned_by_PMC")%>
                                               </ItemTemplate>
                                           </asp:TemplateField>
                                           <asp:TemplateField HeaderText="Approved by BWSSB" ItemStyle-HorizontalAlign="Center">
                                               <ItemTemplate>
                                                   <%#Eval("Approved_by_BWSSB")%>
                                               </ItemTemplate>
                                           </asp:TemplateField>
                                       </Columns>
                                       </asp:GridView>
                            </div>
                        </div>
                    </div>
                </div>
                </div>
        </div>
</asp:Content>