<%@ Page Title="" Language="C#" MasterPageFile="~/_master_pages/default.Master" AutoEventWireup="true" CodeBehind="default.aspx.cs" Inherits="ProjectManagementTool._content_pages.work_packages._default" %>
<asp:Content ID="Content1" ContentPlaceHolderID="default_master_head" runat="server">
    <style type="text/css">
        .hide
        {
            display:none;
        }
    </style>
  <script type="text/javascript">
      function DeleteItem() {
            if (confirm("All data associated with this workpackage will be deleted.Are you sure you want to delete ...?")) {
                return true;
            }
            return false;
    }
  </script>
  <script>
      $(document).ready(function() {
  $(".showModal").click(function(e) {
    e.preventDefault();
    var url = $(this).attr("href");
    $("#ModAddWorkpackages iframe").attr("src", url);
    $("#ModAddWorkpackages").modal("show");
  });

             $(".showAddProjectMasterModal").click(function(e) {
        e.preventDefault();
        var url = $(this).attr("href");
        $("#ModAddProjectMaster iframe").attr("src", url);
        $("#ModAddProjectMaster").modal("show");
             });

            $(".showAddMasterModal").click(function(e) {
        e.preventDefault();
        var url = $(this).attr("href");
        $("#ModAddMasterData iframe").attr("src", url);
        $("#ModAddMasterData").modal("show");
          });

   $(".showModalcategory").click(function(e) {
    e.preventDefault();
    var url = $(this).attr("href");
    $("#ModAddWorkpackageCategory iframe").attr("src", url);
    $("#ModAddWorkpackageCategory").modal("show");
  });
});
</script>
   
    <%--<script src="https://ajax.googleapis.com/ajax/libs/jquery/1.12.4/jquery.min.js"></script>
<script src="https://maxcdn.bootstrapcdn.com/bootstrap/3.3.7/js/bootstrap.min.js"></script>--%>

