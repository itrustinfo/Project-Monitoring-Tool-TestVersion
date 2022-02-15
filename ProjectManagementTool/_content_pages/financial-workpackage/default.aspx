<%@ Page Title="" Language="C#" MasterPageFile="~/_master_pages/default.Master" AutoEventWireup="true" CodeBehind="default.aspx.cs" Inherits="ProjectManagementTool._content_pages.financial_workpackage._default" %>
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
                 

        $(".showModalFinanceTask").click(function(e) {
        e.preventDefault();
        var url = $(this).attr("href");
        $("#ModAddFinanceTask iframe").attr("src", url);
        $("#ModAddFinanceTask").modal("show");
              });

        $(".showModalSubFinanceTask").click(function(e) {
        e.preventDefault();
        var url = $(this).attr("href");
        $("#ModAddFinanceSubTask iframe").attr("src", url);
        $("#ModAddFinanceSubTask").modal("show");
          });

         $(".showModalEditTask").click(function(e) {
        e.preventDefault();
        var url = $(this).attr("href");
        $("#ModEditFinanceTask iframe").attr("src", url);
        $("#ModEditFinanceTask").modal("show");
            });

            $(".showModalFinanceMileStone").click(function(e) {
        e.preventDefault();
        var url = $(this).attr("href");
        $("#ModAddFinanceMileStone iframe").attr("src", url);
        $("#ModAddFinanceMileStone").modal("show");
            });

            $(".showModalEditFinanceMileStone").click(function(e) {
        e.preventDefault();
        var url = $(this).attr("href");
        $("#ModEditFinanceMileStone iframe").attr("src", url);
        $("#ModEditFinanceMileStone").modal("show");
            });

            $(".showModalHistroyFinanceMileStone").click(function(e) {
        e.preventDefault();
        var url = $(this).attr("href");
        $("#ModPaymentHistroyFinanceMileStone iframe").attr("src", url);
        $("#ModPaymentHistroyFinanceMileStone").modal("show");
            });

            $(".showModalViewPaymentHistory").click(function(e) {
        e.preventDefault();
        var url = $(this).attr("href");
        $("#ModTaskPaymentHistroy iframe").attr("src", url);
        $("#ModTaskPaymentHistroy").modal("show");
            });

        }
        $(document).ready(function () {
            BindEvents();
            //$('#loader').fadeOut();
        });
