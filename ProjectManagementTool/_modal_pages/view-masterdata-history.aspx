<%@ Page Title="" Language="C#" MasterPageFile="~/_master_pages/modal.Master" AutoEventWireup="true" CodeBehind="view-masterdata-history.aspx.cs" Inherits="ProjectManagementTool._modal_pages.view_masterdata_history" %>
<asp:Content ID="Content1" ContentPlaceHolderID="modal_master_head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="modal_master_body" runat="server">
    <form id="frmTaskScheduleHistory" runat="server">
        <div class="container-fluid" style="overflow-y:auto; min-height:85vh;">
             <div class="row">
            <div class="col-sm-12">
                <div class="table-responsive">
                    <asp:GridView ID="grdHistory" runat="server" CssClass="table table-bordered" EmptyDataText="No Data" AutoGenerateColumns="False" Width="100%">
                                                    <Columns>
                                                        
                                                         <asp:BoundField DataField="CreatedDate" HeaderText="Date Edited" DataFormatString="{0:dd/MM/yyyy hh:mm tt}">
                                                            <HeaderStyle HorizontalAlign="Left" />
                                                            </asp:BoundField>
                                                            <asp:BoundField DataField="UserName" HeaderText="User Name" >
                                                            <HeaderStyle HorizontalAlign="Left" />
                                                            </asp:BoundField>
                                                            <asp:BoundField DataField="Value" HeaderText="Previous Value">
                                                            <HeaderStyle HorizontalAlign="Left" />
                                                            </asp:BoundField>
                                                          
                                                    </Columns>
                                                 </asp:GridView>
                    </div>
                </div>
                 </div>
            </div>

       

        </form>
</asp:Content>
