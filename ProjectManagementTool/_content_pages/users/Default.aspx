<%@ Page Title="" Language="C#" MasterPageFile="~/_master_pages/default.Master" AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="ProjectManager._content_pages.users.Default" %>
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
        $(document).ready(function () {
            $(".showAddUserModal").click(function (e) {
                e.preventDefault();
                var url = $(this).attr("href");
                $("#ModAddUser iframe").attr("src", url);
                $("#ModAddUser").modal("show");
            });
            $(".showEditUserModal").click(function (e) {
                e.preventDefault();
                var url = $(this).attr("href");
                $("#ModEditUser iframe").attr("src", url);
                $("#ModEditUser").modal("show");
            });
             });
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="default_master_body" runat="server">
    <div class="container-fluid">
            <div class="row">
                <div class="col-md-12 col-lg-4 form-group">User Management</div>
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
                                                    <asp:Label ID="ActivityHeading" Text="List of Users" CssClass="text-uppercase font-weight-bold" runat="server" />
                                                    <%--<asp:Label Text="Foo bar" CssClass="pl-1" runat="server" />--%>
                                                </h6>
                                                <div>
                                                   <a href="/_modal_pages/add-user.aspx" class="showAddUserModal"><asp:Button ID="btnadd" runat="server" Text="+ Add Users" CssClass="btn btn-primary"></asp:Button></a>
                                                </div>
                                            </div>
                                        </div>

                        <div class="table-responsive">
                                <asp:GridView ID="GrdUsers" runat="server" AutoGenerateColumns="False" EmptyDataText="No Data" PageSize="15" Width="100%" CssClass="table table-bordered"  OnPageIndexChanging="GrdUsers_PageIndexChanging" OnRowCommand="GrdUsers_RowCommand" OnRowDeleting="GrdUsers_RowDeleting">
                                <Columns>
                                 <asp:BoundField DataField="UserName" HeaderText="User Name">
                                <HeaderStyle HorizontalAlign="Left" />
                                </asp:BoundField>
                                         <asp:BoundField DataField="EmailID" HeaderText="EmailID">
                                <HeaderStyle HorizontalAlign="Left" />
                                </asp:BoundField>
                                <asp:BoundField DataField="Mobilenumber" HeaderText="Mobile Number" >
                                <HeaderStyle HorizontalAlign="Left" />
                                </asp:BoundField>
                                <asp:BoundField DataField="Address1" HeaderText="Address 1">
                                <HeaderStyle HorizontalAlign="Left" />
                                </asp:BoundField>
                                <asp:BoundField DataField="Address2" HeaderText="Address 2">
                                <HeaderStyle HorizontalAlign="Left" />
                                </asp:BoundField>
                                    <asp:TemplateField HeaderText="User Type">
                                        <ItemTemplate>
                                            <%#getUserType(Eval("TypeOfUser").ToString())%>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                     <asp:BoundField DataField="CreatedDate" HeaderText="Created Date"  DataFormatString="{0:dd/MM/yyyy}">
                                <HeaderStyle HorizontalAlign="Left" />
                                </asp:BoundField>
                                    <asp:TemplateField>
                                        <ItemTemplate>
                                            <a id="EditUsers" href='/_modal_pages/add-user.aspx?UserUID=<%#Eval("UserUID")%>' class="showEditUserModal"><span title="Edit" class="fas fa-edit"></span></a>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                     <asp:TemplateField>
                                        <ItemTemplate>
                                              <asp:LinkButton ID="lnkdelete" runat="server" OnClientClick="return DeleteItem()" CausesValidation="false" CommandArgument='<%#Eval("UserUID")%>' CommandName="delete"><span title="Delete" class="fas fa-trash"></span></asp:LinkButton>
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

    <%--add Add User modal--%>
    <div id="ModAddUser" class="modal it-modal fade">
	    <div class="modal-dialog modal-dialog-centered modal-dialog-scrollable modal-xl">
		    <div class="modal-content">
			    <div class="modal-header">
				    <h5 class="modal-title">Add User</h5>
                    <button aria-label="Close" class="close" data-dismiss="modal" type="button"><span aria-hidden="true">&times;</span></button>
			    </div>
			    <div class="modal-body">
                    <iframe class="border-0 w-100" style="height:500px;" loading="lazy"></iframe>
			    </div>
              
		    </div>
	    </div>
    </div>
    <%--add Edit User modal--%>
    <div id="ModEditUser" class="modal it-modal fade">
	    <div class="modal-dialog modal-dialog-centered modal-dialog-scrollable modal-xl">
		    <div class="modal-content">
			    <div class="modal-header">
				    <h5 class="modal-title">Edit User</h5>
                    <button aria-label="Close" class="close" data-dismiss="modal" type="button"><span aria-hidden="true">&times;</span></button>
			    </div>
			    <div class="modal-body">
                    <iframe class="border-0 w-100" style="height:500px;" loading="lazy"></iframe>
			    </div>
              
		    </div>
	    </div>
    </div>
</asp:Content>
