<%@ Page Title="" Language="C#" MasterPageFile="~/_master_pages/modal.Master" AutoEventWireup="true" CodeBehind="View-RAbill-AdjDetails.aspx.cs" Inherits="ProjectManagementTool._modal_pages.View_RAbill_AdjDetails" %>
<asp:Content ID="Content1" ContentPlaceHolderID="modal_master_head" runat="server">
     <style>
        .Hide
            {
                display : none;
            }
    </style>
    <script type="text/javascript">
        $(document).ready(function () {
            $(".showStatusModal").click(function (e) {
                e.preventDefault();
                jQuery.noConflict();
                var url = $(this).attr("href");
                $("#ModAddDocumentStatus iframe").attr("src", url);
                $("#ModAddDocumentStatus").modal("show");
            });

        });
    </script>
     <script type="text/javascript">
        function DeleteItem() {
            if (confirm("All data associated with this will be deleted. Are you sure you want to delete ...?")) {
                return true;
            }
            return false;
        }
       
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="modal_master_body" runat="server">
     <form id="frmAddDocumentModal" runat="server">
      <div>
                                   <a id="AddDetails" runat="server" href="#" class="showStatusModal"><asp:Button ID="Button2" runat="server" Text="+ Add Details" CssClass="btn btn-primary"></asp:Button></a>
                                </div><br />
         <div class="container-fluid">
            <div class="row">
                <div class="col-lg-12 col-xl-12 form-group">
                <div class="card mb-4">
                    <div class="card-body">
                        <div class="table-responsive">
                            <h5 id="heading" runat="server">List of RA Bill Adjustment</h5>
                                <asp:GridView ID="GrdRABillAdj" runat="server" HeaderStyle-HorizontalAlign="Center" CssClass="table table-bordered" EmptyDataText="No Data" AutoGenerateColumns="False" Width="100%" AlternatingRowStyle-BackColor="lightGray" OnRowDeleting="GrdRABillAdj_RowDeleting">
                                <Columns>
                                      <asp:BoundField DataField="UID" ItemStyle-HorizontalAlign="Center" HeaderText="UID" HeaderStyle-CssClass="Hide" ItemStyle-CssClass="Hide">
                            
                            </asp:BoundField>
                                     <asp:BoundField DataField="ItemDescription" ItemStyle-HorizontalAlign="Center" ItemStyle-Width="150px" HeaderText="Index Description" HtmlEncode="false" HeaderStyle-Width="10%">
                            
                            </asp:BoundField>
                                    <%-- <asp:BoundField DataField="RABillDesc" ItemStyle-HorizontalAlign="Center" HeaderText="RABillDesc" HtmlEncode="False">
                            
                            </asp:BoundField>--%>
                                     <asp:BoundField DataField="SourceIndex" ItemStyle-HorizontalAlign="Right" HeaderText=" Source of Index" HtmlEncode="False" HeaderStyle-Width="30%">
                            
                            </asp:BoundField>
                                     <asp:BoundField DataField="ProposedWeighting"  HeaderText="Proposed Weighting %" ItemStyle-HorizontalAlign="Right" HtmlEncode="False" HeaderStyle-Width="5%">
                            
                            </asp:BoundField>
                                    <asp:BoundField DataField="Coefficient"  HeaderText="Coefficient" ItemStyle-HorizontalAlign="Right" HtmlEncode="False" HeaderStyle-Width="5%">
                            
                            </asp:BoundField>
                                       <asp:BoundField DataField="InitialIndiceValue"  HeaderText="Initial indices" ItemStyle-HorizontalAlign="Right" HtmlEncode="False" HeaderStyle-Width="10%">
                            
                            </asp:BoundField>
                                        <asp:BoundField DataField="LatestIndiceValue"  HeaderText="Latest indices" ItemStyle-HorizontalAlign="Right" HtmlEncode="False" HeaderStyle-Width="10%">
                            
                            </asp:BoundField>
                                        <asp:BoundField DataField="PriceAdjFactor"  HeaderText="Price Adjustment Factor<br/> (F= Coefficeint * Latest indices / Initial indices)" ItemStyle-HorizontalAlign="Right" HtmlEncode="False" HeaderStyle-Width="20%">
                            
                            </asp:BoundField>
                                      <asp:TemplateField HeaderStyle-Width="5%">
                                        <ItemTemplate>
                                            <a id="Edit" href="/_modal_pages//add-rabill-priceadj-details.aspx?UID=<%#Eval("UID")%>&MasterUID=<%#Eval("MasterUID")%>&type=edit" class="showStatusModal"><span title="Edit" class="fas fa-edit"></span></a>
                                             <asp:HiddenField ID="hidrabillDeleteuid" runat="server" value='<%#Eval("UID")%>' />
                                        </ItemTemplate>

                                    </asp:TemplateField>
                                <asp:TemplateField ShowHeader="False" HeaderStyle-Width="5%">
                                                <ItemTemplate>
                                                    <asp:LinkButton ID="lnkdelete" runat="server" CausesValidation="False" CommandName="Delete" Text="Delete" OnClientClick="return DeleteItem();"></asp:LinkButton>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                </Columns>
                                </asp:GridView>
                            <table width="100%" border="1">
                                <tr><td style="width:10%"></td><td style="width:25%;text-align:right"></td><td style="width:8%">Proposed Weighting %</td><td style="width:5%"></td><td style="width:10%"></td><td style="width:10%"></td><td style="width:25%;text-align:right">Price Adjustment Factor</td><td style="width:5%"></td><td style="width:5%"></td></tr>
                                <tr><td style="width:10%">Total</td><td style="width:25%;"></td><td id="tdTotalcf" runat="server" style="width:8%;text-align:right"></td><td style="width:5%"></td><td style="width:10%"></td><td style="width:10%"></td><td id="tdTotalf" runat="server" style="width:25%;text-align:right"></td><td style="width:5%"></td><td style="width:5%"></td></tr>
                            </table>
                            
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </div>
      <%--View document status modal--%>
    <div id="ModAddDocumentStatus" class="modal it-modal fade">
	    <div class="modal-dialog modal-dialog-centered modal-dialog-scrollable modal-lg">
		    <div class="modal-content">
			    <div class="modal-header">
				    <h5 class="modal-title">Add Details</h5>
                    <button aria-label="Close" class="close" data-dismiss="modal" type="button"><span aria-hidden="true">&times;</span></button>
			    </div>
			    <div class="modal-body">
                    <iframe class="border-0 w-100" style="height:600px;width:300px" loading="lazy"></iframe>
			    </div>
              
		    </div>
	    </div>
    </div></form>
</asp:Content>
