<%--<%@ Page Title="" Language="C#" MasterPageFile="~/_master_pages/default.Master" AutoEventWireup="true" CodeBehind="default.aspx.cs" 
    Inherits="ProjectManagementTool._content_pages.invoice_abstract._default" %>--%>
<%@ Page Title="" Language="C#" MasterPageFile="~/_master_pages/default.Master" AutoEventWireup="true" CodeBehind="default.aspx.cs"
    Inherits="ProjectManagementTool._content_pages.invoice_abstract._default" %>
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

        }
        $(document).ready(function () {
            BindEvents();
            //$('#loader').fadeOut();
        });
</script>
    <script type="text/javascript">
        function DeleteItem() {
            if (confirm("All data associated with this project will be deleted. Are you sure you want to delete ...?")) {
                return true;
            }
            return false;
        }
        function DeleteCategoryItem() {
            if (confirm("All data associated with this BOQ will be deleted. Are you sure you want to delete ...?")) {
                return true;
            }
            return false;
        }
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="default_master_body" runat="server">

    <asp:ScriptManager ID="ScriptManager1" runat="server">
   </asp:ScriptManager>
   
    <%--<div id="loader"></div>--%>



    <div class="container-fluid">
        <div class="row">
            <div class="col-lg-6 col-xl-8 form-group">Invoice Abstract</div>
            <div class="col-lg-6 col-xl-4 form-group">
            </div>
        </div>
    </div>
    <div class="container-fluid">
       <%-- <asp:UpdatePanel ID="UpdatePanel2" runat="server" UpdateMode="Conditional" ChildrenAsTriggers="false">
                    <ContentTemplate>--%>
                        
                   <div class="row">
                        <div class="col-lg-6 col-xl-4 form-group">
                            <div class="card h-100" style="max-height:700px; overflow-y:auto;">
                                <div class="card-body">
                               <%--     <h6 class="card-title text-muted text-uppercase font-weight-bold">WorkPackage Directory</h6>
                                    
                                    <div class="form-group">
                                        <label class="sr-only" for="TxtSearchDocuments">Search</label>
                                        <div class="input-group">
                                            <input id="TxtSearchDocuments" runat="server" autocomplete="off" class="form-control" type="text" placeholder="Activity name" />
                                            <div class="input-group-append">
                                                <asp:Button ID="BtnSearchDocuments" CssClass="btn btn-primary" Text="Search" runat="server" OnClientClick="showProgress()"  />
                                            </div>
                                        </div>
                                    </div>--%>
                                   
                                       <asp:TreeView runat="server" CssClass="it_tree_view" ID="TreeView1" ImageSet="XPFileExplorer"
                                           NodeIndent="15" EnableTheming="True" NodeWrap="True" OnSelectedNodeChanged="TreeView1_SelectedNodeChanged">                               
                                        <NodeStyle CssClass="it_tree_view__node" NodeSpacing="2px" />
                                        <ParentNodeStyle Font-Bold="False" />
                                        <SelectedNodeStyle CssClass="it_tree_view__node__selected" HorizontalPadding="4px" VerticalPadding="2px" />
                                    </asp:TreeView>    
                                       
                                    
                                </div>
                            </div>
                        </div>
                        <div class="col-lg-6 col-xl-8 form-group">
                            
                                <div class="card" id="boqsummary" runat="server">
                                        <div class="card-body">
                                            <div class="card-title">
                                                <div class="d-flex justify-content-between">
                                                    <h6 class="text-muted">
                                                        <asp:Label id="lblBOQName" CssClass="text-uppercase font-weight-bold" runat="server" />
                                                        <%--<asp:Label Text="Foo bar" CssClass="pl-1" runat="server" />--%>
                                                    </h6>
                                                    <div>
                                                        <a id="AddDependency"  runat="server"  class="showModalDependency">
                                                            <asp:Button ID="btnAddRABills" runat="server" Text="+ Add RA Bills" CssClass="btn btn-primary"></asp:Button></a>
                                                      
                                                    </div>
                                                </div>
                                            </div>
                                            <div class="table-responsive" id="InvoiceDetails" runat="server">
                                                <table class="table table-borderless">
                                                    <tr>
                                                        <td>
                                                            <asp:Label id="LblGrossAmount" Text="Gross Amount" CssClass="text-uppercase font-weight-bold" runat="server" />
                                                            
                                                        </td>
                                                        <td><b>:</b></td>
                                                        <td >
                                                            <asp:Label id="LblInvoiceGrossAmount" CssClass="text-uppercase font-weight-bold" runat="server" />
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td>
                                                            <asp:Label id="LblDeductions" Text="Deductions" CssClass="text-uppercase font-weight-bold" runat="server" />
                                                            
                                                        </td>
                                                        <td><b>:</b></td>
                                                        <td>
                                                            <asp:GridView ID="GrdDeductions" GridLines="None" runat="server" ShowFooter="true" ShowHeader="false" AllowPaging="false" AutoGenerateColumns="false" OnRowDataBound="GrdDeductions_RowDataBound">
                                                                <Columns>
                                                                    <asp:TemplateField>
                                                                        <ItemTemplate>
                                                                            <%#GetDeductionName(Eval("DeductionUId").ToString())%>
                                                                        </ItemTemplate>
                                                                        <FooterTemplate>
                                                                            <b>Total</b>
                                                                        </FooterTemplate>
                                                                    </asp:TemplateField>
                                                                    <asp:BoundField DataField="amount" />
                                                                    <asp:TemplateField FooterStyle-HorizontalAlign="Left">
                                                                        <FooterTemplate>
                                                                            <asp:Label ID="lbltotal" runat="server"></asp:Label>
                                                                        </FooterTemplate>
                                                                    </asp:TemplateField>
                                                                </Columns>
                                                            </asp:GridView>
                                                            
                                                        </td>
                                                        <td>&nbsp;</td>
                                                        <td>&nbsp;</td>
                                                    </tr>
                                                    <tr>
                                                        <td>
                                                            <asp:Label id="LblNet" Text="Net Amount" CssClass="text-uppercase font-weight-bold" runat="server" />
                                                        </td>
                                                        <td><b>:</b></td>
                                                        <td >
                                                            <asp:Label id="LblNetAmount" CssClass="text-uppercase font-weight-bold" runat="server" />
                                                        </td>
                                                    </tr>
                                                </table>
                                                </div>
                            <div class="table-responsive">

                                            <asp:GridView ID="grdRABillItems" runat="server" AutoGenerateColumns="false" PageSize="20" 
                                        AllowPaging="true" CssClass="table table-bordered" EmptyDataText="No Data"
                                       Width="100%"                                  >
                                        <Columns>
                                            <asp:TemplateField HeaderText="RA Bill No.">
                                                <ItemTemplate>
                                                    <%--<asp:LinkButton ID="lnkview" runat="server" CommandName="view"
                                                        CommandArgument='<%#Eval("RABillUid")%>'><%#Eval("RABillNumber")%></asp:LinkButton>--%>
                                                      <a id="EditDependency"   href="#" data-href="../../_modal_pages/show-RABills.aspx?RABillUid=<%#Eval("RABillUid")%> " class="showModalEditDependency">
                                                         <%#Eval("RABillNumber")%></a>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                          
                                            <asp:TemplateField HeaderText="Project Name" ItemStyle-Width="40%">
                                               <ItemTemplate>
                                                  <%#Eval("ProjectName")%>
                                               </ItemTemplate>
                                           </asp:TemplateField>
                                         
                                             <asp:TemplateField>
                                                  <HeaderTemplate>
                                                      Bill Value
                                                  </HeaderTemplate>
                                                <ItemTemplate>
                                                    <%#Eval("Bill_Value")%>
                                                  
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                             <asp:TemplateField>
                                                   <HeaderTemplate>
                                                     Net Amount
                                                  </HeaderTemplate>
                                                <ItemTemplate>
                                                   <%#Eval("Net_Amount")%>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                              <asp:TemplateField >
                                                  <HeaderTemplate>
                                                    Recoveries
                                                  </HeaderTemplate>
                                                <ItemTemplate>
                                                   <%#Eval("Recoveries")%>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                          
                                        </Columns>
                                        <EmptyDataTemplate>
                                                    <strong>No Records Found ! </strong>
                                                </EmptyDataTemplate>
                                        </asp:GridView>
                                            <asp:GridView ID="grdInvoiceDeatils" runat="server" AutoGenerateColumns="False" PageSize="20" 
                                        AllowPaging="True" CssClass="table table-bordered" EmptyDataText="No Data"
                                       Width="100%"                                 >
                                        <Columns>
                                            <asp:TemplateField HeaderText="RA Bill No.">
                                                <ItemTemplate>
                                                    <%--<asp:LinkButton ID="lnkview" runat="server" CommandName="view"
                                                        CommandArgument='<%#Eval("RABillUid")%>'><%#Eval("RABillNumber")%></asp:LinkButton>--%>
                                                    <%#Eval("RABillNumber")%>
                                                      <a id="EditDependency"   href="#" data-href="../../_modal_pages/show-RABills.aspx?RABillUid=<%#Eval("RABillUid")%> &type=add" class="showModalEditDependency">
                                                         </a>
                                                     <asp:HiddenField ID="hidrabillDeleteuid" runat="server" value='<%#Eval("RABillUid")%>' />
                                                    </ItemTemplate>
                                                  <EditItemTemplate>
                                                      <asp:HiddenField ID="hidrabilluid" runat="server" value='<%#Eval("RABillUid")%>' />
                                                     <asp:TextBox ID="txtRabill" runat="server" Text='<%#Eval("RABillNumber")%>'></asp:TextBox>
                                                 </EditItemTemplate>
                                                </asp:TemplateField>
                                          
                                           
                                         
                                             <asp:TemplateField>
                                                  <HeaderTemplate>
                                                      Bill Value
                                                  </HeaderTemplate>
                                                <ItemTemplate>
                                                                    <%#Eval("Bill_Value")%>
                                                  
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                             <asp:TemplateField>
                                                   <HeaderTemplate>
                                                   Created Date
                                                  </HeaderTemplate>
                                                <ItemTemplate>
                                                   <%#Eval("CreatedDate")%>
                                                </ItemTemplate>
                                               
                                            </asp:TemplateField>
                                              
                                          
                                           <%-- <asp:TemplateField ShowHeader="False">
                                                <EditItemTemplate>
                                                    <asp:LinkButton ID="lnkupdate" runat="server" CausesValidation="True" CommandName="Update" Text="Update"></asp:LinkButton>
                                                    &nbsp;<asp:LinkButton ID="lnkcancel" runat="server" CausesValidation="False" CommandName="Cancel" Text="Cancel"></asp:LinkButton>
                                                </EditItemTemplate>
                                                <ItemTemplate>
                                                    <asp:LinkButton ID="lnkedit" runat="server" CausesValidation="False" CommandName="Edit" ><span title="Edit" class="fas fa-edit"></span></asp:LinkButton>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField ShowHeader="False">
                                                <ItemTemplate>
                                                    <asp:LinkButton ID="lnkdelete" runat="server" CausesValidation="False" CommandName="Delete" Text="Delete"></asp:LinkButton>
                                                </ItemTemplate>
                                            </asp:TemplateField>--%>
                                              
                                          
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
				   <asp:Label ID="lblheader" runat="server"> <h5 class="modal-title">Add RA Bill </h5></asp:Label>
                    <button aria-label="Close" class="close" data-dismiss="modal" type="button"><span aria-hidden="true">&times;</span></button>
			    </div>
			    <div class="modal-body">
                    <iframe class="border-0 w-100" style="height:450px;" loading="lazy"></iframe>
			    </div>
              
		    </div>
	    </div>
        </div>
    <div id="ModEditDependency" class="modal it-modal fade">
	    <div class="modal-dialog modal-dialog-centered modal-dialog-scrollable modal-lg">
		    <div class="modal-content">
			    <div class="modal-header">
				    <h5 class="modal-title">RA Bill Number</h5>
                    <button aria-label="Close" class="close" data-dismiss="modal" type="button"><span aria-hidden="true">&times;</span></button>
			    </div>
			    <div class="modal-body">
                    <iframe class="border-0 w-100" style="height:450px;" loading="lazy"></iframe>
			    </div>
              
		    </div>
	    </div>
        </div>
</asp:Content>
