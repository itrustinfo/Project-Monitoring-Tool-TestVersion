<%@ Page Title="" Language="C#" MasterPageFile="~/_master_pages/default.Master" AutoEventWireup="true" CodeBehind="default.aspx.cs" Inherits="ProjectManagementTool._content_pages.boq_details._default" %>
<asp:Content ID="Content1" ContentPlaceHolderID="default_master_head" runat="server">
    <script type="text/javascript">
        $(document).ready(function () {
            $(".showAddBOQ").click(function(e) {
            e.preventDefault();
            var url = $(this).attr("href");
            $("#ModAddBOQ iframe").attr("src", url);
            $("#ModAddBOQ").modal("show");
            });

            $(".showEditBOQ").click(function(e) {
            e.preventDefault();
            var url = $(this).attr("href");
            $("#ModEditBOQ iframe").attr("src", url);
            $("#ModEditBOQ").modal("show");
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
    <%--<div id="loader"></div>--%>
    <div class="container-fluid">
            <div class="row">
                <div class="col-md-12 col-lg-4 form-group">BOQ Details</div>
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
                                   <asp:Label ID="ActivityHeading" CssClass="text-uppercase font-weight-bold" runat="server" Text="Master BOQ Details" />
                                       <%--<asp:Label Text="Foo bar" CssClass="pl-1" runat="server" />--%>
                               </h6>
                                <div>
                                   <a id="AddBOQ" runat="server" href="/_modal_pages/add-boq.aspx" class="showAddBOQ"><asp:Button ID="btnAdd" runat="server" Text="+ Add BOQ" align="end" CssClass="btn btn-primary"></asp:Button></a>
                                </div>
                          </div>
                            </div>
                        <div class="table-responsive">
                  <asp:GridView ID="GrdBOQDetails" runat="server" AutoGenerateColumns="False" Width="100%" PageSize="15" CssClass="table table-bordered" OnPageIndexChanging="GrdBOQDetails_PageIndexChanging" OnRowCommand="GrdBOQDetails_RowCommand" OnRowDeleting="GrdBOQDetails_RowDeleting">
                    <Columns>
                        <asp:BoundField DataField="Item_Number" HeaderText="Item Number" >
                        </asp:BoundField>
                        <asp:BoundField DataField="Description" HeaderText="Description">                        
                        </asp:BoundField>
                        <asp:BoundField DataField="Unit" HeaderText="Unit" >
                        </asp:BoundField>
                        <asp:BoundField DataField="Quantity" HeaderText="Quantity">
                        </asp:BoundField>
                        <asp:BoundField DataField="GST" HeaderText="GST" >
                        </asp:BoundField>
                        <asp:TemplateField HeaderText="Price">
                            <ItemTemplate>
                                <span style="color:#006699;"><%#Eval("Currency")%></span>&nbsp;<%#string.Format(new System.Globalization.CultureInfo(Eval("Currency_CultureInfo").ToString()),"{0:N}", Eval("Price"))%>
                            </ItemTemplate>
                        </asp:TemplateField>
                        
                        <asp:TemplateField>
                            <ItemTemplate>
                                <a class="showEditBOQ" href="/_modal_pages/add-boq.aspx?type=edit&BOQDetailsUID=<%#Eval("BOQDetailsUID")%>&WorkPackageUID=<%#Eval("WorkpackageUID")%>"><span title="Edit" class="fas fa-edit"></span></a>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField>
                              <ItemTemplate>
                                           <asp:LinkButton ID="lnkdelete" runat="server" OnClientClick="return DeleteItem()" CausesValidation="false" CommandArgument='<%#Eval("BOQDetailsUID")%>' CommandName="delete"><span title="Delete" class="fas fa-trash"></span></asp:LinkButton>
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

    <%--add BOQ modal--%>
    <div id="ModAddBOQ" class="modal it-modal fade">
	    <div class="modal-dialog modal-dialog-centered modal-dialog-scrollable modal-xl">
		    <div class="modal-content">
			    <div class="modal-header">
				    <h5 class="modal-title">Add BOQ</h5>
                    <button aria-label="Close" class="close" data-dismiss="modal" type="button"><span aria-hidden="true">&times;</span></button>
			    </div>
			    <div class="modal-body">
                    <iframe class="border-0 w-100" style="height: 460px;" loading="lazy"></iframe>
			    </div>
              
		    </div>
	    </div>
    </div>

    <%--Edit BOQ modal--%>
    <div id="ModEditBOQ" class="modal it-modal fade">
	    <div class="modal-dialog modal-dialog-centered modal-dialog-scrollable modal-xl">
		    <div class="modal-content">
			    <div class="modal-header">
				    <h5 class="modal-title">Edit BOQ</h5>
                    <button aria-label="Close" class="close" data-dismiss="modal" type="button"><span aria-hidden="true">&times;</span></button>
			    </div>
			    <div class="modal-body">
                    <iframe class="border-0 w-100" style="height: 460px;" loading="lazy"></iframe>
			    </div>
              
		    </div>
	    </div>
    </div>
</asp:Content>
