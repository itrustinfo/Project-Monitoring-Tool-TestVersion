<%@ Page Title="" Language="C#" MasterPageFile="~/_master_pages/default.Master" AutoEventWireup="true" CodeBehind="default.aspx.cs" Inherits="ProjectManagementTool._content_pages.rabill_priceadj._default" %>
<asp:Content ID="Content1" ContentPlaceHolderID="default_master_head" runat="server">
     <script type="text/javascript">
        function DeleteItem() {
            if (confirm("All data associated with this will be deleted. Are you sure you want to delete ...?")) {
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

        }
        $(document).ready(function () {
            BindEvents();
            //$('#loader').fadeOut();
        });
</script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="default_master_body" runat="server">
    <asp:ScriptManager ID="ScriptManager1" runat="server">
   </asp:ScriptManager>
      <div class="container-fluid">
            <div class="row">
                <div class="col-md-12 col-lg-4 form-group">RA Bills Price Adjustment</div>
                <div class="col-md-6 col-lg-4 form-group">
                    <label class="sr-only" for="DDLProject">Project</label>
                    <div class="input-group">
                        <div class="input-group-prepend">
                            <span class="input-group-text">Project</span>
                        </div>
                        <asp:DropDownList ID="DDlProject" runat="server" CssClass="form-control" AutoPostBack="true" OnSelectedIndexChanged="DDlProject_SelectedIndexChanged"></asp:DropDownList>
                       
                    </div>
                </div>
                <div class="col-md-6 col-lg-4 form-group">
                    <label class="sr-only" for="DDLWorkPackage">Work Package</label>
                    <div class="input-group">
                        <div class="input-group-prepend">
                            <span class="input-group-text">Work Package</span>
                        </div>
                        <asp:DropDownList ID="DDLWorkPackage" runat="server" CssClass="form-control" AutoPostBack="true" OnSelectedIndexChanged="DDLWorkPackage_SelectedIndexChanged"></asp:DropDownList>

                       
                    </div>
                </div>
            </div>
        </div>
     <div class="container-fluid">
       <%-- <asp:UpdatePanel ID="UpdatePanel2" runat="server" UpdateMode="Conditional" ChildrenAsTriggers="false">
                    <ContentTemplate>--%>
                        
                   <div class="row">
                        
                        <div class="col-lg-12 col-xl-12 form-group">
                            
                                <div class="card" id="boqsummary" runat="server">
                                        <div class="card-body">
                                            <div class="card-title">
                                                <div class="d-flex justify-content-between">
                                                    <h6 class="text-muted">
                                                        <asp:Label id="lblBOQName" CssClass="text-uppercase font-weight-bold" runat="server" Text="RA Bills Price Adjustment" />
                                                        <%--<asp:Label Text="Foo bar" CssClass="pl-1" runat="server" />--%>
                                                    </h6>
                                                    <div>
                                                        <a id="AddDependency"  runat="server" href="#"  class="showModalDependency">
                                                            <asp:Button ID="btnAddRABills" runat="server" Text="+ Add" CssClass="btn btn-primary"></asp:Button></a>
                                                      
                                                    </div>
                                                </div>
                                            </div>

                            <div class="table-responsive">
                                            <asp:GridView ID="grdRABillsMaster" runat="server" AutoGenerateColumns="False" PageSize="20" 
                                        AllowPaging="True" CssClass="table table-bordered" EmptyDataText="No Data Found." DataKeyNames="UID"
                                       Width="100%" OnRowDeleting="grdRABillsMaster_RowDeleting"                                 >
                                        <Columns>
                                            <asp:TemplateField HeaderText="RA Bill No.">
                                                <ItemTemplate>
                                                    <%--<asp:LinkButton ID="lnkview" runat="server" CommandName="view"
                                                        CommandArgument='<%#Eval("RABillUid")%>'><%#Eval("RABillNumber")%></asp:LinkButton>--%>
                                                      <a id="EditDependency"   href="#" data-href="../../_modal_pages/View-Rabill-AdjDetails.aspx?MasterUID=<%#Eval("UID")%>&RABillUID=<%#Eval("RABillUID")%>&WorkPackageUID=<%#Eval("WorkPackageUID")%>" class="showModalEditDependency">
                                                         <%#Eval("RABillNumber")%></a>
                                                    <asp:HiddenField ID="hidrabillDeleteuid" runat="server" value='<%#Eval("UID")%>' />
                                                     
                                                    </ItemTemplate>
                                                
                                                </asp:TemplateField>
                                              <asp:BoundField DataField="Description"  HeaderText="Adjustment Description" ItemStyle-HorizontalAlign="Left" HtmlEncode="False">
                            
                                             </asp:BoundField>
                                              <asp:BoundField DataField="InitialIndicesDate"  HeaderText="Initial Indices Date" ItemStyle-HorizontalAlign="Left" HtmlEncode="False" DataFormatString="{0:dd/MM/yyyy}">
                            
                                             </asp:BoundField>
                                              <asp:BoundField DataField="LatestIndicesDate"  HeaderText="Latest Indices Date" ItemStyle-HorizontalAlign="Left" HtmlEncode="False" DataFormatString="{0:dd/MM/yyyy}">
                            
                                             </asp:BoundField>
                                               <asp:BoundField DataField="RABillAmount"  HeaderText="RA Bill Amount" ItemStyle-HorizontalAlign="Right" HtmlEncode="False">
                            
                            </asp:BoundField>
                                               <asp:BoundField DataField="PriceAdjFActor"  HeaderText="Price Adjustment Factor-Pn" ItemStyle-HorizontalAlign="Right" HtmlEncode="False">
                            
                            </asp:BoundField>
                                               <asp:BoundField DataField="PriceAdjValue"  HeaderText="Price Adjustment Value" ItemStyle-HorizontalAlign="Right" HtmlEncode="False">
                            
                            </asp:BoundField>
                                               <asp:BoundField DataField="RecievedAmount"  HeaderText="Received Amount " ItemStyle-HorizontalAlign="Right" HtmlEncode="False">
                            
                            </asp:BoundField>
                                               <asp:BoundField DataField="BalanceAmount"  HeaderText="Balance Amount" ItemStyle-HorizontalAlign="Right" HtmlEncode="False">
                            
                            </asp:BoundField>
                                            
                                              
                                          
                                            <asp:TemplateField ShowHeader="False">
      
                                                <ItemTemplate>
                                                    <a class="showModalDependency" href="../../_modal_pages/add-rabill-priceadj.aspx?MasterUID=<%#Eval("UID")%>&RABillUID=<%#Eval("RABillUID")%>&WorkPackageUID=<%#Eval("WorkPackageUID")%>&type=edit"><span title="Edit" class="fas fa-edit"></span></a>
                                                    <%--<asp:LinkButton ID="lnkedit" runat="server" CausesValidation="False" CommandName="Edit" ><span title="Edit" class="fas fa-edit"></span></asp:LinkButton>--%>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField ShowHeader="False">
                                                <ItemTemplate>
                                                    <asp:LinkButton ID="lnkdelete" runat="server" CausesValidation="False" CommandName="Delete" Text="Delete" OnClientClick="return DeleteItem();"></asp:LinkButton>
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
     <%--add work package modal--%>
    <div id="ModAddDependency" class="modal it-modal fade">
	    <div class="modal-dialog modal-dialog-centered modal-dialog-scrollable modal-lg">
		    <div class="modal-content">
			    <div class="modal-header">
				   <asp:Label ID="lblheader" runat="server"> <h5 class="modal-title">Add Price Adjustment Master </h5></asp:Label>
                    <button aria-label="Close" class="close" data-dismiss="modal" type="button"><span aria-hidden="true">&times;</span></button>
			    </div>
			    <div class="modal-body">
                    <iframe class="border-0 w-100" style="height:400px;" loading="lazy"></iframe>
			    </div>
              
		    </div>
	    </div>
        </div>
    <div id="ModEditDependency" class="modal it-modal fade">
	    <div class="modal-dialog modal-dialog-centered modal-dialog-scrollable modal-xl">
		    <div class="modal-content">
			    <div class="modal-header">
				    <h5 class="modal-title">RA Bill Price Adjustment Details</h5>
                    <button aria-label="Close" class="close" data-dismiss="modal" type="button"><span aria-hidden="true">&times;</span></button>
			    </div>
			    <div class="modal-body">
                    <iframe class="border-0 w-100" style="height:450px;" loading="lazy"></iframe>
			    </div>
              
		    </div>
	    </div>
        </div>
</asp:Content>
