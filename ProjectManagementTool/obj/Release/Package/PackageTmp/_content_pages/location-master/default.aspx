<%@ Page Title="" Language="C#" MasterPageFile="~/_master_pages/default.Master" AutoEventWireup="true" CodeBehind="default.aspx.cs" Inherits="ProjectManagementTool._content_pages.location_master._default" %>
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
         $(".showAddLocationMaster").click(function(e) {
        e.preventDefault();
        var url = $(this).attr("href");
        $("#ModAddLocationMaster iframe").attr("src", url);
        $("#ModAddLocationMaster").modal("show");
              });

        $(".showEditLocationMaster").click(function(e) {
        e.preventDefault();
        var url = $(this).attr("href");
        $("#ModEditLocationMaster iframe").attr("src", url);
        $("#ModEditLocationMaster").modal("show");
              });

        $(".showAssignLocationtoUser").click(function(e) {
        e.preventDefault();
        var url = $(this).attr("href");
        $("#ModAssignLocation iframe").attr("src", url);
        $("#ModAssignLocation").modal("show");
          });

         $(".ShowModEditAssignedLocation").click(function(e) {
        e.preventDefault();
        var url = $(this).attr("href");
        $("#ModEditAssignedLocation iframe").attr("src", url);
        $("#ModEditAssignedLocation").modal("show");
            });
        }
        $(document).ready(function () {
            BindEvents();
            //$('#loader').fadeOut();
        });
</script>

