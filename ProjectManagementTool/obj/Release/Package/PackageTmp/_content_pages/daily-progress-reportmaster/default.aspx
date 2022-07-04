<%@ Page Title="" Language="C#" MasterPageFile="~/_master_pages/default.Master" AutoEventWireup="true" CodeBehind="default.aspx.cs" Inherits="ProjectManagementTool._content_pages.daily_progress_reportmaster._default" %>


<asp:Content ID="Content1" ContentPlaceHolderID="default_master_head" runat="server">
    <script type="text/javascript">
        function BindEvents() {

             $(".showAddReviewMasterModal").click(function(e) {
        e.preventDefault();
        var url = $(this).attr("href");
        $("#ModAddReviewMaster iframe").attr("src", url);
        $("#ModAddReviewMaster").modal("show");
            });

            $(".showEditReviewMasterModal").click(function(e) {
        e.preventDefault();
        var url = $(this).attr("href");
        $("#ModEditReviewMaster iframe").attr("src", url);
        $("#ModEditReviewMaster").modal("show");
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
            <div class="col-md-12 col-lg-12 form-group">
                <div class="card mb-4">
                    <div class="card-body">
                        <div class="card-title">
                        <div class="d-flex justify-content-between">
                              <h6 class="text-muted">
                                   <asp:Label ID="Label5" CssClass="text-uppercase font-weight-bold" runat="server" Text="Daily Progress Master List"  />
                               </h6>
                                <div>
                                   <a href="/_modal_pages/add-dailyprogressreport-master.aspx?type=Add" class="showAddReviewMasterModal"><asp:Button ID="btnAddClass" runat="server" Text="+ Add Daily Progress Report Master" CssClass="btn btn-primary"></asp:Button></a>
                                </div>
                          </div>
                            </div>
                        <div class="table-responsive">
                            <asp:GridView ID="GrdReviewMeeting" runat="server" CssClass="table table-bordered" AutoGenerateColumns="False" Width="100%" PageSize="10" OnPageIndexChanging="GrdReviewMeeting_PageIndexChanging">
                               <Columns>             
                                    <asp:TemplateField HeaderText="Description">
                                      <ItemTemplate>
                                              <%#Eval("Description")%>
                                      </ItemTemplate>
                                    </asp:TemplateField>  
                                   <asp:TemplateField HeaderText="Created Date">
                                      <ItemTemplate>
                                              <%#Eval("CreatedDate","{0:dd MMM yyyy}")%>
                                      </ItemTemplate>
                                    </asp:TemplateField>  
                                   <asp:TemplateField>
                                      <ItemTemplate>
                                          <a href='/_modal_pages/add-dailyprogressreport-master.aspx?type=edit&MeetigUID=<%#Eval("DPR_UID")%>' class="showEditReviewMasterModal"><span title="Edit" class="fas fa-edit"></span></a>
                                      </ItemTemplate>
                                    </asp:TemplateField>
                                   <asp:TemplateField>
                                       <ItemTemplate>
                                           <a href='../landing-page/default.aspx?UID=<%#Eval("DPR_UID")%>'>View</a>
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

    <%--add Review Master modal--%>
    <div id="ModAddReviewMaster" class="modal it-modal fade">
	    <div class="modal-dialog modal-dialog-centered modal-dialog-scrollable modal-lg">
		    <div class="modal-content">
			    <div class="modal-header">
				    <h5 class="modal-title">Add Daily Progress Report Master</h5>
                    <button aria-label="Close" class="close" data-dismiss="modal" type="button"><span aria-hidden="true">&times;</span></button>
			    </div>
			    <div class="modal-body">
                    <iframe class="border-0 w-100" style="height:300px;" loading="lazy"></iframe>
			    </div>
              
		    </div>
	    </div>
    </div>

    <%--add Edit Review Master modal--%>
    <div id="ModEditReviewMaster" class="modal it-modal fade">
	    <div class="modal-dialog modal-dialog-centered modal-dialog-scrollable modal-lg">
		    <div class="modal-content">
			    <div class="modal-header">
				    <h5 class="modal-title">Edit Daily Progress Report Master</h5>
                    <button aria-label="Close" class="close" data-dismiss="modal" type="button"><span aria-hidden="true">&times;</span></button>
			    </div>
			    <div class="modal-body">
                    <iframe class="border-0 w-100" style="height:300px;" loading="lazy"></iframe>
			    </div>
              
		    </div>
	    </div>
    </div>
</asp:Content>