<%@ Page Title="" Language="C#" MasterPageFile="~/_master_pages/default.Master" AutoEventWireup="true" CodeBehind="default.aspx.cs" Inherits="ProjectManagementTool._content_pages.bank_guarantee._default" %>
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
        $(document).ready(function () {
            
            $(".showBankGuaranteeModal").click(function (e) {
                e.preventDefault();
                var url = $(this).attr("href");
                $("#ModAddBankGuarantee iframe").attr("src", url);
                $("#ModAddBankGuarantee").modal("show");
            });
            $(".showEditBankGuaranteeModal").click(function (e) {
                e.preventDefault();
                var url = $(this).attr("href");
                $("#ModEditBankGuarantee iframe").attr("src", url);
                $("#ModEditBankGuarantee").modal("show");
            });

            $(".showBankDocumentModal").click(function (e) {
                e.preventDefault();
                var url = $(this).attr("href");
                $("#ModAddBankDocument iframe").attr("src", url);
                $("#ModAddBankDocument").modal("show");
            });
        });
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="default_master_body" runat="server">
     <%--project selection dropdowns--%>
    
        <div class="container-fluid">
            <div class="row">
                <div class="col-md-12 col-lg-4 form-group">Bank Guarantee</div>
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
                                   <asp:Label ID="ActivityHeading" CssClass="text-uppercase font-weight-bold" runat="server" Text="List of Bank Guarantee's" />
                                       <%--<asp:Label Text="Foo bar" CssClass="pl-1" runat="server" />--%>
                               </h6>
                                <div>
                                    <asp:Button ID="btnback" runat="server" Text="Back To Dashboard" CssClass="btn btn-primary" PostBackUrl="/_content_pages/dashboard/" Visible="false"></asp:Button>
                                   <a id="AddBankGuarantee" runat="server" href="/_modal_pages/add-bankguarantee.aspx" class="showBankGuaranteeModal"><asp:Button ID="Button2" runat="server" Text="+ Add Bank Guarantee" CssClass="btn btn-primary"></asp:Button></a>
                                </div>
                          </div>
                            </div>
                        <div class="table-responsive">
                                <asp:GridView ID="GrdBankGuarantee" runat="server" CssClass="table table-bordered" EmptyDataText="No Data" AutoGenerateColumns="False" Width="100%" OnRowCommand="GrdBankGuarantee_RowCommand" OnRowDeleting="GrdBankGuarantee_RowDeleting">
                                <Columns>
                                      <asp:BoundField DataField="BG_Number" HeaderText="BG Number" >
                            <HeaderStyle HorizontalAlign="Left" />
                            </asp:BoundField>
                                   <%-- <asp:BoundField DataField="Vendor_Name" HeaderText="Vendor Name">
                            <HeaderStyle HorizontalAlign="Left" />
                            </asp:BoundField>
                                     <asp:BoundField DataField="Vendor_Address" HeaderText="Vendor Address">
                            <HeaderStyle HorizontalAlign="Left" />
                            </asp:BoundField>--%>
                                    <asp:TemplateField HeaderText="Amount">
                                        <ItemTemplate>
                                            <span style="color:#006699;"><%#Eval("Currency")%></span>&nbsp;<%#string.Format(new System.Globalization.CultureInfo(Eval("Currency_CultureInfo").ToString()),"{0:N}", Eval("Amount"))%>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                           <%-- <asp:BoundField DataField="Validity" HeaderText="Validity">
                            <HeaderStyle HorizontalAlign="Left" />
                            </asp:BoundField>--%>
                              <asp:BoundField DataField="Date_of_Expiry" HeaderText="Date of Expiry"  DataFormatString="{0:dd/MM/yyyy}">
                            <HeaderStyle HorizontalAlign="Left" />
                            </asp:BoundField>
                          <%--  <asp:BoundField DataField="No_of_Collaterals" HeaderText="No. of Collaterals">
                            <HeaderStyle HorizontalAlign="Left" />
                            </asp:BoundField>--%>
                            <asp:BoundField DataField="Bank_Name" HeaderText="Bank Name" >
                            <HeaderStyle HorizontalAlign="Left" />
                            </asp:BoundField>
                             <asp:BoundField DataField="Bank_Branch" HeaderText="Bank Branch" >
                            <HeaderStyle HorizontalAlign="Left" />
                            </asp:BoundField>
                                     <asp:BoundField DataField="IFSC_Code" HeaderText="IFSC Code" >
                            <HeaderStyle HorizontalAlign="Left" />
                            </asp:BoundField>
                                      <asp:BoundField DataField="Date_of_Guarantee" HeaderText="Date of Guarantee"  DataFormatString="{0:dd/MM/yyyy}">
                            <HeaderStyle HorizontalAlign="Left" />
                            </asp:BoundField>
                                     <asp:TemplateField>
                                        <ItemTemplate>
                                            <a id="AddDocuments" href='/_modal_pages/add-bankdocument.aspx?Bank_GuaranteeUID=<%#Eval("Bank_GuaranteeUID")%>&ProjectUID=<%#Eval("ProjectUID")%>' class="showBankDocumentModal">Documents</a>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField>
                                        <ItemTemplate>
                                            <a id="EditBankGuarantee" href="/_modal_pages/add-bankguarantee.aspx?Bank_GuaranteeUID=<%#Eval("Bank_GuaranteeUID")%>" class="showEditBankGuaranteeModal" ><span title="Edit" class="fas fa-edit"></span></a>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField>
                                                    <ItemTemplate>
                                                        <asp:LinkButton ID="lnkdelete" OnClientClick="return DeleteItem()" runat="server" CausesValidation="false" CommandArgument='<%#Eval("Bank_GuaranteeUID")%>' CommandName="delete"><span title="Delete" class="fas fa-trash"></span></asp:LinkButton>
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
    <%--add Bank Guarantee modal--%>
    <div id="ModAddBankGuarantee" class="modal it-modal fade">
	    <div class="modal-dialog modal-dialog-centered modal-dialog-scrollable modal-xl">
		    <div class="modal-content">
			    <div class="modal-header">
				    <h5 class="modal-title">Add Bank Guarantee</h5>
                    <button aria-label="Close" class="close" data-dismiss="modal" type="button"><span aria-hidden="true">&times;</span></button>
			    </div>
			    <div class="modal-body">
                    <iframe class="border-0 w-100" style="height:500px;" loading="lazy"></iframe>
			    </div>
              
		    </div>
	    </div>
    </div>
    <%--add Bank Guarantee modal--%>
    <div id="ModEditBankGuarantee" class="modal it-modal fade">
	    <div class="modal-dialog modal-dialog-centered modal-dialog-scrollable modal-xl">
		    <div class="modal-content">
			    <div class="modal-header">
				    <h5 class="modal-title">Edit Bank Guarantee</h5>
                    <button aria-label="Close" class="close" data-dismiss="modal" type="button"><span aria-hidden="true">&times;</span></button>
			    </div>
			    <div class="modal-body">
                    <iframe class="border-0 w-100" style="height:500px;" loading="lazy"></iframe>
			    </div>
              
		    </div>
	    </div>
    </div>
    <%--add Bank Document modal--%>
    <div id="ModAddBankDocument" class="modal it-modal fade">
	    <div class="modal-dialog modal-dialog-centered modal-dialog-scrollable modal-xl">
		    <div class="modal-content">
			    <div class="modal-header">
				    <h5 class="modal-title">Add Bank Documents</h5>
                    <button aria-label="Close" class="close" data-dismiss="modal" type="button"><span aria-hidden="true">&times;</span></button>
			    </div>
			    <div class="modal-body">
                    <iframe class="border-0 w-100" style="height:500px;" loading="lazy"></iframe>
			    </div>
              
		    </div>
	    </div>
    </div>
</asp:Content>
