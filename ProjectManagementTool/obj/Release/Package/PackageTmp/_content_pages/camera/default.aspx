<%@ Page Title="" Language="C#" MasterPageFile="~/_master_pages/default.Master" AutoEventWireup="true" CodeBehind="default.aspx.cs" Inherits="ProjectManagementTool._content_pages.camera._default" %>
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
       
        $(".showAddCamera").click(function(e) {
        e.preventDefault();
        var url = $(this).attr("href");
        $("#ModAddCamera iframe").attr("src", url);
        $("#ModAddCamera").modal("show");
          });

         $(".showEditCamera").click(function(e) {
        e.preventDefault();
        var url = $(this).attr("href");
        $("#ModEditCamera iframe").attr("src", url);
        $("#ModEditCamera").modal("show");
            });
        }
        $(document).ready(function () {
            BindEvents();
        });
</script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="default_master_body" runat="server">
    <div class="container-fluid">
        <asp:HiddenField ID="HiddenWorkpackageUID" runat="server" />
            <div class="row">
                <div class="col-md-12 col-lg-4 form-group">Dashboard</div>
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
        <div class="row">
            <div class="col-lg-12 col-xl-12 form-group">
                <div class="card mb-4">
                    <div class="card-body">
                        <div class="card-title">
                        <div class="d-flex justify-content-between">
                              <h6 class="text-muted">
                                   <asp:Label ID="lblcamera" CssClass="text-uppercase font-weight-bold" runat="server" Text="List of Cameras"  />
                               </h6>
                                <div>
                                   <a id="AddCamera" runat="server" href="/_modal_pages/add-camera.aspx" class="showAddCamera"><asp:Button ID="btnaddcamera" runat="server" Text="+ Add Camera" CssClass="btn btn-primary"></asp:Button></a>
                                </div>
                          </div>
                            </div>
                        <div class="table-responsive">
                            <asp:GridView ID="GrdCamera" runat="server" CssClass="table table-bordered" AutoGenerateColumns="False" Width="100%" PageSize="10" OnPageIndexChanging="GrdCamera_PageIndexChanging" OnRowCommand="GrdCamera_RowCommand" OnRowDeleting="GrdCamera_RowDeleting">
                               <Columns>             
                                    <asp:TemplateField HeaderText="Camera Name">
                                      <ItemTemplate>
                                              <%#Eval("Camera_Name")%>
                                      </ItemTemplate>
                                    </asp:TemplateField> 
                                   <asp:TemplateField HeaderText="IP Address">
                                      <ItemTemplate>
                                              <%#Eval("Camera_IPAddress")%>
                                      </ItemTemplate>
                                    </asp:TemplateField> 
                                   <asp:TemplateField HeaderText="Description">
                                      <ItemTemplate>
                                              <%#Eval("Camera_Description")%>
                                      </ItemTemplate>
                                    </asp:TemplateField>  
                                   <asp:TemplateField>
                                      <ItemTemplate>
                                              <a href='/_modal_pages/add-camera.aspx?Camera_UID=<%#Eval("Camera_UID")%>&WorkUID=<%#Eval("WorkpackageUID")%>&PrjUID=<%#Eval("ProjectUID")%>' class="showEditCamera"><span title="Edit" class="fas fa-edit"></span></a>
                                      </ItemTemplate>
                                    </asp:TemplateField>
                                   <asp:TemplateField>
                                      <ItemTemplate>
                                              <asp:LinkButton ID="lnkdelete" runat="server" CausesValidation="false" OnClientClick="return DeleteItem()" CommandName="delete" CommandArgument='<%#Eval("Camera_UID")%>'><span title="Delete" class="fas fa-trash"></span></asp:LinkButton>
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
     <%--add Add Camera modal--%>
    <div id="ModAddCamera" class="modal it-modal fade">
	    <div class="modal-dialog modal-dialog-centered modal-dialog-scrollable modal-lg">
		    <div class="modal-content">
			    <div class="modal-header">
				    <h5 class="modal-title">Add Camera</h5>
                    <button aria-label="Close" class="close" data-dismiss="modal" type="button"><span aria-hidden="true">&times;</span></button>
			    </div>
			    <div class="modal-body">
                    <iframe class="border-0 w-100" style="height: 450px;" loading="lazy"></iframe>
			    </div>
              
		    </div>
	    </div>
    </div>

    <%--add Edit Camera modal--%>
    <div id="ModEditCamera" class="modal it-modal fade">
	    <div class="modal-dialog modal-dialog-centered modal-dialog-scrollable modal-lg">
		    <div class="modal-content">
			    <div class="modal-header">
				    <h5 class="modal-title">Edit Camera</h5>
                    <button aria-label="Close" class="close" data-dismiss="modal" type="button"><span aria-hidden="true">&times;</span></button>
			    </div>
			    <div class="modal-body">
                    <iframe class="border-0 w-100" style="height: 450px;" loading="lazy"></iframe>
			    </div>
              
		    </div>
	    </div>
    </div>
</asp:Content>
