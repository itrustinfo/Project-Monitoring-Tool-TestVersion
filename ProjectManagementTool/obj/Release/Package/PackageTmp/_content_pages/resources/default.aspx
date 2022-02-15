<%@ Page Title="" Language="C#" MasterPageFile="~/_master_pages/default.Master" AutoEventWireup="true" CodeBehind="default.aspx.cs" Inherits="ProjectManagementTool._content_pages.resources._default" %>
<asp:Content ID="Content1" ContentPlaceHolderID="default_master_head" runat="server">
    <script type="text/javascript">
        $(document).ready(function () {
            //$('#loader').fadeOut();

            $(".showResourceModal").click(function(e) {
            e.preventDefault();
            var url = $(this).attr("href");
            $("#ModAddResource iframe").attr("src", url);
            $("#ModAddResource").modal("show");
            });

            $(".showEditResourceModal").click(function(e) {
            e.preventDefault();
            var url = $(this).attr("href");
            $("#ModEditResource iframe").attr("src", url);
            $("#ModEditResource").modal("show");
            });

            $(".showPropertyModal").click(function(e) {
            e.preventDefault();
            var url = $(this).attr("href");
            $("#ModAddProperty iframe").attr("src", url);
            $("#ModAddProperty").modal("show");
            });
        });
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="default_master_body" runat="server">
    <%--<div id="loader"></div>--%>
    <div class="container-fluid">
            <div class="row">
                <div class="col-md-12 col-lg-4 form-group">Resources</div>
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
                                   <asp:Label ID="ActivityHeading" CssClass="text-uppercase font-weight-bold" runat="server" Text="Master Resource List" />
                                       <%--<asp:Label Text="Foo bar" CssClass="pl-1" runat="server" />--%>
                               </h6>
                                <div>
                                   <a id="HLAdd" runat="server" href="/_modal_pages/add-resourcemaster.aspx" class="showResourceModal"><asp:Button ID="Button1" runat="server" Text="+ Add Resource" align="end" CssClass="btn btn-primary"></asp:Button></a>
                                </div>
                          </div>
                            </div>
                        <div class="table-responsive">
                  <asp:GridView ID="grdResources" runat="server" AutoGenerateColumns="False" Width="100%" PageSize="15" CssClass="table table-bordered" OnPageIndexChanging="grdResources_PageIndexChanging">
                    <Columns>
                        <asp:BoundField DataField="ResourceName" HeaderText="Resource Name" >
                        </asp:BoundField>
                        <asp:BoundField DataField="Unit_for_Measurement" HeaderText="Unit for Measurement">
                        <HeaderStyle HorizontalAlign="Center" />
                        </asp:BoundField>

                        <asp:BoundField DataField="CostType" HeaderText="Cost Type"  >
                        
                        </asp:BoundField>
                        <asp:TemplateField HeaderText="Basic Cost">
                            <ItemTemplate>
                                <span style="color:#006699;"><%#Eval("Currency")%></span>&nbsp;<%#string.Format(new System.Globalization.CultureInfo(Eval("Currency_CultureInfo").ToString()),"{0:N}", Eval("Basic_Budget"))%>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:BoundField DataField="GST" HeaderText="GST" >
                        </asp:BoundField>
                        <asp:TemplateField HeaderText="Total Cost">
                            <ItemTemplate>
                                <span style="color:#006699;"><%#Eval("Currency")%></span>&nbsp;<%#string.Format(new System.Globalization.CultureInfo(Eval("Currency_CultureInfo").ToString()),"{0:N}", Eval("Total_Budget"))%>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <%--<asp:TemplateField>
                            <ItemTemplate>
                                <a class="showPropertyModal" href="/_modal_pages/add-resourceproperty.aspx?ResourceUID=<%#Eval("ResourceUID")%>&ResourceType_UID=<%#Eval("ResourceType_UID")%>&ProjectUID=<%#Eval("ProjectUID")%>">Add Properties</a>
                            </ItemTemplate>
                        </asp:TemplateField>--%>
                        <asp:TemplateField>
                            <ItemTemplate>
                                <a class="showEditResourceModal" href="/_modal_pages/add-resourcemaster.aspx?type=edit&ResourceUID=<%#Eval("ResourceUID")%>&WorkPackageUID=<%#Eval("WorkPackageUID")%>&ProjectUID=<%#Eval("ProjectUID")%>"><span title="Edit" class="fas fa-edit"></span></a>
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

    <%--add resource modal--%>
    <div id="ModAddResource" class="modal it-modal fade">
	    <div class="modal-dialog modal-dialog-centered modal-dialog-scrollable modal-xl">
		    <div class="modal-content">
			    <div class="modal-header">
				    <h5 class="modal-title">Add Resource</h5>
                    <button aria-label="Close" class="close" data-dismiss="modal" type="button"><span aria-hidden="true">&times;</span></button>
			    </div>
			    <div class="modal-body">
                    <iframe class="border-0 w-100" style="height: 460px;" loading="lazy"></iframe>
			    </div>
              
		    </div>
	    </div>
    </div>

    <%--add resource modal--%>
    <div id="ModEditResource" class="modal it-modal fade">
	    <div class="modal-dialog modal-dialog-centered modal-dialog-scrollable modal-xl">
		    <div class="modal-content">
			    <div class="modal-header">
				    <h5 class="modal-title">Edit Resource</h5>
                    <button aria-label="Close" class="close" data-dismiss="modal" type="button"><span aria-hidden="true">&times;</span></button>
			    </div>
			    <div class="modal-body">
                    <iframe class="border-0 w-100" style="height: 460px;" loading="lazy"></iframe>
			    </div>
              
		    </div>
	    </div>
    </div>

    <%--add property modal--%>
    <div id="ModAddProperty" class="modal it-modal fade">
	    <div class="modal-dialog modal-dialog-centered modal-dialog-scrollable modal-xl">
		    <div class="modal-content">
			    <div class="modal-header">
				    <h5 class="modal-title">Add Property</h5>
                    <button aria-label="Close" class="close" data-dismiss="modal" type="button"><span aria-hidden="true">&times;</span></button>
			    </div>
			    <div class="modal-body">
                    <iframe class="border-0 w-100" style="height:450px;" loading="lazy"></iframe>
			    </div>
              
		    </div>
	    </div>
    </div>
</asp:Content>
