<%@ Page Title="" Language="C#" MasterPageFile="~/_master_pages/default.Master" AutoEventWireup="true" CodeBehind="default.aspx.cs" Inherits="ProjectManagementTool._content_pages.projects._default" %>
<asp:Content ID="Content1" ContentPlaceHolderID="default_master_head" runat="server">
     <script type="text/javascript">
        function DeleteItem() {
            if (confirm("All data associated with this project will be deleted. Are you sure you want to delete ...?")) {
                return true;
            }
            return false;
         }
         function DeleteCategoryItem() {
            if (confirm("All data associated with this project category will be deleted. Are you sure you want to delete ...?")) {
                return true;
            }
            return false;
    }
    </script>

    <script type="text/javascript">
        function BindEvents() {

            $(".showAddProjectModal").click(function(e) {
        e.preventDefault();
        var url = $(this).attr("href");
        $("#ModAddProject iframe").attr("src", url);
        $("#ModAddProject").modal("show");
            });

            
            $(".showAddProjectMasterModal").click(function(e) {
        e.preventDefault();
        var url = $(this).attr("href");
        $("#ModAddProjectMaster iframe").attr("src", url);
        $("#ModAddProjectMaster").modal("show");
          });

         $(".showEditProjectModal").click(function(e) {
        e.preventDefault();
        var url = $(this).attr("href");
        $("#ModEditProject iframe").attr("src", url);
        $("#ModEditProject").modal("show");
            });

             $(".showAddProjectClassModal").click(function(e) {
        e.preventDefault();
        var url = $(this).attr("href");
        $("#ModAddProjectClass iframe").attr("src", url);
        $("#ModAddProjectClass").modal("show");
            });

            $(".showEditProjectClassModal").click(function(e) {
        e.preventDefault();
        var url = $(this).attr("href");
        $("#ModEditProjectClass iframe").attr("src", url);
        $("#ModEditProjectClass").modal("show");
            });
        }
        $(document).ready(function () {
            BindEvents();
            //$('#loader').fadeOut();
        });
