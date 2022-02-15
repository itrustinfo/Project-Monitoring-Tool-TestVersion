<%@ Page Title="" Language="C#" MasterPageFile="~/_master_pages/default.Master" AutoEventWireup="true" CodeBehind="default.aspx.cs" Inherits="ProjectManagementTool._content_pages.complaints_mom._default" %>
<asp:Content ID="Content1" ContentPlaceHolderID="default_master_head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="default_master_body" runat="server">
    <div class="container-fluid">
            <div class="row">
                <div class="col-md-6 col-lg-4 form-group">
                    <label class="sr-only" for="lblname">Compliance of M.O.M</label>
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
            <div class="col-lg-6 col-xl-12 form-group">
                <div class="card mb-4">
                    <div class="card-body">
                        <div class="card-title">
                            <div class="d-flex justify-content-between">
                                <h6 class="text-muted">
                                    <asp:Label ID="LblHeading" CssClass="text-uppercase font-weight-bold" runat="server" />
                                </h6>
                            </div>
                        </div>
                        <div class="table-responsive">
                            <asp:GridView ID="GrdCompliance" runat="server" HeaderStyle-Font-Bold="true" AutoGenerateColumns="False" EmptyDataText="No Data Found" Width="100%" CssClass="table table-bordered">
                               <Columns>
                                    <asp:BoundField DataField="Meeting_Points" HeaderText="Points of Last Review Meeting ">
                                     <HeaderStyle HorizontalAlign="Left" />
                                    </asp:BoundField>
                                    <asp:BoundField DataField="Meeting_Status" HeaderText="Status">
                                     <HeaderStyle HorizontalAlign="Left" />
                                     </asp:BoundField>      
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
