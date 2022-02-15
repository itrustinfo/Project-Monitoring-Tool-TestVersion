<%@ Page Title="" Language="C#" MasterPageFile="~/_master_pages/default.Master" AutoEventWireup="true" CodeBehind="default.aspx.cs" Inherits="ProjectManagementTool._content_pages.insurance._default" %>
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
        $(".showAddInsuranceModal").click(function(e) {
            e.preventDefault();
        var url = $(this).attr("href");
        $("#ModAddInsurance iframe").attr("src", url);
            $("#ModAddInsurance").modal("show");
        });

        $(".showEditInsuranceModal").click(function(e) {
            e.preventDefault();
        var url = $(this).attr("href");
        $("#ModEditInsurance iframe").attr("src", url);
        $("#ModEditInsurance").modal("show");
            });

            $(".showViewInsuranceModal").click(function(e) {
            e.preventDefault();
        var url = $(this).attr("href");
        $("#ModViewInsurance iframe").attr("src", url);
        $("#ModViewInsurance").modal("show");
            });

        $(".showAddDocumentsModal").click(function(e) {
        e.preventDefault();
        var url = $(this).attr("href");
        $("#ModAddDocuments iframe").attr("src", url);
        $("#ModAddDocuments").modal("show");
        });

            $(".showAddInsurancePremiumModal").click(function(e) {
        e.preventDefault();
        var url = $(this).attr("href");
        $("#ModAddPremium iframe").attr("src", url);
        $("#ModAddPremium").modal("show");
            });
        }
        $(document).ready(function () {
            BindEvents();
        });
