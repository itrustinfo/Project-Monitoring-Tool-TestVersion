<%@ Page Title="" Language="C#" MasterPageFile="~/_master_pages/default.Master" AutoEventWireup="true" CodeBehind="default.aspx.cs" Inherits="ProjectManagementTool._content_pages.report_document_status._default" %>

<asp:Content ID="Content1" ContentPlaceHolderID="default_master_head" runat="server">
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="default_master_body" runat="server">
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



    <div class="container-fluid" id="DivDocumentSummary" runat="server">
        <div class="row">
            <div class="col-lg-12 col-xl-12 form-group">
                <div class="card mb-4">
                    <div class="card-body">
                        <div class="col-lg-12 col-xl-12 form-group" align="center">
                            <h5 id="DocumentSummaryReportName" style="font-weight: bold;" runat="server">Document Summary</h5>
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
                                OnRowDataBound="GrdDocumentSummary_RowDataBound" AllowPaging="false" CssClass="table table-bordered">
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
                                    <asp:TemplateField HeaderText="Contractor" ItemStyle-HorizontalAlign="Center">
                                        <ItemTemplate>
                                            <%#Eval("Contractor")%>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="ONTB/PMC" ItemStyle-HorizontalAlign="Center">
                                        <ItemTemplate>
                                            <%#Eval("ONTB")%>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="BWSSB" ItemStyle-HorizontalAlign="Center">
                                        <ItemTemplate>
                                            <%#Eval("BWSSB")%>
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
