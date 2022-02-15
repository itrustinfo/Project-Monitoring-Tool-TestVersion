<%@ Page Title="" Language="C#" MasterPageFile="~/_master_pages/default.Master" AutoEventWireup="true" CodeBehind="default.aspx.cs"
    Inherits="ProjectManagementTool._content_pages.boqdetails_summary._default" %>
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

             $(".showPaymentBreakupModal").click(function(e) {
                e.preventDefault();
            var url = $(this).attr("href");
            $("#ModAddPaymentBreakup iframe").attr("src", url);
            $("#ModAddPaymentBreakup").modal("show");
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
            <div class="col-lg-6 col-xl-2 form-group">BOQ Details</div>
             
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
               <div class="col-md-6 col-lg-5 form-group">
                    <label class="sr-only" for="DDLWorkPackage">Work Package</label>
                    <div class="input-group">
                        <div class="input-group-prepend">
                            <span class="input-group-text">Work Package</span>
                        </div>
                        <asp:DropDownList ID="DDLWorkPackage" runat="server" CssClass="form-control" AutoPostBack="true" OnSelectedIndexChanged="DDLWorkPackage_SelectedIndexChanged"></asp:DropDownList>

                        <%--<select class="form-control" id="DDLWorkPackage" runat="server">
                        </select>--%>
                    </div>
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
                                    <h6 class="card-title text-muted text-uppercase font-weight-bold">WorkPackage Directory</h6>
                                    
                                    <div class="form-group">
                                        <label class="sr-only" for="TxtSearchDocuments">Search</label>
                                        <div class="input-group">
                                            <input id="TxtSearchDocuments" runat="server" autocomplete="off" class="form-control" type="text" placeholder="Activity name" />
                                            <div class="input-group-append">
                                                <asp:Button ID="BtnSearchDocuments" CssClass="btn btn-primary" Text="Search" runat="server" OnClientClick="showProgress()"  />
                                            </div>
                                        </div>
                                    </div>
                                   
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
                                                        <asp:Label id="lblBOQName" CssClass="text-uppercase font-weight-bold" runat="server" /><br /><br />
                                                        <asp:Label id="lblBOQDetails" CssClass="text-uppercase font-weight-bold" runat="server" Visible="false" />
                                                        <%--<asp:Label Text="Foo bar" CssClass="pl-1" runat="server" />--%>
                                                    </h6>
                                                    <div>
                                                        <a id="AddDependency"  runat="server" href="/_modal_pages/add-boqdetails.aspx" class="showModalDependency">
                                                            <asp:Button ID="btnbBOQDetails" runat="server" Text="+ Add BOQ Details" CssClass="btn btn-primary"></asp:Button></a>
                                                    </div>
                                                </div>
                                            </div>

                            <div class="table-responsive">
                                            <asp:GridView ID="GrdTreeView" runat="server" AutoGenerateColumns="false" PageSize="20" 
                                        AllowPaging="true" CssClass="table table-bordered" EmptyDataText="No Data"
                                       Width="100%" OnRowDataBound="GrdTreeView_RowDataBound" OnPageIndexChanging="GrdTreeView_PageIndexChanging"
                                                OnRowCommand="GrdTreeView_RowCommand" OnRowDeleting="GrdTreeView_RowDeleting"                                  >
                                        <Columns>
                                            <asp:TemplateField HeaderText="Item No.">
                                                <ItemTemplate>
                                                       <a id="EditDependency"   href="#" 
                                                           data-href="../../_modal_pages/show-joint-inspection.aspx?BOQUID=<%#Eval("BOQDetailsUID")%>&ProjectUID=<%#Eval("projectuid")%>&WorkpackageUID=<%#Eval("WorkPackageUID")%>" class="showModalEditDependency">
                                                        <%#Eval("Item_Number")%></a>
                                                   <%-- <asp:LinkButton ID="lnkview" runat="server" CommandName="view"
                                                        CommandArgument='<%#Eval("BOQDetailsUID")%>'><%#Eval("Item_Number")%></asp:LinkButton>--%>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                          
                                            <asp:TemplateField HeaderText="Description" ItemStyle-Width="30%">
                                               <ItemTemplate>
                                                  <%#Eval("Description")%>
                                               </ItemTemplate>
                                           </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Unit">
                                                <ItemTemplate>
                                                    <%#Eval("Unit")%>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField>
                                                  <HeaderTemplate>
                                                      Approved Quantity
                                                  </HeaderTemplate>
                                                <ItemTemplate>
                                                     <%#Eval("Quantity").ToString()=="0" ? "" : Eval("Quantity").ToString()%>
                                                    </ItemTemplate>
                                            </asp:TemplateField>
                                             <asp:TemplateField>
                                                  <HeaderTemplate>
                                                      Approved Rate
                                                  </HeaderTemplate>
                                                <ItemTemplate>
                                                    <%#Eval("Quantity").ToString()=="0" ? "" : Eval("Currency").ToString()%>&nbsp;
                                                    <%#string.Format(new System.Globalization.CultureInfo(Eval("Currency_CultureInfo").ToString()),"{0:N}", Eval("INR-Rate").ToString() == "0" || Eval("INR-Rate").ToString() == "-" || Eval("INR-Rate").ToString() == "" ? "0.00" : Eval("INR-Rate"))%>

                                                    <%--<%# Eval("INR-Rate").ToString() == "0" || Eval("INR-Rate").ToString() == "-" || Eval("INR-Rate").ToString() == "" ? "" : Eval("INR-Rate") +"<br />" %>
                                                    <%# Eval("JPY-Rate").ToString() == "0" || Eval("JPY-Rate").ToString() == "-"|| Eval("JPY-Rate").ToString() == "" ?  "" : Eval("JPY-Rate") +"<br />" %>
                                                    <%# Eval("USD-Rate").ToString() == "0" || Eval("USD-Rate").ToString() == "-"|| Eval("USD-Rate").ToString() == "" ?  "" : Eval("USD-Rate") +"<br />" %>--%>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                             <asp:TemplateField>
                                                  <HeaderTemplate>
                                                      Amount
                                                  </HeaderTemplate>
                                                <ItemTemplate>
                                                    <%#Eval("Quantity").ToString()=="0" ? "" : Eval("Currency").ToString()%>
                                                    &nbsp; 
                                                    <%#string.Format(new System.Globalization.CultureInfo(Eval("Currency_CultureInfo").ToString()),"{0:N}", Convert.ToDouble(Eval("INR-Amount").ToString() == "0" || Eval("INR-Amount").ToString() == "-" || Eval("INR-Amount").ToString() == "" ? "0.00" : Eval("INR-Amount")))%>
                                                    <%--<%# Eval("INR-Amount").ToString() == "0" || Eval("INR-Amount").ToString() == "-" || Eval("INR-Amount").ToString() == "" ? "" : Eval("INR-Amount") +"<br />" %>
                                                    <%# Eval("JPY-Amount").ToString() == "0" || Eval("JPY-Amount").ToString() == "-"|| Eval("JPY-Amount").ToString() == "" ?  "" : Eval("JPY-Amount") +"<br />" %>
                                                    <%# Eval("USD-Amount").ToString() == "0" || Eval("USD-Amount").ToString() == "-"|| Eval("USD-Amount").ToString() == "" ?  "" : Eval("USD-Amount") +"<br />" %>--%>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            
                                           <%-- <asp:TemplateField>
                                                  <HeaderTemplate>
                                                      Quantity
                                                  </HeaderTemplate>
                                                <ItemTemplate>
                                                     <%#Eval("Jointquantity")%>
                                                    </ItemTemplate>

                                            </asp:TemplateField>--%>
                                              <%--<asp:TemplateField>
                                                  <HeaderTemplate>
                                                      InvoiceDate
                                                  </HeaderTemplate>
                                                <ItemTemplate>
                                                     <%#Eval("InvoiceDate")%>
                                                    </ItemTemplate>

                                            </asp:TemplateField>--%>
                                              
                                                 <asp:TemplateField HeaderText="Payment Breakups">
                                                <ItemTemplate>
                                                    <a id="AddPaymentBreakup"   href="/_modal_pages/add-paymentbreakupterms.aspx?BOQDetailsUID=<%#Eval("BOQDetailsUID")%>&projectuid=<%#Eval("projectuid")%>" class="showPaymentBreakupModal"><%#Eval("Quantity").ToString()=="0" ? "" : "Add"%></a>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                           <asp:TemplateField>
                                        <ItemTemplate>
                                               <a id="EditDependency"  href="#" data-href="../../_modal_pages/add-boqdetails.aspx?BOQDetailsUID=<%#Eval("BOQDetailsUID")%>" class="showModalEditDependency"><span title="Edit" class="fas fa-edit"></span></a>
                                            
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                            
                                             <asp:TemplateField>
                                        <ItemTemplate>
                                       
                                              <asp:LinkButton ID="lnkdelete" runat="server" CausesValidation="false" OnClientClick="return DeleteCategoryItem()" CommandName="delete" CommandArgument='<%#Eval("BOQDetailsUID")%>'><span title="Delete" class="fas fa-trash"></span></asp:LinkButton>
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
	    <div class="modal-dialog modal-dialog-centered modal-dialog-scrollable modal-xl">
		    <div class="modal-content">
			    <div class="modal-header">
				    <h5 class="modal-title">Add BOQ Details</h5>
                    <button aria-label="Close" class="close" data-dismiss="modal" type="button"><span aria-hidden="true">&times;</span></button>
			    </div>
			    <div class="modal-body">
                    <iframe class="border-0 w-100" style="height:450px;" loading="lazy"></iframe>
			    </div>
              
		    </div>
	    </div>
        </div>
    <div id="ModEditDependency" class="modal it-modal fade">
	    <div class="modal-dialog modal-dialog-centered modal-dialog-scrollable modal-xl">
		    <div class="modal-content">
			    <div class="modal-header">
				    <h5 class="modal-title">Inspection Report</h5>
                    <button aria-label="Close" class="close" data-dismiss="modal" type="button"><span aria-hidden="true">&times;</span></button>
			    </div>
			    <div class="modal-body">
                    <iframe class="border-0 w-100" style="height:450px;" loading="lazy"></iframe>
			    </div>
              
		    </div>
	    </div>
        </div>

     <div id="ModAddPaymentBreakup" class="modal it-modal fade">
	    <div class="modal-dialog modal-dialog-centered modal-dialog-scrollable modal-lg">
		    <div class="modal-content">
			    <div class="modal-header">
				    <h5 class="modal-title">Payment Breakup</h5>
                    <button aria-label="Close" class="close" data-dismiss="modal" type="button"><span aria-hidden="true">&times;</span></button>
			    </div>
			    <div class="modal-body">
                    <iframe class="border-0 w-100" style="height:380px;" loading="lazy"></iframe>
			    </div>
              
		    </div>
	    </div>
        </div>
</asp:Content>