</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="default_master_body" runat="server">
    <%--project selection dropdowns--%>
    <div class="container-fluid">
        <div class="row">
            <div class="col-lg-6 col-xl-8 form-group">Work Packages</div>
            <div class="col-lg-6 col-xl-4 form-group">
                <label class="sr-only" for="DDLProject">Project</label>
                <div class="input-group">
                    <div class="input-group-prepend">
                        <span class="input-group-text">Project</span>
                    </div>
                    <asp:DropDownList CssClass="form-control" ID="DDLProject" runat="server" AutoPostBack="true" OnSelectedIndexChanged="DDLProject_SelectedIndexChanged"></asp:DropDownList>
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
                                    <asp:Label Text="List of Work Packages" CssClass="text-uppercase font-weight-bold" runat="server" />
                                    <%--<asp:Label Text="Foo bar" CssClass="pl-1" runat="server" />--%>
                                </h6>
                                <div>
                                    
                                    <a href="/_modal_pages/add-workpackage.aspx" id="Workpkg" runat="server" class="showModal"><asp:Button ID="btnadd" runat="server" Text="+ Add WorkPackage" CssClass="btn btn-primary"></asp:Button></a>
                                     <a href="/_modal_pages/add-prjmaster-mail-settings.aspx" class="showAddProjectMasterModal"><asp:Button ID="btnaddmail" runat="server" Text="Update WorkPackage Master Data Settings" CssClass="btn btn-primary"></asp:Button></a>
                                    
                                </div>
                            </div>
                        </div>
                        <div class="table-responsive">

                            <asp:GridView ID="GrdWorkPackage" runat="server" CssClass="table table-bordered" AutoGenerateColumns="False" PageSize="10" EmptyDataText="No Data" Width="100%" OnPageIndexChanging="GrdWorkPackage_PageIndexChanging" OnRowCommand="GrdWorkPackage_RowCommand" OnRowDeleting="GrdWorkPackage_RowDeleting" OnRowDataBound="GrdWorkPackage_RowDataBound">
                                <Columns>
                                    <asp:TemplateField HeaderText="Project Name">
                                        <ItemTemplate>
                                            <%#GetProjectName(Eval("ProjectUID").ToString())%>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:BoundField DataField="Name" HeaderText="WorkPackage Name">
                            <HeaderStyle HorizontalAlign="Left" />
                            </asp:BoundField>
                                    <asp:TemplateField HeaderText="Budget">
                                        <ItemTemplate>
                                             <div id="divB" runat="server" visible="false">
                                            <a id="Add" href="/_modal_pages/add-wkpgmaster-data.aspx?WorkPackageUID=<%#Eval("WorkPackageUID")%>&type=budget" class="showAddMasterModal"><span style="color:#006699;"><%#Eval("Currency")%></span>&nbsp;<%#string.Format(new System.Globalization.CultureInfo(Eval("Currency_CultureInfo").ToString()),"{0:N}", Eval("Budget"))%></a>
                                            <a style="color:maroon !important" href="/_modal_pages/view-masterdata-history.aspx?WorkPackageUID=<%#Eval("WorkPackageUID")%>&type=budget" class="showModalcategory">(H)</a></div>
                                             <div id="divB2" runat="server" visible="true">
                                            <span style="color:#006699;"><%#Eval("Currency")%></span>&nbsp;<%#string.Format(new System.Globalization.CultureInfo(Eval("Currency_CultureInfo").ToString()),"{0:N}", Eval("Budget"))%>
                                            <a style="color:maroon !important" href="/_modal_pages/view-masterdata-history.aspx?WorkPackageUID=<%#Eval("WorkPackageUID")%>&type=budget" class="showModalcategory">(H)</a></div>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                   <%-- <asp:BoundField DataField="Budget" HeaderText="Budget">
                            <HeaderStyle HorizontalAlign="Left" />
                            </asp:BoundField>--%>
                                       <%--<asp:BoundField DataField="ActualExpenditure" HeaderText="Actual Expenditure" >
                            <HeaderStyle HorizontalAlign="Left" />
                            </asp:BoundField>--%>
                                    <asp:TemplateField HeaderText="Actual Expenditure">
                                        <ItemTemplate>
                                            <div id="divR" runat="server" visible="false">
                                            <a  href="/_modal_pages/add-wkpgmaster-data.aspx?WorkPackageUID=<%#Eval("WorkPackageUID")%>&type=expenditure" class="showAddMasterModal"><span style="color:#006699;"><%#Eval("Currency")%></span>&nbsp;<%#string.Format(new System.Globalization.CultureInfo(Eval("Currency_CultureInfo").ToString()),"{0:N}", Eval("ActualExpenditure"))%></a>
                                            <a  style="color:maroon !important" href="/_modal_pages/view-masterdata-history.aspx?WorkPackageUID=<%#Eval("WorkPackageUID")%>&type=expenditure" class="showModalcategory">(H)</a></div>
                                              <div id="divR2" runat="server" visible="true">
                                            <span style="color:#006699;"><%#Eval("Currency")%></span>&nbsp;<%#string.Format(new System.Globalization.CultureInfo(Eval("Currency_CultureInfo").ToString()),"{0:N}", Eval("ActualExpenditure"))%>
                                            <a  style="color:maroon !important" href="/_modal_pages/view-masterdata-history.aspx?WorkPackageUID=<%#Eval("WorkPackageUID")%>&type=expenditure" class="showModalcategory">(H)</a></div>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:BoundField DataField="StartDate" HeaderText="Start Date"  DataFormatString="{0:dd MMM yyyy}">
                            <HeaderStyle HorizontalAlign="Left" />
                            </asp:BoundField>
                                     <asp:BoundField DataField="PlannedEndDate" HeaderText="Planned EndDate"  DataFormatString="{0:dd MMM yyyy}">
                            <HeaderStyle HorizontalAlign="Left" />
                            </asp:BoundField>
                                     
                                          <asp:TemplateField HeaderText="Projected EndDate">
                                        <ItemTemplate>
                                            <div id="divE" runat="server" visible="false">
                                            <a href="/_modal_pages/add-wkpgmaster-data.aspx?WorkPackageUID=<%#Eval("WorkPackageUID")%>&type=enddate" class="showAddMasterModal"><span style="color:#006699;"><%#Eval("ProjectedEndDate","{0:dd MMM yyyy}")%></span></a>
                                            <a style="color:maroon !important" href="/_modal_pages/view-masterdata-history.aspx?WorkPackageUID=<%#Eval("WorkPackageUID")%>&type=enddate" class="showModalcategory">(H)</a></div>
                                            <div id="divE2" runat="server" visible="true">
                                            <span style="color:#006699;"><%#Eval("ProjectedEndDate","{0:dd MMM yyyy}")%></span></a>
                                            <a style="color:maroon !important" href="/_modal_pages/view-masterdata-history.aspx?WorkPackageUID=<%#Eval("WorkPackageUID")%>&type=enddate" class="showModalcategory">(H)</a></div>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                             
                         <asp:TemplateField HeaderText="Status">
                             <ItemTemplate>
                                 <%#GetStatus(Eval("Status").ToString())%>
                             </ItemTemplate>
                         </asp:TemplateField>
                            <%--<asp:BoundField DataField="Status" HeaderText="Status">
                            <HeaderStyle HorizontalAlign="Left" />
                            </asp:BoundField>--%>

                             
                                     <asp:TemplateField>
                                        <ItemTemplate>
                                            <a id="AddCategory" href="/_modal_pages/add-workpackagecategory.aspx?WorkPackageUID=<%#Eval("WorkPackageUID")%>&wName=<%#Eval("Name")%>" class="showModalcategory">Add Categories</a>
                                        </ItemTemplate>
                                    </asp:TemplateField>

                                    <asp:TemplateField>
                                        <ItemTemplate>
                                            <a id="EditWorkPackage" href="/_modal_pages/add-workpackage.aspx?WorkPackageUID=<%#Eval("WorkPackageUID")%>&ProjectUID=<%#Eval("ProjectUID")%>" class="showModal"><span title="Edit" class="fas fa-edit"></span></a>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField>
                                        <ItemTemplate>
                                             <asp:LinkButton ID="lnkdelete" runat="server" OnClientClick="return DeleteItem()" CausesValidation="false" CommandArgument='<%#Eval("WorkPackageUID")%>' CommandName="delete"><span title="Delete" class="fas fa-trash"></span></asp:LinkButton>
                                         </ItemTemplate>
                                     </asp:TemplateField>
                                     <asp:TemplateField>
                                        <ItemTemplate>
                                            <asp:LinkButton ID="lnkUp" CommandName="up" CommandArgument='<%#Eval("WorkPackageUID")%>'  runat="server" ><span title="Up" class="fas fa-chevron-circle-up"></span></asp:LinkButton>
                                            <asp:LinkButton ID="lnkDown" CommandName="down" CommandArgument='<%#Eval("WorkPackageUID")%>'  runat="server"><span title="Down" class="fas fa-chevron-circle-down"></span></asp:LinkButton>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                      <asp:BoundField DataField="WorkPackageUID" HeaderText="WorkPackageUID">
                            <HeaderStyle HorizontalAlign="Left" CssClass="hide"/>
                                          <ItemStyle CssClass="hide" />
                            </asp:BoundField>
                                </Columns>
                             </asp:GridView>
                        </div>
                    </div>
                </div>
            </div>
            </div>
        </div>

     <%--add work package modal--%>
    <div id="ModAddWorkpackages" class="modal it-modal fade">
	    <div class="modal-dialog modal-dialog-centered modal-dialog-scrollable modal-xl">
		    <div class="modal-content">
			    <div class="modal-header">
				    <h5 class="modal-title">Add Work Package</h5>
                    <button aria-label="Close" class="close" onclick="javascript:window.location.reload()" data-dismiss="modal" type="button"><span aria-hidden="true">&times;</span></button>
			    </div>
			    <div class="modal-body">
                    <iframe class="border-0 w-100" style="height: 500px;" loading="lazy"></iframe>
			    </div>
                <%--<div class="modal-footer">
                    <button type="button" class="btn btn-secondary" data-dismiss="modal">Close</button>
                    <button type="button" class="btn btn-primary">Save changes</button>
                </div>--%>
		    </div>
	    </div>
    </div>

    <%--add work package category modal--%>
    <div id="ModAddWorkpackageCategory" class="modal it-modal fade">
	    <div class="modal-dialog modal-dialog-centered modal-dialog-scrollable modal-lg">
		    <div class="modal-content">
			    <div class="modal-header">
				    <h5 class="modal-title">View Data History</h5>
                    <button aria-label="Close" class="close" onclick="javascript:window.location.reload()" data-dismiss="modal" type="button"><span aria-hidden="true">&times;</span></button>
			    </div>
			    <div class="modal-body">
                    <iframe class="border-0 w-100" style="height:500px;" loading="lazy"></iframe>
			    </div>
                <%--<div class="modal-footer">
                    <button type="button" class="btn btn-secondary" data-dismiss="modal">Close</button>
                    <button type="button" class="btn btn-primary">Save changes</button>
                </div>--%>
		    </div>
	    </div>
    </div>

     <%--add Project Class modal--%>
    <div id="ModAddProjectMaster" class="modal it-modal fade">
	    <div class="modal-dialog modal-dialog-centered modal-dialog-scrollable modal-lg">
		    <div class="modal-content">
			    <div class="modal-header">
				    <h5 class="modal-title">Update WorkPackage Master Data Settings</h5>
                    <button aria-label="Close" class="close" data-dismiss="modal" type="button"><span aria-hidden="true">&times;</span></button>
			    </div>
			    <div class="modal-body">
                    <iframe class="border-0 w-100" style="height:400px;" loading="lazy"></iframe>
			    </div>
              
		    </div>
	    </div>
    </div>

    <div id="ModAddMasterData" class="modal it-modal fade">
	    <div class="modal-dialog modal-dialog-centered modal-dialog-scrollable modal-lg">
		    <div class="modal-content">
			    <div class="modal-header">
				    <h5 class="modal-title">Update Data</h5>
                    <button aria-label="Close" class="close" data-dismiss="modal" type="button"><span aria-hidden="true">&times;</span></button>
			    </div>
			    <div class="modal-body">
                    <iframe class="border-0 w-100" style="height:400px;" loading="lazy"></iframe>
			    </div>
              
		    </div>
	    </div>
    </div>
    </asp:Content>
