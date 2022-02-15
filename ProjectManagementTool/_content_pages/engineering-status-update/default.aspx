<%@ Page Title="" Language="C#" MasterPageFile="~/_master_pages/default.Master" AutoEventWireup="true" CodeBehind="default.aspx.cs" Inherits="ProjectManagementTool._content_pages.engineering_status_update._default" %>
<asp:Content ID="Content1" ContentPlaceHolderID="default_master_head" runat="server">
    <style>
        .Hide
    {
        display : none;
    }
    </style>
          <link rel="stylesheet" href="//code.jquery.com/ui/1.12.1/themes/base/jquery-ui.css">
  <script src="https://code.jquery.com/jquery-1.12.4.js"></script>
  <script src="https://code.jquery.com/ui/1.12.1/jquery-ui.js"></script>
    <script type="text/javascript">
        function BindEvents() {
            $(".showTaskScheduleModal").click(function (e) {
                e.preventDefault();
                jQuery.noConflict();
                var url = $(this).attr("href");
                $("#ModTaskScheduleHistory iframe").attr("src", url);
                $("#ModTaskScheduleHistory").modal("show");
            });

            $(".showTaskMeasurementModal").click(function (e) {
                e.preventDefault();
                jQuery.noConflict();
                var url = $(this).attr("href");
                $("#ModTaskMeasurementHistory iframe").attr("src", url);
                $("#ModTaskMeasurementHistory").modal("show");
            });

            $(".showResourceDeploymentModal").click(function (e) {
                e.preventDefault();
                jQuery.noConflict();
                var url = $(this).attr("href");
                $("#ModResourceDeploymentHistory iframe").attr("src", url);
                $("#ModResourceDeploymentHistory").modal("show");
            });
        }

        function DateText() {
            $(function () {
                $("input[id$='dtprojectDate']").datepicker({
                    changeMonth: true,
                    changeYear: true,
                    dateFormat: 'dd/mm/yy'
                });
                $(".datepick").datepicker({
                changeMonth: true,
                changeYear: true,
                dateFormat: 'dd/mm/yy'
                });
                 
            });
        }
        $(document).ready(function () {

            DateText();
             BindEvents();
        });
