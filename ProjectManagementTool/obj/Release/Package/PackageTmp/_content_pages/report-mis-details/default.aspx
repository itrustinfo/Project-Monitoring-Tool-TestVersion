<%@ Page Title="" Language="C#" MasterPageFile="~/_master_pages/default.Master" AutoEventWireup="true" CodeBehind="default.aspx.cs" Inherits="ProjectManagementTool._content_pages.report_mis_details._default" %>

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
            <div class="col-md-6 col-lg-4 form-group">MIS details Report</div>
        </div>
    </div>

    <div class="container-fluid" id="divTabular" runat="server">
        <div class="row">
            <div class="col-lg-12 col-xl-12 form-group">
                <div class="card mb-4">
                    <div class="card-body">
                        <div class="card-title">
                            <div class="d-flex justify-content-between">
                                <h6 class="text-muted">
                                    <asp:Label ID="LblDocumentHeading" Text="Documents List" CssClass="text-uppercase font-weight-bold" runat="server" />
                                    <%--<asp:Label Text="Foo bar" CssClass="pl-1" runat="server" />--%>
                                </h6>
                                <div class="col-md-6 col-lg-4 form-group text-right">
                                    <asp:Button ID="btnback" runat="server" Text="Back To Report" CssClass="btn btn-primary" PostBackUrl="/_content_pages/report-mis-status-summary/"></asp:Button>

                                </div>
                            </div>
                        </div>
                        
                        <div class="table-responsive" id="divMain" runat="server">
                            <asp:Label runat="server" Text="" ID="lblTotalcount"></asp:Label>
                                <div id="divsummary" runat="server" visible="true">
                                    <div style="overflow: hidden;" id="DivHeaderRowNew"></div>
                                    <div style="overflow: scroll;" onscroll="OnScrollDivNew(this)" id="DivMainContentNew">
                                        <%--<asp:GridView ID="grdDataList" EmptyDataText="No Data Found." runat="server" Width="100%" AutoGenerateColumns="false" CellPadding="6" CellSpacing="16" HeaderStyle-BackColor="#666666" HeaderStyle-ForeColor="White" AlternatingRowStyle-BackColor="#F5F5F5"
                                            CssClass="table table-bordered" OnRowDataBound="grdDataList_RowDataBound">
                                            <Columns>
                                                <asp:BoundField DataField="ActualDocumentUID"  HeaderText="UID" ItemStyle-CssClass="hideItem" HeaderStyle-CssClass="hideItem">
                                                    <HeaderStyle HorizontalAlign="Left" />
                                                </asp:BoundField>
                                                <asp:TemplateField HeaderText="Sl.No" ItemStyle-HorizontalAlign="Center">
                                                    <ItemTemplate>
                                                        <%# Container.DataItemIndex + 1 %>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:BoundField DataField="ProjectName" HeaderText="Project">
                                                    <HeaderStyle HorizontalAlign="Left" />
                                                </asp:BoundField>
                                                <asp:BoundField DataField="Flow_Name" HeaderText="Flow Name" >
                                                    <HeaderStyle HorizontalAlign="Left" />
                                                </asp:BoundField>
                                                <asp:BoundField DataField="DocName" HeaderText="Submittal Name">
                                                    <HeaderStyle HorizontalAlign="Left" />
                                                </asp:BoundField>
                                                <asp:BoundField DataField="ActualDocument_Name" HeaderText="Document Name">
                                                    <HeaderStyle HorizontalAlign="Left" />
                                                </asp:BoundField>
                                                <asp:BoundField DataField="ActualDocument_Type" HeaderText="Document Type">
                                                    <HeaderStyle HorizontalAlign="Left" />
                                                </asp:BoundField>
                                                <asp:BoundField DataField="ActualDocument_CurrentStatus" HeaderText="Current Status">
                                                    <HeaderStyle HorizontalAlign="Left" />
                                                </asp:BoundField>
                                                <asp:BoundField DataField="ProjectRef_Number" HeaderText="Ontb Reference #">
                                                    <HeaderStyle HorizontalAlign="Left" />
                                                </asp:BoundField>
                                                <asp:BoundField DataField="Ref_Number" HeaderText="Originator Reference #">
                                                    <HeaderStyle HorizontalAlign="Left" />
                                                </asp:BoundField>
                                                
                                                <asp:BoundField DataField="Document_Date" HeaderText="Contractor Uploaded Date" SortExpression="Document_Date" DataFormatString="{0:dd/MM/yyyy}">
                                                    <HeaderStyle HorizontalAlign="Left" />
                                                </asp:BoundField>
                                                <asp:BoundField DataField="CreatedDate" HeaderText="ONTB Accepted/Rejected Date" SortExpression="CreatedDate" DataFormatString="{0:dd/MM/yyyy}">
                                                    <HeaderStyle HorizontalAlign="Left" />
                                                </asp:BoundField>
                                                <asp:BoundField DataField="Remarks" HeaderText="Remarks" SortExpression="Remarks">
                                                    <HeaderStyle HorizontalAlign="Left" />
                                                </asp:BoundField>

                                                
                                                

                                            </Columns>
                                        </asp:GridView>--%>



                                        <asp:GridView ID="GrdDocuments" EmptyDataText="No Documents Found." runat="server" Width="100%" AutoGenerateColumns="false"
                                            AllowPaging="false" DataKeyNames="ActualDocumentUID" CssClass="table table-bordered" OnRowDataBound="GrdDocuments_RowDataBound" OnRowCommand="GrdDocuments_RowCommand" HeaderStyle-BackColor="#666666" HeaderStyle-ForeColor="White" AlternatingRowStyle-BackColor="#F5F5F5" OnDataBound="GrdDocuments_DataBound">
                                            <Columns>
                                                <asp:BoundField DataField="ActualDocumentUID" HeaderText="UID" ItemStyle-CssClass="hideItem" HeaderStyle-CssClass="hideItem">
                                                    <HeaderStyle HorizontalAlign="Left" />
                                                </asp:BoundField>
                                                <asp:TemplateField HeaderText="Sl.No" ItemStyle-HorizontalAlign="Center">
                                                    <ItemTemplate>
                                                        <%# Container.DataItemIndex + 1 %>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:BoundField DataField="ProjectName" HeaderText="Project">
                                                    <HeaderStyle HorizontalAlign="Left" />
                                                </asp:BoundField>
                                                <asp:TemplateField HeaderText="Submittal Name" SortExpression="DocName">
                                                    <ItemTemplate>
                                                        <a href="#" data-toggle="tooltip" data-placement="top" title='<%#GetTaskHierarchy_By_DocumentUID(Eval("DocumentUID").ToString())%>'><i class="fa fa-info-circle" style="color: black;" aria-hidden="true"></i></a>&nbsp;&nbsp; <a href="/_content_pages/documents/Default.aspx?SubmittalUID=<%#Eval("DocumentUID")%>"><%#GetSubmittalName(Eval("DocumentUID").ToString())%></a>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField ItemStyle-Width="35%" HeaderStyle-Width="35%" HeaderText="Document Name" ItemStyle-HorizontalAlign="Left" SortExpression="ActualDocument_Name">
                                                    <ItemTemplate>
                                                        <img width="24" src='../../_assets/images/<%#GetDocumentTypeIcon(Eval("ActualDocument_Type").ToString())%>' alt='<%#Eval("ActualDocument_Type")%>' />
                                                        &nbsp;&nbsp;
                                                       <%--<asp:LinkButton ID="lnkviewpdf" class='<%#Convert.ToString(Eval("ActualDocument_Type")) == ".pdf" ? "view" : "hideItem" %>' runat="server" CausesValidation="false" CommandArgument='<%#Eval("ActualDocument_Path")%>' CommandName="ViewDoc"><%#Eval("ActualDocument_Name")%></asp:LinkButton>--%>
                                                        <a id="ShowFile" href='/_modal_pages/preview-document.aspx?ActualDocumentUID=<%#Eval("ActualDocumentUID")%>&previewpath=<%#Eval("ActualDocument_Path").ToString().Replace('&','!')%>' class='<%#Convert.ToString(Eval("ActualDocument_Type")) == ".pdf" ? "showModalPreviewDocument" : Convert.ToString(Eval("ActualDocument_Type")) == ".xls" ? "showModalPreviewDocument" : Convert.ToString(Eval("ActualDocument_Type")) == ".xlsx" ? "showModalPreviewDocument" : Convert.ToString(Eval("ActualDocument_Type")) == ".docx" ? "showModalPreviewDocument" : Convert.ToString(Eval("ActualDocument_Type")) == ".doc" ? "showModalPreviewDocument" : Convert.ToString(Eval("ActualDocument_Type")) == ".PDF" ? "showModalPreviewDocument" : "hideItem" %>'>
                                                            <asp:Label ID="lblDocumentName" runat="server" Text='<%#Eval("ActualDocument_Name")%>'></asp:Label></a>
                                                        <asp:Label ID="lblName" runat="server" Text='<%#Eval("ActualDocument_Name")%>' Visible='<%#Convert.ToString(Eval("ActualDocument_Type")) == ".pdf" ? false : Convert.ToString(Eval("ActualDocument_Type")) == ".xls" ? false : Convert.ToString(Eval("ActualDocument_Type")) == ".xlsx" ? false : Convert.ToString(Eval("ActualDocument_Type")) == ".docx" ? false : Convert.ToString(Eval("ActualDocument_Type")) == ".doc" ? false : Convert.ToString(Eval("ActualDocument_Type")) == ".PDF" ? false : true %>'></asp:Label>
                                                    </ItemTemplate>

                                                    <ItemStyle HorizontalAlign="Left"></ItemStyle>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Document Type" SortExpression="ActualDocument_Type">
                                                    <ItemTemplate>
                                                        <%#GetDocumentName(Eval("ActualDocument_Type").ToString())%>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:BoundField DataField="ActualDocument_CurrentStatus" HeaderText="Current Status" SortExpression="ActualDocument_CurrentStatus">
                                                    <HeaderStyle HorizontalAlign="Left" />
                                                </asp:BoundField>
                                                <asp:BoundField DataField="ProjectRef_Number" HeaderText="ONTB Reference #" SortExpression="ProjectRef_Number">
                                                    <HeaderStyle HorizontalAlign="Left" />
                                                </asp:BoundField>
                                                <asp:BoundField DataField="Ref_Number" HeaderText="Originator Reference #" SortExpression="Ref_Number">
                                                    <HeaderStyle HorizontalAlign="Left" />
                                                </asp:BoundField>
                                                <asp:BoundField DataField="ActualDocument_CreatedDate" HeaderText="Contractor Uploaded Date" SortExpression="ActualDocument_CreatedDate" DataFormatString="{0:dd/MM/yyyy}">
                                                    <HeaderStyle HorizontalAlign="Left" />
                                                </asp:BoundField>
                                                <asp:BoundField DataField="Flow_Name" HeaderText="Flow Name" SortExpression="Flow_Name" >
                                                    <HeaderStyle HorizontalAlign="Left" />
                                                </asp:BoundField>
                                                <asp:TemplateField>
                                                    <ItemTemplate>
                                                        <asp:LinkButton ID="LnkDownloadnew" runat="server" CausesValidation="false" CommandArgument='<%#Eval("ActualDocumentUID")%>' CommandName="download">Download</asp:LinkButton>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField>
                                                    <ItemTemplate>
                                                        <a class="showModalDocumentMail" href="/_modal_pages/view-documenthistory.aspx?DocID=<%#Eval("ActualDocumentUID")%>&ProjectUID=<%#Eval("ProjectUID")%>">View History</a>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField>
                                                    <ItemTemplate>
                                                        <a class="showModalDocumentView" href="/_modal_pages/view-documentdetails.aspx?DocID=<%#Eval("ActualDocumentUID")%>&ProjectUID=<%#Eval("ProjectUID")%>">View Details</a>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:BoundField DataField="CreatedDate" HeaderText="ONTB Accepted/Rejected Date" SortExpression="CreatedDate" DataFormatString="{0:dd/MM/yyyy}">
                                                    <HeaderStyle HorizontalAlign="Left" />
                                                </asp:BoundField>
                                                <asp:BoundField DataField="Remarks" HeaderText="Remarks" SortExpression="Remarks">
                                                    <HeaderStyle HorizontalAlign="Left" />
                                                </asp:BoundField>

                                            </Columns>
                                            <PagerSettings FirstPageText="First" LastPageText="Last" Mode="NextPreviousFirstLast" NextPageText="Next" PreviousPageText="Previous" />
                                            <PagerStyle CssClass="pager" />
                                        </asp:GridView>







                                    </div>
                                    <div id="DivFooterRowNew" style="overflow: hidden"></div>
                                </div>

                            </div>


                    </div>
                </div>
            </div>




        </div>
    </div>
</asp:Content>

