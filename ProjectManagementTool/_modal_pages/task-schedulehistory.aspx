<%@ Page Title="" Language="C#" MasterPageFile="~/_master_pages/modal.Master" AutoEventWireup="true" CodeBehind="task-schedulehistory.aspx.cs" Inherits="ProjectManagementTool._modal_pages.task_schedulehistory" %>
<asp:Content ID="Content1" ContentPlaceHolderID="modal_master_head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="modal_master_body" runat="server">
    <form id="frmTaskScheduleHistory" runat="server">
        <div class="container-fluid" style="overflow-y:auto; min-height:85vh;">
             <div class="row">
            <div class="col-sm-12">
                <div class="table-responsive">
                    <asp:GridView ID="GrdTaskSchedule" runat="server" DataKeyNames="TaskScheduleUID" CssClass="table table-bordered" Width="100%" AutoGenerateColumns="false" EmptyDataText="No Data Found.." AllowPaging="false" OnRowDataBound="GrdTaskSchedule_RowDataBound">
                                             <Columns>
                                                 <asp:BoundField DataField="StartDate" HeaderText="Start Date"  DataFormatString="{0:dd/MM/yyyy}">
                                                    <HeaderStyle HorizontalAlign="Left" />
                                                    </asp:BoundField>
                                                 <asp:BoundField DataField="EndDate" HeaderText="End Date"  DataFormatString="{0:dd/MM/yyyy}">
                                                    <HeaderStyle HorizontalAlign="Left" />
                                                    </asp:BoundField>
                                                 <asp:BoundField DataField="Schedule_Value" HeaderText="Target Value" >
                                                    <HeaderStyle HorizontalAlign="Left" />
                                                    </asp:BoundField>
                                                 <asp:TemplateField HeaderText="Achieved Value">
                                                     <ItemTemplate>
                                                         <asp:Label ID="LblAchieved" runat="server"></asp:Label>
                                                         <%--<asp:TextBox ID="txtachieved" runat="server" TextMode="Number" CssClass="form-control" Width="70%">0</asp:TextBox>--%>
                                                     </ItemTemplate>
                                                 </asp:TemplateField>
                                                <%-- <asp:BoundField DataField="TaskScheduleUID" HeaderText="TaskScheduleUID" HeaderStyle-CssClass="Hide" ItemStyle-CssClass="Hide">
                                                    <HeaderStyle HorizontalAlign="Left" />
                                                    </asp:BoundField>--%>
                                             </Columns>
                                         </asp:GridView>
                    </div>
                </div>
                 </div>
            </div>
        </form>
</asp:Content>
