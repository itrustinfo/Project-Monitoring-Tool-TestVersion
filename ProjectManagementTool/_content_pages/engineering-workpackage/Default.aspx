<%@ Page Title="" Language="C#" MasterPageFile="~/_master_pages/default.Master" AutoEventWireup="true" MaintainScrollPositionOnPostBack="true" CodeBehind="Default.aspx.cs" Inherits="ProjectManager._content_pages.work_packages.Default" %>
<asp:Content ID="Content1" ContentPlaceHolderID="default_master_head" runat="server">
     <style type="text/css">

  .black_overlay {
  display: none;
  position: absolute;
  top: 0%;
  left: 0%;
  width: 100%;
  height: 100%;
  background-color: black;
  z-index: 1001;
  -moz-opacity: 0.8;
  opacity: .80;
  filter: alpha(opacity=80);
}
.white_content {
  display: none;
  position: absolute;
  top:auto;
  left: 25%;
  width: 35%;
  padding: 10px;
  border: 8px solid #3498db;
  background-color: white;
  z-index: 1002;
  overflow: auto;
  
  text-align:justify;
  line-height:20px;
  box-shadow: 5px 10px #888888;
  font-weight:normal;
  font-size:large;
}
     .hideItem {
         display:none;
     }
    
      
    </style>
    <script type="text/javascript">
        function DeleteItem() {
            if (confirm("Are you sure you want to delete ...?")) {
                return true;
            }
            return false;
    }
    </script>
    <script type="text/javascript">
        function BindEvents() {
                 $(".showModal").click(function(e) {
        e.preventDefault();
        var url = $(this).attr("href");
        $("#ModAddWorkpackages iframe").attr("src", url);
        $("#ModAddWorkpackages").modal("show");
              });

        $(".showModalTask").click(function(e) {
        e.preventDefault();
        var url = $(this).attr("href");
        $("#ModAddTask iframe").attr("src", url);
        $("#ModAddTask").modal("show");
              });

        $(".showModalSubTask").click(function(e) {
        e.preventDefault();
        var url = $(this).attr("href");
        $("#ModAddSubTask iframe").attr("src", url);
        $("#ModAddSubTask").modal("show");
          });

         $(".showModalEditTask").click(function(e) {
        e.preventDefault();
        var url = $(this).attr("href");
        $("#ModEditTask iframe").attr("src", url);
        $("#ModEditTask").modal("show");
            });

       $(".showModalMilestone").click(function(e) {
        e.preventDefault();
        var url = $(this).attr("href");
        $("#ModAddMilestone iframe").attr("src", url);
        $("#ModAddMilestone").modal("show");
          });

        $(".showModalEditMilestone").click(function(e) {
        e.preventDefault();
        var url = $(this).attr("data-href");
        $("#ModEditMilestone iframe").attr("src", url);
        $("#ModEditMilestone").modal("show");
          });

        $(".showModalResource").click(function(e) {
        e.preventDefault();
        var url = $(this).attr("href");
        $("#ModAddResourceAllocation iframe").attr("src", url);
        $("#ModAddResourceAllocation").modal("show");
            });

            $(".showModalSortActivity").click(function(e) {
        e.preventDefault();
        var url = $(this).attr("href");
        $("#ModSortActivity iframe").attr("src", url);
        $("#ModSortActivity").modal("show");
            });

            $(".showModalCopyActivity").click(function(e) {
        e.preventDefault();
        var url = $(this).attr("href");
        $("#ModCopyActivity iframe").attr("src", url);
        $("#ModCopyActivity").modal("show");
            });

            $(".showModalCopyProjectData").click(function(e) {
        e.preventDefault();
        var url = $(this).attr("href");
        $("#ModCopyProjectData iframe").attr("src", url);
        $("#ModCopyProjectData").modal("show");
            });

            $(".showModalDependency").click(function(e) {
        e.preventDefault();
        var url = $(this).attr("href");
        $("#ModAddDependency iframe").attr("src", url);
        $("#ModAddDependency").modal("show");
          });

        $(".showModalEditDependency").click(function(e) {
        e.preventDefault();
        var url = $(this).attr("data-href");
        $("#ModEditDependency iframe").attr("src", url);
        $("#ModEditDependency").modal("show");
            });

            $(".showModalTaskSchedule").click(function(e) {
        e.preventDefault();
        var url = $(this).attr("href");
        $("#ModAddTaskSchedule iframe").attr("src", url);
        $("#ModAddTaskSchedule").modal("show");
            });

            $(".showModalUploadPhotograph").click(function(e) {
        e.preventDefault();
        var url = $(this).attr("href");
        $("#ModAddUploadSiteImages iframe").attr("src", url);
        $("#ModAddUploadSiteImages").modal("show");
            });

            //
            $(".showModalResourceDeployement").click(function(e) {
        e.preventDefault();
        var url = $(this).attr("href");
        $("#ModAddResourceDeployement iframe").attr("src", url);
        $("#ModAddResourceDeployement").modal("show");
            });

            //
            $(".showModalViewSitePhotograph").click(function(e) {
            e.preventDefault();
            var url = $(this).attr("href");
            $("#ModViewSitePhotograph iframe").attr("src", url);
            $("#ModViewSitePhotograph").modal("show");
            });
        }
        $(document).ready(function () {
            BindEvents();
            //$('#loader').fadeOut();
        });
</script>
   
      <%--<script src="http://code.jquery.com/jquery-1.8.2.js"></script> 

<script type="text/javascript">  
   $(window).load(function() {  
      $("#loader").fadeOut(200);  
   });
</script> --%>
   <%--<script type="text/javascript">
