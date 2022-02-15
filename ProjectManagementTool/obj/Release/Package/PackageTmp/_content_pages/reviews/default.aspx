<%@ Page Title="" Language="C#" MasterPageFile="~/_master_pages/default.Master" AutoEventWireup="true" CodeBehind="default.aspx.cs" Inherits="ProjectManagementTool._content_pages.reviews._default" %>
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
            $(".showReviewsModal").click(function (e) {
                e.preventDefault();
                var url = $(this).attr("href");
                $("#ModAddReview iframe").attr("src", url);
                $("#ModAddReview").modal("show");
            });
            $(".showEditReviewsModal").click(function (e) {
                e.preventDefault();
                var url = $(this).attr("href");
                $("#ModEditReview iframe").attr("src", url);
                $("#ModEditReview").modal("show");
            });
            $(".showReviewRecordsModal").click(function (e) {
                e.preventDefault();
                var url = $(this).attr("href");
                $("#ModAddReviewRecords iframe").attr("src", url);
                $("#ModAddReviewRecords").modal("show");
            });
        });
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="default_master_body" runat="server">
       <%--project selection dropdowns--%>
        <div class="container-fluid">
            <div class="row">
                <div class="col-md-12 col-lg-4 form-group">Reviews</div>
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
                                   <asp:Label ID="ActivityHeading" CssClass="text-uppercase font-weight-bold" runat="server" Text="List of Reviews" />
                                       <%--<asp:Label Text="Foo bar" CssClass="pl-1" runat="server" />--%>
                               </h6>
                                <div>
                                   <a id="AddReviews" runat="server" href="/_modal_pages/add-review.aspx" class="showReviewsModal"><asp:Button ID="Button2" runat="server" Text="+ Add Reviews" CssClass="btn btn-primary"></asp:Button></a>
                                </div>
                          </div>
                            </div>
                        <div class="table-responsive">
                                <asp:GridView ID="GrdReviews" runat="server" CssClass="table table-bordered" AutoGenerateColumns="False" PageSize="10" Width="100%" OnPageIndexChanging="GrdReviews_PageIndexChanging" OnRowCommand="GrdReviews_RowCommand" OnRowDeleting="GrdReviews_RowDeleting">
                        <Columns>
                            <asp:TemplateField HeaderText="User">
                                <ItemTemplate>
                                    <%#GetUserName(Eval("User_UID").ToString())%>
                                </ItemTemplate>
                            </asp:TemplateField>
                            
                             <asp:BoundField DataField="Review_Type" HeaderText="Review Type">
                    <HeaderStyle HorizontalAlign="Left" />
                    </asp:BoundField>
                    <asp:BoundField DataField="Review_Description" HeaderText="Review Description" >
                    <HeaderStyle HorizontalAlign="Left" />
                    </asp:BoundField>
                             <asp:BoundField DataField="Review_Date" HeaderText="Review Date"  DataFormatString="{0:dd/MM/yyyy}">
                    <HeaderStyle HorizontalAlign="Left" />
                    </asp:BoundField>
                           <%-- <asp:TemplateField>
                                <ItemTemplate>
                                    <a id="AddTasks" href='AddReviewTasks.aspx?Reviews_UID=<%#Eval("Reviews_UID")%>' class="AddTasks">Add Tasks</a>
                                </ItemTemplate>
                            </asp:TemplateField>--%>

                            <asp:TemplateField>
                                <ItemTemplate>
                                    <a href="/_modal_pages/add-review-record.aspx?Reviews_UID=<%#Eval("Reviews_UID")%>&Review_Date=<%#Eval("Review_Date")%>&PrjID=<%#Eval("ProjectUID")%>" class="showReviewRecordsModal">Add Records</a>
                                </ItemTemplate>
                            </asp:TemplateField>

                            <asp:TemplateField>
                                <ItemTemplate>
                                    <a href="/_modal_pages/add-review.aspx?Reviews_UID=<%#Eval("Reviews_UID")%>" class="showEditReviewsModal"><span title="Edit" class="fas fa-edit"></span></a>
                                </ItemTemplate>
                            </asp:TemplateField>
                           <asp:TemplateField>
                                  <ItemTemplate>
                                         <asp:LinkButton ID="lnkdelete" runat="server" OnClientClick="return DeleteItem()" CausesValidation="false" CommandArgument='<%#Eval("Reviews_UID")%>' CommandName="delete"><span title="Delete" class="fas fa-trash"></span></asp:LinkButton>
                                  </ItemTemplate>
                           </asp:TemplateField>
                        </Columns>
                                    <EmptyDataTemplate>
                        No Records Found !
                    </EmptyDataTemplate>
                    </asp:GridView>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </div>

    <%--add Review modal--%>
    <div id="ModAddReview" class="modal it-modal fade">
	    <div class="modal-dialog modal-dialog-centered modal-dialog-scrollable modal-lg">
		    <div class="modal-content">
			    <div class="modal-header">
				    <h5 class="modal-title">Add Review</h5>
                    <button aria-label="Close" class="close" data-dismiss="modal" type="button"><span aria-hidden="true">&times;</span></button>
			    </div>
			    <div class="modal-body">
                    <iframe class="border-0 w-100" style="height: 460px;" loading="lazy"></iframe>
			    </div>
              
		    </div>
	    </div>
    </div>

    <%--Edir Review modal--%>
    <div id="ModEditReview" class="modal it-modal fade">
	    <div class="modal-dialog modal-dialog-centered modal-dialog-scrollable modal-lg">
		    <div class="modal-content">
			    <div class="modal-header">
				    <h5 class="modal-title">Edit Review</h5>
                    <button aria-label="Close" class="close" data-dismiss="modal" type="button"><span aria-hidden="true">&times;</span></button>
			    </div>
			    <div class="modal-body">
                    <iframe class="border-0 w-100" style="height: 460px;" loading="lazy"></iframe>
			    </div>
              
		    </div>
	    </div>
    </div>

    <div id="ModAddReviewRecords" class="modal it-modal fade">
	    <div class="modal-dialog modal-dialog-centered modal-dialog-scrollable modal-xl">
		    <div class="modal-content">
			    <div class="modal-header">
				    <h5 class="modal-title">Add Review Records</h5>
                    <button aria-label="Close" class="close" data-dismiss="modal" type="button"><span aria-hidden="true">&times;</span></button>
			    </div>
			    <div class="modal-body">
                    <iframe class="border-0 w-100" style="height:500px;" loading="lazy"></iframe>
			    </div>
              
		    </div>
	    </div>
    </div>

</asp:Content>
