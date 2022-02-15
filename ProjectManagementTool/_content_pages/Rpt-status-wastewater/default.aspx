<%@ Page Title="" Language="C#" MasterPageFile="~/_master_pages/default.Master" AutoEventWireup="true" CodeBehind="default.aspx.cs" Inherits="ProjectManagementTool._content_pages.Rpt_status_wastewater._default" %>
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
     <div class="container-fluid">
            <div class="row">
                <div class="col-lg-12 col-xl-12 form-group">
                <div class="card mb-4">
                    <div class="card-body">
                        <div class="table-responsive">
                            <h3 id="heading" runat="server"></h3>
                                <asp:GridView ID="GrdStatus" runat="server" HeaderStyle-Font-Bold="true" HeaderStyle-HorizontalAlign="Center" CssClass="table table-bordered" EmptyDataText="No Data" AutoGenerateColumns="False" Width="100%">
                                <Columns>
                                       <asp:BoundField DataField="Componenttype" HeaderText="Component Type" >
                            
                            </asp:BoundField>
                                      <asp:BoundField DataField="ProjectName" HeaderText="Contract Package" ItemStyle-HorizontalAlign="Center">
                            
                            </asp:BoundField>
                                     <asp:BoundField DataField="PackageDescription" HeaderText="Package Description" HtmlEncode="false">
                            
                            </asp:BoundField>
                                     <asp:BoundField DataField="AwardedCost" HeaderText="Awarded Cost / Sanction Cost <br/> excluding Provisional Sum and Physical Contingency (Rs. Crores)" ItemStyle-HorizontalAlign="Right" HtmlEncode="false">
                            
                            </asp:BoundField>
                                     <asp:BoundField DataField="ProjectComponent" HeaderText="Project Components" HtmlEncode="false">
                            
                            </asp:BoundField>
                                     <asp:BoundField DataField="PresentStatus" HeaderText="Present Status" HtmlEncode="false">
                            
                          
                                   </asp:BoundField>
                                </Columns>
                                </asp:GridView>
                             <div class="text-right">
                                <asp:Button ID="btnexport" runat="server" Text="Export to PDF" CssClass="btn btn-primary" OnClick="btnexport_Click" />
                            </div>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </div>
</asp:Content>