</script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="default_master_body" runat="server">
    <%--project selection dropdowns--%>
        <div class="container-fluid">
            <div class="row">
                <div class="col-md-12 col-lg-4 form-group">Insurance</div>
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
                                   <asp:Label ID="ActivityHeading" CssClass="text-uppercase font-weight-bold" runat="server" Text="List of Insurance" />
                                       <%--<asp:Label Text="Foo bar" CssClass="pl-1" runat="server" />--%>
                               </h6>
                                <div>
                                    <asp:Button ID="btnback" runat="server" Text="Back To Dashboard" CssClass="btn btn-primary" PostBackUrl="/_content_pages/dashboard/" Visible="false"></asp:Button>
                                   <a id="AddInsurance" runat="server" href="/_modal_pages/add-Insurance.aspx" class="showAddInsuranceModal"><asp:Button ID="Button2" runat="server" Text="+ Add Insurance" CssClass="btn btn-primary"></asp:Button></a>
                                </div>
                          </div>
                            </div>
                        <div class="table-responsive">
                                <asp:GridView ID="GrdInsurance" runat="server" CssClass="table table-bordered" EmptyDataText="No Data" AutoGenerateColumns="False" Width="100%" OnRowCommand="GrdInsurance_RowCommand" OnRowDeleting="GrdInsurance_RowDeleting">
                                <Columns>
                                      <asp:BoundField DataField="Vendor_Name" HeaderText="Vendor Name" >
                            <HeaderStyle HorizontalAlign="Left" />
                            </asp:BoundField>
                         
                                    <asp:BoundField DataField="Vendor_Address" HeaderText="Vendor Address">
                    <HeaderStyle HorizontalAlign="Left" />
                    </asp:BoundField>
                    <%--<asp:BoundField DataField="Name_of_InsuranceCompany" HeaderText="Insurance Company" >
                    <HeaderStyle HorizontalAlign="Left" />
                    </asp:BoundField>
                    <asp:BoundField DataField="Branch" HeaderText="Branch">
                    <HeaderStyle HorizontalAlign="Left" />
                    </asp:BoundField>--%>
                   
                    <asp:BoundField DataField="Policy_Number" HeaderText="Policy Number">
                    <HeaderStyle HorizontalAlign="Left" />
                    </asp:BoundField>
                    <asp:BoundField DataField="Policy_Status" HeaderText="Policy Status" >
                    <HeaderStyle HorizontalAlign="Left" />
                    </asp:BoundField>
                             
                     
                                    <asp:TemplateField HeaderText="Amount">
                                        <ItemTemplate>
                                            <span style="color:#006699;"><%#Eval("Currency")%></span>&nbsp;<%#string.Format(new System.Globalization.CultureInfo(Eval("Currency_CultureInfo").ToString()),"{0:N}", Eval("Insured_Amount"))%>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                 <asp:BoundField DataField="Insured_Date" HeaderText="Insured Date"  DataFormatString="{0:dd/MM/yyyy}">
                                    <HeaderStyle HorizontalAlign="Left" />
                                    </asp:BoundField>
                                   <%-- <asp:BoundField DataField="Maturity_Date" HeaderText="Maturity Date"  DataFormatString="{0:dd/MM/yyyy}">
                    <HeaderStyle HorizontalAlign="Left" />
                    </asp:BoundField>--%>
                                    <%--<asp:BoundField DataField="Nominee" HeaderText="Nominee" >
                    <HeaderStyle HorizontalAlign="Left" />
                    </asp:BoundField>--%>
                                  
                                    <asp:TemplateField>
                                        <ItemTemplate>
                                            <a  href="/_modal_pages/add-Insurance.aspx?InsuranceUID=<%#Eval("InsuranceUID")%>&ProjectUID=<%#Eval("ProjectUID")%>&WorkpackgeUID=<%#Eval("WorkPackageUID")%>&View=true" class="showViewInsuranceModal">View</a>
                                        </ItemTemplate>
                                    </asp:TemplateField>

                                     <asp:TemplateField HeaderText="Documents">
                                        <ItemTemplate>
                                            <a id="AddDocuments" href='/_modal_pages/add-insurancedocuments.aspx?InsuranceUID=<%#Eval("InsuranceUID")%>&ProjectUID=<%#Eval("ProjectUID")%>' class="showAddDocumentsModal">Add</a>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Premium Add/View">
                                        <ItemTemplate>
                                            <a href='/_modal_pages/add-insurancepremium.aspx?InsuranceUID=<%#Eval("InsuranceUID")%>&ProjectUID=<%#Eval("ProjectUID")%>' class="showAddInsurancePremiumModal">Add</a> or 
                                            <a href='/_modal_pages/view-insurancepremium.aspx?InsuranceUID=<%#Eval("InsuranceUID")%>&ProjectUID=<%#Eval("ProjectUID")%>' class="showAddInsurancePremiumModal">View</a>
                                        </ItemTemplate>
                                    </asp:TemplateField>

                                    <asp:TemplateField>
                                        <ItemTemplate>
                                            <a id="EditInsurance" href="/_modal_pages/add-Insurance.aspx?InsuranceUID=<%#Eval("InsuranceUID")%>&ProjectUID=<%#Eval("ProjectUID")%>&WorkpackgeUID=<%#Eval("WorkPackageUID")%>" class="showEditInsuranceModal"><span title="Edit" class="fas fa-edit"></span></a>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField>
                                         <ItemTemplate>
                                              <asp:LinkButton ID="lnkdelete" OnClientClick="return DeleteItem()" runat="server" CausesValidation="false" CommandArgument='<%#Eval("InsuranceUID")%>' CommandName="delete"><span title="Delete" class="fas fa-trash"></span></asp:LinkButton>
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

    <%--add Insurance modal--%>
    <div id="ModAddInsurance" class="modal it-modal fade">
	    <div class="modal-dialog modal-dialog-centered modal-dialog-scrollable modal-xl">
		    <div class="modal-content">
			    <div class="modal-header">
				    <h5 class="modal-title">Add Insurance</h5>
                    <button aria-label="Close" class="close" data-dismiss="modal" type="button"><span aria-hidden="true">&times;</span></button>
			    </div>
			    <div class="modal-body">
                    <iframe class="border-0 w-100" style="height:480px;" loading="lazy"></iframe>
			    </div>
              
		    </div>
	    </div>
    </div>
    <%--Edit Insurance modal--%>
    <div id="ModEditInsurance" class="modal it-modal fade">
	    <div class="modal-dialog modal-dialog-centered modal-dialog-scrollable modal-xl">
		    <div class="modal-content">
			    <div class="modal-header">
				    <h5 class="modal-title">Edit Insurance</h5>
                    <button aria-label="Close" class="close" data-dismiss="modal" type="button"><span aria-hidden="true">&times;</span></button>
			    </div>
			    <div class="modal-body">
                    <iframe class="border-0 w-100" style="height:480px;" loading="lazy"></iframe>
			    </div>
              
		    </div>
	    </div>
    </div>

    <%--Edit Insurance modal--%>
    <div id="ModViewInsurance" class="modal it-modal fade">
	    <div class="modal-dialog modal-dialog-centered modal-dialog-scrollable modal-xl">
		    <div class="modal-content">
			    <div class="modal-header">
				    <h5 class="modal-title">View Insurance Details</h5>
                    <button aria-label="Close" class="close" data-dismiss="modal" type="button"><span aria-hidden="true">&times;</span></button>
			    </div>
			    <div class="modal-body">
                    <iframe class="border-0 w-100" style="height:480px;" loading="lazy"></iframe>
			    </div>
              
		    </div>
	    </div>
    </div>


    <%--add Document modal--%>
    <div id="ModAddDocuments" class="modal it-modal fade">
	    <div class="modal-dialog modal-dialog-centered modal-dialog-scrollable modal-xl">
		    <div class="modal-content">
			    <div class="modal-header">
				    <h5 class="modal-title">Add Insurance Documents</h5>
                    <button aria-label="Close" class="close" data-dismiss="modal" type="button"><span aria-hidden="true">&times;</span></button>
			    </div>
			    <div class="modal-body">
                    <iframe class="border-0 w-100" style="height:480px;" loading="lazy"></iframe>
			    </div>
              
		    </div>
	    </div>
    </div>

    <%--add Premium modal--%>
    <div id="ModAddPremium" class="modal it-modal fade">
	    <div class="modal-dialog modal-dialog-centered modal-dialog-scrollable modal-xl">
		    <div class="modal-content">
			    <div class="modal-header">
				    <h5 class="modal-title">Add Insurance Premium</h5>
                    <button aria-label="Close" class="close" data-dismiss="modal" type="button"><span aria-hidden="true">&times;</span></button>
			    </div>
			    <div class="modal-body">
                    <iframe class="border-0 w-100" style="height:480px;" loading="lazy"></iframe>
			    </div>
              
		    </div>
	    </div>
    </div>
</asp:Content>
