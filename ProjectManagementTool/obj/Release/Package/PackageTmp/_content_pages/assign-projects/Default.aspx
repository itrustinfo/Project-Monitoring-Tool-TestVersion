<%@ Page Title="" Language="C#" MasterPageFile="~/_master_pages/default.Master" AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="ProjectManager._content_pages.assign_projects.Default" %>
<asp:Content ID="Content1" ContentPlaceHolderID="default_master_head" runat="server">
    <script>
        $(document).ready(function () {
          $(".showAssignProjectModal").click(function(e) {
            e.preventDefault();
            var url = $(this).attr("href");
            $("#ModAssignProject iframe").attr("src", url);
            $("#ModAssignProject").modal("show");
            });

             $(".showEditAssignProjectModal").click(function(e) {
            e.preventDefault();
            var url = $(this).attr("href");
            $("#ModEditAssignProject iframe").attr("src", url);
            $("#ModEditAssignProject").modal("show");
            });

});
</script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="default_master_body" runat="server">
    <div class="container-fluid">
        <div class="row">
            <div class="col-lg-6 col-xl-8 form-group">Assign Projects to User</div>
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
                                    <asp:Label ID="ActivityHeading" runat="server"  Text="List of Assigned Projects" CssClass="text-uppercase font-weight-bold" />
                                    <%--<asp:Label Text="Foo bar" CssClass="pl-1" runat="server" />--%>
                                </h6>
                                <div>
                                    <a id="HLAdd" runat="server" href="/_modal_pages/assign-projects.aspx" class="showAssignProjectModal"><asp:Button ID="btnassign" runat="server" Text="+ Assign Projects" align="end" CssClass="btn btn-primary"></asp:Button></a>
                                   
                                </div>
                            </div>
                        </div>

                        <div class="table-responsive">
                                <asp:GridView ID="GrdProjects" runat="server" AutoGenerateColumns="False" EmptyDataText="No Data" Width="100%" PageSize="15" CssClass="table table-bordered" OnPageIndexChanging="GrdProjects_PageIndexChanging">
                        <Columns>
                            <asp:TemplateField HeaderText="User Name">
                                <ItemTemplate>
                                    <%#GetUserName(Eval("UserUID").ToString())%>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Project Name">
                                <ItemTemplate>
                                    <%#GetProjectName(Eval("ProjectUID").ToString())%>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="User Role">
                                <ItemTemplate>
                                    <%#getUserType(Eval("UserRole").ToString())%>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField>
                                <ItemTemplate>
                                    <a id="EditAssignProjects" href="/_modal_pages/assign-projects.aspx?AssignID=<%#Eval("AssignID")%>" class="showEditAssignProjectModal"><span title="Edit" class="fas fa-edit"></span></a>
                                </ItemTemplate>
                            </asp:TemplateField>
                        </Columns>
                    </asp:GridView>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </div>

     <%--Assign Project modal--%>
    <div id="ModAssignProject" class="modal it-modal fade">
	    <div class="modal-dialog modal-dialog-centered modal-dialog-scrollable modal-lg">
		    <div class="modal-content">
			    <div class="modal-header">
				    <h5 class="modal-title">Assign Project to User</h5>
                    <button aria-label="Close" class="close" onclick="javascript:window.location.reload()" data-dismiss="modal" type="button"><span aria-hidden="true">&times;</span></button>
			    </div>
			    <div class="modal-body">
                    <iframe class="border-0 w-100" style="height:400px;" loading="lazy"></iframe>
			    </div>
                <%--<div class="modal-footer">
                    <button type="button" class="btn btn-secondary" data-dismiss="modal">Close</button>
                    <button type="button" class="btn btn-primary">Save changes</button>
                </div>--%>
		    </div>
	    </div>
    </div>
     <%--Edit Assign Project modal--%>
    <div id="ModEditAssignProject" class="modal it-modal fade">
	    <div class="modal-dialog modal-dialog-centered modal-dialog-scrollable modal-lg">
		    <div class="modal-content">
			    <div class="modal-header">
				    <h5 class="modal-title">Edit Assigned Project to User</h5>
                    <button aria-label="Close" class="close" onclick="javascript:window.location.reload()" data-dismiss="modal" type="button"><span aria-hidden="true">&times;</span></button>
			    </div>
			    <div class="modal-body">
                    <iframe class="border-0 w-100" style="height:400px;" loading="lazy"></iframe>
			    </div>
                <%--<div class="modal-footer">
                    <button type="button" class="btn btn-secondary" data-dismiss="modal">Close</button>
                    <button type="button" class="btn btn-primary">Save changes</button>
                </div>--%>
		    </div>
	    </div>
    </div>
</asp:Content>