function showProgress() {
var updateProgress = $get("<%= UpdateProgress1.ClientID %>");
updateProgress.style.display = "block";
}
</script>--%>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="default_master_body" runat="server">
    <%--project selection dropdowns--%>
    <asp:ScriptManager ID="ScriptManager1" runat="server">
   </asp:ScriptManager>
   
    <%--<div id="loader"></div>--%>



    <div class="container-fluid">
        <div class="row">
            <div class="col-lg-6 col-xl-4 form-group">Engineering Activities</div>
             <div class="col-md-6 col-lg-3 form-group">
                   
                </div>
                <div class="col-md-6 col-lg-5 form-group">
                    <label class="sr-only" for="DDLProject">Project</label>
                    <div class="input-group">
                        <div class="input-group-prepend">
                            <span class="input-group-text">Project</span>
                        </div>
                        <asp:DropDownList ID="DDlProject" runat="server" CssClass="form-control" AutoPostBack="true" OnSelectedIndexChanged="DDlProject_SelectedIndexChanged"></asp:DropDownList>
                       <%-- <select class="form-control" id="DDlProject" runat="server">
                           
                        </select>--%>
                    </div>
                </div>
               
        </div>
    </div>
        <%--work package contents--%>
    <div class="container-fluid">
        <asp:UpdatePanel ID="UpdatePanel2" runat="server" UpdateMode="Conditional" ChildrenAsTriggers="false">
                    <ContentTemplate>
                        
                    <div class="row">
                        <div class="col-lg-6 col-xl-4 form-group">
                            <div class="card h-100" style="max-height:700px; overflow-y:auto;">
                                <div class="card-body">
                                    <h6 class="card-title text-muted text-uppercase font-weight-bold">WorkPackage Directory</h6>
                                    
                                    <div class="form-group">
                                        <label class="sr-only" for="TxtSearchDocuments">Search</label>
                                        <div class="input-group">
                                            <input id="TxtSearchDocuments" runat="server" autocomplete="off" class="form-control" type="text" placeholder="Activity name" />
                                            <div class="input-group-append">
                                                <asp:Button ID="BtnSearchDocuments" CssClass="btn btn-primary" Text="Search" runat="server" OnClientClick="showProgress()" OnClick="BtnSearchDocuments_Click" />
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
                            <asp:UpdateProgress ID="UpdateProgress1" runat="Server" AssociatedUpdatePanelID="UpdatePanel2" DisplayAfter="1"> 
                                <ProgressTemplate > 
                               
                                    <div id="loader"></div>
                                </ProgressTemplate> 
                            </asp:UpdateProgress>
                                <%--<script type="text/javascript">
                                    function BindEvents() {
                                         $(".showModal").click(function(e) {
                                e.preventDefault();
                                var url = $(this).attr("href");
                                $("#ModAddWorkpackages iframe").attr("src", url);
                                $("#ModAddWorkpackages").modal("show");
                                      });

                                $(".showModalTask").click(function(e) {
                                e.preventDefault();
                                var url = $(this).attr("href");
                                $("#ModAddTask iframe").attr("src", url);
                                $("#ModAddTask").modal("show");
                                      });

                                $(".showModalSubTask").click(function(e) {
                                e.preventDefault();
                                var url = $(this).attr("href");
                                $("#ModAddSubTask iframe").attr("src", url);
                                $("#ModAddSubTask").modal("show");
                                  });

                                 $(".showModalEditTask").click(function(e) {
                                e.preventDefault();
                                var url = $(this).attr("href");
                                $("#ModEditTask iframe").attr("src", url);
                                $("#ModEditTask").modal("show");
                                    });

                               $(".showModalMilestone").click(function(e) {
                                e.preventDefault();
                                var url = $(this).attr("href");
                                $("#ModAddMilestone iframe").attr("src", url);
                                $("#ModAddMilestone").modal("show");
                                  });

                                $(".showModalEditMilestone").click(function(e) {
                                e.preventDefault();
                                var url = $(this).attr("data-href");
                                $("#ModEditMilestone iframe").attr("src", url);
                                $("#ModEditMilestone").modal("show");
                                  });

                                $(".showModalResource").click(function(e) {
                                e.preventDefault();
                                var url = $(this).attr("href");
                                $("#ModAddResourceAllocation iframe").attr("src", url);
                                $("#ModAddResourceAllocation").modal("show");
                                    });

                                    $(".showModalSortActivity").click(function(e) {
                                e.preventDefault();
                                var url = $(this).attr("href");
                                $("#ModSortActivity iframe").attr("src", url);
                                $("#ModSortActivity").modal("show");
                                    });

                                    $(".showModalCopyActivity").click(function(e) {
                                e.preventDefault();
                                var url = $(this).attr("href");
                                $("#ModCopyActivity iframe").attr("src", url);
                                $("#ModCopyActivity").modal("show");
                                    });

                                    $(".showModalCopyProjectData").click(function(e) {
                                e.preventDefault();
                                var url = $(this).attr("href");
                                $("#ModCopyProjectData iframe").attr("src", url);
                                $("#ModCopyProjectData").modal("show");
                                    });

                                    $(".showModalDependency").click(function(e) {
                                e.preventDefault();
                                var url = $(this).attr("href");
                                $("#ModAddDependency iframe").attr("src", url);
                                $("#ModAddDependency").modal("show");
                                  });

                                $(".showModalEditDependency").click(function(e) {
                                e.preventDefault();
                                var url = $(this).attr("data-href");
                                $("#ModEditDependency iframe").attr("src", url);
                                $("#ModEditDependency").modal("show");
                                    });

                                }
                                </script>--%>
                                <div class="card mb-4" id="ProjectDetails" runat="server">
                                    <div class="card-body">
                                        <div class="card-title">
                                            <div class="d-flex justify-content-between">
                                                <h6 class="text-muted">
                                                    <asp:Label ID="LblProjectDetails" CssClass="text-uppercase font-weight-bold" runat="server" Text="Project Details" />
                                                    
                                                    </h6>
                                                <div>
                                                    <a id="CopyProjectData" runat="server" href="/_modal_pages/copy-projectdata.aspx" class="showModalCopyProjectData"><asp:Button ID="btncopyproject" runat="server" Text="Copy Project Data" CssClass="btn btn-primary"></asp:Button></a>
                                                </div>
                                                </div>
                                            </div>
                                        <%--<h6 class="card-title text-muted text-uppercase font-weight-bold">Project Details
                                            </h6>--%>
                                        <div class="table-responsive">
                                             <table class="table table-borderless">
                                           <tr><td>Project Name</td><td>:</td><td colspan="6">
                                               <asp:Label ID="lblPrjName" runat="server" Font-Bold="true"></asp:Label>
                                               </td>
                                               </tr>
                                               <tr>
                                                   <td>Funding Agency</td>
                                                   <td>:</td><td colspan="6">
                                                   <asp:Label ID="LblFundingAgency" runat="server"></asp:Label></td>
                                               </tr>
                                                 <tr><td>Start Date</td><td>:</td><td>
                                                   <asp:Label ID="lblStartDate" runat="server"></asp:Label>
                                                     </td><td>Planned End Date</td><td>:</td><td>
                                                     <asp:Label ID="lblPlannedEndDate" runat="server"></asp:Label>
                                                     </td>
                                                 </tr>
                                                 <tr>
                                                      <td>Projected End Date</td><td>:</td><td>
                                                     <asp:Label ID="lblPrjctedEndDate" runat="server"></asp:Label>
                                                     </td>
                                                     <td>Status</td><td>:</td><td>
                                                     <asp:Label ID="lblPrjStatus" runat="server"></asp:Label>
                                                     </td>
                                                 </tr>
                                                 <tr>
                                                     <td>
                                                         Cumulative Progress(%)
                                                     </td>
                                                     <td>:</td>
                                                     <td>
                                                         <asp:Label ID="LblCumulativeProgressProject" runat="server"></asp:Label>
                                                     </td>
                                                 </tr>
                                               </table>
                                            </div>
                                        </div>
                                    </div>
                                <div class="card mb-4" id="Contractors" runat="server">
                                    <div class="card-body">

                                        <h6 class="card-title text-muted text-uppercase font-weight-bold">Contractors Details
                                            </h6>
                                        <div class="table-responsive">
                                             <table class="table table-borderless">
                                               <tr>
                                                   <td>Contractor</td><td>:</td><td colspan="3">
                                                   <asp:Label ID="LblContractor" runat="server" Font-Bold="true"></asp:Label>
                                                   </td>
                                               </tr>
                                               <tr>
                                                    <%-- <td>
                                                         Representatives
                                                     </td><td>:</td>
                                                     <td colspan="3">
                                                         <asp:Label ID="LblContract_Representatives" runat="server"></asp:Label>
                                                     </td>--%>
                                                   </tr>
                                                    <tr>
                                                             <td>Type of Contract </td><td>:</td><td>
                                                   <asp:Label ID="LblContract_Type" Font-Bold="true" runat="server"></asp:Label>
                                                   </td>
                                                     <td>Contract Duration</td><td>:</td><td>
                                                     <asp:Label ID="LblContract_Duration" runat="server"></asp:Label>
                                                     </td></tr>
                                                 <tr><td>Start Date</td><td>:</td><td>
                                                     <asp:Label ID="LblContract_StartDate" runat="server"></asp:Label>
                                                     </td><td>Completion Date</td><td>:</td><td>
                                                     <asp:Label ID="LblContract_CompletionDate" runat="server"></asp:Label>
                                                     </td></tr>
                                                 <tr>
                                                     <td>
                                                         <asp:Label ID="LBLProjectNumber" runat="server"></asp:Label>
                                                     </td>
                                                     <td>:</td>
                                                     <td>
                                                         <asp:Label ID="LblNJSEIProjectNumber" runat="server"></asp:Label>
                                                     </td>
                                                     <td>
                                                         Project Specific Package Number
                                                     </td>
                                                     <td>:</td>
                                                     <td>
                                                         <asp:Label ID="LblProjectSpecificPackageNumber" runat="server"></asp:Label>
                                                     </td>
                                                 </tr>
                                               </table>
                                            </div>
                                        </div>
                                    </div>
                                <div class="card mb-4" id="WorkPackageDetils" runat="server">
                                    <div class="card-body">
                                        <div class="card-title">
                                            <div class="d-flex justify-content-between">
                                                <h6 class="text-muted">
                                                    <asp:Label ID="Label1" CssClass="text-uppercase font-weight-bold" runat="server" Text="WorkPackage Details" />
                                                    </h6>
                                                <div>
                                                    <a id="CopyActivityData" runat="server" href="/_modal_pages/copy-activitydata.aspx" class="showModalCopyActivity"><asp:Button ID="btncopy" runat="server" Text="Copy Activity Data" CssClass="btn btn-primary"></asp:Button></a>
                                                    <a id="UploadSitePhotograph" runat="server" href="/_modal_pages/upload-sitephotograph.aspx" class="showModalUploadPhotograph"><asp:Button ID="btnuploadphoto" runat="server" Text="Add Site Photographs" CssClass="btn btn-primary"></asp:Button></a>
                                                    <a id="ViewSitePhotograph" runat="server"  href="/_modal_pages/view-sitephotographs.aspx" class="showModalViewSitePhotograph"><asp:Button ID="btnViewSitePhotograph" runat="server" Text="View Site Photographs" CssClass="btn btn-primary"></asp:Button></a>
                                                </div>
                                                </div>
                                            </div>

                                        <%--<h6 class="card-title text-muted text-uppercase font-weight-bold">WorkPackage Details
                                            </h6>--%>
                                        <div class="table-responsive">
                                            <table class="table table-borderless">
                               <tr><td>Name</td><td>:</td><td colspan="2">
                                   <asp:Label ID="LblWorkPackageName" runat="server" Font-Bold="true"></asp:Label>
                                   </td></tr>
                                 <tr>
                                     <td>Location </td><td>:</td><td>
                                   <asp:Label ID="LblWorkPackageLocation" runat="server"></asp:Label>
                                   </td>
                                      <td>End User</td><td>:</td><td>
                                   <asp:Label ID="LblWorkPackageClient" runat="server"></asp:Label>
                                     </td>
                                     </tr>
                               <tr>
                                   <%--<td>Budget</td><td>:</td><td>
                                       <asp:Label ID="LblWorkpackageCurrency" ForeColor="#006699" runat="server"></asp:Label>
                                     <asp:Label ID="LblWorkPackageBudget" runat="server"></asp:Label>
                                     </td>--%>
                                   <td>Status</td><td>:</td><td>
                                     <asp:Label ID="LblWorkPackageStatus" runat="server"></asp:Label>
                                     </td>
                                   <td>
                                                         Cumulative Progress(%)
                                                     </td>
                                   <td>:</td>
                                                     <td>
                                                         <asp:Label ID="LblCumulativeProgressWorkpackage" runat="server"></asp:Label>
                                                     </td>
                               </tr>
                                                      <tr>
                                     <td>Start Date </td><td>:</td><td>
                                   <asp:Label ID="lblWkpgStartDate" runat="server"></asp:Label>
                                   </td>
                                      <td>End Date</td><td>:</td><td>
                                   <asp:Label ID="lblWkpgEndDate" runat="server"></asp:Label>
                                     </td>
                                     </tr>
             
                           </table>
                                            </div>
                                        </div>
                                    </div>
                                <div class="card mb-4" id="TaskDetails" runat="server">
                                    <div class="card-body">
                                         <div class="card-title">
                                            <div class="d-flex justify-content-between">
                                        <h6 class="text-muted text-uppercase font-weight-bold">Task Details
                            
                                            </h6>
                                                 <div>
                                                    <%--<a id="TaskSchedule" runat="server" href="/_modal_pages/addtask-schedule.aspx?type=Add" class="showModalTaskSchedule"><asp:Button ID="btnaddtaskschedule" runat="server" Text="+ Task Schedule" CssClass="btn btn-primary"></asp:Button></a>--%>
                                                     <a id="TaskSchedule" runat="server" href="/_modal_pages/addtask-targetschedule.aspx?type=Add" class="showModalTaskSchedule"><asp:Button ID="btnaddtaskschedule" runat="server" Text="+ Task Schedule" CssClass="btn btn-primary"></asp:Button></a>
                                                     </div>
                                                </div>
                                             </div>
                                        <div class="table-responsive">
                                            <table class="table table-borderless">
                                               
                                                
                                           </table>
                                            <table class="table table-borderless">
                                                <tr><td>Task Name</td><td>:</td><td colspan="6">
                                                   <asp:Label ID="LblTaskName" runat="server" Font-Bold="true"></asp:Label>
                                                   </td></tr>
                                                <tr>
                                                    <td>Task Description </td><td>:</td><td colspan="6">
                                                   <asp:Label ID="LblTaskDescription" runat="server"></asp:Label>
                                                   </td>
                                                </tr>
                                                 <tr><td>Start Date</td><td>:</td><td>
                                                   <asp:Label ID="LblTaskStartDate" runat="server"></asp:Label>
                                                     </td><td>Planned EndDate</td><td>:</td><td>
                                                     <asp:Label ID="LblTaskPlannedEndDate" runat="server"></asp:Label>
                                                     </td></tr>
                                                <tr id="MeasurementDetails" runat="server">
                                                    <td>
                                                        Measurement Unit
                                                    </td><td>:</td>
                                                    <td>
                                                        <asp:Label ID="LblMeasurementUnit" runat="server"></asp:Label>
                                                    </td>
                                                    <td>
                                                        Total Quantity
                                                    </td><td>:</td>
                                                    <td>
                                                        <asp:Label ID="LblMeasurementTotalQuantity" runat="server"></asp:Label>
                                                    </td>
                                                    <td>
                                                        Cumulative Quantity
                                                    </td><td>:</td>
                                                    <td>
                                                        <asp:Label ID="LblMeasurementCumulativeQuantity" runat="server"></asp:Label>
                                                    </td>
                                                </tr>
                                                 <tr><%--<td>Total Budget</td><td>:</td><td>
                                                     <asp:Label ID="LblTaskCurrency" ForeColor="#006699" runat="server"></asp:Label>
                                                     <asp:Label ID="LblTaskBudget" runat="server"></asp:Label>
                                                     </td>--%>
                                                     <td>Status</td><td>:</td><td>
                                                     <asp:Label ID="LblTaskStatus" runat="server"></asp:Label>
                                                     </td>
                                                     <td>Cumulative Percentage</td><td>:</td>
                                                     <td>
                                                         <asp:Label ID="LblCumulativePercentage" runat="server"></asp:Label>
                                                     </td>
                                                     
                                                 </tr>
                                                </table>
                                            </div>
                                        </div>
                                    </div>
                            
                                <div class="card mb-4">
                                    <div class="card-body">
                                        <div class="card-title">
                                            <div class="d-flex justify-content-between">
                                                <h6 class="text-muted">
                                                    <asp:Label ID="ActivityHeading" CssClass="text-uppercase font-weight-bold" runat="server" />
                                                    <%--<asp:Label Text="Foo bar" CssClass="pl-1" runat="server" />--%>
                                                </h6>
                                                <div>
                                                    <a id="AddTask" runat="server" href="/_modal_pages/add-task.aspx?type=Add" class="showModalTask"><asp:Button ID="btnAddTask" runat="server" Text="+ Add Task" CssClass="btn btn-primary"></asp:Button></a>
                                            <a id="AddWorkPackage" runat="server" href="/_modal_pages/add-workpackage.aspx" class="showModal"><asp:Button ID="Btnaddworkpackage" runat="server" Text="+ Add Workpackage" CssClass="btn btn-primary"></asp:Button></a>
                                            <a id="AddSubTask" runat="server" href="#" class="showModalSubTask"><asp:Button ID="btnAddSubTask" runat="server" Text="+ Add SubTask" CssClass="btn btn-primary"></asp:Button></a>
                                                    <a id="SortActivity" runat="server" href="/_modal_pages/change-activityorder.aspx" class="showModalSortActivity"><asp:Button ID="btnorder" runat="server" Text="Change Order" CssClass="btn btn-primary"></asp:Button></a>
                                                </div>
                                            </div>
                                        </div>
                                        <div class="table-responsive">
                                            <asp:GridView ID="GrdTreeView" runat="server" AutoGenerateColumns="false" PageSize="20" 
                                        AllowPaging="true" CssClass="table table-bordered" EmptyDataText="No Data"
                                        DataKeyNames="TaskUID" Width="100%" OnPageIndexChanging="GrdTreeView_PageIndexChanging" OnRowCommand="GrdTreeView_RowCommand" OnRowDataBound="GrdTreeView_RowDataBound" OnRowDeleting="GrdTreeView_RowDeleting">
                                        <Columns>
                                            <asp:TemplateField HeaderText="Name">
                                                <ItemTemplate>
                                                    <asp:LinkButton ID="lnkview" runat="server" CommandName="view" CommandArgument='<%#Eval("TaskUID")%>'><%#Eval("Name")%></asp:LinkButton>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                            <%--<asp:BoundField  DataField="Name" HeaderText="Name" />--%>
                                            <asp:TemplateField HeaderText="Description" ItemStyle-Width="25%">
                                               <ItemTemplate>
                                                   <%#LimitCharts(Eval("Description").ToString())%> &nbsp; <a href="javascript:void(0)" onclick="document.getElementById('<%#Eval("TaskUID")%>').style.display='block';"><%#ShoworHide(Eval("Description").ToString())%></a>
                                                    <div id='<%#Eval("TaskUID")%>' class="white_content"><span><%#Eval("Description").ToString()%></span> <br /><a style="float:right;" href="javascript:void(0)" onclick="document.getElementById('<%#Eval("TaskUID")%>').style.display='none';">Close</a>
                                                     </div>
                                               </ItemTemplate>
                                           </asp:TemplateField>
                                            <asp:BoundField  DataField="StartDate" HeaderText="Activity Start Date" DataFormatString="{0:dd MMM yyyy}"/>
                                              <asp:BoundField DataField="PlannedEndDate" HeaderText="Activity End Date" DataFormatString="{0:dd MMM yyyy}"/>
                                             <asp:TemplateField>
                                                <ItemTemplate>
                                                     <a class="showModalEditTask" id="edit" href="/_modal_pages/add-subtask.aspx?type=edit&TaskUID=<%#Eval("TaskUID")%>&PrjUID=<%#Eval("ProjectUID")%>&WrkUID=<%#Eval("WorkPackageUID")%>&OptionUID=<%#Eval("Workpackage_Option")%>"><span title="Edit" class="fas fa-edit"></span></a>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                           <%--<asp:TemplateField>
                                                <ItemTemplate>
                                                     <a class="add" id="add" href="AddSubTask.aspx?type=add&ParentTaskUID=<%#Eval("TaskUID")%>&PrjUID=<%#Eval("ProjectUID")%>&WrkUID=<%#Eval("WorkPackageUID")%>">Add SubTask</a>
                                                </ItemTemplate>
                                            </asp:TemplateField>--%>
                                            <%--<asp:TemplateField>
                                                <ItemTemplate>
                                                     <a class="AddMileStones" id="AddMileStones" href="AddMileStone.aspx?TaskUID=<%#Eval("TaskUID")%>">Add MileStones</a>
                                                </ItemTemplate>
                                            </asp:TemplateField>--%>
                                            <asp:TemplateField>
                                                <ItemTemplate>
                                                       <asp:LinkButton ID="lnkdelete" runat="server" CausesValidation="false" OnClientClick="return DeleteItem()" CommandArgument='<%#Eval("TaskUID")%>' CommandName="delete"><span title="Delete" class="fas fa-trash"></span></asp:LinkButton>
                                                 </ItemTemplate>
                                             </asp:TemplateField>
                                                <%--<asp:TemplateField ItemStyle-Width="8%">
                                                <ItemTemplate>
                                                    <asp:LinkButton ID="lnkUp" CommandName="up" CommandArgument='<%#Eval("TaskUID")%>' CssClass="btn-link" runat="server" ><span title="Up" class="fas fa-chevron-circle-up"></span></asp:LinkButton>&nbsp;
                                                    <asp:LinkButton ID="lnkDown" CommandName="down" CommandArgument='<%#Eval("TaskUID")%>' CssClass="btn-link"  runat="server"><span title="Down" class="fas fa-chevron-circle-down"></span></asp:LinkButton>
                                                </ItemTemplate>
                                            </asp:TemplateField>--%>
                                        </Columns>
                                        <EmptyDataTemplate>
                                                    <strong>No Records Found ! </strong>
                                                </EmptyDataTemplate>
                                        </asp:GridView>
                                            <asp:GridView ID="GrdOptions" runat="server" AutoGenerateColumns="false"
                                        AllowPaging="false" CssClass="table table-bordered" EmptyDataText="No Data" Width="100%" OnRowCommand="GrdOptions_RowCommand">
                                        <Columns>
                                            <asp:TemplateField HeaderText="Name">
                                                <ItemTemplate>
                                                   <%-- <asp:LinkButton ID="lnkview" runat="server" CommandName="view" CommandArgument='<%#Eval("Workpackage_OptionUID")%>'><%#GetWorkpackageOptionName(Eval("Workpackage_OptionUID").ToString())%></asp:LinkButton>--%>
                                                     <asp:LinkButton ID="lnkview" runat="server" CommandName="view" CommandArgument='<%#Eval("WorkpackageSelectedOption_UID")%>'><%#Eval("WorkpackageSelectedOption_Name")%></asp:LinkButton>
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
                                <div class="card mb-4" id="EnableOption" runat="server">
                                    <div class="card-body">
                                         <div class="card-title">
                                            <div class="d-flex justify-content-between">
                                                <h6 class="text-muted">
                                                    <asp:Label ID="lbloption" runat="server" Text="This option is disbaled. Click 'Enable' button enable the option" />
                                                    <%--<asp:Label Text="Foo bar" CssClass="pl-1" runat="server" />--%>
                                                </h6>
                                                <div>
                                                    <asp:Button ID="btnEnableOption" runat="server" Text="Enable" CausesValidation="false" OnClick="btnEnableOption_Click" CssClass="btn btn-primary"></asp:Button>
                                                </div>
                                            </div>
                                        </div>
                                        </div>
                                </div>
                                <div class="card mb-4" id="MileStoneList" runat="server" visible="false">
                                        <div class="card-body">

                                            <div class="card-title">
                                                <div class="d-flex justify-content-between">
                                                    <h6 class="text-muted">
                                                        <asp:Label Text="List of MileStones" CssClass="text-uppercase font-weight-bold" runat="server" />
                                                        <%--<asp:Label Text="Foo bar" CssClass="pl-1" runat="server" />--%>
                                                    </h6>
                                                    <div>
                                                        <a id="AddTaskMileStone" runat="server" href="/_modal_pages/add-milestone.aspx" class="showModalMilestone"><asp:Button ID="btnMilestoneAdd" runat="server" Text="+ Add MileStone" CssClass="btn btn-primary"></asp:Button></a>
                                                    </div>
                                                </div>
                                            </div>
                                            <div class="table-responsive">
                                            <asp:GridView ID="grdMileStones" runat="server" AllowPaging="false" CssClass="table table-bordered" AutoGenerateColumns="False" Width="100%" OnRowDataBound="grdMileStones_RowDataBound" OnRowCommand="grdMileStones_RowCommand" OnRowDeleting="grdMileStones_RowDeleting">
                                                    <Columns>
                                                        <asp:BoundField DataField="MileStoneUID" HeaderText="UID" HeaderStyle-CssClass="hideItem" ItemStyle-CssClass="hideItem">
                                                        <HeaderStyle HorizontalAlign="Left" />

                                    <ItemStyle CssClass="hideItem"></ItemStyle>
                                                        </asp:BoundField>
                                                        <asp:BoundField DataField="Description" HeaderText="Project MileStone" >
                                                        <HeaderStyle HorizontalAlign="Left" />
                                                        </asp:BoundField>
                                                        <asp:BoundField DataField="MileStoneDate" HeaderText="Planned Date"  DataFormatString="{0:dd MMM yyyy}">
                                                        <HeaderStyle HorizontalAlign="Left" />
                                                        </asp:BoundField>
                                                        <asp:BoundField DataField="ProjectedDate" HeaderText="Projected Date"  DataFormatString="{0:dd MMM yyyy}">
                                                        <HeaderStyle HorizontalAlign="Left" />
                                                        </asp:BoundField>
                                                        <asp:BoundField DataField="Status" HeaderText="Status" >
                                                        <HeaderStyle HorizontalAlign="Left" />
                                                        </asp:BoundField>
                                                           <asp:TemplateField>
                                                        <ItemTemplate>
                                                             <a id="EditMileStone"  href="#" data-href="/_modal_pages/add-milestone.aspx?type=edit&MileStoneUID=<%#Eval("MileStoneUID")%>&TaskUID=<%#Eval("TaskUID")%>" class="showModalEditMilestone"><span title="Edit" class="fas fa-edit"></span></a>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                         <asp:BoundField DataField="ProjectedDate" HeaderText="Projected Date" HeaderStyle-CssClass="hideItem" ItemStyle-CssClass="hideItem">
                                                        <HeaderStyle HorizontalAlign="Left" />
                                                        </asp:BoundField>
                                                         <asp:TemplateField>
                                                            <ItemTemplate>
                                                                   <asp:LinkButton ID="lnkdelete" runat="server" CausesValidation="false" OnClientClick="return DeleteItem()" CommandArgument='<%#Eval("MileStoneUID")%>' CommandName="delete"><span title="Delete" class="fas fa-trash"></span></asp:LinkButton>
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
                                <div class="card mb-4" id="ResourceAllocatedList" runat="server" visible="false">
                                        <div class="card-body">
                                            <div class="card-title">
                                                <div class="d-flex justify-content-between">
                                                    <h6 class="text-muted">
                                                        <asp:Label Text="List of Resources Allocated" CssClass="text-uppercase font-weight-bold" runat="server" />
                                                        <%--<asp:Label Text="Foo bar" CssClass="pl-1" runat="server" />--%>
                                                    </h6>
                                                    <div>
                                                        <a id="AddTaskResources"  runat="server" href="/_modal_pages/add-resourceallocated.aspx" class="showModalResource"><asp:Button ID="btnResourceAdd" runat="server" Text="+ Add Resource" CssClass="btn btn-primary"></asp:Button></a>
                                                    </div>
                                                </div>
                                            </div>

                                            
                                            <div class="table-responsive">
                                            <asp:GridView ID="grdResourceAllocated" runat="server" AllowPaging="false" CssClass="table table-bordered" AutoGenerateColumns="False" Width="100%" OnRowDataBound="grdResourceAllocated_RowDataBound"  OnRowCommand="grdResourceAllocated_RowCommand" OnRowDeleting="grdResourceAllocated_RowDeleting">
                                                <Columns>
                                                    <asp:BoundField DataField="UID" HeaderText="UID" HeaderStyle-CssClass="hideItem" ItemStyle-CssClass="hideItem">
                                                    <HeaderStyle HorizontalAlign="Left" />

                                <ItemStyle CssClass="hideItem"></ItemStyle>
                                                    </asp:BoundField>
                                                    <asp:BoundField DataField="Description" HeaderText="BOQ Description" >
                                                    <HeaderStyle HorizontalAlign="Left" />
                                                    </asp:BoundField>
                                                    <asp:TemplateField HeaderText="Cost" HeaderStyle-HorizontalAlign="Left">
                                                        <ItemTemplate>
                                                            <span style="color:#006699;"><%#Eval("Currency")%></span>&nbsp;<%#string.Format(new System.Globalization.CultureInfo(Eval("Currency_CultureInfo").ToString()),"{0:N}", Eval("Price"))%> + <%#Eval("GST")%>%
                                                            <%--<span style="color:#006699;">&#X20B9;</span>&nbsp;<%#Eval("Total_Budget","{0:N}")%>--%>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>

                                                      <asp:BoundField DataField="Unit" HeaderText="Type">
                                                    <HeaderStyle HorizontalAlign="Left" />
                                                    </asp:BoundField>
                                                    
                                                     <asp:BoundField DataField="AllocatedUnits" HeaderText="AllocatedUnits" >
                                                    <HeaderStyle HorizontalAlign="Left" />
                                                    </asp:BoundField>
                                                    <asp:BoundField DataField="TaskUID" HeaderText="UsedUnits" >
                                                    <HeaderStyle HorizontalAlign="Left" />
                                                    </asp:BoundField>
                                                    <asp:TemplateField HeaderText="Total Cost">
                                                        <ItemTemplate>
                                                            <span style="color:#006699;"><%#Eval("Currency")%></span>&nbsp;<%#string.Format(new System.Globalization.CultureInfo(Eval("Currency_CultureInfo").ToString()),"{0:N}", Eval("TotalCost"))%>
                                                            <%--<span style="color:#006699;">&#X20B9;</span>&nbsp;<%#Eval("TotalCost","{0:N}")%>--%>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                  <asp:BoundField DataField="ResourceUID" HeaderText="ResourceUID" HeaderStyle-CssClass="hideItem" ItemStyle-CssClass="hideItem">
                                                    <HeaderStyle HorizontalAlign="Left" />
                                                    </asp:BoundField>
                   
                                                       <asp:TemplateField>
                                                    <ItemTemplate>
                                                         <a class="showModalResource" href="/_modal_pages/add-resourceallocated.aspx?ResoureAllocationUID=<%#Eval("UID")%>&TaskUID=<%#Eval("TaskUID")%>&ProjectUID=<%#Eval("ProjectUID")%>&WorkPackageUID=<%#Eval("WorkPackageUID")%>"><span title="Edit" class="fas fa-edit"></span></a>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                    <asp:TemplateField>
                                                            <ItemTemplate>
                                                                   <asp:LinkButton ID="lnkdelete" runat="server" CausesValidation="false" OnClientClick="return DeleteItem()" CommandArgument='<%#Eval("UID")%>' CommandName="delete"><span title="Delete" class="fas fa-trash"></span></asp:LinkButton>
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
                                
                                <div class="card mb-4" id="ResourceDeployment" runat="server" visible="false">
                                        <div class="card-body">
                                            <div class="card-title">
                                                <div class="d-flex justify-content-between">
                                                    <h6 class="text-muted">
                                                        <asp:Label Text="List of Resources Deployment" CssClass="text-uppercase font-weight-bold" runat="server" />
                                                        <%--<asp:Label Text="Foo bar" CssClass="pl-1" runat="server" />--%>
                                                    </h6>
                                                    <div>
                                                        <%--<a id="AddResourceDeployment"  runat="server" href="#" class="showModalResource"><asp:Button ID="BtnAddResourceDeployment" runat="server" Text="+ Add Resource Planned" CssClass="btn btn-primary"></asp:Button></a>--%>
                                                    </div>
                                                </div>
                                            </div>
                                            <div class="table-responsive">
                                            <asp:GridView ID="GrdResourceDeployment" runat="server" AllowPaging="false" CssClass="table table-bordered" AutoGenerateColumns="False" Width="100%">
                                                <Columns>
                                                    <asp:BoundField DataField="ResourceName" HeaderText="Resource Name" >
                                                    <HeaderStyle HorizontalAlign="Left" />
                                                    </asp:BoundField>
                                                      <asp:BoundField DataField="Resource_Description" HeaderText="Resource Description">
                                                    <HeaderStyle HorizontalAlign="Left" />
                                                    </asp:BoundField>
                                                    <%--<asp:BoundField DataField="Unit" HeaderText="Type">
                                                    <HeaderStyle HorizontalAlign="Left" />
                                                    </asp:BoundField>--%>
                                                     <asp:BoundField DataField="Unit_for_Measurement" HeaderText="Unit for Measurement">
                                                    <HeaderStyle HorizontalAlign="Left" />
                                                    </asp:BoundField>
                                                    <asp:TemplateField HeaderText="Planned Resource">
                                                        <ItemTemplate>
                                                            <a href="/_modal_pages/add-resourceplanned.aspx?WorkpackageUID=<%#Eval("WorkPackageUID")%>&ResourceUID=<%#Eval("ResourceUID")%>" class="showModalResourceDeployement">+ Add</a>
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
                                <div class="card" id="Dependencies" runat="server" visible="false">
                                        <div class="card-body">
                                            <div class="card-title">
                                                <div class="d-flex justify-content-between">
                                                    <h6 class="text-muted">
                                                        <asp:Label Text="List of Dependencies" CssClass="text-uppercase font-weight-bold" runat="server" />
                                                        <%--<asp:Label Text="Foo bar" CssClass="pl-1" runat="server" />--%>
                                                    </h6>
                                                    <div>
                                                        <a id="AddDependency"  runat="server" href="/_modal_pages/add-dependency.aspx" class="showModalDependency"><asp:Button ID="btnDependency" runat="server" Text="+ Add Dependency" CssClass="btn btn-primary"></asp:Button></a>
                                                    </div>
                                                </div>
                                            </div>

                                            
                                            <div class="table-responsive">
                                            <asp:GridView ID="GrdDependency" runat="server" AllowPaging="false" CssClass="table table-bordered" AutoGenerateColumns="False" Width="100%" OnRowCommand="GrdDependency_RowCommand" OnRowDeleting="GrdDependency_RowDeleting">
                                                <Columns>
                                                  <asp:TemplateField HeaderText="Dependent Task">
                                                    <ItemTemplate>
                                                        <%#getTaskName(Eval("Dependent_TaskUID").ToString())%>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                    <asp:BoundField DataField="Dependency_StartDate" HeaderText="Start Date"  DataFormatString="{0:dd MMM yyyy}" />
                                                    <asp:BoundField DataField="Dependency_PlannedEndDate" HeaderText="Planned EndDate"  DataFormatString="{0:dd MMM yyyy}" />
                                                    <asp:TemplateField HeaderText="Dependency Type">
                                                        <ItemTemplate>
                                                            <%#DependencyType(Eval("Dependency_Type").ToString())%>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:BoundField DataField="Dependency_Desc" HeaderText="Description" />
                                                     <asp:TemplateField>
                                                        <ItemTemplate>
                                                             <a id="EditDependency"  href="#" data-href="/_modal_pages/add-dependency.aspx?type=edit&Dependency_UID=<%#Eval("Dependency_UID")%>&TaskUID=<%#Eval("TaskUID")%>&WorkUID=<%#Eval("WorkPackageUID")%>" class="showModalEditDependency"><span title="Edit" class="fas fa-edit"></span></a>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                     <asp:TemplateField>
                                                            <ItemTemplate>
                                                                   <asp:LinkButton ID="lnkdelete" runat="server" CausesValidation="false" OnClientClick="return DeleteItem()" CommandArgument='<%#Eval("Dependency_UID")%>' CommandName="delete"><span title="Delete" class="fas fa-trash"></span></asp:LinkButton>
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
                        </ContentTemplate>  
                        <Triggers>
                            <asp:AsyncPostBackTrigger ControlID="GrdTreeView" EventName="RowCommand" />
                            <asp:AsyncPostBackTrigger ControlID="GrdOptions" EventName="RowCommand" />
                         <asp:AsyncPostBackTrigger ControlID="TreeView1" EventName="SelectedNodeChanged" />
                            <asp:PostBackTrigger ControlID="BtnSearchDocuments" />
                         </Triggers>
                 </asp:UpdatePanel>
      </div>

     <%--add work package modal--%>
    <div id="ModAddWorkpackages" class="modal it-modal fade">
	    <div class="modal-dialog modal-dialog-centered modal-dialog-scrollable modal-xl">
		    <div class="modal-content">
			    <div class="modal-header">
				    <h5 class="modal-title">Add Work Package</h5>
                    <button aria-label="Close" class="close" onclick="javascript:window.location.reload()" data-dismiss="modal" type="button"><span aria-hidden="true">&times;</span></button>
			    </div>
			    <div class="modal-body">
                    <iframe class="border-0 w-100" style="height: 500px;" loading="lazy"></iframe>
			    </div>
		    </div>
	    </div>
    </div>

      <%--add Task modal--%>
    <div id="ModAddTask" class="modal it-modal fade">
	    <div class="modal-dialog modal-dialog-centered modal-dialog-scrollable modal-xl">
		    <div class="modal-content">
			    <div class="modal-header">
				    <h5 class="modal-title">Add Task</h5>
                    <button aria-label="Close" class="close" data-dismiss="modal" type="button"><span aria-hidden="true">&times;</span></button>
			    </div>
			    <div class="modal-body">
                    <iframe class="border-0 w-100" style="height: 500px;" loading="lazy"></iframe>
			    </div>
              
		    </div>
	    </div>
    </div>

    <%--add Task modal--%>
    <div id="ModAddSubTask" class="modal it-modal fade">
	    <div class="modal-dialog modal-dialog-centered modal-dialog-scrollable modal-xl">
		    <div class="modal-content">
			    <div class="modal-header">
				    <h5 class="modal-title">Add Sub Task</h5>
                    <button aria-label="Close" class="close" data-dismiss="modal" type="button"><span aria-hidden="true">&times;</span></button>
			    </div>
			    <div class="modal-body">
                    <iframe class="border-0 w-100" style="height: 500px;" loading="lazy"></iframe>
			    </div>
              
		    </div>
	    </div>
    </div>

    <%--add Task modal--%>
    <div id="ModEditTask" class="modal it-modal fade">
	    <div class="modal-dialog modal-dialog-centered modal-dialog-scrollable modal-xl">
		    <div class="modal-content">
			    <div class="modal-header">
				    <h5 class="modal-title">Edit Task</h5>
                    <button aria-label="Close" class="close" data-dismiss="modal" type="button"><span aria-hidden="true">&times;</span></button>
			    </div>
			    <div class="modal-body">
                    <iframe class="border-0 w-100" style="height: 500px;" loading="lazy"></iframe>
			    </div>
              
		    </div>
	    </div>
    </div>

      <%--add MileStone modal--%>
    <div id="ModAddMilestone" class="modal it-modal fade">
	    <div class="modal-dialog modal-dialog-centered modal-dialog-scrollable modal-lg">
		    <div class="modal-content">
			    <div class="modal-header">
				    <h5 class="modal-title">Add Milestone</h5>
                    <button aria-label="Close" class="close" data-dismiss="modal" type="button"><span aria-hidden="true">&times;</span></button>
			    </div>
			    <div class="modal-body">
                    <iframe class="border-0 w-100" style="height:420px;" loading="lazy"></iframe>
			    </div>
              
		    </div>
	    </div>
        </div>

    <%--add MileStone modal--%>
    <div id="ModEditMilestone" class="modal it-modal fade">
	    <div class="modal-dialog modal-dialog-centered modal-dialog-scrollable modal-lg">
		    <div class="modal-content">
			    <div class="modal-header">
				    <h5 class="modal-title">Edit Milestone</h5>
                    <button aria-label="Close" class="close" data-dismiss="modal" type="button"><span aria-hidden="true">&times;</span></button>
			    </div>
			    <div class="modal-body">
                    <iframe class="border-0 w-100" style="height:420px;" loading="lazy"></iframe>
			    </div>
              
		    </div>
	    </div>
        </div>

         <%--add Add Resource Allocated modal--%>
    <div id="ModAddResourceAllocation" class="modal it-modal fade">
	    <div class="modal-dialog modal-dialog-centered modal-dialog-scrollable modal-lg">
		    <div class="modal-content">
			    <div class="modal-header">
				    <h5 class="modal-title">Add Resource Allocation</h5>
                    <button aria-label="Close" class="close" data-dismiss="modal" type="button"><span aria-hidden="true">&times;</span></button>
			    </div>
			    <div class="modal-body">
                    <iframe class="border-0 w-100" style="height:420px;" loading="lazy"></iframe>
			    </div>
              
		    </div>
	    </div>
    </div>
    

      <%--add Dependency modal--%>
    <div id="ModAddDependency" class="modal it-modal fade">
	    <div class="modal-dialog modal-dialog-centered modal-dialog-scrollable modal-lg">
		    <div class="modal-content">
			    <div class="modal-header">
				    <h5 class="modal-title">Add Dependency</h5>
                    <button aria-label="Close" class="close" data-dismiss="modal" type="button"><span aria-hidden="true">&times;</span></button>
			    </div>
			    <div class="modal-body">
                    <iframe class="border-0 w-100" style="height:450px;" loading="lazy"></iframe>
			    </div>
              
		    </div>
	    </div>
        </div>

    <%--Edit Dependency modal--%>
    <div id="ModEditDependency" class="modal it-modal fade">
	    <div class="modal-dialog modal-dialog-centered modal-dialog-scrollable modal-lg">
		    <div class="modal-content">
			    <div class="modal-header">
				    <h5 class="modal-title">Edit Dependency</h5>
                    <button aria-label="Close" class="close" data-dismiss="modal" type="button"><span aria-hidden="true">&times;</span></button>
			    </div>
			    <div class="modal-body">
                    <iframe class="border-0 w-100" style="height:450px;" loading="lazy"></iframe>
			    </div>
              
		    </div>
	    </div>
        </div>

    <%--Change Activity Order modal--%>
    <div id="ModSortActivity" class="modal it-modal fade">
	    <div class="modal-dialog modal-dialog-centered modal-dialog-scrollable modal-xl">
		    <div class="modal-content">
			    <div class="modal-header">
				    <h5 class="modal-title">Change Activity Order</h5>
                    <button aria-label="Close" class="close" data-dismiss="modal" type="button"><span aria-hidden="true">&times;</span></button>
			    </div>
			    <div class="modal-body">
                    <iframe class="border-0 w-100" style="height:500px;" loading="lazy"></iframe>
			    </div>
              
		    </div>
	    </div>
    </div>

    <%--Copy Activity Data modal--%>
    <div id="ModCopyActivity" class="modal it-modal fade">
	    <div class="modal-dialog modal-dialog-centered modal-dialog-scrollable modal-lg">
		    <div class="modal-content">
			    <div class="modal-header">
				    <h5 class="modal-title">Copy Activity Data</h5>
                    <button aria-label="Close" class="close" data-dismiss="modal" type="button"><span aria-hidden="true">&times;</span></button>
			    </div>
			    <div class="modal-body">
                    <iframe class="border-0 w-100" style="height:380px;" loading="lazy"></iframe>
			    </div>
              
		    </div>
	    </div>
    </div>    
    <%--Copy Project Data modal--%>
    <div id="ModCopyProjectData" class="modal it-modal fade">
	    <div class="modal-dialog modal-dialog-centered modal-dialog-scrollable modal-lg">
		    <div class="modal-content">
			    <div class="modal-header">
				    <h5 class="modal-title">Copy Project Data</h5>
                    <button aria-label="Close" class="close" data-dismiss="modal" type="button"><span aria-hidden="true">&times;</span></button>
			    </div>
			    <div class="modal-body">
                    <iframe class="border-0 w-100" style="height:300px;" loading="lazy"></iframe>
			    </div>
              
		    </div>
	    </div>
    </div>

    <%--Add Task Schedule--%>
    <div id="ModAddTaskSchedule" class="modal it-modal fade">
	    <div class="modal-dialog modal-dialog-centered modal-dialog-scrollable modal-xl">
		    <div class="modal-content">
			    <div class="modal-header">
				    <h5 class="modal-title">Add Task Schedule</h5>
                    <button aria-label="Close" class="close" data-dismiss="modal" type="button"><span aria-hidden="true">&times;</span></button>
			    </div>
			    <div class="modal-body">
                    <iframe class="border-0 w-100" style="height:450px;" loading="lazy"></iframe>
			    </div>
              
		    </div>
	    </div>
    </div>

    <%--Upload Site Photographs--%>
    <div id="ModAddUploadSiteImages" class="modal it-modal fade">
	    <div class="modal-dialog modal-dialog-centered modal-dialog-scrollable modal-xl">
		    <div class="modal-content">
			    <div class="modal-header">
				    <h5 class="modal-title">Upload Site Photographs</h5>
                    <button aria-label="Close" class="close" data-dismiss="modal" type="button"><span aria-hidden="true">&times;</span></button>
			    </div>
			    <div class="modal-body">
                    <iframe class="border-0 w-100" style="height:450px;" loading="lazy"></iframe>
			    </div>
              
		    </div>
	    </div>
    </div>

    <%--Add Resourec Planned--%>
    <div id="ModAddResourceDeployement" class="modal it-modal fade">
	    <div class="modal-dialog modal-dialog-centered modal-dialog-scrollable modal-xl">
		    <div class="modal-content">
			    <div class="modal-header">
				    <h5 class="modal-title">Add Planned Resource Monthwise</h5>
                    <button aria-label="Close" class="close" data-dismiss="modal" type="button"><span aria-hidden="true">&times;</span></button>
			    </div>
			    <div class="modal-body">
                    <iframe class="border-0 w-100" style="height:450px;" loading="lazy"></iframe>
			    </div>
              
		    </div>
	    </div>
    </div>

    

    <%--View Site Photographs--%>
    <div id="ModViewSitePhotograph" class="modal it-modal fade">
	    <div class="modal-dialog modal-dialog-centered modal-dialog-scrollable modal-xl">
		    <div class="modal-content">
			    <div class="modal-header">
				    <h5 class="modal-title">View Site Photograph/s</h5>
                    <button aria-label="Close" class="close" data-dismiss="modal" type="button"><span aria-hidden="true">&times;</span></button>
			    </div>
			    <div class="modal-body">
                    <iframe class="border-0 w-100" style="height:450px;" loading="lazy"></iframe>
			    </div>
              
		    </div>
	    </div>
    </div>
</asp:Content>
