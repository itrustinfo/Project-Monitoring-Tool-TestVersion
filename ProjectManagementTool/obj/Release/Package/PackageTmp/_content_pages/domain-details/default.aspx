<%@ Page Title="" Language="C#" MasterPageFile="~/_master_pages/default.Master" AutoEventWireup="true" CodeBehind="default.aspx.cs" Inherits="ProjectManagementTool._content_pages.domain_details._default" %>
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
        function BindEvents() {
       
        $(".showAddDomainDetails").click(function(e) {
        e.preventDefault();
        var url = $(this).attr("href");
        $("#ModAddDomainDetails iframe").attr("src", url);
        $("#ModAddDomainDetails").modal("show");
          });

         $(".showEditDomainDetails").click(function(e) {
        e.preventDefault();
        var url = $(this).attr("href");
        $("#ModEditDomainDetails iframe").attr("src", url);
        $("#ModEditDomainDetails").modal("show");
            });
        }
        $(document).ready(function () {
            BindEvents();
        });
</script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="default_master_body" runat="server">
    <div class="container-fluid">
        <asp:HiddenField ID="HiddenWorkpackageUID" runat="server" />
            <div class="row">
                <div class="col-md-12 col-lg-4 form-group">Domain Details</div>
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
                                   <asp:Label ID="lblcamera" CssClass="text-uppercase font-weight-bold" runat="server" Text="Domain Details"  />
                               </h6>
                                <div>
                                   <a href="/_modal_pages/add-domaindetails.aspx" class="showAddDomainDetails"><asp:Button ID="btnadddomain" runat="server" Text="+ Add Domain Details" CssClass="btn btn-primary"></asp:Button></a>
                                </div>
                          </div>
                            </div>
                        <div class="table-responsive">
                            <asp:GridView ID="GrdDomainDetails" runat="server" CssClass="table table-bordered" AutoGenerateColumns="False" AllowPaging="false" Width="100%">
                               <Columns>             
                                    <asp:TemplateField HeaderText="Title">
                                      <ItemTemplate>
                                              <%#Eval("Title")%>
                                      </ItemTemplate>
                                    </asp:TemplateField> 
                                   <asp:TemplateField HeaderText="Description">
                                      <ItemTemplate>
                                              <%#Eval("Description")%>
                                      </ItemTemplate>
                                    </asp:TemplateField> 
                                   <asp:TemplateField HeaderText="URL">
                                      <ItemTemplate>
                                              <%#Eval("URL")%>
                                      </ItemTemplate>
                                    </asp:TemplateField>  
                                   <asp:TemplateField HeaderText="Logo">
                                      <ItemTemplate>
                                              <img src="/_assets/Logos/<%#Eval("Logo")%>" alt='<%#Eval("Title")%>' />
                                      </ItemTemplate>
                                    </asp:TemplateField>  
                                   <asp:TemplateField>
                                      <ItemTemplate>
                                              <a href='/_modal_pages/add-domaindetails.aspx?UID=<%#Eval("UID")%>' class="showEditDomainDetails"><span title="Edit" class="fas fa-edit"></span></a>
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
     <%--Add Domain Details modal--%>
    <div id="ModAddDomainDetails" class="modal it-modal fade">
	    <div class="modal-dialog modal-dialog-centered modal-dialog-scrollable modal-lg">
		    <div class="modal-content">
			    <div class="modal-header">
				    <h5 class="modal-title">Add Domain Details</h5>
                    <button aria-label="Close" class="close" data-dismiss="modal" type="button"><span aria-hidden="true">&times;</span></button>
			    </div>
			    <div class="modal-body">
                    <iframe class="border-0 w-100" style="height: 450px;" loading="lazy"></iframe>
			    </div>
              
		    </div>
	    </div>
    </div>

    <%--Edit Domain Details modal--%>
    <div id="ModEditDomainDetails" class="modal it-modal fade">
	    <div class="modal-dialog modal-dialog-centered modal-dialog-scrollable modal-lg">
		    <div class="modal-content">
			    <div class="modal-header">
				    <h5 class="modal-title">Edit Domain Details</h5>
                    <button aria-label="Close" class="close" data-dismiss="modal" type="button"><span aria-hidden="true">&times;</span></button>
			    </div>
			    <div class="modal-body">
                    <iframe class="border-0 w-100" style="height: 450px;" loading="lazy"></iframe>
			    </div>
              
		    </div>
	    </div>
    </div>
</asp:Content>
