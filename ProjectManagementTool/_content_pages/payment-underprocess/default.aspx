<%@ Page Title="" Language="C#" MasterPageFile="~/_master_pages/default.Master" AutoEventWireup="true" CodeBehind="default.aspx.cs" Inherits="ProjectManagementTool._content_pages.payment_underprocess._default" %>
<asp:Content ID="Content1" ContentPlaceHolderID="default_master_head" runat="server">
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
                            <asp:Label ID="LblPprojectprogress" CssClass="text-uppercase font-weight-bold" runat="server" Text="Claims sent to CAAA/JICA"  />
                        </div>
                        <div class="table-responsive">
                            <asp:GridView ID="GrdProjectProgress" runat="server" HeaderStyle-HorizontalAlign="Center" ShowFooter="True" HeaderStyle-Font-Bold="true" EmptyDataText="No Records Found." Width="100%" CssClass="table table-bordered" AutoGenerateColumns="False" OnRowDataBound="GrdProjectProgress_RowDataBound">                                
                            <Columns>
                                <asp:BoundField DataField="Description" HeaderText="Sl. No." ItemStyle-HorizontalAlign="Center" HeaderStyle-Font-Bold="true" />
                                <asp:BoundField DataField="Description" HeaderText="Description" HeaderStyle-Font-Bold="true" ItemStyle-HorizontalAlign="Left" HeaderStyle-HorizontalAlign="Left" />
                                <asp:BoundField DataField="Amount" HeaderText="Amount (in MJPY)" HeaderStyle-Font-Bold="true"  ItemStyle-HorizontalAlign="Right" />
                                <%--<asp:BoundField DataField="Status" HeaderText="Status" HeaderStyle-Font-Bold="true" />--%>
                                <asp:BoundField DataField="Remarks" HeaderText="Remarks" HeaderStyle-Font-Bold="true" ItemStyle-HorizontalAlign="Left" HeaderStyle-HorizontalAlign="Left" />

                                <%--<asp:TemplateField HeaderText="Sl.No">
                                        <ItemTemplate>
                                            <%# Container.DataItemIndex + 1 %>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                              
                                <asp:TemplateField HeaderText="Description">      
                                    <ItemTemplate>
                                        <asp:Label ID="Label2" runat="server" Text='<%#Eval("Description")%>'></asp:Label>
                                    </ItemTemplate>
                                     <FooterTemplate>
                                          <asp:label id="LblTotal" Font-Bold="true"  runat="server" Text="Total"/>
                                     </FooterTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Amount &lt;br/&gt;(Crores INR)">
                                    <ItemTemplate>
                                        <asp:Label ID="LblAmount" runat="server" Text='<%#Eval("Amount")%>'></asp:Label>
                                    </ItemTemplate>
                                     <FooterTemplate>
                                      <asp:label id="lblSummary" Font-Bold="true"  runat="server"/>
                                    </FooterTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Status">
                                    <ItemTemplate>
                                       <asp:Label ID="Lblstatus" runat="server" Text='<%#Eval("Status")%>'></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Remarks">
                                    <ItemTemplate>
                                       <asp:Label ID="LblRemarks" runat="server" Text='<%#Eval("Remarks")%>'></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>--%>
                                
                            </Columns>
                            </asp:GridView>
                              <div class="text-right">
                            <asp:Button ID="btnExportPDF" runat="server" Text="Export to PDF" CausesValidation="false" CssClass="btn btn-primary" OnClick="btnExportPDF_Click" />
                          </div>
                         </div>
                        
                        </div>
                    </div>
                </div>
            </div>
        </div>
</asp:Content>
