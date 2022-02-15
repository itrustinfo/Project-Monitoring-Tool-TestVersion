<%@ Page Title="" Language="C#" MasterPageFile="~/_master_pages/default.Master" AutoEventWireup="true" CodeBehind="default.aspx.cs" Inherits="ProjectManagementTool._content_pages.claims_CAAA_JICA._default" %>
<asp:Content ID="Content1" ContentPlaceHolderID="default_master_head" runat="server">
    
    <style type="text/css">
        .hideItem {
         display:none;
         
     }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="default_master_body" runat="server">
     <div class="container-fluid">
            <div class="row">
                <div class="col-md-6 col-lg-4 form-group">
                    <label class="sr-only" for="DDLProject">Select Meeting</label>
                    <div class="input-group">
                        <div class="input-group-prepend">
                            <span class="input-group-text">Select Meeting</span>
                        </div>
                        <asp:DropDownList ID="ddlmeeting" runat="server" CssClass="form-control" AutoPostBack="true" OnSelectedIndexChanged="ddlmeeting_SelectedIndexChanged"></asp:DropDownList>
                    </div>
                </div>
            </div>
        </div>
      <div class="container-fluid" id="ProjectProgress" runat="server">
        <div class="row" >
            <div class="col-lg-12 col-xl-12 form-group">
                <div class="card mb-4" >
                    <div class="card-body" >
                        <div class="card-title">
                            <asp:Label ID="LblPprojectprogress" CssClass="text-uppercase font-weight-bold" runat="server" Text="Claims sent to CAAA/JICA in Dec’2020"  />
                        </div>
                        <div class="table-responsive">
                            <asp:GridView ID="GrdProjectProgress" runat="server" ShowFooter="True" EmptyDataText="No Records Found." Width="100%" CssClass="table table-bordered" AutoGenerateColumns="False" OnRowDataBound="GrdProjectProgress_RowDataBound">                                
                            <Columns>
                                <asp:BoundField DataField="Description" ItemStyle-Width="10%" HeaderText="Sl. No." HeaderStyle-Font-Bold="true" />
                                <%--<asp:TemplateField HeaderText="Sl. No." ItemStyle-Width="70px" HeaderStyle-Font-Bold="true">
                                    <ItemTemplate>
                                        <%# Container.DataItemIndex + 1 %>
                                    </ItemTemplate>
                                </asp:TemplateField>--%>
                              <asp:BoundField DataField="Description" ItemStyle-Width="50%" HeaderText="Description" HeaderStyle-Font-Bold="true" />
                                <%--<asp:TemplateField HeaderText="Description" FooterStyle-HorizontalAlign="Right" HeaderStyle-Font-Bold="true">      
                                    <ItemTemplate>
                                        <asp:Label ID="Label2" runat="server" Text='<%# Bind("Description") %>'></asp:Label>
                                    </ItemTemplate>
                                                  <footertemplate>
                                              <asp:label id="LblTotal" Font-Bold="true"  runat="server" Text="Total"/>
                                            </footertemplate>
                                </asp:TemplateField>--%>
                              <asp:BoundField DataField="Amount" ItemStyle-Width="20%" HeaderText="Amount (in MJPY)" HeaderStyle-Font-Bold="true" />

                                <%--<asp:TemplateField HeaderText="Amount (Crores INR)" HeaderStyle-Font-Bold="true">
                                    <ItemTemplate>
                                        <asp:Label ID="LblAmount" runat="server" Text='<%# Bind("Amount") %>'></asp:Label>
                                    </ItemTemplate>
                                 <footertemplate>
                                      <asp:label id="lblSummary" Font-Bold="true"  runat="server"/>
                                    </footertemplate>
                                </asp:TemplateField>--%>

                                <asp:BoundField DataField="CAADate" DataFormatString="{0:dd MMM yyyy}" ItemStyle-Width="20%" HeaderText="Sent to CAAA/JICA" HeaderStyle-Font-Bold="true" />
                               
                                <%--<asp:TemplateField HeaderText="Sent to CAAA/JICA" >
                                    <ItemTemplate>
                                        <asp:Label ID="Label1" runat="server" Text='<%# Bind("[CAADate]","{0:dd MMM yyyy}") %>'></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>--%>
                            </Columns>
                               
                            </asp:GridView>
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