</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="default_master_body" runat="server">
    <div class="container-fluid">
        <div class="row">
            <div class="col-md-6 col-lg-6 form-group">
                <div class="card mb-4">
                    <div class="card-body">
                        <div class="card-title">
                        <div class="d-flex justify-content-between">
                              <h6 class="text-muted">
                                   <asp:Label ID="LblLocationHeading" CssClass="text-uppercase font-weight-bold" runat="server" Text="List of Locations"  />
                               </h6>
                                <div>
                                   <a href="/_modal_pages/add-location.aspx?type=Add" class="showAddLocationMaster"><asp:Button ID="btnAddLocation" runat="server" Text="+ Add Location" CssClass="btn btn-primary"></asp:Button></a>
                                </div>
                          </div>
                            </div>
                        <div class="table-responsive">
                            <asp:GridView ID="GrdLocation" runat="server" CssClass="table table-bordered" AutoGenerateColumns="False" Width="100%" PageSize="10" OnPageIndexChanging="GrdLocation_PageIndexChanging" OnRowCommand="GrdLocation_RowCommand" OnRowDeleting="GrdLocation_RowDeleting" >
                               <Columns>   
                                    <asp:TemplateField HeaderText="Location Name">
                                      <ItemTemplate>
                                              <%#Eval("Location_Name")%>
                                      </ItemTemplate>
                                    </asp:TemplateField> 
                                   <asp:TemplateField>
                                      <ItemTemplate>
                                          <a href='/_modal_pages/add-location.aspx?type=edit&lUID=<%#Eval("LocationMaster_UID")%>' class="showEditLocationMaster"><span title="Edit" class="fas fa-edit"></span></a>
                                              <%--<asp:LinkButton ID="lnkedit" runat="server" CausesValidation="false" CommandName="edit" CommandArgument='<%#Eval("MasterWorkPackageUID")%>'><span title="Edit" class="fas fa-edit"></span></asp:LinkButton>--%>
                                      </ItemTemplate>
                                    </asp:TemplateField>
                                   <asp:TemplateField>
                                      <ItemTemplate>
                                              <asp:LinkButton ID="lnkdelete" runat="server" CausesValidation="false" OnClientClick="return DeleteItem()" CommandName="delete" CommandArgument='<%#Eval("LocationMaster_UID")%>'><span title="Delete" class="fas fa-trash"></span></asp:LinkButton>
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
            <div class="col-md-6 col-lg-6 form-group">
                <div class="card mb-4">
                    <div class="card-body">
                        <div class="card-title">
                        <div class="d-flex justify-content-between">
                              <h6 class="text-muted">
                                   <asp:Label ID="LblAssignLocationHeading" CssClass="text-uppercase font-weight-bold" runat="server" Text="List of Assigned Location for Users"  />
                               </h6>
                                <div>
                                   <a href="/_modal_pages/assign-locationtouser.aspx?type=Add" class="showAssignLocationtoUser"><asp:Button ID="btnAssign" runat="server" Text="+ Assign Location" CssClass="btn btn-primary"></asp:Button></a>
                                </div>
                          </div>
                            </div>
                        <div class="table-responsive">
                            <asp:GridView ID="GrdAssignedLocation" runat="server" DataKeyNames="UserUID" CssClass="table table-bordered" AutoGenerateColumns="False" Width="100%" PageSize="10" OnRowDataBound="GrdAssignedLocation_RowDataBound" OnPageIndexChanging="GrdAssignedLocation_PageIndexChanging" OnRowCommand="GrdAssignedLocation_RowCommand" OnRowDeleting="GrdAssignedLocation_RowDeleting" >
                               <Columns>
                                   <%-- <asp:TemplateField HeaderText="Location Name">
                                      <ItemTemplate>
                                              <%#Eval("LocationMaster_UID")%>
                                      </ItemTemplate>
                                    </asp:TemplateField> --%>
                                   <asp:TemplateField HeaderText="User Name">
                                      <ItemTemplate>
                                              <%#GetUserName(Eval("UserUID").ToString())%>
                                      </ItemTemplate>
                                    </asp:TemplateField>
                                   <asp:TemplateField HeaderText="Assigned Locations">
                                       <ItemTemplate>
                                           <asp:DataList ID="DL_Location" runat="server" ShowHeader="false" ShowFooter="false" RepeatColumns="2">
                                               <ItemTemplate>
                                                   <%#GetLocationName(Eval("LocationMaster_UID").ToString())%>
                                               </ItemTemplate>
                                           </asp:DataList>
                                       </ItemTemplate>
                                   </asp:TemplateField>
                                   <asp:TemplateField>
                                      <ItemTemplate>
                                          <a href='/_modal_pages/assign-locationtouser.aspx?type=edit&uUID=<%#Eval("UserUID")%>' class="ShowModEditAssignedLocation"><span title="Edit" class="fas fa-edit"></span></a>
                                              <%--<asp:LinkButton ID="lnkedit" runat="server" CausesValidation="false" CommandName="edit" CommandArgument='<%#Eval("MasterWorkPackageUID")%>'><span title="Edit" class="fas fa-edit"></span></asp:LinkButton>--%>
                                      </ItemTemplate>
                                    </asp:TemplateField>
                                   <asp:TemplateField>
                                      <ItemTemplate>
                                              <asp:LinkButton ID="lnkdelete" runat="server" CausesValidation="false" OnClientClick="return DeleteItem()" CommandName="delete" CommandArgument='<%#Eval("UserUID")%>'><span title="Delete" class="fas fa-trash"></span></asp:LinkButton>
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

    <%--add location Master modal--%>
    <div id="ModAddLocationMaster" class="modal it-modal fade">
	    <div class="modal-dialog modal-dialog-centered modal-dialog-scrollable modal-lg">
		    <div class="modal-content">
			    <div class="modal-header">
				    <h5 class="modal-title">Add Location</h5>
                    <button aria-label="Close" class="close" onclick="javascript:window.location.reload()" data-dismiss="modal" type="button"><span aria-hidden="true">&times;</span></button>
			    </div>
			    <div class="modal-body">
                    <iframe class="border-0 w-100" style="height:250px;" loading="lazy"></iframe>
			    </div>
		    </div>
	    </div>
    </div>

      <%--add Edit Location Master modal--%>
    <div id="ModEditLocationMaster" class="modal it-modal fade">
	    <div class="modal-dialog modal-dialog-centered modal-dialog-scrollable modal-lg">
		    <div class="modal-content">
			    <div class="modal-header">
				    <h5 class="modal-title">Edit Location</h5>
                    <button aria-label="Close" class="close" data-dismiss="modal" type="button"><span aria-hidden="true">&times;</span></button>
			    </div>
			    <div class="modal-body">
                    <iframe class="border-0 w-100" style="height:250px;" loading="lazy"></iframe>
			    </div>
              
		    </div>
	    </div>
    </div>

    <%--Assign location to user modal--%>
    <div id="ModAssignLocation" class="modal it-modal fade">
	    <div class="modal-dialog modal-dialog-centered modal-dialog-scrollable modal-lg">
		    <div class="modal-content">
			    <div class="modal-header">
				    <h5 class="modal-title">Assign Location to User</h5>
                    <button aria-label="Close" class="close" data-dismiss="modal" type="button"><span aria-hidden="true">&times;</span></button>
			    </div>
			    <div class="modal-body">
                    <iframe class="border-0 w-100" style="height:420px;" loading="lazy"></iframe>
			    </div>
              
		    </div>
	    </div>
    </div>

    <%--add Edit Assign location to user modal--%>
    <div id="ModEditAssignedLocation" class="modal it-modal fade">
	    <div class="modal-dialog modal-dialog-centered modal-dialog-scrollable modal-lg">
		    <div class="modal-content">
			    <div class="modal-header">
				    <h5 class="modal-title">Edit Assigned Location</h5>
                    <button aria-label="Close" class="close" data-dismiss="modal" type="button"><span aria-hidden="true">&times;</span></button>
			    </div>
			    <div class="modal-body">
                    <iframe class="border-0 w-100" style="height:420px;" loading="lazy"></iframe>
			    </div>
              
		    </div>
	    </div>
    </div>
</asp:Content>
