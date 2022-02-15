<%@ Page Title="" Language="C#" MasterPageFile="~/_master_pages/default.Master" AutoEventWireup="true" CodeBehind="default.aspx.cs" Inherits="ProjectManagementTool._content_pages.status_wastewater_forms._default" %>
<asp:Content ID="Content1" ContentPlaceHolderID="default_master_head" runat="server">
    <script type="text/javascript">
        function CopyMessage() {
            if (confirm("Are you sure you want to Copy ...?")) {
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
          
        });
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="default_master_body" runat="server">
     <div class="container-fluid">
            <div class="row">
                <div class="col-md-6 col-lg-4 form-group">
                    <label class="sr-only" for="DDLProject">Select Meeting</label>
                    <div class="input-group">
                        <div class="input-group-prepend">
                            <span class="input-group-text">Select Meeting</span>
                        </div>
                        <asp:DropDownList ID="ddlmeeting" runat="server" CssClass="form-control" AutoPostBack="true"  OnSelectedIndexChanged="ddlmeeting_SelectedIndexChanged"></asp:DropDownList>
                       
                    </div>
                    <div><br />
                        &nbsp;&nbsp;<asp:Button ID="btncopy" runat="server" Text="Copy Data from Previous Report" OnClientClick="return CopyMessage()" CssClass="btn btn-primary" Visible="false" OnClick="btncopy_Click"/> 
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
                                   <asp:Label ID="ActivityHeading" CssClass="text-uppercase font-weight-bold" runat="server" Text="Status of Waste Water Contract Packages" />
                                       <%--<asp:Label Text="Foo bar" CssClass="pl-1" runat="server" />--%>
                               </h6>
                              
                          </div>
                            </div>
                        <div class="table-responsive">
                              <div>
                                   <a id="AddBankGuarantee" runat="server" href="~/_modal_pages/add-status-wastewater.aspx" class="showBankGuaranteeModal"><asp:Button ID="Button2" runat="server" Text="+ Add" CssClass="btn btn-primary"></asp:Button></a>
                                </div><br />
                                <asp:GridView ID="GrdStatus" runat="server" CssClass="table table-bordered" EmptyDataText="No Data" AutoGenerateColumns="False" Width="100%">
                                <Columns>
                                      <asp:TemplateField>
                                        <ItemTemplate>
                                            <a id="Edit" href="/_modal_pages/add-status-wastewater.aspx?UID=<%#Eval("UID")%>&type=edit" class="showBankGuaranteeModal"><span title="Edit" class="fas fa-edit"></span></a>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                       <asp:BoundField DataField="Componenttype" HeaderText="Component Type" >
                            <HeaderStyle HorizontalAlign="Left" />
                            </asp:BoundField>
                                      <asp:BoundField DataField="ProjectName" HeaderText="Contract Package" >
                            <HeaderStyle HorizontalAlign="Left" />
                            </asp:BoundField>
                                     <asp:BoundField DataField="PackageDescription" HeaderText="Package Description" HtmlEncode="false">
                            <HeaderStyle HorizontalAlign="Left" />
                            </asp:BoundField>
                                     <asp:BoundField DataField="AwardedCost" HeaderText="Awarded Cost / Sanction Cost <br/> excluding Provisional Sum and Physical Contingency (Rs. Crores)" HtmlEncode="false">
                            <HeaderStyle HorizontalAlign="Left" />
                            </asp:BoundField>
                                     <asp:BoundField DataField="ProjectComponent" HeaderText="Project Components" HtmlEncode="false">
                            <HeaderStyle HorizontalAlign="Left" />
                            </asp:BoundField>
                                     <asp:BoundField DataField="PresentStatus" HeaderText="Present Status" HtmlEncode="false">
                            <HeaderStyle HorizontalAlign="Left" />
                          
                                   </asp:BoundField>
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
				    <h5 class="modal-title">Add Status Of Waster Water Contract Packages</h5>
                    <button aria-label="Close" class="close" data-dismiss="modal" type="button"><span aria-hidden="true">&times;</span></button>
			    </div>
			    <div class="modal-body">
                    <iframe class="border-0 w-100" style="height:400px;" loading="lazy"></iframe>
			    </div>
              
		    </div>
	    </div>
    </div>
</asp:Content>
