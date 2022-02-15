<%@ Page Title="" Language="C#" MasterPageFile="~/_master_pages/default.Master" AutoEventWireup="true" CodeBehind="default.aspx.cs" Inherits="ProjectManagementTool._content_pages.other_points_discussion_insert._default" %>
<asp:Content ID="Content1" ContentPlaceHolderID="default_master_head" runat="server">
    <script type="text/javascript">
        function CopyMessage() {
            if (confirm("Are you sure you want to Copy ...?")) {
                return true;
            }
            return false;
        }
        function DeleteItem() {
            if (confirm("Are you sure you want to delete ...?")) {
                return true;
            }
            return false;
    }
    </script>
     <script>
      $(document).ready(function() {
  $(".showModalAddPhysicalProgress").click(function(e) {
    e.preventDefault();
    var url = $(this).attr("href");
    $("#ModAddPhysicalProgress iframe").attr("src", url);
    $("#ModAddPhysicalProgress").modal("show");
          });
   $(".showModalEditPhysicalProgress").click(function(e) {
    e.preventDefault();
    var url = $(this).attr("href");
    $("#ModEditPhysicalProgress iframe").attr("src", url);
    $("#ModEditPhysicalProgress").modal("show");
  });
});
</script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="default_master_body" runat="server">
       <div class="container-fluid">
        <div class="row">
            <div class="col-lg-6 col-xl-8 form-group">Other points for discussion</div>
            <div class="col-lg-6 col-xl-4 form-group">
                
            </div>
        </div>
    </div>
     <div class="container-fluid">
        <div class="row">
            <div class="col-lg-6 col-xl-12 form-group">
                <div class="card mb-4">
                    <div class="card-body">
                        <div class="card-title">
                            <div class="d-flex justify-content-between">
                              
                                <div>
                                    <a href="/_modal_pages/add-OtherPointsforDiscussion.aspx" id="PhysicalProgress" runat="server" class="showModalAddPhysicalProgress"><asp:Button ID="btnadd" runat="server" Text="+ Add Points for discussion" CssClass="btn btn-primary"></asp:Button></a>
                                </div>
                            </div>
                        </div>
                        <div class="table-responsive">
                            <label class="lblCss" for="txtxsummary">Select Meeting </label>
                            <asp:DropDownList ID="DDLMeetingMaster" runat="server" CssClass="form-control" Width="30%" AutoPostBack="true" OnSelectedIndexChanged="DDLMeetingMaster_SelectedIndexChanged">

                            </asp:DropDownList>
                                <br />
                            <asp:Button ID="btncopy" runat="server" Text="Copy Data from Previous Report" OnClientClick="return CopyMessage()" CssClass="btn btn-primary" Visible="false" OnClick="btncopy_Click"/>
                          <br /><br />
                            <asp:GridView ID="grdOtherPoins" runat="server" CssClass="table table-bordered" AutoGenerateColumns="False" EmptyDataText="No Data" Width="100%" OnRowCommand="grdOtherPoins_RowCommand" OnRowDeleting="grdOtherPoins_RowDeleting">
                                <Columns>
                                  
                              
                                    <asp:TemplateField HeaderText="Other Points for discussion">
                                        <ItemTemplate>
                                          <li>  <%#Eval("points")%></li>
                                            <%--<asp:HiddenField ID="hiduid" runat="server" value=<%#Eval("uid")%> />--%>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField>
                                        <ItemTemplate>
                                            <a id="EditPhysicalProgress" href="../../_modal_pages/add-OtherPointsforDiscussion.aspx?uid=<%#Eval("uid")%>" class="showModalEditPhysicalProgress"><span title="Edit" class="fas fa-edit"></span></a>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField>
                                  <ItemTemplate>
                                         <asp:LinkButton ID="lnkdelete" runat="server" OnClientClick="return DeleteItem()" CausesValidation="false" CommandArgument='<%#Eval("uid")%>' CommandName="delete"><span title="Delete" class="fas fa-trash"></span></asp:LinkButton>
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

    <%--Add Physical Progress--%>
    <div id="ModAddPhysicalProgress" class="modal it-modal fade">
	    <div class="modal-dialog modal-dialog-centered modal-dialog-scrollable modal-xl">
		    <div class="modal-content">
			    <div class="modal-header">
				    <h5 class="modal-title">Add Other points for discussion</h5>
                    <button aria-label="Close" class="close" data-dismiss="modal" type="button"><span aria-hidden="true">&times;</span></button>
			    </div>
			    <div class="modal-body">
                    <iframe class="border-0 w-100" style="height:480px;" loading="lazy"></iframe>
			    </div>
		    </div>
	    </div>
    </div>

    <%--add work package category modal--%>
    <div id="ModEditPhysicalProgress" class="modal it-modal fade">
	    <div class="modal-dialog modal-dialog-centered modal-dialog-scrollable modal-lg">
		    <div class="modal-content">
			    <div class="modal-header">
				    <h5 class="modal-title">Edit Other points for discussion</h5>
                    <button aria-label="Close" class="close" data-dismiss="modal" type="button"><span aria-hidden="true">&times;</span></button>
			    </div>
			    <div class="modal-body">
                    <iframe class="border-0 w-100" style="height:480px;" loading="lazy"></iframe>
			    </div>
		    </div>
	    </div>
    </div>
</asp:Content>
