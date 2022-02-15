<%@ Page Title="" Language="C#" MasterPageFile="~/_master_pages/default.Master" AutoEventWireup="true" CodeBehind="default.aspx.cs" Inherits="ProjectManagementTool._content_pages.report_projectphysicalprogress._default" %>
<asp:Content ID="Content1" ContentPlaceHolderID="default_master_head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="default_master_body" runat="server">
         <div class="container-fluid">
        <div class="row">
            <div class="col-lg-6 col-xl-8 form-group">Physical Progress Achieved</div>
            <div class="col-lg-6 col-xl-4 form-group">
                
            </div>
        </div>
    </div>
     <div class="container-fluid">
        <div class="row">
            <div class="col-lg-6 col-xl-12 form-group">
                <div class="card mb-4">
                    <div class="card-body">
                        <div class="card-title">
                            <div class="d-flex justify-content-between">
                                <h6 class="text-muted">
                                    <asp:Label Text="Physical Progress Achieved" CssClass="text-uppercase font-weight-bold" runat="server" />
                                    <%--<asp:Label Text="Foo bar" CssClass="pl-1" runat="server" />--%>
                                </h6>
                            </div>
                        </div>
                        <div class="table-responsive">
                            <label class="lblCss" for="txtxsummary">Select Review Meeting</label>
                            <asp:DropDownList ID="DDLMeetingMaster" runat="server" CssClass="form-control" Width="30%" AutoPostBack="true" OnSelectedIndexChanged="DDLMeetingMaster_SelectedIndexChanged">

                            </asp:DropDownList>
                            <br /><br />
                            <asp:GridView ID="GrdPhysicalProgress" runat="server" ShowFooter="true" HeaderStyle-HorizontalAlign="Center" HeaderStyle-Font-Bold="true" CssClass="table table-bordered" AutoGenerateColumns="False" PageSize="20" EmptyDataText="No Data" Width="100%" OnRowDataBound="GrdPhysicalProgress_RowDataBound">
                                <Columns>
                                    <asp:TemplateField HeaderText="Sl.No" ItemStyle-HorizontalAlign="Center">
                                        <ItemTemplate>
                                            <%# Container.DataItemIndex + 1 %>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Project Name" ItemStyle-HorizontalAlign="Center">
                                        <ItemTemplate>
                                            <%#GetProjectName(Eval("ProjectUID").ToString())%>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Name of the Package" ItemStyle-HorizontalAlign="Left" FooterStyle-HorizontalAlign="Right">
                                        <ItemTemplate>
                                            <%#Eval("NameofthePackage")%>
                                        </ItemTemplate>
                                         <FooterTemplate>
                                            <asp:Label ID="lbltotal" runat="server" Text="Total" Font-Bold="true"></asp:Label>
                                        </FooterTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Awarded/Sanctioned excluding Provisional Sum and Physical Contigencies(Rs. Cr)" ItemStyle-Width="10%" ItemStyle-HorizontalAlign="Right" FooterStyle-HorizontalAlign="Right">
                                        <ItemTemplate>
                                            <%#Eval("Awarded_Sanctioned_Value","{0:n}")%>
                                            <asp:Label ID="lblAwardedValue" runat="server" Text='<%#Eval("Awarded_Sanctioned_Value","{0:n}")%>'></asp:Label>
                                        </ItemTemplate>
                                         <FooterTemplate>
                                            <asp:Label ID="lblAwardedtotal" runat="server" Font-Bold="true"></asp:Label>
                                        </FooterTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Status of Award" ItemStyle-HorizontalAlign="Center">
                                        <ItemTemplate>
                                            <%#Eval("Award_Status")%>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Expenditure til Today(Rs. Cr)" FooterStyle-HorizontalAlign="Right" ItemStyle-HorizontalAlign="Right">
                                        <ItemTemplate>
                                            <%#Eval("Expenditure_As_On_Date","{0:n}")%>
                                            <asp:Label ID="LblExpenditure" runat="server" Text='<%#Eval("Expenditure_As_On_Date","{0:n}")%>'></asp:Label>
                                        </ItemTemplate>
                                         <FooterTemplate>
                                            <asp:Label ID="LblExpenditure_Total" runat="server" Font-Bold="true"></asp:Label>
                                        </FooterTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Targeted Physical Progress(%)" ItemStyle-HorizontalAlign="Center">
                                        <ItemTemplate>
                                            <%#Eval("Targeted_PhysicalProgress","{0:n}")%>%
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Targeted Overall Weighted Progress(%)" FooterStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center">
                                        <ItemTemplate>
                                            <%#Eval("Targeted_Overall_WeightedProgress","{0:n}")%>
                                            <asp:Label ID="LblTargetedProgress" runat="server" Text='<%#Eval("Targeted_Overall_WeightedProgress","{0:n}")%>'></asp:Label>%
                                        </ItemTemplate>
                                         <FooterTemplate>
                                            <asp:Label ID="LblTargetedOverallTotal" runat="server" Font-Bold="true"></asp:Label><b>%</b>
                                        </FooterTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Achieved Physical Progress(%)" ItemStyle-HorizontalAlign="Center">
                                        <ItemTemplate>
                                            <%#Eval("Achieved_PhysicalProgress","{0:n}")%>%
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Achieved Overall Weighted Progress(%)" ItemStyle-HorizontalAlign="Center" FooterStyle-HorizontalAlign="Center">
                                        <ItemTemplate>
                                            <%#Eval("Achieved_Overall_WeightedProgress","{0:n}")%>%
                                            <asp:Label ID="LblAchievedProgress" runat="server" Text='<%#Eval("Achieved_Overall_WeightedProgress","{0:n}")%>'></asp:Label>
                                        </ItemTemplate>
                                        <FooterTemplate>
                                            <asp:Label ID="LblAchievedOverallTotal" runat="server" Font-Bold="true"></asp:Label><b>%</b>
                                        </FooterTemplate>
                                    </asp:TemplateField>
                                </Columns>
                             </asp:GridView>
                            <div class="text-right">
                                <asp:Button ID="btnexport" runat="server" Text="Export to PDF" OnClick="btnexport_Click" CssClass="btn btn-primary" Visible="false" />
                                <asp:Button ID="btnExportNew" runat="server" Text="Export to PDF" CssClass="btn btn-primary" OnClick="btnExportNew_Click" />
                            </div>
                        </div>
                    </div>
                </div>
            </div>
            </div>
        </div>
</asp:Content>