</script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="default_master_body" runat="server">
    <asp:ScriptManager ID="ScriptManager1" runat="server">
   </asp:ScriptManager>
    <%--<div id="loader"></div>--%>
    <%--project selection dropdowns--%>
        <div class="container-fluid">
            <div class="row">
                
                <div class="col-md-6 col-lg-4 form-group">
                    <label class="sr-only" for="DDLProject">Project</label>
                    <div class="input-group">
                        <div class="input-group-prepend">
                            <span class="input-group-text">Project</span>
                        </div>
                        <asp:DropDownList ID="ddlProject" runat="server" CssClass="form-control" AutoPostBack="true" OnSelectedIndexChanged="ddlProject_SelectedIndexChanged"></asp:DropDownList>
                    </div>
                </div>
                <div class="col-md-6 col-lg-4 form-group">
                    <label class="sr-only" for="DDLWorkPackage">Work Package</label>
                    <div class="input-group">
                        <div class="input-group-prepend">
                            <span class="input-group-text">Work Package</span>
                        </div>
                        <asp:DropDownList ID="ddlworkpackage" runat="server" CssClass="form-control" AutoPostBack="true" OnSelectedIndexChanged="ddlworkpackage_SelectedIndexChanged"></asp:DropDownList>
                    </div>
                </div>
                <%--<div class="col-md-12 col-lg-4 form-group">Engineering Status Update</div>--%>
                <div class="col-md-12 col-lg-4 form-group"></div>
            </div>
        </div>
    <div class="container-fluid">
        <div class="row">
          <div class="col-lg-6 col-xl-12 form-group">
                        <div class="card">
                            <div class="card-body" style="padding-bottom:0; margin-bottom:0;">
                                <div class="row">
                         <div class="col-lg-2">
                            <h6 class="card-title text-muted text-uppercase font-weight-bold">Status Update</h6>

                        </div>
                        <div class="col-lg-5">
                            <asp:RadioButtonList ID="RDBStatusUpdateView" runat="server" class="card-title text-muted text-uppercase font-weight-bold" AutoPostBack="true" Width="50%" RepeatDirection="Horizontal" OnSelectedIndexChanged="RDBStatusUpdateView_SelectedIndexChanged">
                            <asp:ListItem Value="Engineering">&nbsp;Engineering</asp:ListItem>
                            <asp:ListItem Value="Resource Deployment">&nbsp;Resource Deployment</asp:ListItem>
                        </asp:RadioButtonList>
                        </div>
                                    <div class="col-lg-3">
                                        </div>
                            <div class="col-lg-2">
                            </div>
                                </div>
                            </div>
                        </div>
                       
                     </div>  
            </div>
    </div>

    <div class="container-fluid" id="DivEngineering" runat="server" visible="false">
        <asp:UpdatePanel ID="UpdatePanel2" runat="server" UpdateMode="Conditional">
                    <ContentTemplate>
                    <div class="row">
                        <div class="col-lg-6 col-xl-4 form-group">
                            <div class="card h-100" style="max-height:700px; overflow-y:auto;">
                                <div class="card-body">
                                    <h6 class="card-title text-muted text-uppercase font-weight-bold">WorkPackage Directory</h6>
                                    <div class="form-group">
                                        <label class="sr-only" for="TxtSearchDocuments">Search</label>
                                        <div class="input-group">
                                            <input id="TxtSearchDocuments" class="form-control" type="text" placeholder="Activity name" />
                                            <div class="input-group-append">
                                                <asp:Button ID="BtnSearchDocuments" CssClass="btn btn-primary" Text="Search" runat="server" />
                                            </div>
                                        </div>
                                    </div>
                                    <asp:TreeView runat="server" CssClass="it_tree_view" ID="TreeView1" ImageSet="XPFileExplorer" OnSelectedNodeChanged="TreeView1_SelectedNodeChanged" NodeIndent="15" EnableTheming="True" NodeWrap="True">                               
                                        <NodeStyle CssClass="it_tree_view__node" NodeSpacing="2px" />
                                        <ParentNodeStyle Font-Bold="False" />
                                        <SelectedNodeStyle CssClass="it_tree_view__node__selected" HorizontalPadding="4px" VerticalPadding="2px" />
                                    </asp:TreeView>    
                                </div>
                            </div>
                        </div>
                        <div class="col-lg-6 col-xl-8 form-group">
                            <asp:UpdateProgress ID="UpdateProgress2" runat="Server" AssociatedUpdatePanelID="UpdatePanel2" DisplayAfter="1"> 
                                        <ProgressTemplate > 
                                        <!-- <asp:Label ID="lblwait" runat="server" Text="Please wait while we fetch the data.." ForeColor="Red"></asp:Label>-->
                                            <div id="loader"></div>
                                        </ProgressTemplate> 
                                    </asp:UpdateProgress>
                                <div class="card mb-4">
                                    <div class="card-body">
                                        <div class="card-title">
                                            <div class="d-flex justify-content-between">
                                                <h6 class="text-muted">
                                                    <asp:Label ID="ActivityHeading" CssClass="text-uppercase font-weight-bold" runat="server" />
                                                    <%--<asp:Label Text="Foo bar" CssClass="pl-1" runat="server" />--%>
                                                </h6>
                                            </div>
                                        </div>
                                        <div class="table-responsive">
                                         <div id="divStatus" runat="server" visible="false">
                                         <b>Activity Status Update</b>
                                         <table class="table table-bordered">
                                             <%--<tr><td><strong>Last Status</strong></td><td><strong>Date</strong></td><td><strong>Current Status</strong></td><td><strong>Task Percentage(%)</strong></td><td><strong>Comments</strong></td><td></td></tr>--%>
                                             <tr><td><strong>Last Status</strong></td><td><strong>Date</strong></td><td><strong>Current Status</strong></td><td></td></tr>
                                              <tr><td>
                                                  <asp:Label ID="lblLastStatus" runat="server">-</asp:Label>
                                                  </td><td>
                                                      <asp:Label ID="lblDateL" runat="server" >-</asp:Label>
                                                  </td><td>
                                                   <asp:DropDownList ID="ddlpStatus" runat="server" CssClass="form-control" Width="140px" AutoPostBack="true" OnSelectedIndexChanged="ddlpStatus_SelectedIndexChanged">
                                                                          </asp:DropDownList>
                                                   </td>
                                                <%--  <td>
                                                      <asp:TextBox ID="txtpercentage" runat="server" Width="80px" TextMode="Number" CssClass="form-control" ></asp:TextBox>
                                                  </td>
                                                  <td>
                                                      <asp:TextBox ID="txtcomments" runat="server" CssClass="form-control" TextMode="MultiLine"></asp:TextBox>
                                                  </td>--%>
                                                  <td>
                                                  <asp:Button ID="btnUpdateStatus" runat="server" Text="Update" CssClass="btn btn-primary" OnClick="btnUpdateStatus_Click"/>
                                                  </td></tr>
                                         </table>
                                        <br />

                                             <div class="card-title">
                                            <div class="d-flex justify-content-between">
                                                <%--<b>Activity Schedule Update</b>--%>
                                                <b>Measurement Update</b>
                                                <div>
                                                 <a id="TaskScheduleHistory" runat="server"  href="/_modal_pages/task-schedulehistory.aspx"  class="showTaskMeasurementModal"><asp:Button ID="btnTaskScheduleHistory" runat="server" Text="View History" CausesValidation="false"  CssClass="btn btn-primary" align="end"></asp:Button></a>  
                                             </div>
                                            </div>
                                        </div>
                                              <asp:HiddenField ID="HiddenTaskScheduleUID" runat="server" />
                                             <asp:HiddenField ID="HiddenProgressExceeds" runat="server" />
                                         <table class="table table-bordered" id="ActivitySchedule" runat="server">
                                            
                                             <tr><td><strong>Year</strong></td><td><strong>Month</strong></td><td><strong>Day</strong></td><td><strong>Target</strong></td><td><strong>Achieved (Cumulative)</strong></td><td><strong>Achieved (Current)</strong></td><td><strong>Remarks</strong></td></tr>
                                              <tr><td>
                                                  <asp:DropDownList ID="DDLYear" runat="server" CssClass="form-control" AutoPostBack="true" Width="110px"  OnSelectedIndexChanged="DDLYear_SelectedIndexChanged"></asp:DropDownList>
                                                  
                                                  </td><td>
                                                      <asp:DropDownList ID="DDLMonth" runat="server" CssClass="form-control" AutoPostBack="true" Width="130px"  OnSelectedIndexChanged="DDLMonth_SelectedIndexChanged"></asp:DropDownList>
                                                  </td>
                                                  <td>
                                                      <asp:DropDownList ID="DDLDay" runat="server" CssClass="form-control" Width="70px"></asp:DropDownList>
                                                  </td>
                                                  <td>
                                                        <asp:Label ID="LblTragetValue" runat="server" Text="-" Width="110px" CssClass="form-control" autocomplete="off"></asp:Label>
                                                   </td>
                                                  <td>
                                                      <asp:TextBox ID="txtachieved" runat="server" CssClass="form-control" Enabled="false" Width="110px" autocomplete="off" ></asp:TextBox>
                                                      <asp:Label ID="LblWarningNew" runat="server" ForeColor="Red" Font-Size="Small"></asp:Label>
                                                      <asp:Label ID="LblTaskUnitofProgress" runat="server" ForeColor="Red" CssClass="Hide" Font-Size="Small"></asp:Label>
                                                  </td>
                                                  <td>
                                                        <asp:TextBox ID="txtAchievedCurrent" Width="110px" CssClass="form-control" runat="server" autocomplete="off"></asp:TextBox>
                                                  </td>
                                                  <td>
                                                      <asp:TextBox ID="txtMeasurementRemarks" runat="server" TextMode="MultiLine" CssClass="form-control" autocomplete="off" Width="150px" ></asp:TextBox>
                                                  </td>
                                                  <%--<td>
                                                      <asp:FileUpload ID="SiteUploadPhotograph" runat="server" AllowMultiple="true" Width="150px" />
                                                    
                                                  </td>--%>
                                                  </tr>
                                             <tr>
                                                 <td colspan="7">
                                                  <asp:Button ID="btnScheduleUpdate" runat="server" Text="Update" CssClass="btn btn-primary" OnClick="btnScheduleUpdate_Click"/>
                                                  </td>
                                             </tr>
                                         </table>
                                             <asp:Label ID="LblNotDataTaskSchedule" Font-Bold="true" runat="server"></asp:Label>
                                         <%--<asp:GridView ID="GrdTaskSchedule" runat="server" DataKeyNames="TaskScheduleUID" CssClass="table table-bordered" Width="100%" AutoGenerateColumns="false" AllowPaging="false" OnRowDataBound="GrdTaskSchedule_RowDataBound" OnRowCommand="GrdTaskSchedule_RowCommand">
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
                                                         <asp:TextBox ID="txtachieved" runat="server" TextMode="Number" CssClass="form-control" Width="70%">0</asp:TextBox>
                                                            <asp:Label ID="LblWarningNew" runat="server" ForeColor="Red" Font-Size="Small"></asp:Label>
                                                     </ItemTemplate>
                                                 </asp:TemplateField>
                                                 <asp:TemplateField>
                                                        <ItemTemplate>
                                                            <asp:Button ID="btnUpdate" runat="server" Text="Update" CssClass="btn btn-primary" CommandName="TaskEdit" CommandArgument="<%# ((GridViewRow) Container).RowIndex %>" CausesValidation="false" />
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                 <asp:BoundField DataField="TaskScheduleUID" HeaderText="TaskScheduleUID" HeaderStyle-CssClass="Hide" ItemStyle-CssClass="Hide">
                                                    <HeaderStyle HorizontalAlign="Left" />
                                                    </asp:BoundField>
                                             </Columns>
                                         </asp:GridView>--%>
                                         <br /><br />
                                        <b>Resource Usage Update</b>

                                                        <asp:GridView ID="grdResources" runat="server" CssClass="table table-bordered" AutoGenerateColumns="False" Width="100%" OnRowCommand="grdResources_RowCommand" OnRowDataBound="grdResources_RowDataBound">
                                                <Columns>
                                                     <asp:BoundField DataField="ResourceUID" HeaderText="Resource UID" ItemStyle-HorizontalAlign="Left" ItemStyle-CssClass="Hide" HeaderStyle-CssClass="Hide">
                    
                                <ItemStyle HorizontalAlign="Center"></ItemStyle>
                    
                                                    </asp:BoundField>
                                                     <asp:BoundField DataField="WorkPackageUID" HeaderText="WorkPackage UID" ItemStyle-HorizontalAlign="Left" ItemStyle-CssClass="Hide" HeaderStyle-CssClass="Hide">
                    
                                <ItemStyle HorizontalAlign="Center"></ItemStyle>
                    
                                                    </asp:BoundField>
                                                     <asp:BoundField DataField="ProjectUID" HeaderText="Project UID" ItemStyle-HorizontalAlign="Left" ItemStyle-CssClass="Hide" HeaderStyle-CssClass="Hide">
                    
                                <ItemStyle HorizontalAlign="Center"></ItemStyle>
                    
                                                    </asp:BoundField>
                                                     <asp:BoundField DataField="TaskUID" HeaderText="Task UID" ItemStyle-HorizontalAlign="Left" ItemStyle-CssClass="Hide" HeaderStyle-CssClass="Hide">
                    
                                <ItemStyle HorizontalAlign="Center"></ItemStyle>
                    
                                                    </asp:BoundField>
                                                    <asp:BoundField DataField="Description" HeaderText="BOQ Description" ItemStyle-HorizontalAlign="Left" >
                    
                                <ItemStyle HorizontalAlign="Center"></ItemStyle>
                    
                                                    </asp:BoundField>
                                                    <asp:BoundField DataField="AllocatedUnits" HeaderText="Allocated Units"  >
                                                    <HeaderStyle HorizontalAlign="Center" />
                                                    </asp:BoundField>
                                                    <asp:BoundField DataField="AllocatedUnits" HeaderText="Total Usage" >
                                                    <HeaderStyle HorizontalAlign="Center" />
                                                    </asp:BoundField>
                                                    <asp:TemplateField HeaderText="Current Usage">
                                                        <ItemTemplate>
                                                            <asp:TextBox ID="txtCurrentUsage" runat="server" TextMode="Number" CssClass="form-control" Width="150px">0</asp:TextBox>
                                                            <asp:Label ID="LblWarning" runat="server" ForeColor="Red" Font-Size="Small"></asp:Label>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField>
                                                        <ItemTemplate>
                                                            <asp:Button ID="txtUpdate" runat="server" Text="Update" CssClass="btn btn-primary" CommandName="cmdEdit" CommandArgument="<%# ((GridViewRow) Container).RowIndex %>" CausesValidation="false" />
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                </Columns>
                                                <EmptyDataTemplate>
                                                    <strong>No Records Found ! </strong>
                                                </EmptyDataTemplate>
                                            </asp:GridView>
                                        <br />
                                        <%--<b>Measurement Update</b>--%>
                                             <asp:HiddenField ID="AllowedQuantity" runat="server" />
                                         <table class="table table-bordered" id="MeasurementUpdate" runat="server" visible="false">
                                             <tr><td><strong>Unit for Progress</strong></td><td><strong>Last&nbsp; Quantity</strong></td><td><strong>Date</strong></td><td><strong>Max. Quantity</strong></td><td><strong>Current Quantity</strong></td><td><strong>Upload File</strong></td><td></td></tr>
                                              <tr>
                                                  <td>
                                                  <asp:Label ID="lblUnit" runat="server">-</asp:Label>
                                                  </td>
                                                  <td>
                                                  <asp:Label ID="lblLastQuantity" runat="server">-</asp:Label>
                                                  </td><td>
                                                      <asp:Label ID="lblLastUpdate" runat="server">-</asp:Label>
                                                  </td>
                                                   <td>
                                                      <asp:TextBox ID="txtmaxQuantity" runat="server" Enabled="false" TextMode="Number" CssClass="form-control" Width="120px"></asp:TextBox>
                                                   </td>
                                                  <td>
                                                      <asp:TextBox ID="txtQuantity" runat="server" TextMode="Number" CssClass="form-control" Width="80px"></asp:TextBox>
                                                   </td>
                                                  <td>
                                                      <asp:FileUpload ID="FileUpload1" runat="server" />
                                                  </td>
                                                  <td>
                                                  <asp:Button ID="btnupdateBook" runat="server" CssClass="btn btn-primary" Text="Update" OnClick="btnupdateBook_Click"/>
                                                  </td></tr>
                                         </table>
                                             <div id="MeasurementUpdateText" runat="server">
                                                 <table class="table table-bordered">
                                                     <tr>
                                                         <td>
                                                             <strong>No Records Found ! </strong>
                                                         </td>
                                                     </tr>
                                                 </table>
                                                 
                                             </div>
                                             
                                        <br />
                                        <b>MileStones</b>
                                        <asp:GridView ID="grdMileStones" runat="server" CssClass="table table-bordered" AutoGenerateColumns="False" Width="100%" OnRowEditing="grdMileStones_RowEditing" OnRowDataBound="grdMileStones_RowDataBound" OnRowCommand="grdMileStones_RowCommand" >
                                                <Columns>
                                                    <asp:BoundField DataField="MileStoneUID" HeaderText="UID" HeaderStyle-CssClass="Hide" ItemStyle-CssClass="Hide">
                                                    <HeaderStyle HorizontalAlign="Left" />

                                <ItemStyle CssClass="Hide"></ItemStyle>
                                                    </asp:BoundField>
                                                    <asp:BoundField DataField="Description" HeaderText="MileStone" >
                                                    <HeaderStyle HorizontalAlign="Left" />
                                                    </asp:BoundField>
                                                    <asp:BoundField DataField="MileStoneDate" HeaderText="Planned Date"  DataFormatString="{0:dd/MM/yyyy}">
                                                    <HeaderStyle HorizontalAlign="Left" />
                                                    </asp:BoundField>
                                                    <asp:TemplateField HeaderText="Projected Date">
                                                        <ItemTemplate>
                                                            
                                                            <asp:TextBox ID="dtprojectDate" CssClass="form-control" runat="server" ClientIDMode="Static"></asp:TextBox>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Status">
                                                        <ItemTemplate>
                                                            <asp:DropDownList ID="DDLStatus" runat="server" CssClass="form-control">
                                                                <asp:ListItem Value="Not Completed">Not Completed</asp:ListItem>
                                                                <asp:ListItem Value="Completed">Completed</asp:ListItem>
                                                            </asp:DropDownList>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                       <asp:TemplateField>
                                                    <ItemTemplate>
                                                         <asp:Button ID="btnupdate" runat="server" Text="Update" CssClass="btn btn-primary" CommandName="edit" CommandArgument="<%# ((GridViewRow) Container).RowIndex %>" CausesValidation="false" />
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                      <asp:BoundField DataField="ProjectedDate" HeaderText="Projected Date"  DataFormatString="{0:dd/MM/yyyy}" HeaderStyle-CssClass="Hide" ItemStyle-CssClass="Hide">
                                                    <HeaderStyle HorizontalAlign="Left" />
                                                    </asp:BoundField>
                                                    <asp:BoundField DataField="Status" HeaderText="Status" HeaderStyle-CssClass="Hide" ItemStyle-CssClass="Hide">
                                                    <HeaderStyle HorizontalAlign="Left" />
                                                    </asp:BoundField>

                                                </Columns>
                                                <EmptyDataTemplate>
                                                    <strong>No Records Found ! </strong>
                                                </EmptyDataTemplate>
                                            </asp:GridView>
                                        </div>

                                            <div id="DailyUpdate" runat="server" visible="false">
                                                
                                                <asp:HiddenField ID="HiddenGroupBOQItems" runat="server" />
                                                <div class="text-right">
                                                 <a id="TaskScheduleHistory1" runat="server"  href="/_modal_pages/task-schedulehistory.aspx"  class="showTaskMeasurementModal"><asp:Button ID="btnmeasurement" runat="server" Text="View History" CausesValidation="false"  CssClass="btn btn-primary" align="end"></asp:Button></a>  <br /><br />
                                             </div>
                                                <asp:GridView ID="GrdMeasurmentUpdate" runat="server" DataKeyNames="TaskUID" CssClass="table table-bordered" AutoGenerateColumns="False" AllowPaging="false" OnRowDataBound="GrdMeasurmentUpdate_RowDataBound">
                                                    <Columns>
                                                        <asp:BoundField DataField="BOQDetailsUID" HeaderText="BOQUID" ItemStyle-CssClass="Hide" HeaderStyle-CssClass="Hide" />
                                                        <asp:TemplateField HeaderText="Activity Name">
                                                            <ItemTemplate>
                                                                <%#GetTaskName(Eval("TaskUID").ToString())%>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:BoundField DataField="UnitforProgress" HeaderText="Unit"/>
                                                        <%--<asp:TemplateField HeaderText="Unit">
                                                            <ItemTemplate>
                                                                <asp:Label ID="LblUnitforProgress" runat="server" Text='<%#Eval("UnitforProgress")%>'></asp:Label>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>--%>
                                                        <asp:TemplateField HeaderText="BOQ Quantity">
                                                            <ItemTemplate>
                                                                <asp:Label ID="LblBOQQuantity" runat="server" Text='<%#Eval("UnitQuantity")%>'></asp:Label>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="Cumulative Quantity">
                                                            <ItemTemplate>
                                                                <asp:Label ID="LblCumulativeQuantity" runat="server"></asp:Label>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="Last Update Date">
                                                            <ItemTemplate>
                                                                <asp:Label ID="LblLastUpdatedDate" runat="server"></asp:Label>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="Quantity">
                                                            <ItemTemplate>
                                                                <asp:TextBox ID="txttodayQuantity" runat="server" CssClass="form-control"></asp:TextBox>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="Achieved Date">
                                                            <ItemTemplate>
                                                                <div class="form-control">
                                                                <asp:TextBox ID="dtDate" BorderStyle="None" BorderColor="White" Width="105" class="datepick" runat="server" placeholder="dd/mm/yyyy" autocomplete="off"></asp:TextBox>
                                                                    </div>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="Remarks">
                                                            <ItemTemplate>
                                                                <asp:TextBox ID="txtremarks" runat="server" TextMode="MultiLine" CssClass="form-control"></asp:TextBox>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                    </Columns>
                                                </asp:GridView>

                                                <asp:Button ID="BtnMeasurementUpdate" runat="server" CssClass="btn btn-primary" Text="Update Measurement" OnClick="BtnMeasurementUpdate_Click"/>
                                            </div>

                                            
                                            </div>
                                        </div>
                                    </div>
                          </div>
                     </div>
                        </ContentTemplate>  
                        <Triggers>
                         <asp:AsyncPostBackTrigger ControlID="TreeView1" EventName="SelectedNodeChanged" />
                            <asp:PostBackTrigger ControlID="btnupdateBook" />
                            <asp:PostBackTrigger ControlID="btnScheduleUpdate" />
                            <asp:PostBackTrigger ControlID="BtnMeasurementUpdate" />
                            <%--<asp:PostBackTrigger ControlID="BtnResourceDeploymentSubmit" />
                            <asp:PostBackTrigger ControlID="GrdResourceDeployment" />--%>
                            <%--<asp:AsyncPostBackTrigger ControlID="GrdResourceDeployment" EventName="RowCommand" />--%>
                            <%--<asp:PostBackTrigger ControlID="DDLYear" />
                            <asp:PostBackTrigger ControlID="DDLMonth"/>--%>
                            <asp:AsyncPostBackTrigger ControlID="DDLYear" EventName="SelectedIndexChanged" />
                            <asp:AsyncPostBackTrigger ControlID="DDLMonth" EventName="SelectedIndexChanged" />
                            <%--<asp:PostBackTrigger ControlID="GrdTaskSchedule" />--%>
                            <%--<asp:AsyncPostBackTrigger ControlID="GrdTaskSchedule" EventName="RowCommand" />--%>
                         </Triggers>
                 </asp:UpdatePanel>
      </div>
    <div class="container-fluid" id="DivResourceDeployment" runat="server" visible="false">
        <div class="row">
          <div class="col-lg-6 col-xl-12 form-group">
                        <div class="card">
                            <div class="card-body" style="padding-bottom:0; margin-bottom:0;">
                                <div class="table-responsive">
                                    <div>
                                                <h6 class="text-muted">
                                                    <asp:Label ID="LblResourceDeploy" CssClass="text-uppercase font-weight-bold" Text="Resource Deployment" runat="server" />
                                                </h6>
                                                <table class="table table-borderless">
                                                    <tr>
                                                        <td>
                                                            <asp:DropDownList ID="DDLResourceYear" runat="server" CssClass="form-control">
                                                            </asp:DropDownList>
                                                        </td>
                                                        <td><asp:DropDownList ID="DDLResourceMonth" runat="server" CssClass="form-control">
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
                                                        <td>
                                                             <asp:Button ID="BtnResourceDeploymentSubmit" runat="server" CssClass="btn btn-primary" Text="Submit" OnClick="BtnResourceDeploymentSubmit_Click" />
                                                         </td>
                                                    </tr>
                                                </table>

                                                <asp:GridView ID="GrdResourceDeployment" runat="server" AllowPaging="false" CssClass="table table-bordered" AutoGenerateColumns="False" OnRowCommand="GrdResourceDeployment_RowCommand" OnRowEditing="GrdResourceDeployment_RowEditing" Width="100%">
                                                <Columns>
                                                    <asp:TemplateField HeaderText="Resource Name" ItemStyle-Width="20%">
                                                        <ItemTemplate>
                                                            <a href="/_modal_pages/view-resourcedeployment.aspx?ReourceDeploymentUID=<%#Eval("ReourceDeploymentUID")%>"  class="showResourceDeploymentModal"><%#Eval("ResourceName")%></a>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="UOM">
                                                        <ItemTemplate>
                                                            <%#Eval("Unit_for_Measurement")%>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Planned" ItemStyle-Width="10%">
                                                        <ItemTemplate>
                                                            <asp:Label ID="LblPlanned" runat="server" CssClass="form-control" Text='<%#Eval("Planned")%>'></asp:Label>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Deployed (Cumulative)" ItemStyle-Width="10%">
                                                        <ItemTemplate>
                                                            <asp:Label ID="LblDeployed" runat="server" CssClass="form-control" Text='<%#Eval("Deployed")%>'></asp:Label>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Deployed" ItemStyle-Width="10%">
                                                        <ItemTemplate>
                                                            <asp:TextBox ID="txtdeployedResource" runat="server" CssClass="form-control" TextMode="Number"></asp:TextBox>
                                                            <asp:Label ID="LbldeployedMsg" runat="server" ForeColor="Red"></asp:Label>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Deployed Date">
                                                        <ItemTemplate>
                                                            <div class="form-control">
                                                                <asp:TextBox ID="txtdeployeddate" BorderStyle="None" BorderColor="White" Width="105" class="datepick" runat="server" placeholder="dd/mm/yyyy" autocomplete="off"></asp:TextBox>
                                                            </div> 
                                                            <asp:Label ID="LblDateNotEntered" runat="server" ForeColor="Red"></asp:Label>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Remarks">
                                                        <ItemTemplate>
                                                            <asp:TextBox ID="txtremarks" runat="server" CssClass="form-control" autocomplete="off" TextMode="MultiLine"></asp:TextBox>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField>
                                                        <ItemTemplate>
                                                            <asp:Button ID="btnUpdateDeployed" runat="server" CssClass="btn btn-primary" CommandName="edit" Text="Update" CausesValidation="false" CommandArgument='<%#Eval("ReourceDeploymentUID")%>' />
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                </Columns>
                                                <EmptyDataTemplate>
                                                    <strong>No Records Found ! </strong>
                                                </EmptyDataTemplate>
                                            </asp:GridView>
                                            </div>
                                    </div>
                            </div>
                        </div>
                       
                     </div>  
            </div>
    </div>
    <%--Task Schedule Update History--%>
    <div id="ModTaskScheduleHistory" class="modal it-modal fade">
	    <div class="modal-dialog modal-dialog-centered modal-dialog-scrollable modal-xl">
		    <div class="modal-content">
			    <div class="modal-header">
				    <h5 class="modal-title">Task Schedule Update History</h5>
                    <button aria-label="Close" class="close" onclick="javascript:window.location.reload()" data-dismiss="modal" type="button"><span aria-hidden="true">&times;</span></button>
			    </div>
			    <div class="modal-body">
                    <iframe class="border-0 w-100" style="height:480px;" loading="lazy"></iframe>
			    </div>
              
		    </div>
	    </div>
    </div>

    <%--Task Measurement Update History--%>
    <div id="ModTaskMeasurementHistory" class="modal it-modal fade">
	    <div class="modal-dialog modal-dialog-centered modal-dialog-scrollable modal-xl">
		    <div class="modal-content">
			    <div class="modal-header">
				    <h5 class="modal-title">View History</h5>
                    <button aria-label="Close" class="close" data-dismiss="modal" type="button"><span aria-hidden="true">&times;</span></button>
			    </div>
			    <div class="modal-body">
                    <iframe class="border-0 w-100" style="height:480px;" loading="lazy"></iframe>
			    </div>
              
		    </div>
	    </div>
    </div>

    <%--Resource Deployment Update History--%>
    <div id="ModResourceDeploymentHistory" class="modal it-modal fade">
	    <div class="modal-dialog modal-dialog-centered modal-dialog-scrollable modal-xl">
		    <div class="modal-content">
			    <div class="modal-header">
				    <h5 class="modal-title">Resource Deployment History</h5>
                    <button aria-label="Close" class="close" data-dismiss="modal" type="button"><span aria-hidden="true">&times;</span></button>
			    </div>
			    <div class="modal-body">
                    <iframe class="border-0 w-100" style="height:480px;" loading="lazy"></iframe>
			    </div>
              
		    </div>
	    </div>
    </div>
</asp:Content>
