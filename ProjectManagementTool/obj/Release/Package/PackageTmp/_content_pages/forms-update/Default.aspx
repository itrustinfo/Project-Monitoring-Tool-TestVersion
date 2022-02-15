<%@ Page Title="" Language="C#" MasterPageFile="~/_master_pages/default.Master" AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="ProjectManagementTool._content_pages.forms_update.Default" %>
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
                        <asp:DropDownList ID="ddlmeeting" runat="server" CssClass="form-control" width="50%" AutoPostBack="true" OnSelectedIndexChanged="ddlmeeting_SelectedIndexChanged"></asp:DropDownList>
                       <asp:Button ID="btncopy" runat="server" OnClientClick="return CopyMessage()" Text="Copy Data from Previous Report" CssClass="btn btn-primary" Visible="false" OnClick="btncopy_Click"/>
                    </div>
                    <div>
                         
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
                                   <asp:Label ID="ActivityHeading" CssClass="text-uppercase font-weight-bold" runat="server" Text="Status of Budget Vs Disbursement" />
                                       <%--<asp:Label Text="Foo bar" CssClass="pl-1" runat="server" />--%>
                               </h6>
                              
                          </div>
                            </div>
                        <div class="table-responsive">
                            <asp:HiddenField ID="ReviewMeetingDate" runat="server" />
                              <div>
                                   <a id="AddBankGuarantee" runat="server" href="~/_modal_pages/add-Budget_disburse.aspx" class="showBankGuaranteeModal"><asp:Button ID="Button2" runat="server" Text="+ Add" CssClass="btn btn-primary"></asp:Button></a>
                                </div><br />
                                <asp:GridView ID="GrdBudgetbsDisbursemnt" runat="server" CssClass="table table-bordered" EmptyDataText="No Data" AutoGenerateColumns="False" Width="100%" OnDataBound="GrdBudgetbsDisbursemnt_DataBound">
                                <Columns>
                                      <asp:TemplateField>
                                        <ItemTemplate>
                                            <a id="Edit" href="/_modal_pages/add-Budget_disburse.aspx?UID=<%#Eval("UID")%>&type=edit" class="showBankGuaranteeModal"><span title="Edit" class="fas fa-edit"></span></a>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                      <asp:BoundField DataField="ProjectName" HeaderText="Contract Package" >
                            <HeaderStyle HorizontalAlign="Left" />
                            </asp:BoundField>
                                     <asp:BoundField DataField="ContractorName" HeaderText="ContractorName" >
                            <HeaderStyle HorizontalAlign="Left" />
                            </asp:BoundField>
                                     <asp:BoundField DataField="AwardedCost" HeaderText="Awarded Cost (Crores)" >
                            <HeaderStyle HorizontalAlign="Left" />
                            </asp:BoundField>
                                     <asp:BoundField DataField="Disbursement_Amount" HeaderText="Disbursement_Amount <br/> for FY 2019-20 MJPY" HtmlEncode="false" >
                            <HeaderStyle HorizontalAlign="Left" />
                            </asp:BoundField>
                                    <asp:BoundField DataField="Disbursement_Amount_2021" HeaderText="Disbursement_Amount <br/> for FY 2020-21 MJPY" HtmlEncode="false" >
                            <HeaderStyle HorizontalAlign="Left" />
                            </asp:BoundField>
                                     <asp:BoundField DataField="Q1_Budget_Amount" HeaderText="Q1" >
                            <HeaderStyle HorizontalAlign="Left" />
                            </asp:BoundField>
                                    <asp:BoundField DataField="Q2_Budget_Amount" HeaderText="Q2" >
                            <HeaderStyle HorizontalAlign="Left" />
                            </asp:BoundField>
                                    <asp:BoundField DataField="Q3_Budget_Amount" HeaderText="Q3" >
                            <HeaderStyle HorizontalAlign="Left" />
                            </asp:BoundField>
                                    <asp:BoundField DataField="Q4_Budget_Amount" HeaderText="Q4" >
                            <HeaderStyle HorizontalAlign="Left" />
                            </asp:BoundField>
                            <asp:BoundField DataField="Q1_Actual_Amount" HeaderText="Q1" >
                            <HeaderStyle HorizontalAlign="Left" />
                            </asp:BoundField>
                                      <asp:BoundField DataField="Q2_Actual_Amount" HeaderText="Q2" >
                            <HeaderStyle HorizontalAlign="Left" />
                            </asp:BoundField>
                                      <asp:BoundField DataField="Q3_Actual_Amount" HeaderText="Q3" >
                            <HeaderStyle HorizontalAlign="Left" />
                            </asp:BoundField>
                                      <asp:BoundField DataField="Q4_Actual_Amount" HeaderText="Q4" >
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
				    <h5 class="modal-title">Add Budget vs Disbursement</h5>
                    <button aria-label="Close" class="close" data-dismiss="modal" type="button"><span aria-hidden="true">&times;</span></button>
			    </div>
			    <div class="modal-body">
                    <iframe class="border-0 w-100" style="height:500px;" loading="lazy"></iframe>
			    </div>
              
		    </div>
	    </div>
    </div>

</asp:Content>
