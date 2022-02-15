<%@ Page Title="" Language="C#" MasterPageFile="~/_master_pages/default.Master" AutoEventWireup="true" CodeBehind="default.aspx.cs" Inherits="ProjectManagementTool._content_pages.choose_sitephotographs._default" %>
<asp:Content ID="Content1" ContentPlaceHolderID="default_master_head" runat="server">
    <script>
      $(document).ready(function() {
  $(".showModalAddSitePhotos").click(function(e) {
    e.preventDefault();
    var url = $(this).attr("href");
    $("#ModAddSitePhotos iframe").attr("src", url);
    $("#ModAddSitePhotos").modal("show");
          });
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
                <div class="col-md-12 col-lg-3 form-group">Site Photographs</div>
               
                <div class="col-md-6 col-lg-4 form-group">
                    <label class="sr-only" for="DDLProject">Project</label>
                    <div class="input-group">
                        <div class="input-group-prepend">
                            <span class="input-group-text">Project</span>
                        </div>
                        <asp:DropDownList ID="DDlProject" runat="server" CssClass="form-control"></asp:DropDownList>
                    </div>
                </div>
                <div class="col-md-6 col-lg-4 form-group">
                    <label class="sr-only" for="ddlMeeting">Meeting</label>
                    <div class="input-group">
                        <div class="input-group-prepend">
                            <span class="input-group-text">Meeting</span>
                        </div>
                        <asp:DropDownList ID="ddlMeeting" runat="server" class="form-control"></asp:DropDownList>
                    </div>
                </div>
                <div class="col-md-12 col-lg-1 form-group">
                    <asp:Button ID="btnView" runat="server" Text="View" CssClass="btn btn-primary" OnClick="btnView_Click" />
                </div>
            </div>
        </div>
    <div class="container-fluid" id="SitePhotGraphs" runat="server">
        <div class="row" >
            <div class="col-lg-12 col-xl-12 form-group">
                <div class="card mb-4">
                    <div class="card-body" >
                        <div class="card-title">
                            <div class="d-flex justify-content-between">
                                <h6 class="text-muted">
                                    <asp:Label ID="LblSitePhotograph" CssClass="text-uppercase font-weight-bold" runat="server" Text="Site PhotoGraphs"  />
                                    <%--<asp:Label Text="Foo bar" CssClass="pl-1" runat="server" />--%>
                                </h6>
                                <div>
                                    <a id="addPhoto" runat="server" href="/_modal_pages/add-sitephotographs.aspx" class="showModalAddSitePhotos"><asp:Button ID="btnadd" runat="server" Text="+ Add Site Photographs" CssClass="btn btn-primary"></asp:Button></a>
                                </div>
                            </div>

                            
                        </div>
                        <div class="table-responsive">
                            <asp:DataList ID="GrdSitePhotograph" runat="server" DataKeyField="MeetingPhotoGraphs" RepeatColumns="4" HorizontalAlign="Center" CellPadding="10" RepeatDirection="Horizontal" OnDeleteCommand="GrdSitePhotograph_DeleteCommand">
                                <ItemTemplate>
                                    <div style="width:275px; float:left; border:1px solid Gray; text-align:center; background-color:#f2f2f2;">
                                        <%--<asp:CheckBox ID="ChkPhoto" runat="server" />--%>
                                        <div style="padding:10px;">
                                            <%--<img src='http://localhost:50162<%#Eval("Site_Image").ToString().Replace("~","")%>' alt="" width="250" />--%>
                                            <asp:Image ID="imgEmp" runat="server" Width="225px" ImageUrl='<%# Bind("Site_Image", "{0}")%>' /><br />
                                        <b><asp:Label ID="LblDescription" runat="server" Text='<%#Eval("Description")%>'></asp:Label></b>
                                        </div>
                                        <asp:LinkButton ID="lnkdelete" runat="server" OnClientClick="return DeleteItem()" CausesValidation="false" CommandName="delete"><span title="Delete" class="fas fa-trash"></span></asp:LinkButton>
                                    </div>
                                    </ItemTemplate>
                            </asp:DataList>
                            <asp:Label ID="LblMessage" runat="server" class="lblCss"  Text="No Site Photograph/s Found.."></asp:Label>
                            </div>
                        <%--<div class="text-right">
                            <asp:Button ID="btnSubmit" runat="server" Text="Submit" CssClass="btn btn-primary" OnClick="btnSubmit_Click" />
                        </div>--%>
                        </div>
                    </div>
                </div>
            </div>
        </div>
    <%--Add Site Photos--%>
    <div id="ModAddSitePhotos" class="modal it-modal fade">
	    <div class="modal-dialog modal-dialog-centered modal-dialog-scrollable modal-xl">
		    <div class="modal-content">
			    <div class="modal-header">
				    <h5 class="modal-title">Add Site Photographs</h5>
                    <button aria-label="Close" class="close" data-dismiss="modal" type="button"><span aria-hidden="true">&times;</span></button>
			    </div>
			    <div class="modal-body">
                    <iframe class="border-0 w-100" style="height:480px;" loading="lazy"></iframe>
			    </div>
		    </div>
	    </div>
    </div>
</asp:Content>
