<%@ Page Title="" Language="C#" MasterPageFile="~/_master_pages/default.Master" AutoEventWireup="true" CodeBehind="default.aspx.cs" Inherits="ProjectManagementTool._content_pages.report_resourcedeployment._default" %>
<asp:Content ID="Content1" ContentPlaceHolderID="default_master_head" runat="server">
   
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="default_master_body" runat="server">
    <div class="container-fluid">
            <div class="row">
                <div class="col-md-12 col-lg-4 form-group">Resource Deployment</div>
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
    <div class="container-fluid" id="ByMonth" runat="server">
            <div class="row">
                <div class="col-md-6 col-xl-4 mb-4">
                     <div class="card">
                        <div class="card-body">
                             <h6 class="card-title text-muted text-uppercase font-weight-bold">Report Format</h6>
                            <asp:RadioButtonList CssClass="form-control" ID="RBLReportFor" BorderStyle="None" runat="server" RepeatDirection="Horizontal">
                                <asp:ListItem Value="By Month" Selected="True">&nbsp;By Month</asp:ListItem>             
                        </asp:RadioButtonList>
                            </div>
                         </div>
                    </div>
                <div class="col-md-6 col-xl-8 mb-4">
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
                </div>
        </div>
    <div class="container-fluid" id="ResourceDeployment" runat="server">
        <div class="row">
            <div class="col-lg-12 col-xl-12 form-group">
            <div class="card mb-4">
                                    <div class="card-body">
                                        <div class="col-lg-12 col-xl-12 form-group" align="center">
                                    <h5 id="headingReport" style="font-weight:bold;" runat="server">Resource Deployment</h5>
                                    <h5 id="headingProject" runat="server">CP</h5>
                                    </div>
                                        <div class="card-title">
                                            <div class="d-flex justify-content-between">
                                                <h6 class="text-muted">
                                                    <%--<asp:Label ID="ActivityHeading" CssClass="text-uppercase font-weight-bold" runat="server" Text="Status of Resource Deployment in the Month" />--%>
                                                </h6>
                                                <div>
                                                    <asp:Button ID="btnexcelexport" runat="server" Text="Export to Excel" Visible="false" CssClass="btn btn-primary" OnClick="btnexcelexport_Click"/>
                                                <asp:Button ID="btnExportReportPDF" runat="server" Text="Export PDF" Visible="false" CssClass="btn btn-primary" OnClick="btnExportReportPDF_Click" />
                                                <asp:Button ID="btnPrintPDF" runat="server" Text="Print" CssClass="btn btn-primary" Visible="false" OnClick="btnPrintPDF_Click" />
                                            </div>
                                            </div>
                                        </div>
                                        <div class="table-responsive">
                                                <asp:GridView ID="GrdResourceDeployment" runat="server" AutoGenerateColumns="False" EmptyDataText="No Data Found" Width="100%" AlternatingRowStyle-BackColor="lightGray" CssClass="table table-bordered">
                                                    <Columns>
                                                        <asp:TemplateField HeaderText="Sl.No" ItemStyle-HorizontalAlign="Center" HeaderStyle-CssClass="text-center">
                                                            <ItemTemplate>
                                                                <%# Container.DataItemIndex + 1 %>
                                                            </ItemTemplate>

                                                    <ItemStyle HorizontalAlign="Center"></ItemStyle>
                                                        </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Resource Name">
                                                    <ItemTemplate>
                                                        <%#Eval("ResourceName")%>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Unit for Measurement">
                                                    <ItemTemplate>
                                                        <%#Eval("Unit_for_Measurement")%>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Planned" ItemStyle-HorizontalAlign="Center" HeaderStyle-CssClass="text-center">
                                                    <ItemTemplate>
                                                        <%#Eval("Planned")%>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                 <asp:TemplateField HeaderText="Deployed" ItemStyle-HorizontalAlign="Center" HeaderStyle-CssClass="text-center">
                                                    <ItemTemplate>
                                                        <%#Eval("Deployed")%>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                 <asp:BoundField DataField="Remarks" HeaderText="Remarks" >
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
</asp:Content>
