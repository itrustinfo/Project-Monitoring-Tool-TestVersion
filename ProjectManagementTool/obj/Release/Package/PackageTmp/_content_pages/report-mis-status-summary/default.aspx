<%@ Page Title="" Language="C#" MasterPageFile="~/_master_pages/default.Master" AutoEventWireup="true" CodeBehind="default.aspx.cs" Inherits="ProjectManagementTool._content_pages.report_mis_status_summary._default" %>

<asp:Content ID="Content1" ContentPlaceHolderID="default_master_head" runat="server">
    <style type="text/css">
        .hideItem {
            display: none;
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="default_master_body" runat="server">
    <div class="container-fluid">
        <div class="row">
            <div class="col-md-6 col-lg-4 form-group">MIS Status Summary Report</div>
        </div>
    </div>

    <div class="container-fluid" id="divTabular" runat="server" >
        <div class="row">
            <div class="col-lg-12 col-xl-12 form-group">
                    <div class="card mb-4">
                        <div class="card-body">
                            <div class="col-lg-12 col-xl-12 form-group" align="center">
                                <h5 id="ReportName" style="font-weight: bold;" runat="server">MIS Status Summary Report</h5>
                            </div>
                            <div class="card-title">
                                <div class="d-flex justify-content-between">
                                    <h6 class="text-muted">
                                    </h6>
                                    <div>
                                        <asp:Button ID="btnRefresh" runat="server" Text="Refresh" CssClass="btn btn-primary" Visible="true" OnClick="btnRefresh_Click"></asp:Button>&nbsp;
                                        <asp:Button ID="btnExcel" runat="server" Text="Export to Excel" CssClass="btn btn-primary" Visible="true" OnClick="btnExcel_Click" />&nbsp;
                                        <asp:Button ID="btnPDF" runat="server" Text="Export to PDF" CssClass="btn btn-primary" Visible="false" OnClick="btnPDF_Click" />&nbsp;
                                        <asp:Button ID="tnPrint" runat="server" Text="Print" CssClass="btn btn-primary" Visible="false" OnClick="tnPrint_Click" />
                                    </div>
                                </div>
                            </div>
                            <div class="table-responsive">
                                <div style="overflow: scroll;" onscroll="OnScrollDivNew(this)" id="DivMainContentNew">
                                        <asp:GridView ID="grdDataList" runat="server" AutoGenerateColumns="false" CellPadding="6" CellSpacing="16"  HeaderStyle-BackColor="#666666" HeaderStyle-ForeColor="White" 
                                            CssClass="table table-bordered" OnRowDataBound="grdDataList_RowDataBound" OnDataBound="GrdDataList_DataBound">
                                            <Columns>
                                                <asp:BoundField DataField="ProjectNameDisplay"  HeaderText="Project" >
                                                    <HeaderStyle HorizontalAlign="Left" />
                                                </asp:BoundField>
                                                <asp:BoundField DataField="DocumentType"  HeaderText="Document Type" >
                                                    <HeaderStyle HorizontalAlign="Left" />
                                                </asp:BoundField>
                                                <asp:TemplateField  HeaderText="Total Submission" >
                                                    <ItemTemplate>
                                                        <a class="showModalDocumentView" href="/_content_pages/report-mis-details?ProjectName=<%#Eval("Project")%>&FlowName=<%#Eval("DocumentType")%>&type=Total Submission"><%#Eval("Total Submission")%>   </a>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField  HeaderText="Reconciliation Pending" >
                                                    <ItemTemplate>
                                                        <a class="showModalDocumentView" href="/_content_pages/report-mis-details?ProjectName=<%#Eval("Project")%>&FlowName=<%#Eval("DocumentType")%>&type=Reconciliation Pending"><%#Eval("Reconciliation Pending")%>   </a>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField  HeaderText="Reconciliation Rejected" >
                                                    <ItemTemplate>
                                                        <a class="showModalDocumentView" href="/_content_pages/report-mis-details?ProjectName=<%#Eval("Project")%>&FlowName=<%#Eval("DocumentType")%>&type=Reconciliation Rejected"><%#Eval("Reconciliation Rejected")%>   </a>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField  HeaderText="Reconciliation Accepted" >
                                                    <ItemTemplate>
                                                        <a class="showModalDocumentView" href="/_content_pages/report-mis-details?ProjectName=<%#Eval("Project")%>&FlowName=<%#Eval("DocumentType")%>&type=Reconciliation Accepted"><%#Eval("Reconciliation Accepted")%>   </a>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField  HeaderText="PMC Review" >
                                                    <ItemTemplate>
                                                        <a class="showModalDocumentView" href="/_content_pages/report-mis-details?ProjectName=<%#Eval("Project")%>&FlowName=<%#Eval("DocumentType")%>&type=PMC Review"><%#Eval("PMC Review")%>   </a>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField  HeaderText="Project CO" >
                                                    <ItemTemplate>
                                                        <a class="showModalDocumentView" href="/_content_pages/report-mis-details?ProjectName=<%#Eval("Project")%>&FlowName=<%#Eval("DocumentType")%>&type=Project Co-ordinator"><%#Eval("Project Co-ordinator")%>   </a>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField  HeaderText="Project CO Meeting with EE" >
                                                    <ItemTemplate>
                                                        <a class="showModalDocumentView" href="/_content_pages/report-mis-details?ProjectName=<%#Eval("Project")%>&FlowName=<%#Eval("DocumentType")%>&type=Meeting with EE"><%#Eval("Meeting with EE")%>   </a>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField  HeaderText="DTL Review" >
                                                    <ItemTemplate>
                                                        <a class="showModalDocumentView" href="/_content_pages/report-mis-details?ProjectName=<%#Eval("Project")%>&FlowName=<%#Eval("DocumentType")%>&type=Review"><%#Eval("Review")%>   </a>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField  HeaderText="DTL Rejected" >
                                                    <ItemTemplate>
                                                        <a class="showModalDocumentView" href="/_content_pages/report-mis-details?ProjectName=<%#Eval("Project")%>&FlowName=<%#Eval("DocumentType")%>&type=DTL Rejected"><%#Eval("DTL Rejected")%>   </a>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField  HeaderText="DTL Internal Meeting" >
                                                    <ItemTemplate>
                                                        <a class="showModalDocumentView" href="/_content_pages/report-mis-details?ProjectName=<%#Eval("Project")%>&FlowName=<%#Eval("DocumentType")%>&type=DTL Internal Meeting"><%#Eval("DTL Internal Meeting")%>   </a>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField  HeaderText="DTL Back To Contractor" >
                                                    <ItemTemplate>
                                                        <a class="showModalDocumentView" href="/_content_pages/report-mis-details?ProjectName=<%#Eval("Project")%>&FlowName=<%#Eval("DocumentType")%>&type=DTL Back To Contractor"><%#Eval("DTL Back To Contractor")%>   </a>
                                                    </ItemTemplate>
                                                </asp:TemplateField>

                                                <asp:TemplateField  HeaderText="AEE Approval" >
                                                    <ItemTemplate>
                                                        <a class="showModalDocumentView" href="/_content_pages/report-mis-details?ProjectName=<%#Eval("Project")%>&FlowName=<%#Eval("DocumentType")%>&type=AEE Approval"><%#Eval("AEE Approval")%>   </a>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField  HeaderText="EE Approval" >
                                                    <ItemTemplate>
                                                        <a class="showModalDocumentView" href="/_content_pages/report-mis-details?ProjectName=<%#Eval("Project")%>&FlowName=<%#Eval("DocumentType")%>&type=EE Approval"><%#Eval("EE Approval")%>   </a>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField  HeaderText="ACE Approval" >
                                                    <ItemTemplate>
                                                        <a class="showModalDocumentView" href="/_content_pages/report-mis-details?ProjectName=<%#Eval("Project")%>&FlowName=<%#Eval("DocumentType")%>&type=ACE Approval"><%#Eval("ACE Approval")%>   </a>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField  HeaderText="CE Approval" >
                                                    <ItemTemplate>
                                                        <a class="showModalDocumentView" href="/_content_pages/report-mis-details?ProjectName=<%#Eval("Project")%>&FlowName=<%#Eval("DocumentType")%>&type=CE Approval"><%#Eval("CE Approval")%>   </a>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField  HeaderText="Back to Contractor Stage 1" >
                                                    <ItemTemplate>
                                                        <a class="showModalDocumentView" href="/_content_pages/report-mis-details?ProjectName=<%#Eval("Project")%>&FlowName=<%#Eval("DocumentType")%>&type=Back to Contractor Stage 1"><%#Eval("Back to Contractor Stage 1")%>   </a>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField  HeaderText="Client Approved" >
                                                    <ItemTemplate>
                                                        <a class="showModalDocumentView" href="/_content_pages/report-mis-details?ProjectName=<%#Eval("Project")%>&FlowName=<%#Eval("DocumentType")%>&type=Client Approved"><%#Eval("Client Approved")%>   </a>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField  HeaderText="Code A" >
                                                    <ItemTemplate>
                                                        <a class="showModalDocumentView" href="/_content_pages/report-mis-details?ProjectName=<%#Eval("Project")%>&FlowName=<%#Eval("DocumentType")%>&type=Code A"><%#Eval("Code A")%>   </a>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField  HeaderText="Code B" >
                                                    <ItemTemplate>
                                                        <a class="showModalDocumentView" href="/_content_pages/report-mis-details?ProjectName=<%#Eval("Project")%>&FlowName=<%#Eval("DocumentType")%>&type=Code B"><%#Eval("Code B")%>   </a>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField  HeaderText="Code C" >
                                                    <ItemTemplate>
                                                        <a class="showModalDocumentView" href="/_content_pages/report-mis-details?ProjectName=<%#Eval("Project")%>&FlowName=<%#Eval("DocumentType")%>&type=Code C"><%#Eval("Code C")%>   </a>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField  HeaderText="Code D" >
                                                    <ItemTemplate>
                                                        <a class="showModalDocumentView" href="/_content_pages/report-mis-details?ProjectName=<%#Eval("Project")%>&FlowName=<%#Eval("DocumentType")%>&type=Code D"><%#Eval("Code D")%>   </a>
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
    </div>

</asp:Content>



