<%@ Page Title="" Language="C#" MasterPageFile="~/_master_pages/default.Master" AutoEventWireup="true" CodeBehind="default.aspx.cs" Inherits="ProjectManagementTool._content_pages.document_history._default" %>
<asp:Content ID="Content1" ContentPlaceHolderID="default_master_head" runat="server">
    <script type="text/javascript">
        function BindEvents() {
        $(".showUserDocumentHistoryModal").click(function(e) {
        e.preventDefault();
        var url = $(this).attr("href");
        $("#ModUserDocumentHistory iframe").attr("src", url);
        $("#ModUserDocumentHistory").modal("show");
        });
        }
        $(document).ready(function () {
            BindEvents();
            //$('#loader').fadeOut();
        });
</script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="default_master_body" runat="server">
       <%--project selection dropdowns--%>
    <%--<div id="loader"></div>--%>
        <div class="container-fluid">
            <div class="row">
                <div class="col-md-12 col-lg-2 form-group">History of Accessed Documents</div>
                <div class="col-md-6 col-lg-5 form-group">
                    <label class="sr-only" for="DDLProject">Project</label>
                    <div class="input-group">
                        <div class="input-group-prepend">
                            <span class="input-group-text">Project</span>
                        </div>
                        <asp:DropDownList ID="DDlProject" runat="server" CssClass="form-control" AutoPostBack="true" OnSelectedIndexChanged="DDlProject_SelectedIndexChanged"></asp:DropDownList>
                    </div>
                </div>
                <div class="col-md-6 col-lg-5 form-group" id="WorkPackageDropdown" runat="server">
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
    <div class="container-fluid" id="DivActualDocuments" runat="server">
        <div class="row">
            <div class="col-lg-12 col-xl-12 form-group">
                <div class="card mb-4">
                    <div class="card-body">
                        <div class="card-title">
                        <div class="d-flex justify-content-between">
                              <h6 class="text-muted">
                                   <asp:Label ID="Label1" CssClass="text-uppercase font-weight-bold" runat="server" Text="List of Accessed Documents"  />
                                       <%--<asp:Label Text="Foo bar" CssClass="pl-1" runat="server" />--%>
                               </h6>
                                <div>
                                   
                                </div>
                          </div>
                            </div>
                        <div class="table-responsive">
                         <asp:GridView ID="GrdDcumentHsitroy" EmptyDataText="No Documents Found." runat="server" Width="100%" AutoGenerateColumns="false" CellPadding="6" CellSpacing="16" 
                                            AllowPaging="true" PageSize="15" DataKeyNames="DocumentUID" CssClass="table table-bordered" OnPageIndexChanging="GrdDcumentHsitroy_PageIndexChanging" OnRowDataBound="GrdDcumentHsitroy_RowDataBound">
                                       <Columns>
                                           <asp:TemplateField HeaderText="Activity Name">
                                            <ItemTemplate>
                                                <%#GetActivityName(Eval("ActivityUID").ToString())%>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                           <asp:TemplateField HeaderText="Document">
                                            <ItemTemplate>
                                                <%#GetDocumentname(Eval("DocumentUID").ToString())%>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                           <asp:BoundField DataField="DocumentUID" HeaderText="Downloaded">
                                                                    <HeaderStyle HorizontalAlign="Left" />
                                                                    </asp:BoundField>
                                            <asp:BoundField DataField="DocumentUID" HeaderText="Viewed">
                                                                    <HeaderStyle HorizontalAlign="Left" />
                                                                    </asp:BoundField>
                                           <asp:TemplateField HeaderText="Users">
                                            <ItemTemplate>
                                            <a class="showUserDocumentHistoryModal" href="/_modal_pages/user-documenthistory.aspx?DocumentUID=<%#Eval("DocumentUID")%>"><span title="Users" class="fas fa-user-cog"></span></a>
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
    <div class="container-fluid" id="DivGeneralDocuments" runat="server">
        <div class="row">
            <div class="col-lg-12 col-xl-12 form-group">
                <div class="card mb-4">
                    <div class="card-body">
                        <div class="card-title">
                        <div class="d-flex justify-content-between">
                              <h6 class="text-muted">
                                   <asp:Label ID="Label2" CssClass="text-uppercase font-weight-bold" runat="server" Text="List of Accessed General Documents"  />
                                       <%--<asp:Label Text="Foo bar" CssClass="pl-1" runat="server" />--%>
                               </h6>
                                <div>
                                   
                                </div>
                          </div>
                            </div>
                        <div class="table-responsive">
                         <asp:GridView ID="GrdGeneralDocuments" EmptyDataText="No Documents Found." runat="server" Width="100%" AutoGenerateColumns="false" CellPadding="6" CellSpacing="16" 
                                            AllowPaging="true" PageSize="15" DataKeyNames="DocumentUID" CssClass="table table-bordered" OnPageIndexChanging="GrdGeneralDocuments_PageIndexChanging" OnRowDataBound="GrdGeneralDocuments_RowDataBound">
                                       <Columns>
                                           <asp:TemplateField HeaderText="Document">
                                            <ItemTemplate>
                                                <%#GetGeneraDocumentName(Eval("DocumentUID").ToString())%>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                           <asp:BoundField DataField="DocumentUID" HeaderText="Downloaded">
                                                                    <HeaderStyle HorizontalAlign="Left" />
                                                                    </asp:BoundField>
                                            <asp:BoundField DataField="DocumentUID" HeaderText="Viewed">
                                                                    <HeaderStyle HorizontalAlign="Left" />
                                                                    </asp:BoundField>
                                           <asp:TemplateField HeaderText="Users">
                                            <ItemTemplate>
                                            <a class="showUserDocumentHistoryModal" href="/_modal_pages/user-documenthistory.aspx?DocumentUID=<%#Eval("DocumentUID")%>"><span title="Users" class="fas fa-user-cog"></span></a>
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
    <%--View User Document histroy modal--%>
    <div id="ModUserDocumentHistory" class="modal it-modal fade">
	    <div class="modal-dialog modal-dialog-centered modal-dialog-scrollable modal-lg">
		    <div class="modal-content">
			    <div class="modal-header">
				    <h5 class="modal-title">User History</h5>
                    <button aria-label="Close" class="close" data-dismiss="modal" type="button"><span aria-hidden="true">&times;</span></button>
			    </div>
			    <div class="modal-body">
                    <iframe class="border-0 w-100" style="height:500px;" loading="lazy"></iframe>
			    </div>
              
		    </div>
	    </div>
    </div>
</asp:Content>