</script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="default_master_body" runat="server">
    <%--project selection dropdowns--%>
    <asp:ScriptManager ID="ScriptManager1" runat="server">
   </asp:ScriptManager>
    <%--<div id="loader"></div>--%>
    <div class="container-fluid">
        <div class="row">
            <div class="col-lg-6 col-xl-8 form-group">WorkPackages</div>
            <div class="col-lg-6 col-xl-4 form-group">
               <%-- <label class="sr-only" for="DDLProject">Project</label>
                <div class="input-group">
                    <div class="input-group-prepend">
                        <span class="input-group-text">Project</span>
                    </div>
                    <asp:DropDownList CssClass="form-control" ID="DDLProject" runat="server"></asp:DropDownList>
                </div>--%>
            </div>
        </div>
    </div>
        <%--documents contents--%>
    <div class="container-fluid">
        <asp:UpdatePanel ID="UpdatePanel2" runat="server">
                    <ContentTemplate>
                    <div class="row">
                        <div class="col-lg-6 col-xl-4 form-group">
                            <div class="card h-100" style="max-height:700px; overflow-y:auto;">
                                <div class="card-body">
                                    <h6 class="card-title text-muted text-uppercase font-weight-bold">WorkPackage Directory</h6>
                                    <div class="form-group">
                                        <label class="sr-only" for="TxtSearchDocuments">Search</label>
                                        <div class="input-group">
                                            <asp:TextBox ID="txtTreeviewSearch" runat="server" CssClass="form-control" placeholder="Activity name"></asp:TextBox>
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
                            <asp:UpdateProgress ID="UpdateProgress1" runat="Server" AssociatedUpdatePanelID="UpdatePanel2" DisplayAfter="1"> 
                                <ProgressTemplate > 
                                <!-- <asp:Label ID="lblwait" runat="server" Text="Please wait while we fetch the data.." ForeColor="Red"></asp:Label>-->
                                    <div id="loader"></div>
                                </ProgressTemplate> 
                            </asp:UpdateProgress>
                                <div class="card mb-4" id="ProjectDetails" runat="server">
                                    <div class="card-body">
                                        <h6 class="card-title text-muted text-uppercase font-weight-bold">Project Details
                                            </h6>
                                        <div class="table-responsive">
                                             <table class="table table-borderless">
                                           <tr><td>Project Name</td><td>:</td><td colspan="2">
                                               <asp:Label ID="lblPrjName" runat="server" Font-Bold="true"></asp:Label>
                                               </td>
                                               </tr>
                                               <tr>
                                                   <td>Funding Agency</td>
                                                   <td>:</td><td colspan="2">
                                                   <asp:Label ID="LblFundingAgency" runat="server"></asp:Label></td>
                                               </tr>
                                                 <tr><td>Budget</td><td>:</td><td>
                                                   <asp:Label ID="LblPrjBudgetCurrency" runat="server"></asp:Label>
                                                     <asp:Label ID="lblPrjBudget" runat="server"></asp:Label>
                                                     </td><td>Actual Expenditure</td><td>:</td><td>
                                                     <asp:Label ID="lblPrjActaulExpenditureCurrency" runat="server"></asp:Label>
                                                         <asp:Label ID="lblPrjActaulExpenditure" runat="server"></asp:Label>
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
                                               </table>
                                            </div>
                                        </div>
                                    </div>
                                <div class="card mb-4" id="WorkPackageDetils" runat="server">
                                    <div class="card-body">
                                        <h6 class="card-title text-muted text-uppercase font-weight-bold">WorkPackage Details
                                            </h6>
                                        <div class="table-responsive">
                                            <table class="table table-borderless">
                               <tr><td>Name</td><td>:</td><td colspan="2">
                                   <asp:Label ID="LblWorkPackageName" runat="server" Font-Bold="true"></asp:Label>
                                   </td></tr>
                                 <tr>
                                     <td>Location </td><td>:</td><td colspan="2">
                                   <asp:Label ID="LblWorkPackageLocation" runat="server"></asp:Label>
                                   </td>
                                     
                                     </tr>
                               <tr>
                                   <td>Budget</td><td>:</td><td>
                                     <asp:Label ID="LblWorkPackageBudgetCurrency" runat="server"></asp:Label>
                                       <asp:Label ID="LblWorkPackageBudget" runat="server"></asp:Label>
                                     </td>
                                   <td>Actual Expenditure</td><td>:</td><td>
                                       <asp:Label ID="lblWorkpackageActualExpenditureCurrency" runat="server"></asp:Label>
                                     <asp:Label ID="lblWorkpackageActualExpenditure" runat="server"></asp:Label>
                                     </td>
                               </tr>
             
                           </table>
                                            </div>
                                        </div>
                                    </div>
                                <div class="card mb-4" id="TaskDetails" runat="server">
                                    <div class="card-body">
                                        <h6 class="card-title text-muted text-uppercase font-weight-bold">Task Details
                            
                                            </h6>
                                        <div class="table-responsive">
                                            <table class="table table-borderless">
                                               <tr><td>Task Name</td><td>:</td><td colspan="2">
                                                   <asp:Label ID="LblTaskName" runat="server" Font-Bold="true"></asp:Label>
                                                   </td></tr>
                                                <tr>
                                                    <td>Task Descrition </td><td>:</td><td colspan="2">
                                                   <asp:Label ID="LblTaskDescription" runat="server"></asp:Label>
                                                   </td>
                                                </tr>
                                                 
                                                 <tr><td>Total Budget(Including GST)</td><td>:</td><td>
                                                     <asp:Label ID="LblTaskBudgetCurrency" runat="server"></asp:Label>
                                                     <asp:Label ID="LblTaskBudget" runat="server"></asp:Label>
                                                     </td>
                                                     <td>GST(in %)</td><td>:</td><td>
                                                         <asp:Label ID="LblTaskGST" runat="server"></asp:Label>
                                                                                 </td>
                                                     <td>Actual Expenditure</td><td>:</td><td>
                                                         <asp:Label ID="LblTaskActualExpenditureCurrency" runat="server"></asp:Label>
                                                     <asp:Label ID="LblTaskActualExpenditure" runat="server"></asp:Label>
                                                     </td></tr>
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
                                                   <a id="AddFinanceTask" runat="server" href="/_modal_pages/add-finance-task.aspx?type=Add" class="showModalFinanceTask"><asp:Button ID="btnAddTask" runat="server" Text="+ Add Finance Task" CssClass="btn btn-primary"></asp:Button></a>
                                                   <a id="AddFinanceSubTask" runat="server" href="#" class="showModalSubFinanceTask"><asp:Button ID="btnAddSubTask" runat="server" Text="+ Add Finance SubTask" CssClass="btn btn-primary"></asp:Button></a>
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
                           <asp:TemplateField HeaderText="Budget" ItemStyle-ForeColor="#006699">
                               <ItemTemplate>
                                   <span style="color:#006699;"><%#Eval("Currency")%></span>&nbsp;<%#string.Format(new System.Globalization.CultureInfo(Eval("Currency_CultureInfo").ToString()),"{0:N}", Eval("Budget"))%>
                               </ItemTemplate>
                           </asp:TemplateField>                              
                             <asp:TemplateField HeaderText="Actual Expenditure" ItemStyle-ForeColor="#006699">
                               <ItemTemplate>
                                   <span style="color:#006699;"><%#Eval("Currency")%></span>&nbsp;<%#string.Format(new System.Globalization.CultureInfo(Eval("Currency_CultureInfo").ToString()),"{0:N}", Eval("ActualExpenditure"))%>
                               </ItemTemplate>
                           </asp:TemplateField>
                             <%--<asp:TemplateField>
                                <ItemTemplate>
                                     <a class="view" id="edit" href="AddSubTask.aspx?type=edit&TaskUID=<%#Eval("TaskUID")%>&PrjUID=<%#Eval("ProjectUID")%>&WrkUID=<%#Eval("WorkPackageUID")%>">View</a>
                                </ItemTemplate>
                            </asp:TemplateField>--%>
                            <asp:TemplateField>
                                <ItemTemplate>
                                       <asp:LinkButton ID="lnkdelete" runat="server" CausesValidation="false" OnClientClick="return DeleteItem()" CommandArgument='<%#Eval("TaskUID")%>' CommandName="delete"><span title="Delete" class="fas fa-trash"></span></asp:LinkButton>
                                 </ItemTemplate>
                             </asp:TemplateField>
                                             <asp:BoundField DataField="Currency" HeaderText="Currency" HeaderStyle-CssClass="hideItem" ItemStyle-CssClass="hideItem">
                                                        <HeaderStyle HorizontalAlign="Left" />
                                                <ItemStyle CssClass="hideItem"></ItemStyle>
                                                        </asp:BoundField>
                                                <asp:BoundField DataField="Currency_CultureInfo" HeaderText="Currency_CultureInfo" HeaderStyle-CssClass="hideItem" ItemStyle-CssClass="hideItem">
                                                        <HeaderStyle HorizontalAlign="Left" />
                                                    <ItemStyle CssClass="hideItem"></ItemStyle>
                                                        </asp:BoundField>
                                        </Columns>
                                        <EmptyDataTemplate>
                                    No Records Found !
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
                                
                            <%--<div class="card mb-4">
                                    <div class="card-body">
                                        <div class="card-title">
                                            <div class="d-flex justify-content-between">
                                                <h6 class="text-muted">
                                                    <asp:Label ID="LblPaymentRegisterHeading" CssClass="text-uppercase font-weight-bold" Text="Payment Register" runat="server" />
                                                    
                                                </h6>
                                               <div>
                                                  
                                               </div>
                                            </div>
                                        </div>
                                        <div class="table-responsive">
                                            <asp:GridView ID="grdTaskPayment" runat="server" AllowCustomPaging="false" AutoGenerateColumns="False" CssClass="table table-bordered" Width="100%">
                                            <Columns>
                                               
                                                 <asp:BoundField DataField="ActualPaymentDate" HeaderText="Date"  DataFormatString="{0:dd MMM yyyy}">
                                                <HeaderStyle HorizontalAlign="Left" />
                                                </asp:BoundField>
                                              
                                                <asp:TemplateField HeaderText="Actual Payment">
                                                    <ItemTemplate>
                                                        <span style="color:#006699;"><%#Eval("Currency")%></span>&nbsp;<%#string.Format(new System.Globalization.CultureInfo(Eval("Currency_CultureInfo").ToString()),"{0:N}", Eval("ActualPayment"))%>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                             
                                               
                                                   <asp:TemplateField HeaderText="Cumulative">
                                                    <ItemTemplate>
                                                        <span style="color:#006699;"><%#Eval("Currency")%></span>&nbsp;<%#string.Format(new System.Globalization.CultureInfo(Eval("Currency_CultureInfo").ToString()),"{0:N}", Eval("AllowedPayment"))%>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                               
                                            </Columns>
                                            <EmptyDataTemplate>
                                                <strong>No Records Found ! </strong>
                                            </EmptyDataTemplate>
                                        </asp:GridView>
                                            </div>
                                        </div>
                                    </div>--%>

                            <div class="card mb-4" id="FinanceMileStone" runat="server">
                                    <div class="card-body">
                                        <div class="card-title">
                                            <div class="d-flex justify-content-between">
                                                <h6 class="text-muted">
                                                    <asp:Label ID="LblFinanceMileStoneHeading" CssClass="text-uppercase font-weight-bold" Text="Finance MileStone" runat="server" />
                                                    <%--<asp:Label Text="Foo bar" CssClass="pl-1" runat="server" />--%>
                                                </h6>
                                               <div>
                                                  <a id="AddFinanceMileStone" runat="server" href="/_modal_pages/add-financemilestone.aspx?type=Add" class="showModalFinanceMileStone"><asp:Button ID="btnmilestoneadd" runat="server" Text="+ Add Finance MileStone" CssClass="btn btn-primary"></asp:Button></a>
                                                   <a id="PaymentHistroy" runat="server" href="/_modal_pages/view-taskpaymenthistroy.aspx" class="showModalViewPaymentHistory"><asp:Button ID="tbnhistory" runat="server" Text="Payment History" CssClass="btn btn-primary"></asp:Button></a>
                                               </div>
                                            </div>
                                        </div>
                                        <div class="table-responsive">
                                            <asp:GridView ID="GrdFinanceMileStone" runat="server" AllowPaging="false" AllowCustomPaging="false" AutoGenerateColumns="False" CssClass="table table-bordered" Width="100%" OnRowDataBound="GrdFinanceMileStone_RowDataBound" OnRowCommand="GrdFinanceMileStone_RowCommand" OnRowDeleting="GrdFinanceMileStone_RowDeleting">
                                            <Columns>
                                                <asp:BoundField DataField="Finance_MileStoneName" HeaderText="MileStone Name" >
                                                <HeaderStyle HorizontalAlign="Left" />
                                                </asp:BoundField>

                                                 <asp:BoundField DataField="Finance_PlannedDate" HeaderText="Planned Date"  DataFormatString="{0:dd MMM yyyy}">
                                                <HeaderStyle HorizontalAlign="Left" />
                                                </asp:BoundField>
                                              
                                                <asp:TemplateField HeaderText="Allowed Payment">
                                                    <ItemTemplate>
                                                        <span style="color:#006699;"><%#Eval("Currency")%></span>&nbsp;<%#string.Format(new System.Globalization.CultureInfo(Eval("Currency_CultureInfo").ToString()),"{0:N}", Eval("Finance_AllowedPayment"))%> + GST(<%#Eval("Finance_GST")%> %)
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:BoundField DataField="Finance_MileStoneUID" HeaderText="Cumulative Payment" >
                                                <HeaderStyle HorizontalAlign="Left" />
                                                </asp:BoundField>
                                                <asp:TemplateField>
                                                    <ItemTemplate>
                                                        <a  href="/_modal_pages/view-financemilestonehistory.aspx?Finance_MileStoneUID=<%#Eval("Finance_MileStoneUID")%>" class="showModalHistroyFinanceMileStone">Payment History</a>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                 <asp:TemplateField>
                                                        <ItemTemplate>
                                                             <a  href="/_modal_pages/add-financemilestone.aspx?type=edit&FinanceMileStoneUID=<%#Eval("Finance_MileStoneUID")%>&TaskUID=<%#Eval("TaskUID")%>" class="showModalEditFinanceMileStone"><span title="Edit" class="fas fa-edit"></span></a>
                                                        </ItemTemplate>
                                                 </asp:TemplateField>
                                                <asp:TemplateField>
                                                <ItemTemplate>
                                                       <asp:LinkButton ID="lnkdelete" runat="server" CausesValidation="false" OnClientClick="return DeleteItem()" CommandArgument='<%#Eval("Finance_MileStoneUID")%>' CommandName="delete"><span title="Delete" class="fas fa-trash"></span></asp:LinkButton>
                                                 </ItemTemplate>
                                             </asp:TemplateField>
                                                <asp:BoundField DataField="Currency" HeaderText="Currency" HeaderStyle-CssClass="hideItem" ItemStyle-CssClass="hideItem">
                                                        <HeaderStyle HorizontalAlign="Left" />
                                                <ItemStyle CssClass="hideItem"></ItemStyle>
                                                        </asp:BoundField>
                                                <asp:BoundField DataField="Currency_CultureInfo" HeaderText="Currency_CultureInfo" HeaderStyle-CssClass="hideItem" ItemStyle-CssClass="hideItem">
                                                        <HeaderStyle HorizontalAlign="Left" />
                                                    <ItemStyle CssClass="hideItem"></ItemStyle>
                                                        </asp:BoundField>

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
                         <asp:AsyncPostBackTrigger ControlID="TreeView1" EventName="SelectedNodeChanged" />
                         </Triggers>
                 </asp:UpdatePanel>
      </div>
     <%--add Finance Task modal--%>
    <div id="ModAddFinanceTask" class="modal it-modal fade">
	    <div class="modal-dialog modal-dialog-centered modal-dialog-scrollable modal-xl">
		    <div class="modal-content">
			    <div class="modal-header">
				    <h5 class="modal-title">Add Finance Task</h5>
                    <button aria-label="Close" class="close" data-dismiss="modal" type="button"><span aria-hidden="true">&times;</span></button>
			    </div>
			    <div class="modal-body">
                    <iframe class="border-0 w-100" style="height: 500px;" loading="lazy"></iframe>
			    </div>
              
		    </div>
	    </div>
    </div>

    <%--add Finance Sub Task modal--%>
    <div id="ModAddFinanceSubTask" class="modal it-modal fade">
	    <div class="modal-dialog modal-dialog-centered modal-dialog-scrollable modal-xl">
		    <div class="modal-content">
			    <div class="modal-header">
				    <h5 class="modal-title">Add Finance Sub Task</h5>
                    <button aria-label="Close" class="close" data-dismiss="modal" type="button"><span aria-hidden="true">&times;</span></button>
			    </div>
			    <div class="modal-body">
                    <iframe class="border-0 w-100" style="height: 500px;" loading="lazy"></iframe>
			    </div>
              
		    </div>
	    </div>
    </div>

    <%--Edit Finance Task modal--%>
    <div id="ModEditFinanceTask" class="modal it-modal fade">
	    <div class="modal-dialog modal-dialog-centered modal-dialog-scrollable modal-xl">
		    <div class="modal-content">
			    <div class="modal-header">
				    <h5 class="modal-title">Edit Finance Task</h5>
                    <button aria-label="Close" class="close" data-dismiss="modal" type="button"><span aria-hidden="true">&times;</span></button>
			    </div>
			    <div class="modal-body">
                    <iframe class="border-0 w-100" style="height: 500px;" loading="lazy"></iframe>
			    </div>
              
		    </div>
	    </div>
    </div>

    <%--add Finance milestone modal--%>
    <div id="ModAddFinanceMileStone" class="modal it-modal fade">
	    <div class="modal-dialog modal-dialog-centered modal-dialog-scrollable modal-lg">
		    <div class="modal-content">
			    <div class="modal-header">
				    <h5 class="modal-title">Add Finance MileStone</h5>
                    <button aria-label="Close" class="close" data-dismiss="modal" type="button"><span aria-hidden="true">&times;</span></button>
			    </div>
			    <div class="modal-body">
                    <iframe class="border-0 w-100" style="height:460px;" loading="lazy"></iframe>
			    </div>
              
		    </div>
	    </div>
    </div>

    <%--Edit Finance milestone modal--%>
    <div id="ModEditFinanceMileStone" class="modal it-modal fade">
	    <div class="modal-dialog modal-dialog-centered modal-dialog-scrollable modal-lg">
		    <div class="modal-content">
			    <div class="modal-header">
				    <h5 class="modal-title">Edit Finance MileStone</h5>
                    <button aria-label="Close" class="close" data-dismiss="modal" type="button"><span aria-hidden="true">&times;</span></button>
			    </div>
			    <div class="modal-body">
                    <iframe class="border-0 w-100" style="height:460px;" loading="lazy"></iframe>
			    </div>
              
		    </div>
	    </div>
    </div>

    <%--Edit Finance milestone modal--%>
    <div id="ModPaymentHistroyFinanceMileStone" class="modal it-modal fade">
	    <div class="modal-dialog modal-dialog-centered modal-dialog-scrollable modal-lg">
		    <div class="modal-content">
			    <div class="modal-header">
				    <h5 class="modal-title">MileStone Payment History</h5>
                    <button aria-label="Close" class="close" data-dismiss="modal" type="button"><span aria-hidden="true">&times;</span></button>
			    </div>
			    <div class="modal-body">
                    <iframe class="border-0 w-100" style="height:460px;" loading="lazy"></iframe>
			    </div>
              
		    </div>
	    </div>
    </div>

    <%--Task Payment History modal--%>
    <div id="ModTaskPaymentHistroy" class="modal it-modal fade">
	    <div class="modal-dialog modal-dialog-centered modal-dialog-scrollable modal-xl">
		    <div class="modal-content">
			    <div class="modal-header">
				    <h5 class="modal-title">Task Payment History</h5>
                    <button aria-label="Close" class="close" data-dismiss="modal" type="button"><span aria-hidden="true">&times;</span></button>
			    </div>
			    <div class="modal-body">
                    <iframe class="border-0 w-100" style="height:460px;" loading="lazy"></iframe>
			    </div>
              
		    </div>
	    </div>
    </div>

</asp:Content>
