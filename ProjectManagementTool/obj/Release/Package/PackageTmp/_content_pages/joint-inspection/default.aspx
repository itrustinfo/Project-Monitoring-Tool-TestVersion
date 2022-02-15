<%@ Page Title="" Language="C#" MasterPageFile="~/_master_pages/default.Master" AutoEventWireup="true" CodeBehind="default.aspx.cs" Inherits="ProjectManagementTool._content_pages.joint_inspection._default" %>
<asp:Content ID="Content1" ContentPlaceHolderID="default_master_head" runat="server">
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

            $(".showAddJointInspectionModal").click(function(e) {
        e.preventDefault();
        var url = $(this).attr("href");
        $("#ModAddJointInspection iframe").attr("src", url);
        $("#ModAddJointInspection").modal("show");
          });

         $(".showEditJointInscptionModal").click(function(e) {
        e.preventDefault();
        var url = $(this).attr("href");
        $("#ModEditJointInspection iframe").attr("src", url);
        $("#ModEditJointInspection").modal("show");
            });
        }
        $(document).ready(function () {
            BindEvents();
        });
</script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="default_master_body" runat="server">
    <div class="container-fluid">
            <div class="row">
                <div class="col-md-12 col-lg-4 form-group">Dashboard</div>
                <div class="col-md-6 col-lg-4 form-group">
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
        <div class="row">
            <div class="col-md-12 col-lg-12 form-group">
                <div class="card mb-4">
                    <div class="card-body">
                        <div class="card-title">
                        <div class="d-flex justify-content-between">
                              <h6 class="text-muted">
                                   <asp:Label ID="Label5" CssClass="text-uppercase font-weight-bold" runat="server" Text="Joint Inspection"  />
                               </h6>
                                <div>
                                   <a id="AddJointInspection" runat="server" href="/_modal_pages/add-jointinspection.aspx?type=Add" class="showAddJointInspectionModal"><asp:Button ID="btnAddClass" runat="server" Text="+ Add Joint Inspection" CssClass="btn btn-primary"></asp:Button></a>
                                </div>
                          </div>
                            </div>
                        <div class="table-responsive">
                            <asp:GridView ID="GrdJointInspection" runat="server" CssClass="table table-bordered" AutoGenerateColumns="False" Width="100%" PageSize="10" OnPageIndexChanging="GrdJointInspection_PageIndexChanging" OnRowCommand="GrdJointInspection_RowCommand" OnRowDeleting="GrdJointInspection_RowDeleting">
                               <Columns>  
                                   <asp:TemplateField HeaderText="BOQ Description">
                                      <ItemTemplate>
                                              <%#GetBOQDesc(Eval("BOQUid").ToString())%>
                                      </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Dia of Pipe">
                                      <ItemTemplate>
                                              <%#Eval("DiaPipe")%>
                                      </ItemTemplate>
                                    </asp:TemplateField> 
                                   <asp:TemplateField HeaderText="Invoice Number">
                                      <ItemTemplate>
                                              <%#Eval("invoice_number")%>
                                      </ItemTemplate>
                                    </asp:TemplateField>  
                                   <asp:TemplateField HeaderText="Invoice Date">
                                      <ItemTemplate>
                                              <%#Eval("invoicedate","{0:dd-MMM-yyyy}")%>
                                      </ItemTemplate>
                                    </asp:TemplateField>
                                   <asp:TemplateField HeaderText="Unit">
                                      <ItemTemplate>
                                              <%#Eval("unit")%>
                                      </ItemTemplate>
                                    </asp:TemplateField>  
                                   <asp:TemplateField HeaderText="Quantity">
                                      <ItemTemplate>
                                              <%#Eval("quantity")%>
                                      </ItemTemplate>
                                    </asp:TemplateField>
                                   <asp:TemplateField>
                                      <ItemTemplate>
                                          <a href='/_modal_pages/add-jointinspection.aspx?type=edit&inspectionUid=<%#Eval("inspectionUid")%>&ProjectUID=<%#Eval("ProjectUID")%>&WorkpackgeUID=<%#Eval("WorkpackgeUID")%>' class="showEditJointInscptionModal"><span title="Edit" class="fas fa-edit"></span></a>
                                              <%--<asp:LinkButton ID="lnkedit" runat="server" CausesValidation="false" CommandName="edit" CommandArgument='<%#Eval("MasterWorkPackageUID")%>'><span title="Edit" class="fas fa-edit"></span></asp:LinkButton>--%>
                                      </ItemTemplate>
                                    </asp:TemplateField>
                                   <asp:TemplateField>
                                      <ItemTemplate>
                                              <asp:LinkButton ID="lnkDelete" runat="server" CausesValidation="false" OnClientClick="return DeleteItem()" CommandName="delete" CommandArgument='<%#Eval("inspectionUid")%>'><span title="Delete" class="fas fa-trash"></span></asp:LinkButton>
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
    <%--add Add Joint Inspection modal--%>
    <div id="ModAddJointInspection" class="modal it-modal fade">
	    <div class="modal-dialog modal-dialog-centered modal-dialog-scrollable modal-xl">
		    <div class="modal-content">
			    <div class="modal-header">
				    <h5 class="modal-title">Add Joint Inspection</h5>
                    <button aria-label="Close" class="close" data-dismiss="modal" type="button"><span aria-hidden="true">&times;</span></button>
			    </div>
			    <div class="modal-body">
                    <iframe class="border-0 w-100" style="height: 450px;" loading="lazy"></iframe>
			    </div>
              
		    </div>
	    </div>
    </div>

    <%--add Edit Joint Inspection modal--%>
    <div id="ModEditJointInspection" class="modal it-modal fade">
	    <div class="modal-dialog modal-dialog-centered modal-dialog-scrollable modal-xl">
		    <div class="modal-content">
			    <div class="modal-header">
				    <h5 class="modal-title">Edit Joint Inspection</h5>
                    <button aria-label="Close" class="close" data-dismiss="modal" type="button"><span aria-hidden="true">&times;</span></button>
			    </div>
			    <div class="modal-body">
                    <iframe class="border-0 w-100" style="height: 450px;" loading="lazy"></iframe>
			    </div>
              
		    </div>
	    </div>
    </div>
</asp:Content>
