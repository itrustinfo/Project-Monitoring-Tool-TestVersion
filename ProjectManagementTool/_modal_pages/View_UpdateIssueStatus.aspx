<%@ Page Title="" Language="C#" MasterPageFile="~/_master_pages/modal.Master" AutoEventWireup="true" CodeBehind="View_UpdateIssueStatus.aspx.cs" Inherits="ProjectManagementTool._modal_pages.View_UpdateIssueStatus" %>
<asp:Content ID="Content1" ContentPlaceHolderID="modal_master_head" runat="server">
     <script type="text/javascript">
        function DeleteItem() {
            if (confirm("Are you sure you want to delete ...?")) {
                return true;
            }
            return false;
    }
    </script>

    <style type="text/css">
        .hiddencol { display: none; }
    </style>
    <script type="text/javascript">
        $(document).ready(function () {
            $(".showStatusModal").click(function (e) {
                e.preventDefault();
                jQuery.noConflict();
                var url = $(this).attr("href");
                $("#ModAddIssueStatus iframe").attr("src", url);
                $('#ModAddIssueStatus').modal({ backdrop: 'static', keyboard: false })
                $("#ModAddIssueStatus").modal("show");
            });

            $(".EditStatusModal").click(function (e) {
                e.preventDefault();
                jQuery.noConflict();
                var url = $(this).attr("href");
                $("#ModEditIssueStatus iframe").attr("src", url);
                $('#ModEditIssueStatus').modal({ backdrop: 'static', keyboard: false })
                $("#ModEditIssueStatus").modal("show");
            });
        });
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="modal_master_body" runat="server">
    <form id="frmAddDocumentModal" runat="server">
    <div class="container-fluid" style="overflow-y:auto; min-height:83vh;">
        <div class="row">
           <div class="col-sm-6">
                <div class="form-group">
                       <div class="form-group" >
                        <label class="lblCss" for="lblWorkPackage">Issue</label>&nbsp;&nbsp;<b>:</b>&nbsp;&nbsp;
                         <asp:Label ID="LblIssue" class="lblCss" Font-Bold="true" runat="server"></asp:Label>
                    </div>
                 </div>
           </div>
            <div class="col-sm-6">
                 
                </div>
        </div>
        <div class="row">
            <div class="col-sm-12">
                <div class="table-responsive">
                        <asp:GridView ID="GrdIssueStatus" runat="server" Width="100%" PageSize="10" AllowPaging="true" CssClass="table table-bordered" DataKeyNames="IssueRemarksUID" EmptyDataText="No Status Found" AutoGenerateColumns="false" OnRowDataBound="GrdIssueStatus_RowDataBound" OnRowCommand="GrdIssueStatus_RowCommand" OnRowDeleting="GrdIssueStatus_RowDeleting" OnPageIndexChanging="GrdIssueStatus_PageIndexChanging">
                        <Columns>
                            <asp:BoundField DataField="Issue_Status" HeaderText="Status" >
                            <HeaderStyle HorizontalAlign="Left" />
                            </asp:BoundField>
                            <asp:BoundField DataField="Issue_Remarks" HeaderText="Remarks">
                            <HeaderStyle HorizontalAlign="Left" />
                            </asp:BoundField>
                            <asp:BoundField DataField="IssueRemark_Date" HeaderText="Date" DataFormatString="{0:dd MMM yyyy}">
                            <HeaderStyle HorizontalAlign="Left" />
                            </asp:BoundField>
                            <asp:BoundField DataField="Issue_Document" ItemStyle-CssClass="hiddencol"  HeaderStyle-CssClass="hiddencol" HeaderText="LinkToReviewFile" >
                            <HeaderStyle HorizontalAlign="Left" />
                            </asp:BoundField>
                             <asp:BoundField DataField="IssueRemarksUID" ItemStyle-CssClass="hiddencol"  HeaderStyle-CssClass="hiddencol" HeaderText="LinkToReviewFile" >
                            <HeaderStyle HorizontalAlign="Left" />
                            </asp:BoundField>
                            <asp:TemplateField HeaderText="Status Document">
                                <ItemTemplate>
                                      <%--<asp:LinkButton ID="lnkdown" runat="server" CommandArgument='<%#Eval("IssueRemarksUID")%>' CausesValidation="false" CommandName="download">Download</asp:LinkButton>--%>
                               </ItemTemplate>
                            </asp:TemplateField>
                             <asp:TemplateField>
                                 <ItemTemplate>
                                       <a id="Edit" href="/_modal_pages/add-issuestatus.aspx?IssueRemarksUID=<%#Eval("IssueRemarksUID")%>&Issue_Uid=<%#Eval("Issue_Uid")%>" class="EditStatusModal"><span title="Edit" class="fas fa-edit"></span></a> 
                                 </ItemTemplate>
                             </asp:TemplateField>
                             <asp:TemplateField>
                                  <ItemTemplate>
                                        <asp:LinkButton ID="lnkdelete" runat="server" CausesValidation="false" OnClientClick="return DeleteItem()" CommandArgument='<%#Eval("IssueRemarksUID")%>' CommandName="delete"><span title="Delete" class="fas fa-trash"></span></asp:LinkButton>
                                  </ItemTemplate>
                             </asp:TemplateField>          
                        </Columns>
                    </asp:GridView>
                           </div>
                </div>
            </div>
    </div>
    <div class="modal-footer">
            <a id="AddStatus" runat="server" href="/_modal_pages/add-issuestatus.aspx" class="showStatusModal"><asp:Button ID="btnaddstatus" runat="server" Height="35px" Width="150px" Text="+ Add Status" CssClass="btn btn-primary"></asp:Button></a>
                </div>

    <%--View Issue status modal--%>
    <div id="ModAddIssueStatus" class="modal it-modal fade">
	    <div class="modal-dialog modal-dialog-centered modal-dialog-scrollable modal-lg">
		    <div class="modal-content">
			    <div class="modal-header">
				    <h5 class="modal-title">Add Status</h5>
                    <button aria-label="Close" class="close" data-dismiss="modal" type="button"><span aria-hidden="true">&times;</span></button>
			    </div>
			    <div class="modal-body">
                    <iframe class="border-0 w-100" style="height:280px;" loading="lazy"></iframe>
			    </div>
              
		    </div>
	    </div>
    </div>
         <div id="ModEditIssueStatus" class="modal it-modal fade">
	    <div class="modal-dialog modal-dialog-centered modal-dialog-scrollable modal-lg">
		    <div class="modal-content">
			    <div class="modal-header">
				    <h5 class="modal-title">Edit Status</h5>
                    <button aria-label="Close" class="close" data-dismiss="modal" type="button"><span aria-hidden="true">&times;</span></button>
			    </div>
			    <div class="modal-body">
                    <iframe class="border-0 w-100" style="height:280px;" loading="lazy"></iframe>
			    </div>
              
		    </div>
	    </div>
    </div>
        </form>
</asp:Content>
