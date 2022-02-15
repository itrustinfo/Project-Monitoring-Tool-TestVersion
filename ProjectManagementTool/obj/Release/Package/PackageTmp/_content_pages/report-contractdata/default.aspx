<%@ Page Title="" Language="C#" MasterPageFile="~/_master_pages/default.Master" AutoEventWireup="true" CodeBehind="default.aspx.cs" Inherits="ProjectManagementTool._content_pages.report_contractdata._default" %>
<asp:Content ID="Content1" ContentPlaceHolderID="default_master_head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="default_master_body" runat="server">
    <div class="container-fluid">
            <div class="row">
                <div class="col-md-12 col-lg-4 form-group">Contractor Data</div>
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
    <div class="container-fluid" id="ContractData" runat="server">
        
            <div class="row">
                   
                <div class="col-lg-12 col-xl-12 form-group">
                <div class="card mb-4">
                    <div class="card-body">
                        <div class="col-lg-12 col-xl-12 form-group" align="center">
                                    <h5 id="headingReport" style="font-weight:bold;" runat="server">Contract Data</h5>
                                    <h5 id="headingProject" runat="server">Contract Data</h5>
                                    </div>
                        <div class="card-title">
                        <div class="d-flex justify-content-between">
                              <h6 class="text-muted">
                                   <%--<asp:Label ID="ActivityHeading" CssClass="text-uppercase font-weight-bold" runat="server" Text="Contract Details" />--%>
                               </h6>
                            <div>
                                <asp:Button ID="btnexcelexport" runat="server" Text="Export to Excel" CssClass="btn btn-primary" OnClick="btnexcelexport_Click"/>
                                <asp:Button ID="btnExportReportPDF" runat="server" Text="Export PDF" CssClass="btn btn-primary" OnClick="btnExportReportPDF_Click" />
                                <asp:Button ID="btnPrint" runat="server" Text="Print" CssClass="btn btn-primary" OnClick="btnPrint_Click" />
                            </div>
                          </div>
                            </div>
                        <div class="table-responsive" id="GridExport" runat="server">
                                           
                                
                                <asp:GridView ID="GrdContractData" EmptyDataText="No Data Found." runat="server" Width="100%" ShowHeader="false" AutoGenerateColumns="false" 
                                            AllowPaging="false" CssClass="table table-bordered">
                                       <Columns>
                                           <asp:TemplateField ItemStyle-HorizontalAlign="Left" ItemStyle-Font-Bold="true" ItemStyle-Width="40%">
                                               <ItemTemplate>
                                                   <%#Eval("Description")%>
                                               </ItemTemplate>
                                           </asp:TemplateField>
                                           <asp:TemplateField ItemStyle-HorizontalAlign="Left" ItemStyle-Width="60%">
                                               <ItemTemplate>
                                                   <%#Eval("Value")%>
                                               </ItemTemplate>
                                           </asp:TemplateField>
                                       </Columns>
                                       </asp:GridView>
                                <%--<table class="table table-bordered">
                                    <tr>
                                        <td>
                                            <b>Name of the Project</b>
                                        </td>
                                        <td>
                                            <asp:Label ID="LblNameoftheProject" runat="server"></asp:Label>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <b>Funding Agency</b>
                                        </td>
                                        <td>
                                            <asp:Label ID="LblFundingAgency" runat="server"></asp:Label>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <b>Title of the Work</b>
                                        </td>
                                        <td>
                                            <asp:Label ID="LblTitleoftheWork" runat="server"></asp:Label>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <b>Location</b>
                                        </td>
                                        <td>
                                            <asp:Label ID="LblLocation" runat="server"></asp:Label>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <b>Client</b>
                                        </td>
                                        <td>
                                            <asp:Label ID="LblClient" runat="server"></asp:Label>
                                        </td>
                                    </tr>
                                  
                                    <tr>
                                        <td>
                                            <b>Contractor</b>
                                        </td>
                                        <td>
                                            <asp:Label ID="LblContractor" runat="server"></asp:Label>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <b>Contractor Representatives</b>
                                        </td>
                                        <td>
                                            <asp:Label ID="LblContractorRepresentatives" runat="server"></asp:Label>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <b>Type of Contract</b>
                                        </td>
                                        <td>
                                            <asp:Label ID="LblTypeofContract" runat="server"></asp:Label>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <b>Contract Value</b>
                                        </td>
                                        <td>
                                            <asp:Label ID="LblContractValue" runat="server"></asp:Label>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <b>Letter of Acceptance</b>
                                        </td>
                                        <td>
                                            <asp:Label ID="LblLetterofAcceptance" runat="server"></asp:Label>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <b>Contract Agreement Date</b>
                                        </td>
                                        <td style="text-align:left">
                                            <asp:Label ID="LblContractAgressmentDate" runat="server"></asp:Label>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <b>Contract Duration</b>
                                        </td>
                                        <td style="text-align:left">
                                            <asp:Label ID="LblContractDuration" runat="server"></asp:Label>
                                        </td>
                                    </tr>
                                    <tr style="text-align:left">
                                        <td>
                                            <b>Contract StartDate</b>
                                        </td>
                                        <td>
                                            <asp:Label ID="LblContractStartDate" runat="server"></asp:Label>
                                        </td>
                                    </tr>
                                    <tr style="text-align:left">
                                        <td>
                                            <b>Contract Completion Date</b>
                                        </td>
                                        <td>
                                            <asp:Label ID="LblContractCompletionDate" runat="server"></asp:Label>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <b>Defects Liability Period</b>
                                        </td>
                                        <td>
                                            <asp:Label ID="LblDefectsLiabilityPeriod" runat="server"></asp:Label>
                                        </td>
                                    </tr>
                                </table>--%>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
       </div>
</asp:Content>
