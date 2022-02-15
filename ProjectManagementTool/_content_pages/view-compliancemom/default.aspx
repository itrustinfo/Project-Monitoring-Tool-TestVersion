<%@ Page Title="" Language="C#" MasterPageFile="~/_master_pages/default.Master" AutoEventWireup="true" CodeBehind="default.aspx.cs" Inherits="ProjectManagementTool._content_pages.view_compliancemom._default" %>
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
    <script type="text/javascript">
        $(document).ready(function () {
            $(".showComplianceMOMModal").click(function (e) {
                e.preventDefault();
                var url = $(this).attr("href");
                $("#ModAddComplianceMOM iframe").attr("src", url);
                $("#ModAddComplianceMOM").modal("show");
            });
            $(".showEditComplianceMOMModal").click(function (e) {
                e.preventDefault();
                var url = $(this).attr("href");
                $("#ModEditComplianceMOM iframe").attr("src", url);
                $("#ModEditComplianceMOM").modal("show");
            });
           
        });
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="default_master_body" runat="server">
       <div class="container-fluid">
            <div class="row">
               <div class="col-lg-12 col-xl-12 form-group">
                <div class="card mb-4">
                    <div class="card-body">
                        <div class="card-title">
                        <div class="d-flex justify-content-between">
                              <h6 class="text-muted">
                                   <asp:Label ID="ActivityHeading" CssClass="text-uppercase font-weight-bold" runat="server" Text="List of Compliance of M.O.M" />
                               </h6>
                                <div>
                                     <a id="AddComplianceofMOM" runat="server" href="/_modal_pages/add-complianceofMOM.aspx" class="showComplianceMOMModal"><asp:Button ID="btnaddCAA" runat="server" Text="+ Add Compliance of M.O.M" CssClass="btn btn-primary"></asp:Button></a>
                                </div>
                          </div>
                            </div>
                        <div class="table-responsive">
                            
                <div class="col-sm-12">
                    
                        <label class="lblCss" for="DDLMeeting">Select Meeting</label>
                        <asp:DropDownList ID="ddlMeeting" runat="server" class="form-control" Width="40%" AutoPostBack="true" OnSelectedIndexChanged="ddlMeeting_SelectedIndexChanged"  ></asp:DropDownList>
                    <br />

                    <asp:Button ID="btncopy" runat="server" Text="Copy Data from Previous Report" OnClientClick="return CopyMessage()" CssClass="btn btn-primary" Visible="false" OnClick="btncopy_Click"/>
                    <br /><br />
                   
                    <asp:GridView ID="GrdCompliance" runat="server" Width="100%" BackColor="White" CssClass="table table-bordered" EmptyDataText="No Data Found.."
                        BorderColor="#E7E7FF" BorderStyle="None" BorderWidth="1px" CellPadding="3" AutoGenerateColumns="False" OnRowCommand="GrdCompliance_RowCommand" OnRowDeleting="GrdCompliance_RowDeleting">
                        <Columns>
                            <asp:TemplateField HeaderText="Points of Last Review Meeting" ItemStyle-Width="35%">
                                <ItemTemplate>
                                    <%#Eval("Meeting_Points")%>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Status">
                                <ItemTemplate>
                                  <%#Eval("Meeting_Status")%>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField>
                                <ItemTemplate>
                                       <a id="EditComplianceMOM" href="/_modal_pages/add-complianceofMOM.aspx?ComplianceofMOM_UID=<%#Eval("ComplianceofMOM_UID")%>" class="showEditComplianceMOMModal"><span title="Edit" class="fas fa-edit"></span></a>
                                 </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField>
                                  <ItemTemplate>
                                         <asp:LinkButton ID="lnkdelete" runat="server" OnClientClick="return DeleteItem()" CausesValidation="false" CommandArgument='<%#Eval("ComplianceofMOM_UID")%>' CommandName="delete"><span title="Delete" class="fas fa-trash"></span></asp:LinkButton>
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
        
            </div>
    <%--add Compliance modal--%>

    <div id="ModAddComplianceMOM" class="modal it-modal fade">
	    <div class="modal-dialog modal-dialog-centered modal-dialog-scrollable modal-lg">
		    <div class="modal-content">
			    <div class="modal-header">
				    <h5 class="modal-title">Add Compliance of M.O.M</h5>
                    <button aria-label="Close" class="close" data-dismiss="modal" type="button"><span aria-hidden="true">&times;</span></button>
			    </div>
			    <div class="modal-body">
                    <iframe class="border-0 w-100" style="height:400px;" loading="lazy"></iframe>
			    </div>
              
		    </div>
	    </div>
    </div>
    <%--add Compliance modal--%>
   
      <div id="ModEditComplianceMOM" class="modal it-modal fade">
	    <div class="modal-dialog modal-dialog-centered modal-dialog-scrollable modal-lg">
		    <div class="modal-content">
			    <div class="modal-header">
				    <h5 class="modal-title">Edit Compliance of M.O.M</h5>
                    <button aria-label="Close" class="close" data-dismiss="modal" type="button"><span aria-hidden="true">&times;</span></button>
			    </div>
			    <div class="modal-body">
                    <iframe class="border-0 w-100" style="height:400px;" loading="lazy"></iframe>
			    </div>
              
		    </div>
	    </div>
    </div>
</asp:Content>
