<%@ Page Title="" Language="C#" MasterPageFile="~/_master_pages/default.Master" AutoEventWireup="true" CodeBehind="default.aspx.cs" Inherits="ProjectManagementTool._content_pages.user_functionality._default" %>
<asp:Content ID="Content1" ContentPlaceHolderID="default_master_head" runat="server">
     <style type="text/css">
        .hiddencol { display: none; }
    </style>
         <script type="text/javascript">
             function BindEvents() {
                 $(".showAddPageNameModal").click(function(e) {
                     e.preventDefault();                 
                var url = $(this).attr("href");
                $("#ModAddPageName iframe").attr("src", url);
                $("#ModAddPageName").modal("show");
                 });

                 $(".EditPageName").click(function(e) {
                     e.preventDefault();                 
                var url = $(this).attr("href");
                $("#ModPageName iframe").attr("src", url);
                $("#ModPageName").modal("show");
                 });

                  $(".showAddUserFunctionalityModal").click(function(e) {
                     e.preventDefault();                 
                var url = $(this).attr("href");
                $("#ModAddUserFunctionality iframe").attr("src", url);
                $("#ModAddUserFunctionality").modal("show");
                  });

                 $(".EditUserFunctionality").click(function(e) {
                     e.preventDefault();                 
                var url = $(this).attr("href");
                $("#ModEditUserFunctionality iframe").attr("src", url);
                $("#ModEditUserFunctionality").modal("show");
                 });
             }
            $(document).ready(function () {
                BindEvents();
               });
        </script>

    <script type="text/javascript">
        function DeleteItem() {
            if (confirm("Are you sure you want to delete ...?")) {
                return true;
            }
            return false;
        }
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="default_master_body" runat="server">
     
        <div class="container-fluid">
            <div class="row">
                <div class="col-md-12 col-lg-12 form-group">User Functionality</div>
                
            </div>
        </div>
        <div class="container-fluid">
            <div class="row">
               <div class="col-lg-6 col-xl-4 form-group">
                   <div class="card mb-4">
                       <div class="card-body">
                                     <div class="card-title">
                                            <div class="d-flex justify-content-between">
                                                <h6 class="text-muted">
                                                    <asp:Label ID="ActivityHeading" CssClass="text-uppercase font-weight-bold" runat="server" Text="Master Pages" />
                                                </h6>
                                                <div>
                                                    <a href="/_modal_pages/add-pagenamemaster.aspx" class="showAddPageNameModal"><asp:Button ID="Button2" runat="server" Text="+ Add Master Page" CssClass="btn btn-primary"></asp:Button></a>
                                                </div>
                                            </div>
                                        </div>
                           <div class="table-responsive">
                               <asp:GridView ID="GrdMasterPages" runat="server" AllowPaging="true" PageSize="15" AutoGenerateColumns="False" CssClass="table table-bordered" Width="100%" OnRowCommand="GrdMasterPages_RowCommand" OnRowDeleting="GrdMasterPages_RowDeleting" OnPageIndexChanging="GrdMasterPages_PageIndexChanging">
                                <Columns>
                                    <asp:TemplateField HeaderText="S.No" ItemStyle-HorizontalAlign="Center">
                                        <ItemTemplate>
                                            <%# Container.DataItemIndex + 1 %>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Page Name">
                                        <ItemTemplate>
                                            <%#Eval("MasterPageName")%>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Page URL">
                                        <ItemTemplate>
                                            <%#Eval("MasterPageURL")%>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField>
                                        <ItemTemplate>
                                            <a href='/_modal_pages/add-pagenamemaster.aspx?MasterPageUID=<%#Eval("MasterPageUID")%>' class="EditPageName"><span title="Edit" class="fas fa-edit"></span></a>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField>
                                            <ItemTemplate>
                                                    <asp:LinkButton ID="lnkdelete" runat="server" OnClientClick="return DeleteItem()" CausesValidation="false" CommandArgument='<%#Eval("MasterPageUID")%>' CommandName="delete"><span title="Delete" class="fas fa-trash"></span></asp:LinkButton>
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

    <div class="container-fluid">
            <div class="row">
               <div class="col-lg-6 col-xl-4 form-group">
                   <div class="card mb-4">
                       <div class="card-body">
                                     <div class="card-title">
                                            <div class="d-flex justify-content-between">
                                                <h6 class="text-muted">
                                                    <asp:Label ID="Label1" CssClass="text-uppercase font-weight-bold" runat="server" Text="User Functionality Assignment" />
                                                </h6>
                                                <div>
                                                    <a href="#" class="showAddUserFunctionalityModal"><asp:Button ID="btnUserFunctionality" runat="server" Text="+ Add User Functionality" CssClass="btn btn-primary"></asp:Button></a>
                                                </div>
                                            </div>
                                        </div>
                           <div class="table-responsive">
                               <asp:GridView ID="GrdUserFunctionalityAssignment" runat="server" AllowPaging="true" PageSize="15" AutoGenerateColumns="False" CssClass="table table-bordered" Width="100%" OnRowCommand="" OnRowDeleting="" OnPageIndexChanging="">
                                <Columns>
                                    <asp:TemplateField HeaderText="S.No" ItemStyle-HorizontalAlign="Center">
                                        <ItemTemplate>
                                            <%# Container.DataItemIndex + 1 %>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Page Name">
                                        <ItemTemplate>
                                            <%#Eval("MasterPageUID")%>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="User Name">
                                        <ItemTemplate>
                                            <%#Eval("UserUID")%>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Add">
                                        <ItemTemplate>
                                            <%#Eval("Functionality_Add")%>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Edit">
                                        <ItemTemplate>
                                            <%#Eval("Functionality_Update")%>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Delete">
                                        <ItemTemplate>
                                            <%#Eval("Functionality_Delete")%>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField>
                                        <ItemTemplate>
                                            <a href='#' class="EditUserFunctionality"><span title="Edit" class="fas fa-edit"></span></a>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField>
                                            <ItemTemplate>
                                                    <asp:LinkButton ID="lnkdelete" runat="server" OnClientClick="return DeleteItem()" CausesValidation="false" CommandArgument='<%#Eval("MasterPageUID")%>' CommandName="delete"><span title="Delete" class="fas fa-trash"></span></asp:LinkButton>
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
     <%--add Page Name modal--%>
    <div id="ModAddPageName" class="modal it-modal fade">
	    <div class="modal-dialog modal-dialog-centered modal-dialog-scrollable modal-lg">
		    <div class="modal-content">
			    <div class="modal-header">
				    <h5 class="modal-title">Add Page Name</h5>
                    <button aria-label="Close" class="close" data-dismiss="modal" type="button"><span aria-hidden="true">&times;</span></button>
			    </div>
			    <div class="modal-body">
                    <iframe class="border-0 w-100" style="height: 400px;" loading="lazy"></iframe>
			    </div>
              
		    </div>
	    </div>
    </div>

    <%--Edit PageName modal--%>
    <div id="EditPageName" class="modal it-modal fade">
	    <div class="modal-dialog modal-dialog-centered modal-dialog-scrollable modal-lg">
		    <div class="modal-content">
			    <div class="modal-header">
				    <h5 class="modal-title">Edit Page Name</h5>
                    <button aria-label="Close" class="close" data-dismiss="modal" type="button"><span aria-hidden="true">&times;</span></button>
			    </div>
			    <div class="modal-body">
                    <iframe class="border-0 w-100" style="height: 400px;" loading="lazy"></iframe>
			    </div>
              
		    </div>
	    </div>
    </div>


     <%--add User Functionality modal--%>
    <div id="ModAddUserFunctionality" class="modal it-modal fade">
	    <div class="modal-dialog modal-dialog-centered modal-dialog-scrollable modal-lg">
		    <div class="modal-content">
			    <div class="modal-header">
				    <h5 class="modal-title">Add User Functionality</h5>
                    <button aria-label="Close" class="close" data-dismiss="modal" type="button"><span aria-hidden="true">&times;</span></button>
			    </div>
			    <div class="modal-body">
                    <iframe class="border-0 w-100" style="height: 460px;" loading="lazy"></iframe>
			    </div>
              
		    </div>
	    </div>
    </div>

    <%--Edit PageName modal--%>
    <div id="ModEditUserFunctionality" class="modal it-modal fade">
	    <div class="modal-dialog modal-dialog-centered modal-dialog-scrollable modal-lg">
		    <div class="modal-content">
			    <div class="modal-header">
				    <h5 class="modal-title">Edit User Functionality</h5>
                    <button aria-label="Close" class="close" data-dismiss="modal" type="button"><span aria-hidden="true">&times;</span></button>
			    </div>
			    <div class="modal-body">
                    <iframe class="border-0 w-100" style="height: 460px;" loading="lazy"></iframe>
			    </div>
              
		    </div>
	    </div>
    </div>
</asp:Content>
 