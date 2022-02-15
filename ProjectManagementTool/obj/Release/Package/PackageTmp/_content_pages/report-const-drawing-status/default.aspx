<%@ Page Title="" Language="C#" MasterPageFile="~/_master_pages/default.Master" AutoEventWireup="true" CodeBehind="default.aspx.cs" Inherits="ProjectManagementTool._content_pages.report_const_drawing_status._default" %>
<asp:Content ID="Content1" ContentPlaceHolderID="default_master_head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="default_master_body" runat="server">
     <div class="container-fluid">
            <div class="row">
                <div class="col-md-6 col-lg-3 form-group">Construction Drawings Status Report</div>
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
        </div>
     <div class="container-fluid" id="divTabular" runat="server" visible="false">
            <div class="row">
                <div class="col-lg-12 col-xl-12 form-group">
                <div class="card mb-4">
                    <div class="card-body">
                        <h5 id="headingTb" runat="server" style="text-align:center">Status of Construction Drawings</h5>
                        <div class="table-responsive">
                             <div class="text-right">
                                 <asp:Button ID="btnexcelexport" runat="server" Text="Export to Excel" CssClass="btn btn-primary" OnClick="btnexcelexport_Click" />
                                <asp:Button ID="btnexport" runat="server" Text="Export to PDF" CssClass="btn btn-primary" OnClick="btnexport_Click" />
                                 <asp:Button ID="btnPrintPDF" runat="server" Text="Print" CssClass="btn btn-primary" OnClick="btnPrintPDF_Click" />
                            </div><br />
                           <asp:GridView ID="GrdPhysicalData" runat="server"   CssClass="table table-bordered" EmptyDataText="No Data" AutoGenerateColumns="False" Width="100%" AlternatingRowStyle-BackColor="lightGray" OnRowDataBound="GrdPhysicalData_RowDataBound">
                                <Columns>
                                     <asp:BoundField DataField="Sl_No" ItemStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center" HeaderStyle-CssClass="text-center"  HeaderText="Sl.No" >
                            </asp:BoundField>
                                     <%--<asp:TemplateField HeaderText="Sl.No" ItemStyle-HorizontalAlign="Left">
                                         <HeaderStyle HorizontalAlign="Center" />
                                        <ItemTemplate>
                                            <%#Eval("Sl_No")%>
                                        </ItemTemplate>
                                    </asp:TemplateField>--%>

                                    <asp:TemplateField HeaderText="Name Of Drawings" HeaderStyle-HorizontalAlign="Left" >
                                        <ItemTemplate>
                                            <%#Eval("DocName")%>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                     <%-- <asp:BoundField DataField="DocName" ItemStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Left" HeaderText="Name Of Drawings" >
                           
                            </asp:BoundField>--%>
                                     <asp:BoundField DataField="PendingDocuments" ItemStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center" HeaderStyle-CssClass="text-center" HeaderText="Drawings to be prepared" HtmlEncode="False" DataFormatString="{0:C2}">
                            
                            </asp:BoundField>
                                     <asp:BoundField DataField="SubmittedDocuments" HeaderText="No. of Drawings submitted to BWSSB" HeaderStyle-HorizontalAlign="Center" HeaderStyle-CssClass="text-center" HtmlEncode="False" DataFormatString="{0:C2}" ItemStyle-HorizontalAlign="Center">
                            
                            </asp:BoundField>
                                    <asp:BoundField DataField="ApprovedDocuments" ItemStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center" HeaderStyle-CssClass="text-center" HeaderText="No. of Drawings Approved" HtmlEncode="False" DataFormatString="{0:C2}">
                            
                            </asp:BoundField>
                                    <asp:TemplateField HeaderText="Remarks" HeaderStyle-HorizontalAlign="Left">
                                        <ItemTemplate>
                                            <%#Eval("Remarks")%>
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