</script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="default_master_body" runat="server">
    <%--<div id="loader"></div>--%>
    <div class="container-fluid">
        <div class="row">
            <div class="col-md-12 col-lg-12 form-group">
                <div class="card mb-4">
                    <div class="card-body">
                        <div class="card-title">
                        <div class="d-flex justify-content-between">
                              <h6 class="text-muted">
                                   <asp:Label ID="Label5" CssClass="text-uppercase font-weight-bold" runat="server" Text="List of Project Category"  />
                               </h6>
                                <div>
                                   <a href="/_modal_pages/add-projectclass.aspx?type=Add" class="showAddProjectClassModal"><asp:Button ID="btnAddClass" runat="server" Text="+ Add Project Category" CssClass="btn btn-primary"></asp:Button></a>
                                </div>
                          </div>
                            </div>
                        <div class="table-responsive">
                            <asp:GridView ID="GrdProjectClass" runat="server" CssClass="table table-bordered" AutoGenerateColumns="False" Width="100%" PageSize="10" OnPageIndexChanging="GrdProjectClass_PageIndexChanging" OnRowCommand="GrdProjectClass_RowCommand" OnRowDeleting="GrdProjectClass_RowDeleting">
                               <Columns>             
                                    <asp:TemplateField HeaderText="Project Category">
                                      <ItemTemplate>
                                              <%#Eval("ProjectClass_Name")%>
                                      </ItemTemplate>
                                    </asp:TemplateField> <asp:TemplateField HeaderText="Description">
                                      <ItemTemplate>
                                              <%#Eval("ProjectClass_Description")%>
                                      </ItemTemplate>
                                    </asp:TemplateField>  
                                   <asp:TemplateField>
                                      <ItemTemplate>
                                          <a href='/_modal_pages/add-projectclass.aspx?type=edit&pClassUID=<%#Eval("ProjectClass_UID")%>' class="showEditProjectClassModal"><span title="Edit" class="fas fa-edit"></span></a>
                                              <%--<asp:LinkButton ID="lnkedit" runat="server" CausesValidation="false" CommandName="edit" CommandArgument='<%#Eval("MasterWorkPackageUID")%>'><span title="Edit" class="fas fa-edit"></span></asp:LinkButton>--%>
                                      </ItemTemplate>
                                    </asp:TemplateField>
                                   <asp:TemplateField>
                                      <ItemTemplate>
                                              <asp:LinkButton ID="lnkcategorydelete" runat="server" CausesValidation="false" OnClientClick="return DeleteCategoryItem()" CommandName="delete" CommandArgument='<%#Eval("ProjectClass_UID")%>'><span title="Delete" class="fas fa-trash"></span></asp:LinkButton>
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

    <div class="container-fluid">
        <div class="row">
            <div class="col-lg-12 col-xl-12 form-group">
                <div class="card mb-4">
                    <div class="card-body">
                        <div class="card-title">
                        <div class="d-flex justify-content-between">
                              <h6 class="text-muted">
                                   <asp:Label ID="Label2" CssClass="text-uppercase font-weight-bold" runat="server" Text="List of Projects"  />
                               </h6>
                                <div>
                                   <a href="/_modal_pages/add-project.aspx?type=Add" class="showAddProjectModal"><asp:Button ID="btnaddproject" runat="server" Text="+ Add Project" CssClass="btn btn-primary"></asp:Button></a>&nbsp;
                                   <%-- <a href="/_modal_pages/add-prjmaster-mail-settings.aspx" class="showAddProjectMasterModal"><asp:Button ID="btnaddmail" runat="server" Text="Update Project Master Data Settings" CssClass="btn btn-primary"></asp:Button></a>--%>
                                </div>
                          </div>
                            </div>
                        <div class="table-responsive">
                            <asp:GridView ID="GrdProject" runat="server" CssClass="table table-bordered" AutoGenerateColumns="False" Width="100%" PageSize="10" OnPageIndexChanging="GrdProject_PageIndexChanging" OnRowCommand="GrdProject_RowCommand" OnRowDeleting="GrdProject_RowDeleting" OnRowDataBound="GrdProject_RowDataBound" >
                               <Columns>             
                                    <asp:BoundField DataField="ProjectName" HeaderText="Project Name" DataFormatString="{0:dd MMM yyyy}">
                                  <HeaderStyle HorizontalAlign="Left" />
                              </asp:BoundField>                                
                                   <asp:TemplateField HeaderText="Project Code">
                                      <ItemTemplate>
                                              <%#Eval("ProjectAbbrevation")%>
                                      </ItemTemplate>
                                    </asp:TemplateField> 
                                   <asp:TemplateField HeaderText="Funding Agency">
                                      <ItemTemplate>
                                              <%#Eval("Funding_Agency")%>
                                      </ItemTemplate>
                                    </asp:TemplateField> 
                                   <asp:TemplateField HeaderText="Budget (INR in crores)">
                                  <ItemTemplate>
                                      <span style="color:#006699;"><%#Eval("Currency")%></span>&nbsp;<%#string.Format(new System.Globalization.CultureInfo(Eval("Currency_CultureInfo").ToString()),"{0:N}", Eval("Budget"))%>
                                  </ItemTemplate>
                              </asp:TemplateField>
                              <asp:TemplateField HeaderText="ActualExpenditure (INR in crores)">
                                  <ItemTemplate>
                                      <span style="color:#006699;"><%#Eval("Currency").ToString()%></span>&nbsp;<%#string.Format(new System.Globalization.CultureInfo(Eval("Currency_CultureInfo").ToString()),"{0:N}", Eval("ActualExpenditure"))%>
                                  </ItemTemplate>
                              </asp:TemplateField>
                                   <asp:BoundField DataField="StartDate" HeaderText="Start Date" DataFormatString="{0:dd MMM yyyy}">
                                  <HeaderStyle HorizontalAlign="Left" />
                              </asp:BoundField>
                              <asp:BoundField DataField="PlannedEndDate" HeaderText="Planned EndDate" DataFormatString="{0:dd MMM yyyy}">
                                  <HeaderStyle HorizontalAlign="Left" />
                              </asp:BoundField>
                                     <asp:BoundField DataField="ProjectedEndDate" HeaderText="Projected EndDate" DataFormatString="{0:dd MMM yyyy}">
                                  <HeaderStyle HorizontalAlign="Left" />
                              </asp:BoundField>
                                   <asp:TemplateField>
                                      <ItemTemplate>
                                          <a href='/_modal_pages/add-project.aspx?type=edit&ProjectUID=<%#Eval("ProjectUID")%>' class="showEditProjectModal"><span title="Edit" class="fas fa-edit"></span></a>
                                              <%--<asp:LinkButton ID="lnkedit" runat="server" CausesValidation="false" CommandName="edit" CommandArgument='<%#Eval("MasterWorkPackageUID")%>'><span title="Edit" class="fas fa-edit"></span></asp:LinkButton>--%>
                                      </ItemTemplate>
                                    </asp:TemplateField>
                                   <asp:TemplateField>
                                        <ItemTemplate>
                                             <asp:LinkButton ID="lnkdelete" runat="server" OnClientClick="return DeleteItem()" CausesValidation="false" CommandArgument='<%#Eval("ProjectUID")%>' CommandName="delete"><span title="Delete" class="fas fa-trash"></span></asp:LinkButton>
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

    <%--add Add Project modal--%>
    <div id="ModAddProject" class="modal it-modal fade">
	    <div class="modal-dialog modal-dialog-centered modal-dialog-scrollable modal-xl">
		    <div class="modal-content">
			    <div class="modal-header">
				    <h5 class="modal-title">Add Project</h5>
                    <button aria-label="Close" class="close" data-dismiss="modal" type="button"><span aria-hidden="true">&times;</span></button>
			    </div>
			    <div class="modal-body">
                    <iframe class="border-0 w-100" style="height: 450px;" loading="lazy"></iframe>
			    </div>
              
		    </div>
	    </div>
    </div>

    <%--add Edit Prject modal--%>
    <div id="ModEditProject" class="modal it-modal fade">
	    <div class="modal-dialog modal-dialog-centered modal-dialog-scrollable modal-xl">
		    <div class="modal-content">
			    <div class="modal-header">
				    <h5 class="modal-title">Edit Project</h5>
                    <button aria-label="Close" class="close" data-dismiss="modal" type="button"><span aria-hidden="true">&times;</span></button>
			    </div>
			    <div class="modal-body">
                    <iframe class="border-0 w-100" style="height: 450px;" loading="lazy"></iframe>
			    </div>
              
		    </div>
	    </div>
    </div>

    <%--add Project Class modal--%>
    <div id="ModAddProjectClass" class="modal it-modal fade">
	    <div class="modal-dialog modal-dialog-centered modal-dialog-scrollable modal-lg">
		    <div class="modal-content">
			    <div class="modal-header">
				    <h5 class="modal-title">Add Project Category</h5>
                    <button aria-label="Close" class="close" data-dismiss="modal" type="button"><span aria-hidden="true">&times;</span></button>
			    </div>
			    <div class="modal-body">
                    <iframe class="border-0 w-100" style="height:300px;" loading="lazy"></iframe>
			    </div>
              
		    </div>
	    </div>
    </div>

    <%--add Edit Project Class modal--%>
    <div id="ModEditProjectClass" class="modal it-modal fade">
	    <div class="modal-dialog modal-dialog-centered modal-dialog-scrollable modal-lg">
		    <div class="modal-content">
			    <div class="modal-header">
				    <h5 class="modal-title">Edit Project Category</h5>
                    <button aria-label="Close" class="close" data-dismiss="modal" type="button"><span aria-hidden="true">&times;</span></button>
			    </div>
			    <div class="modal-body">
                    <iframe class="border-0 w-100" style="height:300px;" loading="lazy"></iframe>
			    </div>
              
		    </div>
	    </div>
    </div>

     <%--add Project Class modal--%>
    <div id="ModAddProjectMaster" class="modal it-modal fade">
	    <div class="modal-dialog modal-dialog-centered modal-dialog-scrollable modal-lg">
		    <div class="modal-content">
			    <div class="modal-header">
				    <h5 class="modal-title">Update Project Master Data Settings</h5>
                    <button aria-label="Close" class="close" data-dismiss="modal" type="button"><span aria-hidden="true">&times;</span></button>
			    </div>
			    <div class="modal-body">
                    <iframe class="border-0 w-100" style="height:400px;" loading="lazy"></iframe>
			    </div>
              
		    </div>
	    </div>
    </div>
</asp:Content>
