<%@ Page Title="" Language="C#" MasterPageFile="~/_master_pages/default.Master" AutoEventWireup="true" CodeBehind="default.aspx.cs" Inherits="ProjectManagementTool._content_pages.project_physicalprogress._default" %>
<asp:Content ID="Content1" ContentPlaceHolderID="default_master_head" runat="server">
    <script type="text/javascript">
        function CopyMessage() {
            if (confirm("Are you sure you want to Copy ...?")) {
                return true;
            }
            return false;
    }
    </script>
    <script>
      $(document).ready(function() {
  $(".showModalAddPhysicalProgress").click(function(e) {
    e.preventDefault();
    var url = $(this).attr("href");
    $("#ModAddPhysicalProgress iframe").attr("src", url);
    $("#ModAddPhysicalProgress").modal("show");
          });
   $(".showModalEditPhysicalProgress").click(function(e) {
    e.preventDefault();
    var url = $(this).attr("href");
    $("#ModEditPhysicalProgress iframe").attr("src", url);
    $("#ModEditPhysicalProgress").modal("show");
  });
});
</script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="default_master_body" runat="server">
     <div class="container-fluid">
        <div class="row">
            <div class="col-lg-6 col-xl-8 form-group">Physical Progress Achieved</div>
            <div class="col-lg-6 col-xl-4 form-group">
                
            </div>
        </div>
    </div>
     <div class="container-fluid">
        <div class="row">
            <div class="col-lg-6 col-xl-12 form-group">
                <div class="card mb-4">
                    <div class="card-body">
                        <div class="card-title">
                            <div class="d-flex justify-content-between">
                                <h6 class="text-muted">
                                    <asp:Label Text="Physical Progress Achieved" CssClass="text-uppercase font-weight-bold" runat="server" />
                                    <%--<asp:Label Text="Foo bar" CssClass="pl-1" runat="server" />--%>
                                </h6>
                                <div>
                                    <a href="/_modal_pages/add-projectphysicalprogress.aspx" id="PhysicalProgress" runat="server" class="showModalAddPhysicalProgress"><asp:Button ID="btnadd" runat="server" Text="+ Add Physical Progress" CssClass="btn btn-primary"></asp:Button></a>
                                </div>
                            </div>
                        </div>
                        <div class="table-responsive">
                            <label class="lblCss" for="txtxsummary">Select Review Meeting</label>
                            <asp:DropDownList ID="DDLMeetingMaster" runat="server" CssClass="form-control" Width="30%" AutoPostBack="true" OnSelectedIndexChanged="DDLMeetingMaster_SelectedIndexChanged">

                            </asp:DropDownList>
                                <br />

                                           <asp:Button ID="btncopy" runat="server" Text="Copy Data from Previous Report" CssClass="btn btn-primary" OnClientClick="return CopyMessage()" Visible="false" OnClick="btncopy_Click"/>
      
                            <br /><br />
                            <asp:GridView ID="GrdPhysicalProgress" runat="server" FooterStyle-Font-Bold="true" CssClass="table table-bordered" ShowFooter="true" AutoGenerateColumns="False" EmptyDataText="No Data" Width="100%" OnRowDataBound="GrdPhysicalProgress_RowDataBound">
                                <Columns>
                                    <asp:TemplateField HeaderText="Sl.No">
                                        <ItemTemplate>
                                            <%# Container.DataItemIndex + 1 %>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Project Name">
                                        <ItemTemplate>
                                            <%#GetProjectName(Eval("ProjectUID").ToString())%>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Name of the Package" FooterStyle-HorizontalAlign="Right">
                                        <ItemTemplate>
                                            <%#Eval("NameofthePackage")%>
                                        </ItemTemplate>
                                        <FooterTemplate>
                                            <asp:Label ID="LblTotal" runat="server" Text="Total" Font-Bold="true"></asp:Label>
                                        </FooterTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Awarded/Sanctioned excluding Provisional Sum and Physical Contingencies(Rs.Cr)" ItemStyle-HorizontalAlign="Right" FooterStyle-HorizontalAlign="Right">
                                        <ItemTemplate>
                                            <asp:Label ID="LblAward" runat="server" Text='<%#Eval("Awarded_Sanctioned_Value","{0:n}")%>'></asp:Label>
                                        </ItemTemplate>
                                        <FooterTemplate>
                                            <asp:Label ID="LblAwardedCost_Total" runat="server" Font-Bold="true"></asp:Label>
                                        </FooterTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Status of Award">
                                        <ItemTemplate>
                                            <%#Eval("Award_Status")%>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Expenditure til Today(Rs. Cr)" FooterStyle-HorizontalAlign="Right" ItemStyle-HorizontalAlign="Right">
                                        <ItemTemplate>
                                            <asp:Label ID="LblExpenditure" runat="server" Text='<%#Eval("Expenditure_As_On_Date","{0:n}")%>'></asp:Label>
                                        </ItemTemplate>
                                         <FooterTemplate>
                                            <asp:Label ID="LblExpenditure_Total" runat="server" Font-Bold="true"></asp:Label>
                                        </FooterTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Targeted Physical Progress(%)">
                                        <ItemTemplate>
                                            <%#Eval("Targeted_PhysicalProgress","{0:n}")%>%
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Targeted Overall Weighted Progress(%)">
                                        <ItemTemplate>
                                            <%#Eval("Targeted_Overall_WeightedProgress","{0:n}")%>%
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Achieved Physical Progress(%)">
                                        <ItemTemplate>
                                            <%#Eval("Achieved_PhysicalProgress","{0:n}")%>%
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Achieved Overall Weighted Progress(%)">
                                        <ItemTemplate>
                                            <%#Eval("Achieved_Overall_WeightedProgress","{0:n}")%>%
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField>
                                        <ItemTemplate>
                                            <a id="EditPhysicalProgress" href="/_modal_pages/edit-projectphysicalprogress.aspx?PhysicalProgressUID=<%#Eval("PhysicalProgressUID")%>&ProjectUID=<%#Eval("ProjectUID")%>" class="showModalEditPhysicalProgress"><span title="Edit" class="fas fa-edit"></span></a>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                </Columns>
                             </asp:GridView>
                            <div class="text-right">
                                <asp:Button ID="btnexport" runat="server" Visible="false" Text="Export to PDF" OnClick="btnexport_Click" CssClass="btn btn-primary" />
                            </div>
                        </div>
                    </div>
                </div>
            </div>
            </div>
        </div>

    <%--Add Physical Progress--%>
    <div id="ModAddPhysicalProgress" class="modal it-modal fade">
	    <div class="modal-dialog modal-dialog-centered modal-dialog-scrollable modal-xl">
		    <div class="modal-content">
			    <div class="modal-header">
				    <h5 class="modal-title">Add Project Physical Progess</h5>
                    <button aria-label="Close" class="close" data-dismiss="modal" type="button"><span aria-hidden="true">&times;</span></button>
			    </div>
			    <div class="modal-body">
                    <iframe class="border-0 w-100" style="height:480px;" loading="lazy"></iframe>
			    </div>
		    </div>
	    </div>
    </div>

    <%--add work package category modal--%>
    <div id="ModEditPhysicalProgress" class="modal it-modal fade">
	    <div class="modal-dialog modal-dialog-centered modal-dialog-scrollable modal-xl">
		    <div class="modal-content">
			    <div class="modal-header">
				    <h5 class="modal-title">Edit Project Physical Progess</h5>
                    <button aria-label="Close" class="close" data-dismiss="modal" type="button"><span aria-hidden="true">&times;</span></button>
			    </div>
			    <div class="modal-body">
                    <iframe class="border-0 w-100" style="height:480px;" loading="lazy"></iframe>
			    </div>
		    </div>
	    </div>
    </div>
</asp:Content>
